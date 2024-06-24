using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TFT.Search.Library.Models;
using TFT.Search.Library.Models.RawData;
using TFT.Search.Library.Repositories;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SetController : ControllerBase
    {
        private readonly TftDataBuilder _builder;

        //private readonly ILogger _logger;

        public SetController(TftDataBuilder builder)
        {
            //_logger = logger;
            _builder = builder;
            _builder.CheckDataLastRetrievedAndRefreshIfNecessary();
        }


        [HttpGet, Route("current")]
        public Set Get()
        {
            var dataset = _builder.CurrentSet;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            return dataset;
        }

        [HttpGet, Route("current/traits")]
        public IEnumerable<Traits> Traits()
        {
            var dataset = _builder.CurrentSet;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            return dataset.Traits;
        }
    }
}