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
            TftData = GetJsonFile();
            DataLastRetrieved = DateTime.Now;
        }

        public void CheckDataLastRetrievedAndRefreshIfNecessary()
        {
            if (DataLastRetrieved.AddHours(2) < DateTime.Now)
                TftData = GetJsonFile();
        }

        private RawCdragon LoadJson()
        {
            //return LoadJson<RawCdragon>("C:\\Users\\raeka\\OneDrive\\Desktop\\TftSearch\\TFT.Search\\raw_cdragon_tft.json");
            return GetJsonFile();
        }

        private T LoadJson<T>(string path)
        {
            T item;
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                item = JsonConvert.DeserializeObject<T>(json);
            }
            return item;
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