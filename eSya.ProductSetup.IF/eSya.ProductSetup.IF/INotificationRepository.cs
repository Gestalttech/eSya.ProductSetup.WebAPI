using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface INotificationRepository
    {
        #region Notification Trigger Event
        Task<List<DO_SMSTEvent>> GetAllSMSTriggerEvents();

        Task<DO_ReturnParameter> InsertIntoSMSTriggerEvent(DO_SMSTEvent obj);

        Task<DO_ReturnParameter> UpdateSMSTriggerEvent(DO_SMSTEvent obj);

        Task<DO_ReturnParameter> DeleteSMSTriggerEvent(int TeventId);

        Task<DO_ReturnParameter> ActiveOrDeActiveSMSTriggerEvent(bool status, int TriggerEventId);
        #endregion SMS Trigger Event

        #region Notification Variables
        Task<List<DO_SMSVariable>> GetSMSVariableInformation();
        Task<List<DO_SMSVariable>> GetActiveSMSVariableInformation();
        Task<DO_ReturnParameter> InsertIntoSMSVariable(DO_SMSVariable obj);
        Task<DO_ReturnParameter> UpdateSMSVariable(DO_SMSVariable obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveSMSVariable(bool status, string smsvariable);
        #endregion
    }
}
