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
    public class GatewayRepository: IGatewayRepository
    {
        private readonly IStringLocalizer<GatewayRepository> _localizer;
        public GatewayRepository(IStringLocalizer<GatewayRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Gate way Rules
        public async Task<List<DO_GatewayRules>> GetGatewayRules()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcgwrls
                        .Select(r => new DO_GatewayRules
                        {
                            GwruleId = r.GwruleId,
                            Gwdesc = r.Gwdesc,
                            RuleValue=r.RuleValue,
                            ActiveStatus=r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateGatewayRules(DO_GatewayRules obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _grules = db.GtEcgwrls.Where(w => w.GwruleId == obj.GwruleId).FirstOrDefault();
                        if (_grules != null)
                        {
                            _grules.Gwdesc = obj.Gwdesc;
                            _grules.RuleValue = obj.RuleValue;
                            _grules.ActiveStatus = obj.ActiveStatus;
                            _grules.ModifiedBy = obj.UserID;
                            _grules.ModifiedOn = System.DateTime.Now;
                            _grules.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            int max_gwRuleId = db.GtEcgwrls.Select(c => c.GwruleId).DefaultIfEmpty().Max();
                            int _ruleId = max_gwRuleId + 1;
                            var _gwrules = new GtEcgwrl
                            {
                                GwruleId = _ruleId,
                                Gwdesc = obj.Gwdesc,
                                RuleValue = obj.RuleValue,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcgwrls.Add(_gwrules);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
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
        public async Task<DO_ReturnParameter> ActiveOrDeActiveGatewayRules(bool status, int GwRuleId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcgwrl gwRule = db.GtEcgwrls.Where(w => w.GwruleId == GwRuleId).FirstOrDefault();
                        if (gwRule == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0111", Message = string.Format(_localizer[name: "W0111"]) };
                        }

                        gwRule.ActiveStatus = status;
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
