using Asp.Versioning;
using CodeDesign.WebAPI.ServiceExtensions;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        #region DI
        private readonly ILog _logger = LogManager.GetLogger(typeof(PackagesController));
        public PaymentsController(AppDependencyProvider dependency) : base(dependency)
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
        public IActionResult GetAllPayments()
        {
            return Ok();
        }
    }
}
