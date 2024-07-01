using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application
{
	public class SourceOfOutgoingTransactionsModel
	{
        public int rowID { get; set; }
        public int Id{ get;set; }
        //[Required(ErrorMessageResourceName = "NameOfBeneficiary", ErrorMessageResourceType = typeof(SourceOfOutgoingTransactionsModelErrorMessage))]
        //[Required(ErrorMessage = "Please enter Name of Remitter.")]
        [Display(Name = "Name of Beneficiary")]
        //[MaxLength(65, ErrorMessage = "Max Length is 65")]
		public string NameOfBeneficiary { get; set; }

		//[Required(ErrorMessage = "Please enter Country of Beneficiary.")]
		[Display(Name = "Country of Beneficiary")]
		public string CountryOfBeneficiary { get; set; }

		//[Required(ErrorMessage = "Please enter Country of Beneficiary Bank.")]
		[Display(Name = "Country of Beneficiary Bank")]
		public string CountryOfBeneficiaryBank { get; set; }

        ////[Required(ErrorMessage = "Please enter Country of Beneficiary.")]
        //[Display(Name = "Country of Beneficiary")]
        //public string CountryOfBeneficiaryName { get; set; }

        ////[Required(ErrorMessage = "Please enter Country of Beneficiary Bank.")]
        //[Display(Name = "Country of Beneficiary Bank")]
        //public string CountryOfBeneficiaryBankName { get; set; }
        public bool SourceOfOutgoingTransactions_Status { get; set; }
		[Display(Name ="Status")]
        public string StatusName { get; set; }
	}
    public class SourceOfOutgoingTransactionsModelErrorMessage
    {
        public static string NameOfBeneficiary
        {
            get
            {
                return ResHelper.GetString("Eurobank.SourceOfOutgoingTransactions.Error.NameOfBeneficiary");
            }
        }
        public const string CountryOfBeneficiaryError = "Eurobank.SourceOfOutgoingTransactions.Error.CountryOfBeneficiary";
        public const string CountryOfBeneficiaryBankError = "Eurobank.SourceOfOutgoingTransactions.Error.CountryOfBeneficiaryBank";

    }
}
