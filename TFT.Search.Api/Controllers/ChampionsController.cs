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
        }

        [HttpGet, Route("")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Champion> Champions()
        {
            return _builder.CurrentSet?.Champions;
        }

        [HttpGet, Route("{name}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Champion> Champions(string name)
        {
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var nameLower = name.ToLower();
            return _builder.ChampionIndex.Where(e => e.NameLower == nameLower).Select(e => e.Champion);
        }

        [HttpGet, Route("skills/{keyword}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public IEnumerable<Champion> SearchChampionSkills(string keyword)
        {
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var keywordLower = keyword.ToLower();
            return _builder.ChampionIndex.Where(e => e.DescriptionLower.Contains(keywordLower)).Select(e => e.Champion);
        }
    }
}
