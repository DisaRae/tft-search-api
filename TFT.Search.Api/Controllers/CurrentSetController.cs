using Microsoft.AspNetCore.Mvc;
using TFT.Search.Library.Models;
using TFT.Search.Library.Repositories;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrentSetController : ControllerBase
    {
        private readonly TftRepository _repository;

        //private readonly ILogger _logger;

        public CurrentSetController(TftRepository repository)
        {
            //_logger = logger;
            _repository = repository;
            _repository.CheckDataLastRetrievedAndRefreshIfNecessary();
        }


        [HttpGet, Route("")]
        public Set? Get()
        {
            var dataset = _repository.TftData;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var orderedSetData = dataset.SetData?.OrderByDescending(x => x.Id);
            var currentSet = orderedSetData?.FirstOrDefault();
            return currentSet;
        }

        [HttpGet, Route("traits")]
        public IEnumerable<Traits>? Traits()
        {
            var dataset = _repository.TftData;
            if (dataset == null)
                throw new Exception("Unable to retrieve TFT data at this time");
            var orderedSetData = dataset.SetData?.OrderByDescending(x => x.Id);
            var currentSet = orderedSetData?.FirstOrDefault();
            return currentSet?.Traits;
        }
    }
}