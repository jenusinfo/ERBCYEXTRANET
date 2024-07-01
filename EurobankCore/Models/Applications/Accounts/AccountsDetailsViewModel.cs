using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications.Accounts
{
	public class AccountsDetailsViewModel
	{
		public int rowID { get; set; }
		public int AccountsID { get; set; }
		//[Required(ErrorMessage = "Please select Account Type.")]
		//[Required(ErrorMessageResourceName = "Accounts_AccountType", ErrorMessageResourceType = typeof(AccountsDetailsViewModelErrorMessage))]
		[Display(Name = "Account Type")]
		public string Accounts_AccountType { get; set; }
		[Display(Name = "Account Type")]
		public string Accounts_AccountTypeName { get; set; }



        //[Required(ErrorMessage = "Please select Currency.")]
        [Display(Name = "Currency")]
		public string Accounts_Currency { get; set; }
		[Display(Name = "Currency")]
		public string Accounts_CurrencyName { get; set; }
		//[Required(ErrorMessage = "Please select  Statement Frequency.")]
		[Display(Name = "Statement Frequency")]
		public string Accounts_StatementFrequency { get; set; }
		[Display(Name = "Statement Frequency")]
		public string Accounts_StatementFrequencyName { get; set; }
		public string NodeAliaspath { get; set; }
		public bool Accounts_Status { get; set; }
		[Display(Name = "Status")]
		public string Account_Status_Name { get; set; }
	}
	public class AccountsDetailsViewModelErrorMessage
	{
		public static string Accounts_AccountType
		{
			get
			{
				return ResHelper.GetString("Eurobank.AccountsDetails.Error.Accounts_AccountType");
			}
		}
		public const string Accounts_CurrencyError = "Eurobank.AccountsDetails.Error.Accounts_Currency";
		public const string Accounts_StatementFrequencyError = "Eurobank.AccountsDetails.Error.Accounts_StatementFrequency";

	}
}
