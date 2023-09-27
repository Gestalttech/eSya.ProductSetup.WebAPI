using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _AddressRepository;

        public AddressController(IAddressRepository AddressRepository)
        {
            _AddressRepository = AddressRepository;
        }

        #region States
        /// <summary>
        /// Get States.
        /// UI Reffered - States, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStatesbyISDCode(int isdCode)
        {
            var states = await _AddressRepository.GetStatesbyISDCode(isdCode);
            return Ok(states);
        }

        /// <summary>
        /// Insert into States Table
        /// UI Reffered - States,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateIntoStates(DO_States obj)
        {
            var msg = await _AddressRepository.InsertOrUpdateIntoStates(obj);
            return Ok(msg);
        }


        #endregion

        #region Cities
        /// <summary>
        /// Get Cities.
        /// UI Reffered - Cities, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCitiesbyStateCode(int isdCode, int stateCode)
        {
            var cities = await _AddressRepository.GetCitiesbyStateCode(isdCode, stateCode);
            return Ok(cities);
        }

        /// <summary>
        /// Insert into Cities Table
        /// UI Reffered - Cities,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateIntoCities(DO_Cities obj)
        {
            var msg = await _AddressRepository.InsertOrUpdateIntoCities(obj);
            return Ok(msg);
        }


        #endregion

        #region  #region Common Methods
        /// <summary>
        /// Get Active States for drop down.
        /// UI Reffered - States & Cities & Area, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveStatesbyISDCode(int isdCode)
        {
            var states = await _AddressRepository.GetActiveStatesbyISDCode(isdCode);
            return Ok(states);
        }
        /// <summary>
        /// Get Active Cities for drop down.
        /// UI Reffered - States & Cities & Area, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveCitiesbyStateCode(int isdCode, int stateCode)
        {
            var cities = await _AddressRepository.GetActiveCitiesbyStateCode(isdCode, stateCode);
            return Ok(cities);
        }

        #endregion
    }
}
