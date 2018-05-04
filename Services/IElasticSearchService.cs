using ElasticSearch.Entities;
using System.Collections.Generic;

namespace ElasticSearch.Services
{
    public interface IElasticSearchService
    {
        IEnumerable<T> Search<T>(GifFilter descriptor) where T : class;

        T Get<T>(string id, string indexName) where T : class;
    }
}
