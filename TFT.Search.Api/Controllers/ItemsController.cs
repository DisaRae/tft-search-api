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
        }

        [HttpGet, Route("")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Item> Items()
        {
            return _builder.CurrentSet.Items;
        }

        [HttpGet, Route("{name}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Item> Items(string name)
        {
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            return _builder.ItemIndex.Where(e => e.NameLower.Contains(searchName)).Select(e => e.Item);
        }

        [HttpGet, Route("description/{keyword}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Item> SearchItemDescriptions(string keyword)
        {
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            return _builder.ItemIndex.Where(e => e.DescriptionLower.Contains(searchKeyword)).Select(e => e.Item);
        }
    }
}
