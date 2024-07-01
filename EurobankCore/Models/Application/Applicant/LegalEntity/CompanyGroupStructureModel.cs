using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity
{
	public class CompanyGroupStructureModel
	{
		public int Id { get; set; }

		public int SlNo { get; set; }

		//public bool DoestheEntitybelongtoaGroup { get; set; }

		//public string GroupName { get; set; }

		//public string GroupActivities { get; set; }

		public string EntityType { get; set; }

		public string EntityTypeName { get; set; }

		public string EntityName { get; set; }

		public string Parent { get; set; }

		public string ParentName { get; set; }

		//public string Ownership { get; set; }

		//public string Participation { get; set; }

		public string BusinessActivities { get; set; }

		//public string WebsiteAddressoftheSponsoringEntityGroup { get; set; }

		public bool Status { get; set; }

		public string StatusName { get; set; }
	}

	public class CompanyGroupStructureModelErrorMessage
	{
		public const string EntityType = "Eurobank.Application.Applicant.CompanyGroupStructure.Error.EntityType";
		public const string EntityName = "Eurobank.Application.Applicant.CompanyGroupStructure.Error.EntityName";
		public const string GroupName = "Eurobank.Application.Applicant.CompanyGroupStructure.Error.GroupName";
		public const string GroupActivities = "Eurobank.Application.Applicant.CompanyGroupStructure.Error.GroupActivities";

	}
}
