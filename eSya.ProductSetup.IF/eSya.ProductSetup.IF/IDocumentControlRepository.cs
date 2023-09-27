using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IDocumentControlRepository
    {
        #region Calendar Defination
        Task<DO_ReturnParameter> InsertCalendarHeaderAndDetails(DO_CalendarDefinition calendarheadar);
        Task<List<DO_CalendarDefinition>> GetCalendarHeadersbyBusinessKey(int Businesskey);

        Task<List<DO_CalendarDefinition>> GetCalendarHeaders();

        #endregion Calendar Defination

        #region Document Master
        Task<List<DO_DocumentControlMaster>> GetDocumentControlMaster();
        Task<List<DO_eSyaParameter>> GetDocumentParametersByID(int documentID);
        Task<DO_ReturnParameter> AddOrUpdateDocumentControl(DO_DocumentControlMaster obj);
        #endregion

        #region Form Document Link
        Task<List<DO_Forms>> GetFormsForDocumentControl();
        Task<List<DO_FormDocumentLink>> GetFormDocumentlink(int formID);
        Task<DO_ReturnParameter> UpdateFormDocumentLinks(List<DO_FormDocumentLink> obj);
        #endregion
    }
}
