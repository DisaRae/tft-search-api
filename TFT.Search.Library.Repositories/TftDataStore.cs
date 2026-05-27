using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFT.Search.Library.Models;

namespace TFT.Search.Library.Repositories
{
    /// <summary>
    /// An instance of this class is used to store TFT data in memory. It will be refreshed every 2 hours, or when the application starts up. This is to avoid making unnecessary calls to the Riot API, which has rate limits.
    /// </summary>
    public class TftDataStore
    {
        public Set CurrentSet { get; set; }
        public int CurrentSetId { get; set; }
        public DateTime? DataLastRetrieved { get; set; }

        // Pre-built search indices: lowercased fields computed once at refresh time so
        // controllers pay zero ToLower() cost per request.
        public IReadOnlyList<ChampionSearchEntry> ChampionIndex { get; private set; } = Array.Empty<ChampionSearchEntry>();
        public IReadOnlyList<ItemSearchEntry> ItemIndex { get; private set; } = Array.Empty<ItemSearchEntry>();
        public IReadOnlyList<AugmentSearchEntry> AugmentIndex { get; private set; } = Array.Empty<AugmentSearchEntry>();

        private readonly ITftService _tftService;

        public TftDataStore(ITftService tftService)
        {
            _tftService = tftService;
            // Constructors cannot be async; this single blocking call is acceptable at DI startup time.
            CheckDataLastRetrievedAndRefreshIfNecessaryAsync().GetAwaiter().GetResult();
            DataLastRetrieved = DateTime.Now.AddMinutes(-5);
        }

        public async Task CheckDataLastRetrievedAndRefreshIfNecessaryAsync()
        {
            if ((DataLastRetrieved ?? DateTime.MinValue).AddHours(2) < DateTime.Now)
            {
                await _tftService.RefreshDataAsync();
                CurrentSetId = _tftService.GetCurrentSetId() ?? 0;
                // We aren't using historic sets, so why waste resouces cleaning and populating it?
                //AllSets = _tftService.GetSets();
                CurrentSet = _tftService.GetCurrentSet();
                DataLastRetrieved = DateTime.Now;

                BuildSearchIndices();
            }
        }

        private void BuildSearchIndices()
        {
            ChampionIndex = (CurrentSet?.Champions ?? new List<Champion>())
                .Select(c => new ChampionSearchEntry(
                    c,
                    (c.Name ?? string.Empty).ToLower(),
                    (c.Ability?.Description ?? string.Empty).ToLower()))
                .ToList();

            ItemIndex = (CurrentSet?.Items ?? new List<Item>())
                .Select(i => new ItemSearchEntry(
                    i,
                    (i.Name ?? string.Empty).ToLower(),
                    (i.Description ?? string.Empty).ToLower()))
                .ToList();

            AugmentIndex = (CurrentSet?.Augments ?? new List<Augment>())
                .Select(a => new AugmentSearchEntry(
                    a,
                    (a.Name ?? string.Empty).ToLower(),
                    (a.Description ?? string.Empty).ToLower()))
                .ToList();
        }
    }

    public sealed record ChampionSearchEntry(Champion Champion, string NameLower, string DescriptionLower);
    public sealed record ItemSearchEntry(Item Item, string NameLower, string DescriptionLower);
    public sealed record AugmentSearchEntry(Augment Augment, string NameLower, string DescriptionLower);
}
