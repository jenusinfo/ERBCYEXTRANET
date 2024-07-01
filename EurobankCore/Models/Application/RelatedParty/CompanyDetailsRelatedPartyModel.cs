using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.RelatedParty
{
	public class CompanyDetailsRelatedPartyModel
	{
		
		public int Id { get; set; }
		public bool IsRelatedPartyUBO { get; set; }

		//[Required(ErrorMessage = "Registered Name is required")]
		public string RegisteredName { get; set; }

		//[Required(ErrorMessage = "Entity type is required")]
		public string EntityType { get; set; }

		public string CountryofIncorporation { get; set; }

		public string RegistrationNumber { get; set; }

		public Nullable<DateTime> DateofIncorporation { get; set; }

		public string HdnDateofIncorporation { get; set; }

		public string NodeGUID { get; set; }

		public string NodePath { get; set; }
		[Display(Name = "Is Applicant a PEP?")]
		public bool IsPep { get; set; }
		[Display(Name = "Is any of Applicant's Family Member or Close Associate a PEP?")]

		public bool IsRelatedToPep { get; set; }
		public string Type { get; set; }
		public bool IDVerified { get; set; }
		public int Invited { get; set; }
		public string Status { get; set; }
		public bool SaveInRegistry { get; set; }
		public int RegistryId { get; set; }
		public string Application_Type { get; set; }

		public string CustomerCIF { get; set; }	



    }
}
