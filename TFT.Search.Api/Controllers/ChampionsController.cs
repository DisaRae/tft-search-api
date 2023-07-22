
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
        private const string _key = "53567bbe-4d5f-4513-8595-3d9417839f93";

        //private readonly ILogger _logger;

        public ChampionsController(TftRepository repository)
        {
            //_logger = logger;
            _repository = repository;
            _repository.CheckDataLastRetrievedAndRefreshIfNecessary();
        }

        [HttpGet, Route("champions")]
        public IEnumerable<Champion> Champions()
        {
            return _repository.CurrentSet?.Champions;
        }

        [HttpGet, Route("champions/{name}")]
        public IEnumerable<Champion> Champions(string name)
        {
            var currentSetChampions = _repository.CurrentSet?.Champions;
            if (_repository.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var foundChampions = currentSetChampions.Where(x => (x.Name ?? String.Empty).ToLower() == name.ToLower());
            return foundChampions;
        }

        [HttpGet, Route("champions/search/skills/{keyword}")]
        public IEnumerable<Champion> SearchChampionSkills(string keyword)
        {
            var currentSetChampions = _repository.CurrentSet?.Champions;
            if (_repository.CurrentSet?.Champions == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var foundChampions = currentSetChampions.Where(x => (x.Ability?.Description ?? String.Empty).ToLower().Contains(keyword.ToLower()));
            return foundChampions;
        }
    }
}
