using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;
using TFT.Search.Library.Models;

namespace TFT.Search.Library.Repositories
{
    public interface ITftService
    {
        int? GetCurrentSetId();
        Set GetCurrentSet();
        Task RefreshDataAsync();
    }

    public class TftService : ITftService
    {
        private readonly ITftRepository _tftRepository;
        private RawCdragon _tftData { get; set; }

        public TftService(ITftRepository tftRepository)
        {
            _tftRepository = tftRepository ?? throw new ArgumentNullException(nameof(tftRepository));
        }

        private async Task<RawCdragon> LoadRawDataAsync()
        {
            return await _tftRepository.GetJsonFileAsync();
        }

        public async Task RefreshDataAsync()
        {
            _tftData = await LoadRawDataAsync();
        }

        public int? GetCurrentSetId()
        {
            var setData = _tftData?.SetData;
            if (setData == null || !setData.Any())
                return null;
            return setData.OrderByDescending(x => x.Id).First().Id;
        }

        public Set GetCurrentSet()
        {
            var currentSetId = GetCurrentSetId();
            if (currentSetId == null)
                return null;

            var currentSetRaw = _tftData?.SetData?.FirstOrDefault(x => x.Id == currentSetId.Value);
            if (currentSetRaw == null)
                return null;

            return CleanRawSet(currentSetRaw);
        }

        /// <summary>
        /// Not using historic sets at this time.
        /// </summary>
        private IEnumerable<Set> CleanRawData()
        {
            if (_tftData?.SetData == null)
                return Enumerable.Empty<Set>();

            var results = new List<Set>();
            foreach (var rawSet in _tftData.SetData)
            {
                var cleaned = CleanRawSet(rawSet);
                if (cleaned != null)
                    results.Add(cleaned);
            }
            return results;
        }

        private Set CleanRawSet(SetRaw rawSet)
        {
            if (rawSet == null || rawSet.Champions == null || rawSet.Traits == null)
                return null;

            // Prepare containers for cleaned data
            List<Champion> finalChampions = null;
            List<Traits> finalTraits = null;
            IEnumerable<Item> items = null;
            IEnumerable<Augment> augments = null;

            // Clean champions: single JSON round trip maps ChampionRaw → Champion, then apply
            // formatted descriptions using the original raw data (which still has Variables).
            // FormatDescription is non-mutating so no deep copy of rawSet is needed.
            var sourceChampions = rawSet.Champions ?? new List<ChampionRaw>();
            var championTask = Task.Run(() =>
            {
                var serialized = JsonConvert.SerializeObject(sourceChampions);
                var mapped = JsonConvert.DeserializeObject<List<Champion>>(serialized) ?? new List<Champion>();
                for (int i = 0; i < mapped.Count && i < sourceChampions.Count; i++)
                {
                    if (mapped[i].Ability != null && sourceChampions[i].Ability != null)
                        mapped[i].Ability.Description = ChampionAbilityValueTagPopulationService.FormatDescription(sourceChampions[i].Ability);
                }
                finalChampions = mapped;
            });

            // Clean traits — CleanTraits is non-mutating so no deep copy of rawSet is needed.
            var traitsTask = Task.Run(() =>
            {
                var list = new List<Traits>();
                foreach (var tr in rawSet.Traits ?? new List<TraitsRaw>())
                {
                    var cleaned = TraitScrubbingService.CleanTraits(tr);
                    if (cleaned != null)
                        list.Add(cleaned);
                }
                finalTraits = list;
            });

            // Items and augments
            var itemsTask = Task.Run(() =>
            {
                var result = GetItemsAndAugments(rawSet.Id);
                items = result.Item1 ?? Enumerable.Empty<Item>();
                augments = result.Item2 ?? Enumerable.Empty<Augment>();
            });

            Task.WaitAll(championTask, traitsTask, itemsTask);

            // Build final Set from cleaned pieces. Start from RemoveUnneededFields to strip irrelevant data,
            // then overwrite the fields with the cleaned collections.
            var endSet = RemoveUnneededFields(rawSet) ?? new Set { Id = rawSet.Id };

            endSet.Champions = (finalChampions ?? new List<Champion>())
                                .Where(x => x.Traits != null && x.Traits.Any())
                                .ToList();

            endSet.Traits = finalTraits ?? new List<Traits>();
            endSet.Items = items?.ToList() ?? new List<Item>();
            endSet.Augments = augments?.ToList() ?? new List<Augment>();

            return endSet;
        }

        private (IEnumerable<Item>, IEnumerable<Augment>) GetItemsAndAugments(int setId)
        {
            var itemsSource = _tftData?.Items;
            if (itemsSource == null)
                return (Enumerable.Empty<Item>(), Enumerable.Empty<Augment>());

            var itemList = new List<Item>();
            var augmentList = new List<Augment>();

            foreach (var item in itemsSource)
            {
                var apiName = item?.ApiName;
                if (string.IsNullOrEmpty(apiName))
                    continue;

                if (!apiName.Contains($"TFT{setId}"))
                    continue;

                var json = JsonConvert.SerializeObject(item);
                if (apiName.IndexOf("augment", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var aug = JsonConvert.DeserializeObject<Augment>(json);
                    if (aug != null)
                        augmentList.Add(aug);
                }
                else
                {
                    var it = JsonConvert.DeserializeObject<Item>(json);
                    if (it != null)
                        itemList.Add(it);
                }
            }

            return (itemList, augmentList);
        }

        /// <summary>
        /// Remove data irrelevant to our purposes by casting to a more limited data model
        /// </summary>
        private static Set RemoveUnneededFields(SetRaw startingSet)
        {
            if (startingSet == null)
                return null;

            var json = JsonConvert.SerializeObject(startingSet);
            var endSet = JsonConvert.DeserializeObject<Set>(json);
            if (endSet?.Champions != null)
                endSet.Champions = endSet.Champions.Where(x => x.Traits != null && x.Traits.Any()).ToList();
            return endSet;
        }
    }
}
