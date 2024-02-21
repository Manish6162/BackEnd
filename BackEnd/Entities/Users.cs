
using Newtonsoft.Json;


namespace BackEnd.Entities
{

    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "currentTime")]
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "birthday")]
        public DateTime Birthday { get; set; }

        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }
    }

}

