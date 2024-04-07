using CodeDesign.Models;
using CodeDesign.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        #region DI
        protected readonly DependencyContainer _dependencies;
        public BaseController(DependencyContainer dependencies)
        {
            _dependencies = dependencies;
        }

        protected AppUser AppUser => _dependencies.AppUserService.GetPrincipal();
        #endregion
    }
}
