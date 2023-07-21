using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using TFT.Search.Library.Models;

namespace TFT.Search.Library.Repositories
{
    public class TftRepository
    {
        public RawCdragon TftData { get; set; }
        public DateTime DataLastRetrieved { get; set; }

        public TftRepository()
        {
            TftData = LoadJson();
            DataLastRetrieved = DateTime.Now;
        }

        public void CheckDataLastRetrievedAndRefreshIfNecessary()
        {
            if (DataLastRetrieved.AddHours(2) < DateTime.Now)
                TftData = LoadJson();
        }

        private RawCdragon LoadJson()
        {
            //return LoadJson<RawCdragon>("C:\\Users\\raeka\\OneDrive\\Desktop\\TftSearch\\TFT.Search\\raw_cdragon_tft.json");
            var result = GetJsonFile();
            if (result != null && result.SetData != null)
                result.SetData.ForEach(x =>
                {
                    x.Champions.ForEach(y =>
                    {
                        if (y.Ability != null)
                            y.Ability.CleanDescription();
                    });
                });
            return result;
        }

        private RawCdragon GetJsonFile()
        {
            var url = "https://raw.communitydragon.org/latest/cdragon/tft/en_us.json";

            var request = url.
                GetJsonAsync<RawCdragon>();

            var result = request.GetAwaiter().GetResult();
            return result;
        }
    }
}