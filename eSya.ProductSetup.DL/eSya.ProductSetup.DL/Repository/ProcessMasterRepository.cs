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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0065", Message = string.Format(_localizer[name: "W0065"]) };
                        }

                        var is_ProcessDescExist = db.GtEcprrls.Where(a => a.ProcessDesc.Trim().ToUpper().Replace(" ", "") == obj.ProcessDesc.Trim().ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (is_ProcessDescExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0066", Message = string.Format(_localizer[name: "W0066"]) };
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0066", Message = string.Format(_localizer[name: "W0066"]) };
                        }

                        GtEcprrl pr_ms = db.GtEcprrls.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                        if (pr_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0067", Message = string.Format(_localizer[name: "W0067"]) };
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0068", Message = string.Format(_localizer[name: "W0068"]) };
                        }
                        bool is_RuleIdExist = db.GtEcaprls.Any(a => a.RuleId == obj.RuleId && a.ProcessId == obj.ProcessId);
                        if (is_RuleIdExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0069", Message = string.Format(_localizer[name: "W0069"]) };
                        }
                        if (obj.ActiveStatus)
                        {
                            var pr = db.GtEcprrls.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                            if (pr.ProcessControl == "S")
                            {
                                if (db.GtEcaprls.Where(w => w.ProcessId == obj.ProcessId && w.ActiveStatus).Count() > 0)
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0070", Message = string.Format(_localizer[name: "W0070"]) };
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0068", Message = string.Format(_localizer[name: "W0068"]) };
                        }

                        GtEcaprl pr_ru = db.GtEcaprls.Where(w => w.RuleId == obj.RuleId && w.ProcessId == obj.ProcessId).FirstOrDefault();
                        if (pr_ru == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0071", Message = string.Format(_localizer[name: "W0071"]) };
                        }
                        if (obj.ActiveStatus)
                        {
                            var pr = db.GtEcprrls.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                            if (pr.ProcessControl == "S")
                            {
                                if (db.GtEcaprls.Where(w => w.ProcessId == obj.ProcessId && w.RuleId != obj.RuleId && w.ActiveStatus).Count() > 0)
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0070", Message = string.Format(_localizer[name: "W0070"]) };
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

        #region Process Rule by Business Location wise

        public async Task<List<DO_ProcessRule>> GetProcessRulebySegmentwise()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    return await db.GtEcaprls
                              .GroupJoin(db.GtEcaprbs,
                              a => new { a.RuleId, a.ProcessId },
                              f => new { f.RuleId, f.ProcessId },
                              (a, f) => new { a, f = f.FirstOrDefault() })
                              .Select(r => new DO_ProcessRule
                              {
                                  RuleId = r.a.RuleId,
                                  ProcessId = r.a.ProcessId,
                                  RuleDesc = r.a.RuleDesc,
                                  ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                              }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ProcessRule>> GetProcessRulebyBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    //var ds = db.GtEcprrls.Where(x => x.IsSegmentSpecific == true)
                    //    .Join(db.GtEcaprls,
                    //        x => new { x.ProcessId, },
                    //        y => new { y.ProcessId, },
                    //       (x, y) => new { x, y })
                    //    .GroupJoin(db.GtEcaprbs.Where(w => w.BusinessKey == BusinessKey),
                    //              a => new { a.y.RuleId, a.y.ProcessId },
                    //              f => new { f.RuleId, f.ProcessId },
                    //              (a, f) => new { a, f = f.FirstOrDefault() })
                    //    .Select(r => new DO_ProcessRule
                    //    {
                    //        RuleId = r.a.y.RuleId,
                    //        ProcessId = r.a.y.ProcessId,
                    //        ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                    //        RuleDesc = r.a.y.RuleDesc,
                    //    }).ToListAsync();

                    //return await ds;


                    var ds = await db.GtEcprrls.Where(x => x.IsSegmentSpecific == true)
                      .Join(db.GtEcaprls,
                      f => f.ProcessId,
                      p => p.ProcessId,
                      (f, p) => new { f, p })
                  .GroupJoin(db.GtEcaprbs.Where(w => w.BusinessKey == BusinessKey),
                    e =>new { e.p.RuleId,e.p.ProcessId },
                    d => new { d.RuleId,d.ProcessId },
                   (emp, depts) => new { emp, depts })
                  .SelectMany(z => z.depts.DefaultIfEmpty(),
                   (a, b) => new DO_ProcessRule
                   {
                       RuleId = a.emp.p.RuleId,
                       ProcessId = a.emp.p.ProcessId,
                       RuleDesc = a.emp.p.RuleDesc,
                       ActiveStatus = b == null ? false : b.ActiveStatus
                   }).ToListAsync();
                    return ds;

                    //var Distinctforms = ds.GroupBy(x => x.FormID).Select(y => y.First());
                    //return Distinctforms.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertorUpdateProcessRulebySegment(DO_ProcessRulebySegment obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var pr_ruleExists = db.GtEcaprbs.Where(x => x.BusinessKey == obj.BusinessKey && x.ProcessId == obj.ProcessId && x.RuleId == obj.RuleId).FirstOrDefault();
                        if (pr_ruleExists == null)
                        {
                            var pr_rule = new GtEcaprb
                            {
                                BusinessKey = obj.BusinessKey,
                                RuleId = obj.RuleId,
                                ProcessId = obj.ProcessId,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcaprbs.Add(pr_rule);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        else
                        {
                            pr_ruleExists.BusinessKey = obj.BusinessKey;
                            pr_ruleExists.ProcessId = obj.ProcessId;
                            pr_ruleExists.RuleId = obj.RuleId;
                            pr_ruleExists.ActiveStatus = obj.ActiveStatus;
                            pr_ruleExists.ModifiedBy = obj.UserID;
                            pr_ruleExists.ModifiedOn = System.DateTime.Now;
                            pr_ruleExists.ModifiedTerminal = obj.TerminalID;
                        }
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
