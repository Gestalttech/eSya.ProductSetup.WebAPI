using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IPaymentMethodRepository
    {
        Task<List<DO_PaymentMethod>> GetPaymentMethodbyISDCode(int codetype, int ISDCode);
        Task<DO_ReturnParameter> InsertOrUpdatePaymentMethod(List<DO_PaymentMethod> obj);
    }
}
