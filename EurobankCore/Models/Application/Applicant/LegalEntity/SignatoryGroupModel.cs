using CMS.Helpers;
using Eurobank.Models.KendoExtention;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity
{
	public class SignatoryGroupModel
	{
		public int Id { get; set; }
		
		public string SignatoryGroup { get; set; }
		//[Required(ErrorMessageResourceName = "SignatoryGroupName", ErrorMessageResourceType = typeof(SignatoryGroupErrorMassage))]
		public string SignatoryGroupName { get; set; }

		public string SignatoryPersons { get; set; }

		public string SignatoryPersonNames { get; set; }

		public string[] SignatoryPersonsList { get; set; }

		public Nullable<DateTime> StartDate { get; set; }

		public Nullable<DateTime> EndDate { get; set; }

		public string StartDateString { get; set; }

		public string EndDateString { get; set; }

		public bool Status { get; set; }

		public string StatusName { get; set; }

		public string SignatureRightsValue { get; set; }

		public string SignatureRightsName { get; set; }

		//public bool? SignatureRightAnyOneAloneCanSign { get; set; }

		//public bool SignatureRightAllJointly { get; set; }

		public RadioGroupViewModel SignatureRights { get; set; }
	}

	public class SignatoryGroupErrorMassage
	{
		public static string SignatoryGroupName
		{
			get
			{
				return ResHelper.GetString("Eurobank.Application.Applicant.SignatoryGroup.Error.SignatoryGroupName");
			}
		}
		//public const string SignatoryGroupName = "Eurobank.Application.Applicant.SignatoryGroup.Error.SignatoryGroupName";
		public const string SignatoryPersons = "Eurobank.Application.Applicant.SignatoryGroup.Error.SignatoryPersons";
		public const string StartDate = "Eurobank.Application.Applicant.SignatoryGroup.Error.StartDate";
		public const string EndDate = "Eurobank.Application.Applicant.SignatoryGroup.Error.EndDate";
	}
}
