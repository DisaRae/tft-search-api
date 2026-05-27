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
        Task<RawCdragon> GetJsonFileAsync();
    }

    public class TftRepository : ITftRepository
    {
        public async Task<RawCdragon> GetJsonFileAsync()
        {
            var url = "https://raw.communitydragon.org/latest/cdragon/tft/en_us.json";

            var result = await url.GetJsonAsync();
            //  Returns dynamic and we want a string
            var stringResult = JsonConvert.SerializeObject(result);
            //  Map to data objects
            var json = JsonConvert.DeserializeObject<RawCdragon>(stringResult);
            return json;
        }
    }
}
