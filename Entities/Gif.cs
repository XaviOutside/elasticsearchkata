using System.Collections.Generic;

namespace ElasticSearch.Entities
{
    //[ElasticType(Name = "Mamerto", IdProperty = "IdDocument")]
    public class Gif
    {
        public Gif()
        {
            IdGif = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Description2 = string.Empty;
            Tags = new List<string>();
            URL = string.Empty;
        }
        // elasticsearch will use this as unique id for the documents
        public string IdGif { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string Description2 { get; set; }

        public int Score { get; set; }

        public List<string> Tags { get; set; }

        public string URL { get; set; }
    }
}
