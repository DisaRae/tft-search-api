using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFT.Search.Library.Models.RawData;
using TFT.Search.Library.Models;

namespace TFT.Search.Library.Repositories
{
    public class TftDataBuilder
    {
        public Set CurrentSet { get; set; }
        public IEnumerable<Set> AllSets { get; set; }
        public int CurrentSetId { get; set; }
        public DateTime? DataLastRetrieved { get; set; }

        private readonly ITftService _tftService;

        public TftDataBuilder(ITftService tftService)
        {
            _tftService = tftService;
            CheckDataLastRetrievedAndRefreshIfNecessary();
            DataLastRetrieved = DateTime.Now.AddMinutes(-5);
        }

        public void CheckDataLastRetrievedAndRefreshIfNecessary()
        {
            if ((DataLastRetrieved ?? DateTime.MinValue).AddHours(2) < DateTime.Now)
            {
                CurrentSetId = _tftService.GetCurrentSetId() ?? 0;
                AllSets = _tftService.GetSets();
                CurrentSet = _tftService.GetSet(CurrentSetId);
            }
        }
    }
}
