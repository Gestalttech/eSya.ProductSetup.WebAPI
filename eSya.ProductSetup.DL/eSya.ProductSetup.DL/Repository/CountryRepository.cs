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
    public class CountryRepository: ICountryRepository
    {
        private readonly IStringLocalizer<CountryRepository> _localizer;
        public CountryRepository(IStringLocalizer<CountryRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Country Codes
        public async Task<List<DO_CountryCodes>> GetAllCountryCodesAsync()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEccncds.Join(db.GtEccucos.Where(x=>x.ActiveStatus),
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_CountryCodes

                        {
                            Isdcode = x.Isdcode,
                            CountryCode = x.CountryCode,
                            CountryName = x.CountryName,
                            CountryFlag = x.CountryFlag,
                            CurrencyCode = x.CurrencyCode,
                            MobileNumberPattern = x.MobileNumberPattern,
                            //Uidlabel = x.Uidlabel,
                            //Uidpattern = x.Uidpattern,
                            Nationality = x.Nationality,
                            IsPoboxApplicable = x.IsPoboxApplicable,
                            PoboxPattern = x.PoboxPattern,
                            IsPinapplicable = x.IsPinapplicable,
                            PincodePattern = x.PincodePattern,
                            ActiveStatus = x.ActiveStatus,
                            CurrencyName = y.CurrencyName,
                            DateFormat=x.DateFormat,
                            ShortDateFormat=x.ShortDateFormat
                        }).ToListAsync();
                    List<DO_CountryCodes> countrycodes = new List<DO_CountryCodes>();
                    foreach (var item in await result)
                    {
                        DO_CountryCodes country = new DO_CountryCodes();
                        country.Isdcode = item.Isdcode;
                        country.CountryCode = item.CountryCode;
                        country.CountryName = item.CountryName;
                        country.CountryFlag = "/" + item.CountryFlag;
                        country.CurrencyCode = item.CurrencyCode;
                        country.MobileNumberPattern = item.MobileNumberPattern;
                        // country.Uidlabel = item.Uidlabel;
                        // country.Uidpattern = item.Uidpattern;
                        country.Nationality = item.Nationality;
                        country.IsPoboxApplicable = item.IsPoboxApplicable;
                        country.PoboxPattern = item.PoboxPattern;
                        country.IsPinapplicable = item.IsPinapplicable;
                        country.PincodePattern = item.PincodePattern;
                        country.ActiveStatus = item.ActiveStatus;
                        country.CurrencyName = item.CurrencyName;
                        country.DateFormat = item.DateFormat;
                        country.ShortDateFormat = item.ShortDateFormat;
                        countrycodes.Add(country);
                    }

                    return countrycodes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoCountryCode(DO_CountryCodes countrycode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_IsdCodeExist = db.GtEccncds.Any(c => c.Isdcode == countrycode.Isdcode);
                        if (is_IsdCodeExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0040", Message = string.Format(_localizer[name: "W0040"]) };
                        }

                        var is_CountryCodeExist = db.GtEccncds.Where(c => c.CountryCode.Trim().ToUpper().Replace(" ", "") == countrycode.CountryCode.Trim().ToUpper().Replace(" ", "")).Count();

                        if (is_CountryCodeExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0041", Message = string.Format(_localizer[name: "W0041"]) };
                        }
                        var is_CountryNameExist = db.GtEccncds.Where(c => c.CountryName.Trim().ToUpper().Replace(" ", "") == countrycode.CountryName.Trim().ToUpper().Replace(" ", "")).Count();

                        if (is_CountryNameExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0042", Message = string.Format(_localizer[name: "W0042"]) };
                        }
                        var ctr = new GtEccncd
                        {
                            Isdcode = countrycode.Isdcode,
                            CountryCode = countrycode.CountryCode,
                            CountryName = countrycode.CountryName,
                            CountryFlag = countrycode.CountryFlag,
                            CurrencyCode = countrycode.CurrencyCode,
                            MobileNumberPattern = countrycode.MobileNumberPattern,
                            //Uidlabel = countrycode.Uidlabel,
                            //Uidpattern = countrycode.Uidpattern,
                            Nationality = countrycode.Nationality,
                            IsPoboxApplicable = countrycode.IsPoboxApplicable,
                            PoboxPattern = countrycode.PoboxPattern,
                            IsPinapplicable = countrycode.IsPinapplicable,
                            PincodePattern = countrycode.PincodePattern,
                            ActiveStatus = countrycode.ActiveStatus,
                            FormId = countrycode.FormId,
                            CreatedBy = countrycode.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = countrycode.TerminalID,
                            DateFormat=countrycode.DateFormat,
                            ShortDateFormat=countrycode.ShortDateFormat
                        };
                        db.GtEccncds.Add(ctr);
                        await db.SaveChangesAsync();

                        if (countrycode._lstUIDpattern != null)
                        {
                            foreach (var p in countrycode._lstUIDpattern)
                            {
                                var isExits = db.GtEccnpis.Where(x => x.Isdcode == p.Isdcode && x.Uidlabel.Trim().ToUpper() == p.Uidlabel.Trim().ToUpper()).FirstOrDefault();
                                if (isExits == null)
                                {
                                    var uid = new GtEccnpi
                                    {
                                        Isdcode = p.Isdcode,
                                        Uidlabel = p.Uidlabel,
                                        Uidpattern = p.Uidpattern,
                                        ActiveStatus = p.ActiveStatus,
                                        FormId = p.FormId,
                                        CreatedBy = p.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = p.TerminalID
                                    };
                                    db.GtEccnpis.Add(uid);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }

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

        public async Task<DO_ReturnParameter> UpdateCountryCode(DO_CountryCodes countrycode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_CountryCodeExist = db.GtEccncds.Where(c => c.CountryCode.Trim().ToUpper().Replace(" ", "") == countrycode.CountryCode.Trim().ToUpper().Replace(" ", "")
                        && c.Isdcode != countrycode.Isdcode).Count();

                        if (is_CountryCodeExist > 0)
                        {

                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0041", Message = string.Format(_localizer[name: "W0041"]) };
                        }
                        var is_CountryNameExist = db.GtEccncds.Where(c => c.CountryName.Trim().ToUpper().Replace(" ", "") == countrycode.CountryName.Trim().ToUpper().Replace(" ", "")
                        && c.Isdcode != countrycode.Isdcode).Count();

                        if (is_CountryNameExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0042", Message = string.Format(_localizer[name: "W0042"]) };
                        }
                        GtEccncd ctr = db.GtEccncds.Where(x => x.Isdcode == countrycode.Isdcode).FirstOrDefault();


                        ctr.CountryCode = countrycode.CountryCode;
                        ctr.CountryName = countrycode.CountryName;
                        ctr.CountryFlag = countrycode.CountryFlag;
                        ctr.CurrencyCode = countrycode.CurrencyCode;
                        ctr.MobileNumberPattern = countrycode.MobileNumberPattern;
                        // ctr.Uidlabel = countrycode.Uidlabel;
                        // ctr.Uidpattern = countrycode.Uidpattern;
                        ctr.Nationality = countrycode.Nationality;
                        ctr.IsPoboxApplicable = countrycode.IsPoboxApplicable;
                        ctr.PoboxPattern = countrycode.PoboxPattern;
                        ctr.IsPinapplicable = countrycode.IsPinapplicable;
                        ctr.PincodePattern = countrycode.PincodePattern;
                        ctr.ActiveStatus = countrycode.ActiveStatus;
                        ctr.ModifiedBy = countrycode.UserID;
                        ctr.ModifiedOn = System.DateTime.Now;
                        ctr.ModifiedTerminal = countrycode.TerminalID;
                        ctr.ShortDateFormat = countrycode.ShortDateFormat;
                        ctr.DateFormat = countrycode.DateFormat;
                        await db.SaveChangesAsync();

                        if (countrycode._lstUIDpattern != null)
                        {
                            foreach (var p in countrycode._lstUIDpattern)
                            {
                                var uidpattern = db.GtEccnpis.Where(x => x.Isdcode == p.Isdcode && x.Uidlabel.Trim().ToUpper() == p.Uidlabel.Trim().ToUpper()).FirstOrDefault();
                                if (uidpattern != null)
                                {
                                    uidpattern.Isdcode = p.Isdcode;
                                    uidpattern.Uidlabel = p.Uidlabel;
                                    uidpattern.Uidpattern = p.Uidpattern;
                                    uidpattern.ActiveStatus = p.ActiveStatus;
                                    ctr.ModifiedBy = p.UserID;
                                    ctr.ModifiedOn = System.DateTime.Now;
                                    ctr.ModifiedTerminal = p.TerminalID;
                                    await db.SaveChangesAsync();
                                }
                                else
                                {
                                    var uid = new GtEccnpi
                                    {
                                        Isdcode = p.Isdcode,
                                        Uidlabel = p.Uidlabel,
                                        Uidpattern = p.Uidpattern,
                                        ActiveStatus = p.ActiveStatus,
                                        FormId = p.FormId,
                                        CreatedBy = p.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = p.TerminalID
                                    };
                                    db.GtEccnpis.Add(uid);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }

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

        public async Task<DO_CountryCodes> GetCurrencyNamebyIsdCode(int IsdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var Currency = db.GtEccncds.Where(c => c.Isdcode == IsdCode).Join(db.GtEccucos,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_CountryCodes
                        {
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = y.CurrencyName
                        }).FirstOrDefaultAsync();


                    return await Currency;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveCountryCode(bool status, int Isd_code)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccncd isd_code = db.GtEccncds.Where(w => w.Isdcode == Isd_code).FirstOrDefault();
                        if (isd_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0043", Message = string.Format(_localizer[name: "W0043"]) };
                        }

                        isd_code.ActiveStatus = status;
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

        public async Task<List<DO_UIDPattern>> GetUIDPatternbyIsdcode(int Isdcode)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var Uidpatterns = db.GtEccnpis.Where(x => x.Isdcode == Isdcode)

                                  .Select(l => new DO_UIDPattern
                                  {
                                      Isdcode = l.Isdcode,
                                      Uidlabel = l.Uidlabel,
                                      Uidpattern = l.Uidpattern,
                                      ActiveStatus = l.ActiveStatus
                                  }).ToListAsync();
                    return await Uidpatterns;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Country Codes

        #region Statutory Details

        public async Task<List<DO_eSyaParameter>> GetStatutoryCodesParameterList(int IsdCode, int StatutoryCode)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = db.GtEcsupas
                        .Where(s => s.StatutoryCode == StatutoryCode && s.Isdcode == IsdCode)
                        .Select(p => new DO_eSyaParameter
                        {
                            ParameterID = p.ParameterId,
                            ParmAction = p.Action
                        }).ToListAsync();
                    return await ds;


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_CountryStatutoryDetails>> GetStatutoryCodesbyIsdcode(int Isdcode)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var statutorycodes = db.GtEccnsds.Where(x => x.Isdcode == Isdcode)

                                  .Select(st => new DO_CountryStatutoryDetails
                                  {
                                      Isdcode = st.Isdcode,
                                      StatutoryCode = st.StatutoryCode,
                                      StatShortCode = st.StatShortCode,
                                      StatutoryDescription = st.StatutoryDescription,
                                      StatPattern = st.StatPattern,
                                      ActiveStatus = st.ActiveStatus
                                  }).ToListAsync();
                    return await statutorycodes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateStatutoryCodes(DO_CountryStatutoryDetails obj)
        {
            try
            {
                if (obj.StatutoryCode != 0)
                {
                    return await UpdateStatutoryCodes(obj);
                }
                else
                {
                    return await InsertStatutoryCodes(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertStatutoryCodes(DO_CountryStatutoryDetails obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnsd isStatshortcodeExists = db.GtEccnsds.FirstOrDefault(st => st.Isdcode == obj.Isdcode && st.StatShortCode.ToUpper().Replace(" ", "") == obj.StatShortCode.ToUpper().Replace(" ", ""));

                        if (isStatshortcodeExists != null)
                        {
                            
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0044", Message = string.Format(_localizer[name: "W0044"]) };
                        }

                        int maxval = db.GtEccnsds.Where(x => x.Isdcode == obj.Isdcode).Select(c => c.StatutoryCode).DefaultIfEmpty().Max();
                        int statutorycode_ = maxval + 1;
                        var stat_code = new GtEccnsd
                        {
                            Isdcode = obj.Isdcode,
                            StatutoryCode = statutorycode_,
                            StatShortCode = obj.StatShortCode,
                            StatutoryDescription = obj.StatutoryDescription,
                            StatPattern = obj.StatPattern,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEccnsds.Add(stat_code);
                        List<GtEcsupa> stparam = db.GtEcsupas.Where(p => p.StatutoryCode == obj.StatutoryCode && p.Isdcode == obj.Isdcode).ToList();
                        if (obj.l_FormParameter != null)
                        {
                            if (stparam.Count > 0)
                            {
                                foreach (var p in stparam)
                                {
                                    db.GtEcsupas.Remove(p);
                                    db.SaveChanges();
                                }

                            }
                            foreach (var param in obj.l_FormParameter)
                            {
                                GtEcsupa objparam = new GtEcsupa
                                {
                                    StatutoryCode = statutorycode_,
                                    Isdcode = obj.Isdcode,
                                    ParameterId = param.ParameterID,
                                    Action = param.ParmAction,
                                    ActiveStatus = param.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,

                                };
                                db.GtEcsupas.Add(objparam);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0045", Message = string.Format(_localizer[name: "W0045"]) };
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

        public async Task<DO_ReturnParameter> UpdateStatutoryCodes(DO_CountryStatutoryDetails obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnsd isStatshortcodeExists = db.GtEccnsds.FirstOrDefault(st => st.Isdcode == obj.Isdcode && st.StatShortCode.ToUpper().Replace(" ", "") == obj.StatShortCode.ToUpper().Replace(" ", "")
                        && st.StatutoryCode != obj.StatutoryCode);

                        if (isStatshortcodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0044", Message = string.Format(_localizer[name: "W0044"]) };
                        }


                        GtEccnsd stat_code = db.GtEccnsds.Where(st => st.StatutoryCode == obj.StatutoryCode && st.Isdcode == obj.Isdcode).FirstOrDefault();
                        if (stat_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0046", Message = string.Format(_localizer[name: "W0046"]) };
                        }

                        stat_code.StatShortCode = obj.StatShortCode;
                        stat_code.StatutoryDescription = obj.StatutoryDescription;
                        stat_code.StatPattern = obj.StatPattern;
                        stat_code.ActiveStatus = obj.ActiveStatus;
                        stat_code.ModifiedBy = obj.UserID;
                        stat_code.ModifiedOn = DateTime.Now;
                        stat_code.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        List<GtEcsupa> stparam = db.GtEcsupas.Where(p => p.StatutoryCode == obj.StatutoryCode && p.Isdcode == obj.Isdcode).ToList();
                        if (obj.l_FormParameter != null)
                        {
                            if (stparam.Count > 0)
                            {
                                foreach (var p in stparam)
                                {
                                    db.GtEcsupas.Remove(p);
                                    db.SaveChanges();
                                }

                            }
                            foreach (var param in obj.l_FormParameter)
                            {
                                GtEcsupa objparam = new GtEcsupa
                                {
                                    StatutoryCode = obj.StatutoryCode,
                                    Isdcode = obj.Isdcode,
                                    ParameterId = param.ParameterID,
                                    Action = param.ParmAction,
                                    ActiveStatus = param.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,

                                };
                                db.GtEcsupas.Add(objparam);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0047", Message = string.Format(_localizer[name: "W0047"]) };
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

        public async Task<List<DO_CountryStatutoryDetails>> GetActiveStatutoryCodes()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var statutorycodes = db.GtEccnsds.Where(x => x.ActiveStatus == true)

                                  .Select(st => new DO_CountryStatutoryDetails
                                  {
                                      StatutoryCode = st.StatutoryCode,
                                      StatShortCode = st.StatShortCode,
                                      StatutoryDescription = st.StatutoryDescription
                                  }).ToListAsync();
                    return await statutorycodes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveStatutoryCode(bool status, int Isd_code, int statutorycode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnsd statutory = db.GtEccnsds.Where(w => w.Isdcode == Isd_code && w.StatutoryCode == statutorycode).FirstOrDefault();
                        if (statutory == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0046", Message = string.Format(_localizer[name: "W0046"]) };
                        }

                        statutory.ActiveStatus = status;
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
        #endregion Statutory Details
    }
}
