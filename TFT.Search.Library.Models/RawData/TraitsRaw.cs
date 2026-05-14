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
    public class TraitsRaw : BaseModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

        [JsonPropertyName("effects")]
        public List<Effects> Effects { get; set; }

        public string UnitScale
        {
            get
            {
                string unitScale = string.Empty;
                Effects.Select(x=>x.MinUnits).ToList().ForEach(x=>unitScale += (x.ToString()+"/"));
                return unitScale;
            }
        }
    }

    public class Effects
    {
        //  Shows whether it scales every 1 or 2 champions with the trait
        [JsonProperty("maxUnits")]
        [Newtonsoft.Json.JsonConverter(typeof(NullToZeroConverter))]
        public int MaxUnits { get; set; }

        //  IE. minimum 2 or 3 units
        [JsonProperty("minUnits")]
        [Newtonsoft.Json.JsonConverter(typeof(NullToZeroConverter))]
        public int MinUnits { get; set; }

        //  I think this is a numeric value for bronze, silver, gold, platinum
        [JsonProperty("style")]
        public int Style { get; set; }

        [JsonProperty("variables")]
        public object Variables { get; set; }
    }
}
