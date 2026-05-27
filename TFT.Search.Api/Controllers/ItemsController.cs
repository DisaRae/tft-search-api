using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _builder.CheckDataLastRetrievedAndRefreshIfNecessary();
        }

        [HttpGet, Route("")]
        public IEnumerable<Item> Items()
        {
            return _builder.CurrentSet.Items;
        }

        [HttpGet, Route("{name}")]
        public IEnumerable<Item> Items(string name)
        {
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            return _builder.CurrentSet.Items.Where(x => (x.Name ?? string.Empty).ToLower().Contains(searchName));
        }

        [HttpGet, Route("description/{keyword}")]
        public IEnumerable<Item> SearchItemDescriptions(string keyword)
        {
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            return _builder.CurrentSet.Items.Where(x => (x.Description ?? string.Empty).ToLower().Contains(searchKeyword));
        }
    }
}
