using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IBusinessCalendarRepository
    {
        #region Business Calendar
        Task<List<DO_BusinessCalendar>> GetBusinessCalendarBYBusinessKey(int businessKey);
        Task<DO_ReturnParameter> InsertOrUpdateBusinessCalendar(DO_BusinessCalendar obj);
        #endregion
    }
}
