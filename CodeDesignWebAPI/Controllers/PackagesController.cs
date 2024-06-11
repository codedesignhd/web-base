using CodeDesignBL;
using CodeDesignDtos.Packages;
using CodeDesignWebAPI.Core.Constants;
using CodeDesignWebAPI.Extensions;
using CodeDesignWebAPI.Services.Auth;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesignWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ApiBaseController
    {
        #region DI
        private readonly ILog _logger = LogManager.GetLogger(typeof(PackagesController));
        public PackagesController(IAuthService auth) : base(auth)
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
