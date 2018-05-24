using System.Collections.Generic;

namespace ElasticSearch.Entities
{
    public class GifFilter
    {
        public GifFilter()
        {
            Id = string.Empty;
            Query = string.Empty;
            Tags = new List<string>();
            Page = 1;
            PageSize = 50;
            MinScore = 0;
        }

        public string Id { get; set; }

        public string Query { get; set; }

        public int ScoreMin { get; set; }

        public int ScoreMax { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public List<string> Tags { get; set; }

        public string IndexName { get; set; }

        public string Description { get; set; }

        public string Description2 { get; set; }

        public string Text { get; set; }

        public double MinScore { get; set; }
    }
}
