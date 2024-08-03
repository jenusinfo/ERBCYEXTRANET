using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class ApplicationValidationProcess
	{
        public static List<ValidationResultModel> ValidateApplication(ApplicationViewModel applicationModel)
        {
            List<ValidationResultModel> retVal = new List<ValidationResultModel>();

            ValidationResultModel decision = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.DECISION_HISTORY
            };
            

            ValidationResultModel purposeAndActivity = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.BANK_ACCOUNT_PURPOSE_AND_ANTICIPATED_ACTIVITY
            };
            
            var applicationServiceGroup = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), null, '\0');
            bool isCard = false;
            string cardValue = applicationServiceGroup.Items.Where(x =>string.Equals( x.Label, "CARD",StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
            if (applicationModel.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Contains(cardValue))
            {
                isCard = true;
            }
            bool isEbanking = false;
            string eBankingValue = applicationServiceGroup.Items.Where(x => string.Equals(x.Label, "DIGITAL BANKING", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
            if (applicationModel.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Contains(eBankingValue))
            {
                isEbanking = true;
            }

            retVal.Add(ApplicationGridValidationProcess.ValidateApplicants(applicationModel.ApplicationNumber, applicationModel.ApplicationDetails.ApplicationDetails_ApplicationTypeName));
            bool isLegalEntity = false;
            if (string.Equals(applicationModel.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "Legal Entity", StringComparison.OrdinalIgnoreCase))
            {
                isLegalEntity = true;
                //if (applicationModel.GroupStructureLegalParent.DoesTheEntityBelongToAGroupName == "true")
                {
                    retVal.Add(ApplicationGridValidationProcess.ValidateGroupStructure(applicationModel.Id, applicationModel.ApplicationNumber, applicationModel.GroupStructureLegalParent));
                }
            }
            retVal.Add(ApplicationGridValidationProcess.ValidateRelatedParties(applicationModel.ApplicationNumber, applicationModel.ApplicationDetails.ApplicationDetails_ApplicationTypeName));
            purposeAndActivity = ApplicationFormBasicValidationProcess.ValidatePurposeAndActivity(applicationModel.PurposeAndActivity);
            retVal.Add(purposeAndActivity);
            retVal.Add(ApplicationGridValidationProcess.ValidateSourceOfInComingTransactions(applicationModel.Id));
            retVal.Add(ApplicationGridValidationProcess.ValidateSourceOfOutGoingTransactions(applicationModel.ApplicationNumber));
            retVal.Add(ApplicationGridValidationProcess.ValidateAccountDetails(applicationModel.Id));
            if (string.Equals(applicationModel.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "Legal Entity", StringComparison.OrdinalIgnoreCase))
            {
                retVal.Add(ApplicationGridValidationProcess.ValidateSignatoryGroupDetails(applicationModel.Id, applicationModel.ApplicationNumber));
                retVal.Add(ApplicationGridValidationProcess.ValidateSignatureMandateLegal(applicationModel.Id,applicationModel.ApplicationNumber));
            }
            else
            {
                var signatureMandateGroup = ServiceHelper.SignatureMandateTypeGroup();
                if(signatureMandateGroup != null && signatureMandateGroup.Count > 0 && applicationModel.PurposeAndActivity != null && applicationModel.PurposeAndActivity.SignatureMandateTypeGroup != null && !string.IsNullOrEmpty(applicationModel.PurposeAndActivity.SignatureMandateTypeGroup.RadioGroupValue) && signatureMandateGroup.Any(k => string.Equals(k.Value, applicationModel.PurposeAndActivity.SignatureMandateTypeGroup.RadioGroupValue) && (!string.Equals(k.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase) && !string.Equals(k.Label, "All Jointly", StringComparison.OrdinalIgnoreCase))))
				{
                    retVal.Add(ApplicationGridValidationProcess.ValidateSignatureMandate(applicationModel.ApplicationNumber));
                }
                
            }
            if (isEbanking)
            {
                retVal.Add(ApplicationGridValidationProcess.ValidateEBankingSubscribers(applicationModel.ApplicationNumber, isLegalEntity));
            }
            if (isCard && applicationModel.IsCardNew)
            {
                retVal.Add(ApplicationGridValidationProcess.ValidateDebitCardDetails(applicationModel.Id));
            }
            
            decision = ApplicationFormBasicValidationProcess.ValidateDecision(applicationModel.DecisionHistoryViewModel);
            retVal.Add(decision);

            //retVal.Add(ApplicationGridValidationProcess.ValidateNoteDetaills(applicationModel.ApplicationNumber));
           // retVal.Add(SectionValidator.ValidateApplicantionSections(applicationModel.Id));

            return retVal;
        }
    }
}
