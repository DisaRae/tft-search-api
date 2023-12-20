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
        public ChampionAbilityRaw Ability { get; set; }

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
        public ChampionStatsRaw Stats { get; set; }

        [JsonProperty("tileIcon")]
        [JsonPropertyName("tileIcon")]
        public string TileIcon { get; set; }

        [JsonProperty("traits")]
        [JsonPropertyName("traits")]
        public List<string> Traits { get; set; }
    }
}
