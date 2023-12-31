﻿using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IConfigureMenuRepository
    {

        #region MainMenu
        Task<DO_MainMenu> GetMainMenuById(int mainMenuId);

        Task<DO_ReturnParameter> InsertIntoMainMenu(DO_MainMenu obj);

        Task<DO_ReturnParameter> UpdateMainMenuIndex(int mainMenuId, bool isMoveUp, bool isMoveDown);

        Task<DO_ReturnParameter> DeleteMainMenu(int mainMenuId);
        #endregion MainMenu

        #region SubMenu
        Task<DO_SubMenu> GetSubMenuById(int menuItemId);

        Task<DO_ReturnParameter> InsertIntoSubMenu(DO_SubMenu obj);

        Task<DO_ReturnParameter> UpdateSubMenusIndex(int menuItemId, bool isMoveUp, bool isMoveDown);

        Task<DO_ReturnParameter> DeleteSubMenu(int menuItemId);

        #endregion SubMenu

        #region Forms

        Task<DO_FormMenu> GetFormDetailById(int mainMenuId, int menuItemId, int formId);


        Task<DO_ReturnParameter> InsertIntoFormMenu(DO_FormMenu obj);

        DO_ReturnParameter UpdateFormsIndex(int mainMenuId, int menuItemId, int formID, bool isMoveUp, bool isMoveDown);


        Task<DO_ReturnParameter> DeleteFormMenu(int mainMenuId, int menuItemId, int formId);

        #endregion Forms

        #region Configure Menu
        Task<DO_ConfigureMenu> GetConfigureMenuMaster();


        Task<List<DO_MainMenu>> GetConfigureMenulist();
        #endregion
    }
}
