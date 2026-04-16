using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Repositories
{
    internal static class ChampionAbilityValueTagPopulationService
    {
        public static ChampionAbilityRaw FormatDescription(ChampionAbilityRaw ability)
        {
            //  Don't want the UI to break with any nulls so empty string is fine
            var description = ability.Description ?? string.Empty;
            if (ability.Variables == null || ability.Variables.Count == 0 || string.IsNullOrWhiteSpace(description))
            {
                ability.Description = description;
                return ability;
            }
            ability.Description = HtmlTagScrubbingService.ScrubHtmlTags(ability.Description ?? string.Empty);

            Regex ItemRegex = new Regex("@[A-Za-z0-9*]*@", RegexOptions.Compiled);
            //if (this.Name == "Voracity")
            //    Debug.WriteLine("catch");

            foreach (Match ItemMatch in ItemRegex.Matches(ability.Description))
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
                var potentialValues = ability.Variables.AsEnumerable().FirstOrDefault(x => tagName.Contains(x.Name) || x.Name.Contains(tagName));

                //  There is a list of potential stat values that may or may not scale depending on star level
                string matchValue = "";
                if (potentialValues != null && potentialValues?.Value != null)
                {
                    //  If all the variable values are the same, then it's a non-scaling stat
                    if (potentialValues.Value.All(o => o == potentialValues.Value[0]))
                        matchValue = ParseNonScalingValues(possibleTagNames, potentialValues);
                    //  Or it's a scaling stat
                    else
                        matchValue = ParseScalingValues(potentialValues);
                }
                ability.Description = ability.Description.Replace(ItemMatch.Value, matchValue);
            }
            return ability;
        }

        private static string ParseNonScalingValues(string[] possibleTagNames, VariableRaw potentialValues)
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

        private static string ParseScalingValues(VariableRaw potentialValues)
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
            if (matchValue.Length > 1)
                matchValue = matchValue.Substring(0, matchValue.Length - 1);
            return matchValue;
        }
    }
}
