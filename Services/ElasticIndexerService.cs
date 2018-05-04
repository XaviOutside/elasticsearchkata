using System.Collections.Generic;
using ElasticSearch.Entities;
using ElasticSearch.Repositories;

namespace ElasticSearch.Services
{
    public class ElasticIndexerService : IElasticIndexerService<Gif>
    {
        private IElasticIndexerRepository<Gif> _elasticIndexerRepository;

        public ElasticIndexerService(IElasticIndexerRepository<Gif> elasticIndexerRepository)
        {
            _elasticIndexerRepository = elasticIndexerRepository;
        }

        public bool AddAlias(string indexName, string aliasName)
        {
            return _elasticIndexerRepository.AddAlias(indexName, aliasName);
        }

        public bool BulkIndex(IEnumerable<Gif> entities, string indexName)
        {
            return _elasticIndexerRepository.BulkIndex(entities, indexName);
        }

        public bool CreateIndex(string indexName)
        {
            return _elasticIndexerRepository.CreateIndex(indexName);
        }

        public bool DeleteIndex(string indexName)
        {
            return _elasticIndexerRepository.DeleteIndex(indexName);
        }

        public bool SwapAlias(string newIndexName, string aliasName)
        {
            return _elasticIndexerRepository.SwapAlias(newIndexName, aliasName);
        }
    }
}
