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
        Set GetCurrentSet();
        void RefreshData();
    }
    public class TftService : ITftService
    {
        private readonly ITftRepository _tftRepository;
        private RawCdragon _tftData { get; set; }
        private IDictionary<int, Set> _sets { get; set; }

        public TftService(ITftRepository tftRepository)
        {
            _tftRepository = tftRepository;
            _tftData = LoadRawData();
            _sets = new Dictionary<int, Set>();
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
            //_sets = CleanRawData();
        }

        public int? GetCurrentSetId()
        {
            if (_tftData == null)
                return null;
            var orderedSetData = _tftData.SetData?.OrderByDescending(x => x.Id);
            var currentSet = orderedSetData?.FirstOrDefault();
            return currentSet?.Id;
        }

        public Set GetCurrentSet()
        {
            var currentSetId = GetCurrentSetId();
            var currentSetRaw = _tftData.SetData.FirstOrDefault(x => x.Id == currentSetId);
            var currentSet = CleanRawSet(currentSetRaw);
            return currentSet;
        }

        /// <summary>
        /// Again, not using historic sets at this time.  Will leave in if I ever see the need to expand.
        /// </summary>
        /// <returns>IEnumerable<Set></Set></returns>
        private IEnumerable<Set> CleanRawData()
        {
            List<Set> result = new List<Set>();

            if (_tftData == null && _tftData.SetData != null)
                return null;

            _tftData.SetData.ForEach(rawSet =>
            {
                Set set = null;
                object lockObject = new object();

                set = CleanRawSet(rawSet);
                result.Add(set);
            });
            return result;
        }

        private Set CleanRawSet(SetRaw rawSet)
        {
            Set set = null;
            object lockObject = new object();

            //  If Champions or Traits are null, the whole set might as well be null
            if (rawSet.Champions is null || rawSet.Traits is null)
                return set;

            var deepCopyRawChampions = DeepCopyObjectExtension.DeepCopy<List<ChampionRaw>>(rawSet.Champions);
            var deepCopyRawTraits = DeepCopyObjectExtension.DeepCopy<List<TraitsRaw>>(rawSet.Traits);

            set = RemoveUnneededFields(rawSet);


            // I think I set it up to use the non-dto fields to populate the description before scrubbing them
            Task cleanChampions = Task.Factory.StartNew(() =>
            {
                //  Scrubbing text description
                foreach (var champion in deepCopyRawChampions)
                {
                    if (champion.Ability != null)
                        champion.Ability = ChampionAbilityValueTagPopulationService.FormatDescription(champion.Ability);
                }

                lock(lockObject)
                {
                    rawSet.Champions = deepCopyRawChampions;
                }
            });

            Task cleanItemsAndAugments = Task.Factory.StartNew(() =>
            {
                int setId = 0;
                lock (lockObject)
                {
                    setId = rawSet.Id;
                }
                var itemsAndAugments = GetItemsAndAugments(setId);
                lock (lockObject)
                {
                    set.Items = itemsAndAugments.Item1;
                    set.Augments = itemsAndAugments.Item2;
                }
            });

            Task cleanTraits = Task.Factory.StartNew(() =>
            {

                var traits = new List<Traits>();
                foreach (var trait in deepCopyRawTraits)
                {
                    var cleanedTraits = TraitScrubbingService.CleanTraits(trait);
                    traits.Add(cleanedTraits);
                }

                cleanChampions.ContinueWith(x =>
                {
                    lock (lockObject)
                    {
                        set.Traits = traits;
                    }
                });
            });

            Task.WaitAll(cleanChampions, cleanTraits, cleanItemsAndAugments);
            return set;
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

        /// <summary>
        ///  Remove data irrelevant to our purposes by casting to a more limited data model
        /// </summary>
        /// <param name="startingSet"></param>
        /// <returns></returns>
        private static Set RemoveUnneededFields(SetRaw startingSet)
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
