using Newtonsoft.Json;
using System.Collections.Generic;

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
