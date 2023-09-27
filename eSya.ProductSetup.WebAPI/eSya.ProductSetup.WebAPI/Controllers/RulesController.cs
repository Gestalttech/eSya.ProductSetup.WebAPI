using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IProcessMasterRepository _ProcessMasterRepository;
        public RulesController(IProcessMasterRepository ProcessMasterRepository)
        {
            _ProcessMasterRepository = ProcessMasterRepository;
        }

        #region Application Rules
        /// <summary>
        /// Get Process Master.
        /// UI Reffered - Process Control
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProcessMaster()
        {
            var proc_masters = await _ProcessMasterRepository.GetProcessMaster();
            return Ok(proc_masters);
        }

        /// <summary>
        /// Insert into Process Master Table
        /// UI Reffered - Process Control,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoProcessMaster(DO_ProcessMaster obj)
        {
            var rp = await _ProcessMasterRepository.InsertIntoProcessMaster(obj);
            return Ok(rp);
        }

        /// <summary>
        /// Update into Process Master Table
        /// UI Reffered - Process Control,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateProcessMaster(DO_ProcessMaster obj)
        {
            var rp = await _ProcessMasterRepository.UpdateProcessMaster(obj);
            return Ok(rp);
        }


        /// <summary>
        /// Get Process Rule 
        /// UI Reffered - Application Rule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProcessRule()
        {
            var proc_rules = await _ProcessMasterRepository.GetProcessRule();
            return Ok(proc_rules);
        }

        /// <summary>
        /// Get Process Master List for Combo.
        /// UI Reffered - Application Rule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveProcessMaster()
        {
            var aproc_rules = await _ProcessMasterRepository.GetActiveProcessMaster();
            return Ok(aproc_rules);
        }

        /// <summary>
        /// Get Process Rules for specific Process Id.
        /// UI Reffered - Appplication Rule
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProcessRuleByProcessId(int processId)
        {
            var proc_rule = await _ProcessMasterRepository.GetProcessRuleByProcessId(processId);
            return Ok(proc_rule);
        }

        /// <summary>
        /// Insert into Process Rule Table
        /// UI Reffered - Application Rule,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoProcessRule(DO_ProcessRule obj)
        {
            var rp = await _ProcessMasterRepository.InsertIntoProcessRule(obj);
            return Ok(rp);
        }

        /// <summary>
        /// Update Process Rule Table
        /// UI Reffered - Application Rule,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateProcessRule(DO_ProcessRule obj)
        {
            var rp = await _ProcessMasterRepository.UpdateProcessRule(obj);
            return Ok(rp);
        }
        #endregion Application Rules

        #region Application Rule-Business Location Based
        /// <summary>
        /// Get Process Rule by Segment wise.
        /// UI Reffered - Process Rule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProcessRulebySegmentwise()
        {
            var aproc_rules = await _ProcessMasterRepository.GetProcessRulebySegmentwise();
            return Ok(aproc_rules);
        }

        /// <summary>
        /// Get Process Rule by Segment wise.
        /// UI Reffered - Process Rule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProcessRulebyBusinessKey(int BusinessKey)
        {
            var pro_rules = await _ProcessMasterRepository.GetProcessRulebyBusinessKey(BusinessKey);
            return Ok(pro_rules);
        }

        /// <summary>
        /// Insert or Update into Application Rule by Segment wise
        /// UI Reffered - Process Rule
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertorUpdateProcessRulebySegment(DO_ProcessRulebySegment obj)
        {
            var rp = await _ProcessMasterRepository.InsertorUpdateProcessRulebySegment(obj);
            return Ok(rp);
        }
        #endregion
    }
}
