using eSya.ProductSetup.DL.Repository;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        #region Notification Trigger Event
        /// <summary>
        /// Get SMS Trigger Event by Trigger event Id.
        /// UI Reffered - SMS Trigger Event
        /// </summary>
        /// <param name="TeventId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteSMSTriggerEvent(int TeventId)
        {
            var msg = await _notificationRepository.DeleteSMSTriggerEvent(TeventId);
            return Ok(msg);
        }

        /// <summary>
        /// Get SMS Trigger Event.
        /// UI Reffered - SMS Trigger Event
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSMSTriggerEvents()
        {
            var sms_tevents = await _notificationRepository.GetAllSMSTriggerEvents();
            return Ok(sms_tevents);
        }

        /// <summary>
        /// Insert into SMS Trigger Event .
        /// UI Reffered - SMS Trigger Event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoSMSTriggerEvent(DO_SMSTEvent obj)
        {
            var msg = await _notificationRepository.InsertIntoSMSTriggerEvent(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update SMS Trigger Event .
        /// UI Reffered - SMS Trigger Event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSMSTriggerEvent(DO_SMSTEvent obj)
        {
            var msg = await _notificationRepository.UpdateSMSTriggerEvent(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active SMS Trigger Event.
        /// UI Reffered - SMS Trigger Event
        /// </summary>
        /// <param name="status-smsvariable"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveSMSTriggerEvent(bool status, int TriggerEventId)
        {
            var msg = await _notificationRepository.ActiveOrDeActiveSMSTriggerEvent(status, TriggerEventId);
            return Ok(msg);
        }
        #endregion SMS Trigger Event

        #region Notification Variables
        /// <summary>
        /// Get SMS Variable Information.
        /// UI Reffered - SMS Variable
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSMSVariableInformation()
        {
            var sm_sv = await _notificationRepository.GetSMSVariableInformation();
            return Ok(sm_sv);
        }

        /// <summary>
        /// Get Active SMS Variable Information.
        /// UI Reffered - SMS Information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveSMSVariableInformation()
        {
            var sm_sv = await _notificationRepository.GetActiveSMSVariableInformation();
            return Ok(sm_sv);
        }


        /// <summary>
        /// Insert into SMS Variable .
        /// UI Reffered - SMS Variable
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoSMSVariable(DO_SMSVariable obj)
        {
            var msg = await _notificationRepository.InsertIntoSMSVariable(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update SMS Variable .
        /// UI Reffered - SMS Variable
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSMSVariable(DO_SMSVariable obj)
        {
            var msg = await _notificationRepository.UpdateSMSVariable(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active SMS Variable.
        /// UI Reffered - SMS Variable
        /// </summary>
        /// <param name="status-smsvariable"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveSMSVariable(bool status, string smsvariable)
        {
            var msg = await _notificationRepository.ActiveOrDeActiveSMSVariable(status, smsvariable);
            return Ok(msg);
        }
        #endregion
    }
}
