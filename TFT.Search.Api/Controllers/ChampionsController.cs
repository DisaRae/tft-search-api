
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
        private readonly TftDataBuilder _builder;
        private const string _key = "53567bbe-4d5f-4513-8595-3d9417839f93";

        //private readonly ILogger _logger;

        public ChampionsController(TftDataBuilder builder)
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
            var currentSetChampions = _builder.CurrentSet?.Champions;
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var foundChampions = currentSetChampions.Where(x => (x.Name ?? String.Empty).ToLower() == name.ToLower());
            return foundChampions;
        }

        [HttpGet, Route("skills/{keyword}")]
        public IEnumerable<Champion> SearchChampionSkills(string keyword)
        {
            var currentSetChampions = _builder.CurrentSet?.Champions;
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var foundChampions = currentSetChampions.Where(x => (x.Ability?.Desc ?? String.Empty).ToLower().Contains(keyword.ToLower()));
            return foundChampions;
        }
    }
}
