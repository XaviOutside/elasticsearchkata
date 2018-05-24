using System.Collections.Generic;
using ElasticSearch.Entities;

namespace ElasticSearch.Services
{
    public interface IElasticIndexerService<T>
    {
        bool CreateIndex(string indexName);

        bool AddAlias(string indexName, string aliasName);

        bool BulkIndex(IEnumerable<T> entities, string indexName);
            
        bool SwapAlias(string newIndexName, string aliasName);

        bool DeleteIndex(string indexName);
        
        bool Update(Gif gifToUpdate, int idDocument, string indexName);

        bool Delete(int idDocument, string indexName);
    }
}
