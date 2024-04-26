﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using log4net;
using CodeDesign.Models;
using CodeDesign.BL;
using CodeDesign.Dtos.Responses;
using CodeDesign.Dtos.Accounts;
using CodeDesign.Dtos.Validators;
using CodeDesign.WebAPI.ServiceExtensions;
using Serilog;
using CodeDesign.WebAPI.Services;
using System.Security.Principal;
namespace CodeDesign.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        #region DI
        private readonly ILog _logger = LogManager.GetLogger(typeof(AccountsController));
        public AccountsController(DependencyContainer dependency) : base(dependency)
        {

        }
        #endregion

        #region Register
        [AllowAnonymous]
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            ValidationResult validate = await _dependencies.Validator.ValidateAsync(request);
            if (validate.IsValid)
            {
                var res = AccountBL.Instance.Register(request);
                return Ok(res);
            }
            return BadRequest(new Response(false, validate.GetMessage()));
        }

        #endregion

        #region User settings
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePwdRequest request)
        {
            ValidationResult validate = await _dependencies.Validator.ValidateAsync(request);
            if (validate.IsValid)
            {
                var res = AccountBL.Instance.ChangePassword(request);
                return Ok(res);
            }
            return BadRequest(new Response(false, validate.GetMessage()));
        }

        [HttpPost]
        [Route("update-info")]
        public IActionResult UpdateInfo()
        {
            return Ok(null);
        }

        [HttpPost]
        [Route("update-avatar")]
        public IActionResult UpdateAvatar(IFormFile file)
        {
            var res = _dependencies.File.IsValidImage(file);
            if (res.success)
            {
                try
                {
                    string filePath = string.Format(@"images\avatar\ava_{0}{1}", AppUser.Username, Path.GetExtension(file.FileName));
                    string savePath = Path.Combine(_dependencies.Enviroment.ContentRootPath, filePath);
                    using (FileStream ms = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(ms);
                    }
                    bool success = AccountBL.Instance.UpdateAvatar(AppUser.Username, filePath.Replace("\\", "/"));
                    if (success)
                    {
                        return Ok(new Response(true, "Cập nhật avatar thành công"));
                    }
                    return Ok(new Response(false, "Có lỗi khi cập nhật, vui lòng thử lại"));
                }
                catch (Exception)
                {
                    return Ok(new Response(false, "Có lỗi khi lưu ảnh, vui lòng thử lại"));
                }
            }
            return BadRequest(res);
        }

        /// <summary>
        /// Gửi yêu cầu đặt lại mật khẩu qua email
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("recovery-password")]
        [AllowAnonymous]
        public IActionResult RecoverPassword(string identity)
        {
            if (string.IsNullOrWhiteSpace(identity))
                return BadRequest(new Response(false, "Vui lòng nhập email hoặc username để tiếp tục"));
            var res = AccountBL.Instance.RecoverPassword(identity);
            return Ok(res);
        }

        /// <summary>
        /// Xác minh token đặt lại mật khẩu từ link trong email
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("verify-recover-password-token")]
        [AllowAnonymous]
        public IActionResult VerifyRecoverPasswordToken(string token)
        {
            var res = AccountBL.Instance.VerifyRecoverPasswordToken(token);
            return Ok(res);
        }

        /// <summary>
        /// Đặt lại mật khẩu mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var res = AccountBL.Instance.ResetPassword(request);
            return Ok(res);
        }
        #endregion

        #region Others
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            List<Account> accounts = AccountBL.Instance.GetAll();
            return Ok(new Response<List<Account>>() { data = accounts });
        }

        [HttpGet("check-user-exist")]
        public IActionResult CheckUserExist(string username)
        {
            bool isExist = AccountBL.Instance.IsUserExist(username);
            string message = "Username is available";
            if (isExist)
            {
                message = "Username has been registed by another";
            }
            return Ok(new Response<object>()
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
