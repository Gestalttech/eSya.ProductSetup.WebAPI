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
    public class NotificationRepository: INotificationRepository
    {
        private readonly IStringLocalizer<NotificationRepository> _localizer;
        public NotificationRepository(IStringLocalizer<NotificationRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Notification Trigger Event

        public async Task<List<DO_SMSTEvent>> GetAllSMSTriggerEvents()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsts
                         .Select(t => new DO_SMSTEvent
                         {
                             TEventID = t.TeventId,
                             TEventDesc = t.TeventDesc,
                             ActiveStatus = t.ActiveStatus
                         }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSMSTriggerEvent(DO_SMSTEvent obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_SMSTeventExist = db.GtEcsmsts.Any(a => a.TeventId == obj.TEventID);
                        if (is_SMSTeventExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0118", Message = string.Format(_localizer[name: "W0118"]) };
                        }

                        bool is_SMSTdescExist = db.GtEcsmsts.Any(a => a.TeventDesc.Trim().ToUpper() == obj.TEventDesc.Trim().ToUpper());
                        if (is_SMSTdescExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0119", Message = string.Format(_localizer[name: "W0119"]) };
                        }

                        var sm_tevnt = new GtEcsmst
                        {
                            TeventId = obj.TEventID,
                            TeventDesc = obj.TEventDesc,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsmsts.Add(sm_tevnt);

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

        public async Task<DO_ReturnParameter> UpdateSMSTriggerEvent(DO_SMSTEvent obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_SMSTEventExist = db.GtEcsmsts.Where(w => w.TeventDesc.Trim().ToUpper().Replace(" ", "") == obj.TEventDesc.Trim().ToUpper().Replace(" ", "")
                                && w.TeventId != obj.TEventID).FirstOrDefault();
                        if (is_SMSTEventExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0118", Message = string.Format(_localizer[name: "W0118"]) };
                        }

                        GtEcsmst sm_tevent = db.GtEcsmsts.Where(w => w.TeventId == obj.TEventID).FirstOrDefault();
                        if (sm_tevent == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0120", Message = string.Format(_localizer[name: "W0120"]) };
                        }

                        sm_tevent.TeventDesc = obj.TEventDesc;
                        sm_tevent.ActiveStatus = obj.ActiveStatus;
                        sm_tevent.ModifiedBy = obj.UserID;
                        sm_tevent.ModifiedOn = DateTime.Now;
                        sm_tevent.ModifiedTerminal = obj.TerminalID;
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

        public async Task<DO_ReturnParameter> DeleteSMSTriggerEvent(int TeventId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsmst sm_tevent = db.GtEcsmsts.Where(w => w.TeventId == TeventId).FirstOrDefault();

                        if (sm_tevent == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0120", Message = string.Format(_localizer[name: "W0120"]) };
                        }

                        db.GtEcsmsts.Remove(sm_tevent);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveSMSTriggerEvent(bool status, int TriggerEventId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsmst t_evevt = db.GtEcsmsts.Where(w => w.TeventId == TriggerEventId).FirstOrDefault();
                        if (t_evevt == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0120", Message = string.Format(_localizer[name: "W0120"]) };
                        }

                        t_evevt.ActiveStatus = status;
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
        #endregion SMS Trigger Event

        #region Notification Variables
        public async Task<List<DO_SMSVariable>> GetSMSVariableInformation()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsvs
                         .Select(r => new DO_SMSVariable
                         {
                             Smsvariable = r.Smsvariable,
                             Smscomponent = r.Smscomponent,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.Smsvariable).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_SMSVariable>> GetActiveSMSVariableInformation()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsvs
                        .Where(w => w.ActiveStatus)
                         .Select(r => new DO_SMSVariable
                         {
                             Smsvariable = r.Smsvariable,
                             Smscomponent = r.Smscomponent
                         }).OrderBy(o => o.Smsvariable).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSMSVariable(DO_SMSVariable obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_SMSVariableExist = db.GtEcsmsvs.Any(a => a.Smsvariable.Trim().ToUpper() == obj.Smsvariable.Trim().ToUpper());
                        if (is_SMSVariableExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0121", Message = string.Format(_localizer[name: "W0121"]) };
                        }

                        bool is_SMSComponentExist = db.GtEcsmsvs.Any(a => a.Smscomponent.Trim().ToUpper() == obj.Smscomponent.Trim().ToUpper());
                        if (is_SMSComponentExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0122", Message = string.Format(_localizer[name: "W0122"]) };
                        }

                        var sm_sv = new GtEcsmsv
                        {
                            Smsvariable = obj.Smsvariable,
                            Smscomponent = obj.Smscomponent,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsmsvs.Add(sm_sv);

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

        public async Task<DO_ReturnParameter> UpdateSMSVariable(DO_SMSVariable obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_SMSComponentExist = db.GtEcsmsvs.Where(w => w.Smscomponent.Trim().ToUpper().Replace(" ", "") == obj.Smscomponent.Trim().ToUpper().Replace(" ", "")
                                && w.Smsvariable != obj.Smsvariable).FirstOrDefault();
                        if (is_SMSComponentExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0122", Message = string.Format(_localizer[name: "W0122"]) };
                        }

                        GtEcsmsv sm_sv = db.GtEcsmsvs.Where(w => w.Smsvariable == obj.Smsvariable).FirstOrDefault();
                        if (sm_sv == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0126", Message = string.Format(_localizer[name: "W0126"]) };
                        }

                        sm_sv.Smscomponent = obj.Smscomponent;
                        sm_sv.ActiveStatus = obj.ActiveStatus;
                        sm_sv.ModifiedBy = obj.UserID;
                        sm_sv.ModifiedOn = DateTime.Now;
                        sm_sv.ModifiedTerminal = obj.TerminalID;
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveSMSVariable(bool status, string smsvariable)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsmsv sms_var = db.GtEcsmsvs.Where(w => w.Smsvariable.Trim().ToUpper().Replace(" ", "") == smsvariable.Trim().ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (sms_var == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0126", Message = string.Format(_localizer[name: "W0126"]) };
                        }

                        sms_var.ActiveStatus = status;
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
