using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TFT.Search.Library.Models.RawData
{
    //  Lets revisit this because I would like to keep this scrub method internal, but obviously I haven't built a full description override for Trait
    public class BaseModel
    {
        internal string _imageBaseUrl = "https://raw.communitydragon.org/latest/game/";

        [JsonProperty("desc")]
        [JsonPropertyName("desc")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon
        {
            get
            {
                var replacedImageType = (_icon ?? string.Empty).Replace(".dds", ".png").Replace(".tex", ".png");
                if (replacedImageType.StartsWith("http://") || replacedImageType.StartsWith("https://"))
                    return replacedImageType;
                return _imageBaseUrl + replacedImageType.ToLower() ?? string.Empty;
            }
            set
            {
                _icon = value;
            }
        }
        private string _icon;
    }
}
