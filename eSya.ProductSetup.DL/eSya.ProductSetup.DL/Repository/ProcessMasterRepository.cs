using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DL.Repository
{
    public class ProcessMasterRepository : IProcessMasterRepository
    {
        private readonly IStringLocalizer<ProcessMasterRepository> _localizer;
        public ProcessMasterRepository(IStringLocalizer<ProcessMasterRepository> localizer)
        {
            _localizer = localizer;
        }
       
        #region Process Master

        public async Task<List<DO_ProcessMaster>> GetProcessMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcprrls
                        .Select(r => new DO_ProcessMaster
                        {
                            ProcessId = r.ProcessId,
                            ProcessDesc = r.ProcessDesc,
                            IsSegmentSpecific = r.IsSegmentSpecific,
                            SystemControl = r.SystemControl,
                            ProcessControl = r.ProcessControl,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoProcessMaster(DO_ProcessMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_ProcessIdExist = db.GtEcprrls.Any(a => a.ProcessId == obj.ProcessId);
                        if (is_ProcessIdExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0042", Message = string.Format(_localizer[name: "W0042"]) };
                        }

                        var is_ProcessDescExist = db.GtEcprrls.Where(a => a.ProcessDesc.Trim().ToUpper().Replace(" ", "") == obj.ProcessDesc.Trim().ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (is_ProcessDescExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0043", Message = string.Format(_localizer[name: "W0043"]) };
                        }

                        var pr_ms = new GtEcprrl
                        {
                            ProcessId = obj.ProcessId,
                            ProcessDesc = obj.ProcessDesc,
                            IsSegmentSpecific = obj.IsSegmentSpecific,
                            SystemControl = obj.SystemControl,
                            ProcessControl = obj.ProcessControl,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcprrls.Add(pr_ms);

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

        public async Task<DO_ReturnParameter> UpdateProcessMaster(DO_ProcessMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_ProcessDescExist = db.GtEcprrls.Where(w => w.ProcessDesc.Trim().ToUpper().Replace(" ", "") == obj.ProcessDesc.Trim().ToUpper().Replace(" ", "")
                                && w.ProcessId != obj.ProcessId).Count();
                        if (is_ProcessDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0043", Message = string.Format(_localizer[name: "W0043"]) };
                        }

                        GtEcprrl pr_ms = db.GtEcprrls.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                        if (pr_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0044", Message = string.Format(_localizer[name: "W0044"]) };
                        }

                        pr_ms.ProcessDesc = obj.ProcessDesc;
                        pr_ms.IsSegmentSpecific = obj.IsSegmentSpecific;
                        pr_ms.SystemControl = obj.SystemControl;
                        pr_ms.ProcessControl = obj.ProcessControl;
                        pr_ms.ActiveStatus = obj.ActiveStatus;
                        pr_ms.ModifiedBy = obj.UserID;
                        pr_ms.ModifiedOn = System.DateTime.Now;
                        pr_ms.ModifiedTerminal = obj.TerminalID;

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

        #endregion
       
        #region Process Rule

        public async Task<List<DO_ProcessRule>> GetProcessRule()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcaprls
                        .Select(r => new DO_ProcessRule
                        {
                            ProcessId = r.ProcessId,
                            RuleId = r.RuleId,
                            RuleDesc = r.RuleDesc,
                            Notes = r.Notes,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ProcessMaster>> GetActiveProcessMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcprrls.Where(w => w.ActiveStatus == true)
                        .Select(r => new DO_ProcessMaster
                        {
                            ProcessId = r.ProcessId,
                            ProcessDesc = r.ProcessDesc,

                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ProcessRule>> GetProcessRuleByProcessId(int processId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcaprls.Where(w => w.ProcessId == processId)
                        .Select(r => new DO_ProcessRule
                        {
                            ProcessId = r.ProcessId,
                            RuleId = r.RuleId,
                            RuleDesc = r.RuleDesc,
                            Notes = r.Notes,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoProcessRule(DO_ProcessRule obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_RuleDescExist = db.GtEcaprls.Where(w => w.RuleDesc.Trim().ToUpper().Replace(" ", "") == obj.RuleDesc.Trim().ToUpper().Replace(" ", "")
                                && w.ProcessId == obj.ProcessId).Count();
                        if (is_RuleDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0045", Message = string.Format(_localizer[name: "W0045"]) };
                        }
                        bool is_RuleIdExist = db.GtEcaprls.Any(a => a.RuleId == obj.RuleId && a.ProcessId == obj.ProcessId);
                        if (is_RuleIdExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0046", Message = string.Format(_localizer[name: "W0046"]) };
                        }
                        if (obj.ActiveStatus)
                        {
                            var pr = db.GtEcprrls.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                            if (pr.ProcessControl == "S")
                            {
                                if (db.GtEcaprls.Where(w => w.ProcessId == obj.ProcessId && w.ActiveStatus).Count() > 0)
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0047", Message = string.Format(_localizer[name: "W0047"]) };
                            }
                        }

                        var pr_ru = new GtEcaprl
                        {
                            RuleId = obj.RuleId,
                            ProcessId = obj.ProcessId,
                            RuleDesc = obj.RuleDesc,
                            Notes = obj.Notes,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcaprls.Add(pr_ru);

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

        public async Task<DO_ReturnParameter> UpdateProcessRule(DO_ProcessRule obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_RuleDescExist = db.GtEcaprls.Where(w => w.RuleDesc.ToUpper().Replace(" ", "") == obj.RuleDesc.ToUpper().Replace(" ", "")
                                && w.ProcessId != obj.ProcessId && w.RuleId != obj.RuleId).Count();
                        if (is_RuleDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0045", Message = string.Format(_localizer[name: "W0045"]) };
                        }

                        GtEcaprl pr_ru = db.GtEcaprls.Where(w => w.RuleId == obj.RuleId && w.ProcessId == obj.ProcessId).FirstOrDefault();
                        if (pr_ru == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0048", Message = string.Format(_localizer[name: "W0048"]) };
                        }
                        if (obj.ActiveStatus)
                        {
                            var pr = db.GtEcprrls.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                            if (pr.ProcessControl == "S")
                            {
                                if (db.GtEcaprls.Where(w => w.ProcessId == obj.ProcessId && w.RuleId != obj.RuleId && w.ActiveStatus).Count() > 0)
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0047", Message = string.Format(_localizer[name: "W0047"]) };
                            }
                        }

                        pr_ru.RuleDesc = obj.RuleDesc;
                        pr_ru.Notes = obj.Notes;
                        pr_ru.ActiveStatus = obj.ActiveStatus;
                        pr_ru.ModifiedBy = obj.UserID;
                        pr_ru.ModifiedOn = System.DateTime.Now;
                        pr_ru.ModifiedTerminal = obj.TerminalID;

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

        #endregion
        
    }
}
