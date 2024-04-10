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
        [AllowAnonymous, HttpPost, Route("login")]
        public IActionResult Login(string username, string password)
        {

            Account tai_khoan = AccountBL.Instance.Login(username, password);
            if (tai_khoan != null)
            {
                string token = JwtManager.GenToken(tai_khoan);
                _logger.Debug("From Login");
                return new JsonResult(new Response<object>
                {
                    success = true,
                    data = new
                    {
                        token,
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
        [HttpGet, Route("signout")]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return new JsonResult(new Response(true, "success"));
        }
        #endregion

        #region User settings
        [HttpPost]
        [Route("changePassword")]
        public IActionResult ChangePassword(string newPassword)
        {
            return new JsonResult(new Response());
        }
        #endregion

        [AllowAnonymous, HttpGet]
        [Route("Users")]
        public IActionResult GetAllUsers()
        {
            List<Account> accounts = AccountBL.Instance.GetAll();
            return new JsonResult(new Response<List<Account>>() { data = accounts });
        }

        [AllowAnonymous]
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
    }
}
