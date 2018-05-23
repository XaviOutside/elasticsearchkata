using Extensions.Logger;
using Nest;
using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ElasticSearch.Repositories
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        private string _url = string.Empty;
        protected ElasticClient _client;

        public ElasticSearchRepository()
        {
            _url = ConfigurationManager.AppSettings["URL_ELASTIC"] != null ? ConfigurationManager.AppSettings["URL_ELASTIC"] : string.Empty;

            //alternativa, en la declaración de la clase
            var settings = new ConnectionSettings(new Uri(_url));
            settings.EnableDebugMode();

            _client = new ElasticClient(settings);
            //pa ver un mapping, 
        }

        public T Get<T>(string id, string indexName) where T : class
        {
            var resultGet = _client.Search<T>(d => d.Index(indexName).Query(q => q.Ids(s => s.Values(id))));

            string.Format("ElasticSearchRepository --> Get( id : {0}, indexName : {1})", id, indexName).ToLog();
            Encoding.UTF8.GetString(resultGet.ApiCall.RequestBodyInBytes).ToLog(false);

            return resultGet.Documents.FirstOrDefault();
        }

        public ISearchResponse<T> Search<T>(SearchDescriptor<T> descriptor) where T : class
        {
            var resultSearch = _client.Search<T>(d => descriptor);

            "ElasticSearchService --> Search( descriptor : SearchDescriptor<T> )".ToLog();
            Encoding.UTF8.GetString(resultSearch.ApiCall.RequestBodyInBytes).ToLog(false);

            return resultSearch;
        }
    }
}
