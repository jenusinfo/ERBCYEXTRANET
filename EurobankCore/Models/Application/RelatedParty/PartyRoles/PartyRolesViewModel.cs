using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.RelatedParty.PartyRoles
{
    public class PartyRolesViewModel
    {
        public int RelatedPartyRolesID { get; set; }
        [Display(Name ="Contact Person")]
        public bool RelatedPartyRoles_IsContactPerson { get; set; }
        [Display(Name = "Digital Banking User")]
        public bool RelatedPartyRoles_IsEBankingUser { get; set; }
        [Display(Name ="Has Power Of Attorney")]
        public bool RelatedPartyRoles_HasPowerOfAttorney { get; set; }


        //[Display(Name ="Attorney")]
        //public bool RelatedPartyRoles_IsAttorney { get; set; }

        //[Display(Name = "Cardholder")]
        //public bool RelatedPartyRoles_IsCardholder { get; set; }
        //[Display(Name = "Signatory")]
        //public bool RelatedPartyRoles_IsSignatory { get; set; }
        //[Display(Name = "Director")]
        //public bool RelatedPartyRoles_IsDirector { get; set; }
        //[Display(Name = "Shareholder")]
        //public bool RelatedPartyRoles_IsShareholder { get; set; }
        //[Display(Name = "Secretary")]
        //public bool RelatedPartyRoles_IsSecretary { get; set; }
        //[Display(Name = "Director")]
        //public bool RelatedPartyRoles_IsPartner { get; set; }
    }
}
