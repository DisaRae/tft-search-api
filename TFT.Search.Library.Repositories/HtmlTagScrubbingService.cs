using System.Text.RegularExpressions;

namespace TFT.Search.Library.Repositories
{
    /// <summary>
    /// Who likes Regex?
    /// </summary>
    internal static class HtmlTagScrubbingService
    {
        // "@[A-Za-z]*@"
        //  "<[A-Za-z]*>|<\/[A-Za-z]*>"
        //  "\(%[A-Za-z:]*%\)"gm
        //  <[A-Za-z]*>^(<br>)$|<\/[A-Za-z]*>
        //  https://regex101.com/
        public static string ScrubHtmlTags(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) return string.Empty;
            var newLineAdded = Regex.Replace(description, "<\\bbr\\b>", "\r\n");
            var descriptionCleanedofTags = Regex.Replace(newLineAdded, "<[A-Za-z]*>|<\\/[A-Za-z]*>", "");
            var descriptionCleaned = Regex.Replace(descriptionCleanedofTags, "\\(%i:[A-Za-z]*%\\)|%[A-Za-z:]*%", "");
            return descriptionCleaned.Replace("&nbsp;", " ");
        }
    }
}
