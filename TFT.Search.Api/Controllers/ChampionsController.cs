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
            return _builder.CurrentSet.Champions.Where(x => (x.Name ?? string.Empty).ToLower() == name.ToLower());
        }

        [HttpGet, Route("skills/{keyword}")]
        public async Task<IEnumerable<Champion>> SearchChampionSkills(string keyword)
        {
            await _builder.CheckDataLastRetrievedAndRefreshIfNecessaryAsync();
            if (_builder.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            return _builder.CurrentSet.Champions.Where(x => (x.Ability?.Description ?? string.Empty).ToLower().Contains(keyword.ToLower()));
        }
    }
}
