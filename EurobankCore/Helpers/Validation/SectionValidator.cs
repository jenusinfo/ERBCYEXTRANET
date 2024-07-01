using Eurobank.Helpers.Process;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.Applications;
using Eurobank.Models.Applications.Accounts;
using Eurobank.Models.Applications.DebitCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class SectionValidator
	{
		#region Application

		public static ValidationResultModel ValidateApplicantionSections(int applicationId)
		{
			ValidationResultModel retVal = new ValidationResultModel() {
				IsValid = false,
				Errors = new List<ValidationError>()
			};

			if(applicationId > 0)
			{
				retVal.IsValid = true;
				ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationDetailsModelById(applicationId);
				if(applicationDetailsModel != null && !string.IsNullOrEmpty(applicationDetailsModel.ApplicationDetails_ApplicationNumber) && !string.IsNullOrEmpty(applicationDetailsModel.ApplicationDetails_ApplicationTypeName))
				{
					//Applicant
					ValidationResultModel applicantCountValidation = ValidateAppplicantCountBasedOnApplicationType(applicationDetailsModel);
					if(!applicantCountValidation.IsValid && applicantCountValidation.Errors != null && applicantCountValidation.Errors.Count > 0)
					{
						retVal.IsValid = applicantCountValidation.IsValid;
						retVal.Errors.AddRange(applicantCountValidation.Errors);
					}
					//Group Structure
					bool isLegalEntity = false;
					if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Legal Entity", StringComparison.OrdinalIgnoreCase))
					{
						isLegalEntity = true;
						ValidationResultModel groupStructureCountValidation = ValidateGroupStructureCountLegalApplication(applicationDetailsModel);
						if(!groupStructureCountValidation.IsValid && groupStructureCountValidation.Errors != null && groupStructureCountValidation.Errors.Count > 0)
						{
							retVal.IsValid = groupStructureCountValidation.IsValid;
							retVal.Errors.AddRange(groupStructureCountValidation.Errors);
						}
					}
					//Related Party
					ValidationResultModel relatedPartyCountValidation = ValidateRealtedPartyCountBasedOnApplicationType(applicationDetailsModel);
					if(!relatedPartyCountValidation.IsValid && relatedPartyCountValidation.Errors != null && relatedPartyCountValidation.Errors.Count > 0)
					{
						retVal.IsValid = relatedPartyCountValidation.IsValid;
						retVal.Errors.AddRange(relatedPartyCountValidation.Errors);
					}
					//Account
					ValidationResultModel accountCountValidation = ValidateAccountCount(applicationDetailsModel);
					if(!accountCountValidation.IsValid && accountCountValidation.Errors != null && accountCountValidation.Errors.Count > 0)
					{
						retVal.IsValid = accountCountValidation.IsValid;
						retVal.Errors.AddRange(accountCountValidation.Errors);
					}
					//Debit Card
					ValidationResultModel debitCardCountValidation = ValidateDebitCardCount(applicationDetailsModel);
					if(!debitCardCountValidation.IsValid && debitCardCountValidation.Errors != null && debitCardCountValidation.Errors.Count > 0)
					{
						retVal.IsValid = debitCardCountValidation.IsValid;
						retVal.Errors.AddRange(debitCardCountValidation.Errors);
					}
					//E-Banking
					ValidationResultModel eBankingCountValidation = ValidateEBankingSubscriberCount(applicationDetailsModel, isLegalEntity);
					if(!eBankingCountValidation.IsValid && eBankingCountValidation.Errors != null && eBankingCountValidation.Errors.Count > 0)
					{
						retVal.IsValid = eBankingCountValidation.IsValid;
						retVal.Errors.AddRange(eBankingCountValidation.Errors);
					}
					//Signature Mandate
					ValidationResultModel signatureMandateValidation = ValidateSignatureMandateCount(applicationDetailsModel);
					if(!signatureMandateValidation.IsValid && signatureMandateValidation.Errors != null && signatureMandateValidation.Errors.Count > 0)
					{
						retVal.IsValid = signatureMandateValidation.IsValid;
						retVal.Errors.AddRange(signatureMandateValidation.Errors);
					}
				}


			}

			return retVal;
		}

		#endregion

		#region Applicants

		//public static ValidationResultModel ValidateApplicants(List<ApplicantModel> applicants)
		//{
		//	ValidationResultModel retVal = new ValidationResultModel() { IsValid = false, MessageCode = Convert.ToString(ValidationErrorCode.InvalidApplicant) };

		//	return retVal;
		//}

		#endregion

		#region Application Privte Methods

		private static ValidationResultModel ValidateAppplicantCountBasedOnApplicationType(ApplicationDetailsModelView applicationDetailsModel)
		{
			ValidationResultModel retVal = new ValidationResultModel()
			{
				IsValid = true,
				Errors = new List<ValidationError>(),
				ApplicationModuleName = ApplicationModule.APPLICATION_APPLICANT
			};

			//Applicant section
			int applicationCount = 0;
			List<ApplicantModel> applicants = null;
			if(string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Legal Entity", StringComparison.OrdinalIgnoreCase))
			{
				applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
			}
			else
			{
				applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
			}

			if(applicants != null)
			{
				applicationCount = applicants.Count;
			}

			if(applicationCount == 0)
			{
				retVal.IsValid = false;
				retVal.ApplicationModuleName = ApplicationModule.APPLICATION_APPLICANT;
				retVal.Errors.Add(new ValidationError()
				{
					ErrorMessage = ValidationConstant.Applicant_Count_CanNotBeZero
				});
			}
			else if(applicationCount > 1)
			{
				if(string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Individual", StringComparison.OrdinalIgnoreCase))
				{
					retVal.IsValid = false;
					retVal.ApplicationModuleName = ApplicationModule.APPLICATION_APPLICANT;
					retVal.Errors.Add(new ValidationError()
					{
						ErrorMessage = ValidationConstant.Applicant_Individual_Count_NotMoreThanOne
					});
				}
				else if(string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Legal Entity", StringComparison.OrdinalIgnoreCase))
				{
					retVal.IsValid = false;
					retVal.ApplicationModuleName = ApplicationModule.APPLICATION_APPLICANT;
					retVal.Errors.Add(new ValidationError()
					{
						ErrorMessage = ValidationConstant.Applicant_Legal_Count_NotMoreThanOne
					});
				}
			}
			else if(applicationCount == 1 && string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Joint Individual", StringComparison.OrdinalIgnoreCase))
			{
				retVal.IsValid = false;
				retVal.ApplicationModuleName = ApplicationModule.APPLICATION_APPLICANT;
				retVal.Errors.Add(new ValidationError()
				{
					ErrorMessage = ValidationConstant.Applicant_Joint_Count_ShouldMoreThanOne
				});
			}

			return retVal;
		}

		private static ValidationResultModel ValidateGroupStructureCountLegalApplication(ApplicationDetailsModelView applicationDetailsModel)
		{
			ValidationResultModel retVal = new ValidationResultModel()
			{
				IsValid = true,
				Errors = new List<ValidationError>(),
				ApplicationModuleName = ApplicationModule.GROUP_STRUCTURE
            };

			GroupStructureLegalParentModel groupStructureLegalParentModel = GroupStructureLegalParentProcess.GetGroupStructureLegalParentModel(applicationDetailsModel.ApplicationDetails_ApplicationNumber);

			if(groupStructureLegalParentModel != null && groupStructureLegalParentModel.DoesTheEntityBelongToAGroupName == "true")
			{
				List<CompanyGroupStructureModel> companyGroupStructureModels = CompanyGroupStructureProcess.GetCompanyGroupStructure(applicationDetailsModel.ApplicationDetailsID, applicationDetailsModel.ApplicationDetails_ApplicationNumber);
				if(companyGroupStructureModels == null || companyGroupStructureModels.Count < 2)
				{
					retVal.IsValid = false;
					retVal.ApplicationModuleName = ApplicationModule.GROUP_STRUCTURE;
					retVal.Errors.Add(new ValidationError()
					{
						ErrorMessage = ValidationConstant.Application_GroupStructure_Count_NotLessThanTwo
					});
				}
			}

			return retVal;
		}

		private static ValidationResultModel ValidateRealtedPartyCountBasedOnApplicationType(ApplicationDetailsModelView applicationDetailsModel)
		{
			ValidationResultModel retVal = new ValidationResultModel()
			{
				IsValid = true,
				Errors = new List<ValidationError>(),
				ApplicationModuleName = ApplicationModule.APPLICATION_RELATED_PARTY
			};

			int relatedPartyCount = 0;
			List<RelatedPartyModel> relatedParties = null;
			if(string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Legal Entity", StringComparison.OrdinalIgnoreCase))
			{
				relatedParties = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
				if(relatedParties != null)
				{
					relatedPartyCount = relatedParties.Count;
				}
				if(relatedPartyCount == 0)
				{
					retVal.IsValid = false;
					retVal.ApplicationModuleName = ApplicationModule.APPLICATION_RELATED_PARTY;
					retVal.Errors.Add(new ValidationError()
					{
						ErrorMessage = ValidationConstant.RelatedParty_Legal_Count_CanNotBeZero
					});
				}
			}

			return retVal;
		}

		private static ValidationResultModel ValidateAccountCount(ApplicationDetailsModelView applicationDetailsModel)
		{
			ValidationResultModel retVal = new ValidationResultModel()
			{
				IsValid = true,
				Errors = new List<ValidationError>(),
				ApplicationModuleName = ApplicationModule.APPLICATION_ACCOUNT
			};

			int accountsCount = 0;
			List<AccountsDetailsViewModel> accountsDetailsViewModel = null;
			accountsDetailsViewModel = AccountsProcess.GetAccountsByApplicationID(applicationDetailsModel.ApplicationDetailsID);
			if(accountsDetailsViewModel != null)
			{
				accountsCount = accountsDetailsViewModel.Count;
			}
			if(accountsCount == 0)
			{
				retVal.IsValid = false;
				retVal.ApplicationModuleName = ApplicationModule.APPLICATION_ACCOUNT;
				retVal.Errors.Add(new ValidationError()
				{
					ErrorMessage = ValidationConstant.Accounts_Count_CanNotBeZero
				});
			}

			return retVal;
		}

		private static ValidationResultModel ValidateDebitCardCount(ApplicationDetailsModelView applicationDetailsModel)
		{
			ValidationResultModel retVal = new ValidationResultModel()
			{
				IsValid = true,
				Errors = new List<ValidationError>(),
				ApplicationModuleName = ApplicationModule.CARDS
            };

			int debitCardCount = 0;
			List<DebitCardDetailsViewModel> debitCardDetailsViewModel = null;
			debitCardDetailsViewModel = DebitCardeDetailsProcess.GetDebitCardDetailsByApplicationID(applicationDetailsModel.ApplicationDetailsID);
			if(debitCardDetailsViewModel != null)
			{
				debitCardCount = debitCardDetailsViewModel.Count;
			}
			if(debitCardCount == 0)
			{
				retVal.IsValid = false;
				retVal.ApplicationModuleName = ApplicationModule.CARDS;
				retVal.Errors.Add(new ValidationError()
				{
					ErrorMessage = ValidationConstant.DebitCard_Count_CanNotBeZero
				});
			}

			return retVal;
		}

		private static ValidationResultModel ValidateEBankingSubscriberCount(ApplicationDetailsModelView applicationDetailsModel,bool isLegalEntity)
		{
			ValidationResultModel retVal = new ValidationResultModel()
			{
				IsValid = true,
				Errors = new List<ValidationError>(),
				ApplicationModuleName = ApplicationModule.EBANKING_SUBSCRIBERS
            };

			int eBankingSubscriberCount = 0;
			List<EBankingSubscriberDetailsModel> eBankingSubscriberDetailsModel = null;
			eBankingSubscriberDetailsModel = EBankingSubscriberDetailsProcess.GetEBankingSubscriberDetailsModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber, isLegalEntity);
			if(eBankingSubscriberDetailsModel != null)
			{
				eBankingSubscriberCount = eBankingSubscriberDetailsModel.Count;
			}
			if(eBankingSubscriberCount == 0)
			{
				retVal.IsValid = false;
				retVal.ApplicationModuleName = ApplicationModule.EBANKING_SUBSCRIBERS;
				retVal.Errors.Add(new ValidationError()
				{
					ErrorMessage = ValidationConstant.EBankingSubscriber_Count_CanNotBeZero
				});
			}

			return retVal;
		}

		private static ValidationResultModel ValidateSignatureMandateCount(ApplicationDetailsModelView applicationDetailsModel)
		{
			ValidationResultModel retVal = new ValidationResultModel()
			{
				IsValid = true,
				Errors = new List<ValidationError>(),
				ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE
			};

			int eBankingSubscriberCount = 0;
			List<SignatureMandateIndividualModel> signatureMandateIndividualModel = null;
			signatureMandateIndividualModel = SignatureMandateIndividualProcess.GetSignatureMandateIndividualModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
			if(signatureMandateIndividualModel != null)
			{
				eBankingSubscriberCount = signatureMandateIndividualModel.Count;
			}

			if(string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Legal Entity", StringComparison.OrdinalIgnoreCase))
			{
				if(eBankingSubscriberCount == 0)
				{
					retVal.IsValid = false;
					retVal.ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE;
					retVal.Errors.Add(new ValidationError()
					{
						ErrorMessage = ValidationConstant.SignatureMandate_Legal_Count_NotBeZero
					});
				}
			}
			else if(string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Joint Individual", StringComparison.OrdinalIgnoreCase))
			{
				if(eBankingSubscriberCount < 2)
				{
					retVal.IsValid = false;
					retVal.ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE;
					retVal.Errors.Add(new ValidationError()
					{
						ErrorMessage = ValidationConstant.SignatureMandate_JointIndividual_Count_ShouldBeTwo
					});
				}
			}
			else if(string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "Individual", StringComparison.OrdinalIgnoreCase))
			{
				if(eBankingSubscriberCount != 0)
				{
					retVal.IsValid = false;
					retVal.ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE;
					retVal.Errors.Add(new ValidationError()
					{
						ErrorMessage = ValidationConstant.SignatureMandate_Individual_Count_ShouldBeZero
					});
				}
			}

			

			return retVal;
		}

		#endregion
	}
}
