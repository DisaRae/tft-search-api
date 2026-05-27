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
        }

        [HttpGet, Route("")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Augment> Augments()
        {
            return _builder.CurrentSet.Augments;
        }

        [HttpGet, Route("{name}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Augment> Augments(string name)
        {
            if (_builder.CurrentSet.Augments == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchName = name.ToLower();
            return _builder.AugmentIndex.Where(e => e.NameLower.Contains(searchName)).Select(e => e.Augment);
        }

        [HttpGet, Route("description/{keyword}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Augment> SearchAugmentDescriptions(string keyword)
        {
            if (_builder.CurrentSet.Augments == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var searchKeyword = keyword.ToLower();
            return _builder.AugmentIndex.Where(e => e.DescriptionLower.Contains(searchKeyword)).Select(e => e.Augment);
        }
    }
}
