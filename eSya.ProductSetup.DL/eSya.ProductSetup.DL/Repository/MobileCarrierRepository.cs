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
    public class MobileCarrierRepository: IMobileCarrierRepository
    {
        private readonly IStringLocalizer<MobileCarrierRepository> _localizer;
        public MobileCarrierRepository(IStringLocalizer<MobileCarrierRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Mobile Carrier
        public async Task<List<DO_MobileCarrier>> GetMobileCarriers()
        {
            using (var db = new eSyaEnterprise())
            {
                var age = db.GtEccnmcs
               .Select(s => new DO_MobileCarrier
               {
                   Isdcode = s.Isdcode,
                   MobilePrefix = s.MobilePrefix,
                   MobileNoDigit = s.MobileNoDigit,
                   ActiveStatus = s.ActiveStatus
               })
               .ToListAsync();
                return await age;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateMobileCarrier(DO_MobileCarrier obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _mobcari = db.GtEccnmcs.Where(w => w.Isdcode == obj.Isdcode && w.MobilePrefix.ToUpper().Replace(" ", "") == obj.MobilePrefix.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_mobcari != null)
                        {
                            _mobcari.MobileNoDigit = obj.MobileNoDigit;
                            _mobcari.ActiveStatus = obj.ActiveStatus;
                            _mobcari.ModifiedBy = obj.UserID;
                            _mobcari.ModifiedOn = System.DateTime.Now;
                            _mobcari.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                          
                            var _mcarier = new GtEccnmc
                            {
                                Isdcode = obj.Isdcode,
                                MobilePrefix = obj.MobilePrefix,
                                MobileNoDigit = obj.MobileNoDigit,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEccnmcs.Add(_mcarier);
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveMobileCarrier(bool status,int ISDCode, string MobilePrefix)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _mobcari = db.GtEccnmcs.Where(w => w.Isdcode == ISDCode && w.MobilePrefix.ToUpper().Replace(" ", "") == MobilePrefix.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_mobcari == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0184", Message = string.Format(_localizer[name: "W0184"]) };
                        }

                        _mobcari.ActiveStatus = status;
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
