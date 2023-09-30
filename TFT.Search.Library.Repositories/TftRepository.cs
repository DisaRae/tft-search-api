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
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Augment> Augments { get; set; }
        public int CurrentSetNumber { get; set; }
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
                var rawItems = LoadItemsAndAugments();
                Items = rawItems.Item1;
                Augments = rawItems.Item2;
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
            var currentSet = orderedSetData?.FirstOrDefault();
            CurrentSetNumber = currentSet.Id;
            return currentSet;
        }

        private (IEnumerable<Item>, IEnumerable<Augment>) LoadItemsAndAugments()
        {
            if (TftData == null)
                return (null, null);
            var orderedSetData = TftData.Items;
            var itemList = new List<Item>();
            var augmentList = new List<Augment>();
            foreach (var item in orderedSetData)
            {
                if (item.ApiName.Contains($"TFT{CurrentSetNumber}"))
                {
                    if (item.ApiName.ToLower().Contains("augment"))
                    {
                        string json = JsonConvert.SerializeObject(item);
                        var endAugment = JsonConvert.DeserializeObject<Augment>(json);
                        augmentList.Add(endAugment);
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(item);
                        var endItem = JsonConvert.DeserializeObject<Item>(json);
                        itemList.Add(endItem);
                    }
                }
            }
            return (itemList, augmentList);
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