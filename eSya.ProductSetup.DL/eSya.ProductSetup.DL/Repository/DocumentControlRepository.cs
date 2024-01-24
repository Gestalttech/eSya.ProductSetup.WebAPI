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
        
        #region Calendar Header
        public async Task<List<DO_CalendarHeader>> GetCalendarHeaders()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    
                    var result =await db.GtEcclcos

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
                    List<DO_CalendarHeader> lstheader = new List<DO_CalendarHeader>();

                    foreach(var link in result)
                    {
                        var exists = db.GtEcblcls.Where(x => x.CalenderKey == link.CalenderKey).FirstOrDefault();
                        if(exists != null)
                        {
                            link.Alreadylinked = true;
                        }
                        else
                        {
                            link.Alreadylinked = false;
                        }
                        DO_CalendarHeader model = new DO_CalendarHeader()
                        {
                            CalenderType=link.CalenderType,
                            Year= link.Year,
                            CalenderKey= link.CalenderKey,
                            FromDate=link.FromDate,
                            TillDate=link.TillDate,
                            YearEndStatus=link.YearEndStatus,
                            ActiveStatus=link.ActiveStatus,
                            Alreadylinked=link.Alreadylinked
                        };
                        lstheader.Add(model);
                    }
                    return  lstheader;
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

        #region Calendar Patient ID Generation

        public async Task<List<DO_CalendarHeader>> GetCalenderKeybyBusinessKey(int Businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcblcls.Where(x => x.BusinessKey == Businesskey && x.ActiveStatus)

                                 .Select(c => new DO_CalendarHeader
                                 {
                                     CalenderKey = c.CalenderKey

                                 }).OrderByDescending(x => x.CalenderKey).Distinct().ToListAsync();
                    return await result;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DO_CalendarPatientIdGeneration> GetCalendarPatientGenerationbyBusinessKeyAndCalenderKey(int BusinessKey, string CalenderKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    if (BusinessKey != 0 && CalenderKey == "0")
                    {
                        return GetCalendarGenerationbyBusinessKey(BusinessKey);
                    }
                    var result = db.GtEcclpis.Where(h => h.BusinessKey == BusinessKey && h.CalenderKey.ToUpper().Replace(" ", "") == CalenderKey.ToUpper().Replace(" ", ""))
                        .Join(db.GtEcclcos,
                     x => x.CalenderKey,
                     y => y.CalenderKey,

                     (x, y) => new DO_CalendarPatientIdGeneration
                     {

                         BusinessKey = x.BusinessKey,
                         CalenderKey = x.CalenderKey,
                         MonthId = x.MonthId,
                         EditMonthId = x.MonthId,
                         PatientIdgen = x.PatientIdgen,
                         PatientIdserial = x.PatientIdserial,
                         ActiveStatus = x.ActiveStatus,
                         Fromdate = y.FromDate,
                         Tilldate = y.TillDate,
                         Year = y.Year

                     }).ToList();

                    List<DO_CalendarPatientIdGeneration> Calendarpatientlist = new List<DO_CalendarPatientIdGeneration>();

                    foreach (var item in result)
                    {
                        DO_CalendarPatientIdGeneration obj = new DO_CalendarPatientIdGeneration();
                        obj.BusinessKey = item.BusinessKey;
                        obj.CalenderKey = item.CalenderKey;
                        obj.MonthId = item.MonthId;
                        obj.EditMonthId = item.MonthId;
                        obj.PatientIdgen = item.PatientIdgen;
                        obj.PatientIdserial = item.PatientIdserial;
                        obj.ActiveStatus = item.ActiveStatus;
                        obj.Fromdate = item.Fromdate;
                        obj.Tilldate = item.Tilldate;
                        obj.Year = item.Year;
                        String Monthlength = item.MonthId.ToString();
                        String firstletter = "";
                        if (!string.IsNullOrEmpty(Monthlength))
                        {
                            firstletter = Monthlength.Remove(0, 4);
                        }

                        if (firstletter == "01")
                        {
                            obj.MonthDescription = "January";


                        }
                        if (firstletter == "02")
                        {
                            obj.MonthDescription = "February";

                        }
                        if (firstletter == "03")
                        {
                            obj.MonthDescription = "March";

                        }
                        if (firstletter == "04")
                        {
                            obj.MonthDescription = "April";

                        }
                        if (firstletter == "05")
                        {
                            obj.MonthDescription = "May";

                        }
                        if (firstletter == "06")
                        {
                            obj.MonthDescription = "June";

                        }
                        if (firstletter == "07")
                        {
                            obj.MonthDescription = "July";

                        }
                        if (firstletter == "08")
                        {
                            obj.MonthDescription = "August";

                        }
                        if (firstletter == "09")
                        {
                            obj.MonthDescription = "September";

                        }
                        if (firstletter == "10")
                        {
                            obj.MonthDescription = "October";

                        }
                        if (firstletter == "11")
                        {
                            obj.MonthDescription = "November";

                        }
                        if (firstletter == "12")
                        {
                            obj.MonthDescription = "December";

                        }
                        Calendarpatientlist.Add(obj);
                    }


                    return Calendarpatientlist.ToList();


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DO_CalendarPatientIdGeneration> GetCalendarGenerationbyBusinessKey(int BusinessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                   
                    var result = db.GtEcclpis.Where(k => k.BusinessKey == BusinessKey).Join(db.GtEcclcos,
                     x => x.CalenderKey,
                     y => y.CalenderKey,
                     (x, y) => new { x, y })
                    .Select(
                     m=> new DO_CalendarPatientIdGeneration
                     {

                         BusinessKey = m.x.BusinessKey,
                         CalenderKey =m.x.CalenderKey,
                         MonthId = m.x.MonthId,
                         EditMonthId = m.x.MonthId,
                         PatientIdgen =m. x.PatientIdgen,
                         PatientIdserial = m.x.PatientIdserial,
                         ActiveStatus = m.x.ActiveStatus,
                         Fromdate = m.y.FromDate,
                         Tilldate = m.y.TillDate,
                         Year=m.y.Year


                     }).ToList();

                    List<DO_CalendarPatientIdGeneration> CalendarDtailslist = new List<DO_CalendarPatientIdGeneration>();

                    foreach (var item in result)
                    {
                        DO_CalendarPatientIdGeneration obj = new DO_CalendarPatientIdGeneration();
                        obj.BusinessKey = item.BusinessKey;
                        obj.CalenderKey = item.CalenderKey;
                        obj.Year = item.Year;
                        obj.MonthId = item.MonthId;
                        obj.EditMonthId = item.MonthId;
                        obj.PatientIdgen = item.PatientIdgen;
                        obj.PatientIdserial = item.PatientIdserial;
                        obj.ActiveStatus = item.ActiveStatus;
                        obj.Fromdate = item.Fromdate;
                        obj.Tilldate = item.Tilldate;
                        String Monthlength = item.MonthId.ToString();
                        String firstletter = "";
                        if (!string.IsNullOrEmpty(Monthlength))
                        {
                            firstletter = Monthlength.Remove(0, 4);
                        }
                        if (firstletter == "01")
                        {
                            obj.MonthDescription = "January";


                        }
                        if (firstletter == "02")
                        {
                            obj.MonthDescription = "February";

                        }
                        if (firstletter == "03")
                        {
                            obj.MonthDescription = "March";

                        }
                        if (firstletter == "04")
                        {
                            obj.MonthDescription = "April";

                        }
                        if (firstletter == "05")
                        {
                            obj.MonthDescription = "May";

                        }
                        if (firstletter == "06")
                        {
                            obj.MonthDescription = "June";

                        }
                        if (firstletter == "07")
                        {
                            obj.MonthDescription = "July";

                        }
                        if (firstletter == "08")
                        {
                            obj.MonthDescription = "August";

                        }
                        if (firstletter == "09")
                        {
                            obj.MonthDescription = "September";

                        }
                        if (firstletter == "10")
                        {
                            obj.MonthDescription = "October";

                        }
                        if (firstletter == "11")
                        {
                            obj.MonthDescription = "November";

                        }
                        if (firstletter == "12")
                        {
                            obj.MonthDescription = "December";

                        }
                        CalendarDtailslist.Add(obj);
                    }


                    return CalendarDtailslist.ToList();


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DO_ReturnParameter> UpdateCalendarGeneration(DO_CalendarPatientIdGeneration obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var calgen = db.GtEcclpis.Where(x => x.MonthId == obj.MonthId && x.BusinessKey == obj.BusinessKey && x.CalenderKey.ToUpper().Replace(" ", "") == obj.CalenderKey.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (calgen != null)
                        {
                            calgen.PatientIdgen = obj.PatientIdgen;
                            //calgen.PatientIdserial = obj.PatientIdserial;
                            calgen.ActiveStatus = obj.ActiveStatus;
                            calgen.ModifiedBy = obj.UserID;
                            calgen.ModifiedOn = DateTime.Now;
                            calgen.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00171", Message = string.Format(_localizer[name: "W00171"]) };
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

                    var ds =await db.GtEcfmfds.Where(x=>x.ActiveStatus==true)
                        .Join(db.GtEcfmpas.Where(x => x.ParameterId == 2),
                        f => f.FormId,
                        p => p.FormId,
                        (f, p) => new { f, p })
                      .Select( x=> new DO_Forms
                     {
                         FormID =x.f.FormId,
                         FormName = x.f.FormName,
                         FormCode = x.f.FormCode,
                         ActiveStatus = x.f.ActiveStatus
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

        #region  Document Link with Form
        public async Task<List<DO_FormDocumentLink>> GetActiveDocumentControls()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtDccnsts.Where(w => w.ActiveStatus == true).Select(x => new DO_FormDocumentLink
                    {
                        DocumentId=x.DocumentId,
                        DocumentName=x.DocumentDesc,
                    }).ToListAsync();
                    return ds;
                  

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_FormDocumentLink>> GetDocumentFormlink(int documentID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcfmfds.Where(x => x.ActiveStatus == true)
                        .Join(db.GtEcfmpas.Where(x => x.ParameterId == 2),
                        f => f.FormId,
                        p => p.FormId,
                        (f, p) => new { f, p })
                   .GroupJoin(db.GtDncnfds.Where(w => w.DocumentId == documentID),
                     d => d.f.FormId,
                     l => l.FormId,
                    (form, doc) => new { form, doc })
                   .SelectMany(z => z.doc.DefaultIfEmpty(),
                    (a, b) => new DO_FormDocumentLink
                    {
                        DocumentId = documentID,
                        FormId = a.form.f.FormId,
                        FormName = a.form.f.FormName,
                        ActiveStatus = b == null ? false : b.ActiveStatus
                    }).ToListAsync();

                    var Distinctform = ds.GroupBy(x => x.FormId).Select(y => y.First());
                    return Distinctform.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> UpdateDocumentFormlink(List<DO_FormDocumentLink> obj)
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
