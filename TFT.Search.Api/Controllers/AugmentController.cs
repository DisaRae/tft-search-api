using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (_builder.CurrentSet.Augments == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            return _builder.CurrentSet.Augments.Where(x => (x.Name ?? string.Empty).ToLower().Contains(searchName));
        }

        [HttpGet, Route("description/{keyword}")]
        public IEnumerable<Augment> SearchAugmentDescriptions(string keyword)
        {
            if (_builder.CurrentSet.Augments == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            return _builder.CurrentSet.Augments.Where(x => (x.Description ?? string.Empty).ToLower().Contains(searchKeyword));
        }
    }
}
