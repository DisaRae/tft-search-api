using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;
using static System.Net.WebRequestMethods;

namespace TFT.Search.Library.Models
{
    public class Champion
    {
        [JsonProperty("ability")]
        public ChampionAbility Ability { get; set; }

        [JsonProperty("cost")]
        public int? Cost { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("squareIcon")]
        public string Icon { get; set; }

        [JsonProperty("stats")]
        public ChampionStats Stats { get; set; }

        [JsonProperty("traits")]
        public List<string> Traits { get; set; }
    }

    public class ChampionAbility: BaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ChampionStats
    {
        [JsonProperty("armor")]
        public double? Armor { get; set; }

        [JsonProperty("attackSpeed")]
        public double? AttackSpeed { get; set; }

        [JsonProperty("critChance")]
        public double? CritChance { get; set; }

        [JsonProperty("critMultiplier")]
        public double? CritMultiplier { get; set; }

        [JsonProperty("damage")]
        public double? Damage { get; set; }

        [JsonProperty("hp")]
        public double? Hp { get; set; }

        [JsonProperty("initialMana")]
        public double? InitialMana { get; set; }

        [JsonProperty("magicResist")]
        public double? MagicResist { get; set; }

        [JsonProperty("mana")]
        public double? Mana { get; set; }

        [JsonProperty("range")]
        public double? Range { get; set; }
    }

}
