using System.Text.Json.Serialization;

namespace ViajesAPI.Models
{
    public class BotOption
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }
}
