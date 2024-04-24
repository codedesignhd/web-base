using CodeDesign.Dtos.Accounts;
using CodeDesign.WebAPI.Core.Authorization;
using CodeDesign.WebAPI.ServiceExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [Authorized]
    public class BaseController : ControllerBase
    {
        #region DI
        protected readonly DependencyContainer _dependencies;
        public BaseController(DependencyContainer dependencies)
        {
            _dependencies = dependencies;
        }

        protected AppUser AppUser => _dependencies.Auth.GetUser();
        #endregion
    }
}
