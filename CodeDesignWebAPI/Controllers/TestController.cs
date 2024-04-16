using Asp.Versioning;
using CodeDesign.GoogleService;
using CodeDesign.WebAPI.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [ApiVersion(AppApiVersion.v1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        private GDriverService _gDriveService;
        public TestController(GDriverService gDriverService)
        {
            _gDriveService = gDriverService;
        }
        [HttpPost]
        public IActionResult CreateDrive([FromForm] string driveName)
        {
            _gDriveService.Test();
            return new JsonResult("ok");
        }
        [HttpGet]
        [Route("~/Auth")]
        public IActionResult Auth()
        {
            return new JsonResult("ok");
        }
    }

}
