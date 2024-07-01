using CMS.Helpers;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Common;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class LeftMenuProcess
    {
        public static List<StepperStep> GetApplicantConfiguration()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.Applicant") });
            container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.PersonalDetails"), Error = true });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.IdentificationDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.AddressDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.ContactDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.TaxDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.BusinessAndFinancialProfile") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.PEPDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.BankingRelationship") });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.Submit") });

            //StepperStepFactory retVal = new StepperStepFactory(container);
            //retVal.Add().Icon("user").Label("Personal Details").Enabled(true).Selected(true);
            //retVal.Add().Icon("calendar").Label("Employment Details");
            //retVal.Add().Icon("preview").Label("Address Details");

            //return retVal;
            return container;
        }

        public static List<StepperStep> GetApplicantConfigurationLegal()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.Applicant") });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.CompanyDetails"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.AddressDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.ContactDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.TaxDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.FATCADetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.CRSDetails") });
            //container.Add(new StepperStep() { Icon = "checkmark-circle", Label = "FATCA Details" });
            //container.Add(new StepperStep() { Icon = "checkmark-circle", Label = "CRS Details" });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.BusinessProfile") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.FinancialProfile") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.BankingRelationship") });
            //container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.FinancialProfile") });
            //container.Add(new StepperStep() { Icon = "checkmark-circle", Label = "Banking Relationship" });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.Submit") });

            //StepperStepFactory retVal = new StepperStepFactory(container);
            //retVal.Add().Icon("user").Label("Personal Details").Enabled(true).Selected(true);
            //retVal.Add().Icon("calendar").Label("Employment Details");
            //retVal.Add().Icon("preview").Label("Address Details");

            //return retVal;
            return container;
        }

        public static List<StepperStep> GetRelatedPartyConfiguration()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.RelatedParty") });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.PersonalDetails"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.IdentificationDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.AddressDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.ContactDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.BusinessProfile") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.PEPDetails") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.Roles") });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.Submit") });

            return container;
        }

        public static List<StepperStep> GetRelatedPartyConfigurationLegal()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.RelatedParty") });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.CompanyDetails"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.AddressDetails") });
            //container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.ContactDetails") });
            //container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.BusinessAndFinancialProfile") });
            container.Add(new StepperStep() { Icon = "video-external", Error = true, Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.Roles") });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.Submit") });

            return container;
        }

        public static List<StepperStep> GetApplicationConfiguration(ApplicationViewModel application)
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.Application"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.Applicants"), Error = true });
            if (application.ApplicationDetails != null && string.Equals(application.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
            {
                container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.GroupStructure"), Error = true });
            }
            container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.RelatedParties"), Error = true });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.PurposeAndActivity") });
            //container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.PurposeAndActivity"), Error = true });
            if (application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.Any(u => string.Equals(u.Label, ApplicationProductAndService.Account.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.Accounts"), Error = true });
            }
            container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.SignatureMandate"), Error = true });
            //if (application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.Any(u => string.Equals(u.Label.Replace("-", ""), ApplicationProductAndService.Ebanking.ToString(), StringComparison.OrdinalIgnoreCase)) && application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Any(i => string.Equals(i, application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.FirstOrDefault(u => string.Equals(u.Label.Replace("-", ""), ApplicationProductAndService.Ebanking.ToString(), StringComparison.OrdinalIgnoreCase)).Value, StringComparison.OrdinalIgnoreCase)))
            //{
            //    container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.EBanking"), Error = true, SuccessIcon = "checkmark-circle" });
            //}
            //if (application.IsCardNew && application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.Any(u => string.Equals(u.Label, ApplicationProductAndService.Card.ToString(), StringComparison.OrdinalIgnoreCase)) && application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Any(i => string.Equals(i, application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.FirstOrDefault(u => string.Equals(u.Label.Replace("-", ""), ApplicationProductAndService.Card.ToString(), StringComparison.OrdinalIgnoreCase)).Value, StringComparison.OrdinalIgnoreCase)))
            //{
            //    container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.DebitCards"), Error = true });
            //}
			container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.EBanking"), Error = true, SuccessIcon = "checkmark-circle" });
			container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.DebitCards"), Error = true });
			container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.Documents"), Error = true });
            container.Add(new StepperStep() { Icon = "video-external", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.Notes"), Error = true });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.LeftMenu.Submit") });
            //StepperStepFactory retVal = new StepperStepFactory(container);
            //retVal.Add().Icon("user").Label("Personal Details").Enabled(true).Selected(true);
            //retVal.Add().Icon("calendar").Label("Employment Details");
            //retVal.Add().Icon("preview").Label("Address Details");

            //return retVal;
            return container;
        }


        //public static StepperStepFactory GetApplicantConfiguration()
        //{
        //	List<StepperStep> container = new List<StepperStep>();
        //	//container.Add(new StepperStep() { Icon = "user", Label = "Personal Details", Enabled = true, Selected = true  });
        //	//container.Add(new StepperStep() { Icon = "user", Label = "Employment Details" });
        //	//container.Add(new StepperStep() { Icon = "user", Label = "Address Details" });

        //	StepperStepFactory retVal = new StepperStepFactory(container);
        //	retVal.Add().Icon("user").Label("Personal Details").Enabled(true).Selected(true);
        //	retVal.Add().Icon("calendar").Label("Employment Details");
        //	retVal.Add().Icon("preview").Label("Address Details");

        //	return retVal;
        //}

        #region Menu Validation
        #endregion
        public static List<StepperStep> GetApplicantConfigurationCreate()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.Applicant") });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.PersonalDetails"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.Applicant.LeftMenu.Submit") });
            return container;
        }
        public static List<StepperStep> GetApplicantConfigurationLegalCreate()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.Applicant") });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.CompanyDetails"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.Applicant.LegalEntity.LeftMenu.Submit") });
            return container;
        }
        public static List<StepperStep> GetRelatedPartyConfigurationLegalCreate()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.RelatedParty") });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.CompanyDetails"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LegalEntity.LeftMenu.Submit") });
            return container;
        }
        public static List<StepperStep> GetRelatedPartyConfigurationCreate()
        {
            List<StepperStep> container = new List<StepperStep>();
            container.Add(new StepperStep() { Icon = "user", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.RelatedParty") });
            container.Add(new StepperStep() { Icon = "checkmark-circle", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.PersonalDetails"), Enabled = true, Selected = true });
            container.Add(new StepperStep() { Icon = "radiobutton-checked", Label = ResHelper.GetString("Eurobank.Application.RelatedParty.LeftMenu.Submit") });
            return container;
        }
    }
}
