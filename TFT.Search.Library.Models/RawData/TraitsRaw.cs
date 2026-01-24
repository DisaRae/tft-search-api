using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Models
{
    public class TraitsRaw : RawDataBase
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

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

        [JsonPropertyName("effects")]
        public List<Effects> Effects { get; set; }

        public string UnitScale
        {
            get
            {
                string unitScale = string.Empty;
                Effects.Select(x=>x.MinUnits).ToList().ForEach(x=>unitScale += (x.ToString()+"/"));
                return unitScale;
            }
        }

        public Traits CleanTraits()
        {
            var result = new Traits()
            {
                Name = Name,
                ApiName = ApiName,
                Icon = Icon,
            };

            // Set Unit scale
            string unitScale = string.Empty;
            Effects.Select(x => x.MinUnits).ToList().ForEach(x => unitScale += (x.ToString() + "/"));
            if(unitScale.Length > 1)
                unitScale = unitScale[..^1];
            result.UnitScale = unitScale;

            //  Don't want the UI to break with any nulls so empty string is fine
            var description = Desc ?? string.Empty;
            if (Effects == null || Effects.Count == 0 || string.IsNullOrWhiteSpace(description))
            {
                result.Description = description;
                return result;
            }

            ScrubHtmlTags();

            //  We want to flatten all the Variable values into a scaling string
            var scaledValues = new Dictionary<string, string>();
            IEnumerable<string> variableNamesAccrossTraitEffects = new List<string>();

            //  Actually, I think there's a better way to do this...
            foreach (var effect in Effects)
            {
                var stringVariables = effect.Variables.ToString();
                Dictionary<string, dynamic> variables = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(stringVariables);
                if (effect.MinUnits == Effects.FirstOrDefault()?.MinUnits)
                {
                    variableNamesAccrossTraitEffects = variables.Select(x => x.Key);
                    foreach (var name in variableNamesAccrossTraitEffects)
                    {
                        var keyword = String.Empty;
                        if (variables.ContainsKey(name) == false) continue;
                        var numberValue = variables[name];
                        if (numberValue is null || numberValue.ToString() == "null")
                        {
                            scaledValues.Add(name, keyword);
                            continue;
                        }
                        keyword = Math.Round((decimal)numberValue).ToString();
                        var parsedForPercentage = ParseScalingValues(keyword);
                        if(scaledValues.ContainsKey(name) == false)
                            scaledValues.Add(name, parsedForPercentage);
                    }
                }
                else
                {
                    foreach (var name in variableNamesAccrossTraitEffects)
                    {
                        var keyword = String.Empty;
                        if (variables.ContainsKey(name) == false) continue;
                        var numberValue = variables[name];
                        if (numberValue is null || numberValue.ToString() == "null")
                        {
                            scaledValues[name] += keyword;
                            continue;
                        }
                        keyword = Math.Round((decimal)numberValue).ToString();
                        var parsedForPercentage = ParseScalingValues(keyword);
                        scaledValues[name] += ("/" + parsedForPercentage);
                    }
                }
            }

            Regex ItemRegex = new Regex("@[A-Za-z0-9*]*@", RegexOptions.Compiled);
            //if (this.Name == "Voracity")
            //    Debug.WriteLine("catch");

            foreach (Match ItemMatch in ItemRegex.Matches(Desc))
            {
                var tagName = ItemMatch.Value.Replace('@', ' ').Trim();
                //  This is only if it's a percentage, I think
                string[] possibleTagNames = new string[0];
                //  I don't know why they put astrisks in the tags sometimes
                if (tagName.Contains('*'))
                {
                    possibleTagNames = tagName.Split('*');
                    tagName = possibleTagNames[0];
                }

                //  This is not precise, but works 95% of the time for now
                string matchValue = String.Empty;
                if (tagName == "MinUnits")
                    matchValue = result.UnitScale;
                else
                    scaledValues.TryGetValue(tagName, out matchValue);
                Desc = Desc.Replace(ItemMatch.Value, matchValue);
            }
            result.Description = Desc;
            return result;
        }

        private static string ParseScalingValues(string value)
        {
            string matchValue = "";
            decimal number = 0;
            if (Decimal.TryParse(value, out number))
            {
                //  Value is a percentage
                if (number < 1)
                {
                    number = Math.Round(number, 2) * 100;
                    matchValue = number.ToString();
                }
                else
                    matchValue = Math.Round(number, 2).ToString();
            }
            return matchValue;
        }
    }

    public class Effects
    {
        //  Shows whether it scales every 1 or 2 champions with the trait
        [JsonProperty("maxUnits")]
        public int MaxUnits { get; set; }

        //  IE. minimum 2 or 3 units
        [JsonProperty("minUnits")]
        public int MinUnits { get; set; }

        //  I think this is a numeric value for bronze, silver, gold, platinum
        [JsonProperty("style")]
        public int Style { get; set; }

        [JsonProperty("variables")]
        public object Variables { get; set; }
    }
}
