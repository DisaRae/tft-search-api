﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TFT.Search.Library.Models
{
    public class Set
    {
        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("champions")]
        public List<Champion> Champions { get; set; }

        [JsonPropertyName("traits")]
        public List<Traits> Traits { get; set; }
    }
}
