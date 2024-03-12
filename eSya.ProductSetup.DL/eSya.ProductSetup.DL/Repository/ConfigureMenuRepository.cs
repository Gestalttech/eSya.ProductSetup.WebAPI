using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DL.Repository
{
    public class ConfigureMenuRepository: IConfigureMenuRepository
    {
        private readonly IStringLocalizer<ConfigureMenuRepository> _localizer;
        public ConfigureMenuRepository(IStringLocalizer<ConfigureMenuRepository> localizer)
        {
            _localizer = localizer;
        }
       
        #region MainMenu
        public async Task<DO_MainMenu> GetMainMenuById(int mainMenuId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcmamns
                                  .Where(w => w.MainMenuId == mainMenuId)
                                  .Select(r => new DO_MainMenu
                                  {
                                      MainMenuId = r.MainMenuId,
                                      MainMenu = r.MainMenu,
                                      ImageURL = r.ImageUrl,
                                      MenuIndex = r.MenuIndex,
                                      ActiveStatus = r.ActiveStatus
                                  }).FirstOrDefaultAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertIntoMainMenu(DO_MainMenu obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (obj.MainMenuId <= 0)
                        {
                            if (db.GtEcmamns.Where(w => w.MainMenu == obj.MainMenu).Count() > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0032", Message = string.Format(_localizer[name: "W0032"]) };
                            }

                            var mainMenuId = db.GtEcmamns.Select(w => w.MainMenuId).DefaultIfEmpty().Max();
                            mainMenuId += 1;
                            var index = db.GtEcmamns.Select(w => w.MenuIndex).DefaultIfEmpty().Max();
                            index += 1;
                            var mn = new GtEcmamn
                            {
                                MainMenuId = mainMenuId,
                                MainMenu = obj.MainMenu,
                                ImageUrl = obj.ImageURL,
                                MenuIndex = index,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalId,
                                CreatedBy = obj.UserId,

                            };
                            db.GtEcmamns.Add(mn);
                        }
                        else
                        {
                            var mn = db.GtEcmamns.Where(w => w.MainMenuId == obj.MainMenuId).FirstOrDefault();
                            if (mn != null)
                            {
                                mn.MainMenu = obj.MainMenu;
                                mn.ImageUrl = obj.ImageURL;
                                mn.ActiveStatus = obj.ActiveStatus;
                                mn.ModifiedOn = DateTime.Now;
                                mn.ModifiedTerminal = obj.TerminalId;
                                mn.ModifiedBy = obj.UserId;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateMainMenuIndex(int mainMenuId, bool isMoveUp, bool isMoveDown)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var mn = db.GtEcmamns.Where(W => W.MainMenuId == mainMenuId).FirstOrDefault();

                        List<GtEcmamn> list = db.GtEcmamns.ToList();
                        var l_first = list.Where(W => W.MainMenuId != mainMenuId && W.MenuIndex <= mn.MenuIndex).OrderBy(o => o.MenuIndex);
                        var l_last = list.Where(W => W.MainMenuId != mainMenuId && W.MenuIndex > mn.MenuIndex).OrderBy(o => o.MenuIndex);

                        int runningIndex = 0;
                        if (isMoveUp)
                        {
                            foreach (var m in l_first)
                            {
                                if (l_first.ToList().IndexOf(m) != l_first.Count() - 1)
                                {
                                    runningIndex = runningIndex + 1;
                                    m.MenuIndex = (short)runningIndex;
                                }
                                else
                                {
                                    runningIndex = runningIndex + 1;
                                    mn.MenuIndex = (short)runningIndex;

                                    runningIndex = runningIndex + 1;
                                    m.MenuIndex = (short)runningIndex;
                                }
                            }

                            foreach (var m in l_last.Skip(1))
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }
                        }
                        else if (isMoveDown)
                        {
                            foreach (var m in l_first)
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }

                            foreach (var m in l_last.Take(1))
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }

                            runningIndex = runningIndex + 1;
                            mn.MenuIndex = (short)runningIndex;

                            foreach (var m in l_last.Skip(1))
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> DeleteMainMenu(int mainMenuId)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (db.GtEcsbmns.Where(w => w.MainMenuId == mainMenuId).Count() > 0)
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0033", Message = string.Format(_localizer[name: "W0033"]) };
                        var del = db.GtEcmamns.Where(w => w.MainMenuId == mainMenuId).FirstOrDefault();
                        if (del != null)
                        {
                            db.GtEcmamns.Remove(del);

                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0034", Message = string.Format(_localizer[name: "W0034"]) };
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion MainMenu

        #region SubMenu
        public async Task<DO_SubMenu> GetSubMenuById(int menuItemId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcsbmns
                                  .Where(w => w.MenuItemId == menuItemId)
                                  .Select(r => new DO_SubMenu
                                  {
                                      MainMenuId = r.MainMenuId,
                                      MenuItemId = r.MenuItemId,
                                      MenuItemName = r.MenuItemName,
                                      MenuIndex = r.MenuIndex,
                                      ActiveStatus = r.ActiveStatus
                                  }).FirstOrDefaultAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> InsertIntoSubMenu(DO_SubMenu obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (obj.MenuItemId <= 0)
                        {
                            if (obj.MainMenuId == 0)
                            {
                                var mm = db.GtEcsbmns.Where(w => w.MenuItemId == obj.ParentID).FirstOrDefault();
                                obj.MainMenuId = mm.MainMenuId;
                            }
                            if (db.GtEcsbmns.Where(w => w.MainMenuId == obj.MainMenuId && w.ParentId == obj.ParentID && w.MenuItemName == obj.MenuItemName).Count() > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0035", Message = string.Format(_localizer[name: "W0035"]) };
                            }

                            var subMenuID = db.GtEcsbmns.Select(w => w.MenuItemId).DefaultIfEmpty().Max();
                            subMenuID += 1;
                            var index = db.GtEcsbmns.Where(w => w.MainMenuId == obj.MainMenuId && w.ParentId == obj.ParentID)
                                .Select(w => w.MenuIndex).DefaultIfEmpty().Max();

                            var fm_index = db.GtEcmnfls.Where(w => w.MainMenuId == obj.MainMenuId && w.MenuItemId == obj.ParentID)
                                .Select(w => w.FormIndex).DefaultIfEmpty().Max();

                            if (fm_index > index)
                                index = fm_index;

                            index += 1;
                            var sm = new GtEcsbmn
                            {
                                MenuItemId = subMenuID,
                                MainMenuId = obj.MainMenuId,
                                MenuItemName = obj.MenuItemName,
                                ParentId = obj.ParentID,
                                MenuIndex = index,
                                ImageUrl = obj.ImageURL,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalId,
                                CreatedBy = obj.UserId,

                            };
                            db.GtEcsbmns.Add(sm);
                        }
                        else
                        {
                            var sm = db.GtEcsbmns.Where(w => w.MenuItemId == obj.MenuItemId).FirstOrDefault();
                            if (sm != null)
                            {
                                sm.MenuItemName = obj.MenuItemName;
                                sm.ImageUrl = obj.ImageURL;
                                sm.ActiveStatus = obj.ActiveStatus;
                                sm.ModifiedOn = DateTime.Now;
                                sm.ModifiedTerminal = obj.TerminalId;
                                sm.ModifiedBy = obj.UserId;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateSubMenusIndex(int menuItemId, bool isMoveUp, bool isMoveDown)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<GtEcsbmn> list = (from y in db.GtEcsbmns
                                               select y).ToList();


                        var mn = list.Where(W => W.MenuItemId == menuItemId).FirstOrDefault();

                        var l_first = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId != menuItemId && W.MenuIndex <= mn.MenuIndex).OrderBy(o => o.MenuIndex);
                        var l_last = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId != menuItemId && W.MenuIndex > mn.MenuIndex).OrderBy(o => o.MenuIndex);

                        int runningIndex = 0;
                        if (isMoveUp)
                        {
                            foreach (var m in l_first)
                            {
                                if (l_first.ToList().IndexOf(m) != l_first.Count() - 1)
                                {
                                    runningIndex = runningIndex + 1;
                                    m.MenuIndex = (short)runningIndex;
                                }
                                else
                                {
                                    runningIndex = runningIndex + 1;
                                    mn.MenuIndex = (short)runningIndex;

                                    runningIndex = runningIndex + 1;
                                    m.MenuIndex = (short)runningIndex;
                                }
                            }

                            foreach (var m in l_last.Skip(1))
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }
                        }
                        else if (isMoveDown)
                        {
                            foreach (var m in l_first)
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }

                            foreach (var m in l_last.Take(1))
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }

                            runningIndex = runningIndex + 1;
                            mn.MenuIndex = (short)runningIndex;

                            foreach (var m in l_last.Skip(1))
                            {
                                runningIndex = runningIndex + 1;
                                m.MenuIndex = (short)runningIndex;
                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> DeleteSubMenu(int menuItemId)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (db.GtEcsbmns.Where(w => w.ParentId == menuItemId).Count() > 0)
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0036", Message = string.Format(_localizer[name: "W0036"]) };
                        if (db.GtEcmnfls.Where(w => w.MenuItemId == menuItemId).Count() > 0)
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0037", Message = string.Format(_localizer[name: "W0037"]) };

                        var del = db.GtEcsbmns.Where(w => w.MenuItemId == menuItemId).FirstOrDefault();
                        if (del != null)
                        {
                            db.Remove(del);

                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0038", Message = string.Format(_localizer[name: "W0038"]) };
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion SubMenu

        #region Forms
        public async Task<DO_FormMenu> GetFormDetailById(int mainMenuId, int menuItemId, int formId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var fm = db.GtEcmnfls
                                  .Where(w => (w.MenuItemId == menuItemId || (w.MenuItemId == 0 && w.MainMenuId == mainMenuId))
                                  && w.FormId == formId)
                                  .Select(r => new DO_FormMenu
                                  {
                                      FormId = r.FormId,
                                      MainMenuId = r.MainMenuId,
                                      MenuItemId = r.MenuItemId,
                                      FormNameClient = r.FormNameClient,
                                      FormIndex = r.FormIndex,
                                      ActiveStatus = r.ActiveStatus,
                                  }).FirstOrDefaultAsync();
                    return await fm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertIntoFormMenu(DO_FormMenu obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.MainMenuId == 0)
                        {
                            var mm = db.GtEcsbmns.Where(w => w.MenuItemId == obj.MenuItemId).FirstOrDefault();
                            obj.MainMenuId = mm.MainMenuId;
                        }

                        var fm = db.GtEcmnfls.Where(w => w.MainMenuId == obj.MainMenuId && w.MenuItemId == obj.MenuItemId && w.FormId == obj.FormId).FirstOrDefault();
                        if (fm == null)
                        {
                            var formIndex = db.GtEcmnfls.Where(w => w.MainMenuId == obj.MainMenuId && (w.MenuItemId == obj.MenuItemId || (w.MenuItemId == 0 && w.MainMenuId == obj.MainMenuId)))
                                .Select(w => w.FormIndex).DefaultIfEmpty().Max();

                            var sbIndex = db.GtEcsbmns.Where(w => w.MainMenuId == obj.MainMenuId && w.ParentId == obj.MenuItemId)
                              .Select(w => w.MenuIndex).DefaultIfEmpty().Max();

                            if (sbIndex > formIndex)
                                formIndex = sbIndex;

                            obj.FormIndex = formIndex + 1;

                            var menuKey = Convert.ToInt32(obj.MainMenuId.ToString() + obj.MenuItemId.ToString() + obj.FormId.ToString());

                            GtEcmnfl ins = new GtEcmnfl
                            {
                                FormId = obj.FormId,
                                MainMenuId = obj.MainMenuId,
                                MenuItemId = obj.MenuItemId,
                                MenuKey = menuKey,
                                FormNameClient = obj.FormNameClient,
                                FormIndex = obj.FormIndex,
                                ActiveStatus = true,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalId,
                                CreatedBy = obj.UserId,

                            };
                            db.GtEcmnfls.Add(ins);
                        }
                        else
                        {
                            fm.FormNameClient = obj.FormNameClient;
                            fm.ActiveStatus = obj.ActiveStatus;
                            fm.ModifiedOn = DateTime.Now;
                            fm.ModifiedBy = obj.UserId;
                            fm.ModifiedTerminal = obj.TerminalId;
                        }
                        await db.SaveChangesAsync();


                        UpdateFormsIndex(db, obj.MainMenuId, obj.MenuItemId, obj.FormId);

                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]), ID = obj.FormId };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public void UpdateFormsIndex(eSyaEnterprise db, int mainMenuID, int menuItemID, int formID)
        {

            try
            {
                List<GtEcmnfl> list = db.GtEcmnfls.ToList();

                var mn = list.Where(w => ((w.MenuItemId != 0 && w.MenuItemId == menuItemID) || (w.MenuItemId == 0 && w.MainMenuId == mainMenuID)) && w.FormId == formID).FirstOrDefault();

                var l_first = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId == mn.MenuItemId && W.FormId != formID && W.FormIndex < mn.FormIndex).OrderBy(o => o.FormIndex).ToList();
                var l_last = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId == mn.MenuItemId && W.FormId != formID && W.FormIndex >= mn.FormIndex).OrderBy(o => o.FormIndex).ToList();

                int runningIndex = 0;
                foreach (var m in l_first)
                {
                    runningIndex = runningIndex + 1;
                    m.FormIndex = (short)runningIndex;
                }

                runningIndex = mn.FormIndex;
                foreach (var m in l_last)
                {
                    runningIndex = runningIndex + 1;
                    m.FormIndex = (short)runningIndex;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DO_ReturnParameter UpdateFormsIndex(int mainMenuId, int menuItemId, int formID, bool isMoveUp, bool isMoveDown)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<GtEcmnfl> list = db.GtEcmnfls.ToList();

                        var mn = list.Where(w => ((w.MenuItemId != 0 && w.MenuItemId == menuItemId) || (w.MenuItemId == 0 && w.MainMenuId == mainMenuId))
                            && w.FormId == formID).FirstOrDefault();

                        var l_first = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId == mn.MenuItemId && W.FormId != formID && W.FormIndex <= mn.FormIndex).OrderBy(o => o.FormIndex).ToList();

                        int runningIndex = 0;
                        if (isMoveUp)
                        {
                            foreach (var m in l_first)
                            {
                                if (l_first.ToList().IndexOf(m) != l_first.Count() - 1)
                                {
                                    runningIndex = runningIndex + 1;
                                    m.FormIndex = (short)runningIndex;
                                }
                                else
                                {
                                    runningIndex = runningIndex + 1;
                                    mn.FormIndex = (short)(runningIndex);

                                    runningIndex = runningIndex + 1;
                                    m.FormIndex = (short)runningIndex;
                                }
                            }

                            var l_last = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId == mn.MenuItemId && W.FormId != formID && W.FormIndex > runningIndex).OrderBy(o => o.FormIndex).ToList();

                            foreach (var m in l_last)
                            {
                                runningIndex = runningIndex + 1;
                                m.FormIndex = (short)runningIndex;
                            }
                        }
                        else if (isMoveDown)
                        {
                            foreach (var m in l_first)
                            {
                                runningIndex = runningIndex + 1;
                                m.FormIndex = (short)runningIndex;
                            }

                            var l_last = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId == mn.MenuItemId && W.FormId != formID && W.FormIndex > mn.FormIndex).OrderBy(o => o.FormIndex).ToList();
                            foreach (var m in l_last.Take(1))
                            {
                                runningIndex = runningIndex + 1;
                                m.FormIndex = (short)runningIndex;
                            }

                            runningIndex = runningIndex + 1;
                            mn.FormIndex = (short)runningIndex;

                            l_last = list.Where(W => W.MainMenuId == mn.MainMenuId && W.MenuItemId == mn.MenuItemId && W.FormId != formID && W.FormIndex > mn.FormIndex).OrderBy(o => o.FormIndex).ToList();
                            foreach (var m in l_last)
                            {
                                runningIndex = runningIndex + 1;
                                m.FormIndex = (short)runningIndex;
                            }
                        }

                        db.SaveChanges();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> DeleteFormMenu(int mainMenuId, int menuItemId, int formId)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var del = db.GtEcmnfls.Where(w => ((w.MenuItemId != 0 && w.MenuItemId == menuItemId) || (w.MenuItemId == 0 && w.MainMenuId == mainMenuId))
                           && w.FormId == formId).FirstOrDefault();
                        if (del != null)
                        {
                            db.GtEcmnfls.Remove(del);

                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0039", Message = string.Format(_localizer[name: "W0039"]) };
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion Forms

        #region Configure Menu
        public async Task<DO_ConfigureMenu> GetConfigureMenuMaster()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    DO_ConfigureMenu mn = new DO_ConfigureMenu();
                    mn.l_MainMenu = await db.GtEcmamns
                                    .Select(m => new DO_MainMenu()
                                    {
                                        MainMenuId = m.MainMenuId,
                                        MainMenu = m.MainMenu,
                                        MenuIndex = m.MenuIndex,
                                        ActiveStatus = m.ActiveStatus
                                    }).ToListAsync();

                    mn.l_SubMenu = await db.GtEcsbmns
                                    .Select(s => new DO_SubMenu()
                                    {
                                        MainMenuId = s.MainMenuId,
                                        MenuItemId = s.MenuItemId,
                                        MenuItemName = s.MenuItemName,
                                        MenuIndex = s.MenuIndex,
                                        ParentID = s.ParentId,
                                        ActiveStatus = s.ActiveStatus
                                    }).ToListAsync();

                    mn.l_FormMenu = await db.GtEcmnfls
                                    .Select(f => new DO_FormMenu()
                                    {
                                        MainMenuId = f.MainMenuId,
                                        MenuItemId = f.MenuItemId,
                                        FormId = f.FormId,
                                        FormNameClient = f.FormNameClient,
                                        FormIndex = f.FormIndex,
                                        ActiveStatus = f.ActiveStatus

                                    }).ToListAsync();

                    return mn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_MainMenu>> GetConfigureMenulist()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var menuList = db.GtEcmamns.Where(w => w.ActiveStatus == true)
                                   .Select(m => new DO_MainMenu()
                                   {
                                       MainMenuId = m.MainMenuId,
                                       MainMenu = m.MainMenu,
                                       MenuIndex = m.MenuIndex,
                                       l_SubMenu = db.GtEcsbmns.Where(w => w.MainMenuId == m.MainMenuId && w.ActiveStatus == true)
                                        .Select(s => new DO_SubMenu()
                                        {
                                            MainMenuId = s.MainMenuId,
                                            MenuItemId = s.MenuItemId,
                                            MenuItemName = s.MenuItemName,
                                            MenuIndex = s.MenuIndex,
                                            l_FormMenu = db.GtEcmnfls.Where(w => w.MenuItemId == s.MenuItemId && w.ActiveStatus == true)
                                                .Select(f => new DO_FormMenu()
                                                {
                                                    FormId = f.FormId,
                                                    FormNameClient = f.FormNameClient,
                                                    FormIndex = f.FormIndex,
                                                    //for User Group
                                                    MenuKey = f.MenuKey
                                                }).ToList(),

                                        }).ToList()
                                   });
                    return await menuList.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
