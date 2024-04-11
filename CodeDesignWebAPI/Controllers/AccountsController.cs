using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using log4net;
using CodeDesign.Models;
using CodeDesign.BL;
using CodeDesign.WebAPI.Services;
using CodeDesign.BL.Response;
using CodeDesign.Dtos;
using Asp.Versioning;
using CodeDesign.Dtos.Account;
using Humanizer;

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

        #region Auth
        /// <summary>
        /// Create Auth Request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            Account account = AccountBL.Instance.Login(username, password);
            if (account != null)
            {
                string token = JwtManager.GenToken(account);
                return new JsonResult(new Response<string>
                {
                    success = true,
                    message = "Login success",
                    data = token,
                });
            }
            else
            {
                return new JsonResult(new Response
                {
                    success = false,
                    message = "Tài khoản hoặc mật khẩu không chính xác"
                });
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("Register")]
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
        [HttpGet, Route("SignOut")]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return new JsonResult(new Response(true, "success"));
        }
        #endregion

        #region User settings
        [HttpPost]
        [Route("ChangePassword")]
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
        [Route("UpdateInfo")]
        public IActionResult UpdateInfo()
        {
            return new JsonResult(null);
        }

        [HttpPost]
        [Route("UpdateAvatar")]
        public IActionResult UpdateAvatar(IFormFile file)
        {
            return new JsonResult(null);
        }

        #endregion

        #region Others
        [AllowAnonymous, HttpGet]
        [Route("Users")]
        public IActionResult GetAllUsers()
        {
            List<Account> accounts = AccountBL.Instance.GetAll();
            return new JsonResult(new Response<List<Account>>() { data = accounts });
        }

        [HttpGet, Route("CheckUserExist")]
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
