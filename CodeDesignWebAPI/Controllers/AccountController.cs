using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using log4net;
using CodeDesign.Models;
using CodeDesign.BL;
using CodeDesign.WebAPI.Services;
using CodeDesign.DTO.Validators;
using CodeDesign.BL.Response;
using CodeDesign.DTO.Dtos.Account;

namespace CodeDesign.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        #region DI
        private readonly ILog _logger = LogManager.GetLogger(typeof(AccountController));
        public AccountController(AppDependencyProvider dependency) : base(dependency)
        {

        }
        #endregion

        [AllowAnonymous, HttpGet]
        [Route("users")]
        public IActionResult GetAllUsers()
        {
            List<Account> accounts = AccountBL.Instance.GetAll();
            return new JsonResult(new Response<List<Account>>() { data = accounts });
        }

        [AllowAnonymous, HttpPost, Route("login")]
        public IActionResult Login(string username, string password)
        {
            Account account = AccountBL.Instance.Login(username, password);
            if (account != null)
            {
                string token = JwtManager.GenToken(account);
                _logger.Debug("From Login");
                return new JsonResult(new Response<object>
                {
                    success = true,
                    data = new
                    {
                        access_token = token,
                    }
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

        [AllowAnonymous, HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            ValidationResult result = await _dependencies.Validator.Register.ValidateAsync(dto);
            if (result.IsValid)
            {
                KeyValuePair<bool, string> res = AccountBL.Instance.Register(dto);
                return new JsonResult(new Response(res));
            }
            return new JsonResult(new Response(false, result.GetMessage()));
        }

        [HttpPost]
        [Route("changePassword")]
        public IActionResult ChangePassword(string newPassword)
        {
            return new JsonResult(new Response());
        }



        [HttpGet, Route("signout")]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return new JsonResult(new Response(true, "success"));
        }

        [AllowAnonymous]
        [HttpGet, Route("checkUserExist")]
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
    }
}
