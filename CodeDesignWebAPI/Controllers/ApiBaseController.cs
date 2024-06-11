using CodeDesignWebAPI.Core.Authorization;
using CodeDesignDtos.Accounts;
using Microsoft.AspNetCore.Mvc;
using CodeDesignWebAPI.Extensions;
using CodeDesignWebAPI.Services.Auth;

namespace CodeDesignWebAPI.Controllers
{
    [Authorized]
    public class ApiBaseController : ControllerBase
    {
        #region DI
        protected readonly IAuthService _auth;
        public ApiBaseController(IAuthService auth)
        {
            _auth = auth;
        }

        protected AppUser AppUser => _auth.GetUser();
        #endregion
    }
}
