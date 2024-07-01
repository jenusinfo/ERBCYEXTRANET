using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
	public class CompanyDetailsModel
	{
		public string Application_Type { get; set; }
		public int Id { get; set; }
		//[Required(ErrorMessage = "Registered Name is required")]
		public string RegisteredName { get; set; }

		public string TradingName { get; set; }
		//[Required(ErrorMessage = "Entity type is required")]
		public string EntityType { get; set; }

		public string CountryofIncorporation { get; set; }

		public string RegistrationNumber { get; set; }
		//[Required(ErrorMessage = "Date of incorporation is required")]

		public Nullable<DateTime> DateofIncorporation { get; set; }

		public string HdnDateofIncorporation { get; set; }

		public string ListingStatus { get; set; }

		public string Status { get; set; }

		public bool CorporationSharesIssuedToTheBearer { get; set; }

		public bool IstheEntitylocatedandoperatesanofficeinCyprus { get; set; }

		public string SharesIssuedToTheBearerName { get; set; }

		public string IsOfficeinCyprusName { get; set; }

		public string NodeGUID { get; set; }
		[Display(Name = "Is the applicant liable to pay defence tax in Cyprus? ")]
		public bool IsLiableToPayDefenseTaxInCyprus { get; set; }

		[Display(Name = "Is the applicant liable to pay defence tax in Cyprus? ")]
		public string IsLiableToPayDefenseTaxInCyprusName { get; set; }

		public string NodePath { get; set; }
		[Display(Name = "Does the Applicant maintain Bank Account(s) with another Banking Institution?")]
		public bool HasAccountInOtherBank { get; set; }
		[Display(Name = "Does the Applicant maintain Bank Account(s) with another Banking Institution?")]
		public string HasAccountInOtherBankName { get; set; }
		[Display(Name = "Name of Banking Institution")]
		public string NameOfBankingInstitution { get; set; }
		[Display(Name = "Country of Banking Institution")]
		public string CountryOfBankingInstitution { get; set; }
		public int RegistryId { get; set; }
		public string Type { get; set; }
		public bool IDVerified { get; set; }
		public int Invited { get; set; }

		public bool SaveInRegistry { get; set; }

		public DateTime CreatedDateTime { get; set; }

        public string CustomerCIF { get; set; }
    }
}
