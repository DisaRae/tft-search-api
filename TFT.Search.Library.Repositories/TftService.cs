using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;
using TFT.Search.Library.Models;

namespace TFT.Search.Library.Repositories
{
    public interface ITftService
    {
        int? GetCurrentSetId();
        IEnumerable<Set> GetSets();
        Set GetSet(int setId); 
        void RefreshData();
    }
    public class TftService: ITftService
    {
        private readonly ITftRepository _tftRepository;
        private RawCdragon _tftData { get; set; }
        private IEnumerable<Set> _sets { get; set; }

        public TftService(ITftRepository tftRepository)
        {
            _tftRepository = tftRepository;
            _tftData = LoadRawData();
            _sets = CleanRawData(); 
        }

        private RawCdragon LoadRawData()
        {
            //return LoadJson<RawCdragon>("C:\\Users\\raeka\\OneDrive\\Desktop\\TftSearch\\TFT.Search\\raw_cdragon_tft.json");
            var result = _tftRepository.GetJsonFile();
            return result;
        }

        public void RefreshData()
        {
            _tftData = LoadRawData();
            _sets = CleanRawData();
        }

        public int? GetCurrentSetId()
        {
            if (_tftData == null)
                return null;
            var orderedSetData = _tftData.SetData?.OrderByDescending(x => x.Id);
            var currentSet = orderedSetData?.FirstOrDefault();
            return currentSet?.Id;
        }

        public IEnumerable<Set> GetSets()
        {
            return _sets;
        }

        public Set GetSet(int setId)
        {
            return _sets.FirstOrDefault(x => x.Id == setId);
        }

        private IEnumerable<Set> CleanRawData()
        {
            List<Set> result = new List<Set>();

            if (_tftData == null && _tftData.SetData != null)
                return null;

            _tftData.SetData.ForEach(rawSet =>
            {
                Set set = null;
                object lockObject = new object();

                Task cleanChampions = Task.Factory.StartNew(() =>
                {
                    //  Scrubbing text description
                    if (rawSet.Champions != null)
                        foreach (var champion in rawSet.Champions)
                        {
                            if (champion.Ability != null)
                                champion.Ability.FormatDescription();
                        }

                });

                cleanChampions.ContinueWith(x =>
                {
                    set = CleanRawSet(rawSet);
                    var itemsAndAugments = GetItemsAndAugments(rawSet.Id);
                    set.Items = itemsAndAugments.Item1;
                    set.Augments = itemsAndAugments.Item2;
                });

                cleanChampions.ContinueWith(x =>
                {
                    var traits = new List<Traits>();
                    if (rawSet.Traits != null)
                        foreach (var trait in rawSet.Traits)
                        {
                            var cleanedTraits = trait.CleanTraits();
                            traits.Add(cleanedTraits);
                        }
                    set.Traits = traits;
                    result.Add(set);
                });

                cleanChampions.Wait();
            });
            return result;
        }

        private (IEnumerable<Item>, IEnumerable<Augment>) GetItemsAndAugments(int setId)
        {
            if (_tftData == null)
                return (null, null);
            var orderedSetData = _tftData.Items;
            var itemList = new List<Item>();
            var augmentList = new List<Augment>();
            foreach (var item in orderedSetData)
            {
                if (item.ApiName.Contains($"TFT{setId}"))
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

        //  Remove irrelevant data
        private Set CleanRawSet(SetRaw startingSet)
        {
            if (startingSet == null)
                return null;
            string json = JsonConvert.SerializeObject(startingSet);
            var endSet = JsonConvert.DeserializeObject<Set>(json);
            if (endSet != null && endSet?.Champions != null)
                endSet.Champions = endSet.Champions.Where(x => x.Traits != null && x.Traits.Any()).ToList();
            return endSet;
        }
    }
}
