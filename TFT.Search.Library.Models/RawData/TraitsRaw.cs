using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Models
{
    public class TraitsRaw: RawDataBase
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("effects")]
        public List<object> Effects { get; set; }
    }

    public class Effects
    {

        [JsonProperty("maxUnits")]
        public int MaxUnits { get; set; }

        [JsonProperty("minUnits")]
        public int MinUnits { get; set; }

        [JsonProperty("style")]
        public int Style { get; set; }

        [JsonProperty("variables")]
        public List<VariableRaw> Variables { get; set; }
    }
}
