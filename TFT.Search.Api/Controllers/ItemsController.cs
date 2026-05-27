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
    [Route("set/current/items")]
    public class ItemsController : Controller
    {
        private readonly TftDataStore _builder;

        //private readonly ILogger _logger;

        public ItemsController(TftDataStore builder)
        {
            //_logger = logger;
            _builder = builder;
        }

        [HttpGet, Route("")]
        public async Task<IEnumerable<Item>> Items()
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            return _builder.CurrentSet.Items;
        }

        [HttpGet, Route("{name}")]
        public async Task<IEnumerable<Item>> Items(string name)
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            return _builder.CurrentSet.Items.Where(x => (x.Name ?? string.Empty).ToLower().Contains(searchName));
        }

        [HttpGet, Route("description/{keyword}")]
        public async Task<IEnumerable<Item>> SearchItemDescriptions(string keyword)
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            return _builder.CurrentSet.Items.Where(x => (x.Description ?? string.Empty).ToLower().Contains(searchKeyword));
        }
    }
}
