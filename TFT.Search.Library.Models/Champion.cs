using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace TFT.Search.Library.Models
{
    public class Champion
    {
        [JsonProperty("ability")]
        [JsonPropertyName("ability")]
        public Ability Ability { get; set; }

        [JsonProperty("cost")]
        [JsonPropertyName("cost")]
        public int? Cost { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("squareIcon")]
        [JsonPropertyName("squareIcon")]
        public string Icon { get; set; }

        [JsonProperty("stats")]
        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("traits")]
        [JsonPropertyName("traits")]
        public List<string> Traits { get; set; }
    }

    public class Ability: ImageBase
    {
        [JsonProperty("desc")]
        [JsonPropertyName("desc")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Stats
    {
        [JsonProperty("armor")]
        [JsonPropertyName("armor")]
        public double? Armor { get; set; }

        [JsonProperty("attackSpeed")]
        [JsonPropertyName("attackSpeed")]
        public double? AttackSpeed { get; set; }

        [JsonProperty("critChance")]
        [JsonPropertyName("critChance")]
        public double? CritChance { get; set; }

        [JsonProperty("critMultiplier")]
        [JsonPropertyName("critMultiplier")]
        public double? CritMultiplier { get; set; }

        [JsonProperty("damage")]
        [JsonPropertyName("damage")]
        public double? Damage { get; set; }

        [JsonProperty("hp")]
        [JsonPropertyName("hp")]
        public double? Hp { get; set; }

        [JsonProperty("initialMana")]
        [JsonPropertyName("initialMana")]
        public double? InitialMana { get; set; }

        [JsonProperty("magicResist")]
        [JsonPropertyName("magicResist")]
        public double? MagicResist { get; set; }

        [JsonProperty("mana")]
        [JsonPropertyName("mana")]
        public double? Mana { get; set; }

        [JsonProperty("range")]
        [JsonPropertyName("range")]
        public double? Range { get; set; }
    }

}
