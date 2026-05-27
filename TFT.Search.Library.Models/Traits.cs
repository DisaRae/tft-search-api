using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TFT.Search.Library.Models
{
    public class Traits
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("apiName")]
        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonProperty("desc")]
        [JsonPropertyName("desc")]
        public string Description { get; set; }

        public string UnitScale { get; set; }
    }
}
