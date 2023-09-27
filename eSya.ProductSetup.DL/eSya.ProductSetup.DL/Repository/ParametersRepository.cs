using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DL.Repository
{
    public class ParametersRepository : IParametersRepository
    {
        private readonly IStringLocalizer<ParametersRepository> _localizer;
        public ParametersRepository(IStringLocalizer<ParametersRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Parameter Header

        public async Task<List<DO_Parameters>> GetParametersHeaderInformation()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcparhs
                         .AsNoTracking()
                         .Select(r => new DO_Parameters
                         {
                             ParameterType = r.ParameterType,
                             ParameterHeaderDesc = r.ParameterHeaderDesc,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.ParameterHeaderDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoParameterHeader(DO_Parameters obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_ParameterIdExist = db.GtEcparhs.Any(a => a.ParameterType == obj.ParameterType);
                        if (is_ParameterIdExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0060", Message = string.Format(_localizer[name: "W0060"]) };
                        }

                        bool is_ParameterDescExist = db.GtEcparhs.Any(a => a.ParameterHeaderDesc.Trim().ToUpper() == obj.ParameterHeaderDesc.Trim().ToUpper());
                        if (is_ParameterDescExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0061", Message = string.Format(_localizer[name: "W0061"]) };
                        }
                        //int maxval = db.GtEcparh.Select(c => c.ParameterType).DefaultIfEmpty().Max();
                        //int _parameterType = maxval + 1;
                        var pa_rh = new GtEcparh
                        {
                            ParameterType = obj.ParameterType,
                            ParameterHeaderDesc = obj.ParameterHeaderDesc,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcparhs.Add(pa_rh);

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

        public async Task<DO_ReturnParameter> UpdateParameterHeader(DO_Parameters obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcparh pa_rh = db.GtEcparhs.Where(w => w.ParameterType == obj.ParameterType).FirstOrDefault();
                        if (pa_rh == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0062", Message = string.Format(_localizer[name: "W0062"]) };
                        }

                        pa_rh.ParameterHeaderDesc = obj.ParameterHeaderDesc;
                        pa_rh.ActiveStatus = obj.ActiveStatus;
                        pa_rh.ModifiedBy = obj.UserID;
                        pa_rh.ModifiedOn = DateTime.Now;
                        pa_rh.ModifiedTerminal = obj.TerminalID;

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveParameterHeader(bool status, int parm_type)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcparh param = db.GtEcparhs.Where(w => w.ParameterType == parm_type).FirstOrDefault();
                        if (param == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0062", Message = string.Format(_localizer[name: "W0062"]) };
                        }

                        param.ActiveStatus = status;
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
        #endregion Parameter Header

        #region Parameters

        public async Task<List<DO_Parameters>> GetParametersInformationByParameterType(int parameterType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcparms
                         .Where(w => w.ParameterType == parameterType)
                         .Select(r => new DO_Parameters
                         {
                             ParameterType = r.ParameterType,
                             ParameterId = r.ParameterId,
                             ParameterDesc = r.ParameterDesc,
                             ParameterValueType = r.ParameterValueType,
                             ActiveStatus = r.ActiveStatus

                         }).OrderBy(o => o.ParameterId).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoParameters(DO_Parameters obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_ParameterDescExist = db.GtEcparms.Any(a => a.ParameterDesc.Trim().ToUpper() == obj.ParameterDesc.Trim().ToUpper() && a.ParameterType == obj.ParameterType);
                        if (is_ParameterDescExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0063", Message = string.Format(_localizer[name: "W0063"]) };
                        }
                        int maxval = db.GtEcparms.Where(x => x.ParameterType == obj.ParameterType).Select(c => c.ParameterId).DefaultIfEmpty().Max();
                        int _parameterId = maxval + 1;
                        var pa_rm = new GtEcparm
                        {
                            ParameterType = obj.ParameterType,
                            ParameterId = _parameterId,
                            ParameterDesc = obj.ParameterDesc,
                            ParameterValueType = obj.ParameterValueType,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcparms.Add(pa_rm);

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

        public async Task<DO_ReturnParameter> UpdateParameters(DO_Parameters obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_ParameterExist = db.GtEcparms.Where(w => w.ParameterDesc.Trim().ToUpper().Replace(" ", "") == obj.ParameterDesc.Trim().ToUpper().Replace(" ", "")
                                && w.ParameterId != obj.ParameterId && w.ParameterType == obj.ParameterType).FirstOrDefault();
                        if (is_ParameterExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0063", Message = string.Format(_localizer[name: "W0063"]) };
                        }

                        GtEcparm pa_rm = db.GtEcparms.Where(w => w.ParameterId == obj.ParameterId && w.ParameterType == obj.ParameterType).FirstOrDefault();
                        if (pa_rm == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0064", Message = string.Format(_localizer[name: "W0064"]) };
                        }

                        pa_rm.ParameterDesc = obj.ParameterDesc;
                        pa_rm.ParameterValueType = obj.ParameterValueType;
                        pa_rm.ActiveStatus = obj.ActiveStatus;
                        pa_rm.ModifiedBy = obj.UserID;
                        pa_rm.ModifiedOn = DateTime.Now;
                        pa_rm.ModifiedTerminal = obj.TerminalID;
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

        #endregion Parameters
    }
}
