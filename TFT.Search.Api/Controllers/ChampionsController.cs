using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TFT.Search.Library.Models;
using TFT.Search.Library.Repositories;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("set/current/champions")]
    public class ChampionsController : Controller
    {
        private readonly TftDataStore _builder;

        //private readonly ILogger _logger;

        public ChampionsController(TftDataStore builder)
        {
            //_logger = logger;
            _builder = builder;
            _builder.CheckDataLastRetrievedAndRefreshIfNecessary();
        }

        [HttpGet, Route("")]
        public IEnumerable<Champion> Champions()
        {
            return _builder.CurrentSet?.Champions;
        }

        [HttpGet, Route("{name}")]
        public IEnumerable<Champion> Champions(string name)
        {
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            return _builder.CurrentSet.Champions.Where(x => (x.Name ?? string.Empty).ToLower() == name.ToLower());
        }

        [HttpGet, Route("skills/{keyword}")]
        public IEnumerable<Champion> SearchChampionSkills(string keyword)
        {
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            return _builder.CurrentSet.Champions.Where(x => (x.Ability?.Description ?? string.Empty).ToLower().Contains(keyword.ToLower()));
        }
    }
}
