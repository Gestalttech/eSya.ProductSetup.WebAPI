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
    public class PaymentMethodRepository: IPaymentMethodRepository
    {
        private readonly IStringLocalizer<PaymentMethodRepository> _localizer;
        public PaymentMethodRepository(IStringLocalizer<PaymentMethodRepository> localizer)
        {
            _localizer = localizer;
        }

        public async Task<List<DO_PaymentMethod>> GetPaymentMethodbyISDCode(int codetype,int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcapcds.Where(x => x.ActiveStatus && x.CodeType== codetype)
                        .Select(r => new DO_PaymentMethod
                        {
                            Isdcode = ISDCode,
                            InstrumentType = r.ApplicationCode,
                            PaymentMethod = r.CodeDesc,
                            ActiveStatus = false,
                        }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEccnpm pay = db.GtEccnpms.Where(x => x.Isdcode == ISDCode && x.InstrumentType== obj.InstrumentType).FirstOrDefault();
                        if (pay != null)
                        {
                            obj.ActiveStatus = pay.ActiveStatus;
                            obj.InstrumentType = pay.InstrumentType;
                           
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
        public async Task<DO_ReturnParameter> InsertOrUpdatePaymentMethod(List<DO_PaymentMethod> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var _pay in obj)
                        {
                            var _payExist = db.GtEccnpms.Where(w => w.Isdcode == _pay.Isdcode && w.InstrumentType == _pay.InstrumentType).FirstOrDefault();
                            if (_payExist != null)
                            {
                                
                                    _payExist.ActiveStatus = _pay.ActiveStatus;
                                    _payExist.ModifiedBy = _pay.UserID;
                                    _payExist.ModifiedOn = System.DateTime.Now;
                                    _payExist.ModifiedTerminal = _pay.TerminalID;

                            }
                            else
                            {
                                
                                    var _payment = new GtEccnpm
                                    {
                                        Isdcode = _pay.Isdcode,
                                        InstrumentType = _pay.InstrumentType,
                                        ActiveStatus = _pay.ActiveStatus,
                                        CreatedBy = _pay.UserID,
                                        FormId=_pay.FormID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _pay.TerminalID
                                    };
                                    db.GtEccnpms.Add(_payment);

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
    }
}
