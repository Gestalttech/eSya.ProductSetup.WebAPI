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
    public class AddressRepository : IAddressRepository
    {
        private readonly IStringLocalizer<AddressRepository> _localizer;
        public AddressRepository(IStringLocalizer<AddressRepository> localizer)
        {
            _localizer = localizer;
        }
      
        #region States

        public async Task<List<DO_States>> GetStatesbyISDCode(int isdCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var states = db.GtAddrsts.Where(x => x.Isdcode == isdCode)
               .Select(s => new DO_States
               {
                   Isdcode = s.Isdcode,
                   StateCode = s.StateCode,
                   StateDesc = s.StateDesc,
                   ActiveStatus = s.ActiveStatus
               })
               .ToListAsync();
                return await states;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateIntoStates(DO_States obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _sa = db.GtAddrsts.Where(w => w.Isdcode == obj.Isdcode && w.StateCode == obj.StateCode).FirstOrDefault();
                        if (_sa != null)
                        {
                            _sa.StateDesc = obj.StateDesc;
                            _sa.ActiveStatus = obj.ActiveStatus;
                            _sa.ModifiedBy = obj.UserID;
                            _sa.ModifiedOn = System.DateTime.Now;
                            _sa.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            int max_stateId = db.GtAddrsts.Select(c => c.StateCode).DefaultIfEmpty().Max();
                            int _stateId = max_stateId + 1;
                            var _states = new GtAddrst
                            {
                                Isdcode = obj.Isdcode,
                                StateCode = _stateId,
                                StateDesc = obj.StateDesc,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtAddrsts.Add(_states);
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
        #endregion
      
        #region Cities

        public async Task<List<DO_Cities>> GetCitiesbyStateCode(int isdCode, int stateCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var cities = db.GtAddrcts.Join
                    (db.GtAddrsts,
                    c => new { c.Isdcode, c.StateCode },
                    s => new { s.Isdcode, s.StateCode },
                    (c, s) => new { c, s }).Where(x => x.c.Isdcode == isdCode && x.c.StateCode == stateCode)
               .Select(r => new DO_Cities
               {
                   Isdcode = r.c.Isdcode,
                   StateCode = r.c.StateCode,
                   CityCode = r.c.CityCode,
                   CityDesc = r.c.CityDesc,
                   StateDesc = r.s.StateDesc,
                   ActiveStatus = r.c.ActiveStatus
               })
               .ToListAsync();
                return await cities;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateIntoCities(DO_Cities obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _ci = db.GtAddrcts.Where(w => w.Isdcode == obj.Isdcode && w.CityCode == obj.CityCode && w.StateCode == obj.StateCode).FirstOrDefault();
                        if (_ci != null)
                        {
                            _ci.CityDesc = obj.CityDesc;
                            _ci.ActiveStatus = obj.ActiveStatus;
                            _ci.ModifiedBy = obj.UserID;
                            _ci.ModifiedOn = System.DateTime.Now;
                            _ci.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            int max_cityId = db.GtAddrcts.Select(c => c.CityCode).DefaultIfEmpty().Max();
                            int _CityId = max_cityId + 1;
                            var _city = new GtAddrct
                            {
                                Isdcode = obj.Isdcode,
                                CityCode = _CityId,
                                StateCode = obj.StateCode,
                                CityDesc = obj.CityDesc,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtAddrcts.Add(_city);
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
        #endregion
       
        #region Common Methods
        public async Task<List<DO_States>> GetActiveStatesbyISDCode(int isdCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var states = db.GtAddrsts.Where(x => x.Isdcode == isdCode && x.ActiveStatus == true)
               .Select(s => new DO_States
               {
                   StateCode = s.StateCode,
                   StateDesc = s.StateDesc

               })
               .ToListAsync();
                return await states;
            }
        }

        public async Task<List<DO_Cities>> GetActiveCitiesbyStateCode(int isdCode, int stateCode)
        {
            using (var db = new eSyaEnterprise())
            {
                var cities = db.GtAddrcts.Where(x => x.Isdcode == isdCode && x.StateCode == stateCode && x.ActiveStatus == true)
               .Select(r => new DO_Cities
               {
                   CityCode = r.CityCode,
                   CityDesc = r.CityDesc,
               })
               .ToListAsync();
                return await cities;
            }
        }
        #endregion
    }
}
