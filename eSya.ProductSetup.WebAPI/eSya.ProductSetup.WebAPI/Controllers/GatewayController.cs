using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayRepository _gatewayRepository;
        public GatewayController(IGatewayRepository gatewayRepository)
        {
            _gatewayRepository = gatewayRepository;
        }
        #region Gate way Rules
        /// <summary>
        /// Get Gate way Rules
        /// UI Reffered - Gate way Rules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGatewayRules()
        {
            var grules = await _gatewayRepository.GetGatewayRules();
            return Ok(grules);
        }
        /// <summary>
        /// Insert Or Update into Gate way Rules Table
        /// UI Reffered - Gate way Rules,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateGatewayRules(DO_GatewayRules obj)
        {
            var rp = await _gatewayRepository.InsertOrUpdateGatewayRules(obj);
            return Ok(rp);
        }
        /// <summary>
        /// Activate Or De Activate Gate way Rules
        /// UI Reffered - Gate way Rules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveGatewayRules(bool status, int GwRuleId)
        {
            var proc_masters = await _gatewayRepository.ActiveOrDeActiveGatewayRules(status, GwRuleId);
            return Ok(proc_masters);
        }
        #endregion
    }
}
