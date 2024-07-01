using CMS.Helpers;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Applications.Accounts;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.Applications.DecisionHistory;
using Eurobank.Models.Applications.SourceofIncommingTransactions;
using Eurobank.Models.Documents;
using Eurobank.Models.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class ApplicationFormBasicValidationProcess
    {
        #region----Accounts Details------
        public static ValidationResultModel ValidateAccountDetails(AccountsDetailsViewModel accountsDetailsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ACCOUNTS,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(accountsDetailsViewModel.Accounts_AccountType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Accounts_AccountType;
                validationError.PropertyName = "Accounts_AccountType";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(accountsDetailsViewModel.Accounts_Currency))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Accounts_Currency;
                validationError.PropertyName = "Accounts_Currency";
                lstvalidationError.Add(validationError);
            }
            //if (string.IsNullOrEmpty(accountsDetailsViewModel.Accounts_StatementFrequency))
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.Accounts_StatementFrequency;
            //    validationError.PropertyName = "Accounts_StatementFrequency";
            //    lstvalidationError.Add(validationError);
            //}
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Purpose And Activity-----
        public static ValidationResultModel ValidatePurposeAndActivity(PurposeAndActivityModel purposeAndActivityModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.BANK_ACCOUNT_PURPOSE_AND_ANTICIPATED_ACTIVITY,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (purposeAndActivityModel.ReasonForOpeningTheAccountGroup == null || purposeAndActivityModel.ReasonForOpeningTheAccountGroup.MultiSelectValue == null || purposeAndActivityModel.ReasonForOpeningTheAccountGroup.MultiSelectValue.Length == 0)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ReasonForOpeningTheAccountGroup;
                validationError.PropertyName = "ReasonForOpeningTheAccountGroup";
                lstvalidationError.Add(validationError);
            }
            if (purposeAndActivityModel.ExpectedNatureOfInAndOutTransactionGroup == null || purposeAndActivityModel.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue == null || purposeAndActivityModel.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue.Length == 0)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ExpectedNatureOfInAndOutTransactionGroup;
                validationError.PropertyName = "ExpectedNatureOfInAndOutTransactionGroup";
                lstvalidationError.Add(validationError);
            }
            if (purposeAndActivityModel.ExpectedFrequencyOfInAndOutTransactionGroup == null || purposeAndActivityModel.ExpectedFrequencyOfInAndOutTransactionGroup.RadioGroupValue == null || purposeAndActivityModel.ExpectedFrequencyOfInAndOutTransactionGroup.RadioGroupValue.Length == 0)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ExpectedFrequencyOfInAndOutTransactionGroup;
                validationError.PropertyName = "ExpectedFrequencyOfInAndOutTransactionGroup";
                lstvalidationError.Add(validationError);
            }
            if (purposeAndActivityModel.ExpectedIncomingAmount == null || purposeAndActivityModel.ExpectedIncomingAmount == "0")
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ExpectedIncomingAmount;
                validationError.PropertyName = "ExpectedIncomingAmount";
                lstvalidationError.Add(validationError);
            }
            else if (Convert.ToDecimal( purposeAndActivityModel.ExpectedIncomingAmount) < 0)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ExpectedIncomingAmount_Negetive;
                validationError.PropertyName = "ExpectedIncomingAmount";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---Source Of Incoming Transaction---
        public static ValidationResultModel ValidateSourceOfIncomingTransaction(SourceOfIncomingTransactionsViewModel sourceOfIncomingTransactionsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.COUNTERPARTIES_OF_INCOMING_TRANSACTION,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_NameOfRemitter))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.SourceOfIncomingTransactions_NameOfRemitter;
                validationError.PropertyName = "SourceOfIncomingTransactions_NameOfRemitter";
                lstvalidationError.Add(validationError);
            }
            else if (sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_NameOfRemitter.Length > 65)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.MaxLength_65_Exceeded;
                validationError.PropertyName = "SourceOfIncomingTransactions_NameOfRemitter";
                lstvalidationError.Add(validationError);
            }
            else if (!string.IsNullOrEmpty(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_NameOfRemitter))
            {
                if (string.IsNullOrEmpty(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.SourceOfIncomingTransactions_CountryOfRemitter;
                    validationError.PropertyName = "SourceOfIncomingTransactions_CountryOfRemitter";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.SourceOfIncomingTransactions_CountryOfRemitterBank;
                    validationError.PropertyName = "SourceOfIncomingTransactions_CountryOfRemitterBank";
                    lstvalidationError.Add(validationError);
                }
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---Source Of Outgoing Transaction---
        public static ValidationResultModel ValidateSourceOfOutgoingTransaction(SourceOfOutgoingTransactionsModel sourceOfOutgoingTransactionsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.COUNTERPARTIES_OF_OUTGOING_TRANSACTION,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(sourceOfOutgoingTransactionsModel.NameOfBeneficiary))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.NameOfBeneficiary;
                validationError.PropertyName = "NameOfBeneficiary";
                lstvalidationError.Add(validationError);
            }
            else if (sourceOfOutgoingTransactionsModel.NameOfBeneficiary.Length > 65)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.MaxLength_65_Exceeded;
                validationError.PropertyName = "NameOfBeneficiary";
                lstvalidationError.Add(validationError);
            }
            else if (!string.IsNullOrEmpty(sourceOfOutgoingTransactionsModel.NameOfBeneficiary))
            {
                if (string.IsNullOrEmpty(sourceOfOutgoingTransactionsModel.CountryOfBeneficiary))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CountryOfBeneficiary;
                    validationError.PropertyName = "CountryOfBeneficiary";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(sourceOfOutgoingTransactionsModel.CountryOfBeneficiaryBank))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CountryOfBeneficiaryBank;
                    validationError.PropertyName = "CountryOfBeneficiaryBank";
                    lstvalidationError.Add(validationError);
                }
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region----Debit Card Details----
        public static ValidationResultModel ValidateDebitCardDetails(DebitCardDetailsViewModel debitCardDetailsViewModel, string applicationType, int applicationID)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.APPLICATION_DEBITCARD_DETAILS,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            var DebitCardAccount = AccountsProcess.GetDebitCardAccountsByApplicationID(applicationID);
            var DebitCardDetails = DebitCardeDetailsProcess.GetDebitCardDetailsByApplicationID(applicationID);
            
            bool isLegalEntity = string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase);
            if (isLegalEntity)
            {
                var LegalCardHoderName = CommonProcess.GetCardHolderNameLegal(ServiceHelper.GetApplicationNumber(applicationID, applicationType));
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CardType))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CardType;
                    validationError.PropertyName = "DebitCardDetails_CardType";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CardholderName) || !LegalCardHoderName.Any(x => string.Equals(x.Value, debitCardDetailsViewModel.DebitCardDetails_CardholderName, StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CardholderName;
                    validationError.PropertyName = "DebitCardDetails_CardholderName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.Country_Code))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Country_Code;
                    validationError.PropertyName = "Country_Code";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_MobileNumber))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_MobileNumber;
                    validationError.PropertyName = "DebitCardDetails_MobileNumber";
                    lstvalidationError.Add(validationError);
                }
                if (!string.IsNullOrEmpty(debitCardDetailsViewModel.Country_Code) && !string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_MobileNumber))
                {
                    if (debitCardDetailsViewModel.Country_Code == "357")
                    {
                        if (debitCardDetailsViewModel.DebitCardDetails_MobileNumber.Length != 8)
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_MobileNumber_CyprusLength;
                            validationError.PropertyName = "DebitCardDetails_MobileNumber";
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
                //else if (debitCardDetailsViewModel.DebitCardDetails_MobileNumber.Length > 8)
                //{
                //    ValidationError validationError = new ValidationError();
                //    retVal.IsValid = false;
                //    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_MobileNumber_MaxLength;
                //    validationError.PropertyName = "DebitCardDetails_MobileNumber";
                //    lstvalidationError.Add(validationError);
                //}
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_FullName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_FullName;
                    validationError.PropertyName = "DebitCardDetails_FullName";
                    lstvalidationError.Add(validationError);
                }
                else if (debitCardDetailsViewModel.DebitCardDetails_FullName.Length > 26)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.MaxLength_26_Exceeded;
                    validationError.PropertyName = "DebitCardDetails_FullName";
                    lstvalidationError.Add(validationError);
                }
                else if (!Regex.IsMatch(debitCardDetailsViewModel.DebitCardDetails_FullName, ValidationConstant.RegexNameOnDebitCard))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_FullName_Regex;
                    validationError.PropertyName = "DebitCardDetails_FullName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CompanyNameAppearOnCard))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CompanyNameAppearOnCard;
                    validationError.PropertyName = "DebitCardDetails_CompanyNameAppearOnCard";
                    lstvalidationError.Add(validationError);
                }
                else if (debitCardDetailsViewModel.DebitCardDetails_CompanyNameAppearOnCard.Length > 26)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.MaxLength_26_Exceeded;
                    validationError.PropertyName = "DebitCardDetails_CompanyNameAppearOnCard";
                    lstvalidationError.Add(validationError);
                }
                //else if (!Regex.IsMatch(debitCardDetailsViewModel.DebitCardDetails_CompanyNameAppearOnCard, ValidationConstant.RegexNameOnDebitCard))
                //{
                //    ValidationError validationError = new ValidationError();
                //    retVal.IsValid = false;
                //    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CompanyNameAppearOnCard_Regex;
                //    validationError.PropertyName = "DebitCardDetails_CompanyNameAppearOnCard";
                //    lstvalidationError.Add(validationError);
                //}
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CardHolderIdentityNumber))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CardHolderIdentityNumber;
                    validationError.PropertyName = "DebitCardDetails_CardHolderIdentityNumber";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CardHolderCountryOfIssue))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CardHolderCountryOfIssue;
                    validationError.PropertyName = "DebitCardDetails_CardHolderCountryOfIssue";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.AssociatedAccount) || !DebitCardAccount.Any(x => string.Equals(x.Value, debitCardDetailsViewModel.AssociatedAccount, StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.AssociatedAccount;
                    validationError.PropertyName = "AssociatedAccount";
                    lstvalidationError.Add(validationError);
                }



                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DispatchMethod))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_DispatchMethod;
                    validationError.PropertyName = "DebitCardDetails_DispatchMethod";
                    lstvalidationError.Add(validationError);
                }
                if (!string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DispatchMethod))
                {
                    //string despatchMethodName = ServiceHelper.GetName(ValidationHelper.GetString(debitCardDetailsViewModel.DebitCardDetails_DispatchMethod, ""), Constants.DISPATCH_METHOD);
                    string despatchMethodName = ServiceHelper.GetDispatchMethod().Where(x => x.Value == debitCardDetailsViewModel.DebitCardDetails_DispatchMethod).Select(x => x.Text).FirstOrDefault();
                    if (string.Equals(despatchMethodName, "To be collected from the banking centre by (Full Name and Identity Card/Passport No.)", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CollectedBy))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CollectedBy;
                            validationError.PropertyName = "DebitCardDetails_CollectedBy";
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_IdentityNumber))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_IdentityNumber;
                            validationError.PropertyName = "DebitCardDetails_IdentityNumber";
                            lstvalidationError.Add(validationError);
                        }
                        //if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DeliveryDetails))
                        //{
                        //    ValidationError validationError = new ValidationError();
                        //    retVal.IsValid = false;
                        //    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_DeliveryDetails;
                        //    validationError.PropertyName = "DebitCardDetails_DeliveryDetails";
                        //    lstvalidationError.Add(validationError);
                        //}

                    }
                    //else if (string.Equals(despatchMethodName, "To be collected from the Banking Centre by an authorised cardholder", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DeliveryDetails))
                    //    {
                    //        ValidationError validationError = new ValidationError();
                    //        retVal.IsValid = false;
                    //        validationError.ErrorMessage = ValidationConstant.DebitCardDetails_DeliveryDetails;
                    //        validationError.PropertyName = "DebitCardDetails_DeliveryDetails";
                    //        lstvalidationError.Add(validationError);
                    //    }
                    //}
                    else if (string.Equals(despatchMethodName, "To be dispatched by courier service to the following address (Applicable for Overseas Addresses)", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DeliveryAddress))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_DeliveryAddress;
                            validationError.PropertyName = "DebitCardDetails_DeliveryAddress";
                            lstvalidationError.Add(validationError);
                        }
                        else if (string.Equals(debitCardDetailsViewModel.DebitCardDetails_DeliveryAddress, "Other Address", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_OtherDeliveryAddress))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_OtherDeliveryAddress;
                            validationError.PropertyName = "DebitCardDetails_OtherDeliveryAddress";
                            lstvalidationError.Add(validationError);
                        }
                        //if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DeliveryDetails))
                        //{
                        //    ValidationError validationError = new ValidationError();
                        //    retVal.IsValid = false;
                        //    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_DeliveryDetails;
                        //    validationError.PropertyName = "DebitCardDetails_DeliveryDetails";
                        //    lstvalidationError.Add(validationError);
                        //}
                    }
                }

                if (DebitCardDetails != null && DebitCardDetails.Any())
                {
                    var isCardRecordExist = DebitCardDetails.Where(x => string.Equals(x.DebitCardDetails_CardType, debitCardDetailsViewModel.DebitCardDetails_CardType, StringComparison.OrdinalIgnoreCase)
                             && string.Equals(x.DebitCardDetails_CardholderName, debitCardDetailsViewModel.DebitCardDetails_CardholderName, StringComparison.OrdinalIgnoreCase)
                             && x.DebitCardDetailsID != debitCardDetailsViewModel.DebitCardDetailsID);

                    if (isCardRecordExist != null && isCardRecordExist.Any())
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.DebitCardDetails_Unique;
                        validationError.PropertyName = "DebitCardDetails_CardholderName";
                        lstvalidationError.Add(validationError);
                    } 
                }

            }
            else
            {
                var IndividualCardHoderName = CommonProcess.GetCardHolderNameIndividual(ServiceHelper.GetApplicationNumber(applicationID, applicationType));
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CardType))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CardType;
                    validationError.PropertyName = "DebitCardDetails_CardType";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CardholderName) || !IndividualCardHoderName.Any(x => string.Equals(x.Value, debitCardDetailsViewModel.DebitCardDetails_CardholderName, StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CardholderName;
                    validationError.PropertyName = "DebitCardDetails_CardholderName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.AssociatedAccount) || !DebitCardAccount.Any(x=> string.Equals(x.Value, debitCardDetailsViewModel.AssociatedAccount, StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_AssociatedAccount;
                    validationError.PropertyName = "AssociatedAccount";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.Country_Code))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Country_Code;
                    validationError.PropertyName = "Country_Code";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_MobileNumber))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_MobileNumber;
                    validationError.PropertyName = "DebitCardDetails_MobileNumber";
                    lstvalidationError.Add(validationError);
                }
                if (!string.IsNullOrEmpty(debitCardDetailsViewModel.Country_Code) && !string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_MobileNumber))
                {
                    if (debitCardDetailsViewModel.Country_Code == "357")
                    {
                        if (debitCardDetailsViewModel.DebitCardDetails_MobileNumber.Length != 8)
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_MobileNumber_CyprusLength;
                            validationError.PropertyName = "DebitCardDetails_MobileNumber";
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
                //else if(debitCardDetailsViewModel.DebitCardDetails_MobileNumber.Length > 8)
                //{
                //    ValidationError validationError = new ValidationError();
                //    retVal.IsValid = false;
                //    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_MobileNumber_MaxLength;
                //    validationError.PropertyName = "DebitCardDetails_MobileNumber";
                //    lstvalidationError.Add(validationError);
                //}
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_FullName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_FullName;
                    validationError.PropertyName = "DebitCardDetails_FullName";
                    lstvalidationError.Add(validationError);
                }
                else if (debitCardDetailsViewModel.DebitCardDetails_FullName.Length > 26)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.MaxLength_26_Exceeded;
                    validationError.PropertyName = "DebitCardDetails_FullName";
                    lstvalidationError.Add(validationError);
                }
                else if (!Regex.IsMatch(debitCardDetailsViewModel.DebitCardDetails_FullName, ValidationConstant.RegexNameOnDebitCard))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_FullName_Regex;
                    validationError.PropertyName = "DebitCardDetails_FullName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DispatchMethod))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DebitCardDetails_DispatchMethod;
                    validationError.PropertyName = "DebitCardDetails_DispatchMethod";
                    lstvalidationError.Add(validationError);
                }
                else
                {
                    //string despatchMethodName = ServiceHelper.GetName(ValidationHelper.GetString(debitCardDetailsViewModel.DebitCardDetails_DispatchMethod, ""), Constants.DISPATCH_METHOD_INDIVIDUAL);
                    string despatchMethodName = ServiceHelper.GetDispatchMethodIndividual().Where(x=>x.Value== debitCardDetailsViewModel.DebitCardDetails_DispatchMethod).Select(x=>x.Text).FirstOrDefault();
                    if (string.Equals(despatchMethodName, "To be collected from the banking centre by (Full Name and Identity Card/Passport No.)", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_CollectedBy))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_CollectedBy_Individual;
                            validationError.PropertyName = "DebitCardDetails_CollectedBy";
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_IdentityNumber))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_IdentityNumber;
                            validationError.PropertyName = "DebitCardDetails_IdentityNumber";
                            lstvalidationError.Add(validationError);
                        }
                    }
                    else if (string.Equals(despatchMethodName, "To be dispatched by courier service to the following address (Applicable only for Overseas Addresses)", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DeliveryAddress))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_DeliveryAddress;
                            validationError.PropertyName = "DebitCardDetails_DeliveryAddress";
                            lstvalidationError.Add(validationError);
                        }
                        else if (string.Equals(debitCardDetailsViewModel.DebitCardDetails_DeliveryAddress, "Other Address", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_OtherDeliveryAddress))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.DebitCardDetails_OtherDeliveryAddress;
                            validationError.PropertyName = "DebitCardDetails_OtherDeliveryAddress";
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
				if (DebitCardDetails != null && DebitCardDetails.Any())
				{
					var isCardRecordExist = DebitCardDetails.Where(x => string.Equals(x.DebitCardDetails_CardType, debitCardDetailsViewModel.DebitCardDetails_CardType, StringComparison.OrdinalIgnoreCase)
							 && string.Equals(x.DebitCardDetails_CardholderName, debitCardDetailsViewModel.DebitCardDetails_CardholderName, StringComparison.OrdinalIgnoreCase)
							 && x.DebitCardDetailsID != debitCardDetailsViewModel.DebitCardDetailsID);

					if (isCardRecordExist != null && isCardRecordExist.Any())
					{
						ValidationError validationError = new ValidationError();
						retVal.IsValid = false;
						validationError.ErrorMessage = ValidationConstant.DebitCardDetails_Unique;
						validationError.PropertyName = "DebitCardDetails_CardholderName";
						lstvalidationError.Add(validationError);
					}
				}
			}

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----EBanking Subscriber-----
        public static ValidationResultModel ValidateEBankingSuscriber(EBankingSubscriberDetailsModel eBankingSubscriberDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.EBANKING_SUBSCRIBERS,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (string.IsNullOrEmpty(eBankingSubscriberDetailsModel.Subscriber))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Subscriber;
                validationError.PropertyName = "Subscriber";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(eBankingSubscriberDetailsModel.IdentityPassportNumber))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IdentityPassportNumber;
                validationError.PropertyName = "IdentityPassportNumber";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(eBankingSubscriberDetailsModel.CountryOfIssue))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CountryOfIssue;
                validationError.PropertyName = "CountryOfIssue";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(eBankingSubscriberDetailsModel.AccessLevel) || string.Equals(eBankingSubscriberDetailsModel.AccessLevel, "00000000-0000-0000-0000-000000000000", StringComparison.OrdinalIgnoreCase))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AccessLevel;
                validationError.PropertyName = "AccessLevel";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(eBankingSubscriberDetailsModel.AccessLevel))
            {
                string accessLevelName = ServiceHelper.GetName(eBankingSubscriberDetailsModel.AccessLevel,Constants.Access_Level);
                if (accessLevelName.ToUpper() == "FULL") // || accessLevelName.ToUpper() == "AUTHORISE"
				{
                    if (string.IsNullOrEmpty(eBankingSubscriberDetailsModel.SignatoryGroupName) || string.Equals(eBankingSubscriberDetailsModel.SignatoryGroupName, "00000000-0000-0000-0000-000000000000", StringComparison.OrdinalIgnoreCase))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.SignatoryGroupName;
                        validationError.PropertyName = "SignatoryGroupName";
                        lstvalidationError.Add(validationError);
                    }
                    
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Signature Mandate-----
        public static ValidationResultModel ValidateSignatureMandate(SignatureMandateCompanyModel signatureMandateCompanyModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            //if (signatureMandateCompanyModel.LimitFrom==null)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.LimitFrom;
            //    validationError.PropertyName = "LimitFrom";
            //    lstvalidationError.Add(validationError);
            //}
            //if (signatureMandateCompanyModel.LimitTo == null)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.LimitTo;
            //    validationError.PropertyName = "LimitTo";
            //    lstvalidationError.Add(validationError);
            //}
            if (signatureMandateCompanyModel.TotalNumberofSignature == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.TotalNumberofSignature;
                validationError.PropertyName = "TotalNumberofSignature";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateSignatureMandateLegal(SignatureMandateCompanyModel signatureMandateCompanyModel, string applicationNumber)
        {
            var signatureMandateLegal = SignatureMandateLegalProcess.GetSignatureMandateLegalModels(applicationNumber);
            var mandateTypes = ServiceHelper.GetMandateType();
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (signatureMandateCompanyModel.LimitFrom == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.LimitFrom;
                validationError.PropertyName = "LimitFrom";
                lstvalidationError.Add(validationError);
            }
            if (signatureMandateCompanyModel.LimitTo == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.LimitTo;
                validationError.PropertyName = "LimitTo";
                lstvalidationError.Add(validationError);
            }
            if (signatureMandateCompanyModel.TotalNumberofSignature == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.TotalNumberofSignature;
                validationError.PropertyName = "TotalNumberofSignature";
                lstvalidationError.Add(validationError);
            }

            if (signatureMandateCompanyModel.AuthorizedSignatoryGroupList == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AuthorizedSignatoryGroup;
                validationError.PropertyName = "AuthorizedSignatoryGroupList";
                lstvalidationError.Add(validationError);
            }
            if (signatureMandateCompanyModel.Rights != null)
            {
                if (signatureMandateCompanyModel.AuthorizedSignatoryGroup1List == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.AuthorizedSignatoryGroup;
                    validationError.PropertyName = "AuthorizedSignatoryGroup1List";
                    lstvalidationError.Add(validationError);
                }
            }


            if (signatureMandateCompanyModel.AuthorizedSignatoryGroupValueList != null)
            {
                if (signatureMandateCompanyModel.NumberofSignatures == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.NumberofSignature;
                    validationError.PropertyName = "NumberofSignatures";
                    lstvalidationError.Add(validationError);
                }
            }
            if (signatureMandateCompanyModel.AuthorizedSignatoryGroupOneValueList != null)
            {
                if (signatureMandateCompanyModel.NumberofSignatures1 == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.NumberofSignature;
                    validationError.PropertyName = "NumberofSignatures1";
                    lstvalidationError.Add(validationError);
                }
            }
            if (signatureMandateCompanyModel.TotalNumberofSignature != null)
            {
                int numberOfSignature = 0;
                int numberofSignatures1 = 0;
                if (signatureMandateCompanyModel.NumberofSignatures != null)
                    numberOfSignature = Convert.ToInt32(signatureMandateCompanyModel.NumberofSignatures);
                if (signatureMandateCompanyModel.NumberofSignatures1 != null)
                    numberofSignatures1 = Convert.ToInt32(signatureMandateCompanyModel.NumberofSignatures1);

                int numberOfSign = Convert.ToInt32(numberOfSignature + numberofSignatures1);
                if (signatureMandateCompanyModel.Rights == "716b2ddb-6837-4b8a-8aa3-ce1f6c577f44") //AND
                {
                    if (signatureMandateCompanyModel.TotalNumberofSignature != numberOfSign)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CalculateTotalNumberOfSignature;
                        validationError.PropertyName = "TotalNumberofSignature";
                        lstvalidationError.Add(validationError);
                    }
                }
                else if(signatureMandateCompanyModel.Rights == "88bb92a4-3f06-4e91-b2e2-f66f4eb3308a") //OR
                {
                    if (signatureMandateCompanyModel.TotalNumberofSignature != numberOfSignature || signatureMandateCompanyModel.TotalNumberofSignature != numberofSignatures1)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CalculateTotalNumberOfSignatureORright;
                        validationError.PropertyName = "TotalNumberofSignature";
                        lstvalidationError.Add(validationError);
                    }
                }
                else
                {
                    if (signatureMandateCompanyModel.TotalNumberofSignature != numberOfSignature)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CalculateTotalNumberOfSignatureORright;
                        validationError.PropertyName = "TotalNumberofSignature";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            if (signatureMandateCompanyModel.AuthorizedSignatoryGroupOneValueList != null && signatureMandateCompanyModel.AuthorizedSignatoryGroupValueList != null)
            {
                List<string> list1 = signatureMandateCompanyModel.AuthorizedSignatoryGroupOneValueList.Remove(signatureMandateCompanyModel.AuthorizedSignatoryGroupOneValueList.Length - 1, 1).Split('|').ToList();
                List<string> list2 = signatureMandateCompanyModel.AuthorizedSignatoryGroupValueList.Remove(signatureMandateCompanyModel.AuthorizedSignatoryGroupValueList.Length - 1, 1).Split('|').ToList();
                bool isEqual = list1.Any(x => list2.Contains(x));
                if (isEqual)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DifferentAuthorizedSignatoryGroup;
                    validationError.PropertyName = "AuthorizedSignatoryGroup1List";
                    lstvalidationError.Add(validationError);
                }
            }
            string currentMandateTypename = mandateTypes.FirstOrDefault(x => string.Equals(x.Value, signatureMandateCompanyModel.MandateType, StringComparison.OrdinalIgnoreCase)).Text;
            if (string.Equals(currentMandateTypename,"FOR AUTHORISED PERSONS",StringComparison.OrdinalIgnoreCase))
            {
                if(signatureMandateLegal.Exists(x=> string.Equals(x.MandateType, signatureMandateCompanyModel.MandateType, StringComparison.OrdinalIgnoreCase) && x.Id != signatureMandateCompanyModel.Id))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.AuthorizeMandateType;
                    validationError.PropertyName = "MandateType";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Signatory Group-----
        public static ValidationResultModel ValidateSignatoryGroup(SignatoryGroupModel signatoryGroupModel, int applicationId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATORY_GROUP,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            var signatoryGroup = ServiceHelper.GetSignatoryGroup();

            if (string.IsNullOrEmpty(signatoryGroupModel.SignatoryGroupName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.SignatoryGroupName;
                validationError.PropertyName = "SignatoryGroupName";
                lstvalidationError.Add(validationError);
            }
            else
            {
                if (signatoryGroup != null && ServiceHelper.GetName(signatoryGroupModel.SignatoryGroupName, Constants.SIGNATORY_GROUP) != "AUTHORISED PERSONS" && signatoryGroupModel.StatusName != "Complete")
                {
                    List<SignatoryGroupModel> signatoryGroups = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId)?.Where(x => x.StatusName == "Complete").ToList();

                    int lastGroup = 0, currentGroup = 0;
                    currentGroup = (int)(SignatoryGroupType)Enum.Parse(typeof(SignatoryGroupType), String.Concat(ServiceHelper.GetName(signatoryGroupModel.SignatoryGroupName, Constants.SIGNATORY_GROUP).Where(c => !Char.IsWhiteSpace(c))));
                    if (signatoryGroups != null)
                    {
                        foreach (var item in signatoryGroups)
                        {
                            if (item.SignatoryGroup != "AUTHORISED PERSONS")
                                lastGroup = (int)(SignatoryGroupType)Enum.Parse(typeof(SignatoryGroupType), String.Concat(item.SignatoryGroup.Where(c => !Char.IsWhiteSpace(c))));
                        }
                    }
                    if (lastGroup <= currentGroup && (++lastGroup) != currentGroup)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.SignatoryGroupNameSequence;
                        validationError.PropertyName = "SignatoryGroupName";
                        lstvalidationError.Add(validationError);
                    }
                }
                if (signatoryGroup != null && ServiceHelper.GetName(signatoryGroupModel.SignatoryGroupName, Constants.SIGNATORY_GROUP) == "AUTHORISED PERSONS")
                {
                    List<SignatoryGroupModel> signatoryGroups = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId)?.Where(x => string.Equals(x.SignatoryGroup,"AUTHORISED PERSONS",StringComparison.OrdinalIgnoreCase) && x.Id != signatoryGroupModel.Id).ToList();
                    if(signatoryGroups != null)
                    {
                        if(signatoryGroups.Count()>0)
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.SignatoryGroupAuthorizeUnique;
                            validationError.PropertyName = "SignatoryGroupName";
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
                //if (signatoryGroup != null && signatoryGroup.Any(u => string.Equals(u.Value, signatoryGroupModel.SignatoryGroupName, StringComparison.OrdinalIgnoreCase) && string.Equals(u.Text, "AUTHORISED PERSONS", StringComparison.OrdinalIgnoreCase)) && string.IsNullOrEmpty(signatoryGroupModel.SignatureRightsValue))
                //{
                //    ValidationError validationError = new ValidationError();
                //    retVal.IsValid = false;
                //    validationError.ErrorMessage = ValidationConstant.SignatoryGroupSignatureRights;
                //    validationError.PropertyName = "SignatureRightsValue";
                //    lstvalidationError.Add(validationError);
                //}
                if (string.IsNullOrEmpty(signatoryGroupModel.SignatoryPersons) && signatoryGroupModel.SignatoryPersonsList == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.SignatoryPersons;
                    validationError.PropertyName = "SignatoryPersons";
                    lstvalidationError.Add(validationError);
                }
                if (!string.IsNullOrEmpty(signatoryGroupModel.SignatoryPersons) && ServiceHelper.GetName(signatoryGroupModel.SignatoryGroupName, Constants.SIGNATORY_GROUP) != "AUTHORISED PERSONS")
                {
                    List<SignatoryGroupModel> signatoryGroups = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId)?.Where(x => x.StatusName == "Complete" && x.Id != signatoryGroupModel.Id && x.SignatoryGroup != "AUTHORISED PERSONS").ToList();

                    List<string> list1 = new List<string>();
                    if (signatoryGroups != null)
                    { list1.AddRange(from item1 in signatoryGroups from item2 in item1.SignatoryPersons.Split('|').ToList() select item2); }
                    list1 = list1.Where(x => !string.IsNullOrEmpty(x)).Distinct<string>().ToList();
                    List<string> list2 = signatoryGroupModel.SignatoryPersons.Split('|').Where(x => !string.IsNullOrEmpty(x)).Distinct<string>().ToList();
                    if (list1.Any(x => list2.Contains(x)))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.SignatoryPersonAlreadyAdded;
                        validationError.PropertyName = "SignatoryPersons";
                        lstvalidationError.Add(validationError);
                    }
                }
                //if (!string.IsNullOrEmpty(signatoryGroupModel.SignatoryPersons) && ServiceHelper.GetName(signatoryGroupModel.SignatoryGroupName, Constants.SIGNATORY_GROUP) == "AUTHORISED PERSONS")
                //{
                //    List<SignatoryGroupModel> signatoryGroups = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId)?.Where(x => x.StatusName == "Complete" && x.Id != signatoryGroupModel.Id && x.SignatoryGroup == "AUTHORISED PERSONS").ToList();

                //    List<string> list1 = new List<string>();
                //    if (signatoryGroups != null)
                //    { list1.AddRange(from item1 in signatoryGroups from item2 in item1.SignatoryPersons.Split('|').ToList() select item2); }
                //    list1 = list1.Where(x => !string.IsNullOrEmpty(x)).Distinct<string>().ToList();
                //    List<string> list2 = signatoryGroupModel.SignatoryPersons.Split('|').Where(x => !string.IsNullOrEmpty(x)).Distinct<string>().ToList();
                //    if (list1.Any(x => list2.Contains(x)))
                //    {
                //        ValidationError validationError = new ValidationError();
                //        retVal.IsValid = false;
                //        validationError.ErrorMessage = ValidationConstant.SignatoryPersonAlreadyAddedAuth;
                //        validationError.PropertyName = "SignatoryPersons";
                //        lstvalidationError.Add(validationError);
                //    }
                //}
            }
            //if (signatoryGroupModel.StartDate == DateTime.MinValue || signatoryGroupModel.StartDate == null)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.StartDate;
            //    validationError.PropertyName = "StartDate";
            //    lstvalidationError.Add(validationError);
            //}
            //if (signatoryGroupModel.StartDate != null)
            //{
            //    if (DateTime.Today< signatoryGroupModel.StartDate)
            //    {
            //        ValidationError validationError = new ValidationError();
            //        retVal.IsValid = false;
            //        validationError.ErrorMessage = ValidationConstant.StartDateValidate;
            //        validationError.PropertyName = "StartDate";
            //        lstvalidationError.Add(validationError);
            //    }
            //}
            //if (signatoryGroupModel.EndDate == DateTime.MinValue || signatoryGroupModel.EndDate == null)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.EndDate;
            //    validationError.PropertyName = "EndDate";
            //    lstvalidationError.Add(validationError);
            //}
            //if (signatoryGroupModel.EndDate < signatoryGroupModel.StartDate)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.EndDateValidate;
            //    validationError.PropertyName = "EndDate";
            //    lstvalidationError.Add(validationError);
            //}



            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateDeleteSignatoryGroup(string SignatoryGroupName, int applicationId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATORY_GROUP,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            var signatoryGroup = ServiceHelper.GetSignatoryGroup();
            if (signatoryGroup != null && SignatoryGroupName.ToUpper() != "AUTHORISED PERSONS")// && signatoryGroupModel.StatusName != "Complete")
            {
                List<SignatoryGroupModel> signatoryGroups = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId)?.Where(x => x.StatusName == "Complete").ToList();

                int lastGroup = 0, currentGroup = 0; List<int> grouplist = new List<int>();
                currentGroup = (int)(SignatoryGroupType)Enum.Parse(typeof(SignatoryGroupType), String.Concat(SignatoryGroupName.Where(c => !Char.IsWhiteSpace(c))));
                if (signatoryGroups != null)
                {
                    foreach (var item in signatoryGroups)
                    {
                        if (item.SignatoryGroup != "AUTHORISED PERSONS")
                        {
                            lastGroup = (int)(SignatoryGroupType)Enum.Parse(typeof(SignatoryGroupType), String.Concat(item.SignatoryGroup.Where(c => !Char.IsWhiteSpace(c))));
                            grouplist.Add(lastGroup);
                        }
                    }
                }
                int max = grouplist.DefaultIfEmpty(0).Max();
                if (max > currentGroup)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.SignatoryGroupNameDeleteSequence;
                    //validationError.PropertyName = "SignatoryGroupName";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }


        #endregion

        #region----Note Details----
        public static ValidationResultModel ValidateNoteDetails(NoteDetailsModel noteDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.APPLICATION_NOTE_DETAILS,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (string.IsNullOrEmpty(noteDetailsModel.NoteDetailsType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.NoteDetailsType;
                validationError.PropertyName = "NoteDetailsType";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(noteDetailsModel.Subject))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Subject;
                validationError.PropertyName = "Subject";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(noteDetailsModel.Details))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Details;
                validationError.PropertyName = "Details";
                lstvalidationError.Add(validationError);
            }
            //if (string.IsNullOrEmpty(noteDetailsModel.PendingOn))
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.PendingOn;
            //    validationError.PropertyName = "PendingOn";
            //    lstvalidationError.Add(validationError);
            //}
            //if (noteDetailsModel.ExpectedDate==null)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.ExpectedDate;
            //    validationError.PropertyName = "ExpectedDate";
            //    lstvalidationError.Add(validationError);
            //}
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Bank  Document----
        public static ValidationResultModel ValidateBankDocument(DocumentsViewModel documentsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.APPLICATION_BANK_DOCUMENTS,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (string.IsNullOrEmpty(documentsViewModel.Entity))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Entity;
                validationError.PropertyName = "Entity";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(documentsViewModel.EntityType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.EntityType;
                validationError.PropertyName = "EntityType";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(documentsViewModel.DocumentType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.DocumentType;
                validationError.PropertyName = "DocumentType";
                lstvalidationError.Add(validationError);
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Expected  Document----
        public static ValidationResultModel ValidateExpectedDocument(DocumentsViewModel documentsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.APPLICATION_EXPECTED_DOCUMENTS,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (string.IsNullOrEmpty(documentsViewModel.Entity))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Entity;
                validationError.PropertyName = "Entity";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(documentsViewModel.EntityType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.EntityType;
                validationError.PropertyName = "EntityType";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(documentsViewModel.DocumentType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.DocumentType;
                validationError.PropertyName = "DocumentType";
                lstvalidationError.Add(validationError);
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Purpose And Activity-----
        public static ValidationResultModel ValidateDecision(DecisionHistoryViewModel decisionHistoryViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.DECISION_HISTORY,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(decisionHistoryViewModel.DecisionHistory_Decision))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Decision_History;
                validationError.PropertyName = "DecisionHistory_Decision";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Signature Mandate Individual-----
        public static ValidationResultModel ValidateSignatureMandateIndividual(SignatureMandateIndividualModel signatureMandateIndividualModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.SIGNATURE_MANDATE,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            //if (signatureMandateIndividualModel.AmountFrom == null)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.AmountFrom;
            //    validationError.PropertyName = "AmountFrom";
            //    lstvalidationError.Add(validationError);
            //}
            //if (signatureMandateIndividualModel.AmountTo == null)
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.AmountTo;
            //    validationError.PropertyName = "AmountTo";
            //    lstvalidationError.Add(validationError);
            //}
            if (signatureMandateIndividualModel.NumberOfSignatures == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.NumberOfSignatures;
                validationError.PropertyName = "NumberOfSignatures";
                lstvalidationError.Add(validationError);
            }
            if (signatureMandateIndividualModel.AccessRights == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AccessRights;
                validationError.PropertyName = "AccessRights";
                lstvalidationError.Add(validationError);
            }
            if (signatureMandateIndividualModel.SignatoryPersonsList == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.SignatoryPersonsList;
                validationError.PropertyName = "SignatoryPersons";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion
    }
}
