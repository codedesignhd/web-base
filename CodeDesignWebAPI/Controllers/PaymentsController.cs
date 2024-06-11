using Asp.Versioning;
using CodeDesignBL;
using CodeDesignDtos.Vnpay;
using CodeDesignWebAPI.Core.Constants;
using CodeDesignWebAPI.Extensions;
using CodeDesignWebAPI.Services.Auth;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace CodeDesignWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ApiBaseController
    {
        #region DI
        private readonly ILog _logger = LogManager.GetLogger(typeof(PackagesController));
        public PaymentsController(IAuthService auth) : base(auth)
        {

        }
        #endregion

        [HttpGet]
        [Route("get-active-payment")]
        public IActionResult GetActivePayment()
        {
            return Ok();
        }

        [HttpGet]
        [Route("get-all-payment")]
        [Authorize(Policy = PolicyNames.AdminOnly)]
        public IActionResult GetAllPayments()
        {
            return Ok();
        }

        [HttpGet]
        [Route("get-user-payments")]
        public IActionResult GetUserPayment()
        {
            var res = PaymentBL.Instance.GetPaymentHistoriesByUser(AppUser.Username);
            return Ok(res);
        }


        /// <summary>
        /// Client post thông tin thanh toán, server trả lại link redirect đến VnPay
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("vnpay/create-order")]
        public IActionResult CreateVnPayOrder(VnPayPaymentRequest request)
        {
            var res = VnPayBL.Instance.CreatePaymentUrl(request);
            return Ok(res);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPaymentResult()
        {
            return Ok();
        }
    }
}
