﻿using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IFormsRepository
    {
        #region Form Master
        Task<List<DO_AreaController>> GetAreaController();

        Task<List<DO_Forms>> GetFormDetails();

        Task<List<DO_Forms>> GetInternalFormDetails();

        Task<DO_Forms> GetFormDetailsByID(int formID);

        Task<List<DO_Forms>> GetInternalFormByFormID(int formID);

        Task<DO_ReturnParameter> InsertUpdateIntoFormMaster(DO_Forms obj);

        Task<DO_Forms> GetNextInternalFormByID(int formID);

        Task<DO_ReturnParameter> InsertIntoInternalForm(DO_Forms obj);

        Task<List<DO_FormAction>> GetFormAction();

        Task<List<DO_FormAction>> GetFormActionByID(int formID);

        Task<DO_ReturnParameter> InsertIntoFormAction(DO_Forms obj);

        Task<List<DO_FormParameter>> GetFormParameterByID(int formID);
        Task<DO_ReturnParameter> InsertIntoFormParameter(DO_Forms obj);
        //Task<List<DO_FormSubParameter>> GetFormSubParameterByID(int formID, int parameterId);
        //Task<DO_ReturnParameter> InsertIntoFormSubParameter(DO_Forms obj);

        #endregion

        #region Area Controllers
        Task<List<DO_AreaController>> GetControllerbyArea(string Area);
        Task<List<DO_AreaController>> GetAllAreaController();

        Task<DO_ReturnParameter> InsertIntoAreaController(DO_AreaController obj);

        Task<DO_ReturnParameter> UpdateAreaController(DO_AreaController obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveAreaController(bool status, int Id);

        #endregion

        #region Define Actions 
        Task<List<DO_Actions>> GetAllActions();
        Task<DO_ReturnParameter> InsertIntoActions(DO_Actions obj);
        Task<DO_ReturnParameter> UpdateActions(DO_Actions obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveActions(bool status, int actionId);
        #endregion
    }
}
