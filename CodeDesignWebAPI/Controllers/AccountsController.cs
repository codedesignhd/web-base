using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using log4net;
using CodeDesign.Models;
using CodeDesign.BL;
using CodeDesign.Dtos.Responses;
using CodeDesign.Dtos.Accounts;
using CodeDesign.Dtos.Validators;
using CodeDesign.WebAPI.ServiceExtensions;
namespace CodeDesign.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        #region DI
        private readonly ILog _logger = LogManager.GetLogger(typeof(AccountsController));
        public AccountsController(AppDependencyProvider dependency) : base(dependency)
        {

        }
        #endregion

        #region Register
        [AllowAnonymous]
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            ValidationResult result = await _dependencies.Validator.ValidateAsync(request);
            if (result.IsValid)
            {
                var res = AccountBL.Instance.Register(request);
                return new JsonResult(res);
            }
            return new JsonResult(new Response(false, result.GetMessage()));
        }

        #endregion

        #region User settings
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePwdRequest request)
        {
            ValidationResult result = await _dependencies.Validator.ValidateAsync(request);
            if (result.IsValid)
            {
                var res = AccountBL.Instance.ChangePassword(request);
                return new JsonResult(res);
            }
            return new JsonResult(new Response(false, result.GetMessage()));
        }

        [HttpPost]
        [Route("update-info")]
        public IActionResult UpdateInfo()
        {
            return new JsonResult(null);
        }

        [HttpPost]
        [Route("update-avatar")]
        public IActionResult UpdateAvatar(IFormFile file)
        {
            return new JsonResult(null);
        }

        #endregion

        #region Others
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            List<Account> accounts = AccountBL.Instance.GetAll();
            return new JsonResult(new Response<List<Account>>() { data = accounts });
        }

        [HttpGet("check-user-exist")]
        public IActionResult CheckUserExist(string username)
        {
            bool isExist = AccountBL.Instance.IsUserExist(username);
            string message = "Username is avaiable";
            if (isExist)
            {
                message = "Username has been registed by another";
            }
            return new JsonResult(new Response<object>()
            {
                success = true,
                message = message,
                data = new
                {
                    isExist,
                }
            });
        }
        #endregion

    }
}
