using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Chat
    {
        [Key]
        [JsonProperty("senderUserId")]
        public int SenderUserId { get; set; }

        [JsonProperty("receiverUserId")]
        public int ReceiverUserId { get; set; }

        [JsonProperty("chat")]
        public string? ChatMessage { get; set; }
    }

}
