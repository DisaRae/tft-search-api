using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TFT.Search.Library.Models;

namespace TFT.Search.Library.Repositories
{
    internal static class TraitScrubbingService
    {
        private static readonly Regex TagPattern = new Regex(@"@[A-Za-z0-9*._:]*@", RegexOptions.Compiled);

        public static Traits CleanTraits(TraitsRaw trait)
        {
            var result = new Traits()
            {
                Name = trait.Name,
                ApiName = trait.ApiName,
                Icon = trait.Icon,
            };

            // Set Unit scale
            string unitScale = string.Empty;
            trait.Effects.Select(x => x.MinUnits).ToList().ForEach(x => unitScale += (x.ToString() + "/"));
            if (unitScale.Length > 1)
                unitScale = unitScale[..^1];
            result.UnitScale = unitScale;

            //  Don't want the UI to break with any nulls so empty string is fine
            var description = trait.Description ?? string.Empty;
            if (trait.Effects == null || trait.Effects.Count == 0 || string.IsNullOrWhiteSpace(description))
            {
                result.Description = description;
                return result;
            }

            var workingDescription = HtmlTagScrubbingService.ScrubHtmlTags(description);

            //  We want to flatten all the Variable values into a scaling string
            var scaledValues = new Dictionary<string, string>();
            IEnumerable<string> variableNamesAccrossTraitEffects = new List<string>();

            //  Actually, I think there's a better way to do this...
            foreach (var effect in trait.Effects)
            {
                var stringVariables = effect.Variables.ToString();
                Dictionary<string, dynamic> variables = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(stringVariables);
                if (effect.MinUnits == trait.Effects.FirstOrDefault()?.MinUnits)
                {
                    variableNamesAccrossTraitEffects = variables.Select(x => x.Key);
                    foreach (var name in variableNamesAccrossTraitEffects)
                    {
                        var keyword = string.Empty;
                        if (variables.ContainsKey(name) == false) continue;
                        var numberValue = variables[name];
                        //  null check must come before GetType() — calling GetType() on a null dynamic throws RuntimeBinderException
                        if (numberValue is null || numberValue.ToString() == "null")
                        {
                            scaledValues.Add(name, keyword);
                            continue;
                        }
                        Type type = numberValue.GetType();
                        if (type.IsArray || type.Name == "JArray")
                        {
                            scaledValues.Add(name, keyword);
                            continue;
                        }
                        var canParse = Decimal.TryParse(numberValue.ToString(), out decimal decimalNumberValue);
                        keyword = canParse ? Math.Round(decimalNumberValue).ToString() : string.Empty;
                        var parsedForPercentage = ParseScalingValues(keyword);
                        if (scaledValues.ContainsKey(name) == false)
                            scaledValues.Add(name, parsedForPercentage);
                    }
                }
                else
                {
                    foreach (var name in variableNamesAccrossTraitEffects)
                    {
                        var keyword = string.Empty;
                        if (variables.ContainsKey(name) == false) continue;
                        var numberValue = variables[name];
                        //  null check must come before GetType() — calling GetType() on a null dynamic throws RuntimeBinderException
                        if (numberValue is null || numberValue.ToString() == "null")
                        {
                            scaledValues[name] += keyword;
                            continue;
                        }
                        Type type = numberValue.GetType();
                        if (type.IsArray || type.Name == "JArray")
                        {
                            scaledValues[name] += keyword;
                            continue;
                        }
                        var canParse = Decimal.TryParse(numberValue.ToString(), out decimal decimalNumberValue);
                        keyword = canParse ? Math.Round(decimalNumberValue).ToString() : string.Empty;
                        var parsedForPercentage = ParseScalingValues(keyword);
                        scaledValues[name] += ("/" + parsedForPercentage);
                    }
                    /*
                     * foreach (var name in variableNamesAccrossTraitEffects)
{
    // 1. Use TryGetValue to avoid looking up the key twice
    if (!variables.TryGetValue(name, out var numberValue) || numberValue == null)
        continue;

    // 2. Cache type or use pattern matching to avoid multiple GetType/Name calls
    if (numberValue is Array || numberValue is IEnumerable<object> || numberValue.ToString() == "null")
    {
        // scaledValues[name] += string.Empty; // This does nothing, so we can just skip it
        continue;
    }

    // 3. Direct parsing and string interpolation for efficiency
    if (Decimal.TryParse(numberValue.ToString(), out decimal decimalNumberValue))
    {
        var rounded = Math.Round(decimalNumberValue).ToString();
        var parsedForPercentage = ParseScalingValues(rounded);

        // 4. Update the scaled value efficiently
        scaledValues[name] += $"/{parsedForPercentage}";
    }
}
                    */
                }
            }

            //if (this.Name == "Voracity")
            //    Debug.WriteLine("catch");

            foreach (Match match in TagPattern.Matches(workingDescription))
            {
                var tagName = match.Value.Replace('@', ' ').Trim();
                //  This is only if it's a percentage, I think
                var possibleTagNames = Array.Empty<string>();
                //  I don't know why they put astrisks in the tags sometimes
                if (tagName.Contains('*'))
                {
                    possibleTagNames = tagName.Split('*');
                    tagName = possibleTagNames[0];
                }

                //  This is not precise, but works 95% of the time for now
                string matchValue = string.Empty;
                if (tagName == "MinUnits")
                    matchValue = result.UnitScale;
                else
                    scaledValues.TryGetValue(tagName, out matchValue);
                workingDescription = workingDescription.Replace(match.Value, matchValue);
            }
            result.Description = workingDescription;
            return result;
        }

        private static string ParseScalingValues(string value)
        {
            string matchValue = string.Empty;
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
}
