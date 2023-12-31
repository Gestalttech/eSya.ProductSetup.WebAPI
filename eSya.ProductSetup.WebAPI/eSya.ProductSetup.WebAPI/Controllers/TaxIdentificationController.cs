﻿using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaxIdentificationController : ControllerBase
    {
        private readonly ITaxIdentificationRepository _TaxIdentificationRepository;
        public TaxIdentificationController(ITaxIdentificationRepository TaxIdentificationRepository)
        {
            _TaxIdentificationRepository = TaxIdentificationRepository;
        }

        #region Tax Identification
        /// <summary>
        /// Get Tax Identification for specific ISD Code.
        /// UI Reffered - Fill Grid on ISD Code selection in Tax Identification
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaxIdentificationByISDCode(int ISDCode)
        {
            var tax_structs = await _TaxIdentificationRepository.GetTaxIdentificationByISDCode(ISDCode);
            return Ok(tax_structs);
        }

        /// <summary>
        /// Insert into  Tax Identification Table
        /// UI Reffered -  Tax Identification,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoTaxIdentification(DO_TaxIdentification obj)
        {
            var msg = await _TaxIdentificationRepository.InsertIntoTaxIdentification(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into  Tax Identification Table
        /// UI Reffered -  Tax Identification,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateTaxIdentification(DO_TaxIdentification obj)
        {
            var msg = await _TaxIdentificationRepository.UpdateTaxIdentification(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Tax Identification.
        /// UI Reffered - Tax Identification
        /// </summary>
        /// <param name="status-Isd_code-TaxIdentificationId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveTaxIdentification(bool status, int Isd_code, int TaxIdentificationId)
        {
            var msg = await _TaxIdentificationRepository.ActiveOrDeActiveTaxIdentification(status, Isd_code, TaxIdentificationId);
            return Ok(msg);
        }
        #endregion


    }
}
