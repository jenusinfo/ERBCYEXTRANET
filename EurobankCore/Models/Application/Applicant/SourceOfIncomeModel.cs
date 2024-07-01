using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant
{
	public class SourceOfIncomeModel
	{
		public int Id { get; set; }

		//[Required(ErrorMessageResourceName = "SourceOfAnnualIncome", ErrorMessageResourceType = typeof(SourceOfIncomeModelErrorMassage))]
		public string SourceOfAnnualIncome { get; set; }

		public string SourceOfAnnualIncomeName { get; set; }

		public string SpecifyOtherSource { get; set; }

		[RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "Only two digits allowed after decimal point")] //Accepts type 123, 123.4, 123.45
		public double? AmountOfIncome { get; set; }

		public string StatusName { get; set; }

		public bool Status { get; set; }
	}

	public class SourceOfIncomeModelErrorMassage
	{
		public static string SourceOfAnnualIncome
		{
			get
			{
				return ResHelper.GetString("Eurobank.Application.Applicant.SourceOfIncome.Error.SourceOfAnnualIncome");
			}
		}
		//public const string SourceOfAnnualIncome = "Eurobank.Application.Applicant.SourceOfIncome.Error.SourceOfAnnualIncome";
		public const string SpecifyOtherSource = "Eurobank.Application.Applicant.SourceOfIncome.Error.SpecifyOtherSource";
		public const string AmountOfIncome = "Eurobank.Application.Applicant.SourceOfIncome.Error.AmountOfIncome";
	}
}
