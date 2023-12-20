using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Models
{
    public class Traits: RawDataBase
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
        public int MaxUnits { get; set; }
        public int MinUnits { get; set; }
        public int Style { get; set; }
        public List<Variable> Variables { get; set; }
    }
}
