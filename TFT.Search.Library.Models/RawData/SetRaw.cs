using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TFT.Search.Library.Models.RawData
{
    public class SetRaw
    {
        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mutator")]
        public string Mutator { get; set; }

        [JsonProperty("champions")]
        public List<ChampionRaw> Champions { get; set; }

        [JsonProperty("traits")]
        public List<TraitsRaw> Traits { get; set; }
    }
}
