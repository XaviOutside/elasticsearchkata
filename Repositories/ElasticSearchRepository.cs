using Nest;
using System;
using System.Linq;

namespace ElasticSearch.Repositories
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        private string _url = "http://localhost:9200/";
        protected ElasticClient _client;

        public ElasticSearchRepository()
        {
            //alternativa, en la declaración de la clase
            var settings = new ConnectionSettings(new Uri(_url));
            _client = new ElasticClient(settings);
            //pa ver un mapping, 
        }

        public T Get<T>(string id, string indexName) where T : class
        {
            var resultGet = _client.Search<T>(d => d.Index(indexName).Query(q => q.Ids(s => s.Values(id))));

            return resultGet.Documents.FirstOrDefault();
        }

        public ISearchResponse<T> Search<T>(SearchDescriptor<T> descriptor) where T : class
        {
            var resultSearch = _client.Search<T>(d => descriptor);

            return resultSearch;
        }
    }
}
