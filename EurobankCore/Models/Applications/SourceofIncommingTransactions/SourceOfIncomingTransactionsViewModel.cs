using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications.SourceofIncommingTransactions
{
    public class SourceOfIncomingTransactionsViewModel
    {
        public int rowID { get; set; }
        public int apllicationID { get; set; }
        public string nodePath { get; set; }
        public int SourceOfIncomingTransactionsID { get; set; }
        //[Required(ErrorMessageResourceName = "SourceOfIncomingTransactions_NameOfRemitter", ErrorMessageResourceType = typeof(SourceOfIncomingTransactionsViewModelErrorMessage))]
        //[Required(ErrorMessage = "Please enter Name of Remitter.")]
        [Display(Name = "Name of Remitter")]
        public string SourceOfIncomingTransactions_NameOfRemitter { get; set; }
        //[Required(ErrorMessage = "Please enter Country of Remitter.")]
        [Display(Name = "Country of Remitter")]
        public string SourceOfIncomingTransactions_CountryOfRemitter { get; set; }
        [Display(Name = "Country of Remitter")]
        public string SourceOfIncomingTransactions_CountryOfRemitterName { get; set; }
        //[Required(ErrorMessage = "Please enter Country of Remitter's Bank.")]
        [Display(Name = "Country of Remitter's Bank")]
        public string SourceOfIncomingTransactions_CountryOfRemitterBank { get; set; }
        [Display(Name = "Country of Remitter's Bank")]
        public string SourceOfIncomingTransactions_CountryOfRemitterBankName { get; set; }
        [Display(Name = "Status")]
        public string SourceOfIncomingTransactions_Status_Name { get; set; }
        public bool SourceOfIncomingTransactions_Status { get; set; }

    }
    public class SourceOfIncomingTransactionsViewModelErrorMessage
    {
        public static string SourceOfIncomingTransactions_NameOfRemitter
        {
            get
            {
                return ResHelper.GetString("Eurobank.SourceofIncommingTransactions.Error.SourceOfIncomingTransactions_NameOfRemitter");
            }
        }
        public const string SourceOfIncomingTransactions_CountryOfRemitterError = "Eurobank.SourceofIncommingTransactions.Error.SourceOfIncomingTransactions_CountryOfRemitter";
        public const string SourceOfIncomingTransactions_CountryOfRemitterBankError = "Eurobank.SourceofIncommingTransactions.Error.SourceOfIncomingTransactions_CountryOfRemitterBank";

    }
}
