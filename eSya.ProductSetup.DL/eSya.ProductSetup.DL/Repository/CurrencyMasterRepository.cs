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
    public class CurrencyMasterRepository: ICurrencyMasterRepository
    {
        private readonly IStringLocalizer<CurrencyMasterRepository> _localizer;
        public CurrencyMasterRepository(IStringLocalizer<CurrencyMasterRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Currency Master

        public async Task<List<DO_CurrencyMaster>> GetCurrencyMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccucos
                        .Select(r => new DO_CurrencyMaster
                        {
                            CurrencyCode = r.CurrencyCode,
                            CurrencyName = r.CurrencyName,
                            Symbol = r.Symbol,
                            DecimalPlaces = r.DecimalPlaces,
                            ShowInMillions = r.ShowInMillions,
                            SymbolSuffixToAmount = r.SymbolSuffixToAmount,
                            DecimalPortionWord = r.DecimalPortionWord,
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

        public async Task<List<DO_CurrencyMaster>> GetActiveCurrencyList()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtEccucos.Where(w => w.ActiveStatus)
                        .Select(c => new DO_CurrencyMaster
                        {
                            CurrencyCode = c.CurrencyCode,
                            CurrencyName = c.CurrencyName

                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_CurrencyMaster>> GetCurrencyListByCurrenyPrefix(string currencyPrefix)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccucos.Where(w => w.ActiveStatus && w.CurrencyName.Trim().ToLower().StartsWith(currencyPrefix.Trim().ToLower()))
                        .Select(r => new DO_CurrencyMaster
                        {
                            CurrencyCode = r.CurrencyCode,
                            CurrencyName = r.CurrencyName
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoCurrencyMaster(DO_CurrencyMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_CurrencyCodeExist = db.GtEccucos.Where(w => w.CurrencyCode == obj.CurrencyCode).Count();
                        if (is_CurrencyCodeExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0048", Message = string.Format(_localizer[name: "W0048"]) };
                        }

                        var is_CurrencyNameExist = db.GtEccucos.Where(w => w.CurrencyName.Trim().ToUpper() == obj.CurrencyName.Trim().ToUpper()).Count();
                        if (is_CurrencyNameExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0049", Message = string.Format(_localizer[name: "W0049"]) };
                        }
                        var cu_ms = new GtEccuco()
                        {
                            CurrencyCode = obj.CurrencyCode,
                            CurrencyName = obj.CurrencyName,
                            Symbol = obj.Symbol,
                            DecimalPlaces = obj.DecimalPlaces,
                            ShowInMillions = obj.ShowInMillions,
                            SymbolSuffixToAmount = obj.SymbolSuffixToAmount,
                            DecimalPortionWord = obj.DecimalPortionWord,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEccucos.Add(cu_ms);

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

        public async Task<DO_ReturnParameter> UpdateCurrencyMaster(DO_CurrencyMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_CurrencyNameExist = db.GtEccucos.Where(w => w.CurrencyName.Trim().ToUpper() == obj.CurrencyName.Trim().ToUpper() && w.CurrencyCode != obj.CurrencyCode).Count();
                        if (is_CurrencyNameExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0050", Message = string.Format(_localizer[name: "W0050"]) };
                        }

                        GtEccuco cu_ms = db.GtEccucos.Where(w => w.CurrencyCode == obj.CurrencyCode).FirstOrDefault();
                        if (cu_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0048", Message = string.Format(_localizer[name: "W0048"]) };
                        }

                        cu_ms.CurrencyName = obj.CurrencyName;
                        cu_ms.Symbol = obj.Symbol;
                        cu_ms.DecimalPlaces = obj.DecimalPlaces;
                        cu_ms.ShowInMillions = obj.ShowInMillions;
                        cu_ms.SymbolSuffixToAmount = obj.SymbolSuffixToAmount;
                        cu_ms.DecimalPortionWord = obj.DecimalPortionWord;
                        cu_ms.ActiveStatus = obj.ActiveStatus;
                        cu_ms.FormId = obj.FormId;
                        cu_ms.ModifiedBy = obj.UserID;
                        cu_ms.ModifiedOn = System.DateTime.Now;
                        cu_ms.ModifiedTerminal = obj.TerminalID;

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

        public async Task<DO_ReturnParameter> DeleteCurrencyMasterByCurrencyCode(string currencyCode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccuco cu_ms = db.GtEccucos.Where(w => w.CurrencyCode == currencyCode).FirstOrDefault();
                        if (cu_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0048", Message = string.Format(_localizer[name: "W0048"]) };
                        }

                        db.GtEccucos.Remove(cu_ms);

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveCurrencyMaster(bool status, string currency_code)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccuco cuurency_code = db.GtEccucos.Where(w => w.CurrencyCode.ToUpper().Replace(" ", "") == currency_code.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (cuurency_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0048", Message = string.Format(_localizer[name: "W0048"]) };
                        }

                        cuurency_code.ActiveStatus = status;
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

        #region Currency Denomination Information

        public async Task<List<DO_CurrencyDenominationInformation>> GetCurrencyDenominationInfoByCurrencyCode(string currencyCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccudns
                         .Where(w => w.CurrencyCode.ToUpper().Replace(" ", "") == currencyCode.ToUpper().Replace(" ", ""))
                         .Select(r => new DO_CurrencyDenominationInformation
                         {
                             CurrencyCode = r.CurrencyCode,
                             BnorCnId = r.BnorCnId,
                             DenomId = r.DenomId,
                             DenomDesc = r.DenomDesc,
                             DenomConversion = r.DenomConversion,
                             Sequence = r.Sequence,
                             EffectiveDate = r.EffectiveDate,
                             ActiveStatus = r.ActiveStatus

                         }).OrderBy(o => o.Sequence).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertCurrencyDenominationInformation(DO_CurrencyDenominationInformation obj)
        {

            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_DenominationExist = db.GtEccudns.Any(c => c.DenomId == obj.DenomId && c.CurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "") && c.BnorCnId == obj.BnorCnId);
                        if (is_DenominationExist)
                        {

                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0051", Message = string.Format(_localizer[name: "W0051"]) };
                        }
                        var is_SequenceExists = await db.GtEccudns.Where(x => x.CurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "") && x.Sequence == obj.Sequence).FirstOrDefaultAsync();
                        if (is_SequenceExists != null)
                        {
                            var seq_count = await db.GtEccudns.Where(x => x.CurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "") && x.Sequence >= obj.Sequence).OrderBy(x => x.Sequence).ToListAsync();
                            if (seq_count != null)
                            {
                                foreach (var item in seq_count)
                                {
                                    var cur_deseq = await db.GtEccudns.Where(y => y.CurrencyCode.ToUpper().Replace(" ", "") == item.CurrencyCode.ToUpper().Replace(" ", "") && y.Sequence == item.Sequence).FirstOrDefaultAsync();
                                    cur_deseq.Sequence = item.Sequence + 1;
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                        var cu_di = new GtEccudn()
                        {
                            CurrencyCode = obj.CurrencyCode,
                            BnorCnId = obj.BnorCnId,
                            DenomId = obj.DenomId,
                            DenomDesc = obj.DenomDesc,
                            DenomConversion = obj.DenomConversion,
                            Sequence = obj.Sequence,
                            EffectiveDate = obj.EffectiveDate,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEccudns.Add(cu_di);
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

        public async Task<DO_ReturnParameter> UpdateCurrencyDenominationInformation(DO_CurrencyDenominationInformation obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccudn cu_di = db.GtEccudns.Where(w => w.CurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "") && w.DenomId == obj.DenomId && w.BnorCnId == obj.BnorCnId).FirstOrDefault();
                        var is_SequenceExists = await db.GtEccudns.Where(x => x.CurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "") && x.Sequence == obj.Sequence && x.DenomId != obj.DenomId).FirstOrDefaultAsync();
                        if (is_SequenceExists != null)
                        {
                            var seq_count = await db.GtEccudns.Where(x => x.CurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "") && x.Sequence >= obj.Sequence && x.DenomId != obj.DenomId).OrderBy(x => x.Sequence).ToListAsync();
                            if (seq_count != null)
                            {
                                foreach (var item in seq_count)
                                {
                                    var cur_deseq = await db.GtEccudns.Where(y => y.CurrencyCode.ToUpper().Replace(" ", "") == item.CurrencyCode.ToUpper().Replace(" ", "") && y.Sequence == item.Sequence).FirstOrDefaultAsync();
                                    cur_deseq.Sequence = item.Sequence + 1;
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                        cu_di.DenomDesc = obj.DenomDesc;
                        cu_di.DenomConversion = obj.DenomConversion;
                        cu_di.Sequence = obj.Sequence;
                        cu_di.EffectiveDate = obj.EffectiveDate;
                        cu_di.ActiveStatus = obj.ActiveStatus;
                        cu_di.FormId = obj.FormId;
                        cu_di.ModifiedBy = obj.UserID;
                        cu_di.ModifiedOn = System.DateTime.Now;
                        cu_di.ModifiedTerminal = obj.TerminalID;
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

        public async Task<DO_ReturnParameter> DeleteCurrencyDenominationInformation(string currencyCode, decimal DenomId, string BnorCNId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccudn cu_di = db.GtEccudns.Where(w => w.CurrencyCode == currencyCode && w.DenomId == DenomId && w.BnorCnId == BnorCNId).FirstOrDefault();

                        if (cu_di == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0052", Message = string.Format(_localizer[name: "W0052"]) };
                        }

                        db.GtEccudns.Remove(cu_di);

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveCurrencyDenomination(bool status, string currencycode, int denomId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccudn cu_denomination = db.GtEccudns.Where(w => w.CurrencyCode.ToUpper().Replace(" ", "") == currencycode.ToUpper().Replace(" ", "") && w.DenomId == denomId).FirstOrDefault();
                        if (cu_denomination == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0052", Message = string.Format(_localizer[name: "W0052"]) };
                        }

                        cu_denomination.ActiveStatus = status;
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
