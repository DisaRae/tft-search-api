using Microsoft.AspNetCore.Mvc;
using TFT.Search.Library.Models;
using TFT.Search.Library.Repositories;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
        public IEnumerable<Champions>? Champions()
        {
            var dataset = _repository.TftData;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var orderedSetData = dataset.SetData?.OrderByDescending(x => x.Id);
            var currentSet = orderedSetData?.FirstOrDefault();
            return currentSet?.Champions;
        }

        [HttpGet,Route("champions/{name}")]
        public IEnumerable<Champions>? Champions(string name)
        {
            var dataset = _repository.TftData;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var orderedSetData = dataset.SetData?.OrderByDescending(x => x.Id);
            var currentSetChampions = orderedSetData?.FirstOrDefault().Champions;
            var foundChampions = currentSetChampions.Where(x => (x.Name ?? String.Empty).ToLower() == name.ToLower());
            return foundChampions;
        }

        [HttpGet, Route("champions/search/skills/{keyword}")]
        public IEnumerable<Champions>? SearchChampionSkills(string keyword)
        {
            var dataset = _repository.TftData;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var orderedSetData = dataset.SetData?.OrderByDescending(x => x.Id);
            var currentSetChampions = orderedSetData?.FirstOrDefault()?.Champions;
            var foundChampions = currentSetChampions.Where(x => (x.Ability?.Desc ?? String.Empty).ToLower().Contains(keyword.ToLower()));
            return foundChampions;
        }
    }
}
