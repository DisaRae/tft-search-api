using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Models
{
    public class TraitsRaw : RawDataBase
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("effects")]
        public List<Effects> Effects { get; set; }

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
        public object VariablesRaw { get; set; }

        public Dictionary<string, dynamic> Variables
        {
            get {
                var stringVariables = VariablesRaw.ToString();
                //var removeBrackets = stringVariables.Substring(1, stringVariables.Length - 2);
                Dictionary<string, dynamic> variables = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(stringVariables);
                return variables;
            }
        }
    }
}
