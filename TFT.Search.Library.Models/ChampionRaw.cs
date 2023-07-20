﻿using Newtonsoft.Json;
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
    public class ChampionRaw: Champion
    {
        [JsonProperty("ability")]
        [JsonPropertyName("ability")]
        public AbilityRaw Ability { get; set; }

        [JsonProperty("apiName")]
        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

        [JsonProperty("characterName")]
        [JsonPropertyName("characterName")]
        public string CharacterName { get; set; }

        [JsonProperty("cost")]
        [JsonPropertyName("cost")]
        public int? Cost { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon
        { 
            get 
            { 
                return _imageBaseUrl + (_icon??String.Empty).ToLower() ?? String.Empty; 
            } 
            set 
            { 
                _icon = value;
            } 
        }
        private string _icon;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("squareIcon")]
        [JsonPropertyName("squareIcon")]
        public string SquareIcon
        {
            get 
            { 
                return _imageBaseUrl + (_squareIcon??String.Empty).ToLower() ?? String.Empty; 
            }
            set
            {
                _squareIcon = value;
            }
        }
        public string _squareIcon;

        [JsonProperty("stats")]
        [JsonPropertyName("stats")]
        public StatsRaw Stats { get; set; }

        [JsonProperty("tileIcon")]
        [JsonPropertyName("tileIcon")]
        public string TileIcon { get; set; }

        [JsonProperty("traits")]
        [JsonPropertyName("traits")]
        public List<string> Traits { get; set; }
    }

    public class AbilityRaw: ImageBase
    {
        [JsonProperty("desc")]
        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon
        {
            get
            {
                return _imageBaseUrl + (_icon ?? String.Empty).ToLower() ?? String.Empty;
            }
            set
            {
                _icon = value;
            }
        }
        private string _icon;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("variables")]
        [JsonPropertyName("variables")]
        public List<Variable> Variables { get; set; }
    }

    public class StatsRaw
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

    public class Variable
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public List<double?> Value { get; set; }
    }

}