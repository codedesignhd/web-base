using Asp.Versioning;
using CodeDesign.BL;
using CodeDesign.Dtos.Packages;
using CodeDesign.WebAPI.Core.Authorization;
using CodeDesign.WebAPI.Core.Constants;
using CodeDesign.WebAPI.ServiceExtensions;
using log4net;
using Microsoft.AspNetCore.Authorization;
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
        public PackagesController(DependencyContainer dependency) : base(dependency)
        {

        }
        #endregion


        [HttpPost]
        [Route("add-package")]
        [Authorize(Policy = PolicyNames.AdminOnly)]
        public IActionResult AddPackage(CreatePackageRequest data)
        {
            return Ok();
        }

        [HttpPost]
        [Route("update-package")]
        public IActionResult UpdatePackage(CreatePackageRequest data)
        {
            return Ok();
        }


        [HttpDelete]
        [Route("{id}/delete")]
        public IActionResult Delete(string id)
        {
            var res = PackageBL.Instance.DeletePack(id);
            return Ok(res);
        }
    }
}
