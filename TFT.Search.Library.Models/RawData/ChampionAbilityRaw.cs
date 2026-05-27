using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TFT.Search.Library.Models.RawData
{
    public class ChampionAbilityRaw : BaseModel
    {
        [JsonProperty("variables")]
        [JsonPropertyName("variables")]
        public List<VariableRaw> Variables { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
