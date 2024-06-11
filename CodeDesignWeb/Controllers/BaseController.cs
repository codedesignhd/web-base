using CodeDesign.Web.Services;
using CodeDesignDtos.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        #region DI
        protected readonly ServicePool _services;
        public BaseController(ServicePool dependencies)
        {
            _services = dependencies;
        }

        protected AppUser AppUser => _services.AppUserService.GetPrincipal();
        #endregion
    }
}
