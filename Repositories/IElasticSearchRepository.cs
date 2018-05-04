using Nest;

namespace ElasticSearch.Repositories
{
    public interface IElasticSearchRepository
    {
        ISearchResponse<T> Search<T>(SearchDescriptor<T> descriptor) where T : class;

        T Get<T>(string id, string indexName) where T : class;
    }
}
