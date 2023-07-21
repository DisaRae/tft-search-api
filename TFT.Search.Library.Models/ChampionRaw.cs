using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
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

        [JsonProperty("variables")]
        [JsonPropertyName("variables")]
        public List<Variable> Variables { get; set; }

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

        public void CleanDescription()
        {
            var description = Desc ?? String.Empty;
            if (Variables == null || Variables.Count == 0 || String.IsNullOrWhiteSpace(description))
            {
                Desc = description;
                return;
            }
            // "@[A-Za-z]*@"
            //  "<[A-Za-z]*>|<\/[A-Za-z]*>"
            //  "\(%[A-Za-z:]*%\)"gm
            //  <[A-Za-z]*>^(<br>)$|<\/[A-Za-z]*>
            //  https://regex101.com/
            var newLineAdded = Regex.Replace(description, "<\\bbr\\b>", "\r\n");
            var descriptionCleanedofTags = Regex.Replace(newLineAdded, "<[A-Za-z]*>|<\\/[A-Za-z]*>", "");
            var descriptionCleaned = Regex.Replace(descriptionCleanedofTags, "\\(%i:[A-Za-z]*%\\)|%[A-Za-z:]*%", "");

            Regex ItemRegex = new Regex("@[A-Za-z0-9*]*@", RegexOptions.Compiled);
            if (this.Name == "Voracity")
                Debug.WriteLine("catch");

            foreach (Match ItemMatch in ItemRegex.Matches(descriptionCleaned))
            {
                var tagName = ItemMatch.Value.Replace('@', ' ').Trim();
                string[] possibleTagNames = new string[0];
                if(tagName.Contains('*'))
                {
                    possibleTagNames = tagName.Split('*');
                    tagName = possibleTagNames[0];
                }
                var potentialValues = Variables.AsEnumerable().FirstOrDefault(x => tagName.Contains(x.Name)||x.Name.Contains(tagName));
                string matchValue = "";
                if (potentialValues != null)
                {
                    if (potentialValues.Value.Any(o => o != potentialValues.Value[0]))
                    {
                        potentialValues.Value.ForEach(x =>
                        {
                            if (x != 0)
                                matchValue += x.ToString() + "/";
                        });
                    }
                    else
                    {
                        if (possibleTagNames.Length > 1)
                        {
                            var calculatedValue = potentialValues.Value[0] * Convert.ToInt32(possibleTagNames[1]);
                            var roundedValue = Math.Round(calculatedValue??0);
                            matchValue = roundedValue.ToString();
                        }
                        else
                            matchValue = potentialValues.Value[0].ToString();
                    }
                }
                descriptionCleaned = descriptionCleaned.Replace(ItemMatch.Value, matchValue);
            }
            Desc = descriptionCleaned;
        }
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
