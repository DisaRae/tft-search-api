using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TFT.Search.Library.Models.RawData
{
    public class VariableRaw
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public List<double?> Value { get; set; }
    }
}
