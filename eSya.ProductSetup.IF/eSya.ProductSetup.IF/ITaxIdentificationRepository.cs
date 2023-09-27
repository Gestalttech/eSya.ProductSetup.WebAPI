using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface ITaxIdentificationRepository
    {
        #region Tax Identification
        Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int ISDCode);

        Task<DO_ReturnParameter> InsertIntoTaxIdentification(DO_TaxIdentification obj);

        Task<DO_ReturnParameter> UpdateTaxIdentification(DO_TaxIdentification obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveTaxIdentification(bool status, int Isd_code, int TaxIdentificationId);
        #endregion


    }
}
