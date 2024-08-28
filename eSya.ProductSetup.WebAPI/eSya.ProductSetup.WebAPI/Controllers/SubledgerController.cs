using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubledgerController : ControllerBase
    {
        private readonly ISubledgerRepository _subledgerRepository;
        public SubledgerController(ISubledgerRepository subledgerRepository)
        {
            _subledgerRepository = subledgerRepository;
        }
        #region Subledger Type
        /// <summary>
        /// Get Subledger Types.
        /// UI Reffered - Subledger
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubledgerTypes()
        {
            var ds = await _subledgerRepository.GetSubledgerTypes();
            return Ok(ds);
        }

        /// <summary>
        /// Insert into Subledger Type.
        /// UI Reffered - Subledger
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoSubledgerType(DO_Subledger obj)
        {
            var msg = await _subledgerRepository.InsertIntoSubledgerType(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update Subledger Type. .
        /// UI Reffered - Subledger
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSubledgerType(DO_Subledger obj)
        {
            var msg = await _subledgerRepository.UpdateSubledgerType(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Subledger Type .
        /// UI Reffered - Subledger
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveSubledgerType(bool status, string stype)
        {
            var msg = await _subledgerRepository.ActiveOrDeActiveSubledgerType(status, stype);
            return Ok(msg);

        }
        #endregion

        #region Subledger Group
        /// <summary>
        /// Get Subledger Group Information by Subledger Type.
        /// UI Reffered - Subledger
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubledgerGroupInformationBySubledgerType(string stype)
        {
            var ds = await _subledgerRepository.GetSubledgerGroupInformationBySubledgerType(stype);
            return Ok(ds);
        }

        /// <summary>
        /// Insert into Subledger Group .
        /// UI Reffered - Subledger
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoSubledgerGroup(DO_Subledger obj)
        {
            var msg = await _subledgerRepository.InsertIntoSubledgerGroup(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update Subledger Group .
        /// UI Reffered - Subledger
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSubledgerGroup(DO_Subledger obj)
        {
            var msg = await _subledgerRepository.UpdateSubledgerGroup(obj);
            return Ok(msg);

        }

        #endregion

        
    }
}
