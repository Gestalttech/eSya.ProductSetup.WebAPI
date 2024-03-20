using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DL.Repository
{
    public class eSyaCultureRepository: IeSyaCultureRepository
    {
        private readonly IStringLocalizer<eSyaCultureRepository> _localizer;
        public eSyaCultureRepository(IStringLocalizer<eSyaCultureRepository> localizer)
        {
            _localizer = localizer;
        }
       
        #region define eSya Culture 
        public async Task<List<DO_eSyaCulture>> GetAlleSyaCultures()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEbeculs
                        .Select(a => new DO_eSyaCulture
                        {
                            CultureCode  = a.CultureCode,
                            CultureDesc = a.CultureDesc,
                            ActiveStatus = a.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateIntoeSyaCultures(DO_eSyaCulture obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var esya_cul = db.GtEbeculs.Where(a => a.CultureCode.ToUpper().Replace(" ", "") == obj.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                       
                        if (esya_cul==null)
                        {
                            bool is_ctrlExist = db.GtEbeculs.Any(a => a.CultureDesc.ToUpper().Replace(" ", "") == obj.CultureDesc.ToUpper().Replace(" ", ""));
                            if (is_ctrlExist)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0030", Message = string.Format(_localizer[name: "W0030"]) };
                            }
                            var _ctrl = new GtEbecul
                            {
                                CultureCode=obj.CultureCode,
                                CultureDesc=obj.CultureDesc,
                                ActiveStatus = obj.ActiveStatus,
                                FormId=obj.FormID,
                                CreatedBy=obj.UserID,
                                CreatedOn=System.DateTime.Now,
                                CreatedTerminal= obj.TerminalID
                            };
                            db.GtEbeculs.Add(_ctrl);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

                        else
                        {
                            GtEbecul ctrl = db.GtEbeculs.Where(a => a.CultureCode.ToUpper().Replace(" ", "") == obj.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                            
                            if (ctrl == null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0031", Message = string.Format(_localizer[name: "W0031"]) };
                            }

                            bool is_ctrlExist = db.GtEbeculs.Any(a => a.CultureCode.ToUpper().Trim().Replace(" ", "") != obj.CultureCode.ToUpper().Trim().Replace(" ", "") && a.CultureDesc.ToUpper().Trim().Replace(" ", "") == obj.CultureDesc.ToUpper().Trim().Replace(" ", ""));
                            if (is_ctrlExist)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0030", Message = string.Format(_localizer[name: "W0030"]) };
                            }

                            ctrl.CultureDesc = obj.CultureDesc;
                            ctrl.ActiveStatus = obj.ActiveStatus;
                            ctrl.ModifiedBy = obj.UserID;
                            ctrl.ModifiedOn = System.DateTime.Now;
                            ctrl.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveeSyaCultures(bool status, string esyaculture)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEbecul ctrl = db.GtEbeculs.Where(a => a.CultureCode.ToUpper().Replace(" ", "") == esyaculture.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (ctrl == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0031", Message = string.Format(_localizer[name: "W0031"]) };
                        }

                        ctrl.ActiveStatus = status;
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
