using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Comment
    {
        [Key]
        [JsonProperty("feedId")]
        public int FeedId { get; set; }

        [JsonProperty("commentorUserId")]
        public int CommentorUserId { get; set; }

        [JsonProperty("comment")]
        public string? CommentMessage { get; set; }
    }

}
