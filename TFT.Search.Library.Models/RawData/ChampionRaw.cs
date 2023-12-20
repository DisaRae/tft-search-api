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

namespace TFT.Search.Library.Models.RawData
{
    public class ChampionRaw : RawDataBase
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
                var replacedImageType = (_icon ?? string.Empty).Replace(".tex", ".png");
                return _imageBaseUrl + replacedImageType.ToLower() ?? string.Empty;
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
                var replacedImageType = (_squareIcon ?? string.Empty).Replace(".tex", ".png");
                return _imageBaseUrl + replacedImageType.ToLower() ?? string.Empty;
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

    public class AbilityRaw : RawDataBase
    {

        [JsonProperty("variables")]
        [JsonPropertyName("variables")]
        public List<Variable> Variables { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon
        {
            get
            {
                var replacedImageType = (_icon ?? string.Empty).Replace(".dds", ".png");
                return _imageBaseUrl + replacedImageType.ToLower() ?? string.Empty;
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
            //  Don't want the UI to break with any nulls so empty string is fine
            var description = Desc ?? string.Empty;
            if (Variables == null || Variables.Count == 0 || string.IsNullOrWhiteSpace(description))
            {
                Desc = description;
                return;
            }
            var descriptionCleaned = ScrubHtmlTags();

            Regex ItemRegex = new Regex("@[A-Za-z0-9*]*@", RegexOptions.Compiled);
            //if (this.Name == "Voracity")
            //    Debug.WriteLine("catch");

            foreach (Match ItemMatch in ItemRegex.Matches(descriptionCleaned))
            {
                var tagName = ItemMatch.Value.Replace('@', ' ').Trim();
                string[] possibleTagNames = new string[0];
                //  I don't know why they put astrisks in the tags sometimes
                if (tagName.Contains('*'))
                {
                    possibleTagNames = tagName.Split('*');
                    tagName = possibleTagNames[0];
                }

                //  This is not precise, but works 95% of the time for now
                var potentialValues = Variables.AsEnumerable().FirstOrDefault(x => tagName.Contains(x.Name) || x.Name.Contains(tagName));

                //  There is a list of potential stat values that may or may not scale depending on star level
                string matchValue = "";
                if (potentialValues != null)
                {
                    //  If all the variable values are the same, then it's a non-scaling stat
                    if (potentialValues.Value.All(o => o == potentialValues.Value[0]))
                        matchValue = ParseNonScalingValues(possibleTagNames, potentialValues);
                    //  Or it's a scaling stat
                    else
                        matchValue = ParseScalingValues(potentialValues);
                }
                descriptionCleaned = descriptionCleaned.Replace(ItemMatch.Value, matchValue);
            }
            Desc = descriptionCleaned;
        }
        
        private static string ParseNonScalingValues(string[] possibleTagNames, Variable potentialValues)
        {
            string matchValue = "";
            if (possibleTagNames.Length > 1)
            {
                var calculatedValue = potentialValues.Value[0] * Convert.ToInt32(possibleTagNames[1]);
                var roundedValue = Math.Round(calculatedValue ?? 0);
                matchValue = roundedValue.ToString();
            }
            else
            {
                //  Value is a percentage
                if (potentialValues.Value[0] < 1)
                {
                    potentialValues.Value[0] = Math.Round(potentialValues.Value[0] ?? 0, 2) * 100;
                    matchValue = potentialValues.Value[0].ToString() + "%";
                }
                else
                    matchValue = Math.Round(potentialValues.Value[0] ?? 0, 2).ToString();
            }
            return matchValue;
        }

        private static string ParseScalingValues(Variable potentialValues)
        {
            string matchValue = "";
            potentialValues.Value.ForEach(x =>
            {
                if (x != 0)
                {

                    if (x < 1)
                    {
                        x = Math.Round(x ?? 0, 2) * 100;
                        matchValue += x.ToString() + "%/";
                    }
                    else
                        matchValue += Math.Round(x ?? 0, 2).ToString() + "/";
                }
            });
            return matchValue;
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
}
