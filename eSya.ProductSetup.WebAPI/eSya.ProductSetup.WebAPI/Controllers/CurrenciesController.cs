using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyMasterRepository _CurrencyMasterRepository;

        public CurrenciesController(ICurrencyMasterRepository CurrencyMasterRepository)
        {
            _CurrencyMasterRepository = CurrencyMasterRepository;
        }

        #region Currency Master
        /// <summary>
        /// Get Currency Master.
        /// UI Reffered - Currency Master
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrencyMaster()
        {
            var currencies = await _CurrencyMasterRepository.GetCurrencyMaster();
            return Ok(currencies);
        }

        /// <summary>
        /// Get Currency List search by Currency Prefix.
        /// UI Reffered - Currency Master
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrencyListByCurrenyPrefix(string prefix)
        {
            var currencies = await _CurrencyMasterRepository.GetCurrencyListByCurrenyPrefix(prefix);
            return Ok(currencies);
        }

        /// <summary>
        /// Insert into Currency Master Table
        /// UI Reffered - Currency Master
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoCurrencyMaster(DO_CurrencyMaster obj)
        {
            var msg = await _CurrencyMasterRepository.InsertIntoCurrencyMaster(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update Currency Master Table
        /// UI Reffered - Currency Master
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCurrencyMaster(DO_CurrencyMaster obj)
        {
            var msg = await _CurrencyMasterRepository.UpdateCurrencyMaster(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Delete Currency Master Table
        /// UI Reffered - Currency Master
        /// <param name="prefix"></param>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteCurrencyMasterByCurrencyCode(string currencyCode)
        {
            var msg = await _CurrencyMasterRepository.DeleteCurrencyMasterByCurrencyCode(currencyCode);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Currency Master.
        /// UI Reffered - Currency Master
        /// </summary>
        /// <param name="status-currency_code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveCurrencyMaster(bool status, string currency_code)
        {
            var ac = await _CurrencyMasterRepository.ActiveOrDeActiveCurrencyMaster(status, currency_code);
            return Ok(ac);
        }
        #endregion

        #region Currency Denomination
        /// <summary>
        /// Get Currency Denomination Information by Currency Code.
        /// UI Reffered - Currency Master
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrencyDenominationInfoByCurrencyCode(string currencyCode)
        {
            var curr_denominations = await _CurrencyMasterRepository.GetCurrencyDenominationInfoByCurrencyCode(currencyCode);
            return Ok(curr_denominations);
        }

        /// <summary>
        /// Insert  into Currency information Table
        /// UI Reffered - Currency Denomination
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertCurrencyDenominationInformation(DO_CurrencyDenominationInformation obj)
        {
            var msg = await _CurrencyMasterRepository.InsertCurrencyDenominationInformation(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Currency information Table
        /// UI Reffered - Currency Denomination
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCurrencyDenominationInformation(DO_CurrencyDenominationInformation obj)
        {
            var msg = await _CurrencyMasterRepository.UpdateCurrencyDenominationInformation(obj);
            return Ok(msg);
        }
        /// <summary>
        /// Delete Currency information by Currency code and Denomination
        /// UI Reffered - Currency Master
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteCurrencyDenominationInformation(string currencyCode, decimal DenomId, string BnorCNId)
        {
            var msg = await _CurrencyMasterRepository.DeleteCurrencyDenominationInformation(currencyCode, DenomId, BnorCNId);
            return Ok(msg);
        }

        /// <summary>
        /// Get Currency List.
        /// UI Reffered - Country Master
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveCurrencyList()
        {
            var currencies = await _CurrencyMasterRepository.GetActiveCurrencyList();
            return Ok(currencies);
        }


        /// <summary>
        /// Active Or De Active Currency Denomination.
        /// UI Reffered - Currency Denomination
        /// </summary>
        /// <param name="status-currency_code-denominationId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveCurrencyDenomination(bool status, string currencycode, int denomId)
        {
            var msg = await _CurrencyMasterRepository.ActiveOrDeActiveCurrencyDenomination(status, currencycode, denomId);
            return Ok(msg);
        }
        #endregion
    }
}
