using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TFT.Search.Library.Models.RawData
{
    //  Lets revisit this because I would like to keep this scrub method internal, but obviously I haven't built a full description override for Trait
    public class RawDataBase
    {
        internal string _imageBaseUrl = "https://raw.communitydragon.org/14.9/game/";

        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        // "@[A-Za-z]*@"
        //  "<[A-Za-z]*>|<\/[A-Za-z]*>"
        //  "\(%[A-Za-z:]*%\)"gm
        //  <[A-Za-z]*>^(<br>)$|<\/[A-Za-z]*>
        //  https://regex101.com/
        public void ScrubHtmlTags()
        {
            var description = Desc ?? string.Empty;
            if (string.IsNullOrWhiteSpace(description)) return;
            var newLineAdded = Regex.Replace(description, "<\\bbr\\b>", "\r\n");
            var descriptionCleanedofTags = Regex.Replace(newLineAdded, "<[A-Za-z]*>|<\\/[A-Za-z]*>", "");
            var descriptionCleaned = Regex.Replace(descriptionCleanedofTags, "\\(%i:[A-Za-z]*%\\)|%[A-Za-z:]*%", "");
            var spacesConverted = descriptionCleaned.Replace("&nbsp;", " ");
            Desc = spacesConverted;
        }
    }
}
