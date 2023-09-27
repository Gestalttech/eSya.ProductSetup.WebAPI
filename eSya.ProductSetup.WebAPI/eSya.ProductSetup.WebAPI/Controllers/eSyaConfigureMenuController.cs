using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class eSyaConfigureMenuController : ControllerBase
    {
        private readonly IConfigureMenuRepository _ConfigureMenuRepository;

        public eSyaConfigureMenuController(IConfigureMenuRepository ConfigureMenuRepository)
        {
            _ConfigureMenuRepository = ConfigureMenuRepository;
        }

        #region MainMenu
        [HttpGet]
        public async Task<IActionResult> GetMainMenuById(int mainMenuId)
        {
            var main_menus = await _ConfigureMenuRepository.GetMainMenuById(mainMenuId);
            return Ok(main_menus);
        }
        [HttpPost]

        public async Task<IActionResult> InsertIntoMainMenu(DO_MainMenu obj)
        {
            var msg = await _ConfigureMenuRepository.InsertIntoMainMenu(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateMainMenuIndex(int mainMenuId, bool isMoveUp, bool isMoveDown)
        {
            var msg = await _ConfigureMenuRepository.UpdateMainMenuIndex(mainMenuId, isMoveUp, isMoveDown);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteMainMenuByID(int mainMenuId)
        {
            var msg = await _ConfigureMenuRepository.DeleteMainMenu(mainMenuId);
            return Ok(msg);
        }
        #endregion

        #region SubMenu
        [HttpGet]
        public async Task<IActionResult> GetSubMenuById(int menuItemId)
        {
            var sub_menus = await _ConfigureMenuRepository.GetSubMenuById(menuItemId);
            return Ok(sub_menus);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoSubMenu(DO_SubMenu obj)
        {
            var msg = await _ConfigureMenuRepository.InsertIntoSubMenu(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateSubMenusIndex(int menuItemId, bool isMoveUp, bool isMoveDown)
        {
            var msg = await _ConfigureMenuRepository.UpdateSubMenusIndex(menuItemId, isMoveUp, isMoveDown);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSubMenuByID(int menuItemId)
        {
            var msg = await _ConfigureMenuRepository.DeleteSubMenu(menuItemId);
            return Ok(msg);
        }
        #endregion

        #region Forms
        [HttpGet]
        public async Task<IActionResult> GetFormDetailById(int mainMenuId, int menuItemId, int formId)
        {
            var form_details = await _ConfigureMenuRepository.GetFormDetailById(mainMenuId, menuItemId, formId);
            return Ok(form_details);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoFormMenu(DO_FormMenu obj)
        {
            var msg = await _ConfigureMenuRepository.InsertIntoFormMenu(obj);
            return Ok(msg);
        }
        [HttpGet]
        public IActionResult UpdateFormsIndex(int mainMenuId, int menuItemId, int formId, bool isMoveUp, bool isMoveDown)
        {
            var msg = _ConfigureMenuRepository.UpdateFormsIndex(mainMenuId, menuItemId, formId, isMoveUp, isMoveDown);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteFormMenuByID(int mainMenuId, int menuItemId, int formId)
        {
            var msg = await _ConfigureMenuRepository.DeleteFormMenu(mainMenuId, menuItemId, formId);
            return Ok(msg);
        }
        #endregion

        #region Configure Menu
        [HttpGet]
        public async Task<IActionResult> GetConfigureMenuMaster()
        {
            var config_menus = await _ConfigureMenuRepository.GetConfigureMenuMaster();
            return Ok(config_menus);
        }
        [HttpGet]
        public async Task<IActionResult> GetConfigureMenulist()
        {
            var config_Menu = await _ConfigureMenuRepository.GetConfigureMenulist();
            return Ok(config_Menu);
        }
        #endregion
    }
}
