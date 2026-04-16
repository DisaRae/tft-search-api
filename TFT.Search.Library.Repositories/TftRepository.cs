using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TFT.Search.Library.Models;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Repositories
{
    public interface ITftRepository
    {
        RawCdragon GetJsonFile();
    }

    /// <summary>
    /// This is the class that pulls the raw JSON from Community Dragon
    /// </summary>
    public class TftRepository: ITftRepository
    {
        public RawCdragon GetJsonFile()
        {
            var url = "https://raw.communitydragon.org/latest/cdragon/tft/en_us.json";

            var request = url.GetJsonAsync();
            var result = request.GetAwaiter().GetResult();
            //  Returns dynamic and we want a string
            var stringResult = JsonConvert.SerializeObject(result);
            //  Map to data objects
            var json = JsonConvert.DeserializeObject<RawCdragon>(stringResult);
            return json;
        }
    }
}