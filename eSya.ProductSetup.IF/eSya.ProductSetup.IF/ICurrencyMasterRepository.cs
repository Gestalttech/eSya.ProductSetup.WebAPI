using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface ICurrencyMasterRepository
    {
        #region Currency Master

        Task<List<DO_CurrencyMaster>> GetCurrencyMaster();

        Task<List<DO_CurrencyMaster>> GetActiveCurrencyList();

        Task<List<DO_CurrencyMaster>> GetCurrencyListByCurrenyPrefix(string currencyPrefix);

        Task<DO_ReturnParameter> InsertIntoCurrencyMaster(DO_CurrencyMaster obj);

        Task<DO_ReturnParameter> UpdateCurrencyMaster(DO_CurrencyMaster obj);

        Task<DO_ReturnParameter> DeleteCurrencyMasterByCurrencyCode(string currencyCode);

        Task<DO_ReturnParameter> ActiveOrDeActiveCurrencyMaster(bool status, string currency_code);
        #endregion

        #region Currency Denomination Information

        Task<List<DO_CurrencyDenominationInformation>> GetCurrencyDenominationInfoByCurrencyCode(string currencyCode);

        Task<DO_ReturnParameter> InsertCurrencyDenominationInformation(DO_CurrencyDenominationInformation obj);

        Task<DO_ReturnParameter> UpdateCurrencyDenominationInformation(DO_CurrencyDenominationInformation obj);

        Task<DO_ReturnParameter> DeleteCurrencyDenominationInformation(string currencyCode, decimal DenomId, string BnorCNId);

        Task<DO_ReturnParameter> ActiveOrDeActiveCurrencyDenomination(bool status, string currencycode, int denomId);
        #endregion
    }
}
