
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
    [Route("set/current/augments")]
    public class AugmentController : Controller
    {
        private readonly TftDataBuilder _builder;
        private const string _key = "53567bbe-4d5f-4513-8595-3d9417839f93";

        //private readonly ILogger _logger;

        public AugmentController(TftDataBuilder builder)
        {
            //_logger = logger;
            _builder = builder;
            _builder.CheckDataLastRetrievedAndRefreshIfNecessary();
        }

        [HttpGet, Route("")]
        public IEnumerable<Augment> Augments()
        {
            return _builder.CurrentSet.Augments;
        }

        [HttpGet, Route("{name}")]
        public IEnumerable<Augment> Augments(string name)
        {
            var currentSetItems = _builder.CurrentSet.Augments;
            if (_builder.CurrentSet.Items == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            var foundItems = currentSetItems.Where(x => (x.Name ?? String.Empty).ToLower().Contains(searchName));
            return foundItems;
        }

        [HttpGet, Route("description/{keyword}")]
        public IEnumerable<Augment> SearchAugmentDescripttion(string keyword)
        {
            var currentSetAugments = _builder.CurrentSet.Augments;
            if (_builder.CurrentSet.Augments == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            var foundItems = currentSetAugments.Where(x => (x.Description ?? String.Empty).ToLower().Contains(searchKeyword));
            return foundItems;
        }
    }
}
