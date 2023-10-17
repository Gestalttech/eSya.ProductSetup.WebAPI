﻿using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface ILicenseRepository
    {
        #region Business Entity

        Task<List<DO_BusinessEntity>> GetBusinessEntities();

        Task<DO_BusinessEntity> GetBusinessEntityInfo(int BusinessId);

        Task<DO_ReturnParameter> InsertBusinessEntity(DO_BusinessEntity obj);

        Task<DO_ReturnParameter> UpdateBusinessEntity(DO_BusinessEntity obj);

        Task<DO_ReturnParameter> DeleteBusinessEntity(int BusinessEntityId);

        Task<List<DO_BusinessEntity>> GetActiveBusinessEntities();

        Task<List<DO_EntityPreferredLanguage>> GetPreferredLanguagebyBusinessKey(int BusinessId);

        #endregion  Business Entity

        #region Business Subscription

        Task<List<DO_BusinessSubscription>> GetBusinessSubscription(int BusinessKey);

        Task<DO_ReturnParameter> InsertOrUpdateBusinessSubscription(DO_BusinessSubscription businesssubs);

        #endregion  Business Subscription

        #region  Business Location
        Task<List<DO_BusinessLocation>> GetBusinessLocationByBusinessId(int BusinessId);
        Task<DO_ReturnParameter> InsertBusinessLocation(DO_BusinessLocation obj);
        Task<DO_ReturnParameter> UpdateBusinessLocation(DO_BusinessLocation obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveBusinessLocation(bool status, int BusinessId, int locationId);
        Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int IsdCode);
        Task<List<DO_CountryCodes>> GetCurrencyListbyIsdCode(int IsdCode);
        Task<List<DO_BusinessLocation>> GetActiveLocationsAsSegments();
        Task<List<DO_Cities>> GetCityListbyISDCode(int isdCode);
        Task<DO_TaxIdentification> GetStateCodeByISDCode(int isdCode, int TaxIdentificationId);
        Task<List<DO_BusienssSegmentCurrency>> GetCurrencybyBusinessKey(int BusinessKey);
        Task<DO_BusinessEntity> GetBusinessUnitType(int businessId);
        #endregion

        #region Define User Role Action
        Task<List<DO_ApplicationCodes>> GetUserRoleByCodeType(int codeType);
        Task<List<DO_UserRoleActionLink>> GetUserRoleActionLink(int userRole);
        Task<DO_ReturnParameter> InsertOrUpdateUpdateUserRoleActionLink(List<DO_UserRoleActionLink> obj);
        #endregion

        #region Define Menu Link to Location
        Task<DO_ConfigureMenu> GetLocationMenuLinkbyBusinessKey(int businesskey);

        Task<DO_ReturnParameter> InsertOrUpdateLocationMenuLink(List<DO_LocationMenuLink> obj);
        #endregion
    }
}
