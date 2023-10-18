using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IeSyaCultureRepository
    {
        #region define eSya Culture 
        Task<List<DO_eSyaCulture>> GetAlleSyaCultures();
        Task<DO_ReturnParameter> InsertOrUpdateIntoeSyaCultures(DO_eSyaCulture obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveeSyaCultures(bool status, string esyaculture);
        #endregion
    }
}
