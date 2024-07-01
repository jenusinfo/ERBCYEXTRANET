using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.IdentificationDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class ApplicantProcess
	{
		//Need to ask and add applicant fields showing in grid
		public static List<ApplicantModel> GetApplicantModels(string applicationNumber)
		{
			List<ApplicantModel> retVal = null;

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				List<PersonalDetailsModel> PersonalDetailsModels = PersonalDetailsProcess.GetApplicantPersonalDetails(applicationNumber);
				if(PersonalDetailsModels != null && PersonalDetailsModels.Count > 0)
				{
					retVal = new List<ApplicantModel>();
					foreach(PersonalDetailsModel personalDetailsModel in PersonalDetailsModels)
					{
						if(personalDetailsModel != null)
						{
							List<IdentificationDetailsViewModel> identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							ApplicantModel applicantModel = new ApplicantModel();
							applicantModel.PersonalDetails = personalDetailsModel;
							applicantModel.Type = ServiceHelper.GetName(ValidationHelper.GetString(personalDetailsModel.Type, ""), "/Lookups/General/APPLICATION-TYPE");
							applicantModel.IdVerified = personalDetailsModel.IdVerified==true?"YES": Constants.Pending;
							if (personalDetailsModel.Invited == 0)
							{
								applicantModel.Invited = Constants.Pending;
							}
							else if (personalDetailsModel.Invited == 1)
							{
								applicantModel.Invited = Constants.Invited;
							}
							else if (personalDetailsModel.Invited == 2)
							{
								applicantModel.Invited = Constants.Skipped;
							}
							applicantModel.Id = personalDetailsModel.Id;
							applicantModel.ApplicationNumber = applicationNumber;
							applicantModel.FullName = personalDetailsModel.FirstName + " " + personalDetailsModel.LastName;
							applicantModel.FirstIdentificationNumber = (identificationDetails != null && identificationDetails.Count > 0) ? identificationDetails.OrderBy(y => y.IdentificationDetailsID).FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty;
							applicantModel.NodeGUID = personalDetailsModel.NodeGUID;
							applicantModel.Status = personalDetailsModel.Status;
							applicantModel.CreatedDateTime = personalDetailsModel.CreatedDateTime;
                            applicantModel.HIDInviteFlag = personalDetailsModel.HIDInviteFlag == 1 ? "YES" : "NO";
                            retVal.Add(applicantModel);
						}
					}
				}
			}

			return retVal;
		}

		public static List<ApplicantModel> GetLegalApplicantModels(string applicationNumber)
		{
			List<ApplicantModel> retVal = null;

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				List<CompanyDetailsModel> companyDetailsModels = CompanyDetailsProcess.GetApplicantCompanyDetails(applicationNumber);
				if(companyDetailsModels != null && companyDetailsModels.Count > 0)
				{
					retVal = new List<ApplicantModel>();
					foreach(CompanyDetailsModel companyDetailsModel in companyDetailsModels)
					{
						if(companyDetailsModel != null)
						{
							ApplicantModel applicantModel = new ApplicantModel();
							applicantModel.CompanyDetails = companyDetailsModel;
							applicantModel.Id = companyDetailsModel.Id;
							applicantModel.FirstIdentificationNumber = companyDetailsModel.RegistrationNumber;
							applicantModel.ApplicationNumber = applicationNumber;
							applicantModel.FullName = companyDetailsModel.RegisteredName;
							applicantModel.NodeGUID = companyDetailsModel.NodeGUID;
							applicantModel.Status = companyDetailsModel.Status;
							applicantModel.Type = ServiceHelper.GetName(ValidationHelper.GetString(companyDetailsModel.EntityType, ""), Constants.COMPANY_ENTITY_TYPE);
							applicantModel.IdVerified = companyDetailsModel.IDVerified == true ? "YES" : Constants.Pending;
							if (companyDetailsModel.Invited == 0)
							{
								//applicantModel.Invited = Constants.Pending;
								applicantModel.Invited = Constants.NA;
							}
							else if (companyDetailsModel.Invited == 1)
							{
								applicantModel.Invited = Constants.Invited;
							}
							else if (companyDetailsModel.Invited == 2)
							{
								applicantModel.Invited = Constants.Skipped;
							}
							applicantModel.CreatedDateTime = companyDetailsModel.CreatedDateTime;
							//applicantModel._lst_TaxDetails = TaxDetailsProcess.GetTaxDetailsLegalByApplicantId(companyDetailsModel.Id);
							//applicantModel.FATCACRSDetails = FATCACRSDetailsProcess.GetFATCACRSDetailsModelByApplicantId(companyDetailsModel.Id);
							//applicantModel.CRSDetails = FATCACRSDetailsProcess.GetCRSDetailsModelByApplicantId(companyDetailsModel.Id);
							//applicantModel._lst_AddressDetails = AddressDetailsProcess.GetApplicantAddressDetailsLegal(companyDetailsModel.Id);
							//applicantModel.ContactDetailsLegal = ContactDetailsLegalProcess.GetContactDetailsByApplicantId(companyDetailsModel.Id);
							//applicantModel._lst_OriginOfTotalAssetsModel = OriginOfTotalAssetsProcess.GetOriginOfTotalAssetsLegal(companyDetailsModel.Id);
							//applicantModel.CompanyBusinessProfile = CompanyBusinessProfileProcess.GetCompanyBusinessProfileModelByApplicantId(companyDetailsModel.Id);
							//applicantModel.CompanyFinancialInformation = CompanyFinancialInformationProcess.GetCompanyFinancialInformationModelByApplicantId(companyDetailsModel.Id);
							retVal.Add(applicantModel);
						}
					}
				}
			}

			return retVal;
		}

		public static List<ApplicantModel> GetApplicantModelsExtended(string applicationNumber)
		{
			List<ApplicantModel> retVal = null;

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				List<PersonalDetailsModel> PersonalDetailsModels = PersonalDetailsProcess.GetApplicantPersonalDetails(applicationNumber);
				if(PersonalDetailsModels != null && PersonalDetailsModels.Count > 0)
				{
					retVal = new List<ApplicantModel>();
					foreach(PersonalDetailsModel personalDetailsModel in PersonalDetailsModels)
					{
						if(personalDetailsModel != null)
						{
							List<IdentificationDetailsViewModel> identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							ApplicantModel applicantModel = new ApplicantModel();
							applicantModel.PersonalDetails = personalDetailsModel;
							applicantModel.Id = personalDetailsModel.Id;
							applicantModel.ApplicationNumber = applicationNumber;
							applicantModel.FullName = personalDetailsModel.FirstName + " " + personalDetailsModel.LastName;
							applicantModel.FirstIdentificationNumber = (identificationDetails != null && identificationDetails.Count > 0) ? identificationDetails.OrderBy(y => y.IdentificationDetailsID).FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty;
							applicantModel._lst_AddressDetails = AddressDetailsProcess.GetApplicantAddressDetails(personalDetailsModel.Id);
							applicantModel._lst_IdentificationDetails= IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							////Contact Details
							applicantModel.ContactDetails = ContactDetailsProcess.GetContactDetailsByApplicantId(personalDetailsModel.Id);
							////TaxDetails
							applicantModel._lst_TaxDetails = TaxDetailsProcess.GetTaxDetailsByApplicantId(personalDetailsModel.Id);
							////PepApplicant
							applicantModel._lst_PepApplicantViewModel = PEPDetailsProcess.GetPepApplicantsByApplicantId(personalDetailsModel.Id);
							////PepAssociates
						    applicantModel._lst_PepAssociatesViewModel = PEPDetailsProcess.GetPepAssociatesByApplicantId(personalDetailsModel.Id);
							////Source of Income
							applicantModel._lst_SourceOfIncomeModel= SourceOfIncomeProcess.GetSourceOfIncome(personalDetailsModel.Id);
							////Origin of Total Assets
							applicantModel._lst_OriginOfTotalAssetsModel= OriginOfTotalAssetsProcess.GetOriginOfTotalAssets(personalDetailsModel.Id);
							////Business and Financial Profile
							applicantModel.EmploymentDetails= EmploymentDetailsProcess.GetEmploymentDetailsModelByApplicantId(personalDetailsModel.Id);

							retVal.Add(applicantModel);

						}
					}
				}
			}

			return retVal;
		}

		public static List<ApplicantModel> GetApplicantModelsExtendedLegal(string applicationNumber)
		{
			List<ApplicantModel> retVal = null;

			if (!string.IsNullOrEmpty(applicationNumber))
			{
				List<CompanyDetailsModel> companyDetailsModels = CompanyDetailsProcess.GetApplicantCompanyDetails(applicationNumber);
				if (companyDetailsModels != null && companyDetailsModels.Count > 0)
				{
					retVal = new List<ApplicantModel>();
					foreach (CompanyDetailsModel companyDetailsModel in companyDetailsModels)
					{
						if (companyDetailsModel != null)
						{
							ApplicantModel applicantModel = new ApplicantModel();
							applicantModel.CompanyDetails = companyDetailsModel;
							applicantModel.Id = companyDetailsModel.Id;
							applicantModel.ApplicationNumber = applicationNumber;
							applicantModel.FullName = companyDetailsModel.RegisteredName;
							applicantModel.NodeGUID = companyDetailsModel.NodeGUID;

							applicantModel._lst_TaxDetails = TaxDetailsProcess.GetTaxDetailsLegalByApplicantId(companyDetailsModel.Id);
							applicantModel.FATCACRSDetails = FATCACRSDetailsProcess.GetFATCACRSDetailsModelByApplicantId(companyDetailsModel.Id);
							applicantModel.CRSDetails = FATCACRSDetailsProcess.GetCRSDetailsModelByApplicantId(companyDetailsModel.Id);
							applicantModel._lst_AddressDetails = AddressDetailsProcess.GetApplicantAddressDetailsLegal(companyDetailsModel.Id);
							applicantModel.ContactDetailsLegal = ContactDetailsLegalProcess.GetContactDetailsByApplicantId(companyDetailsModel.Id);
							applicantModel._lst_OriginOfTotalAssetsModel = OriginOfTotalAssetsProcess.GetOriginOfTotalAssetsLegal(companyDetailsModel.Id);
							applicantModel.CompanyBusinessProfile = CompanyBusinessProfileProcess.GetCompanyBusinessProfileModelByApplicantId(companyDetailsModel.Id);
							applicantModel.CompanyFinancialInformation = CompanyFinancialInformationProcess.GetCompanyFinancialInformationModelByApplicantId(companyDetailsModel.Id);
							retVal.Add(applicantModel);
						}
					}
				}
			}

			return retVal;
		}
	}
}
