using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;

namespace TFT.Search.Library.Repositories
{
    /// <summary>
    /// This is the class that pulls the raw JSON from Community Dragon
    /// </summary>
    public interface ITftRepository
    {
        /// <summary>
        /// Returns null when the server responds 304 Not Modified (data unchanged since last fetch).
        /// </summary>
        Task<RawCdragon> GetJsonFileAsync();
    }

    public class TftRepository : ITftRepository
    {
        private volatile string _lastEtag;

        public async Task<RawCdragon> GetJsonFileAsync()
        {
            Url url = "https://raw.communitydragon.org/latest/cdragon/tft/en_us.json";

            var request = url.AllowHttpStatus("304");
            if (!string.IsNullOrEmpty(_lastEtag))
                request = request.WithHeader("If-None-Match", _lastEtag);

            var response = await request.GetAsync();

            //  304 means the data has not changed since our last fetch — skip the download
            if (response.StatusCode == 304)
                return null;

            var etag = response.Headers.FirstOrDefault("ETag");
            if (!string.IsNullOrEmpty(etag))
                _lastEtag = etag;

            var result = await response.GetJsonAsync();
            //  Returns dynamic and we want a string
            var stringResult = JsonConvert.SerializeObject(result);
            //  Map to data objects
            var json = JsonConvert.DeserializeObject<RawCdragon>(stringResult);
            return json;
        }
    }
}
