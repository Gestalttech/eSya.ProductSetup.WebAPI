using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MobileCarrierController : ControllerBase
    {
        private readonly IMobileCarrierRepository _mobileCarrierRepository;

        public MobileCarrierController(IMobileCarrierRepository mobileCarrierRepository)
        {
            _mobileCarrierRepository = mobileCarrierRepository;
        }

        #region Mobile Carrier
        /// <summary>
        /// Get Mobile Carrier.
        /// UI Reffered - Mobile Carrier, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMobileCarriers()
        {
            var mcaris = await _mobileCarrierRepository.GetMobileCarriers();
            return Ok(mcaris);
        }

        /// <summary>
        /// Insert into Or Update Mobile Carrier Table
        /// UI Reffered - Mobile Carrier,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateMobileCarrier(DO_MobileCarrier obj)
        {
            var msg = await _mobileCarrierRepository.InsertOrUpdateMobileCarrier(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get Active or D-Active Mobile Carrier.
        /// UI Reffered - Mobile Carrier, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveMobileCarrier(bool status, int ISDCode, string MobilePrefix)
        {
            var msg = await _mobileCarrierRepository.ActiveOrDeActiveMobileCarrier(status, ISDCode, MobilePrefix);
            return Ok(msg);
        }
        #endregion
    }
}
