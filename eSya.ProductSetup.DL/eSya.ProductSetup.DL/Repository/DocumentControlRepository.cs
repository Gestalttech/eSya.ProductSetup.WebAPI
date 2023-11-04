using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DL.Repository
{
    public class DocumentControlRepository: IDocumentControlRepository
    {
        private readonly IStringLocalizer<DocumentControlRepository> _localizer;
        public DocumentControlRepository(IStringLocalizer<DocumentControlRepository> localizer)
        {
            _localizer = localizer;
        }
        //#region Calendar Defination
        //public async Task<DO_ReturnParameter> InsertCalendarHeaderAndDetails(DO_CalendarDefinition calendarheadar)
        //{
        //    using (eSyaEnterprise db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                int financialcalr = Convert.ToInt32(calendarheadar.FinancialYear);

        //                var isCalendarExists = db.GtEcclcos.Where(x => x.FinancialYear == financialcalr && x.BusinessKey == calendarheadar.BusinessKey).FirstOrDefault();

        //                if (isCalendarExists == null)
        //                {
        //                    GtEcclco calheader = new GtEcclco();
        //                    calheader.FinancialYear = Convert.ToInt32(calendarheadar.FinancialYear);
        //                    calheader.BusinessKey = calendarheadar.BusinessKey;
        //                    calheader.FromDate = calendarheadar.FromDate;
        //                    calheader.TillDate = calendarheadar.TillDate;
        //                    calheader.Status = calendarheadar.Status;
        //                    calheader.FormId = calendarheadar.FormId;
        //                    calheader.CreatedBy = calendarheadar.UserID;
        //                    calheader.CreatedOn = System.DateTime.Now;
        //                    calheader.CreatedTerminal = calendarheadar.TerminalID;
        //                    db.GtEcclcos.Add(calheader);
        //                    await db.SaveChangesAsync();
        //                    List<int> MonthIds = new List<int>();
        //                    string months;
        //                    var financeyear = Convert.ToInt32(calendarheadar.FinancialYear);

        //                    for (int i = calendarheadar.FromDate.Month; i <= calendarheadar.TillDate.Month; i++)
        //                    {
        //                        if (i.ToString().Length == 1)
        //                        {
        //                            string strMonth = 0 + i.ToString();
        //                            months = financeyear.ToString() + "" + strMonth;
        //                        }
        //                        else
        //                        {
        //                            months = financeyear.ToString() + "" + i.ToString();

        //                        }

        //                        MonthIds.Add(Convert.ToInt32(months));
        //                    }

        //                    GtEccldt caldetails = new GtEccldt();

        //                    foreach (var month in MonthIds)
        //                    {
        //                        var calendardetailsExists = db.GtEccldts.Where(x => x.BusinessKey == calendarheadar.BusinessKey &&
        //                          x.FinancialYear == Convert.ToInt32(calendarheadar.FinancialYear) && x.MonthId == month).FirstOrDefault();
        //                        if (calendardetailsExists != null)
        //                        {
        //                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0053", Message = string.Format(_localizer[name: "W0053"]) };
        //                        }
        //                        else
        //                        {
        //                            caldetails.BusinessKey = calendarheadar.BusinessKey;
        //                            caldetails.FinancialYear = Convert.ToInt32(calendarheadar.FinancialYear);
        //                            caldetails.MonthId = month;
        //                            caldetails.MonthFreezeHis = false;
        //                            caldetails.MonthFreezeFin = false;
        //                            caldetails.MonthFreezeHr = false;
        //                            caldetails.PatientIdgen = 0;
        //                            caldetails.PatientIdserial = "0";
        //                            caldetails.BudgetMonth = "Q";
        //                            caldetails.ActiveStatus = true;
        //                            caldetails.FormId = calendarheadar.FormId;
        //                            caldetails.CreatedBy = calendarheadar.UserID;
        //                            caldetails.CreatedOn = DateTime.Now;
        //                            caldetails.CreatedTerminal = calendarheadar.TerminalID;
        //                            db.GtEccldts.Add(caldetails);
        //                            await db.SaveChangesAsync();
        //                        }
        //                    }
        //                    dbContext.Commit();

        //                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
        //                }
        //                else
        //                {
        //                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0054", Message = string.Format(_localizer[name: "W0054"]) };
        //                }
        //            }

        //            catch (Exception ex)
        //            {
        //                dbContext.Rollback();
        //                throw ex;
        //            }
        //        }




        //    }
        //}
        //public async Task<List<DO_CalendarDefinition>> GetCalendarHeadersbyBusinessKey(int Businesskey)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var result = db.GtEcclcos.Where(x => x.BusinessKey == Businesskey)

        //                         .Select(c => new DO_CalendarDefinition
        //                         {
        //                             BusinessKey = c.BusinessKey,
        //                             FinancialYear = c.FinancialYear,
        //                             FromDate = c.FromDate,
        //                             TillDate = c.TillDate,
        //                             Status = c.Status
        //                         }).OrderByDescending(x => x.FinancialYear).ToListAsync();
        //            return await result;


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<List<DO_CalendarDefinition>> GetCalendarHeaders()
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var result = db.GtEcclcos

        //                         .Select(c => new DO_CalendarDefinition
        //                         {
        //                             BusinessKey = c.BusinessKey,
        //                             FinancialYear = c.FinancialYear,
        //                             FromDate = c.FromDate,
        //                             TillDate = c.TillDate,
        //                             Status = c.Status
        //                         }).OrderByDescending(x => x.FinancialYear).ToListAsync();
        //            return await result;


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion Calendar Defination

        #region Calendar Header
        public async Task<List<DO_CalendarHeader>> GetCalendarHeaders()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcclcos

                                 .Select(c => new DO_CalendarHeader
                                 {
                                     CalenderType = c.CalenderType,
                                     Year = c.Year,
                                     CalenderKey = c.CalenderKey,
                                     FromDate = c.FromDate,
                                     TillDate = c.TillDate,
                                     YearEndStatus = c.YearEndStatus,
                                     ActiveStatus = c.ActiveStatus
                                 }).OrderByDescending(x => x.Year).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertCalendarHeader(DO_CalendarHeader obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        int finyear = Convert.ToInt32(obj.Year);

                        var _finyearExist = db.GtEcclcos.Where(x => x.Year == finyear && x.CalenderType.ToUpper().Replace(" ", "") == obj.CalenderType.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (_finyearExist == null)
                        {
                            GtEcclco _chead = new GtEcclco()
                            {
                                CalenderType = obj.CalenderType,
                                Year = Convert.ToInt32(obj.Year),
                                CalenderKey = (obj.CalenderType + Convert.ToInt32(obj.Year)).ToString(),
                                FromDate = obj.FromDate,
                                TillDate = obj.TillDate,
                                YearEndStatus = false,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcclcos.Add(_chead);
                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0054", Message = string.Format(_localizer[name: "W0054"]) };
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

        #endregion

        #region Calendar Details
        public async Task<DO_ReturnParameter> InsertCalendarDetails(DO_CalendarHeader obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        #region Insert into Calendar Business Link
                        var cblink = db.GtEcblcls.Where(x => x.BusinessKey == obj.BusinessKey && x.CalenderKey.ToUpper().Replace(" ", "") == obj.CalenderKey.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (cblink != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00168", Message = string.Format(_localizer[name: "W00168"]) };
                        }
                        GtEcblcl calblink = new GtEcblcl()
                        {
                            BusinessKey = obj.BusinessKey,
                            CalenderKey = obj.CalenderKey,
                            FromDate = obj.FromDate,
                            TillDate = obj.TillDate,
                            YearEndStatus = obj.YearEndStatus,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal=obj.TerminalID
                        };
                        db.GtEcblcls.Add(calblink);
                        await db.SaveChangesAsync();
                        #endregion

                        #region Insrert into PatientID Generation

                        List<int> MonthIds = new List<int>();
                            string months;
                            var financeyear = Convert.ToInt32(obj.Year);

                            for (int i = obj.FromDate.Month; i <= obj.TillDate.Month; i++)
                            {
                                if (i.ToString().Length == 1)
                                {
                                    string strMonth = 0 + i.ToString();
                                    months = financeyear.ToString() + "" + strMonth;
                                }
                                else
                                {
                                    months = financeyear.ToString() + "" + i.ToString();

                                }

                                MonthIds.Add(Convert.ToInt32(months));
                            }

                            GtEcclpi calpatIdgen = new GtEcclpi();

                        foreach (var month in MonthIds)
                        {
                            var patIdExists = db.GtEcclpis.Where(x => x.BusinessKey == obj.BusinessKey &&
                             x.CalenderKey.ToUpper().Replace(" ", "") == obj.CalenderKey.ToUpper().Replace(" ", "") && x.MonthId == month).FirstOrDefault();

                            if (patIdExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W00169", Message = string.Format(_localizer[name: "W00169"]) };

                            }
                            else
                            {
                                calpatIdgen.BusinessKey = obj.BusinessKey;
                                calpatIdgen.CalenderKey =obj.CalenderKey;
                                calpatIdgen.MonthId = month;
                                calpatIdgen.PatientIdgen = "-";
                                calpatIdgen.PatientIdserial = 0;
                                calpatIdgen.ActiveStatus = obj.ActiveStatus;
                                calpatIdgen.FormId = obj.FormID;
                                calpatIdgen.CreatedBy = obj.UserID;
                                calpatIdgen.CreatedOn = DateTime.Now;
                                calpatIdgen.CreatedTerminal = obj.TerminalID;
                                db.GtEcclpis.Add(calpatIdgen);
                                await db.SaveChangesAsync();
                            }
                        }

                        #endregion

                        #region Insrert into Details

                        List<int> pt_MonthIds = new List<int>();
                        string pt_months;
                        var pt_year = Convert.ToInt32(obj.Year);

                        for (int i = obj.FromDate.Month; i <= obj.TillDate.Month; i++)
                        {
                            if (i.ToString().Length == 1)
                            {
                                string strMonth = 0 + i.ToString();
                                pt_months = pt_year.ToString() + "" + strMonth;
                            }
                            else
                            {
                                pt_months = pt_year.ToString() + "" + i.ToString();

                            }

                            pt_MonthIds.Add(Convert.ToInt32(pt_months));
                        }

                       
                        var calparams = db.GtEcparms.Where(x => x.ParameterType == 24).ToList();

                        GtEccldt caldetails = new GtEccldt();

                        foreach (var m in pt_MonthIds)
                        {
                            foreach(var p in calparams)
                            {
                                var pdetailsExists = db.GtEccldts.Where(x => x.BusinessKey == obj.BusinessKey &&
                             x.CalenderKey.ToUpper().Replace(" ", "") == obj.CalenderKey.ToUpper().Replace(" ", "") && x.MonthId == m && x.ParameterId==p.ParameterId).FirstOrDefault();

                                if (pdetailsExists != null)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W00170", Message = string.Format(_localizer[name: "W00170"]) };

                                }
                                else
                                {
                                    caldetails.BusinessKey = obj.BusinessKey;
                                    caldetails.CalenderKey = obj.CalenderKey;
                                    caldetails.MonthId = m;
                                    caldetails.ParameterId = p.ParameterId;
                                    caldetails.ParmPerc = 0;
                                    caldetails.ParmAction = p.ActiveStatus;
                                    //caldetails.ParmDesc = p.ParameterDesc;
                                    caldetails.ParmValue = 0;
                                    caldetails.ActiveStatus = obj.ActiveStatus;
                                    caldetails.FormId = obj.FormID;
                                    caldetails.CreatedBy = obj.UserID;
                                    caldetails.CreatedOn = DateTime.Now;
                                    caldetails.CreatedTerminal = obj.TerminalID;
                                    db.GtEccldts.Add(caldetails);
                                    await db.SaveChangesAsync();
                                }
                            } 
                            
                        }

                        dbContext.Commit();

                        #endregion

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

        #endregion

        #region Document Master
        public async Task<List<DO_DocumentControlMaster>> GetDocumentControlMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtDccnsts.Select(
                        s => new DO_DocumentControlMaster
                        {
                            DocumentId = s.DocumentId,
                            DocumentDesc = s.DocumentDesc,
                            ShortDesc = s.ShortDesc,
                            SchemeId = s.SchemaId,
                            DocumentType = s.DocumentType,
                            UsageStatus = s.UsageStatus,
                            ActiveStatus = s.ActiveStatus
                        }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_eSyaParameter>> GetDocumentParametersByID(int documentID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtDccnpas.Select(
                        s => new DO_eSyaParameter
                        {
                            ParameterID = s.ParameterId,
                            ParmAction = s.ParamAction,
                            ActiveStatus = s.ActiveStatus

                        }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateDocumentControl(DO_DocumentControlMaster obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.Isadd == 1)
                        {
                            var RecordExist = db.GtDccnsts.Where(w => w.DocumentId == obj.DocumentId || w.DocumentDesc == obj.DocumentDesc || w.ShortDesc == obj.ShortDesc).FirstOrDefault();
                            if (RecordExist != null)
                            {
                                if (RecordExist.DocumentId == obj.DocumentId)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0055", Message = string.Format(_localizer[name: "W0055"]) };
                                }
                                else if (RecordExist.DocumentDesc == obj.DocumentDesc)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0056", Message = string.Format(_localizer[name: "W0056"]) };
                                }
                                else if (RecordExist.ShortDesc == obj.ShortDesc)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0057", Message = string.Format(_localizer[name: "W0057"]) };
                                }

                            }
                            else
                            {
                                var _documentcontrol = new GtDccnst
                                {
                                    DocumentId = obj.DocumentId,
                                    DocumentDesc = obj.DocumentDesc,
                                    ShortDesc = obj.ShortDesc,
                                    DocumentType = obj.DocumentType,
                                    SchemaId = obj.SchemeId,
                                    ActiveStatus = true,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtDccnsts.Add(_documentcontrol);
                                foreach (DO_eSyaParameter dp in obj.l_DocumentParameter)
                                {
                                    var dParameter = new GtDccnpa
                                    {
                                        DocumentId = obj.DocumentId,
                                        ParameterId = dp.ParameterID,
                                        ParamAction = dp.ParmAction,
                                        ActiveStatus = dp.ActiveStatus,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,
                                    };
                                    db.GtDccnpas.Add(dParameter);

                                }
                            }
                        }
                        else
                        {
                            var updatedDocumentControl = db.GtDccnsts.Where(w => w.DocumentId == obj.DocumentId).FirstOrDefault();
                            if (updatedDocumentControl.DocumentDesc != obj.DocumentDesc)
                            {
                                var RecordExist = db.GtDccnsts.Where(w => w.DocumentDesc == obj.DocumentDesc).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0056", Message = string.Format(_localizer[name: "W0056"]) };
                                }
                            }
                            if (updatedDocumentControl.ShortDesc != obj.ShortDesc)
                            {
                                var RecordExist = db.GtDccnsts.Where(w => w.ShortDesc == obj.ShortDesc).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0057", Message = string.Format(_localizer[name: "W0057"]) };
                                }
                            }

                            updatedDocumentControl.DocumentDesc = obj.DocumentDesc;
                            updatedDocumentControl.ShortDesc = obj.ShortDesc;
                            updatedDocumentControl.DocumentType = obj.DocumentType;
                            updatedDocumentControl.SchemaId = obj.SchemeId;
                            //updatedDocumentControl.ActiveStatus = obj.ActiveStatus;
                            updatedDocumentControl.ModifiedBy = obj.UserID;
                            updatedDocumentControl.ModifiedOn = System.DateTime.Now;
                            updatedDocumentControl.ModifiedTerminal = obj.TerminalID;

                            foreach (DO_eSyaParameter dp in obj.l_DocumentParameter)
                            {
                                var dPar = db.GtDccnpas.Where(x => x.DocumentId == obj.DocumentId && x.ParameterId == dp.ParameterID).FirstOrDefault();
                                if (dPar != null)
                                {
                                    if (dPar.ParamAction != dp.ParmAction || dPar.ActiveStatus != dp.ActiveStatus)
                                    {
                                        dPar.ParamAction = dp.ParmAction;
                                        dPar.ActiveStatus = dp.ActiveStatus;
                                        dPar.ModifiedBy = obj.UserID;
                                        dPar.ModifiedOn = System.DateTime.Now;
                                        dPar.ModifiedTerminal = obj.TerminalID;
                                    }
                                }
                                else
                                {
                                    var dParameter = new GtDccnpa
                                    {
                                        DocumentId = obj.DocumentId,
                                        ParameterId = dp.ParameterID,
                                        ParamAction = dp.ParmAction,
                                        ActiveStatus = dp.ActiveStatus,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,

                                    };
                                    db.GtDccnpas.Add(dParameter);
                                }

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


        #endregion

        #region Form Document Link
        public async Task<List<DO_Forms>> GetFormsForDocumentControl()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {


                    //var ds = await db.GtEcfmfds
                    //    .Join(db.GtEcfmpas,
                    //    f => f.FormId,
                    //    p => p.FormId,
                    //    (f, p) => new { f, p })
                    //    .GroupJoin(db.GtDncnfds.Where(w => w.ActiveStatus),
                    //    fp => fp.f.FormId,
                    //    d => d.FormId,
                    //    (fp, d) => new { fp, d = d.FirstOrDefault() })
                    //    .Where(w => w.fp.f.ActiveStatus && w.fp.p.ParameterId == 2)
                    //   .Select(s => new DO_Forms
                    //   {
                    //       FormID = s.fp.f.FormId,
                    //       FormName = s.fp.f.FormName,
                    //       FormCode = s.fp.f.FormCode,
                    //       ActiveStatus = s.d != null ? true : false

                    //   }).ToListAsync();
                    //return ds;

                    var ds =await db.GtEcfmfds.Where(x=>x.ActiveStatus==true)
                        .Join(db.GtEcfmpas.Where(x => x.ParameterId == 2),
                        f => f.FormId,
                        p => p.FormId,
                        (f, p) => new { f, p })
                    .GroupJoin(db.GtDncnfds.Where(w => w.ActiveStatus == true),
                      e => e.f.FormId,
                      d => d.FormId,
                     (emp, depts) => new { emp, depts })
                    //.Where(w => w.emp.f.ActiveStatus==true &&w.emp.p.ParameterId==2)
                    .SelectMany(z => z.depts.DefaultIfEmpty(),
                     (a, b) => new DO_Forms
                     {
                         FormID =a.emp.f.FormId,
                         FormName =a.emp.f.FormName,
                         FormCode =a.emp.f.FormCode,
                         ActiveStatus = b == null ? false : b.ActiveStatus
                     }).ToListAsync();
                    //return ds;
                    var Distinctforms = ds.GroupBy(x => x.FormID).Select(y => y.First());
                    return Distinctforms.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_FormDocumentLink>> GetFormDocumentlink(int formID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {


                    //var ds = await db.GtDccnsts
                    //    .GroupJoin(db.GtDncnfds.Where(w => w.FormId == formID),
                    //    d => d.DocumentId,
                    //    l => l.DocumentId,
                    //    (d, l) => new { d, l = l.FirstOrDefault() })
                    //    .Where(w => w.d.ActiveStatus)
                    //   .Select(s => new DO_FormDocumentLink
                    //   {
                    //       FormId = formID,
                    //       DocumentId = s.d.DocumentId,
                    //       DocumentName = s.d.DocumentDesc,
                    //       ActiveStatus = s.l != null ? s.l.ActiveStatus : false

                    //   }).ToListAsync();

                    var ds = await db.GtDccnsts.Where(x=>x.ActiveStatus==true)
                   .GroupJoin(db.GtDncnfds.Where(w => w.FormId == formID),
                     d => d.DocumentId,
                     l => l.DocumentId,
                    (emp, depts) => new { emp, depts })
                   .SelectMany(z => z.depts.DefaultIfEmpty(),
                    (a, b) => new DO_FormDocumentLink
                    {
                        FormId = formID,
                        DocumentId = a.emp.DocumentId,
                        DocumentName =a.emp.DocumentDesc,
                        ActiveStatus = b == null ? false : b.ActiveStatus
                    }).ToListAsync();

                    var Distinctdocuments = ds.GroupBy(x => x.DocumentId).Select(y => y.First());
                    return Distinctdocuments.ToList();
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> UpdateFormDocumentLinks(List<DO_FormDocumentLink> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var _link in obj)
                        {
                            var LinkExist = db.GtDncnfds.Where(w => w.FormId == _link.FormId && w.DocumentId == _link.DocumentId).FirstOrDefault();
                            if (LinkExist != null)
                            {
                                if (_link.ActiveStatus != LinkExist.ActiveStatus)
                                {
                                    LinkExist.ActiveStatus = _link.ActiveStatus;
                                    LinkExist.ModifiedBy = _link.UserID;
                                    LinkExist.ModifiedOn = System.DateTime.Now;
                                    LinkExist.ModifiedTerminal = _link.TerminalID;
                                }
                            }
                            else
                            {
                                if (_link.ActiveStatus)
                                {
                                    var formdoclink = new GtDncnfd
                                    {
                                        FormId = _link.FormId,
                                        DocumentId = _link.DocumentId,
                                        ActiveStatus = _link.ActiveStatus,

                                        CreatedBy = _link.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _link.TerminalID
                                    };
                                    db.GtDncnfds.Add(formdoclink);
                                }

                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        return new DO_ReturnParameter() { Status = false, Message = ex.Message };
                    }
                }
            }
        }
        #endregion

    }
}
