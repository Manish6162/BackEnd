using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class User
    {
        [Key]
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        //[JsonProperty("used")]
        //public bool Used { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("profilePicUrl")]
        public string ProfilePicUrl { get; set; }
    }


}
