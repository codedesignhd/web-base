using CodeDesignWebAPI.Core.Authorization;
using CodeDesignDtos.Accounts;
using Microsoft.AspNetCore.Mvc;
using CodeDesignWebAPI.Extensions;

namespace CodeDesignWebAPI.Controllers
{
    [Authorized]
    public class ApiBaseController : ControllerBase
    {
        #region DI
        protected readonly ServicesPool _services;
        public ApiBaseController(ServicesPool services)
        {
            _services = services;
        }

        protected AppUser AppUser => _services.Auth.GetUser();
        #endregion
    }
}
