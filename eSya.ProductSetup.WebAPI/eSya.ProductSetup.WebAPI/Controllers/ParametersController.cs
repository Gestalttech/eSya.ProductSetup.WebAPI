using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        private readonly IParametersRepository _ParametersRepository;
        public ParametersController(IParametersRepository parametersRepository)
        {
            _ParametersRepository = parametersRepository;
        }

        #region Parameters
        /// <summary>
        /// Get Parameters Information by Parameter Type.
        /// UI Reffered - Parameters
        /// </summary>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetParametersInformationByParameterType(int parameterType)
        {
            var pa_rm = await _ParametersRepository.GetParametersInformationByParameterType(parameterType);
            return Ok(pa_rm);
        }

        /// <summary>
        /// Insert into Parameter .
        /// UI Reffered - Parameter
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoParameters(DO_Parameters obj)
        {
            var msg = await _ParametersRepository.InsertIntoParameters(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update Parameter .
        /// UI Reffered - Parameter
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateParameters(DO_Parameters obj)
        {
            var msg = await _ParametersRepository.UpdateParameters(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Get Parameter Header Information.
        /// UI Reffered - Parameters
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetParametersHeaderInformation()
        {
            var pa_rm = await _ParametersRepository.GetParametersHeaderInformation();
            return Ok(pa_rm);
        }

        /// <summary>
        /// Insert into Parameter Header.
        /// UI Reffered - Parameter
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoParameterHeader(DO_Parameters obj)
        {
            var msg = await _ParametersRepository.InsertIntoParameterHeader(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update Parameter Header .
        /// UI Reffered - Parameter
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateParameterHeader(DO_Parameters obj)
        {
            var msg = await _ParametersRepository.UpdateParameterHeader(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Parameter Header .
        /// UI Reffered - Parameter
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveParameterHeader(bool status, int parm_type)
        {
            var msg = await _ParametersRepository.ActiveOrDeActiveParameterHeader(status, parm_type);
            return Ok(msg);

        }
        #endregion

        #region Link Parameter with Schema

        /// <summary>
        /// Get Parameter Type for drop down.
        /// UI Reffered - Parameter Link Schema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveParameterTypes()
        {
            var ptyes = await _ParametersRepository.GetActiveParameterTypes();
            return Ok(ptyes);
        }

        /// <summary>
        /// Get Parameter Link Schema by Parameter Type.
        /// UI Reffered - Parameter Link Schema
        /// </summary>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetParameterLinkSchema(int parametertype)
        {
            var pa_schema = await _ParametersRepository.GetParameterLinkSchema(parametertype);
            return Ok(pa_schema);
        }

        /// <summary>
        /// Insert into Parameter Link Schema .
        /// UI Reffered - Parameter Link Schema
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLinkParameterSchema(List<DO_LinkParameterSchema> obj)
        {
            var msg = await _ParametersRepository.InsertOrUpdateLinkParameterSchema(obj);
            return Ok(msg);

        }
        #endregion
    }
}
