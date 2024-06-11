using Asp.Versioning;
using CodeDesignWebAPI.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesignWebAPI.Controllers
{
    [ApiVersion(AppApiVersion.v1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {

        public TestController()
        {

        }
        [HttpPost]
        public IActionResult CreateDrive([FromForm] string driveName)
        {

            return Ok("ok");
        }
        [HttpGet]
        [Route("~/Auth")]
        public IActionResult Auth()
        {
            return Ok("ok");
        }
    }

}
