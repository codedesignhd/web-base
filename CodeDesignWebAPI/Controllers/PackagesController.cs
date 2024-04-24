using Asp.Versioning;
using CodeDesign.Dtos.Packages;
using CodeDesign.WebAPI.ServiceExtensions;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : BaseController
    {
        #region DI
        private readonly ILog _logger = LogManager.GetLogger(typeof(PackagesController));
        public PackagesController(AppDependencyProvider dependency) : base(dependency)
        {

        }
        #endregion

        [HttpPost]
        [Route("add-package")]
        public IActionResult AddPackage(AddPackageRequest data)
        {
            return Ok();
        }

        [HttpPost]
        [Route("update-package")]
        public IActionResult UpdatePackage(AddPackageRequest data)
        {
            return Ok();
        }


        [HttpDelete]
        [Route("{id}/delete")]
        public IActionResult Delete(string id)
        {
            return Ok();
        }
    }
}
