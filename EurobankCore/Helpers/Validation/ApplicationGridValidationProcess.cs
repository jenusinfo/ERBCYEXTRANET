using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.RelatedParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class ApplicationGridValidationProcess
    {
        public static ValidationResultModel ValidateApplicants(string applicationNumber, string applicationType)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.APPLICANTS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (!string.IsNullOrEmpty(applicationNumber) && !string.IsNullOrEmpty(applicationType))
            {
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationNumber);
                }
                if (applicants == null || applicants.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Grid;
                    lstvalidationError.Add(validationError);
                }
                else if (string.Equals(applicationType, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase) && applicants.Count < 2)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Joint_Grid_Count;
                    lstvalidationError.Add(validationError);
                }
                else if (string.Equals(applicationType, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase) && applicants.Count > 1)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Individual_Count_NotMoreThanOne;
                    lstvalidationError.Add(validationError);
                }
                else if (string.Equals(applicationType, "Legal Entity", StringComparison.OrdinalIgnoreCase) && applicants.Count > 1)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Legal_Count_NotMoreThanOne;
                    lstvalidationError.Add(validationError);
                }
                if (applicants != null && applicants.Any(y => string.Equals(y.Status, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateRelatedParties(string applicationNumber, string applicationType)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.RELATED_PARTIES
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (!string.IsNullOrEmpty(applicationNumber) && !string.IsNullOrEmpty(applicationType))
            {
                List<RelatedPartyModel> relatedParties = null;
                if (string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    relatedParties = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber);
                    if (relatedParties == null || relatedParties.Count == 0)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Relatedparty_Grid;
                        lstvalidationError.Add(validationError);
                    }

                    if (relatedParties != null && relatedParties.Any(y => string.Equals(y.Type, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase)) && !relatedParties.Any(y => y.Role.Contains("Authorised Person", StringComparison.OrdinalIgnoreCase)))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.RelatedPartyRoles_HasAuthorisedPerson;
                        lstvalidationError.Add(validationError);
                    }

                }
                else
                {
                    relatedParties = RelatedPartyProcess.GetRelatedPartyModels(applicationNumber);
                }

                if (relatedParties != null && relatedParties.Any(y => string.Equals(y.Status, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateAccountDetails(int applicantionId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ACCOUNTS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantionId > 0)
            {
                var accountDetails = AccountsProcess.GetAccountsByApplicationID(applicantionId);
                if (accountDetails == null || accountDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Accounts_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (accountDetails != null && accountDetails.Any(y => string.Equals(y.Account_Status_Name, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateDebitCardDetails(int applicantionId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.CARDS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantionId > 0)
            {
                var debitCardDetails = DebitCardeDetailsProcess.GetDebitCardDetailsByApplicationID(applicantionId);
                if (debitCardDetails == null || debitCardDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Debit_Card_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (debitCardDetails != null && debitCardDetails.Any(y => string.Equals(y.DebitCardDetails_StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateEBankingSubscribers(string applicationNumber, bool isLegalEntity)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.EBANKING_SUBSCRIBERS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (!string.IsNullOrEmpty(applicationNumber))
            {
                var ebankingSubscriber = EBankingSubscriberDetailsProcess.GetEBankingSubscriberDetailsModels(applicationNumber, isLegalEntity);
                if (ebankingSubscriber == null || ebankingSubscriber.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Ebanking_Subscriber_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (ebankingSubscriber != null && ebankingSubscriber.Any(y => string.Equals(y.Status_Name, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        public static ValidationResultModel ValidateSignatoryGroupDetails(int applicantionId, string applicationNumber)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATORY_GROUP
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (applicantionId > 0)
            {
                var signatoryGroup = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicantionId);
                if (signatoryGroup == null || signatoryGroup.Count == 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Signatory_Group_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (signatoryGroup != null && signatoryGroup.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    lstvalidationError.Add(validationError);
                }
                if (signatoryGroup != null)
                {
                    List<RelatedPartyModel> authorisedPersonRelatedParties = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber).Where(y => y.Role.Contains("Authorised Person")).ToList();
                    if (authorisedPersonRelatedParties != null && authorisedPersonRelatedParties.Count > 0)
                    {
                        foreach (var item in authorisedPersonRelatedParties)
                        {
                            var isExistInSignatoryGroup = signatoryGroup.Where(x => x.SignatoryPersonsList.Contains(item.NodeGUID) && x.SignatoryGroup.ToUpper()== "AUTHORISED PERSONS").FirstOrDefault();
                            if (isExistInSignatoryGroup == null)
                            {
                                ValidationError validationError = new ValidationError();
                                retVal.IsValid = false;
                                validationError.ErrorMessage = item.FullName.ToUpper() +" "+ "must belong to the AUTHORISED PERSONS Group";
                                lstvalidationError.Add(validationError);
                            }
                        }
                    }
                    List<RelatedPartyModel> relatedParties = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber).Where(y => y.Role.Contains("Authorised Person") || y.Role.Contains("Authorised Signatory")).ToList();
                    if (relatedParties.Any(y => !signatoryGroup.Any(x => x.SignatoryPersons.Contains(y.NodeGUID))))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.SignatoryGroupWithAuthorizePerson;
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        public static ValidationResultModel ValidateNoteDetaills(string applicationNumber)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.CARDS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (!string.IsNullOrEmpty(applicationNumber))
            {
                var debitCardDetails = NoteDetailsProcess.GetNoteDetailsModels(applicationNumber);
                if (debitCardDetails != null && debitCardDetails.Any(y => string.Equals(y.Status_Name, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateSignatureMandate(string applicationNumber)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (!string.IsNullOrEmpty(applicationNumber))
            {
                var signatureMandate = SignatureMandateIndividualProcess.GetSignatureMandateIndividualModels(applicationNumber);
                if (signatureMandate == null || signatureMandate.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Signature_Mandate_Grid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (signatureMandate != null && signatureMandate.Any(y => string.Equals(y.Status_Name, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateSignatureMandateLegal(int applicantionId, string applicationNumber)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (!string.IsNullOrEmpty(applicationNumber))
            {
                var signatureMandate = SignatureMandateLegalProcess.GetSignatureMandateLegalModels(applicationNumber);
                if (signatureMandate == null || signatureMandate.Count == 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Signature_Mandate_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (signatureMandate != null && signatureMandate.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (signatureMandate != null && signatureMandate.Count > 0)
                {
                    var signatoryGroup = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicantionId).Where(x=>x.SignatoryGroup!= "AUTHORISED PERSONS");
                    if (signatoryGroup != null)
                    {
                        if (signatoryGroup.Any(y => !signatureMandate.Any(x => x.AuthorizedSignatoryGroup.Contains(y.SignatoryGroupName) || x.AuthorizedSignatoryGroup1.Contains(y.SignatoryGroupName))))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.SignatoryGroupList;
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
                if (signatureMandate != null && !signatureMandate.Any(y => string.Equals(y.MandateTypeName, "FOR AUTHORISED PERSONS", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.AuthorizePersonCumpulsory;
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateSourceOfInComingTransactions(int applicantionId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.COUNTERPARTIES_OF_INCOMING_TRANSACTION
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantionId > 0)
            {
                var sourceOfIncoming = SourceOfIncomingTransactionsProcess.GetSourceOfIncomeByApplicationID(applicantionId);
                if (sourceOfIncoming == null || sourceOfIncoming.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Source_Of_Incoming_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (sourceOfIncoming != null && sourceOfIncoming.Any(y => string.Equals(y.SourceOfIncomingTransactions_Status_Name, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateSourceOfOutGoingTransactions(string applicantionNumber)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.COUNTERPARTIES_OF_OUTGOING_TRANSACTION
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (!string.IsNullOrEmpty(applicantionNumber))
            {
                var debitCardDetails = SourceOfOutgoingTransactionProcess.GetSourceOfOutgoingTransactionModels(applicantionNumber);
                if (debitCardDetails == null || debitCardDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Source_Of_Outgoing_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (debitCardDetails != null && debitCardDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateGroupStructure(int applicantionId, string applicationNumber, GroupStructureLegalParentModel groupStructureLegalParentModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.GROUP_STRUCTURE
            };
            string appplicantEntityType = string.Empty;
            string EntityName = string.Empty;
            List<ApplicantModel> applicantLegal = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
            if (applicantLegal != null && applicantLegal.Count > 0 && applicantLegal.FirstOrDefault().CompanyDetails != null)
            {
                appplicantEntityType = applicantLegal.FirstOrDefault().CompanyDetails.EntityType;
                EntityName = ServiceHelper.GetName(appplicantEntityType, Constants.COMPANY_ENTITY_TYPE);

            }
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantionId > 0 && groupStructureLegalParentModel != null && !string.Equals(EntityName, "Trust", StringComparison.OrdinalIgnoreCase) && !string.Equals(EntityName, "Provident Fund", StringComparison.OrdinalIgnoreCase) && !string.Equals(EntityName, "Pension Fund", StringComparison.OrdinalIgnoreCase))
            {
                if (groupStructureLegalParentModel.DoesTheEntityBelongToAGroupName == null)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Group_Structure_Grid;
                    lstvalidationError.Add(validationError);
                }
                if (string.Equals(groupStructureLegalParentModel.DoesTheEntityBelongToAGroupName, "true", StringComparison.OrdinalIgnoreCase))
                {
                    var groupStructure = CompanyGroupStructureProcess.GetCompanyGroupStructure(applicantionId, applicationNumber);
                    if (groupStructure == null || groupStructure.Count == 0)
                    {
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Group_Structure_Grid;
                        lstvalidationError.Add(validationError);
                    }
                    if (groupStructure != null && groupStructure.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                    {
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                        lstvalidationError.Add(validationError);
                    }
                    //if (string.Equals(groupStructureLegalParentModel.DoesTheEntityBelongToAGroupName, "true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(groupStructureLegalParentModel.GroupActivities))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.Group_Structure_GroupActivities;
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(groupStructureLegalParentModel.GroupName))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.Group_Structure_GroupName;
                            lstvalidationError.Add(validationError);
                        }
                    }
                }

            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
    }
}
