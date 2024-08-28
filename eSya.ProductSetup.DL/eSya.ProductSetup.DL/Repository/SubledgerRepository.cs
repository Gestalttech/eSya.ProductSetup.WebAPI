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
    public class SubledgerRepository: ISubledgerRepository
    {
        private readonly IStringLocalizer<SubledgerRepository> _localizer;
        public SubledgerRepository(IStringLocalizer<SubledgerRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Subledger Type

        public async Task<List<DO_Subledger>> GetSubledgerTypes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsults
                         .AsNoTracking()
                         .Select(r => new DO_Subledger
                         {
                             SubledgerType = r.SubledgerType,
                             Sltdesc = r.Sltdesc,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.Sltdesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSubledgerType(DO_Subledger obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_stype = db.GtEcsults.Any(a => a.SubledgerType.ToUpper().Replace(" ", "") == obj.SubledgerType.ToUpper().Replace(" ", ""));
                        if (is_stype)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0112", Message = string.Format(_localizer[name: "W0112"]) };
                        }

                        bool is_stypeDesc = db.GtEcsults.Any(a => a.Sltdesc.ToUpper().Replace(" ", "") == obj.Sltdesc.ToUpper().Replace(" ", ""));
                        if (is_stypeDesc)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0113", Message = string.Format(_localizer[name: "W0113"]) };
                        }
                       
                        var stype = new GtEcsult
                        {
                            SubledgerType = obj.SubledgerType,
                            Sltdesc = obj.Sltdesc,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsults.Add(stype);

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

        public async Task<DO_ReturnParameter> UpdateSubledgerType(DO_Subledger obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsult stype = db.GtEcsults.Where(w => w.SubledgerType.ToUpper().Replace(" ", "") == obj.SubledgerType.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (stype == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0114", Message = string.Format(_localizer[name: "W0114"]) };
                        }

                        stype.Sltdesc = obj.Sltdesc;
                        stype.ActiveStatus = obj.ActiveStatus;
                        stype.ModifiedBy = obj.UserID;
                        stype.ModifiedOn = DateTime.Now;
                        stype.ModifiedTerminal = obj.TerminalID;

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveSubledgerType(bool status, string stype)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsult _stype = db.GtEcsults.Where(w => w.SubledgerType.ToUpper().Replace(" ", "") == stype.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_stype == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0114", Message = string.Format(_localizer[name: "W0114"]) };
                        }

                        _stype.ActiveStatus = status;
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
        #endregion Subledger Type

        #region Subledger Group
        public async Task<List<DO_Subledger>> GetSubledgerGroupInformationBySubledgerType(string stype)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsulgs
                         .Where(w => w.SubledgerType.ToUpper().Replace(" ", "") ==  stype.ToUpper().Replace(" ", ""))
                         .Select(r => new DO_Subledger
                         {
                             SubledgerType = r.SubledgerType,
                             SubledgerGroup = r.SubledgerGroup,
                             SubledgerDesc = r.SubledgerDesc,
                             Coahead = r.Coahead,
                             ActiveStatus = r.ActiveStatus

                         }).OrderBy(o => o.SubledgerGroup).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSubledgerGroup(DO_Subledger obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_sGroupDesc = db.GtEcsulgs.Any(a => a.SubledgerDesc.ToUpper().Replace(" ", "") == obj.SubledgerDesc.ToUpper().Replace(" ", "")
                        && a.SubledgerType.ToUpper().Replace(" ", "") == obj.SubledgerType.ToUpper().Replace(" ", ""));
                        if (is_sGroupDesc)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0115", Message = string.Format(_localizer[name: "W0115"]) };
                        }
                        int maxval = db.GtEcsulgs.Select(c => c.SubledgerGroup).DefaultIfEmpty().Max();
                        int _sGroupId = maxval + 1;
                        var s_group = new GtEcsulg
                        {
                            SubledgerGroup = _sGroupId,
                            SubledgerType = obj.SubledgerType,
                            SubledgerDesc = obj.SubledgerDesc,
                            Coahead = obj.Coahead,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsulgs.Add(s_group);

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

        public async Task<DO_ReturnParameter> UpdateSubledgerGroup(DO_Subledger obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_DescExist = db.GtEcsulgs.Where(w => w.SubledgerDesc.ToUpper().Replace(" ", "") == obj.SubledgerDesc.Trim().ToUpper().Replace(" ", "")
                                && w.SubledgerGroup != obj.SubledgerGroup && w.SubledgerType.ToUpper().Replace(" ", "") == obj.SubledgerType.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_DescExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0115", Message = string.Format(_localizer[name: "W0115"]) };
                        }

                        GtEcsulg sgroup = db.GtEcsulgs.Where(w => w.SubledgerGroup == obj.SubledgerGroup).FirstOrDefault();
                        if (sgroup == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0116", Message = string.Format(_localizer[name: "W0116"]) };
                        }

                        sgroup.SubledgerType = obj.SubledgerType;
                        sgroup.SubledgerDesc = obj.SubledgerDesc;
                        sgroup.Coahead = obj.Coahead;
                        sgroup.ActiveStatus = obj.ActiveStatus;
                        sgroup.ModifiedBy = obj.UserID;
                        sgroup.ModifiedOn = DateTime.Now;
                        sgroup.ModifiedTerminal = obj.TerminalID;
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

        #endregion Subledger Group
    }
}
