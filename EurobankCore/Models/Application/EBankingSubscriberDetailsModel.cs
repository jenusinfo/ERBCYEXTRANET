using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application
{
    public class EBankingSubscriberDetailsModel
    {
        public int rowID { get; set; }
        public int Id { get; set; }
        public int PartyReferenceId { get; set; }
        //[Required(ErrorMessageResourceName = "Subscriber", ErrorMessageResourceType = typeof(EBankingSubscriberDetailsModelErrorMessage))]
        [Display(Name = "Designated User(select from related parties and applicants)")]
        public string Subscriber { get; set; }
        [Display(Name = "Designated User")]
        public string SubscriberName { get; set; }
        [Display(Name = "Identity / PassportNumber")]
        public string IdentityPassportNumber { get; set; }
        [Display(Name = "Country Of Issue")]
        public string CountryOfIssue { get; set; }
        [Display(Name = "Access Level")]
        public string AccessLevel { get; set; }

        public string AccessLevelName { get; set; }
        [Display(Name = "Access to ALL Accounts")]
        public string SignatoryGroup { get; set; }
        public string SignatoryGroupName { get; set; }
        public bool AccessToAllPersonalAccounts { get; set; }
        //[Display(Name = "Automatically Add Future Personal Accounts")]
        public bool AutomaticallyAddFuturePersonalAccounts { get; set; }

        public string AccessToAllPersonalAccountsValue { get; set; }

        public string AutomaticallyAddFuturePersonalAccountsValue { get; set; }

        public bool ReceiveEStatements { get; set; }

        public bool EbankingSubscriberDetails_Status { get; set; }
        public string Status_Name { get; set; }
        [Display(Name ="Limit Amount")]
        public string LimitAmount { get; set; }
        public string LimitAmountName { get; set; }
    }
    public class EBankingSubscriberDetailsModelErrorMessage
    {
        public static string Subscriber
        {
            get
            {
                return ResHelper.GetString("Eurobank.EBankingSubscriberDetails.Error.Subscriber");
            }
        }
        public const string IdentityPassportNumberError = "Eurobank.EBankingSubscriberDetails.Error.IdentityPassportNumber";
        public const string CountryOfIssueError = "Eurobank.EBankingSubscriberDetails.Error.CountryOfIssue";
        public const string AccessLevelError = "Eurobank.EBankingSubscriberDetails.Error.AccessLevel";

    }
}
