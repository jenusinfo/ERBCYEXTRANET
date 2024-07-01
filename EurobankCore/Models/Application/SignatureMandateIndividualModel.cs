using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CMS.Helpers;

namespace Eurobank.Models.Application
{
	public class SignatureMandateIndividualModel
	{
		public int rowID { get; set; }
		public int Id { get; set; }
		
		public string SignatoryPersons { get; set; }
	     //[Required(ErrorMessageResourceName = "SignatoryPersonsList", ErrorMessageResourceType = typeof(SignatureMandateIndividualModelErrorMessage))]
		public string[] SignatoryPersonsList { get; set; }
		public string SignatoryPersonsValueString { get; set; }

		public string SignatoryPersonNames { get; set; }

		public int? NumberOfSignatures { get; set; }

		public string AccessRights { get; set; }

		public string AccessRightsName { get; set; }

		public decimal? AmountFrom { get; set; }

		public decimal? AmountTo { get; set; }

		public bool SignatureMandateIndividual_Status { get; set; }
		[Display(Name = "Status")]
		public string Status_Name { get; set; }
	}
	public class SignatureMandateIndividualModelErrorMessage
	{
		public static string SignatoryPersonsList
		{
			get
			{
				return ResHelper.GetString("Eurobank.SignatureMandateIndividualModel.Error.SignatoryPersonsList");
			}
		}
		public const string NumberOfSignaturesError = "Eurobank.SignatureMandateIndividualModel.Error.NumberOfSignatures";
		public const string AccessRightsError = "Eurobank.SignatureMandateIndividualModel.Error.AccessRights";
		public const string AmountFromError = "Eurobank.SignatureMandateIndividualModel.Error.AmountFrom";
		public const string AmountToError = "Eurobank.SignatureMandateIndividualModel.Error.AmountTo";

	}
}
