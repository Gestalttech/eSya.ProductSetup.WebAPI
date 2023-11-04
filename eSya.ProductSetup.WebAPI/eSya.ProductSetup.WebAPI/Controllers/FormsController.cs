using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FormsController : ControllerBase
    {
        private readonly IFormsRepository _FormsRepository;
        public FormsController(IFormsRepository FormsRepository)
        {
            _FormsRepository = FormsRepository;
        }

        #region Form Master
        [HttpGet]
        public async Task<IActionResult> GetAreaController()
        {
            var form_details = await _FormsRepository.GetAreaController();
            return Ok(form_details);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormDetails()
        {
            var form_details = await _FormsRepository.GetFormDetails();
            return Ok(form_details);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormDetailsByID(int formID)
        {
            var form_detail = await _FormsRepository.GetFormDetailsByID(formID);
            return Ok(form_detail);
        }
        [HttpGet]
        public async Task<IActionResult> GetInternalFormDetails()
        {
            var intform_details = await _FormsRepository.GetInternalFormDetails();
            return Ok(intform_details);
        }
        [HttpGet]
        public async Task<IActionResult> GetInternalFormByFormID(int formID)
        {
            var intform_detail = await _FormsRepository.GetInternalFormByFormID(formID);
            return Ok(intform_detail);
        }
        [HttpPost]
        public async Task<IActionResult> InsertUpdateIntoFormMaster(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertUpdateIntoFormMaster(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> GetNextInternalFormByID(int formID)
        {
            var intform_details = await _FormsRepository.GetNextInternalFormByID(formID);
            return Ok(intform_details);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoInternalForm(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoInternalForm(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormAction()
        {
            var form_actions = await _FormsRepository.GetFormAction();
            return Ok(form_actions);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormActionByID(int formID)
        {
            var form_action = await _FormsRepository.GetFormActionByID(formID);
            return Ok(form_action);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoFormAction(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoFormAction(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormParameterByID(int formID)
        {
            var form_action = await _FormsRepository.GetFormParameterByID(formID);
            return Ok(form_action);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoFormParameter(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoFormParameter(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormSubParameterByID(int formID, int parameterId)
        {
            var form_action = await _FormsRepository.GetFormSubParameterByID(formID, parameterId);
            return Ok(form_action);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoFormSubParameter(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoFormSubParameter(obj);
            return Ok(msg);
        }

        #endregion Form Master

        #region Area Controller
        [HttpGet]
        public async Task<IActionResult> GetAllAreaController()
        {
            var ar_ctrls = await _FormsRepository.GetAllAreaController();
            return Ok(ar_ctrls);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoAreaController(DO_AreaController obj)
        {
            var msg = await _FormsRepository.InsertIntoAreaController(obj);
            return Ok(msg);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAreaController(DO_AreaController obj)
        {
            var msg = await _FormsRepository.UpdateAreaController(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveAreaController(bool status, int Id)
        {
            var msg = await _FormsRepository.ActiveOrDeActiveAreaController(status, Id);
            return Ok(msg);
        }
        #endregion

        #region Define Actions 
        /// <summary>
        /// Get All Actions
        /// UI Reffered - Actions,
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAllActions()
        {
            var actions = await _FormsRepository.GetAllActions();
            return Ok(actions);
        }
        /// <summary>
        /// Insert into Actions Table
        /// UI Reffered - Actions,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoActions(DO_Actions obj)
        {
            var msg = await _FormsRepository.InsertIntoActions(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Actions Table
        /// UI Reffered - Actions,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateActions(DO_Actions obj)
        {
            var msg = await _FormsRepository.UpdateActions(obj);
            return Ok(msg);
        }
        /// <summary>
        /// Active Or De Active Code Types.
        /// UI Reffered - Code Types
        /// </summary>
        /// <param name="status-code_type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveActions(bool status, int actionId)
        {
            var ac = await _FormsRepository.ActiveOrDeActiveActions(status, actionId);
            return Ok(ac);
        }
        #endregion
    }
}
