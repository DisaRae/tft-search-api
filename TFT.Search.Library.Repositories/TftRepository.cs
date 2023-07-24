using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TFT.Search.Library.Models;

namespace TFT.Search.Library.Repositories
{
    public class TftRepository
    {
        public RawCdragon TftData { get; set; }
        public Set CurrentSet { get; set; }
        public DateTime? DataLastRetrieved { get; set; }

        public TftRepository()
        {
            CheckDataLastRetrievedAndRefreshIfNecessary();
            DataLastRetrieved = DateTime.Now.AddMinutes(-5);
        }

        public void CheckDataLastRetrievedAndRefreshIfNecessary()
        {
            if ((DataLastRetrieved??DateTime.MinValue).AddHours(2) < DateTime.Now)
            {
                TftData = LoadJson();
                var rawCurrentSet = LoadCurrentSet();
                CurrentSet = CleanRawSet(rawCurrentSet);
            }
        }

        private RawCdragon LoadJson()
        {
            //return LoadJson<RawCdragon>("C:\\Users\\raeka\\OneDrive\\Desktop\\TftSearch\\TFT.Search\\raw_cdragon_tft.json");
            var result = GetJsonFile();
            if (result != null && result.SetData != null)
                foreach (var set in result.SetData)
                {
                    if (set.Champions != null)
                        foreach (var champion in set.Champions)
                        {
                            if (champion.Ability != null)
                                champion.Ability.CleanDescription();
                        }
                }
            return result;
        }

        private SetRaw LoadCurrentSet()
        {
            if (TftData == null)
                return null;
            var orderedSetData = TftData.SetData?.OrderByDescending(x => x.Id);
            return orderedSetData?.FirstOrDefault();
        }

        private RawCdragon GetJsonFile()
        {
            var url = "https://raw.communitydragon.org/latest/cdragon/tft/en_us.json";

            var request = url.
                GetJsonAsync<RawCdragon>();

            var result = request.GetAwaiter().GetResult();
            return result;
        }

        private Set CleanRawSet(SetRaw startingSet)
        {
            if (TftData == null)
                return null;
            string json = JsonConvert.SerializeObject(startingSet);
            var endSet = JsonConvert.DeserializeObject<Set>(json);
            if (endSet != null && endSet?.Champions != null)
                endSet.Champions = endSet.Champions.Where(x => x.Traits != null && x.Traits.Any()).ToList();
            return endSet;
        }
    }
}