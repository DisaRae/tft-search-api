using System;
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
            }
        }
    }
}
