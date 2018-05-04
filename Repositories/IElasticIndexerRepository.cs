using System.Collections.Generic;


namespace ElasticSearch.Repositories
{
    public interface IElasticIndexerRepository<T>
    {
        bool CreateIndex(string indexName);

        bool AddAlias(string indexName, string aliasName);

        bool BulkIndex(IEnumerable<T> entities, string indexName);

        bool SwapAlias(string newIndexName, string aliasName);

        bool DeleteIndex(string indexName);
    }
}
