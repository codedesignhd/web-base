using Asp.Versioning;
using CodeDesign.WebAPI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    //[ApiVersion(AppApiVersion.v1)]
    //[Route("api/v{v:apiVersion}/[controller]")]
    //[ApiController]
    //public class VersionsController : ControllerBase
    //{
    //    [MapToApiVersion(AppApiVersion.v1)]
    //    [HttpGet]
    //    [Route("GetVer")]
    //    public IActionResult Ver1()
    //    {
    //        return Ok(new { ver = "1.0" });
    //    }


    //    //[MapToApiVersion(2.0)]
    //    //[HttpGet]
    //    //[Route("GetVer")]
    //    //public IActionResult Ver2()
    //    //{
    //    //    return Ok(new { ver = "2.0" });
    //    //}
    //}
}
