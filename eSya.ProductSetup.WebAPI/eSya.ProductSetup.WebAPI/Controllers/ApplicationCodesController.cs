using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicationCodesController : ControllerBase
    {
        private readonly IApplicationCodesRepository _ApplicationCodesRepository;

        public ApplicationCodesController(IApplicationCodesRepository applicationCodesRepository)
        {
            _ApplicationCodesRepository = applicationCodesRepository;
        }

        #region Code Types
        /// <summary>
        /// Get Code Type.
        /// UI Reffered - CodeType, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCodeTypes()
        {
            var ct = await _ApplicationCodesRepository.GetCodeTypes();
            return Ok(ct);
        }

        /// <summary>
        /// Insert into Code Type Table
        /// UI Reffered - CodeType,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoCodeType(DO_CodeTypes obj)
        {
            var msg = await _ApplicationCodesRepository.InsertIntoCodeType(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Code Type Table
        /// UI Reffered - CodeType,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCodeType(DO_CodeTypes obj)
        {
            var msg = await _ApplicationCodesRepository.UpdateCodeType(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get Code Type List.
        /// UI Reffered - Code Types dropdwon in Application Codes, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveCodeTypes()
        {
            var act = await _ApplicationCodesRepository.GetActiveCodeTypes();
            return Ok(act);
        }

        /// <summary>
        /// Get System Defined Code Type List.
        /// UI Reffered - Application code, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSystemDefinedCodeTypesList()
        {
            var uct = await _ApplicationCodesRepository.GetSystemDefinedCodeTypesList();
            return Ok(uct);
        }

        /// <summary>
        /// Get User Defined Code Type List.
        /// UI Reffered - CodeType, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserDefinedCodeTypesList()
        {
            var uct = await _ApplicationCodesRepository.GetUserDefinedCodeTypesList();
            return Ok(uct);
        }

        /// <summary>
        /// Active Or De Active Code Types.
        /// UI Reffered - Code Types
        /// </summary>
        /// <param name="status-code_type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveCodeTypes(bool status, int code_type)
        {
            var ac = await _ApplicationCodesRepository.ActiveOrDeActiveCodeTypes(status, code_type);
            return Ok(ac);
        }
        #endregion

        #region Application Codes
        /// <summary>
        /// Get Application Codes.
        /// UI Reffered - ApplicationCodes, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesAsync()
        {
            var ac = await _ApplicationCodesRepository.GetApplicationCodes();
            return Ok(ac);
        }

        /// <summary>
        /// Get Application Codes for specific Code Type.
        /// UI Reffered - ApplicationCodes,AssetGroup
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codeType)
        {
            var ac = await _ApplicationCodesRepository.GetApplicationCodesByCodeType(codeType);
            return Ok(ac);
        }

        /// <summary>
        /// Insert into Application Codes Table
        /// UI Reffered - ApplicationCode,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoApplicationCodes(DO_ApplicationCodes obj)
        {
            var msg = await _ApplicationCodesRepository.InsertIntoApplicationCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Application Codes Table
        /// UI Reffered - ApplicationCode,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateApplicationCodes(DO_ApplicationCodes obj)
        {
            var msg = await _ApplicationCodesRepository.UpdateApplicationCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Application code.
        /// UI Reffered - Application Code
        /// </summary>
        /// <param name="status-app_code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveApplicationCode(bool status, int app_code)
        {
            var ac = await _ApplicationCodesRepository.ActiveOrDeActiveApplicationCode(status, app_code);
            return Ok(ac);
        }
        #endregion

      
    }
}
