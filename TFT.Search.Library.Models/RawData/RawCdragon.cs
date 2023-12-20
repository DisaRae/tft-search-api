using System.Collections.Generic;

namespace TFT.Search.Library.Models.RawData
{
    /// <summary>
    /// https://raw.communitydragon.org/latest/cdragon/tft/en_us.json
    /// </summary>
    public class RawCdragon
    {
        public List<ItemRaw> Items { get; set; }
        public List<SetRaw> SetData { get; set; }
    }
}