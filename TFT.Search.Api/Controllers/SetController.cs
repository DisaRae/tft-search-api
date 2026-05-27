using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TFT.Search.Library.Models;
using TFT.Search.Library.Repositories;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SetController : ControllerBase
    {
        private readonly TftDataStore _builder;

        //private readonly ILogger _logger;

        public SetController(TftDataStore builder)
        {
            //_logger = logger;
            _builder = builder;
        }

        [HttpGet, Route("current")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public Set Get()
        {
            var dataset = _builder.CurrentSet;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            return dataset;
        }

        [HttpGet, Route("current/traits")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Traits> Traits()
        {
            var dataset = _builder.CurrentSet;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            return dataset.Traits;
        }
    }
}
