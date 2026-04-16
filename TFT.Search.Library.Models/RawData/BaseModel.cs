using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
