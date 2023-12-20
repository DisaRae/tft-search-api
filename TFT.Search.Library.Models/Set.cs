using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public IEnumerable<TraitsRaw> Traits { get; set; }

        [JsonProperty("items")]
        public IEnumerable<Item> Items { get; set; }

        [JsonProperty("augments")]
        public IEnumerable<Augment> Augments { get; set; }
    }
}
