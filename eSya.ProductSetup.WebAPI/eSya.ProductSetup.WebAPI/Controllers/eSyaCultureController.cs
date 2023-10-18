using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class eSyaCultureController : ControllerBase
    {
        private readonly IeSyaCultureRepository _eSyaCultureRepository;
        public eSyaCultureController(IeSyaCultureRepository eSyaCultureRepository)
        {
            _eSyaCultureRepository = eSyaCultureRepository;
        }
        #region define eSya Culture 
        [HttpGet]
        public async Task<IActionResult> GetAlleSyaCultures()
        {
            var esya_ctrls = await _eSyaCultureRepository.GetAlleSyaCultures();
            return Ok(esya_ctrls);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateIntoeSyaCultures(DO_eSyaCulture obj)
        {
            var msg = await _eSyaCultureRepository.InsertOrUpdateIntoeSyaCultures(obj);
            return Ok(msg);
        }
        
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveeSyaCultures(bool status, string esyaculture)
        {
            var msg = await _eSyaCultureRepository.ActiveOrDeActiveeSyaCultures(status, esyaculture);
            return Ok(msg);
        }
        #endregion
    }
}
