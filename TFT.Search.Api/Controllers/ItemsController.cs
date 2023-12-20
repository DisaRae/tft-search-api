
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TFT.Search.Library.Models;
using TFT.Search.Library.Repositories;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("set/current/items")]
    public class ItemsController : Controller
    {
        private readonly TftDataBuilder _builder;
        private const string _key = "53567bbe-4d5f-4513-8595-3d9417839f93";

        //private readonly ILogger _logger;

        public ItemsController(TftDataBuilder builder)
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
            var currentSetItems = _builder.CurrentSet.Items;
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            var foundItems = currentSetItems.Where(x => (x.Name ?? String.Empty).ToLower().Contains(searchName));
            return foundItems;
        }

        [HttpGet, Route("description/{keyword}")]
        public IEnumerable<Item> SearchItemDescriptions(string keyword)
        {
            var currentSetItems = _builder.CurrentSet.Items;
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            var foundItems = currentSetItems.Where(x => (x.Description ?? String.Empty).ToLower().Contains(searchKeyword));
            return foundItems;
        }
    }
}
