using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ControlController : ControllerBase
    {
        private readonly IDocumentControlRepository _DocumentControlRepository;

        public ControlController(IDocumentControlRepository DocumentControlRepository)
        {
            _DocumentControlRepository = DocumentControlRepository;
        }

        //#region Calendar Header

        ///// <summary>
        ///// Getting Calendar Headers by BusineeKey.
        ///// UI Reffered - Calendar Header Grid
        ///// </summary>
        //[HttpGet]
        //public async Task<IActionResult> GetCalendarHeadersbyBusinessKey(int Businesskey)
        //{
        //    var cal_headers = await _DocumentControlRepository.GetCalendarHeadersbyBusinessKey(Businesskey);
        //    return Ok(cal_headers);
        //}

        ///// <summary>
        ///// Getting Calendar Header.
        ///// UI Reffered - Calendar Header Grid
        ///// </summary>
        //[HttpGet]
        //public async Task<IActionResult> GetCalendarHeaders()
        //{
        //    var cal_headers = await _DocumentControlRepository.GetCalendarHeaders();
        //    return Ok(cal_headers);
        //}

        ///// <summary>
        ///// Insert Calendar Header & Details Table .
        ///// UI Reffered -Calendar Header
        ///// </summary>
        //[HttpPost]
        //public async Task<IActionResult> InsertCalendarHeaderAndDetails(DO_CalendarDefinition calendarheadar)
        //{
        //    var msg = await _DocumentControlRepository.InsertCalendarHeaderAndDetails(calendarheadar);
        //    return Ok(msg);

        //}

        //#endregion Calendar Header

        #region Calendar Header
        /// <summary>
        /// Getting Calendar Header.
        /// UI Reffered - Calendar Header Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCalendarHeaders()
        {
            var cal_headers = await _DocumentControlRepository.GetCalendarHeaders();
            return Ok(cal_headers);
        }

        /// <summary>
        /// Insert Calendar Header  Table .
        /// UI Reffered -Calendar Header
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertCalendarHeader(DO_CalendarHeader obj)
        {
            var msg = await _DocumentControlRepository.InsertCalendarHeader(obj);
            return Ok(msg);

        }
        #endregion

        #region Calendar Details
        /// <summary>
        /// Insert Calendar Details  Table .
        /// UI Reffered -Calendar Details
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertCalendarDetails(DO_CalendarHeader obj)
        {
            var msg = await _DocumentControlRepository.InsertCalendarDetails(obj);
            return Ok(msg);

        }
        #endregion

        #region Document Master

        /// <summary>
        /// Getting Document Control List.
        /// UI Reffered - Document Control Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentControlMaster()
        {
            var ds = await _DocumentControlRepository.GetDocumentControlMaster();
            return Ok(ds);
        }

        /// <summary>
        /// Getting Document Parameters List.
        /// UI Reffered - Document Control
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentParametersByID(int documentID)
        {
            var ds = await _DocumentControlRepository.GetDocumentParametersByID(documentID);
            return Ok(ds);
        }

        /// <summary>
        /// Insert or Update Document Control .
        /// UI Reffered -Document Control
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateDocumentControl(DO_DocumentControlMaster obj)
        {
            var msg = await _DocumentControlRepository.AddOrUpdateDocumentControl(obj);
            return Ok(msg);

        }
        #endregion

        #region Form Document Link
        /// <summary>
        /// Getting Forms (IsDocumentControl) List.
        /// UI Reffered - Document Control -> Forms Tree
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFormsForDocumentControl()
        {
            var ds = await _DocumentControlRepository.GetFormsForDocumentControl();
            return Ok(ds);
        }

        /// <summary>
        /// Getting Forms-Document Link .
        /// UI Reffered - Document Control -> Documents Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFormDocumentlink(int formID)
        {
            var ds = await _DocumentControlRepository.GetFormDocumentlink(formID);
            return Ok(ds);
        }

        /// <summary>
        /// Update Form-Document Links .
        /// UI Reffered - Document Control
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateFormDocumentLinks(List<DO_FormDocumentLink> obj)
        {
            var msg = await _DocumentControlRepository.UpdateFormDocumentLinks(obj);
            return Ok(msg);

        }
        #endregion
    }
}
