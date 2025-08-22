using Microsoft.AspNetCore.Mvc;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/test")]

    public class TestController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}