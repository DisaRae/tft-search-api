
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TFT.Search.Library.Models;
using TFT.Search.Library.Repositories;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("currentset")]
    public class ChampionsController : Controller
    {
        private readonly TftRepository _repository;

        //private readonly ILogger _logger;

        public ChampionsController(TftRepository repository)
        {
            //_logger = logger;
            _repository = repository;
            _repository.CheckDataLastRetrievedAndRefreshIfNecessary();
        }

        [HttpGet, Route("champions")]
        public IEnumerable<ChampionRaw>? Champions()
        {
            return _repository.CurrentSet?.Champions;
        }

        [HttpGet,Route("champions/{name}")]
        public IEnumerable<ChampionRaw> Champions(string name)
        {
            var currentSetChampions = _repository.CurrentSet?.Champions;
            if (_repository.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var foundChampions = currentSetChampions.Where(x => (x.Name ?? String.Empty).ToLower() == name.ToLower());
            return foundChampions;
        }

        [HttpGet, Route("champions/search/skills/{keyword}")]
        public IEnumerable<ChampionRaw> SearchChampionSkills(string keyword)
        {
            var currentSetChampions = _repository.CurrentSet?.Champions;
            if (_repository.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var foundChampions = currentSetChampions.Where(x => (x.Ability?.Desc ?? String.Empty).ToLower().Contains(keyword.ToLower()));
            return foundChampions;
        }
    }
}
