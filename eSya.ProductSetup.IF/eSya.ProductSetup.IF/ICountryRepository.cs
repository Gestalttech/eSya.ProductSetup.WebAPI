using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface ICountryRepository
    {
        #region Country Codes
        Task<List<DO_CountryCodes>> GetAllCountryCodesAsync();
        Task<DO_ReturnParameter> InsertIntoCountryCode(DO_CountryCodes countrycode);
        Task<DO_ReturnParameter> UpdateCountryCode(DO_CountryCodes countrycode);
        Task<DO_CountryCodes> GetCurrencyNamebyIsdCode(int IsdCode);
        Task<DO_ReturnParameter> ActiveOrDeActiveCountryCode(bool status, int Isd_code);
        Task<List<DO_UIDPattern>> GetUIDPatternbyIsdcode(int Isdcode);
        #endregion Country Codes

        #region Statutory Details

        Task<List<DO_eSyaParameter>> GetStatutoryCodesParameterList(int IsdCode, int StatutoryCode);

        Task<List<DO_CountryStatutoryDetails>> GetStatutoryCodesbyIsdcode(int Isdcode);

        Task<DO_ReturnParameter> InsertOrUpdateStatutoryCodes(DO_CountryStatutoryDetails obj);

        Task<List<DO_CountryStatutoryDetails>> GetActiveStatutoryCodes();

        Task<DO_ReturnParameter> ActiveOrDeActiveStatutoryCode(bool status, int Isd_code, int statutorycode);
        #endregion Statutory Details
    }
}
