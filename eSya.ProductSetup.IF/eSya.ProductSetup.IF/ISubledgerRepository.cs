using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface ISubledgerRepository
    {
        #region Subledger Type
        Task<List<DO_Subledger>> GetSubledgerTypes();
        Task<DO_ReturnParameter> InsertIntoSubledgerType(DO_Subledger obj);
        Task<DO_ReturnParameter> UpdateSubledgerType(DO_Subledger obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveSubledgerType(bool status, string stype);
        #endregion

        #region Subledger Group
        Task<List<DO_Subledger>> GetSubledgerGroupInformationBySubledgerType(string stype);
        Task<DO_ReturnParameter> InsertIntoSubledgerGroup(DO_Subledger obj);
        Task<DO_ReturnParameter> UpdateSubledgerGroup(DO_Subledger obj);
        #endregion Subledger Group
    }
}
