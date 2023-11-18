using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessCalendarController : ControllerBase
    {
        private readonly IBusinessCalendarRepository _businessCalendarRepository;

        public BusinessCalendarController(IBusinessCalendarRepository businessCalendarRepository)
        {
            _businessCalendarRepository = businessCalendarRepository;
        }
        #region Business Calendar
        /// <summary>
        /// Get Business Calendar
        /// UI Reffered - Business Calendar, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessCalendarBYBusinessKey(int businessKey)
        {
            var bcal = await _businessCalendarRepository.GetBusinessCalendarBYBusinessKey(businessKey);
            return Ok(bcal);
        }

        /// <summary>
        /// Insert into Business Calendar Table
        /// UI Reffered - Business Calendar,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinessCalendar(DO_BusinessCalendar obj)
        {
            var msg = await _businessCalendarRepository.InsertOrUpdateBusinessCalendar(obj);
            return Ok(msg);
        }


        #endregion
    }
}
