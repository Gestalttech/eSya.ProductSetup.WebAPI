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


        #region Calendar Patient ID Generation
        /// <summary>
        /// Getting CalenderKey by BusinessKey.
        /// UI Reffered - Calendar Calendar Key for drop down
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCalenderKeybyBusinessKey(int Businesskey)
        {
            var calkeys = await _DocumentControlRepository.GetCalenderKeybyBusinessKey(Businesskey);
            return Ok(calkeys);
        }

        /// <summary>
        /// Getting Patient Generation by BusinessKey.
        /// UI Reffered - Calendar Patient Generation for Grid
        /// </summary>
        [HttpGet]
        public IActionResult GetCalendarPatientGenerationbyBusinessKeyAndCalenderKey(int BusinessKey, string CalenderKey)
        {
            var pa_gens =  _DocumentControlRepository.GetCalendarPatientGenerationbyBusinessKeyAndCalenderKey(BusinessKey, CalenderKey);
            return Ok(pa_gens);
        }

        /// <summary>
        /// Update Calendar Patient Generation .
        /// UI Reffered -Calendar Patient Generation
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCalendarGeneration(DO_CalendarPatientIdGeneration obj)
        {
            var msg = await _DocumentControlRepository.UpdateCalendarGeneration(obj);
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

        #region Document Link with Form
        /// <summary>
        /// Docuemt  List.
        /// UI Reffered - Document Control -> Docuemnt Tree
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveDocumentControls()
        {
            var ds = await _DocumentControlRepository.GetActiveDocumentControls();
            return Ok(ds);
        }

        /// <summary>
        /// Getting Getting Forms (IsDocumentControl=true) .
        /// UI Reffered - Document Control -> Form Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentFormlink(int documentID)
        {
            var ds = await _DocumentControlRepository.GetDocumentFormlink(documentID);
            return Ok(ds);
        }

        /// <summary>
        /// Update Document-Form Links .
        /// UI Reffered - Document Control
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentFormlink(List<DO_FormDocumentLink> obj)
        {
            var msg = await _DocumentControlRepository.UpdateDocumentFormlink(obj);
            return Ok(msg);

        }
        #endregion
    }
}
