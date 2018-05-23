using ElasticSearch.Entities;
using Extensions.Logger;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ElasticSearch.Repositories
{
    public class ElasticIndexerRepository : IElasticIndexerRepository<Gif>
    {
        private string _url = string.Empty;
        protected ElasticClient _client;

        public ElasticIndexerRepository()
        {
            _url = ConfigurationManager.AppSettings["URL_ELASTIC"] != null ? ConfigurationManager.AppSettings["URL_ELASTIC"] : string.Empty;

            //alternativa, en la declaración de la clase
            var settings = new ConnectionSettings(new Uri(_url)).InferMappingFor<Gif>(doc => doc.IdProperty(o => o.IdGif));
            settings.EnableDebugMode();
            _client = new ElasticClient(settings);
            //pa ver un mapping, 
        }

        public bool AddAlias(string indexName, string aliasName)
        {
            //alternativamente podemos mirar si ese index tiene alias
            //NOTA: un alias puede apuntar a N índices y elastic se lo maneja
            var putRequest = new PutAliasRequest(indexName, aliasName);

            var result = _client.PutAlias(putRequest);

            string.Format("ElasticIndexerRepository --> AddAlias( indexName : {0}, aliasName : {1} )", indexName, aliasName).ToLog();
            Encoding.UTF8.GetString(result.ApiCall.RequestBodyInBytes).ToLog(false);

            return result.IsValid;
        }

        public bool BulkIndex(IEnumerable<Gif> entities, string indexName)
        {
            var bulk = new BulkDescriptor();

            //alternativa: foreach, si queremos hacer algo diferente por ejemplo routing
            //foreach (var doc in entities)
            //{
            //    bulk.Index<Gif>(b => b
            //        .Document(doc)
            //        .Index(indexName)
            //        .Routing(DateTime.Now.ToString("yyyyMMdd")));
            //}

            bulk.IndexMany(entities, (f, b) => f.Index(indexName));

            var bulkRes = _client.Bulk(bulk);
            //examinar bulkRes, hay cosillas interesantes como el número de docs bulkeados etc

            string.Format("ElasticIndexerRepository --> BulkIndex( entities : IEnumerable<Gif>, indexName : {0} )", indexName).ToLog();
            Encoding.UTF8.GetString(bulkRes.ApiCall.RequestBodyInBytes).ToLog(false);

            return bulkRes.IsValid;
        }

        public bool CreateIndex(string indexName)
        {
            indexName = indexName.ToLower();

            //var idxRes = _client.CreateIndex(indexName,
            //    c => c
            //        .Settings(s => s
            //            .NumberOfShards(3)
            //            .NumberOfReplicas(2)
            //        ).Mappings(m => m
            //            .Map("Gif", o => o
            //                .Properties(p => p
            //                    .Text(f => f.Name("Name"))
            //                    .Number(s => s.Type(NumberType.Integer).Name("Score"))
            //                )
            //            .AutoMap()
            //            )
            //        )
            //    );

            var idxRes = _client.CreateIndex(indexName,
                c => c
                    .Settings(s => s
                        .NumberOfShards(3)
                        .NumberOfReplicas(2)
                    )
                );

            string.Format("ElasticIndexerRepository --> CreateIndex( indexName : {0} )", indexName).ToLog();
            Encoding.UTF8.GetString(idxRes.ApiCall.RequestBodyInBytes).ToLog(false);

            return idxRes.IsValid;
        }

        public bool DeleteIndex(string indexName)
        {
            var delRes = _client.DeleteIndex(indexName);

            string.Format("ElasticIndexerRepository --> DeleteIndex( indexName : {0} )", indexName).ToLog();
            Encoding.UTF8.GetString(delRes.ApiCall.RequestBodyInBytes).ToLog(false);

            return delRes.IsValid;
        }

        public bool SwapAlias(string newIndexName, string aliasName)
        {
            var indicesPointingToAlias = _client.GetIndicesPointingToAlias(aliasName);

            string.Format("ElasticIndexerRepository --> SwapAlias( newIndexName : {0}, aliasName {1} ) ", newIndexName, aliasName).ToLog();

            foreach (var index in indicesPointingToAlias)
            {
                var delAliasRes = _client.DeleteAlias(index, aliasName);
                Encoding.UTF8.GetString(delAliasRes.ApiCall.RequestBodyInBytes).ToLog(false);
            }

            return AddAlias(newIndexName, aliasName);
        }
    }
}
