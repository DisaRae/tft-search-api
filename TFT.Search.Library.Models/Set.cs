using Newtonsoft.Json;
using System.Collections.Generic;

namespace TFT.Search.Library.Models
{
    public class Set
    {
        [JsonProperty("number")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("champions")]
        public IEnumerable<Champion> Champions { get; set; }

        [JsonProperty("traits")]
        public IEnumerable<Traits> Traits { get; set; }

        [JsonProperty("items")]
        public IEnumerable<Item> Items { get; set; }

        [JsonProperty("augments")]
        public IEnumerable<Augment> Augments { get; set; }
    }
}
