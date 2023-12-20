using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFT.Search.Library.Models.RawData
{
    public class SetRaw
    {
        [JsonProperty("number")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mutator")]
        public string Mutator { get; set; }

        [JsonProperty("champions")]
        public List<ChampionRaw> Champions { get; set; }

        [JsonProperty("traits")]
        public List<TraitsRaw> Traits { get; set; }
    }
}
