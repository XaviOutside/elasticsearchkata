using ElasticSearch.Entities;
using ElasticSearch.Repositories;
using Nest;
using System.Collections.Generic;


namespace ElasticSearch.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        IElasticSearchRepository _elasticRepository;

        public ElasticSearchService(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticRepository = elasticSearchRepository;
        }

        IEnumerable<Gif> IElasticSearchService.Search<Gif>(GifFilter descriptor)
        {

            var queryContainerList = new List<QueryContainer>();
            var qF = new QueryContainerDescriptor<Gif>();

            if (!string.IsNullOrWhiteSpace(descriptor.Id))
            {
                queryContainerList.Add(qF.Bool(b => b
                        .Must(q => q
                            .Term(t => t
                                .Field("idGif")
                                .Value(descriptor.Id)))));
            }

            if (descriptor.ScoreMax != 0 && descriptor.ScoreMin != 0)
            {
                queryContainerList.Add(qF.Range(r => r
                        .Field("score")//OJO: case SENSITIVE, si le ponemos Score, no funciona
                        .LessThanOrEquals(descriptor.ScoreMax)
                        .GreaterThanOrEquals(descriptor.ScoreMin)));
            }
            else if (descriptor.ScoreMax != 0)
            {
                queryContainerList.Add(qF.Range(r => r
                       .Field("score")
                       .LessThanOrEquals(descriptor.ScoreMax)));
            }
            else if (descriptor.ScoreMin != 0)
            {
                queryContainerList.Add(qF.Range(r => r
                       .Field("score")
                       .GreaterThanOrEquals(descriptor.ScoreMin)));
            }

            if (!string.IsNullOrEmpty(descriptor.Description))
            {
                queryContainerList.Add(qF.Match(m => m
                    .Field("description")
                    .Query(descriptor.Description)
                    .Fuzziness(Fuzziness.Auto)
                ));
            }

            //if(descriptor.Tags.Any())
            //{

            //}
            var elasticRequest = new SearchDescriptor<Gif>();
            elasticRequest
                .Index(descriptor.IndexName)
                .Query(q => q.Bool(b => b.Must(queryContainerList.ToArray())) &&
                q.Match(m => m
                    .Field("name")
                    .Query(descriptor.Query)
                    .Operator(Operator.And)
                    .Fuzziness(Fuzziness.EditDistance(2))
                    .Strict(false)));


            return _elasticRepository.Search<Gif>(elasticRequest).Documents;                        
        }

        Gif IElasticSearchService.Get<Gif>(string id, string indexName)
        {
            return _elasticRepository.Get<Gif>(id, indexName);
        }
    }
}
