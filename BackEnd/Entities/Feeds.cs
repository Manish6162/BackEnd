
namespace BackEnd.Entities

{
    using Newtonsoft.Json;

    public class Feeds
    {
        [JsonProperty(PropertyName = "id")]
        public string FeedId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "currentTime")]
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "uploaderUserId")]
        public string UploaderUserId { get; set; }

        [JsonProperty(PropertyName = "uploaderUserName")]
        public string UploaderUserName{ get; set; }

        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

}

