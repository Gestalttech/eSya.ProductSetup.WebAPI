using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IAddressRepository
    {
        #region States
        Task<List<DO_States>> GetStatesbyISDCode(int isdCode);

        Task<DO_ReturnParameter> InsertOrUpdateIntoStates(DO_States obj);
        #endregion

        #region Cities
        Task<List<DO_Cities>> GetCitiesbyStateCode(int isdCode, int stateCode);

        Task<DO_ReturnParameter> InsertOrUpdateIntoCities(DO_Cities obj);
        #endregion

        #region Common Methods
        Task<List<DO_States>> GetActiveStatesbyISDCode(int isdCode);

        Task<List<DO_Cities>> GetActiveCitiesbyStateCode(int isdCode, int stateCode);
        #endregion
    }
}
