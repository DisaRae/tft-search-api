﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Models
{
    public class Traits
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        public string UnitScale { get; set; }
    }
}
