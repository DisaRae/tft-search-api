using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<Champion>> Champions()
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            return _builder.CurrentSet?.Champions;
        }

        [HttpGet, Route("{name}")]
        public async Task<IEnumerable<Champion>> Champions(string name)
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var nameLower = name.ToLower();
            return _builder.ChampionIndex.Where(e => e.NameLower == nameLower).Select(e => e.Champion);
        }

        [HttpGet, Route("skills/{keyword}")]
        public async Task<IEnumerable<Champion>> SearchChampionSkills(string keyword)
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var keywordLower = keyword.ToLower();
            return _builder.ChampionIndex.Where(e => e.DescriptionLower.Contains(keywordLower)).Select(e => e.Champion);
        }
    }
}
