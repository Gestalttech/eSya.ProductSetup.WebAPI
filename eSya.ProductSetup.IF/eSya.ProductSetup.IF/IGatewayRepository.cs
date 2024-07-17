using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IGatewayRepository
    {
        #region Gate way Rules
        Task<List<DO_GatewayRules>> GetGatewayRules();
        Task<DO_ReturnParameter> InsertOrUpdateGatewayRules(DO_GatewayRules obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveGatewayRules(bool status, int GwRuleId);
        #endregion
    }
}
