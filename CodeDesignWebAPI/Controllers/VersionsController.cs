using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class VersionsController : ControllerBase
    {
        [MapToApiVersion(1.0)]
        [HttpGet]
        [Route("GetVer")]
        public IActionResult Ver1()
        {
            return Ok(new { ver = "1.0" });
        }


        //[MapToApiVersion(2.0)]
        //[HttpGet]
        //[Route("GetVer")]
        //public IActionResult Ver2()
        //{
        //    return Ok(new { ver = "2.0" });
        //}
    }
}
