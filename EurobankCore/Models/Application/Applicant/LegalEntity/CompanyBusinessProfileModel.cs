using Eurobank.Models.KendoExtention;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity
{
	public class CompanyBusinessProfileModel
	{
		public int Id { get; set; }

		public string MainBusinessActivities { get; set; }

		[RegularExpression(@"[0-9]+$", ErrorMessage = "Negative values are not allowed")]
		//public int? NumberofYearsinOperation { get; set; } = null;
		public string NumberofYearsinOperation { get; set; } = null;

		[RegularExpression(@"[0-9]+$", ErrorMessage = "Negative values are not allowed")]
		//public int? NumberofEmployes { get; set; }
		public string NumberofEmployes { get; set; }

		public string WebsiteAddress { get; set; }

		public bool CorporationIsengagedInTheProvision { get; set; }

		public string CorporationIsengagedInTheProvisionName { get; set; }

		public string CorporationIsengagedInTheProvisionValue { get; set; }

		public string IssuingAuthority { get; set; }

		public string EconomicSectorIndustry { get; set; }

		public string EconomicSectorIndustryName { get; set; }

		public string CountryofOriginofWealthActivities { get; set; }

		public string[] CountryofOriginofWealthActivitiesValues { get; set; }

		public string SponsoringEntityName { get; set; }
		public string LineOfBusinessOfTheSponsoringEntity { get; set; }
		public string WebsiteOfTheSponsoringEntity { get; set; }

		//public MultiselectDropDownViewModel CountryofOriginofWealthActivitiesGroup { get; set; }
	}
}
