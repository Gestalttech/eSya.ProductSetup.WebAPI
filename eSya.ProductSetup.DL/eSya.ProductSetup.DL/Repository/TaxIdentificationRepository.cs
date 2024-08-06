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
    public class TaxIdentificationRepository: ITaxIdentificationRepository
    {
        private readonly IStringLocalizer<TaxIdentificationRepository> _localizer;
        public TaxIdentificationRepository(IStringLocalizer<TaxIdentificationRepository> localizer)
        {
            _localizer = localizer;
        }
        
        #region Tax Identification
        public async Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntis.Where(w => w.Isdcode == ISDCode)
                        .Select(x => new DO_TaxIdentification
                        {
                            Isdcode = x.Isdcode,
                            TaxIdentificationId = x.TaxIdentificationId,
                            TaxIdentificationDesc = x.TaxIdentificationDesc,
                            StateCode = x.StateCode,
                            IsUt=x.IsUt,
                            ActiveStatus = x.ActiveStatus
                        }).OrderBy(o => o.TaxIdentificationId).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoTaxIdentification(DO_TaxIdentification obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isTaxDescriptionExist = db.GtEccntis.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationDesc == obj.TaxIdentificationDesc).Count();
                        if (_isTaxDescriptionExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0049", Message = string.Format(_localizer[name: "W0049"]) };
                        }

                        var _isTaxIdentificationExist = db.GtEccntis.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationId == obj.TaxIdentificationId).Count();
                        if (_isTaxIdentificationExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0050", Message = string.Format(_localizer[name: "W0050"]) };
                        }
                        var _isTaxstatecodeExist = db.GtEccntis.Where(w => w.Isdcode == obj.Isdcode && w.StateCode.ToUpper().Replace(" ", "") == obj.StateCode.ToUpper().Replace(" ", "")).Count();
                        if (_isTaxstatecodeExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0051", Message = string.Format(_localizer[name: "W0051"]) };
                        }

                        //int _taxIdnId = 0;
                        //_taxIdnId = db.GtEccnti.Select(c => c.TaxIdentificationId).DefaultIfEmpty().Max();
                        //_taxIdnId = _taxIdnId + 1;


                        var ap_cd = new GtEccnti
                        {
                            Isdcode = obj.Isdcode,
                            TaxIdentificationId = obj.TaxIdentificationId,
                            TaxIdentificationDesc = obj.TaxIdentificationDesc.Trim(),
                            StateCode = obj.StateCode,
                            IsUt=obj.IsUt,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID,

                        };
                        db.GtEccntis.Add(ap_cd);

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

        public async Task<DO_ReturnParameter> UpdateTaxIdentification(DO_TaxIdentification obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnti ap_cd = db.GtEccntis.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationId == obj.TaxIdentificationId).FirstOrDefault();
                        if (ap_cd == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0052", Message = string.Format(_localizer[name: "W0052"]) };
                        }

                        GtEccnti ap_cd1 = db.GtEccntis.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationDesc.ToUpper().Replace(" ", "") == obj.TaxIdentificationDesc.ToUpper().Replace(" ", "")
                         && w.TaxIdentificationId != obj.TaxIdentificationId).FirstOrDefault();
                        if (ap_cd1 != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0049", Message = string.Format(_localizer[name: "W0049"]) };
                        }

                        GtEccnti statecode = db.GtEccntis.Where(w => w.Isdcode == obj.Isdcode && w.StateCode.ToUpper().Replace(" ", "") == obj.StateCode.ToUpper().Replace(" ", "")
                         && w.TaxIdentificationId != obj.TaxIdentificationId).FirstOrDefault();
                        if (statecode != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0053", Message = string.Format(_localizer[name: "W0053"]) };
                        }
                        //IEnumerable<GtEccnti> ls_apct = db.GtEccnti.Where(w => w.Isdcode == obj.Isdcode).ToList();

                        //var is_SameDescExists = ls_apct.Where(w => w.TaxIdentificationDesc.ToUpper().Replace(" ", "") == obj.TaxIdentificationDesc.ToUpper().Replace(" ", "")
                        //        && w.TaxIdentificationId != obj.TaxIdentificationId).Count();
                        //if (is_SameDescExists > 0)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Tax Identification Description is already Exists" };
                        //}

                        ap_cd.TaxIdentificationId = obj.TaxIdentificationId;
                        ap_cd.TaxIdentificationDesc = obj.TaxIdentificationDesc.Trim();
                        ap_cd.StateCode = obj.StateCode;
                        ap_cd.IsUt=obj.IsUt;
                        ap_cd.ActiveStatus = obj.ActiveStatus;
                        ap_cd.ModifiedBy = obj.UserID;
                        ap_cd.ModifiedOn = System.DateTime.Now;
                        ap_cd.ModifiedTerminal = obj.TerminalID;

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveTaxIdentification(bool status, int Isd_code, int TaxIdentificationId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnti tax_identification = db.GtEccntis.Where(w => w.Isdcode == Isd_code && w.TaxIdentificationId == TaxIdentificationId).FirstOrDefault();
                        if (tax_identification == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0052", Message = string.Format(_localizer[name: "W0052"]) };
                        }

                        tax_identification.ActiveStatus = status;
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
