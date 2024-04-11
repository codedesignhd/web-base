using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        public PackagesController()
        {
            
        }
        [HttpPost]
        [Route("EnrollPackage")]
        public IActionResult EnrollPackage()
        {
            return Ok();
        }
    }
}
