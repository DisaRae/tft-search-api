using System.Collections.Generic;

namespace TFT.Search.Library.Models 
{
    /// <summary>
    /// https://raw.communitydragon.org/latest/cdragon/tft/en_us.json
    /// </summary>
    public class RawCdragon
    {
        public List<object> Items { get; set; }
        public List<SetRaw> SetData { get; set; }
    }
}