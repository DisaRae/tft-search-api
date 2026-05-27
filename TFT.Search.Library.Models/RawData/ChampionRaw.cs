using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TFT.Search.Library.Models.RawData
{
    public class ChampionRaw : BaseModel
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

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonProperty("squareIcon")]
        [JsonPropertyName("squareIcon")]
        public string SquareIcon
        {
            get
            {
                var replacedImageType = (_squareIcon ?? string.Empty).Replace(".tex", ".png");
                if (replacedImageType.StartsWith("http://") || replacedImageType.StartsWith("https://"))
                    return replacedImageType;
                return _imageBaseUrl + replacedImageType.ToLower() ?? string.Empty;
            }
            set
            {
                _squareIcon = value;
            }
        }
        private string _squareIcon;

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
