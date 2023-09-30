using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TFT.Search.Library.Models
{
    public class ItemRaw
    {
        [JsonProperty("apiName")]
        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

        [JsonProperty("associatedTraits")]
        [JsonPropertyName("associatedTraits")]
        public List<object> AssociatedTraits { get; set; }

        [JsonProperty("composition")]
        [JsonPropertyName("composition")]
        public List<object> Composition { get; set; }

        [JsonProperty("desc")]
        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonProperty("effects")]
        [JsonPropertyName("effects")]
        public List<KeyValuePair<string, string>> Effects { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public object From { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonProperty("incompatibleTraits")]
        [JsonPropertyName("incompatibleTraits")]
        public List<object> IncompatibleTraits { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("unique")]
        [JsonPropertyName("unique")]
        public bool Unique { get; set; }
    }


}
