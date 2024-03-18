using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DL.Repository
{
    public class FormsRepository: IFormsRepository
    {
        private readonly IStringLocalizer<FormsRepository> _localizer;
        public FormsRepository(IStringLocalizer<FormsRepository> localizer)
        {
            _localizer = localizer;
        }
       
        #region Form Master
        public async Task<List<DO_AreaController>> GetAreaController()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEbecnts
                                  .Where(w => w.ActiveStatus)
                                  .Select(r => new DO_AreaController
                                  {
                                      Area = r.Area,
                                      Controller = r.Controller,
                                  }).OrderBy(o => o.Area).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetFormDetails()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmfds
                                  .Where(w => w.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      FormCode = r.FormCode,
                                      FormName = r.FormName,
                                      ControllerName = r.ControllerName
                                  }).OrderBy(o => o.FormCode).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetInternalFormDetails()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmnms
                                  // .Where(w => w.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      InternalFormNumber = r.FormIntId,
                                      FormName = r.FormDescription,
                                      NavigateURL = r.NavigateUrl
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_Forms> GetFormDetailsByID(int formID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmfds
                                  .Where(w => w.FormId == formID && w.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      FormCode = r.FormCode,
                                      FormName = r.FormName,
                                      ControllerName = r.ControllerName,
                                      ToolTip = r.ToolTip,
                                      //IsDocumentNumberRequired = r.IsDocumentNumberRequired,
                                      //IsStoreLink = r.IsStoreLink,
                                      //IsDoctor = r.IsDoctor,
                                      //l_FormParameter = r.GtEcfmpa.Where(w=>w.FormId == formID).Select(p => new DO_eSyaParameter
                                      //{
                                      //    ParameterID = p.ParameterId,
                                      //    ParmAction = p.ParmAction
                                      //}).ToList()
                                  }).FirstOrDefaultAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetInternalFormByFormID(int formID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmnms
                                  .Where(w => w.FormId == formID)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      InternalFormNumber = r.FormIntId,
                                      NavigateURL = r.NavigateUrl,
                                      FormDescription = r.FormDescription,
                                      ActiveStatus = r.ActiveStatus,
                                  }).Distinct().ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertUpdateIntoFormMaster(DO_Forms obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.IsInsert)
                        {
                            if (db.GtEcfmfds.Where(w => w.FormCode == obj.FormCode).Count() > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0058", Message = string.Format(_localizer[name: "W0058"]) };
                            }
                        }
                        //   if (db.GtEcfmfd.Where(w => w.FormId == obj.FormID).Count() <= 0)
                        if (obj.IsInsert)
                        {
                            obj.FormID = db.GtEcfmfds.Select(s => s.FormId).DefaultIfEmpty().Max() + 1;
                            var ag = new GtEcfmfd
                            {
                                FormId = obj.FormID,
                                FormCode = obj.FormCode,
                                FormName = obj.FormName,
                                ControllerName = obj.ControllerName,
                                ToolTip = obj.ToolTip,
                                //IsDocumentNumberRequired = obj.IsDocumentNumberRequired,
                                //IsStoreLink = obj.IsStoreLink,
                                //IsDoctor = obj.IsDoctor,
                                ActiveStatus = true,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcfmfds.Add(ag);

                            var formNumber = db.GtEcfmnms.Where(w => w.FormId == obj.FormID).Count();
                            string internalFormID = obj.FormCode.ToString() + "_00";
                            obj.NavigateURL = obj.ControllerName + "/" + internalFormID;
                            if (formNumber <= 0)
                            {
                                formNumber = formNumber + 1;
                                GtEcfmnm fn = new GtEcfmnm
                                {
                                    FormId = obj.FormID,
                                    FormIntId = internalFormID,
                                    NavigateUrl = obj.NavigateURL,
                                    FormDescription = "Standard",
                                    ActiveStatus = obj.ActiveStatus,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,
                                    CreatedBy = obj.UserID,

                                };
                                db.GtEcfmnms.Add(fn);
                            }
                        }
                        else
                        {
                            var ag = db.GtEcfmfds.Where(w => w.FormId == obj.FormID).FirstOrDefault();
                            ag.FormName = obj.FormName;
                            ag.ToolTip = obj.ToolTip;
                            //ag.IsDocumentNumberRequired = obj.IsDocumentNumberRequired;
                            //ag.IsStoreLink = obj.IsStoreLink;
                            //ag.IsDoctor = obj.IsDoctor;
                            ag.ModifiedBy = obj.UserID;
                            ag.ModifiedOn = System.DateTime.Now;
                            ag.ModifiedTerminal = obj.TerminalID;
                        }



                        //if (obj.IsStoreLink)
                        //{
                        //    foreach (var p in obj.l_FormParameter)
                        //    {
                        //        var fp = db.GtEcfmpa.Where(w => w.FormId == obj.FormID && w.ParameterId == p.ParameterID).FirstOrDefault();
                        //        if (fp == null)
                        //        {
                        //            fp = new GtEcfmpa
                        //            {
                        //                FormId = obj.FormID,
                        //                ParameterId = p.ParameterID,
                        //                ParmAction = p.ParmAction,
                        //                ActiveStatus = obj.ActiveStatus,
                        //                CreatedOn = DateTime.Now,
                        //                CreatedTerminal = obj.TerminalID,
                        //                CreatedBy = obj.UserID,
                        //            };
                        //            db.GtEcfmpa.Add(fp);
                        //        }
                        //        else
                        //        {
                        //            fp.ParmAction = p.ParmAction;
                        //            fp.ActiveStatus = obj.ActiveStatus;
                        //            fp.ModifiedBy = obj.UserID;
                        //            fp.ModifiedOn = System.DateTime.Now;
                        //            fp.ModifiedTerminal = obj.TerminalID;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    await db.GtEcfmpa.Where(w => w.FormId == obj.FormID)
                        //    .ForEachAsync(
                        //        a =>
                        //        {
                        //            a.ParmAction = false;
                        //        }
                        //    );
                        //}

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]), ID = obj.FormID };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_Forms> GetNextInternalFormByID(int formID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    DO_Forms obj = new DO_Forms();
                    var fm = await db.GtEcfmfds.Where(w => w.FormId == formID).FirstOrDefaultAsync();

                    int formNo = db.GtEcfmnms.Where(w => w.FormId == formID).Count();
                    while (true)
                    {
                        string internalFormNo = fm.FormCode.ToString() + "_" + formNo.ToString().PadLeft(2, '0');
                        obj.InternalFormNumber = internalFormNo;
                        obj.NavigateURL = fm.ControllerName + "/" + internalFormNo;

                        var f = await db.GtEcfmnms.Where(w => w.FormId == formID && w.NavigateUrl == obj.NavigateURL).FirstOrDefaultAsync();
                        if (f != null)
                        {
                            formNo++;
                            continue;
                        }
                        else
                            break;
                    }

                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoInternalForm(DO_Forms obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fm = db.GtEcfmfds.Where(w => w.FormId == obj.FormID).FirstOrDefault();
                        obj.NavigateURL = fm.ControllerName + "/" + obj.InternalFormNumber;

                        var fn = db.GtEcfmnms.Where(w => w.FormId == obj.FormID && w.FormIntId == obj.InternalFormNumber).FirstOrDefault();
                        if (fn != null)
                        {
                            fn.NavigateUrl = obj.NavigateURL;
                            fn.FormDescription = obj.FormDescription;
                            fn.ActiveStatus = obj.ActiveStatus;
                            fn.ModifiedBy = obj.UserID;
                            fn.ModifiedOn = DateTime.Now;
                            fn.ModifiedTerminal = obj.TerminalID;
                        }
                        else
                        {
                            fn = new GtEcfmnm
                            {
                                FormId = obj.FormID,
                                FormIntId = obj.InternalFormNumber,
                                NavigateUrl = obj.NavigateURL,
                                FormDescription = obj.FormDescription,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                                CreatedBy = obj.UserID,

                            };
                            db.GtEcfmnms.Add(fn);
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

        public async Task<List<DO_FormAction>> GetFormAction()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var fa = db.GtEcfmacs
                        .Select(r => new DO_FormAction
                        {
                            ActionId = r.ActionId,
                            ActionDesc = r.ActionDesc,
                            ActiveStatus = r.ActiveStatus,
                        }).ToListAsync();
                    return await fa;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_FormAction>> GetFormActionByID(int formID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var fa = db.GtEcfmacs
                    .GroupJoin(db.GtEcfmals.Where(w => w.FormId == formID),
                      e => e.ActionId,
                      d => d.ActionId,
                     (emp, depts) => new { emp, depts })
                    .SelectMany(z => z.depts.DefaultIfEmpty(),
                     (a, b) => new DO_FormAction
                     {
                         ActionId = a.emp.ActionId,
                         ActionDesc = a.emp.ActionDesc,
                         ActiveStatus = b == null ? false : b.ActiveStatus
                    }).ToListAsync();


                    //var fa = db.GtEcfmacs
                    //    .GroupJoin(db.GtEcfmals.Where(w => w.FormId == formID),
                    //    a => a.ActionId,
                    //    f => f.ActionId,
                    //    (a, f) => new { a, f = f.FirstOrDefault() })
                    //    .Select(r => new DO_FormAction
                    //    {
                    //        ActionId = r.a.ActionId,
                    //        ActionDesc = r.a.ActionDesc,
                    //        ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                    //    }).ToListAsync();
                    return await fa;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> InsertIntoFormAction(DO_Forms obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fa = db.GtEcfmals.Where(w => w.FormId == obj.FormID);
                        foreach (GtEcfmal f in fa)
                        {
                            f.ActiveStatus = false;
                            f.ModifiedBy = obj.UserID;
                            f.ModifiedOn = System.DateTime.Now;
                            f.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();

                        if (obj.l_FormAction != null)
                        {
                            foreach (DO_FormAction i in obj.l_FormAction)
                            {
                                var obj_FA = db.GtEcfmals.Where(w => w.FormId == obj.FormID && w.ActionId == i.ActionId).FirstOrDefault();
                                if (obj_FA != null)
                                {
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.ModifiedBy = obj.UserID;
                                    obj_FA.ModifiedOn = DateTime.Now;
                                    obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                }
                                else
                                {
                                    obj_FA = new GtEcfmal();
                                    obj_FA.FormId = obj.FormID;
                                    obj_FA.ActionId = i.ActionId;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.CreatedBy = obj.UserID;
                                    obj_FA.CreatedOn = DateTime.Now;
                                    obj_FA.CreatedTerminal = System.Environment.MachineName;
                                    db.GtEcfmals.Add(obj_FA);
                                }
                            }
                            await db.SaveChangesAsync();
                        }


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

        public async Task<List<DO_FormParameter>> GetFormParameterByID(int formID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var fa = db.GtEcparms.Where(w => w.ParameterType == 13)
                    .GroupJoin(db.GtEcfmpas.Where(w => w.FormId == formID),
                      e => e.ParameterId,
                      d => d.ParameterId,
                     (emp, depts) => new { emp, depts })
                    .SelectMany(z => z.depts.DefaultIfEmpty(),
                     (a, b) => new DO_FormParameter
                     {
                         ParameterId = a.emp.ParameterId,
                         ParameterDesc = a.emp.ParameterDesc,
                         ActiveStatus = b == null ? false : b.ActiveStatus
                     }).ToListAsync();
                    return await fa;
                    //var fa = db.GtEcparms.Where(w => w.ParameterType == 13)
                    //    .GroupJoin(db.GtEcfmpas.Where(w => w.FormId == formID),
                    //    a => a.ParameterId,
                    //    f => f.ParameterId,
                    //    (a, f) => new { a, f = f.FirstOrDefault() })
                    //    .Select(r => new DO_FormParameter
                    //    {
                    //        ParameterId = r.a.ParameterId,
                    //        ParameterDesc = r.a.ParameterDesc,
                    //        ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                    //    }).ToListAsync();
                    //return await fa;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> InsertIntoFormParameter(DO_Forms obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fa = db.GtEcfmpas.Where(w => w.FormId == obj.FormID);
                        foreach (GtEcfmpa f in fa)
                        {
                            f.ActiveStatus = false;
                            f.ModifiedBy = obj.UserID;
                            f.ModifiedOn = System.DateTime.Now;
                            f.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();

                        if (obj.l_FormParameter != null)
                        {
                            foreach (DO_FormParameter i in obj.l_FormParameter)
                            {
                                var obj_FA = db.GtEcfmpas.Where(w => w.FormId == obj.FormID && w.ParameterId == i.ParameterId).FirstOrDefault();
                                if (obj_FA != null)
                                {
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.ModifiedBy = obj.UserID;
                                    obj_FA.ModifiedOn = DateTime.Now;
                                    obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                }
                                else
                                {
                                    obj_FA = new GtEcfmpa();
                                    obj_FA.FormId = obj.FormID;
                                    obj_FA.ParameterId = i.ParameterId;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.CreatedBy = obj.UserID;
                                    obj_FA.CreatedOn = DateTime.Now;
                                    obj_FA.CreatedTerminal = System.Environment.MachineName;
                                    db.GtEcfmpas.Add(obj_FA);
                                }
                            }
                            await db.SaveChangesAsync();
                        }


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

        //public async Task<List<DO_FormSubParameter>> GetFormSubParameterByID(int formID, int parameterId)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            if (parameterId == 1)
        //            {


        //                var fa = db.GtEcparms.Where(w => w.ParameterType == 8)
        //               .GroupJoin(db.GtEcfmps.Where(w => w.FormId == formID && w.ParameterId == parameterId),
        //              e => e.ParameterId,
        //              d => d.SubParameterId,
        //             (emp, depts) => new { emp, depts })
        //            .SelectMany(z => z.depts.DefaultIfEmpty(),
        //             (a, b) => new DO_FormSubParameter
        //             {
        //                 SubParameterId = a.emp.ParameterId,
        //                 SubParameterDesc = a.emp.ParameterDesc,
        //                 ActiveStatus = b == null ? false : b.ActiveStatus
        //             }).ToListAsync();
        //                return await fa;
        //            }
        //            else
        //            {
        //                var fa = db.GtEcparms.Where(w => w.ParameterType == 13 && w.ParameterId == parameterId)
        //               .GroupJoin(db.GtEcfmps.Where(w => w.FormId == formID && w.ParameterId == parameterId),
        //              e => e.ParameterId,
        //              d => d.ParameterId,
        //             (emp, depts) => new { emp, depts })
        //            .SelectMany(z => z.depts.DefaultIfEmpty(),
        //             (a, b) => new DO_FormSubParameter
        //             {
        //                 SubParameterId = a.emp.ParameterId,
        //                 SubParameterDesc = a.emp.ParameterDesc,
        //                 ActiveStatus = b == null ? false : b.ActiveStatus
        //             }).ToListAsync();
        //                return await fa;


        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //public async Task<DO_ReturnParameter> InsertIntoFormSubParameter(DO_Forms obj)
        //{
        //    using (eSyaEnterprise db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                int parameterId = obj.ParameterId;
        //                var fa = db.GtEcfmps.Where(w => w.FormId == obj.FormID && w.ParameterId == parameterId);
        //                foreach (GtEcfmp f in fa)
        //                {
        //                    f.ParmAction = true;
        //                    f.ActiveStatus = false;
        //                    f.ModifiedBy = obj.UserID;
        //                    f.ModifiedOn = System.DateTime.Now;
        //                    f.ModifiedTerminal = obj.TerminalID;
        //                }
        //                await db.SaveChangesAsync();

        //                var obj_P = db.GtEcfmpas.Where(w => w.FormId == obj.FormID && w.ParameterId == parameterId).FirstOrDefault();
        //                if (obj_P != null)
        //                {
        //                    obj_P.ActiveStatus = false;
        //                    obj_P.ModifiedBy = obj.UserID;
        //                    obj_P.ModifiedOn = DateTime.Now;
        //                    obj_P.ModifiedTerminal = System.Environment.MachineName;
        //                }

        //                if (obj.l_FormSubParameter != null)
        //                {
        //                    if (obj_P != null)
        //                    {
        //                        obj_P.ActiveStatus = true;
        //                        obj_P.ModifiedBy = obj.UserID;
        //                        obj_P.ModifiedOn = DateTime.Now;
        //                        obj_P.ModifiedTerminal = System.Environment.MachineName;
        //                    }
        //                    else
        //                    {
        //                        obj_P = new GtEcfmpa();
        //                        obj_P.FormId = obj.FormID;
        //                        obj_P.ParameterId = parameterId;
        //                        obj_P.ActiveStatus = true;
        //                        obj_P.CreatedBy = obj.UserID;
        //                        obj_P.CreatedOn = DateTime.Now;
        //                        obj_P.CreatedTerminal = System.Environment.MachineName;
        //                        db.GtEcfmpas.Add(obj_P);
        //                    }

        //                    foreach (DO_FormSubParameter i in obj.l_FormSubParameter)
        //                    {
        //                        var obj_FA = db.GtEcfmps.Where(w => w.FormId == obj.FormID && w.ParameterId == parameterId && w.SubParameterId == i.SubParameterId).FirstOrDefault();
        //                        if (obj_FA != null)
        //                        {
        //                            obj_FA.ParmAction = true;
        //                            obj_FA.ActiveStatus = true;
        //                            obj_FA.ModifiedBy = obj.UserID;
        //                            obj_FA.ModifiedOn = DateTime.Now;
        //                            obj_FA.ModifiedTerminal = System.Environment.MachineName;
        //                        }
        //                        else
        //                        {
        //                            obj_FA = new GtEcfmp();
        //                            obj_FA.FormId = obj.FormID;
        //                            obj_FA.ParameterId = parameterId;
        //                            obj_FA.SubParameterId = i.SubParameterId;
        //                            obj_FA.ParmAction = true;
        //                            obj_FA.ActiveStatus = true;
        //                            obj_FA.CreatedBy = obj.UserID;
        //                            obj_FA.CreatedOn = DateTime.Now;
        //                            obj_FA.CreatedTerminal = System.Environment.MachineName;
        //                            db.GtEcfmps.Add(obj_FA);
        //                        }
        //                    }
        //                }

        //                await db.SaveChangesAsync();
        //                dbContext.Commit();

        //                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
        //            }
        //            catch (Exception ex)
        //            {
        //                dbContext.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //}

        #endregion


        #region Area Controller
        public async Task<List<DO_AreaController>> GetControllerbyArea(string Area)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (Area == "All"||string.IsNullOrEmpty(Area))
                    {

                        var ds = db.GtEbecnts
                        .Select(a => new DO_AreaController
                        {
                            Id = a.Id,
                            Area = a.Area,
                            Controller = a.Controller,
                            ActiveStatus = a.ActiveStatus
                        }).OrderBy(x => x.Controller).ToListAsync();

                        return await ds;
                    }
                    else
                    {
                        var ds = db.GtEbecnts.Where(x =>x.Area.ToUpper().Replace(" ", "")== Area.ToUpper().Replace(" ", ""))
                        .Select(a => new DO_AreaController
                        {
                            Id = a.Id,
                            Area = a.Area,
                            Controller = a.Controller,
                            ActiveStatus = a.ActiveStatus
                        }).OrderBy(x => x.Controller).ToListAsync();

                        return await ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_AreaController>> GetAllAreaController()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEbecnts
                        .Select(a => new DO_AreaController
                        {
                            Id = a.Id,
                            Area = a.Area,
                            Controller = a.Controller,
                            ActiveStatus = a.ActiveStatus
                        }).ToList();
                    var DistinctKeys = ds.GroupBy(x => x.Area).Select(y => y.First());
                    return DistinctKeys.OrderBy(x=>x.Area).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoAreaController(DO_AreaController obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        //int maxId = db.GtEbecnt.Select(a => a.Id).DefaultIfEmpty().Max();
                        //int Ar_Id = maxId + 1;
                        //Here Id is Identity column

                        var ar_ct = new GtEbecnt
                        {
                            //Id=Ar_Id,
                            Area = obj.Area,
                            Controller = obj.Controller,
                            ActiveStatus = obj.ActiveStatus
                        };
                        db.GtEbecnts.Add(ar_ct);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateAreaController(DO_AreaController obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEbecnt ar_ct = db.GtEbecnts.Where(x => x.Id == obj.Id).FirstOrDefault();
                        if (ar_ct == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0059", Message = string.Format(_localizer[name: "W0059"]) };
                        }

                        ar_ct.Area = obj.Area;
                        ar_ct.Controller = obj.Controller;
                        ar_ct.ActiveStatus = obj.ActiveStatus;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveAreaController(bool status, int Id)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEbecnt ar_ct = db.GtEbecnts.Where(w => w.Id == Id).FirstOrDefault();
                        if (ar_ct == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0059", Message = string.Format(_localizer[name: "W0059"]) };
                        }

                        ar_ct.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion
        

        #region Define Actions 
        public async Task<List<DO_Actions>> GetAllActions()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcfmacs
                        .Select(r => new DO_Actions
                        {
                            ActionId = r.ActionId,
                            ActionDesc = r.ActionDesc,
                            ActiveStatus = r.ActiveStatus,
                        }).OrderBy(o => o.ActionDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertIntoActions(DO_Actions obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {


                        bool is_actionExist = db.GtEcfmacs.Any(a => a.ActionDesc.ToUpper().Replace(" ", "") == obj.ActionDesc.ToUpper().Replace(" ", ""));
                        if (is_actionExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00157", Message = string.Format(_localizer[name: "W00157"]) };
                        }
                        int maxAcctionId = db.GtEcfmacs.Select(c => c.ActionId).DefaultIfEmpty().Max();
                        maxAcctionId = maxAcctionId + 1;
                        var obj_act = new GtEcfmac
                        {
                            ActionId = maxAcctionId,
                            ActionDesc = obj.ActionDesc,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcfmacs.Add(obj_act);

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> UpdateActions(DO_Actions obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_actionExist = db.GtEcfmacs.Any(a => a.ActionId != obj.ActionId && a.ActionDesc.ToUpper().Replace(" ", "") == obj.ActionDesc.ToUpper().Replace(" ", ""));
                        if (is_actionExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00157", Message = string.Format(_localizer[name: "W00157"]) };
                        }


                        GtEcfmac act = db.GtEcfmacs.Where(w => w.ActionId == obj.ActionId).FirstOrDefault();
                        if (act == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00158", Message = string.Format(_localizer[name: "W00158"]) };
                        }

                        act.ActionDesc = obj.ActionDesc;
                        act.ActiveStatus = obj.ActiveStatus;
                        act.ModifiedBy = obj.UserID;
                        act.ModifiedOn = System.DateTime.Now;
                        act.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> ActiveOrDeActiveActions(bool status, int actionId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcfmac act = db.GtEcfmacs.Where(w => w.ActionId == actionId).FirstOrDefault();
                        if (act == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00158", Message = string.Format(_localizer[name: "W00158"]) };
                        }

                        act.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

       
    }
}
