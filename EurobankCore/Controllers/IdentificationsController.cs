using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using DocumentFormat.OpenXml.EMMA;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Generics;
using Eurobank.Helpers.Process;
using Eurobank.Helpers.Validation;
using Eurobank.Models.IdentificationDetails;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Controllers
{
    public class IdentificationsController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly IdentificationDetailsRepository identificationDetailsRepository;
        public IdentificationsController(IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IPageAttachmentUrlRetriever attachmentUrlRetriever, IdentificationDetailsRepository identificationDetailsRepository)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
            this.identificationDetailsRepository = identificationDetailsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region Identification
        public ActionResult IdentificationDetails(string NodeGUID, string NodeAliaspath)
        {

            ViewBag.CountryIdentification = ServiceHelper.GetCountriesWithID();
            ViewBag.identificationType = ServiceHelper.GetTypeIdentification();
            IdentificationDetailsViewModel identificationDetailsViewModel = new IdentificationDetailsViewModel();
            identificationDetailsViewModel.PnodeGUID = NodeGUID;
            identificationDetailsViewModel.NodeAliaspath = NodeAliaspath;
            return PartialView(identificationDetailsViewModel);
        }
        public IActionResult Identification_Read([DataSourceRequest] DataSourceRequest request, string pNodeAliaspath)
        {
            List<IdentificationDetailsViewModel> identificationDetailsViewModels = new List<IdentificationDetailsViewModel>();
            var identification = identificationDetailsRepository.GetIdentifications(pNodeAliaspath);
            if (identification != null)
            {
                identificationDetailsViewModels = IdentificationDetailsProcess.GetIdetifications(identification);
            }
            return Json(identificationDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult IdentificationPopup_Create([DataSourceRequest] DataSourceRequest request, IdentificationDetailsViewModel identificationDetailsViewModel, string pNodeAliaspath, string nodeGUID, string applicationType, int ApplicantId)
        {
            if (identificationDetailsViewModel.Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateIdentificationDetails(identificationDetailsViewModel, applicationType, ApplicantId);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (identificationDetailsViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(identificationDetailsViewModel))
            {
                TreeNode treeNode = ServiceHelper.GetTreeNode(nodeGUID, pNodeAliaspath);
                if (treeNode != null)
                {
                    identificationDetailsViewModel.PnodeGUID = nodeGUID;
                    identificationDetailsViewModel.NodeAliaspath = pNodeAliaspath;
                    var successData = IdentificationDetailsProcess.SaveIdendificationsModel(identificationDetailsViewModel, treeNode);
                    identificationDetailsViewModel = successData;

                }
                //PersonsRegistryCreate(personsRegistry);
                //personsRegistry.AddressTypeName = GetAddressName(personsRegistry.AddressType);



            }
            return Json(new[] { identificationDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult IdentificationPopup_Update([DataSourceRequest] DataSourceRequest request, IdentificationDetailsViewModel identificationDetailsViewModel, string applicationType, int ApplicantId)
        {
            if (identificationDetailsViewModel.Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateIdentificationDetails(identificationDetailsViewModel, applicationType, ApplicantId);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (identificationDetailsViewModel != null && ModelState.IsValid)
            {
                var identification = identificationDetailsRepository.GetIdentificationDetailsByID(identificationDetailsViewModel.IdentificationDetailsID);

                if (identification != null)
                {

                    var successData = IdentificationDetailsProcess.UpdateIdendificationsModel(identificationDetailsViewModel, identification);
                }
            }
            return Json(new[] { identificationDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult IdentificationPopup_Destroy([DataSourceRequest] DataSourceRequest request, IdentificationDetailsViewModel identificationDetailsViewModel)
        {
            if (identificationDetailsViewModel != null)
            {
                identificationDetailsRepository.GetIdentificationDetailsByID(identificationDetailsViewModel.IdentificationDetailsID).DeleteAllCultures();
            }
            return Json(new[] { identificationDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion
        public JsonResult ValidateIdentificationDates(string issueDate, string expiryDate)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(issueDate))
            {
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(issueDate))
            {
                if (DateTime.Today < DateTime.ParseExact(issueDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                {
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(expiryDate))
            {
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(expiryDate))
            {
                if (DateTime.ParseExact(issueDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) > DateTime.ParseExact(expiryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                {
                    isValid = false;
                }
            }
            return Json(isValid);
        }
    }
}
