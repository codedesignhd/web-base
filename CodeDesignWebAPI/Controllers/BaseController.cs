using CodeDesign.Dtos.Accounts;
using CodeDesign.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        #region DI
        protected readonly AppDependencyProvider _dependencies;
        public BaseController(AppDependencyProvider dependencies)
        {
            _dependencies = dependencies;
        }

        protected AppUser AppUser => _dependencies.AppUserService.GetCurrentUser();
        #endregion
    }
}
