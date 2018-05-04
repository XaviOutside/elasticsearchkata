using ElasticSearch.Entities;
using Nest;
using System;
using System.Collections.Generic;

namespace ElasticSearch.Repositories
{
    public class ElasticIndexerRepository : IElasticIndexerRepository<Gif>
    {
        private string _url = "http://localhost:9200/";
        protected ElasticClient _client;

        public ElasticIndexerRepository()
        {
            //alternativa, en la declaración de la clase
            var settings = new ConnectionSettings(new Uri(_url)).InferMappingFor<Gif>(doc => doc.IdProperty(o => o.IdGif));
            _client = new ElasticClient(settings);
            //pa ver un mapping, 
        }

        public bool AddAlias(string indexName, string aliasName)
        {
            //alternativamente podemos mirar si ese index tiene alias
            //NOTA: un alias puede apuntar a N índices y elastic se lo maneja
            var putRequest = new PutAliasRequest(indexName, aliasName);

            var result = _client.PutAlias(putRequest);

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

            return idxRes.IsValid;
        }

        public bool DeleteIndex(string indexName)
        {
            var delRes = _client.DeleteIndex(indexName);

            return delRes.IsValid;
        }

        public bool SwapAlias(string newIndexName, string aliasName)
        {
            var indicesPointingToAlias = _client.GetIndicesPointingToAlias(aliasName);

            foreach (var index in indicesPointingToAlias)
            {
                _client.DeleteAlias(index, aliasName);
            }

            return AddAlias(newIndexName, aliasName);
        }
    }
}
