using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFT.Search.Library.Models;
using TFT.Search.Library.Repositories;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("set/current/augments")]
    public class AugmentController : Controller
    {
        private readonly TftDataStore _builder;

        //private readonly ILogger _logger;

        public AugmentController(TftDataStore builder)
        {
            //_logger = logger;
            _builder = builder;
        }

        [HttpGet, Route("")]
        public async Task<IEnumerable<Augment>> Augments()
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            return _builder.CurrentSet.Augments;
        }

        [HttpGet, Route("{name}")]
        public async Task<IEnumerable<Augment>> Augments(string name)
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            if (_builder.CurrentSet.Augments == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            return _builder.AugmentIndex.Where(e => e.NameLower.Contains(searchName)).Select(e => e.Augment);
        }

        [HttpGet, Route("description/{keyword}")]
        public async Task<IEnumerable<Augment>> SearchAugmentDescriptions(string keyword)
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            if (_builder.CurrentSet.Augments == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            return _builder.AugmentIndex.Where(e => e.DescriptionLower.Contains(searchKeyword)).Select(e => e.Augment);
        }
    }
}
