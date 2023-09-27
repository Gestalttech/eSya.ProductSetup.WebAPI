using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IApplicationCodesRepository
    {
        #region Code Types
        Task<List<DO_CodeTypes>> GetCodeTypes();

        Task<DO_ReturnParameter> InsertIntoCodeType(DO_CodeTypes obj);

        Task<DO_ReturnParameter> UpdateCodeType(DO_CodeTypes obj);

        Task<List<DO_CodeTypes>> GetActiveCodeTypes();

        Task<List<DO_CodeTypes>> GetUserDefinedCodeTypesList();

        Task<List<DO_CodeTypes>> GetSystemDefinedCodeTypesList();

        Task<DO_ReturnParameter> ActiveOrDeActiveCodeTypes(bool status, int code_type);
        #endregion

        #region Application Codes
        Task<List<DO_ApplicationCodes>> GetApplicationCodes();
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType);
        Task<DO_ReturnParameter> InsertIntoApplicationCodes(DO_ApplicationCodes obj);
        Task<DO_ReturnParameter> UpdateApplicationCodes(DO_ApplicationCodes obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveApplicationCode(bool status, int app_code);
        #endregion

      

    }
}
