using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly IConnectRepository _connectRepository;

        public ConnectController(IConnectRepository connectRepository)
        {
            _connectRepository = connectRepository;
        }

        #region SMS Connect
        /// <summary>
        /// Getting Locations by Business ID.
        /// UI Reffered - SMS Connect Dropdown
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocationByBusinessID(int BusinessId)
        {
            var locs = await _connectRepository.GetBusinessLocationByBusinessID(BusinessId);
            return Ok(locs);
        }
        /// <summary>
        /// Getting Active Entities.
        /// UI Reffered - SMS Connect Dropdown
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveEntites()
        {
            var entities = await _connectRepository.GetActiveEntites();
            return Ok(entities);
        }
        /// <summary>
        /// Getting ISD Code by BusineeKey.
        /// UI Reffered - SMS Connect display ISD Code in to Label
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLocationISDCodeByBusinessKey(int BusinessKey)
        {
            var isd = await _connectRepository.GetLocationISDCodeByBusinessKey(BusinessKey);
            return Ok(isd);
        }
        /// <summary>
        /// Getting SMS Connect by Business ID.
        /// UI Reffered - SMS Connect Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSMSConnectbyBusinessID(int BusinessId)
        {
            var sms = await _connectRepository.GetSMSConnectbyBusinessID(BusinessId);
            return Ok(sms);
        }
        /// <summary>
        /// Insert Insert Or Update into SMS Connect .
        /// UI Reffered -SMS Connect
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateSMSConnect(DO_SMSConnect obj)
        {
            var msg = await _connectRepository.InsertOrUpdateSMSConnect(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De SMS Connect.
        /// UI Reffered - SMS Connect
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveSMSConnect(DO_SMSConnect obj)
        {
            var res = await _connectRepository.ActiveOrDeActiveSMSConnect( obj);
            return Ok(res);
        }
        #endregion

        #region Email Connect
        /// <summary>
        /// Getting Email Connect by Business ID.
        /// UI Reffered - Email Connect Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmailConnectbyBusinessID(int BusinessId)
        {
            var emails = await _connectRepository.GetEmailConnectbyBusinessID(BusinessId);
            return Ok(emails);
        }
        /// <summary>
        /// Insert Insert Or Update into Email Connect .
        /// UI Reffered -Email Connect
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateEmailConnect(DO_EmailConnect obj)
        {
            var msg = await _connectRepository.InsertOrUpdateEmailConnect(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De Email Connect.
        /// UI Reffered - Email Connect
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveEmailConnect(DO_EmailConnect obj)
        {
            var res = await _connectRepository.ActiveOrDeActiveEmailConnect(obj);
            return Ok(res);
        }
        #endregion
    }
}
