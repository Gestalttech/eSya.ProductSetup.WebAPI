using eSya.ProductSetup.DL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigMasterDataController : ControllerBase
    {
        #region Common Methods
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codeType)
        {
            var ds = await new CommonMethod().GetApplicationCodesByCodeType(codeType);
            return Ok(ds);
        }

        [HttpPost]
        public async Task<IActionResult> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            var ds = await new CommonMethod().GetApplicationCodesByCodeTypeList(l_codeType);
            return Ok(ds);
        }

       

        /// <summary>
        /// Get ISDCodes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetISDCodes()
        {
            var ds = await new CommonMethod().GetISDCodes();
            return Ok(ds);
        }


        /// <summary>
        /// Get Form Detail.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFormDetails()
        {
            var ds = await new CommonMethod().GetFormDetails();
            return Ok(ds);
        }

        /// <summary>
        /// Get ISDCodes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetIndiaISDCodes()
        {
            var ds = await new CommonMethod().GetIndiaISDCodes();
            return Ok(ds);
        }

        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaxCodesListByISDCode(int Isdcode)
        {
            var ds = await new CommonMethod().GetTaxCodesListByISDCode(Isdcode);
            return Ok(ds);
        }


        /// <summary>
        /// Get Active Currencies for dropdown.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveCurrencyCodes()
        {
            var ds = await new CommonMethod().GetActiveCurrencyCodes();
            return Ok(ds);
        }
        #endregion
    }
}
