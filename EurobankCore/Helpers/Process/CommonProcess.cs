using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.RelatedParty.PartyRolesLegal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class CommonProcess
    {

        internal static List<SelectListItem> GetCardHolderNameIndividual(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            List<SelectListItem> applicantResult = new List<SelectListItem>();
            List<SelectListItem> relatedPartResult = new List<SelectListItem>();
            var applicant = ApplicantProcess.GetApplicantModels(applicationNumber);
            //var relatedParty = RelatedPartyProcess.GetRelatedPartyModels(applicationNumber);

            if (applicant != null)
            {
                if (applicant.Count > 0)
                {
                    applicantResult = applicant.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.FullName.ToUpper() }).ToList();
                }
            }
            //if (relatedParty != null)
            //{
            //    if (relatedParty.Count > 0)
            //    {
            //        relatedPartResult = relatedParty.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.FullName }).ToList();
            //    }
            //}
            retValues.AddRange(applicantResult);
            // retValues.AddRange(relatedPartResult);
            return retValues;
        }
        internal static List<SelectListItem> GetCardHolderNameLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // List<SelectListItem> applicantResult = new List<SelectListItem>();
            List<SelectListItem> relatedPartResult = new List<SelectListItem>();
            //var applicant = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
            var relatedParty = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber);

            //if (applicant != null)
            //{
            //    if (applicant.Count > 0)
            //    {
            //        applicantResult = applicant.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.FullName }).ToList();
            //    }
            //}
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    foreach (var item in relatedParty)
                    {
                        var getPartyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(item.Id);
                        if (getPartyRoles != null)
                        {
                            if (getPartyRoles.RelatedPartyRoles_IsAuthorisedCardholder == true)
                            {
                                relatedPartResult.Add(new SelectListItem { Text = item.FullName.ToUpper(), Value = item.NodeGUID.ToString() });
                            }
                        }
                    }

                }
            }
            //retValues.AddRange(applicantResult);
            retValues.AddRange(relatedPartResult);
            return retValues;
        }
        internal static List<SelectListItem> GetSignatoryPersonLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var relatedParty = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber);
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    foreach (var item in relatedParty)
                    {
                        PartyRolesLegalViewModel partyRoles = null;
                        if (string.Equals(item.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                        {
                            partyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalByApplicantId(item.Id);
                        }
                        else
                        {
                            partyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(item.Id);
                        }
                        if (partyRoles != null)
                        {
                            if (partyRoles.RelatedPartyRoles_IsAuthorisedSignatory == true || partyRoles.RelatedPartyRoles_IsAuthorisedPerson == true)
                            {
                                retValues.Add(new SelectListItem { Text = item.FullName.ToUpper(), Value = item.NodeGUID.ToString() });
                            }
                        }

                    }
                }
            }

            return retValues;
        }

        internal static List<SelectListItem> GetSignatoryPersonAuthorizedPersonLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var relatedParty = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber);
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    foreach (var item in relatedParty)
                    {
                        PartyRolesLegalViewModel partyRoles = null;
                        if (!string.Equals(item.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                        {
                            partyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(item.Id);
                        }
                        if (partyRoles != null)
                        {
                            if (partyRoles.RelatedPartyRoles_IsAuthorisedPerson)
                            {
                                retValues.Add(new SelectListItem { Text = item.FullName.ToUpper(), Value = item.NodeGUID.ToString() });
                            }
                        }

                    }
                }
            }

            return retValues;
        }

        internal static List<SelectListItem> GetSignatoryPersonAuthorisedSignatoryLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var relatedParty = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber);
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    foreach (var item in relatedParty)
                    {
                        PartyRolesLegalViewModel partyRoles = null;
                        if (string.Equals(item.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                        {
                            partyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalByApplicantId(item.Id);
                        }
                        else
                        {
                            partyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(item.Id);
                        }

                        if (partyRoles != null)
                        {
                            if (partyRoles.RelatedPartyRoles_IsAuthorisedSignatory)
                            {
                                retValues.Add(new SelectListItem { Text = item.FullName.ToUpper(), Value = item.NodeGUID.ToString() });
                            }
                        }

                    }
                }
            }

            return retValues;
        }

        internal static List<SelectListItem> GetSignatoryPersonIndividual(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var applicant = ApplicantProcess.GetApplicantModels(applicationNumber);
            if (applicant != null)
            {
                if (applicant.Count > 0)
                {
                    foreach (var item in applicant)
                    {
                        retValues.Add(new SelectListItem { Text = item.FullName.ToUpper(), Value = item.NodeGUID.ToString() });
                    }
                }
            }
            return retValues;
        }
        internal static string GetApplicationValue(string applicationTypeName)
        {
            var applicationType = ServiceHelper.GetApplicationType();
            string applicationTypeValue = (applicationType != null && applicationType.Count > 0 && applicationTypeName != null && applicationType.Any(f => f.Text == applicationTypeName.ToString())) ? applicationType.FirstOrDefault(f => f.Text == applicationTypeName.ToString()).Value : string.Empty;
            return applicationTypeValue;
        }
        internal static string GetSelectedPartyRolesLegalForIndividual(int companyDetailsId)
        {
            string retVal = string.Empty;
            var getPartyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(companyDetailsId);
            if (getPartyRoles != null)
            {
                if (getPartyRoles.RelatedPartyRoles_IsDirector == true)
                {
                    retVal = "Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAlternativeDirector == true)
                {
                    retVal = retVal + "Alternate Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretary == true)
                {
                    retVal = retVal + "Secretary,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsShareholder == true)
                {
                    retVal = retVal + "Shareholder,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsUltimateBeneficiaryOwner == true)
                {
                    retVal = retVal + "Ultimate Beneficial Owner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedSignatory == true)
                {
                    retVal = retVal + "Authorised Signatory,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedPerson == true)
                {
                    retVal = retVal + "Authorised Person,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsDesignatedEBankingUser == true)
                {
                    retVal = retVal + "Digital Banking User,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedCardholder == true)
                {
                    retVal = retVal + "Authorised Cardholder,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedContactPerson == true)
                {
                    retVal = retVal + "Authorised Contact Person,</br>";
                }
                //new
                if (getPartyRoles.RelatedPartyRoles_IsAlternateSecretery == true)
                {
                    retVal = retVal + "Alternate Secretary,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector == true)
                {
                    retVal = retVal + "Chairman Of The Board Of Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector == true)
                {
                    retVal = retVal + "Vice-Chairman Of The Board Of Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector == true)
                {
                    retVal = retVal + "Secretary Of The Board Of Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors == true)
                {
                    retVal = retVal + "Treasurer Of Board Of Directors,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemeberOfBoardOfDirectors == true)
                {
                    retVal = retVal + "Member Of Board Of Directors,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsPartner == true)
                {
                    retVal = retVal + "Partner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_GeneralPartner == true)
                {
                    retVal = retVal + "General Partner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_LimitedPartner == true)
                {
                    retVal = retVal + "Limited Partner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsPresidentOfCommittee == true)
                {
                    retVal = retVal + "President Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsVicePresidentOfCommittee == true)
                {
                    retVal = retVal + "Vice-President Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfCommittee == true)
                {
                    retVal = retVal + "Secretary Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfCommittee == true)
                {
                    retVal = retVal + "Treasurer Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemeberOfCommittee == true)
                {
                    retVal = retVal + "Member Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTrustee == true)
                {
                    retVal = retVal + "Trustee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSettlor == true)
                {
                    retVal = retVal + "Settlor,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsProtector == true)
                {
                    retVal = retVal + "Protector,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsBenificiary == true)
                {
                    retVal = retVal + "Benificiary,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsFounder == true)
                {
                    retVal = retVal + "Founder,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsPresidentOfCouncil == true)
                {
                    retVal = retVal + "President Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsVicePresidentOfCouncil == true)
                {
                    retVal = retVal + "Vice-President Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfCouncil == true)
                {
                    retVal = retVal + "Secretary Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfCouncil == true)
                {
                    retVal = retVal + "Treasurer Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemberOfCouncil == true)
                {
                    retVal = retVal + "Member Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsFundMlco == true)
                {
                    retVal = retVal + "Fund MLCO,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsFundAdministrator == true)
                {
                    retVal = retVal + "Fund Administrator,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsManagementCompany == true)
                {
                    retVal = retVal + "Management Company,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsHolderOfManagementShares == true)
                {
                    retVal = retVal + "Holder Of Management Shares,</br>";
                }
                while (retVal.EndsWith("</br>"))
                    retVal = retVal.Substring(0, retVal.Length - 6);

            }

            return retVal;
        }
        internal static string GetSelectedPartyRolesLegal(int companyDetailsId)
        {
            string retVal = string.Empty;
            var getPartyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalByApplicantId(companyDetailsId);
            if (getPartyRoles != null)
            {
                if (getPartyRoles.RelatedPartyRoles_IsDirector == true)
                {
                    retVal = "Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAlternativeDirector == true)
                {
                    retVal = retVal + "Alternate Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretary == true)
                {
                    retVal = retVal + "Secretary,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsShareholder == true)
                {
                    retVal = retVal + "Shareholder,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsUltimateBeneficiaryOwner == true)
                {
                    retVal = retVal + "Ultimate Beneficial Owner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedSignatory == true)
                {
                    retVal = retVal + "Authorised Signatory,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedPerson == true)
                {
                    retVal = retVal + "Authorised Person,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsDesignatedEBankingUser == true)
                {
                    retVal = retVal + "Designated E-Banking User,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedCardholder == true)
                {
                    retVal = retVal + "Authorised Cardholder,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedContactPerson == true)
                {
                    retVal = retVal + "Authorised Contact Person,</br>";
                }
                //new
                if (getPartyRoles.RelatedPartyRoles_IsAlternateSecretery == true)
                {
                    retVal = retVal + "Alternate Secretary,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector == true)
                {
                    retVal = retVal + "Chairman Of The Board Of Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector == true)
                {
                    retVal = retVal + "Vice-Chairman Of The Board Of Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector == true)
                {
                    retVal = retVal + "Secretary Of The Board Of Director,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors == true)
                {
                    retVal = retVal + "Treasurer Of Board Of Directors,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemeberOfBoardOfDirectors == true)
                {
                    retVal = retVal + "Member Of Board Of Directors,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsPartner == true)
                {
                    retVal = retVal + "Partner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_GeneralPartner == true)
                {
                    retVal = retVal + "General Partner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_LimitedPartner == true)
                {
                    retVal = retVal + "Limited Partner,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsPresidentOfCommittee == true)
                {
                    retVal = retVal + "President Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsVicePresidentOfCommittee == true)
                {
                    retVal = retVal + "Vice-President Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfCommittee == true)
                {
                    retVal = retVal + "Secretary Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfCommittee == true)
                {
                    retVal = retVal + "Treasurer Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemeberOfCommittee == true)
                {
                    retVal = retVal + "Member Of Committee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTrustee == true)
                {
                    retVal = retVal + "Trustee,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSettlor == true)
                {
                    retVal = retVal + "Settlor,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsProtector == true)
                {
                    retVal = retVal + "Protector,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsBenificiary == true)
                {
                    retVal = retVal + "Benificiary,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsFounder == true)
                {
                    retVal = retVal + "Founder,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsPresidentOfCouncil == true)
                {
                    retVal = retVal + "President Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsVicePresidentOfCouncil == true)
                {
                    retVal = retVal + "Vice-President Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfCouncil == true)
                {
                    retVal = retVal + "Secretary Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfCouncil == true)
                {
                    retVal = retVal + "Treasurer Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemberOfCouncil == true)
                {
                    retVal = retVal + "Member Of Council,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsFundMlco == true)
                {
                    retVal = retVal + "Fund MLCO,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsFundAdministrator == true)
                {
                    retVal = retVal + "Fund Administrator,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsManagementCompany == true)
                {
                    retVal = retVal + "Management Company,</br>";
                }
                if (getPartyRoles.RelatedPartyRoles_IsHolderOfManagementShares == true)
                {
                    retVal = retVal + "Holder Of Management Shares,</br>";
                }

                while (retVal.EndsWith("</br>"))
                    retVal = retVal.Substring(0, retVal.Length - 6);

            }

            return retVal;
        }
        internal static string GetSelectedPartyRolesIndiVidual(int personDetailsId)
        {
            string retVal = string.Empty;
            var partyRoles = RelatedPartyRolesProcess.GetPartyRolesDetailsByApplicantId(personDetailsId);
            if (partyRoles != null)
            {
                if (partyRoles.RelatedPartyRoles_IsContactPerson == true)
                {
                    retVal = retVal + "Contact Person,</br>";
                }
                if (partyRoles.RelatedPartyRoles_IsEBankingUser == true)
                {
                    retVal = retVal + "E-Banking User,</br>";
                }
                if (partyRoles.RelatedPartyRoles_HasPowerOfAttorney == true)
                {
                    retVal = retVal + "Has Power Of Attorney,</br>";
                }
                while (retVal.EndsWith("</br>"))
                    retVal = retVal.Substring(0, retVal.Length - 6);
            }
            return retVal;
        }

        internal static List<SelectListItem> GetDDLCollectedByLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var relatedParty = RelatedPartyProcess.GetRelatedPartyModels(applicationNumber);
            if (relatedParty != null)
            {
                foreach (var item in relatedParty)
                {
                    retValues.Add(new SelectListItem { Text = item.FullName.ToUpper(), Value = item.NodeGUID.ToString() });
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetDDLGroupStructureParent(string applicationNumber, int applicationId)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            List<SelectListItem> lstApplicants = new List<SelectListItem>();
            List<SelectListItem> lstGroupStructure = new List<SelectListItem>();
            var applicant = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
            var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicationId);
            if (applicationDetails != null && applicationDetails.Id > 0)
            {
                var groupStructure = CompanyGroupStructureProcess.GetCompanyGroupStructureParent(applicationId);
                if (groupStructure != null)
                {
                    //foreach (var item in groupStructure)
                    //{
                    //    lstGroupStructure.Add(new SelectListItem { Text = item.Text, Value = item.Value.ToString() });
                    //}
                    retValues.AddRange(groupStructure);
                }
            }
            if (applicant != null)
            {
                foreach (var item in applicant)
                {
                    lstApplicants.Add(new SelectListItem { Text = item.FullName.ToUpper(), Value = item.NodeGUID.ToString() });
                }
            }

            retValues.AddRange(lstApplicants);
            return retValues;
        }
        internal static List<SelectListItem> GetDocumentsEntityName(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            List<SelectListItem> applicantResult = new List<SelectListItem>();
            List<SelectListItem> relatedPartResult = new List<SelectListItem>();
            var applicant = ApplicantProcess.GetApplicantModels(applicationNumber);
            var relatedParty = RelatedPartyProcess.GetRelatedPartyModels(applicationNumber);
            if (applicant != null)
            {
                if (applicant.Count > 0)
                {
                    applicantResult = applicant.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.FullName.ToUpper() }).ToList();
                }
            }
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    relatedPartResult = relatedParty.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.FullName.ToUpper() }).ToList();
                }
            }
            retValues.AddRange(applicantResult);
            retValues.AddRange(relatedPartResult);
            return retValues;
        }
        internal static List<SelectListItem> GetDocumentsEntityNameLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            List<SelectListItem> applicantResult = new List<SelectListItem>();
            List<SelectListItem> relatedPartResult = new List<SelectListItem>();
            var applicant = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
            var relatedParty = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationNumber);
            if (applicant != null)
            {
                if (applicant.Count > 0)
                {
                    applicantResult = applicant.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.FullName.ToUpper() }).ToList();
                }
            }
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    relatedPartResult = relatedParty.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.FullName.ToUpper() }).ToList();
                }
            }
            retValues.AddRange(applicantResult);
            retValues.AddRange(relatedPartResult);
            return retValues;
        }
        internal static string GetEntityRoleType(string guid)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            string retval = string.Empty;
            if (guid != "")
            {
                retVal = tree.SelectNodes()

                    .WhereEquals("NodeGUID", new Guid(guid))
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (retVal != null && retVal.Parent != null)
                {
                    string roleType = retVal.Parent.DocumentName;
                    if (string.Equals(roleType, "Applicants", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "APPLICANT";
                    }
                    else if (string.Equals(roleType, "Related Parties", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "RELATED PARTY";
                    }
                    var role = ServiceHelper.GetPERSON_ROLE();
                    retval = (role != null && role.Count > 0 && roleType != null) ? role.FirstOrDefault(f => f.Text == roleType.ToString()).Value : string.Empty;

                }

            }
            return retval;
        }
        internal static string GetEntityRoleTypeLegal(string guid)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            string retval = string.Empty;
            if (guid != "")
            {
                retVal = tree.SelectNodes()

                    .WhereEquals("NodeGUID", new Guid(guid))
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (retVal != null && retVal.Parent != null)
                {
                    string roleType = retVal.Parent.DocumentName;
                    if (string.Equals(roleType, "Applicants", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "APPLICANT";
                    }
                    else if (string.Equals(roleType, "Related Parties", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "RELATED PARTY";
                    }
                    var role = ServiceHelper.GetLegalPERSON_ROLE();
                    retval = (role != null && role.Count > 0 && roleType != null) ? role.FirstOrDefault(f => f.Text == roleType.ToString()).Value : string.Empty;

                }

            }
            return retval;
        }
        internal static string GetCorporateDocumentPersonTypeValue(string personType)
        {
            string retVal = string.Empty;
            var personTypeList = ServiceHelper.GetCorporatePersonType();
            if (personType != null)
            {
                retVal = (personTypeList != null && personTypeList.Count > 0 && personTypeList != null) ? personTypeList.FirstOrDefault(f => f.Text == personType.ToString()).Value : string.Empty;
            }

            return retVal;
        }
        internal static string GetIndividualJointDocumentPersonTypeValue(string personType)
        {
            string retVal = string.Empty;
            var personTypeList = ServiceHelper.GetIndividualJointPersonType();
            if (personType != null)
            {
                retVal = (personTypeList != null && personTypeList.Count > 0 && personTypeList != null) ? personTypeList.FirstOrDefault(f => f.Text == personType.ToString()).Value : string.Empty;
            }

            return retVal;
        }
        internal static string GetCorporateDocumentTypeValue(string documentType)
        {
            string retVal = string.Empty;
            var type = ServiceHelper.GetCorporateDocumentTypeValue();
            if (type != null)
            {
                retVal = (type != null && type.Count > 0 && type != null) ? type.FirstOrDefault(f => string.Equals(f.Text, documentType.ToString(), StringComparison.OrdinalIgnoreCase)).Value : string.Empty;
            }
            return retVal;
        }
        internal static List<string> GetEntitySeletedRolesIndividual(string personNodeID)
        {

            List<string> mylist = new List<string>(new string[] { });
            PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(new Guid(personNodeID), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
            var roles = RelatedPartyRolesProcess.GetPartyRolesDetailsByApplicantId(personalDetails.PersonalDetailsID);
            if (roles != null)
            {
                if (roles.RelatedPartyRoles_HasPowerOfAttorney == true)
                {
                    mylist.Add("POWER OF ATTORNEY");
                }
                if (roles.RelatedPartyRoles_IsContactPerson == true)
                {
                    mylist.Add("AUTHORISED CONTACT PERSON");
                }
                if (roles.RelatedPartyRoles_IsEBankingUser == true)
                {
                    mylist.Add("DESIGNATED E-BANKING USER");
                }
            }
            return mylist;
        }
        internal static List<string> GetEntitySeletedRolesLegal(string personNodeID, bool IsRelatedPartyUBO)
        {

            List<string> mylist = new List<string>(new string[] { });
            PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(new Guid(personNodeID), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
            var getPartyRoles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(personalDetails.PersonalDetailsID);
            if (getPartyRoles != null)
            {
                if (getPartyRoles.RelatedPartyRoles_IsDirector == true)
                {
                    mylist.Add("DIRECTOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsAlternativeDirector == true)
                {
                    mylist.Add("ALTERNATE DIRECTOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretary == true)
                {
                    mylist.Add("SECRETARY");
                }
                if (getPartyRoles.RelatedPartyRoles_IsShareholder == true)
                {
                    mylist.Add("SHAREHOLDER");
                }
                if (getPartyRoles.RelatedPartyRoles_IsUltimateBeneficiaryOwner == true || IsRelatedPartyUBO)
                {
                    mylist.Add("ULTIMATE BENEFICIAL OWNER");
                }

                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedSignatory == true)
                {
                    mylist.Add("AUTHORISED SIGNATORY");
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedPerson == true)
                {
                    mylist.Add("AUTHORISED PERSON");
                }
                if (getPartyRoles.RelatedPartyRoles_IsDesignatedEBankingUser == true)
                {
                    mylist.Add("DESIGNATED E-BANKING USER");
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedCardholder == true)
                {
                    mylist.Add("AUTHORISED CARDHOLDER");
                }
                if (getPartyRoles.RelatedPartyRoles_IsAuthorisedContactPerson == true)
                {
                    mylist.Add("AUTHORISED CONTACT PERSON");
                }
                if (getPartyRoles.RelatedPartyRoles_IsAlternateSecretery == true)
                {
                    mylist.Add("ALTERNATE SECRETARY");
                }
                if (getPartyRoles.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector == true)
                {
                    mylist.Add("CHAIRMAN OF THE BOARD OF DIRECTOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector == true)
                {
                    mylist.Add("VICE-CHAIRMAN OF THE BOARD OF DIRECTOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector == true)
                {
                    mylist.Add("SECRETARY OF THE BOARD OF DIRECTOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors == true)
                {
                    mylist.Add("TREASURER OF BOARD OF DIRECTORS");
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemeberOfBoardOfDirectors == true)
                {
                    mylist.Add("MEMBER OF BOARD OF DIRECTORS");
                }
                if (getPartyRoles.RelatedPartyRoles_IsPartner == true)
                {
                    mylist.Add("PARTNER");
                }
                if (getPartyRoles.RelatedPartyRoles_GeneralPartner == true)
                {
                    mylist.Add("GENERAL PARTNER");
                }
                if (getPartyRoles.RelatedPartyRoles_LimitedPartner == true)
                {
                    mylist.Add("LIMITED PARTNER");
                }
                if (getPartyRoles.RelatedPartyRoles_IsPresidentOfCommittee == true)
                {
                    mylist.Add("PRESIDENT OF COMMITTEE");
                }
                if (getPartyRoles.RelatedPartyRoles_IsVicePresidentOfCommittee == true)
                {
                    mylist.Add("VICE-PRESIDENT OF COMMITTEE");
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfCommittee == true)
                {
                    mylist.Add("SECRETARY OF COMMITTEE");
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfCommittee == true)
                {
                    mylist.Add("TREASURER OF COMMITTEE");
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemeberOfCommittee == true)
                {
                    mylist.Add("MEMBER OF COMMITTEE");
                }
                if (getPartyRoles.RelatedPartyRoles_IsTrustee == true)
                {
                    mylist.Add("TRUSTEE");
                }
                if (getPartyRoles.RelatedPartyRoles_IsSettlor == true)
                {
                    mylist.Add("SETTLOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsProtector == true)
                {
                    mylist.Add("PROTECTOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsBenificiary == true)
                {
                    mylist.Add("BENIFICIARY");
                }
                if (getPartyRoles.RelatedPartyRoles_IsFounder == true)
                {
                    mylist.Add("FOUNDER");
                }
                if (getPartyRoles.RelatedPartyRoles_IsPresidentOfCouncil == true)
                {
                    mylist.Add("PRESIDENT OF COUNCIL");
                }
                if (getPartyRoles.RelatedPartyRoles_IsVicePresidentOfCouncil == true)
                {
                    mylist.Add("VICE-PRESIDENT OF COUNCIL");
                }
                if (getPartyRoles.RelatedPartyRoles_IsSecretaryOfCouncil == true)
                {
                    mylist.Add("SECRETARY OF COUNCIL");
                }
                if (getPartyRoles.RelatedPartyRoles_IsTreasurerOfCouncil == true)
                {
                    mylist.Add("TREASURER OF COUNCIL");
                }
                if (getPartyRoles.RelatedPartyRoles_IsMemberOfCouncil == true)
                {
                    mylist.Add("MEMBER OF COUNCIL");
                }
                if (getPartyRoles.RelatedPartyRoles_IsFundMlco == true)
                {
                    mylist.Add("FUND MLCO");
                }
                if (getPartyRoles.RelatedPartyRoles_IsFundAdministrator == true)
                {
                    mylist.Add("FUND ADMINISTRATOR");
                }
                if (getPartyRoles.RelatedPartyRoles_IsManagementCompany == true)
                {
                    mylist.Add("MANAGEMENT COMPANY");
                }
                if (getPartyRoles.RelatedPartyRoles_IsHolderOfManagementShares == true)
                {
                    mylist.Add("HOLDER OF MANAGEMENT SHARES");
                }
            }
            return mylist;
        }

        internal static string GetCompanyEntitySubTypeName(string CompanyEntity)
        {
            string retval = string.Empty;
            string companyEntityName = ValidationHelper.GetString(ServiceHelper.GetName(CompanyEntity, Constants.COMPANY_ENTITY_TYPE), "");
            if (string.Equals(companyEntityName.TrimEnd(), "Private Limited Company", StringComparison.OrdinalIgnoreCase))
            {
                retval = "PRIVATE LIMITED COMPANY";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Public Limited Company", StringComparison.OrdinalIgnoreCase))
            {
                retval = "PUBLIC LIMITED COMPANY";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "General Partnership", StringComparison.OrdinalIgnoreCase))
            {
                retval = "GENERAL PARTNERSHIP";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Limited Liability Partnership", StringComparison.OrdinalIgnoreCase))
            {
                retval = "LIMITED LIABILITY PARTNERSHIP";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Provident Fund", StringComparison.OrdinalIgnoreCase))
            {
                retval = "PROVIDENT FUND";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Pension Fund", StringComparison.OrdinalIgnoreCase))
            {
                retval = "PENSION FUND";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Foundation", StringComparison.OrdinalIgnoreCase))
            {
                retval = "FOUNDATION";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Trade Union", StringComparison.OrdinalIgnoreCase))
            {
                retval = "TRADE UNION";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Club / Association", StringComparison.OrdinalIgnoreCase))
            {
                retval = "CLUB/ASSOCIATION";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "City Council / Local Authority", StringComparison.OrdinalIgnoreCase))
            {
                retval = "CITY COUNCIL/LOCAL AUTHORITY";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Government Organization", StringComparison.OrdinalIgnoreCase))
            {
                retval = "GOVERNMENT ORGANIZATION";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Semi - Government Organization", StringComparison.OrdinalIgnoreCase))
            {
                retval = "SEMI-GOVERNMENT ORGANIZATION";
            }
            if (string.Equals(companyEntityName.TrimEnd(), "Fund", StringComparison.OrdinalIgnoreCase))
            {
                retval = "FUND";
            }
            var entity = ServiceHelper.GetCorporate_Sub_Type();
            string name = (entity != null && entity.Count > 0 && retval != null) ? entity.FirstOrDefault(f => f.Text == retval.ToString()).Value : string.Empty;
            return retval = name;
        }

        internal static List<SelectListItem> GetEbankingSubscriberLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            List<SelectListItem> relatedPartResult = new List<SelectListItem>();

            var relatedParty = PersonalDetailsProcess.GetRelatedPartyPersonalDetails(applicationNumber);
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    foreach (var item in relatedParty)
                    {
                        var roles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(item.Id);
                        if (roles != null)
                        {
                            if (roles.RelatedPartyRoles_IsDesignatedEBankingUser || roles.RelatedPartyRoles_IsAuthorisedPerson)
                            {
                                relatedPartResult.Add(new SelectListItem { Text = item.FirstName.ToUpper() + " " + item.LastName.ToUpper(), Value = item.NodeGUID.ToString() });
                            }
                        }

                    }

                }
            }

            retValues.AddRange(relatedPartResult);
            return retValues;
        }

        internal static List<SelectListItem> GetEbankingSubscriberLegalRoleConcat(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            List<SelectListItem> relatedPartResult = new List<SelectListItem>();

            var relatedParty = PersonalDetailsProcess.GetRelatedPartyPersonalDetails(applicationNumber);
            if (relatedParty != null)
            {
                if (relatedParty.Count > 0)
                {
                    foreach (var item in relatedParty)
                    {
                        var roles = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(item.Id);
                        if (roles != null)
                        {
                            if (roles.RelatedPartyRoles_IsDesignatedEBankingUser || roles.RelatedPartyRoles_IsAuthorisedPerson)
                            {
                                relatedPartResult.Add(new SelectListItem { Text = item.FirstName.ToUpper() + " " + item.LastName.ToUpper(), Value = item.NodeGUID.ToString() });
                            }
                        }

                    }

                }
            }

            retValues.AddRange(relatedPartResult);
            return retValues;
        }
        /// <summary>
        /// It is use to check the person is Applicant or Related party in EBanking Subscriber Individual
        /// </summary>
        /// <param name="guid">Person GUID</param>
        /// <returns>Person Type: APPLICANT/ RELATED PARTY</returns>
        internal static string GetPersonType(string guid)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            string retval = string.Empty;
            if (guid != "")
            {
                retVal = tree.SelectNodes()

                    .WhereEquals("NodeGUID", new Guid(guid))
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (retVal != null && retVal.Parent != null)
                {
                    string roleType = retVal.Parent.DocumentName;
                    if (string.Equals(roleType, "Applicants", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "APPLICANT";
                    }
                    else if (string.Equals(roleType, "Related Parties", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "RELATED PARTY";
                    }
                    retval = roleType;
                }

            }
            return retval;
        }

        internal static List<SelectListItem> GetEbankingSignatoryGroupLegal(string applicationNumber)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var signatureMandate = SignatureMandateLegalProcess.GetSignatureMandateLegalModels(applicationNumber);

            var mandateType = ServiceHelper.GetMandateType();
            var signatoryGroup = ServiceHelper.GetSignatoryGroup();
            if (signatoryGroup != null && signatureMandate != null)
            {
                if (signatoryGroup.Count > 0)
                {
                    foreach (var mandate in signatureMandate)
                    {
                        string mandateTypeName = (mandateType != null && mandateType.Count > 0 && !string.IsNullOrEmpty(mandate.MandateType)) ? mandateType.FirstOrDefault(f => f.Value == mandate.MandateType.ToString()).Text : string.Empty;
                        if (mandateTypeName.ToUpper() == "FOR EBANKING TRANSACTIONS" || mandateTypeName.ToUpper() == "FOR OPERATIONS AND EBANKING")
                        {
                            if (mandate.AuthorizedSignatoryGroupList != null && mandate.AuthorizedSignatoryGroupList.Any())
                            {
                                foreach (var groupname in mandate.AuthorizedSignatoryGroupList)
                                {
                                    string groupName = (signatoryGroup != null && signatoryGroup.Count > 0 && !string.IsNullOrEmpty(groupname)) ? signatoryGroup.FirstOrDefault(f => f.Value == groupname.ToString()).Text : string.Empty;
                                    retValues.Add(new SelectListItem { Text = groupName, Value = groupname.ToString() });
                                }
                            }
                            if (mandate.AuthorizedSignatoryGroup1List != null && mandate.AuthorizedSignatoryGroup1List.Any())
                            {
                                foreach (var groupname in mandate.AuthorizedSignatoryGroup1List)
                                {
                                    string groupName = (signatoryGroup != null && signatoryGroup.Count > 0 && !string.IsNullOrEmpty(groupname)) ? signatoryGroup.FirstOrDefault(f => f.Value == groupname.ToString()).Text : string.Empty;
                                    retValues.Add(new SelectListItem { Text = groupName, Value = groupname.ToString() });

                                }
                            }
                        }
                    }
                }
            }
            return retValues.GroupBy(x => x.Text).Select(y => y.First()).OrderBy(x => x.Text).ToList();
        }
        /// <summary>
        /// Using application type get the Type of PersonalAndJoinAccount document
        /// </summary>
        /// <param name="applicationType"></param>
        /// <returns></returns>
        internal static string GetApplicationTypeForPersonalAndJoinAccount(string applicationType)
        {
            string retVal = string.Empty;
            var entityTpe = ServiceHelper.GetEntityType();
            if (!string.IsNullOrEmpty(applicationType))
            {
                if (string.Equals(applicationType, "JOINT-INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                {
                    applicationType = "JOINT ACCOUNT";
                }
                retVal = (entityTpe != null && entityTpe.Count > 0) ? entityTpe.FirstOrDefault(f => f.Text == applicationType.ToString()).Value : string.Empty;
            }
            return retVal;
        }
        /// <summary>
        /// Get Applicant ID using personode GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal static int GetCompanyDetailsIdLegalApplicant(string guid)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            int retval = 0;
            if (guid != "")
            {
                retVal = tree.SelectNodes()

                    .WhereEquals("NodeGUID", new Guid(guid))
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (retVal != null && retVal.Parent != null)
                {
                    retval = Convert.ToInt32(retVal.GetValue("CompanyDetailsID"));
                }

            }
            return retval;
        }
        /// <summary>
        /// Get Relatedparty ID using personode GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal static int GetCompanyDetailsIdLegalRelatedParty(string guid)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            int retval = 0;
            if (guid != "")
            {
                retVal = tree.SelectNodes()

                    .WhereEquals("NodeGUID", new Guid(guid))
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (retVal != null && retVal.Parent != null)
                {
                    retval = Convert.ToInt32(retVal.GetValue("CompanyDetailsRelatedPartyID"));
                }

            }
            return retval;
        }
        /// <summary>
        /// Get personal details of Related party legal
        /// Application Type=Legal and Related party=Individual
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal static int GetPersonalDetailsLegalRelatedParty(string guid)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            int retval = 0;
            if (guid != "")
            {
                retVal = tree.SelectNodes()

                    .WhereEquals("NodeGUID", new Guid(guid))
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (retVal != null && retVal.Parent != null)
                {
                    retval = Convert.ToInt32(retVal.GetValue("PersonalDetailsID"));
                }

            }
            return retval;
        }
        /// <summary>
        /// Get Jurisdiction Value using Country of Incorporation value
        /// </summary>
        /// /// <param name="CountryuOfIncorporation"></param>
        /// <returns></returns>
        internal static string GetJurisdictionValue(string CountryuOfIncorporation)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(CountryuOfIncorporation))
            {
                CountryuOfIncorporation = ServiceHelper.GetCountryNameById(Convert.ToInt32(CountryuOfIncorporation));
                if (!string.IsNullOrEmpty(CountryuOfIncorporation))
                {
                    var jurisdiction = ServiceHelper.GetCorporateJurisdiction();
                    //retval = (jurisdiction != null && jurisdiction.Count > 0 && !string.IsNullOrEmpty(CountryuOfIncorporation.ToUpper())) ? jurisdiction.FirstOrDefault(f => f.Text == CountryuOfIncorporation.ToUpper()).Value : string.Empty;
                    var selectValue = jurisdiction.Where(x => x.Text.Trim().Replace(" ", "") == CountryuOfIncorporation.Trim().Replace(" ","").ToUpper()).FirstOrDefault();
                    if (selectValue != null)
                    {
                        retval = selectValue.Value == null ? string.Empty : selectValue.Value.ToString();
                    }
                }
            }
            return retval;
        }
        internal static string GetOtherJurisdictionValue()
        {
            string retval = string.Empty;
            var jurisdiction = ServiceHelper.GetCorporateJurisdiction();
            if (jurisdiction != null)
            {
                var selectValue = jurisdiction.Where(x => x.Text == "OTHER JURISDICTION").FirstOrDefault();
                if (selectValue != null)
                {
                    retval = selectValue.Value == null ? string.Empty : selectValue.Value.ToString();
                }
            }
            return retval;
        }
        internal static string GetEntitySubTypeValue(string EntityType)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(EntityType))
            {
                string companyEntityName = ValidationHelper.GetString(ServiceHelper.GetName(EntityType, Constants.COMPANY_ENTITY_TYPE), "");
                var entity = ServiceHelper.GetCorporate_Sub_Type();
                //retval = (entity != null && entity.Count > 0 && companyEntityName.ToUpper() != null) ? entity.FirstOrDefault(f => f.Text == companyEntityName.ToUpper()).Value : string.Empty;
                var selectValue = entity.Where(x => x.Text.Trim() == companyEntityName.Trim().ToUpper()).FirstOrDefault();
                if (selectValue != null)
                {
                    retval = selectValue.Value == null ? string.Empty : selectValue.Value.ToString();
                }

            }
            return retval;
        }

        internal static string GetEntityRoleTypeByPersonGUID(string guid)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            string retval = string.Empty;
            if (guid != "")
            {
                retVal = tree.SelectNodes()

                    .WhereEquals("NodeGUID", new Guid(guid))
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (retVal != null && retVal.Parent != null)
                {
                    string roleType = retVal.Parent.DocumentName;
                    if (string.Equals(roleType, "Applicants", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "APPLICANT";
                    }
                    else if (string.Equals(roleType, "Related Parties", StringComparison.OrdinalIgnoreCase))
                    {
                        roleType = "RELATED PARTY";
                    }
                    retval = roleType;

                }

            }
            return retval;
        }
    }
}







