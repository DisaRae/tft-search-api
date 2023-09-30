
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
    [Route("currentset")]
    public class ItemsController : Controller
    {
        private readonly TftRepository _repository;
        private const string _key = "53567bbe-4d5f-4513-8595-3d9417839f93";

        //private readonly ILogger _logger;

        public ItemsController(TftRepository repository)
        {
            //_logger = logger;
            _repository = repository;
            _repository.CheckDataLastRetrievedAndRefreshIfNecessary();
        }

        [HttpGet, Route("items")]
        public IEnumerable<Item> Items()
        {
            return _repository.Items;
        }

        [HttpGet, Route("items/{name}")]
        public IEnumerable<Item> Items(string name)
        {
            var currentSetItems = _repository.Items;
            if (_repository.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            var foundItems = currentSetItems.Where(x => (x.Name ?? String.Empty).ToLower().Contains(searchName));
            return foundItems;
        }

        [HttpGet, Route("items/search/description/{keyword}")]
        public IEnumerable<Item> SearchItemDescriptions(string keyword)
        {
            var currentSetItems = _repository.Items;
            if (_repository.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            var foundItems = currentSetItems.Where(x => (x.Description ?? String.Empty).ToLower().Contains(searchKeyword));
            return foundItems;
        }
    }
}
