using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        public PaymentsController()
        {

        }

       

        [HttpGet]
        [Route("GetActivePayment")]
        public IActionResult GetActivePayment()
        {
            return Ok();
        }
    }
}
