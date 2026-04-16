using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
