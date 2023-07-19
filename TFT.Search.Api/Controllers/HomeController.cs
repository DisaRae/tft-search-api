using Microsoft.AspNetCore.Mvc;

namespace TFT.Search.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet, Route("")]
        public string Index()
        {
            return "Heartbeat";
        }
    }
}
