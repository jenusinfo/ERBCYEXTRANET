using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.RelatedParty.PartyRolesLegal
{
    public class PartyRolesLegalViewModel
    {
        public int ? RelatedPartyRolesLegalID { get; set; }
        [Display(Name = "Director")]
        public bool RelatedPartyRoles_IsDirector { get; set; }
        [Display(Name = "Alternate Director")]
        public bool RelatedPartyRoles_IsAlternativeDirector { get; set; }
        [Display(Name = "Secretary")]
        public bool RelatedPartyRoles_IsSecretary { get; set; }
        [Display(Name = "Shareholder")]
        public bool RelatedPartyRoles_IsShareholder { get; set; }
        [Display(Name = "Ultimate Beneficial Owner")]
        public bool RelatedPartyRoles_IsUltimateBeneficiaryOwner { get; set; }
        [Display(Name = "Authorised Signatory")]
        public bool RelatedPartyRoles_IsAuthorisedSignatory { get; set; }
        [Display(Name = "Authorised Person")]
        public bool RelatedPartyRoles_IsAuthorisedPerson { get; set; }
        [Display(Name = "Digital Banking User")]
        public bool RelatedPartyRoles_IsDesignatedEBankingUser { get; set; }
        [Display(Name = "Authorised Cardholder")]
        public bool RelatedPartyRoles_IsAuthorisedCardholder { get; set; }
        [Display(Name = "Authorised Contact Person")]
        public bool RelatedPartyRoles_IsAuthorisedContactPerson { get; set; }
        //New added
        [Display(Name = "Alternate Secretary")]
        public bool RelatedPartyRoles_IsAlternateSecretery { get; set; }
        [Display(Name = "Chairman Of The Board Of Director")]
        public bool RelatedPartyRoles_IsChairmanOfTheBoardOfDirector { get; set; }
        [Display(Name = "Vice-Chairman Of The Board Of Director")]
        public bool RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector { get; set; }
        [Display(Name = "Secretary Of The Board Of Director")]
        public bool RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector { get; set; }
        [Display(Name = "Treasurer Of Board Of Directors")]
        public bool RelatedPartyRoles_IsTreasurerOfBoardOfDirectors { get; set; }
        [Display(Name = "Member Of Board Of Directors")]
        public bool RelatedPartyRoles_IsMemeberOfBoardOfDirectors { get; set; }
        [Display(Name = "Partner")]
        public bool RelatedPartyRoles_IsPartner { get; set; }
        [Display(Name = "General Partner")]
        public bool RelatedPartyRoles_GeneralPartner { get; set; }
        [Display(Name = "Limited Partner")]
        public bool RelatedPartyRoles_LimitedPartner { get; set; }
        [Display(Name = "President Of Committee")]
        public bool RelatedPartyRoles_IsPresidentOfCommittee { get; set; }
        [Display(Name = "Vice-President Of Committee")]
        public bool RelatedPartyRoles_IsVicePresidentOfCommittee { get; set; }
        [Display(Name = "Secretary Of Committee")]
        public bool RelatedPartyRoles_IsSecretaryOfCommittee { get; set; }
        [Display(Name = "Treasurer Of Committee")]
        public bool RelatedPartyRoles_IsTreasurerOfCommittee { get; set; }
        [Display(Name = "Member Of Committee")]
        public bool RelatedPartyRoles_IsMemeberOfCommittee { get; set; }
        [Display(Name = "Trustee")]
        public bool RelatedPartyRoles_IsTrustee { get; set; }
        [Display(Name = "Settlor")]
        public bool RelatedPartyRoles_IsSettlor { get; set; }
        [Display(Name = "Protector")]
        public bool RelatedPartyRoles_IsProtector { get; set; }
        [Display(Name = "Beneficiary")]
        public bool RelatedPartyRoles_IsBenificiary { get; set; }
        [Display(Name = "Founder")]
        public bool RelatedPartyRoles_IsFounder { get; set; }
        [Display(Name = "President Of Council")]
        public bool RelatedPartyRoles_IsPresidentOfCouncil { get; set; }
        [Display(Name = "Vice-President Of Council")]
        public bool RelatedPartyRoles_IsVicePresidentOfCouncil { get; set; }
        [Display(Name = "Secretary Of Council")]
        public bool RelatedPartyRoles_IsSecretaryOfCouncil { get; set; }
        [Display(Name = "Treasurer Of Council")]
        public bool RelatedPartyRoles_IsTreasurerOfCouncil { get; set; }
        [Display(Name = "Member Of Council")]
        public bool RelatedPartyRoles_IsMemberOfCouncil { get; set; }
        [Display(Name = "Fund MLCO")]
        public bool RelatedPartyRoles_IsFundMlco { get; set; }
        [Display(Name = "Fund Administrator")]
        public bool RelatedPartyRoles_IsFundAdministrator { get; set; }
        [Display(Name = "Management Company")]
        public bool RelatedPartyRoles_IsManagementCompany { get; set; }
        [Display(Name = "Holder Of Management Shares")]
        public bool RelatedPartyRoles_IsHolderOfManagementShares { get; set; }
    }
}
