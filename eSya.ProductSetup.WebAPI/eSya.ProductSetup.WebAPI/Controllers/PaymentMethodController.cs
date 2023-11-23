using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }
        /// <summary>
        /// Get Payment methods.
        /// UI Reffered - Payment methods, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodbyISDCode(int codetype, int ISDCode)
        {
            var cities = await _paymentMethodRepository.GetPaymentMethodbyISDCode(codetype, ISDCode);
            return Ok(cities);
        }

        /// <summary>
        /// Insert into Payment methods Table
        /// UI Reffered - Payment methods,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePaymentMethod(List<DO_PaymentMethod> obj)
        {
            var msg = await _paymentMethodRepository.InsertOrUpdatePaymentMethod(obj);
            return Ok(msg);
        }
    }
}
