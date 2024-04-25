using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KlioWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
