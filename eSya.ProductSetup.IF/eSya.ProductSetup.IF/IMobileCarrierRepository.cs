using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IMobileCarrierRepository
    {
        #region Mobile Carrier
        Task<List<DO_MobileCarrier>> GetMobileCarriers();
        Task<DO_ReturnParameter> InsertOrUpdateMobileCarrier(DO_MobileCarrier obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveMobileCarrier(bool status, int ISDCode, string MobilePrefix);
        #endregion
    }
}
