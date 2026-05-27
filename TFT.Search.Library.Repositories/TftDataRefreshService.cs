using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TFT.Search.Library.Repositories
{
    /// <summary>
    /// Runs in the background and refreshes TftDataStore every 2 hours so the cache check
    /// never blocks a request thread.
    /// </summary>
    public class TftDataRefreshService : BackgroundService
    {
        private readonly TftDataStore _dataStore;

        public TftDataRefreshService(TftDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
                await _dataStore.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            }
        }
    }
}
