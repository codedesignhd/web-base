using Asp.Versioning;
using CodeDesign.GoogleService;
using CodeDesign.WebAPI.Models;
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
            _gDriveService.CreateFolder(driveName);
            return new JsonResult("ok");
        }
    }
}
