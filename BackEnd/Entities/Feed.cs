using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Feed
    {
        [Key]
        [JsonProperty("feedId")]
        public int FeedId { get; set; }

        [JsonProperty("feedUrl")]
        public string FeedUrl { get; set; }

        [JsonProperty("uploaderId")]
        public int UploaderId { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("numberOfRatings")]
        public int NumberOfRatings { get; set; }

        [JsonProperty("averageRatings")]
        public double AverageRatings { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
    }

}
