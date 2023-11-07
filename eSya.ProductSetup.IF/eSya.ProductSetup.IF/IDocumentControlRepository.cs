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
        
        #region Calendar Header
        Task<List<DO_CalendarHeader>> GetCalendarHeaders();
        Task<DO_ReturnParameter> InsertCalendarHeader(DO_CalendarHeader obj);
        #endregion

        #region Calendar Header
        Task<DO_ReturnParameter> InsertCalendarDetails(DO_CalendarHeader obj);

        #endregion

        #region Calendar Patient ID Generation
        Task<List<DO_CalendarHeader>> GetCalenderKeybyBusinessKey(int Businesskey);
        List<DO_CalendarPatientIdGeneration> GetCalendarPatientGenerationbyBusinessKeyAndCalenderKey(int BusinessKey, string CalenderKey);
        Task<DO_ReturnParameter> UpdateCalendarGeneration(DO_CalendarPatientIdGeneration obj);
        #endregion

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

        #region  Document Link with Form
        Task<List<DO_FormDocumentLink>> GetActiveDocumentControls();
        Task<List<DO_FormDocumentLink>> GetDocumentFormlink(int documentID);
        Task<DO_ReturnParameter> UpdateDocumentFormlink(List<DO_FormDocumentLink> obj);
        #endregion
    }
}
