using API.Entities;
using ElasticSearch.Entities;
using ElasticSearch.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace API.Controllers
{
    public class ElasticController : ApiController
    {
        IElasticSearchService _elasticService;
        IElasticIndexerService<Gif> _elasticIndexer;

        public ElasticController(IElasticSearchService elasticService, IElasticIndexerService<Gif> elasticIndexer)
        {
            _elasticService = elasticService;
            _elasticIndexer = elasticIndexer;
        }

        [HttpPost]
        public bool CreateIndex(CreateIndexProperties indexProperties)
        //public bool CreateIndex()
        {
            return _elasticIndexer.CreateIndex(indexProperties.IndexName);
        }

        [HttpPost]
        [ActionName("addalias")]
        public bool AddAlias(string indexName, string aliasName)
        {
            return _elasticIndexer.AddAlias(indexName, aliasName);
        }

        [HttpPost]
        [ActionName("swapalias")]
        public bool SwapAlias(string aliasName, string indexName)
        {
            return _elasticIndexer.SwapAlias(indexName, aliasName);
        }

        [HttpPost]
        [ActionName("index")]
        public bool IndexDocuments([FromBody]List<Gif> documentToIndex, string indexName)
        {
            return _elasticIndexer.BulkIndex(documentToIndex, indexName);
        }

        [HttpDelete]
        [ActionName("deleteindex")]
        public bool DeleteIndex(string indexName)
        {
            return _elasticIndexer.DeleteIndex(indexName);
        }

        [HttpGet]
        [ActionName("get")]
        public Gif Get(string id, string indexName)
        {
            return _elasticService.Get<Gif>(id, indexName);
        }

        [HttpGet]
        [ActionName("search")]
        public IEnumerable<Gif> Search([FromUri]GifFilter filter)
        {
            //vistazo: scroll
            return _elasticService.Search<Gif>(filter);
        }


        [HttpPost]
        [ActionName("update")]
        public bool Update([FromBody] Gif gifToUpdate, int idDocument, string indexName)
        {
            return _elasticIndexer.Update(gifToUpdate, idDocument, indexName);
        }

        [HttpGet]
        [ActionName("delete")]
        public bool Delete(int idDocument, string indexName)
        {
            return _elasticIndexer.Delete(idDocument, indexName);
        }

    }
}
