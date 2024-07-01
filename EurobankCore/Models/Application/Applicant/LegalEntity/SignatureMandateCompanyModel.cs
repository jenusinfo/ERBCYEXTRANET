using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity
{
	public class SignatureMandateCompanyModel
	{
		public int Id { get; set; }

		public decimal? LimitFrom { get; set; }

		public decimal? LimitTo { get; set; }

		public int? TotalNumberofSignature { get; set; }

		public string AuthorizedSignatoryGroup { get; set; }

		public string AuthorizedSignatoryGroupName { get; set; }
		public string AuthorizedSignatoryGroupValueList { get; set; }
		public string[] AuthorizedSignatoryGroupList { get; set; }

		public int? NumberofSignatures { get; set; }

		public string Rights { get; set; }

		public string AuthorizedSignatoryGroup1 { get; set; }

		public string AuthorizedSignatoryGroup1Name { get; set; }
		public string AuthorizedSignatoryGroupOneValueList { get; set; }
		public string[] AuthorizedSignatoryGroup1List { get; set; }

		public int? NumberofSignatures1 { get; set; }

		public bool Status { get; set; }

		public string StatusName { get; set; }
		public string MandateType { get; set; }
		public string MandateTypeName { get; set; }
		public string Description { get; set; }
	}

	public class SignatureMandateCompanyErrorMassage
	{
		public const string LimitFrom = "Eurobank.Application.Applicant.SignatureMandateCompany.Error.LimitFrom";
		public const string LimitTo = "Eurobank.Application.Applicant.SignatureMandateCompany.Error.LimitTo";
		public const string TotalNumberofSignature = "Eurobank.Application.Applicant.SignatureMandateCompany.Error.TotalNumberofSignature";
	}
}
