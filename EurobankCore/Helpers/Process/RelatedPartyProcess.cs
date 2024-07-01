using CMS.Helpers;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.IdentificationDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class RelatedPartyProcess
	{
		//Need to ask and add applicant fields showing in grid
		public static List<RelatedPartyModel> GetRelatedPartyModels(string applicationNumber)
		{
			List<RelatedPartyModel> retVal = null;

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				List<PersonalDetailsModel> PersonalDetailsModels = PersonalDetailsProcess.GetRelatedPartyPersonalDetails(applicationNumber);
				if(PersonalDetailsModels != null && PersonalDetailsModels.Count > 0)
				{
					retVal = new List<RelatedPartyModel>();
					foreach(PersonalDetailsModel personalDetailsModel in PersonalDetailsModels)
					{
						if(personalDetailsModel != null)
						{
							List<IdentificationDetailsViewModel> identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							RelatedPartyModel relatedPartyModel = new RelatedPartyModel();
							relatedPartyModel.PersonalDetails = personalDetailsModel;
							relatedPartyModel.Id = personalDetailsModel.Id;
							relatedPartyModel.ApplicationNumber = applicationNumber;
							relatedPartyModel.Type = ServiceHelper.GetName(ValidationHelper.GetString(personalDetailsModel.Type, ""), "/Lookups/General/APPLICATION-TYPE"); 
							relatedPartyModel.IdVerified = personalDetailsModel.IdVerified==true?"YES":Constants.Pending;
							if (personalDetailsModel.Invited == 0)
							{
								relatedPartyModel.Invited = Constants.Pending;
							}
							else if (personalDetailsModel.Invited == 1)
							{
								relatedPartyModel.Invited = Constants.Invited;
							}
							else if (personalDetailsModel.Invited == 2)
							{
								relatedPartyModel.Invited = Constants.Skipped;
							}
							relatedPartyModel.FirstIdentificationNumber = (identificationDetails != null && identificationDetails.Count > 0) ? identificationDetails.OrderBy(y => y.IdentificationDetailsID).FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty;
							relatedPartyModel.FullName = personalDetailsModel.FirstName + " " + personalDetailsModel.LastName;
							relatedPartyModel.NodeGUID = personalDetailsModel.NodeGUID;
							relatedPartyModel.Status = personalDetailsModel.Status;
							relatedPartyModel.Role= CommonProcess.GetSelectedPartyRolesIndiVidual(personalDetailsModel.Id);
							relatedPartyModel.HIDInviteFlag = personalDetailsModel.HIDInviteFlag == 1 ? "YES" : "NO";
							retVal.Add(relatedPartyModel);
						}
					}
				}
			}

			return retVal;
		}

		public static List<RelatedPartyModel> GetLegalRelatedPartyModels(string applicationNumber)
		{
			List<RelatedPartyModel> retVal = null;

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				List<CompanyDetailsRelatedPartyModel> companyDetailsModels = CompanyDetailsRelatedPartyProcess.GetRelatedPartyCompanyDetailsRelatedParty(applicationNumber);
				List<PersonalDetailsModel> personalModel = PersonalDetailsProcess.GetRelatedPartyPersonalDetails(applicationNumber);
				retVal = new List<RelatedPartyModel>();
				if (companyDetailsModels != null && companyDetailsModels.Count > 0)
				{
					
					foreach(CompanyDetailsRelatedPartyModel companyDetailsModel in companyDetailsModels)
					{
						if(companyDetailsModel != null)
						{
							RelatedPartyModel relatedPartyModel = new RelatedPartyModel();
							relatedPartyModel.CompanyDetails = companyDetailsModel;
							relatedPartyModel.Id = companyDetailsModel.Id;
							relatedPartyModel.ApplicationNumber = applicationNumber;
							relatedPartyModel.FullName = companyDetailsModel.RegisteredName;
							relatedPartyModel.NodeGUID = companyDetailsModel.NodeGUID;
							relatedPartyModel.Status = companyDetailsModel.Status;
							relatedPartyModel.Type = ServiceHelper.GetName(ValidationHelper.GetString(companyDetailsModel.Type, ""), "/Lookups/General/APPLICATION-TYPE");
							relatedPartyModel.IdVerified = companyDetailsModel.IDVerified == true ? "YES" : Constants.NA;
							if (companyDetailsModel.Invited == 0)
							{
								relatedPartyModel.Invited = Constants.Pending;
							}
							if (companyDetailsModel.Invited == 1)
							{
								relatedPartyModel.Invited = Constants.Invited;
							}
							if (companyDetailsModel.Invited == 2)
							{
								relatedPartyModel.Invited = Constants.Skipped;
							}
							relatedPartyModel.FirstIdentificationNumber = companyDetailsModel.RegistrationNumber;
							relatedPartyModel.Role = CommonProcess.GetSelectedPartyRolesLegal(companyDetailsModel.Id);
							//relatedPartyModel._lst_AddressDetailsModel = AddressDetailsProcess.GetRelatedPartyAddressDetailsLegal(companyDetailsModel.Id);
							//relatedPartyModel.PartyRolesLegal = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalByApplicantId(companyDetailsModel.Id);
							//relatedPartyModel._lst_PepApplicantViewModel = PEPDetailsProcess.GetPepApplicantsRelatedPartyByApplicantId(companyDetailsModel.Id);
							//relatedPartyModel._lst_PepAssociatesViewModel = PEPDetailsProcess.GetPepAssociatesRelatedPartyByApplicantId(companyDetailsModel.Id);
							retVal.Add(relatedPartyModel);
						}
					}
				}
				if (personalModel != null && personalModel.Count > 0)
				{
					foreach (PersonalDetailsModel personalDetailsModel in personalModel)
					{
						if (personalDetailsModel != null)
						{
							List<IdentificationDetailsViewModel> identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							RelatedPartyModel relatedPartyModel = new RelatedPartyModel();
							relatedPartyModel.PersonalDetails = personalDetailsModel;
							relatedPartyModel.Id = personalDetailsModel.Id;
							relatedPartyModel.ApplicationNumber = applicationNumber;
							relatedPartyModel.Type = ServiceHelper.GetName(ValidationHelper.GetString(personalDetailsModel.Type, ""), "/Lookups/General/APPLICATION-TYPE");
							relatedPartyModel.IdVerified = personalDetailsModel.IdVerified == true ? "YES" : Constants.Pending;
							if (personalDetailsModel.Invited == 0)
							{
								relatedPartyModel.Invited = Constants.Pending;
							}
							else if (personalDetailsModel.Invited == 1)
							{
								relatedPartyModel.Invited = Constants.Invited;
							}
							else if (personalDetailsModel.Invited == 2)
							{
								relatedPartyModel.Invited = Constants.Skipped;
							}
							relatedPartyModel.FirstIdentificationNumber = (identificationDetails != null && identificationDetails.Count > 0) ? identificationDetails.OrderBy(y => y.IdentificationDetailsID).FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty;
							relatedPartyModel.FullName = personalDetailsModel.FirstName + " " + personalDetailsModel.LastName;
							relatedPartyModel.NodeGUID = personalDetailsModel.NodeGUID;
							relatedPartyModel.Status = personalDetailsModel.Status;
							relatedPartyModel.Role= CommonProcess.GetSelectedPartyRolesLegalForIndividual(personalDetailsModel.Id);
							relatedPartyModel.HIDInviteFlag = personalDetailsModel.HIDInviteFlag == 1 ? "YES" : "NO";
							retVal.Add(relatedPartyModel);
						}
					}
				}
			}

			return retVal;
		}

		public static List<RelatedPartyModel> GetRelatedPartyModelsExtended(string applicationNumber)
		{
			List<RelatedPartyModel> retVal = null;

			if(!string.IsNullOrEmpty(applicationNumber))
			{
				List<PersonalDetailsModel> PersonalDetailsModels = PersonalDetailsProcess.GetRelatedPartyPersonalDetails(applicationNumber);
				if(PersonalDetailsModels != null && PersonalDetailsModels.Count > 0)
				{
					retVal = new List<RelatedPartyModel>();
					foreach(PersonalDetailsModel personalDetailsModel in PersonalDetailsModels)
					{
						if(personalDetailsModel != null)
						{
							List<IdentificationDetailsViewModel> identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							RelatedPartyModel relatedPartyModel = new RelatedPartyModel();
							relatedPartyModel.PersonalDetails = personalDetailsModel;
							relatedPartyModel.Id = personalDetailsModel.Id;
							relatedPartyModel.ApplicationNumber = applicationNumber;
							relatedPartyModel.FirstIdentificationNumber = (identificationDetails != null && identificationDetails.Count > 0) ? identificationDetails.OrderBy(y => y.IdentificationDetailsID).FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty;
							relatedPartyModel.FullName = personalDetailsModel.FirstName + " " + personalDetailsModel.LastName;
							relatedPartyModel._lst_IdentificationDetailsViewModel = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							relatedPartyModel._lst_AddressDetailsModel = AddressDetailsProcess.GetApplicantAddressDetails(personalDetailsModel.Id);
							relatedPartyModel.ContactDetails = ContactDetailsProcess.GetContactDetailsByApplicantId(personalDetailsModel.Id);
							relatedPartyModel._lst_SourceOfIncomeModel = SourceOfIncomeProcess.GetSourceOfIncomeRelatedParty(personalDetailsModel.Id);
							relatedPartyModel._lst_OriginOfTotalAssetsModel = OriginOfTotalAssetsProcess.GetOriginOfTotalAssets(personalDetailsModel.Id);
							relatedPartyModel._lst_PepApplicantViewModel = PEPDetailsProcess.GetPepApplicantsByApplicantId(personalDetailsModel.Id);
							relatedPartyModel._lst_PepAssociatesViewModel = PEPDetailsProcess.GetPepAssociatesByApplicantId(personalDetailsModel.Id);
							relatedPartyModel.PartyRoles = RelatedPartyRolesProcess.GetPartyRolesDetailsByApplicantId(personalDetailsModel.Id);
							relatedPartyModel.EmploymentDetails = EmploymentDetailsRelatedPartyProcess.GetEmploymentDetailsRelatedPartyModelByRelatedPartyId(personalDetailsModel.Id);
							retVal.Add(relatedPartyModel);
						}
					}
				}
			}

			return retVal;
		}
		public static List<RelatedPartyModel> GetRelatedPartyModelsExtendedLegal(string applicationNumber)
		{
			List<RelatedPartyModel> retVal = null;

			if (!string.IsNullOrEmpty(applicationNumber))
			{
				List<CompanyDetailsRelatedPartyModel> companyDetailsModels = CompanyDetailsRelatedPartyProcess.GetRelatedPartyCompanyDetailsRelatedParty(applicationNumber);
				List<PersonalDetailsModel> personalModel = PersonalDetailsProcess.GetRelatedPartyPersonalDetails(applicationNumber);
				retVal = new List<RelatedPartyModel>();
				if (companyDetailsModels != null && companyDetailsModels.Count > 0)
				{

					foreach (CompanyDetailsRelatedPartyModel companyDetailsModel in companyDetailsModels)
					{
						if (companyDetailsModel != null)
						{
							RelatedPartyModel relatedPartyModel = new RelatedPartyModel();
							relatedPartyModel.CompanyDetails = companyDetailsModel;
							relatedPartyModel.Id = companyDetailsModel.Id;
							relatedPartyModel.ApplicationNumber = applicationNumber;
							relatedPartyModel.FullName = companyDetailsModel.RegisteredName;
							relatedPartyModel.NodeGUID = companyDetailsModel.NodeGUID;
							relatedPartyModel.Type = ServiceHelper.GetName(ValidationHelper.GetString(companyDetailsModel.Type, ""), "/Lookups/General/APPLICATION-TYPE");
							relatedPartyModel.IdVerified = companyDetailsModel.IDVerified == true ? "YES" : "NO";
							if (companyDetailsModel.Invited == 0)
							{
								relatedPartyModel.Invited = Constants.Pending;
							}
							if (companyDetailsModel.Invited == 1)
							{
								relatedPartyModel.Invited = Constants.Invited;
							}
							if (companyDetailsModel.Invited == 2)
							{
								relatedPartyModel.Invited = Constants.Skipped;
							}
							relatedPartyModel._lst_AddressDetailsModel = AddressDetailsProcess.GetRelatedPartyAddressDetailsLegal(companyDetailsModel.Id);
							relatedPartyModel.PartyRolesLegal = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalByApplicantId(companyDetailsModel.Id);
							relatedPartyModel._lst_PepApplicantViewModel = PEPDetailsProcess.GetPepApplicantsRelatedPartyByApplicantId(companyDetailsModel.Id);
							relatedPartyModel._lst_PepAssociatesViewModel = PEPDetailsProcess.GetPepAssociatesRelatedPartyByApplicantId(companyDetailsModel.Id);
							retVal.Add(relatedPartyModel);
						}
					}
				}
				if (personalModel != null && personalModel.Count > 0)
				{
					foreach (PersonalDetailsModel personalDetailsModel in personalModel)
					{
						if (personalDetailsModel != null)
						{
							List<IdentificationDetailsViewModel> identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							RelatedPartyModel relatedPartyModel = new RelatedPartyModel();
							relatedPartyModel.PersonalDetails = personalDetailsModel;
							relatedPartyModel.Id = personalDetailsModel.Id;
							relatedPartyModel.ApplicationNumber = applicationNumber;
							relatedPartyModel.Type = ServiceHelper.GetName(ValidationHelper.GetString(personalDetailsModel.Type, ""), "/Lookups/General/APPLICATION-TYPE");
							relatedPartyModel.IdVerified = personalDetailsModel.IdVerified == true ? "YES" : "NO";
							if (personalDetailsModel.Invited == 0)
							{
								relatedPartyModel.Invited = Constants.Pending;
							}
							else if (personalDetailsModel.Invited == 1)
							{
								relatedPartyModel.Invited = Constants.Invited;
							}
							else if (personalDetailsModel.Invited == 2)
							{
								relatedPartyModel.Invited = Constants.Skipped;
							}
							relatedPartyModel.FirstIdentificationNumber = (identificationDetails != null && identificationDetails.Count > 0) ? identificationDetails.OrderBy(y => y.IdentificationDetailsID).FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty;
							relatedPartyModel.FullName = personalDetailsModel.FirstName + " " + personalDetailsModel.LastName;
							relatedPartyModel.NodeGUID = personalDetailsModel.NodeGUID;
							relatedPartyModel.PartyRolesLegal = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(personalDetailsModel.Id);
							relatedPartyModel._lst_IdentificationDetailsViewModel = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsModel.Id);
							relatedPartyModel._lst_AddressDetailsModel = AddressDetailsProcess.GetApplicantAddressDetails(personalDetailsModel.Id);
							relatedPartyModel.ContactDetails = ContactDetailsProcess.GetContactDetailsByApplicantId(personalDetailsModel.Id);
							relatedPartyModel._lst_SourceOfIncomeModel = SourceOfIncomeProcess.GetSourceOfIncomeRelatedParty(personalDetailsModel.Id);
							relatedPartyModel._lst_OriginOfTotalAssetsModel = OriginOfTotalAssetsProcess.GetOriginOfTotalAssets(personalDetailsModel.Id);
							relatedPartyModel._lst_PepApplicantViewModel = PEPDetailsProcess.GetPepApplicantsByApplicantId(personalDetailsModel.Id);
							relatedPartyModel._lst_PepAssociatesViewModel = PEPDetailsProcess.GetPepAssociatesByApplicantId(personalDetailsModel.Id);
                            relatedPartyModel.EmploymentDetails = EmploymentDetailsRelatedPartyProcess.GetEmploymentDetailsRelatedPartyModelByRelatedPartyId(personalDetailsModel.Id);
							relatedPartyModel.HIDInviteFlag = personalDetailsModel.HIDInviteFlag == 1 ? "YES" : "NO";
                            retVal.Add(relatedPartyModel);
						}
					}
				}
			}

			return retVal;
		}
	}
}
