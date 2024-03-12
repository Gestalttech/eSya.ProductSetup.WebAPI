using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
       
        public async Task<List<DO_PaymentMethod>> GetPaymentMethodbyISDCode(int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEccnpms.Where(x => x.Isdcode == ISDCode)
                       .Join(db.GtEcapcds,
                       a => a.PaymentMethod,
                       p => p.ApplicationCode,
                       (a, p) => new { a, p })
                      .Join(db.GtEcapcds,
                      aa => aa.a.InstrumentType,
                      I => I.ApplicationCode,
                      (aa, I) => new { aa, I })
                     .Select(r => new DO_PaymentMethod
                     {
                         Isdcode = r.aa.a.Isdcode,
                         PaymentMethod = r.aa.a.PaymentMethod,
                         InstrumentType = r.aa.a.InstrumentType,
                         PaymentMethodDesc = r.aa.p.CodeDesc,
                         InstrumentTypeDesc = r.I.CodeDesc,
                         ActiveStatus = r.aa.a.ActiveStatus
                     }).ToListAsync();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdatePaymentMethod(DO_PaymentMethod obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var _payExist = db.GtEccnpms.Where(w => w.Isdcode == obj.Isdcode && w.PaymentMethod == obj.PaymentMethod && w.InstrumentType == obj.InstrumentType).FirstOrDefault();
                        if (_payExist != null)
                        {

                            _payExist.ActiveStatus = obj.ActiveStatus;
                            _payExist.ModifiedBy = obj.UserID;
                            _payExist.ModifiedOn = System.DateTime.Now;
                            _payExist.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                        }
                        else
                        {

                            var _payment = new GtEccnpm
                            {
                                Isdcode = obj.Isdcode,
                                PaymentMethod = obj.PaymentMethod,
                                InstrumentType = obj.InstrumentType,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                FormId = obj.FormID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEccnpms.Add(_payment);
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
                        return new DO_ReturnParameter() { Status = false, Message = ex.Message };
                    }
                }
            }
        }
    }
}
