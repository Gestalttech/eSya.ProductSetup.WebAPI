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
    public class LicenseRepository: ILicenseRepository
    {
        private readonly IStringLocalizer<LicenseRepository> _localizer;
        public LicenseRepository(IStringLocalizer<LicenseRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Business Entity
        public async Task<List<DO_BusinessEntity>> GetBusinessEntities()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsens
                                   .Where(w => w.ActiveStatus)
                                  .Select(be => new DO_BusinessEntity
                                  {
                                      BusinessId = be.BusinessId,
                                      BusinessDesc = be.BusinessId.ToString() + " - " + be.BusinessDesc,
                                      BusinessUnitType = be.BusinessUnitType,
                                      NoOfUnits = be.NoOfUnits,
                                      ActiveNoOfUnits = be.ActiveNoOfUnits,
                                      UsageStatus = be.UsageStatus,
                                      ActiveStatus = be.ActiveStatus
                                  }).OrderBy(b => b.BusinessId).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessEntity> GetBusinessEntityInfo(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbsens
                        .Where(w => w.BusinessId == BusinessId)
                        .Select(r => new DO_BusinessEntity
                        {
                            BusinessId = r.BusinessId,
                            BusinessDesc = r.BusinessDesc,
                            BusinessUnitType = r.BusinessUnitType,
                            NoOfUnits = r.NoOfUnits,
                            ActiveNoOfUnits = r.ActiveNoOfUnits,
                            UsageStatus = r.UsageStatus,
                            ActiveStatus = r.ActiveStatus
                        }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertBusinessEntity(DO_BusinessEntity obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen is_EntityDescExists = db.GtEcbsens.FirstOrDefault(u => u.BusinessDesc.ToUpper().Replace(" ", "") == obj.BusinessDesc.ToUpper().Replace(" ", ""));
                        if (is_EntityDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0022", Message = string.Format(_localizer[name: "W0022"]) };
                        }

                        int _businessID = db.GtEcbsens.Select(c => c.BusinessId).DefaultIfEmpty().Max();
                        _businessID = _businessID + 1;

                        var b_Entity = new GtEcbsen
                        {
                            BusinessId = _businessID,
                            BusinessDesc = obj.BusinessDesc,
                            BusinessUnitType = obj.BusinessUnitType,
                            NoOfUnits = obj.NoOfUnits,
                            ActiveNoOfUnits = obj.ActiveNoOfUnits,
                            UsageStatus = false,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcbsens.Add(b_Entity);
                        await db.SaveChangesAsync();

                        if (obj.l_Preferredlang != null)
                        {
                            foreach (var pl in obj.l_Preferredlang)
                            {
                                var plExist = db.GtEcbspls.Where(w => w.BusinessId == _businessID && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                                if (plExist != null)
                                {
                                    db.GtEcbspls.Remove(plExist);
                                    await db.SaveChangesAsync();
                                }
                            }
                            foreach (var _pl in obj.l_Preferredlang)
                            {
                                //if (_pl.ActiveStatus == true)
                                //{
                                    var plang = new GtEcbspl
                                    {
                                        BusinessId = _businessID,
                                        PreferredLanguage = _pl.CultureCode,
                                        Pldesc=_pl.Pldesc,
                                        ActiveStatus = _pl.ActiveStatus,
                                        FormId = _pl.FormID,
                                        CreatedBy = _pl.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _pl.TerminalID
                                    };
                                    db.GtEcbspls.Add(plang);
                                    await db.SaveChangesAsync();
                                //}
                            }
                            //await db.SaveChangesAsync();
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

        public async Task<DO_ReturnParameter> UpdateBusinessEntity(DO_BusinessEntity obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen is_EntityExists = db.GtEcbsens.FirstOrDefault(be => be.BusinessDesc.ToUpper().Replace(" ", "") == obj.BusinessDesc.ToUpper().Replace(" ", "") && be.BusinessId != obj.BusinessId);
                        if (is_EntityExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0022", Message = string.Format(_localizer[name: "W0022"]) };

                        }


                        GtEcbsen b_Entity = db.GtEcbsens.Where(be => be.BusinessId == obj.BusinessId).FirstOrDefault();
                        if (b_Entity != null)
                        {
                            b_Entity.BusinessDesc = obj.BusinessDesc;
                            b_Entity.BusinessUnitType = obj.BusinessUnitType;
                            b_Entity.NoOfUnits = obj.NoOfUnits;
                            b_Entity.ActiveNoOfUnits = obj.ActiveNoOfUnits;
                            b_Entity.ActiveStatus = obj.ActiveStatus;
                            b_Entity.ModifiedBy = obj.UserID;
                            b_Entity.ModifiedOn = System.DateTime.Now;
                            b_Entity.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();

                            if (obj.l_Preferredlang != null)
                            {
                                foreach (var pl in obj.l_Preferredlang)
                                {
                                    var plExist = db.GtEcbspls.Where(w => w.BusinessId == pl.BusinessId && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                                    if (plExist != null)
                                    {
                                        db.GtEcbspls.Remove(plExist);
                                        await db.SaveChangesAsync();
                                    }
                                }
                                foreach (var _pl in obj.l_Preferredlang)
                                {
                                    //if (_pl.ActiveStatus == true)
                                    //{
                                        var plang = new GtEcbspl
                                        {
                                            BusinessId = _pl.BusinessId,
                                            PreferredLanguage = _pl.CultureCode,
                                            Pldesc = _pl.Pldesc,
                                            ActiveStatus = _pl.ActiveStatus,
                                            FormId = _pl.FormID,
                                            CreatedBy = _pl.UserID,
                                            CreatedOn = System.DateTime.Now,
                                            CreatedTerminal = _pl.TerminalID
                                        };
                                        db.GtEcbspls.Add(plang);
                                        await db.SaveChangesAsync();
                                    //}
                                }
                                //await db.SaveChangesAsync();
                            }


                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0023", Message = string.Format(_localizer[name: "W0023"]) };

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

        public async Task<DO_ReturnParameter> DeleteBusinessEntity(int BusinessEntityId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen bu_en = db.GtEcbsens.Where(w => w.BusinessId == BusinessEntityId).FirstOrDefault();
                        if (bu_en == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0023", Message = string.Format(_localizer[name: "W0023"]) };
                        }

                        if (bu_en.UsageStatus == true)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0024", Message = string.Format(_localizer[name: "W0024"]) };

                        }

                        db.GtEcbsens.Remove(bu_en);

                        await db.SaveChangesAsync();


                        var plist = db.GtEcbspls.Where(x => x.BusinessId == BusinessEntityId).ToList();
                        if (plist != null)
                        {
                            foreach (var pl in plist)
                            {
                                var plExist = db.GtEcbspls.Where(w => w.BusinessId == pl.BusinessId && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.PreferredLanguage.ToUpper().Replace(" ", "")).FirstOrDefault();
                                if (plExist != null)
                                {
                                    db.GtEcbspls.Remove(plExist);
                                    await db.SaveChangesAsync();
                                }
                            }
                           
                        }


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

        public async Task<List<DO_BusinessEntity>> GetActiveBusinessEntities()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsens.Where(x => x.ActiveStatus == true)

                                  .Select(be => new DO_BusinessEntity
                                  {
                                      BusinessId = be.BusinessId,
                                      BusinessDesc = be.BusinessDesc

                                  }).OrderBy(b => b.BusinessId).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_EntityPreferredLanguage>> GetPreferredLanguagebyBusinessKey(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEbeculs.Where(x=>x.ActiveStatus)
                        .Select(r => new DO_EntityPreferredLanguage
                        {
                            BusinessId= BusinessId,
                            CultureCode = r.CultureCode,
                            CultureDesc = r.CultureDesc,
                            ActiveStatus = false
                        }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcbspl cul = db.GtEcbspls.Where(x => x.BusinessId == BusinessId && x.PreferredLanguage.ToUpper().Replace(" ", "") == obj.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (cul != null)
                        {
                            obj.ActiveStatus = cul.ActiveStatus;
                            obj.Pldesc = cul.Pldesc;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                            obj.Pldesc = string.Empty;
                        }
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion  Business Entity

        #region Business Subscription
        public async Task<List<DO_BusinessSubscription>> GetBusinessSubscription(int BusinessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbssus.Where(bs => bs.BusinessKey == BusinessKey)
                                  .Select(be => new DO_BusinessSubscription
                                  {
                                      SubscribedFrom = be.SubscribedFrom,
                                      SubscribedTill = be.SubscribedTill,
                                      ActiveStatus = be.ActiveStatus
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateBusinessSubscription(DO_BusinessSubscription businessSubscription)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (businessSubscription.isEdit == 0)
                        {
                            GtEcbssu is_SubsFrmDateExist = db.GtEcbssus.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedFrom >= businessSubscription.SubscribedFrom);
                            if (is_SubsFrmDateExist != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0025", Message = string.Format(_localizer[name: "W0025"]) };
                            }

                            GtEcbssu is_SubstoDateExist = db.GtEcbssus.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedTill >= businessSubscription.SubscribedTill);
                            if (is_SubstoDateExist != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0026", Message = string.Format(_localizer[name: "W0026"]) };

                            }

                            var is_SubsCheck = db.GtEcbssus.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && (be.SubscribedTill >= businessSubscription.SubscribedFrom || businessSubscription.SubscribedTill >= businessSubscription.SubscribedFrom));
                            if (is_SubsCheck != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0027", Message = string.Format(_localizer[name: "W0027"]) };

                            }
                        }
                        GtEcbssu b_Susbs = db.GtEcbssus.Where(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedFrom == businessSubscription.SubscribedFrom).FirstOrDefault();
                        if (b_Susbs != null)
                        {
                            b_Susbs.SubscribedTill = businessSubscription.SubscribedTill;
                            b_Susbs.ModifiedBy = businessSubscription.UserID;
                            b_Susbs.ModifiedOn = System.DateTime.Now;
                            b_Susbs.ModifiedTerminal = businessSubscription.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            var b_Subs = new GtEcbssu
                            {
                                BusinessKey = businessSubscription.BusinessKey,
                                SubscribedFrom = businessSubscription.SubscribedFrom,
                                SubscribedTill = businessSubscription.SubscribedTill,
                                ActiveStatus = businessSubscription.ActiveStatus,
                                FormId = businessSubscription.FormID,
                                CreatedBy = businessSubscription.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = businessSubscription.TerminalID
                            };

                            db.GtEcbssus.Add(b_Subs);
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

        #endregion  Business Subscription

        #region  Business Location
        public async Task<List<DO_BusinessLocation>> GetBusinessLocationByBusinessId(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var locs = db.GtEcbslns.Where(x => x.BusinessId == BusinessId)
                        .Join(db.GtEccucos,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_BusinessLocation
                        {
                            BusinessId = x.BusinessId,
                            BusinessKey = x.BusinessKey,
                            LocationId = x.LocationId,
                            LocationDescription = x.LocationDescription,
                            BusinessName = x.BusinessName,
                            EBusinessKey = x.EBusinessKey,
                            Isdcode = x.Isdcode,
                            CityCode = x.CityCode,
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = y.CurrencyName,
                            TaxIdentification = x.TaxIdentification,
                            ESyaLicenseType = x.ESyaLicenseType,
                            EUserLicenses = Convert.ToInt32(Encoding.UTF8.GetString(x.EUserLicenses)),
                            ENoOfBeds = Convert.ToInt32(x.ENoOfBeds != null ? Encoding.UTF8.GetString(x.ENoOfBeds) : "0"),
                            EActiveUsers = x.EActiveUsers,
                            TolocalCurrency = x.TolocalCurrency,
                            TocurrConversion = x.TocurrConversion,
                            TorealCurrency = x.TorealCurrency,
                            IsBookOfAccounts = x.IsBookOfAccounts,
                            BusinessSegmentId = x.BusinessSegmentId,
                            ActiveStatus = x.ActiveStatus
                        }).ToListAsync();

                    return await locs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertBusinessLocation(DO_BusinessLocation obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (obj.IsBookOfAccounts == true)
                        {
                            obj.BusinessSegmentId = 0;
                        }
                        //int _segmentID = db.GtEcbsln.Where(x => x.BusinessId == obj.BusinessId).Select(c => c.SegmentId).DefaultIfEmpty().Max();
                        //_segmentID = _segmentID + 1;
                        //obj.SegmentId = _segmentID;

                        int _locationID = db.GtEcbslns.Where(x => x.BusinessId == obj.BusinessId).Select(c => c.LocationId).DefaultIfEmpty().Max();
                        _locationID = _locationID + 1;
                        obj.LocationId = _locationID;

                        GtEcbsln exists = db.GtEcbslns.Where(x => x.BusinessId == obj.BusinessId && x.LocationId == obj.LocationId).FirstOrDefault();
                        if (exists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0028", Message = string.Format(_localizer[name: "W0028"]) };
                        }


                        GtEcbsln is_locDescExists = db.GtEcbslns.FirstOrDefault(l => l.LocationDescription.ToUpper().Replace(" ", "") == obj.LocationDescription.ToUpper().Replace(" ", "")
                        && l.BusinessId == obj.BusinessId);

                        if (is_locDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0029", Message = string.Format(_localizer[name: "W0029"]) };

                        }
                        int Business_Key = Convert.ToInt32(obj.BusinessId.ToString() + obj.LocationId.ToString());
                        var is_BusinessKeyExist = db.GtEcbslns.Where(x => x.ActiveStatus == true && x.BusinessKey == Business_Key).FirstOrDefault();
                        if (is_BusinessKeyExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0030", Message = string.Format(_localizer[name: "W0030"]) };
                        }

                        byte[] eBKey = Encoding.UTF8.GetBytes(obj.BusinessKey.ToString());

                        byte[] tbUserLicenses = Encoding.UTF8.GetBytes(obj.EUserLicenses.ToString());
                        byte[] tbActiveUser = Encoding.UTF8.GetBytes(0.ToString());
                        byte[] tbNoOfBeds = Encoding.UTF8.GetBytes(obj.ENoOfBeds.ToString());

                        var b_Location = new GtEcbsln
                        {
                            BusinessId = obj.BusinessId,
                            LocationId = obj.LocationId,
                            BusinessKey = Business_Key,
                            LocationDescription = obj.LocationDescription,
                            BusinessName = obj.BusinessName,
                            EBusinessKey = eBKey,
                            Isdcode = obj.Isdcode,
                            CityCode = obj.CityCode,
                            //StateCode=obj.StateCode,
                            CurrencyCode = obj.CurrencyCode,
                            TaxIdentification = obj.TaxIdentification,
                            ESyaLicenseType = obj.ESyaLicenseType,
                            EUserLicenses = tbUserLicenses,
                            EActiveUsers = tbActiveUser,
                            ENoOfBeds = tbNoOfBeds,
                            IsBookOfAccounts = obj.IsBookOfAccounts,
                            BusinessSegmentId = obj.BusinessSegmentId,
                            TolocalCurrency = obj.TolocalCurrency,
                            TocurrConversion = obj.TocurrConversion,
                            TorealCurrency = obj.TorealCurrency,
                            ActiveStatus = obj.ActiveStatus,
                            LocnDateFormat = "DD/MM/YYYY",
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID,
                        };

                        db.GtEcbslns.Add(b_Location);
                        await db.SaveChangesAsync();

                        if (obj.TorealCurrency)
                        {
                            var fa = await db.GtEcbsscs.Where(w => w.BusinessKey == Business_Key).ToListAsync();

                            foreach (GtEcbssc f in fa)
                            {
                                f.ActiveStatus = false;
                                f.ModifiedBy = obj.UserID;
                                f.ModifiedOn = System.DateTime.Now;
                                f.ModifiedTerminal = obj.TerminalID;
                            }
                            await db.SaveChangesAsync();

                            if (obj.l_BSCurrency != null)
                            {
                                foreach (DO_BusienssSegmentCurrency i in obj.l_BSCurrency)
                                {
                                    var obj_FA = await db.GtEcbsscs.Where(w => w.BusinessKey == Business_Key && w.TocurrencyCode == i.CurrencyCode).FirstOrDefaultAsync();
                                    if (obj_FA != null)
                                    {
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.ModifiedBy = obj.UserID;
                                        obj_FA.ModifiedOn = DateTime.Now;
                                        obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                    }
                                    else
                                    {
                                        obj_FA = new GtEcbssc();
                                        obj_FA.BusinessKey = Business_Key;
                                        obj_FA.TocurrencyCode = i.CurrencyCode;
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.FormId = obj.FormId;
                                        obj_FA.CreatedBy = obj.UserID;
                                        obj_FA.CreatedOn = DateTime.Now;
                                        obj_FA.CreatedTerminal = System.Environment.MachineName;
                                        db.GtEcbsscs.Add(obj_FA);
                                    }
                                }
                                await db.SaveChangesAsync();
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

        public async Task<DO_ReturnParameter> UpdateBusinessLocation(DO_BusinessLocation obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.IsBookOfAccounts == true)
                        {
                            obj.BusinessSegmentId = 0;
                        }
                        GtEcbsln is_locDescExists = db.GtEcbslns.FirstOrDefault(l => l.LocationDescription.ToUpper().Replace(" ", "") == obj.LocationDescription.ToUpper().Replace(" ", "") &&
                        l.BusinessId == obj.BusinessId && l.LocationId != obj.LocationId);
                        if (is_locDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0029", Message = string.Format(_localizer[name: "W0029"]) };
                        }

                        byte[] tbUserLicenses = Encoding.UTF8.GetBytes(obj.EUserLicenses.ToString());
                        byte[] tbNoOfBeds = Encoding.UTF8.GetBytes(obj.ENoOfBeds.ToString());

                        GtEcbsln b_loc = db.GtEcbslns.Where(bl => bl.BusinessId == obj.BusinessId && bl.LocationId == obj.LocationId).FirstOrDefault();

                        if (b_loc != null)
                        {
                            b_loc.LocationDescription = obj.LocationDescription;
                            b_loc.BusinessName = obj.BusinessName;
                            b_loc.Isdcode = obj.Isdcode;
                            b_loc.CityCode = obj.CityCode;
                            //b_loc.StateCode = obj.StateCode;
                            b_loc.CurrencyCode = obj.CurrencyCode;
                            b_loc.TaxIdentification = obj.TaxIdentification;
                            b_loc.ESyaLicenseType = obj.ESyaLicenseType;
                            b_loc.EUserLicenses = tbUserLicenses;
                            b_loc.ENoOfBeds = tbNoOfBeds;
                            b_loc.TolocalCurrency = obj.TolocalCurrency;
                            b_loc.TocurrConversion = obj.TocurrConversion;
                            b_loc.TorealCurrency = obj.TorealCurrency;
                            b_loc.ActiveStatus = obj.ActiveStatus;
                            b_loc.IsBookOfAccounts = obj.IsBookOfAccounts;
                            b_loc.BusinessSegmentId = obj.BusinessSegmentId;
                            b_loc.ModifiedBy = obj.UserID;
                            b_loc.ModifiedOn = System.DateTime.Now;
                            b_loc.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();

                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0031", Message = string.Format(_localizer[name: "W0031"]) };
                        }
                        if (obj.TorealCurrency)
                        {
                            if (obj.l_BSCurrency != null)
                            {
                                foreach (DO_BusienssSegmentCurrency i in obj.l_BSCurrency)
                                {
                                    var obj_FA = await db.GtEcbsscs.Where(w => w.BusinessKey == b_loc.BusinessKey && w.TocurrencyCode == i.CurrencyCode).FirstOrDefaultAsync();
                                    if (obj_FA != null)
                                    {
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.ModifiedBy = obj.UserID;
                                        obj_FA.ModifiedOn = DateTime.Now;
                                        obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                    }
                                    else
                                    {
                                        obj_FA = new GtEcbssc();
                                        obj_FA.BusinessKey = b_loc.BusinessKey;
                                        obj_FA.TocurrencyCode = i.CurrencyCode;
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.FormId = obj.FormId;
                                        obj_FA.CreatedBy = obj.UserID;
                                        obj_FA.CreatedOn = DateTime.Now;
                                        obj_FA.CreatedTerminal = System.Environment.MachineName;
                                        db.GtEcbsscs.Add(obj_FA);
                                    }
                                }
                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var bsCurrency = db.GtEcbsscs.Where(w => w.BusinessKey == b_loc.BusinessKey).ToList();

                            if (bsCurrency != null)
                                db.GtEcbsscs.RemoveRange(bsCurrency);
                            await db.SaveChangesAsync();
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveBusinessLocation(bool status, int BusinessId, int locationId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsln b_loc = db.GtEcbslns.Where(bl => bl.BusinessId == BusinessId && bl.LocationId == locationId).FirstOrDefault();
                        if (b_loc == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0031", Message = string.Format(_localizer[name: "W0031"]) };
                        }

                        b_loc.ActiveStatus = status;
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

        public async Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntis.Where(w => w.Isdcode == ISDCode && w.ActiveStatus == true)
                        .Select(x => new DO_TaxIdentification
                        {
                            Isdcode = x.Isdcode,
                            TaxIdentificationId = x.TaxIdentificationId,
                            TaxIdentificationDesc = x.TaxIdentificationDesc,
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

        public async Task<List<DO_CountryCodes>> GetCurrencyListbyIsdCode(int IsdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var Currency = db.GtEccncds.Where(c => c.Isdcode == IsdCode && c.ActiveStatus).Join(db.GtEccucos,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_CountryCodes
                        {
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = y.CurrencyName
                        }).ToListAsync();


                    return await Currency;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusinessLocation>> GetActiveLocationsAsSegments()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbslns.Where(w => w.ActiveStatus == true && w.IsBookOfAccounts == true)
                        .Select(x => new DO_BusinessLocation
                        {
                            SegmentId = x.BusinessKey,
                            BusinessName = x.BusinessName
                        }).OrderBy(o => o.SegmentId).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Cities>> GetCityListbyISDCode(int isdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var pf = db.GtAddrcts.Where(x => x.Isdcode == isdCode && x.ActiveStatus)
                   .Select(s => new DO_Cities
                   {
                       CityCode = s.CityCode,
                       CityDesc = s.CityDesc
                   }).
                    Distinct()
                   .ToListAsync();
                    return await pf;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_TaxIdentification> GetStateCodeByISDCode(int isdCode, int TaxIdentificationId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntis.Where(w => w.Isdcode == isdCode && w.TaxIdentificationId == TaxIdentificationId && w.ActiveStatus)
                        .Select(x => new DO_TaxIdentification
                        {
                            TaxIdentificationId = x.TaxIdentificationId,
                            TaxIdentificationDesc = x.TaxIdentificationDesc,
                            StateCode = x.StateCode,

                        }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusienssSegmentCurrency>> GetCurrencybyBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEccucos
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusienssSegmentCurrency
                        {
                            CurrencyCode = r.CurrencyCode,
                            CurrencyName = r.CurrencyName,
                            ActiveStatus = false
                        }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcbssc sbCurrency = db.GtEcbsscs.Where(x => x.BusinessKey == BusinessKey && x.TocurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (sbCurrency != null)
                        {
                            obj.ActiveStatus = sbCurrency.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                        }
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessEntity> GetBusinessUnitType(int businessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    DO_BusinessEntity _entity = new DO_BusinessEntity();

                    var bt = await db.GtEcbsens.Where(x => x.BusinessId == businessId).FirstOrDefaultAsync();
                    if (bt != null)
                    {
                        _entity.BusinessUnitType = bt.BusinessUnitType;
                    }

                    return _entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Define User Role Action

        public async Task<List<DO_ApplicationCodes>> GetUserRoleByCodeType(int codeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    
                        var ds = db.GtEcapcds
                       .Where(w => w.CodeType == codeType && w.ActiveStatus==true)
                       .Select(r => new DO_ApplicationCodes
                       {
                           ApplicationCode = r.ApplicationCode,
                           CodeDesc = r.CodeDesc
                       }).OrderBy(o => o.CodeDesc).ToListAsync();
                        return await ds;
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_UserRoleActionLink>> GetUserRoleActionLink(int userRole)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcfmacs.Where(x => x.ActiveStatus == true)
                   .GroupJoin(db.GtEuusrls.Where(w => w.UserRole == userRole),
                     d => d.ActionId,
                     l => l.ActionId,
                    (act, rol) => new { act, rol })
                   .SelectMany(z => z.rol.DefaultIfEmpty(),
                    (a, b) => new DO_UserRoleActionLink
                    {
                        ActionId = a.act.ActionId,
                        ActionDesc=a.act.ActionDesc,
                        UserRole = b == null ? 0 : b.UserRole,
                        ActiveStatus = b == null ? false : b.ActiveStatus
                    }).ToListAsync();

                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateUpdateUserRoleActionLink(List<DO_UserRoleActionLink> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var _role in obj)
                        {
                            var roleExist = db.GtEuusrls.Where(w => w.UserRole == _role.UserRole && w.ActionId == _role.ActionId).FirstOrDefault();
                            if (roleExist != null)
                            {
                                db.GtEuusrls.Remove(roleExist);
                                await db.SaveChangesAsync();
                            }
                         }
                        foreach (var _role in obj)
                        { 
                            if (_role.ActiveStatus == true) 
                                { 
                                    var userrolelink = new GtEuusrl
                                    {
                                        UserRole = _role.UserRole,
                                        ActionId = _role.ActionId,
                                        ActiveStatus = _role.ActiveStatus,
                                        FormId= _role.FormID,
                                        CreatedBy = _role.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _role.TerminalID
                                    };
                                    db.GtEuusrls.Add(userrolelink);
                                    await db.SaveChangesAsync();
                                }
                        }
                    
                       
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

        #region Define Menu Link to Location
        public async Task<DO_ConfigureMenu> GetLocationMenuLinkbyBusinessKey(int businesskey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    DO_ConfigureMenu mn = new DO_ConfigureMenu();
                    mn.l_MainMenu = await db.GtEcmamns.Where(x=>x.ActiveStatus)
                                    .Select(m => new DO_MainMenu()
                                    {
                                        MainMenuId = m.MainMenuId,
                                        MainMenu = m.MainMenu,
                                        MenuIndex = m.MenuIndex,
                                        ActiveStatus = m.ActiveStatus
                                    }).ToListAsync();

                    mn.l_SubMenu = await db.GtEcsbmns.Where(x => x.ActiveStatus)
                                    .Select(s => new DO_SubMenu()
                                    {
                                        MainMenuId = s.MainMenuId,
                                        MenuItemId = s.MenuItemId,
                                        MenuItemName = s.MenuItemName,
                                        MenuIndex = s.MenuIndex,
                                        ParentID = s.ParentId,
                                        ActiveStatus = s.ActiveStatus
                                    }).ToListAsync();

                    mn.l_FormMenu = await db.GtEcmnfls.Where(x => x.ActiveStatus)
                                    .Select(f => new DO_FormMenu()
                                    {
                                        MainMenuId = f.MainMenuId,
                                        MenuItemId = f.MenuItemId,
                                        //FormId = f.FormId,
                                        FormId = f.MenuKey,
                                        FormNameClient = f.FormNameClient,
                                        FormIndex = f.FormIndex,
                                        ActiveStatus = f.ActiveStatus,

                                    }).ToListAsync();
                    foreach (var obj in mn.l_FormMenu)
                    {
                        GtEcbsmn menulink = db.GtEcbsmns.Where(c => c.BusinessKey == businesskey && c.MenuKey == obj.FormId).FirstOrDefault();
                        if (menulink != null)
                        {
                            obj.ActiveStatus = menulink.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                        }
                    }
                    return mn;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateLocationMenuLink(List<DO_LocationMenuLink> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var _link in obj)
                        {
                            var _linkExist = db.GtEcbsmns.Where(w => w.MenuKey == _link.MenuKey && w.BusinessKey == _link.BusinessKey).FirstOrDefault();
                            if (_linkExist != null)
                            {
                                if (_linkExist.ActiveStatus != _link.ActiveStatus)
                                {
                                    _linkExist.ActiveStatus = _link.ActiveStatus;
                                    _linkExist.ModifiedBy = _link.UserID;
                                    _linkExist.ModifiedOn = System.DateTime.Now;
                                    _linkExist.ModifiedTerminal = _link.TerminalID;
                                }

                            }
                            else
                            {
                                if (_link.ActiveStatus)
                                {
                                    var _loclink = new GtEcbsmn
                                    {
                                        BusinessKey = _link.BusinessKey,
                                        MenuKey = _link.MenuKey,
                                        ActiveStatus = _link.ActiveStatus,
                                        CreatedBy = _link.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _link.TerminalID
                                    };
                                    db.GtEcbsmns.Add(_loclink);
                                }

                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

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
