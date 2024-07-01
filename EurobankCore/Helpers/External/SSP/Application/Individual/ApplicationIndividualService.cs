using CMS.DocumentEngine.Types.Eurobank;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.Reporting;
using CMS.SiteProvider;
//using CodeBeautify;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.XMLServiceModel.Individual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Eurobank.Helpers.External.SSP.Application.Individual
{
    public class ApplicationIndividualService
    {
        public static string GetApplicationXMLString(int applicationId)
        {
            string retVal = string.Empty;

            ApplicationIndividualXMLModel applicationIndividualXMLModel = new ApplicationIndividualXMLModel();
            applicationIndividualXMLModel.ApplictionXMLModel = GetApplicationRoot(applicationId);

            retVal = IndividualApplicationToXml(applicationIndividualXMLModel.ApplictionXMLModel);
            return retVal;
        }
        public static string IndividualApplicationToXml(Root model)
        {
            XmlSerializer xmlSerializer = new XmlSerializer
        (typeof(Root));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, model);
                return textWriter.ToString();
            }
        }
        public static Root GetApplicationRoot(int applicationId)
        {
            Root retVal = new Root();
            ApplicationModel application = ApplicationsProcess.GetApplicationModelByApplicationIdExtented(applicationId);
            GetApplicationXML(application);
            GetApplicationServiceXML(application);
            //retVal = BindApplicationRootXML(application);
            return retVal;
        }
        public static string GetApplicationIndividualElement(int applicationId)
        {
            string result = string.Empty;
            ApplicationModel application = ApplicationsProcess.GetApplicationModelByApplicationIdExtented(applicationId);

            StringBuilder writer = new StringBuilder();
            writer.Append("<root>");

            string applicationDetails = GetApplicationXML(application);
            writer.Append(applicationDetails);

            string applicant = GetApplicantXML(application);
            writer.Append(applicant);

            string eBankingSubscriberDetails = GetEBankingSubscriberDetailsXML(application);
            writer.Append(eBankingSubscriberDetails);
            string accountDetails = GetAccountDetailsXML(application);
            writer.Append(accountDetails);
            string EBankingSignatureMandate = GetEBankingSignatureMandateXML(application);
            writer.Append(EBankingSignatureMandate);
            string signatoryGroup = GetSignatoryGroupXML(application);
            writer.Append(signatoryGroup);
            string signatureMandate = GetSignatureMandateXML(application);
            writer.Append(signatureMandate);

            string relatedParty = GetRealtedPartyXML(application);
            writer.Append(relatedParty);

            writer.Append("</root>");
            result = writer.ToString();
            return result;
        }
        #region----APPLICATION------
        public static string GetApplicationXML(ApplicationModel application)
        {
            string result = string.Empty;
            var responsibleOffice = ServiceHelper.GetResponsibleOfficer();
            string responsibleofficeName = (responsibleOffice != null && responsibleOffice.Count > 0 && application.ApplicationDetails.ApplicationDetails_ResponsibleOfficer != null && responsibleOffice.Any(f => f.Value == application.ApplicationDetails.ApplicationDetails_ResponsibleOfficer.ToString())) ? responsibleOffice.FirstOrDefault(f => f.Value == application.ApplicationDetails.ApplicationDetails_ResponsibleOfficer.ToString()).Text : string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/Application";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.ApplicationDetails != null)
                    {
                        string applictionTypeName = string.Empty;
                        if (string.Equals(application.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                        {
                            applictionTypeName = "JOINT";
                        }
                        else
                        {
                            applictionTypeName = application.ApplicationDetails.ApplicationDetails_ApplicationTypeName;
                        }
                        string applicationstatusname = ServiceHelper.GetName(ValidationHelper.GetString(application.ApplicationDetails.ApplicationDetails_ApplicationStatus, ""), Constants.APPLICATION_STATUS);
                        writer.WriteStartElement(GetLookupCode("Application", path));
                        writer.WriteStartElement(GetXmlElementName("General", path));
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationNumber", path), application.ApplicationDetails.ApplicationDetails_ApplicationNumber.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationStatusName", path), applicationstatusname.Trim().ToUpper());
                        //writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationStatusName", path)+"_code", GetLookUpItemCode(applicationstatusname, Constants.APPLICATION_STATUS)?.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationStatusName", path) + "_code", "1");
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_SubmittedBy", path), application.ApplicationDetails.ApplicationDetails_SubmittedBy.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_SubmittedOn", path), Convert.ToDateTime(application.ApplicationDetails.ApplicationDetails_SubmittedOn).ToString("yyyy-MM-dd HH:mm:ss"));
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ResponsibleOfficer", path), responsibleofficeName.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationTypeName", path), applictionTypeName.Trim().ToUpper());
                        if (application.ApplicationDetails != null && application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup != null)
                        {
                            writer.WriteStartElement(GetLookupCode("Application Service", path));
                            foreach (var item in application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items)
                            {
                                var selectedValue = application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue;
                                if (selectedValue != null)
                                {
                                    if (selectedValue.Contains(item.Value))
                                    {
                                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicatonServices", path), item.Label.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicatonServices", path) + "_code", GetLookUpItemCode(item.Label, Constants.APPLICATION_SERVICES)?.Trim().ToUpper());
                                    }
                                }
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicationServiceXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/Application-Service";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.ApplicationDetails != null && application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup != null)
                    {
                        writer.WriteStartElement(GetLookupCode("Application Service", path));
                        foreach (var item in application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items)
                        {
                            var selectedValue = application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue;
                            if (selectedValue != null)
                            {
                                if (selectedValue.Contains(item.Value))
                                {
                                    writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicatonServices", path), item.Label.Trim().ToUpper());
                                }
                            }
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetPurposeAndActivityXML(ApplicationModel application)
        {
            string result = string.Empty;

            string path = "/XML-Field-Lookups/COMMON/Purpose-and-Activity";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.ApplicationDetails != null)
                    {
                        writer.WriteStartElement(GetLookupCode("Purpose and Activity", path));
                        if (application.PurposeAndActivity != null)
                        {
                            writer.WriteStartElement(GetXmlElementName("ReasonsForOpeningTheAccount", path));
                            foreach (var reason in application.PurposeAndActivity.ReasonForOpeningTheAccountGroup.Items)
                            {
                                var selectedValue = application.PurposeAndActivity.ReasonForOpeningTheAccountGroup.MultiSelectValue;
                                if (selectedValue != null)
                                {
                                    if (selectedValue.Contains(reason.Value))
                                    {
                                        writer.WriteElementString(GetXmlElementName("ReasonsForOpeningTheAccount", path), reason.Text.Trim().ToUpper());
                                    }
                                }
                            }
                            writer.WriteEndElement();
                            writer.WriteStartElement(GetXmlElementName("ExpectedNaturesOfIncomingAndOutgoingTransactions", path));
                            foreach (var nature in application.PurposeAndActivity.ExpectedNatureOfInAndOutTransactionGroup.Items)
                            {
                                var selectedValue = application.PurposeAndActivity.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue;
                                if (selectedValue != null)
                                {
                                    if (selectedValue.Contains(nature.Value))
                                    {
                                        writer.WriteElementString(GetXmlElementName("ExpectedNaturesOfIncomingAndOutgoingTransactions", path), nature.Text.Trim().ToUpper());
                                    }
                                }
                            }
                            writer.WriteEndElement();
                            writer.WriteStartElement(GetXmlElementName("ExpectedFrequencyOfIncomingAndOutgoingTransactions", path));
                            foreach (var frequency in application.PurposeAndActivity.ExpectedFrequencyOfInAndOutTransactionGroup.Items)
                            {
                                var selectedValue = application.PurposeAndActivity.ExpectedFrequencyOfInAndOutTransactionGroup.RadioGroupValue;
                                if (selectedValue != null)
                                {
                                    if (selectedValue.Contains(frequency.Value))
                                    {
                                        writer.WriteElementString(GetXmlElementName("ExpectedFrequencyOfIncomingAndOutgoingTransactions", path), frequency.Label.Trim().ToUpper());
                                    }
                                }
                            }
                            writer.WriteEndElement();
                            writer.WriteElementString(GetXmlElementName("ExpectedIncomingAmount", path), application.PurposeAndActivity.ExpectedIncomingAmount.ToString().Trim().ToUpper());
                        }

                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetAccountDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/Account-Details";
            string accountType = GetXmlElementName("Accounts_AccountType", path);
            string accountCurrency = GetXmlElementName("Accounts_Currency", path);
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    writer.WriteStartElement(GetLookupCode("Account Details", path));
                    if (application.Accounts != null)
                    {
                        foreach (var item in application.Accounts)
                        {
                            string accountTypeText = ServiceHelper.GetName(ValidationHelper.GetString(item.Accounts_AccountType, ""), Constants.Accounts_AccountType);
                            writer.WriteStartElement("account");
                            writer.WriteElementString(accountType, accountTypeText.Trim().ToUpper());
                            //writer.WriteElementString(GetXmlElementName("Accounts_AccountType", path) + "_code", GetLookUpItemCode(accountTypeText, Constants.Accounts_AccountType)?.Trim().ToUpper());
                            if (string.Equals(application.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                            {
                                writer.WriteElementString(accountType + "_code", "CURR.ACCT.NO.LIMIT.JOINT");
                            }
                            else
                            {
                                writer.WriteElementString(accountType + "_code", GetLookUpItemCode(accountTypeText, Constants.Accounts_AccountType)?.Trim().ToUpper());
                            }
                            writer.WriteElementString(accountCurrency, item.Accounts_CurrencyName.Trim().ToUpper());
                            writer.WriteElementString(accountCurrency + "_code", GetLookUpItemCode(item.Accounts_CurrencyName, Constants.Accounts_Currency)?.Trim().ToUpper());
                            //writer.WriteElementString(GetXmlElementName("Accounts_StatementFrequency", path), item.Accounts_StatementFrequencyName);
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetDebitCardDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Card-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.DebitCards != null)
                    {
                        writer.WriteStartElement("carddetails");
                        foreach (var card in application.DebitCards)
                        {
                            writer.WriteStartElement(GetLookupCode("Card Details", path));
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardType", path), card.DebitCardDetails_CardTypeName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("AssociatedAccount", path), card.AssociatedAccountName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardholderName", path), card.DebitCardDetails_CardholderName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_FullName", path), card.DebitCardDetails_FullName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Country_Code", path), card.Country_Code.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_MobileNumber", path), card.DebitCardDetails_MobileNumber.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_DispatchMethod", path), card.DebitCardDetails_DispatchMethodName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_DeliveryAddress", path), card.DebitCardDetails_DeliveryAddress.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_CollectedBy", path), card.DebitCardDetails_CollectedBy.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_IdentityNumber", path), card.DebitCardDetails_IdentityNumber.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_DeliveryDetails", path), card.DebitCardDetails_DeliveryDetails.Trim().ToUpper());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }

                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetEBankingSubscriberDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/EBanking-Subscriber";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.EBankingSubscribers != null)
                    {
                        string eBankingFlag = "NO";
                        if (application.ApplicationDetails != null && application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup != null)
                        {

                            foreach (var item in application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items)
                            {
                                var selectedValue = application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue;
                                string eBankingValue = application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.Where(x => x.Label == "DIGITAL BANKING").Select(x => x.Value).FirstOrDefault();
                                if (selectedValue != null)
                                {
                                    if (selectedValue.Contains(eBankingValue))
                                    {
                                        eBankingFlag = "YES";
                                    }
                                }
                            }
                        }

                        writer.WriteStartElement("ebankingdetails");
                        writer.WriteElementString(GetXmlElementName("EbankingFlag", path), eBankingFlag.Trim().ToUpper());

                        string ebankingSubscriber = GetLookupCode("EBanking Subscriber", path);
                        string partyReference = GetXmlElementName("Partyreference", path);
                        string subscriberName = GetXmlElementName("SubscriberName", path);
                        string accessLevel = GetXmlElementName("AccessLevel", path);
                        string accessToAllPersonalAccounts = GetXmlElementName("AccessToAllPersonalAccounts", path);
                        string automaticallyAddFuturePersonalAccounts = GetXmlElementName("AutomaticallyAddFuturePersonalAccounts", path);
                        string limitAmount = GetXmlElementName("LimitAmount", path);
                        foreach (var item in application.EBankingSubscribers)
                        {
                            writer.WriteStartElement(ebankingSubscriber);
                            writer.WriteElementString(partyReference, item.PartyReferenceId.ToString());
                            writer.WriteElementString(subscriberName, item.SubscriberName.Trim().ToUpper());
                            writer.WriteElementString(accessLevel, item.AccessLevelName.Trim().ToUpper());
                            writer.WriteElementString(accessLevel + "_code", GetLookUpItemCode(item.AccessLevelName, Constants.Access_Level_Individual)?.Trim().ToUpper());
                            writer.WriteElementString(accessToAllPersonalAccounts, item.AccessToAllPersonalAccounts == true ? "YES" : "NO");
                            writer.WriteElementString(automaticallyAddFuturePersonalAccounts, item.AutomaticallyAddFuturePersonalAccounts == true ? "YES" : "NO");
                            writer.WriteElementString(limitAmount, item.LimitAmountName);
                            writer.WriteElementString(limitAmount + "_code", GetLookUpItemCode(item.LimitAmountName, Constants.LIMIT_AMOUNT)?.Trim().ToUpper());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetSourceOfIncomingTransactionXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Source-Of-Incoming-Transaction";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.SourceOfIncomingTransactions != null)
                    {
                        writer.WriteStartElement(GetLookupCode("Source Of Incoming Transaction", path));
                        foreach (var item in application.SourceOfIncomingTransactions)
                        {
                            writer.WriteStartElement(GetLookupCode("Source Of Incoming Transaction", path));
                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_NameOfRemitter", path), item.SourceOfIncomingTransactions_NameOfRemitter.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitter", path), item.SourceOfIncomingTransactions_CountryOfRemitter.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitterBank", path), item.SourceOfIncomingTransactions_CountryOfRemitterBank.Trim().ToUpper());
                            writer.WriteEndElement();

                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }

                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetSourceOfOutgoingTransactionXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Source-Of-Outgoing-Transaction";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.SourceOfOutgoingTransactions != null)
                    {
                        writer.WriteStartElement(GetLookupCode("Source Of Outgoing Transaction", path));
                        foreach (var item in application.SourceOfOutgoingTransactions)
                        {
                            writer.WriteStartElement(GetLookupCode("Source Of Outgoing Transaction", path));
                            writer.WriteElementString(GetXmlElementName("NameOfBeneficiary", path), item.NameOfBeneficiary.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiary", path), item.CountryOfBeneficiary.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiaryBank", path), item.CountryOfBeneficiaryBank.Trim().ToUpper());
                            writer.WriteEndElement();

                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }

                }
                result = textWriter.ToString();
            }
            return result;
        }

        public static string GetEBankingSignatureMandateXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/EBankingSignatureMandate";
            string ebankingSignatureMandate = GetLookupCode("EBankingSignatureMandate", path);
            string partyReference = GetXmlElementName("PartyReference", path);
            string mandateType = GetXmlElementName("MandateType", path);
            string description = GetXmlElementName("Description", path);
            string limitFrom = GetXmlElementName("LimitFrom", path);
            string limitTo = GetXmlElementName("LimitTo", path);
            string totalSignatures = GetXmlElementName("TotalSignatures", path);
            string signatorySet = GetXmlElementName("SignatorySet", path);
            string signatoryGroups = GetXmlElementName("SignatoryGroups", path);
            string signatoryGroup = GetXmlElementName("SignatoryGroup", path);
            string minnumSignatures = GetXmlElementName("MinnumSignatures", path);
            string rights = GetXmlElementName("Rights", path);

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            var authorisedGroup = SignatoryGroupProcess.GetDDLSignatoryGroupsByApplicationId(application.ApplicationDetails.ApplicationDetailsID);
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.EBankingSubscribers != null)
                    {
                        writer.WriteStartElement("ebankingsignaturemandates");
                        foreach (var item in application.EBankingSubscribers)
                        {
                            writer.WriteStartElement(ebankingSignatureMandate);

                            writer.WriteElementString(partyReference, string.Empty);
                            writer.WriteElementString(mandateType, "FOR OPERATIONS AND EBANKING");
                            writer.WriteElementString(mandateType + "_code", GetLookUpItemCode("FOR OPERATIONS AND EBANKING", Constants.MANDATE_TYPE)?.Trim().ToUpper());
                            writer.WriteElementString(description, "FOR ALL ACCOUNTS");
                            writer.WriteElementString(limitFrom, "0.00");
                            writer.WriteElementString(limitTo, "999999999999.00");
                            writer.WriteElementString(totalSignatures, "1");
                            writer.WriteStartElement("signatorysets");
                            writer.WriteStartElement(signatorySet);

                            writer.WriteStartElement(signatoryGroups);
                            writer.WriteElementString(signatoryGroup, "GROUP A");
                            writer.WriteElementString(signatoryGroup + "_code", GetLookUpItemCode("GROUP A", Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
                            writer.WriteElementString(minnumSignatures, "1");
                            writer.WriteElementString(rights, "");
                            writer.WriteElementString(rights + "_code", "");
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetSignatoryGroupXML(ApplicationModel application)
        {
            string result = string.Empty;
            string signatoryGroupPath = "/XML-Field-Lookups/INDIVIDUAL/Signatory-Groups";

            var signtoryPerson = CommonProcess.GetSignatoryPersonIndividual(application.ApplicationDetails.ApplicationDetails_ApplicationNumber);
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.PurposeAndActivity.SignatureMandateTypeGroup != null)
                    {
                        string selectedOptionValue = application.PurposeAndActivity.SignatureMandateTypeGroup.RadioGroupValue;
                        string selectedItemName = application.PurposeAndActivity.SignatureMandateTypeGroup.Items.FirstOrDefault(x => x.Value == selectedOptionValue).Label;
                        if (string.Equals(selectedItemName, "ANY ONE ALONE CAN SIGN", StringComparison.OrdinalIgnoreCase) || string.Equals(selectedItemName, "ALL JOINTLY", StringComparison.OrdinalIgnoreCase))
                        {
                            writer.WriteStartElement(GetLookupCode("Signatory Groups", signatoryGroupPath));
                            writer.WriteStartElement(GetXmlElementName("SignatoryGroup", signatoryGroupPath));
                            writer.WriteElementString(GetXmlElementName("SignatoryGroupName", signatoryGroupPath), "GROUPA");
                            writer.WriteElementString(GetXmlElementName("SignatoryGroupName", signatoryGroupPath) + "_code", GetLookUpItemCode("GROUP A", Constants.SIGNATORY_GROUP)?.Trim().ToUpper());


                            string signatoryPersons = GetXmlElementName("SignatoryPersons", signatoryGroupPath);
                            string partyReference = GetXmlElementName("PartyReference", signatoryGroupPath);
                            string signatoryPerson = GetXmlElementName("SignatoryPerson", signatoryGroupPath);
                            string startDate = GetXmlElementName("StartDate", signatoryGroupPath);
                            string endDate = GetXmlElementName("EndDate", signatoryGroupPath);
                            if(application.Applicants != null)
                            {
								foreach (var item in application.Applicants)
								{
									writer.WriteStartElement(signatoryPersons);
									writer.WriteElementString(partyReference, PersonalDetailsProcess.GetPersonDetailsIdByGuid(item.PersonalDetails.NodeGUID).ToString()); //item.Id.ToString()
									writer.WriteElementString(signatoryPerson, item.FullName.Trim().ToUpper());
									writer.WriteElementString(startDate, DateTime.Now.ToString("yyyy-MM-dd"));
									writer.WriteElementString(endDate, "2099-01-01");
									writer.WriteEndElement();
								}
							}

                            writer.WriteElementString(GetXmlElementName("Description", signatoryGroupPath), selectedItemName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Description", signatoryGroupPath) + "_code", GetLookUpItemCode(selectedItemName, Constants.SIGNATURE_MANDATE_TYPE)?.Trim().ToUpper());
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        if (string.Equals(selectedItemName, "OTHER", StringComparison.OrdinalIgnoreCase))
                        {
                            writer.WriteStartElement(GetLookupCode("Signatory Groups", signatoryGroupPath));
                            if (application.SignatureMandates != null)
                            {
                                char groupChar = 'A';
                                string signatoryGroup = GetXmlElementName("SignatoryGroup", signatoryGroupPath);
                                string signatorygroupName = GetXmlElementName("SignatoryGroupName", signatoryGroupPath);
                                string signatoryPersons = GetXmlElementName("SignatoryPersons", signatoryGroupPath);
                                string partyReference = GetXmlElementName("PartyReference", signatoryGroupPath);
                                string signatoryPerson = GetXmlElementName("SignatoryPerson", signatoryGroupPath);
                                string startDate = GetXmlElementName("StartDate", signatoryGroupPath);
                                string endDate = GetXmlElementName("EndDate", signatoryGroupPath);
                                string description = GetXmlElementName("Description", signatoryGroupPath);
                                foreach (var item in application.SignatureMandates)
                                {

                                    writer.WriteStartElement(signatoryGroup);
                                    writer.WriteElementString(signatorygroupName, "GROUP" + groupChar);
                                    writer.WriteElementString(signatorygroupName + "_code", GetLookUpItemCode("GROUP " + groupChar, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
                                    if (item.SignatoryPersonsList != null)
                                    {
                                        foreach (var person in item.SignatoryPersonsList)
                                        {
                                            if (signtoryPerson.Any(x => x.Value == person))
                                            {
                                                writer.WriteStartElement(signatoryPersons);
                                                writer.WriteElementString(partyReference, PersonalDetailsProcess.GetPersonDetailsIdByGuid(signtoryPerson.FirstOrDefault(x => x.Value == person).Value).ToString());
                                                writer.WriteElementString(signatoryPerson, signtoryPerson.FirstOrDefault(x => x.Value == person).Text.Trim().ToUpper());
                                                writer.WriteElementString(startDate, DateTime.Now.ToString("yyyy-MM-dd"));
                                                writer.WriteElementString(endDate, "2099-01-01");
                                                writer.WriteEndElement();
                                            }
                                        }
                                    }
                                    writer.WriteElementString(description, item.AccessRightsName.Trim().ToUpper());
                                    writer.WriteElementString(description + "_code", GetLookUpItemCode(item.AccessRightsName, Constants.Access_Right)?.Trim().ToUpper());
                                    writer.WriteEndElement();
                                    groupChar++;
                                }
                            }
                            writer.WriteEndElement();
                        }

                        //if (string.Equals(selectedItemName, "OTHER", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    writer.WriteStartElement(GetLookupCode("Signatory Groups", signatoryGroupPath));
                        //    writer.WriteStartElement(GetXmlElementName("SignatoryGroup", signatoryGroupPath));
                        //    writer.WriteElementString(GetXmlElementName("SignatoryGroupName", signatoryGroupPath), "GROUP " + groupChar);
                        //    if (item.SignatoryPersonsList != null)
                        //    {
                        //        foreach (var person in item.SignatoryPersonsList)
                        //        {
                        //            writer.WriteStartElement(GetXmlElementName("SignatoryPersons", signatoryGroupPath));
                        //            writer.WriteElementString(GetXmlElementName("PartyReference", signatoryGroupPath), item.Id.ToString());
                        //            writer.WriteElementString(GetXmlElementName("SignatoryPerson", signatoryGroupPath), signtoryPerson.FirstOrDefault(x => x.Value == person).Text);
                        //            writer.WriteEndElement();
                        //        }
                        //    }
                        //    writer.WriteElementString(GetXmlElementName("StartDate", signatoryGroupPath), DateTime.Now.ToString("yyyy-MM-dd"));
                        //    writer.WriteElementString(GetXmlElementName("EndDate", signatoryGroupPath), "2099-01-01");
                        //    writer.WriteElementString(GetXmlElementName("Description", signatoryGroupPath), item.AccessRightsName);
                        //    writer.WriteEndElement();
                        //    writer.WriteEndElement();
                        //}

                        //    }
                        //}
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }


        public static string GetSignatureMandateXML(ApplicationModel application)
        {
            string result = string.Empty;
            string signatureMandatePath = "/XML-Field-Lookups/INDIVIDUAL/SignatureMandates";
            var signtoryPerson = CommonProcess.GetSignatoryPersonIndividual(application.ApplicationDetails.ApplicationDetails_ApplicationNumber);

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    string selectedOptionValue = application.PurposeAndActivity.SignatureMandateTypeGroup.RadioGroupValue;
                    string selectedItemName = application.PurposeAndActivity.SignatureMandateTypeGroup.Items.FirstOrDefault(x => x.Value == selectedOptionValue).Label;

                    if (string.Equals(application.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                    {
                        string signatureMandates = GetLookupCode("SignatureMandates", signatureMandatePath);
                        string mandateType = GetXmlElementName("MandateType", signatureMandatePath);
                        string description = GetXmlElementName("Description", signatureMandatePath);
                        string signatureMandate = GetXmlElementName("SignatureMandate", signatureMandatePath);
                        string limitForm = GetXmlElementName("LimitFrom", signatureMandatePath);
                        string limitTo = GetXmlElementName("LimitTo", signatureMandatePath);
                        string totalSignature = GetXmlElementName("TotalSignatures", signatureMandatePath);
                        string signatorySet = GetXmlElementName("SignatorySet", signatureMandatePath);
                        string signatoryGroups = GetXmlElementName("SignatoryGroups", signatureMandatePath);
                        string signatoryGroup = GetXmlElementName("SignatoryGroup", signatureMandatePath);
                        string minimumSignature = GetXmlElementName("MinnumSignatures", signatureMandatePath);
                        string rights = GetXmlElementName("Rights", signatureMandatePath);
                        if(application.Applicants != null)
                        {
							foreach (var item in application.Applicants)
							{
								if (string.Equals(selectedItemName, "ANY ONE ALONE CAN SIGN", StringComparison.OrdinalIgnoreCase))
								{
									writer.WriteStartElement(signatureMandates);
									writer.WriteElementString(mandateType, "OPERATION OF ACCOUNTS");
									writer.WriteElementString(mandateType + "_code", GetLookUpItemCode("FOR OPERATION OF ACCOUNTS", Constants.MANDATE_TYPE)?.Trim().ToUpper());
									writer.WriteElementString(description, "FOR ALL ACCOUNTS");
									writer.WriteStartElement(signatureMandate);


									writer.WriteElementString(limitForm, "0");
									writer.WriteElementString(limitTo, "99999999");
									writer.WriteElementString(totalSignature, application.Applicants.Count().ToString());


									writer.WriteStartElement(signatorySet);
									writer.WriteStartElement(signatoryGroups);
									writer.WriteElementString(signatoryGroup, "GROUP A");
									writer.WriteElementString(signatoryGroup + "_code", GetLookUpItemCode("GROUP A", Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
									writer.WriteElementString(minimumSignature, application.Applicants.Count().ToString());
									writer.WriteElementString(rights, "AND");
									writer.WriteElementString(rights + "_code", GetLookUpItemCode("AND", Constants.SIGNATURE_RIGHTS)?.Trim().ToUpper());

									writer.WriteEndElement();
									writer.WriteEndElement();
									writer.WriteEndElement();
									writer.WriteEndElement();
								}
							}
						}
                        
                    }
                    else if (string.Equals(application.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(selectedItemName, "ANY ONE ALONE CAN SIGN", StringComparison.OrdinalIgnoreCase) || string.Equals(selectedItemName, "ALL JOINTLY", StringComparison.OrdinalIgnoreCase))
                        {
                            writer.WriteStartElement(GetLookupCode("SignatureMandates", signatureMandatePath));
                            writer.WriteElementString(GetXmlElementName("MandateType", signatureMandatePath), "OPERATION OF ACCOUNTS");
                            writer.WriteElementString(GetXmlElementName("MandateType", signatureMandatePath) + "_code", GetLookUpItemCode("FOR OPERATION OF ACCOUNTS", Constants.MANDATE_TYPE)?.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Description", signatureMandatePath), "FOR ALL ACCOUNTS"); 
                            writer.WriteStartElement(GetXmlElementName("SignatureMandate", signatureMandatePath));


                            writer.WriteElementString(GetXmlElementName("LimitFrom", signatureMandatePath), "0");
                            writer.WriteElementString(GetXmlElementName("LimitTo", signatureMandatePath), "99999999");
                            writer.WriteElementString(GetXmlElementName("TotalSignatures", signatureMandatePath), selectedItemName.Trim().ToUpper() == "ANY ONE ALONE CAN SIGN" ? "1" : application.Applicants.Count().ToString());

                            
                            writer.WriteStartElement(GetXmlElementName("SignatorySet", signatureMandatePath));
                            writer.WriteStartElement(GetXmlElementName("SignatoryGroups", signatureMandatePath));
                            writer.WriteElementString(GetXmlElementName("SignatoryGroup", signatureMandatePath), "GROUP A");
                            writer.WriteElementString(GetXmlElementName("MinnumSignatures", signatureMandatePath), selectedItemName.Trim().ToUpper() == "ANY ONE ALONE CAN SIGN" ? "1" : application.Applicants.Count().ToString());
                            writer.WriteElementString(GetXmlElementName("Rights", signatureMandatePath), selectedItemName.Trim().ToUpper() == "ANY ONE ALONE CAN SIGN" ? "OR" : "AND");
                            writer.WriteElementString(GetXmlElementName("Rights", signatureMandatePath) + "_code", GetLookUpItemCode(selectedItemName.Trim().ToUpper() == "ANY ONE ALONE CAN SIGN" ? "OR" : "AND", Constants.SIGNATURE_RIGHTS)?.Trim().ToUpper());
                            writer.WriteEndElement();
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        else if (string.Equals(selectedItemName, "OTHER", StringComparison.OrdinalIgnoreCase))
                        {
                            writer.WriteStartElement(GetLookupCode("SignatureMandates", signatureMandatePath));
                            writer.WriteElementString(GetXmlElementName("MandateType", signatureMandatePath), "OPERATION OF ACCOUNTS");
                            writer.WriteElementString(GetXmlElementName("MandateType", signatureMandatePath) + "_code", GetLookUpItemCode("FOR OPERATION OF ACCOUNTS", Constants.MANDATE_TYPE)?.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Description", signatureMandatePath), "FOR ALL ACCOUNTS"); 

                            char groupChar = 'A';
                            string signatureMandate = GetXmlElementName("SignatureMandate", signatureMandatePath);
                            string limitFrom = GetXmlElementName("LimitFrom", signatureMandatePath);
                            string limitTo = GetXmlElementName("LimitTo", signatureMandatePath);
                            string totalSignature = GetXmlElementName("TotalSignatures", signatureMandatePath);
                            string signatorySet = GetXmlElementName("SignatorySet", signatureMandatePath);
                            string signatoryGroups = GetXmlElementName("SignatoryGroups", signatureMandatePath);
                            string signatoryGroup = GetXmlElementName("SignatoryGroup", signatureMandatePath);
                            string minimumSignature = GetXmlElementName("MinnumSignatures", signatureMandatePath);
                            string rights = GetXmlElementName("Rights", signatureMandatePath);
                            
                            foreach (var item in application.SignatureMandates)
                            {
                                writer.WriteStartElement(signatureMandate);
                                writer.WriteElementString(limitFrom, "0");
                                writer.WriteElementString(limitTo, "99999999");
                                writer.WriteElementString(totalSignature, item.AccessRightsName.Trim().ToUpper() == "ACTING JOINTLY" ? item.SignatoryPersonsList.Count().ToString() : "1");

                                writer.WriteStartElement(signatorySet);
                                writer.WriteStartElement(signatoryGroups);
                                writer.WriteElementString(signatoryGroup, "GROUP" + groupChar);
                                writer.WriteElementString(minimumSignature, item.AccessRightsName.Trim().ToUpper() == "ACTING JOINTLY" ? item.SignatoryPersonsList.Count().ToString() : "1"); //application.Applicants.Count().ToString()
                                writer.WriteElementString(rights, item.AccessRightsName.Trim().ToUpper() == "ACTING JOINTLY" ? "AND" : "OR");
                                writer.WriteElementString(rights + "_code", GetLookUpItemCode(item.AccessRightsName.Trim().ToUpper() == "ACTING JOINTLY" ? "AND" : "OR", Constants.SIGNATURE_RIGHTS)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.WriteEndElement();

                                writer.WriteEndElement();
                                groupChar++;
                            }
                            writer.WriteEndElement();
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        #endregion
        #region----APPLICANT--------
        public static string GetApplicantXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var educationLevel = ServiceHelper.GetEducationLevel();
            string personalDetailPath = "/XML-Field-Lookups/COMMON/Personal-Details";
            string customerCheckPath = "/XML-Field-Lookups/COMMON/Customer-Check";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {
                        //Customer Check
                        string customerCheck = GetLookupCode("Customer Check", customerCheckPath);
                        string customerExists = GetXmlElementName("CustomerExists", customerCheckPath);
                        string customerCode = GetXmlElementName("CustomerCode", customerCheckPath);
                        string partyreference = GetXmlElementName("Partyreference", customerCheckPath);
                        string duediligenceType = GetXmlElementName("Duediligencetype", customerCheckPath);
                        string introducerCIF = GetXmlElementName("Introducer CIF", customerCheckPath);
                        string introducerName = GetXmlElementName("IntroducerName", customerCheckPath);
                        string responsibleBranch = GetXmlElementName("ResponsibleBranch", customerCheckPath);
                        //Personal Details
                        string personalDetails = GetLookupCode("Personal Details", personalDetailPath);
                        string title = GetXmlElementName("Title", personalDetailPath);
                        string firstName = GetXmlElementName("FirstName", personalDetailPath);
                        string lastName = GetXmlElementName("LastName", personalDetailPath);
                        string fathersName = GetXmlElementName("FathersName", personalDetailPath);
                        string gender = GetXmlElementName("Gender", personalDetailPath);
                        string dateOfBirth = GetXmlElementName("DateOfBirth", personalDetailPath);
                        string placeOfBirth = GetXmlElementName("PlaceOfBirth", personalDetailPath);
                        string countryOfBirth = GetXmlElementName("CountryOfBirth", personalDetailPath);
                        string educationLevelEliment = GetXmlElementName("EducationLevel", personalDetailPath);
                        //Purpose and Activity
                        string purposeAndActivitypath = "/XML-Field-Lookups/COMMON/Purpose-and-Activity";
                        string purposeAndActivity = GetLookupCode("Purpose and Activity", purposeAndActivitypath);
                        string reasonForTheOpeningAccounts = GetXmlElementName("ReasonsforOpeningtheAccounts", purposeAndActivitypath);
                        string reasonForTheOpeningAccount = GetXmlElementName("ReasonsForOpeningTheAccount", purposeAndActivitypath);
                        string NatureInAndOutTransactions = GetXmlElementName("NatureInAndOutTransactions", purposeAndActivitypath);
                        string expectedFrequencyOfIncomingOutgoingTransaction = GetXmlElementName("EXPECTEDFREQUENCYOFINCOMINGANDOUTGOINGTRANSACTIONS", purposeAndActivitypath);
                        string expectedFrequencyOfInAndOuttransactions = GetXmlElementName("ExpectedFrequencyOfInAndOuttransactions", purposeAndActivitypath);
                        string expectedIncomingAmount = GetXmlElementName("ExpectedIncomingAmount", purposeAndActivitypath);
                        string newExpectedIncomingAmount = GetXmlElementName("NewExpectedIncomingAmount", purposeAndActivitypath);
                        //Source Of Incoming Transaction
                        string sotPath = "/XML-Field-Lookups/COMMON/Source-Of-Incoming-Transaction";
                        string sourceOfIncomingTransaction = GetLookupCode("Source Of Incoming Transaction", sotPath);
                        string nameOfRemitter = GetXmlElementName("SourceOfIncomingTransactions_NameOfRemitter", sotPath);
                        string countryOfRemitter = GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitter", sotPath);
                        string countryOfRemitterBank = GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitterBank", sotPath);
                        //Source Of Outgoing Transaction
                        string outgoingPath = "/XML-Field-Lookups/COMMON/Source-Of-Outgoing-Transaction";
                        string sourceOfOutgoingTransaction = GetLookupCode("Source Of Outgoing Transaction", outgoingPath);
                        string nameOfBeneficiary = GetXmlElementName("NameOfBeneficiary", outgoingPath);
                        string countryOfBeneficiary = GetXmlElementName("CountryOfBeneficiary", outgoingPath);
                        string countryOfBeneficiaryBank = GetXmlElementName("CountryOfBeneficiaryBank", outgoingPath);
                        //Identification
                        string identificationPath = "/XML-Field-Lookups/COMMON/Identifications";
                        string indentification = GetLookupCode("Identifications", identificationPath);
                        string citizenship = GetXmlElementName("IdentificationDetails_Citizenship", identificationPath);
                        string typeOfIdentification = GetXmlElementName("IdentificationDetails_TypeOfIdentification", identificationPath);
                        string identificationNumber = GetXmlElementName("IdentificationDetails_IdentificationNumber", identificationPath);
                        string countryOfIssue = GetXmlElementName("IdentificationDetails_CountryOfIssue", identificationPath);
                        string issueDate = GetXmlElementName("IdentificationDetails_IssueDate", identificationPath);
                        string expiryDate = GetXmlElementName("IdentificationDetails_ExpiryDate", identificationPath);
                        //Address Details
                        string addressPath = "/XML-Field-Lookups/COMMON/Address-Details";
                        string addressDetails = GetLookupCode("Address Details", addressPath);
                        string addressType = GetXmlElementName("AddressType", addressPath);
                        string mainCorrespondenceAddress = GetXmlElementName("MainCorrespondenceAddress", addressPath);
                        string addressLine1 = GetXmlElementName("AddressLine1", addressPath);
                        string addressLine2 = GetXmlElementName("AddressLine2", addressPath);
                        string postalCode = GetXmlElementName("PostalCode", addressPath);
                        string poBox = GetXmlElementName("POBox", addressPath);
                        string city = GetXmlElementName("City", addressPath);
                        string countryName = GetXmlElementName("Country", addressPath);
                        //Contcat Details
                        string contactDetailsPath = "/XML-Field-Lookups/COMMON/Contact-Details";
                        var preferredLanguage = ServiceHelper.GetCommunicationLanguage();
                        string contactDetails = GetLookupCode("Contact Details", contactDetailsPath);
                        string mobileNumberCode = GetXmlElementName("Country_Code_MobileTelNoNumber", contactDetailsPath);
                        string mobileNumber = GetXmlElementName("ContactDetails_MobileTelNoNumber", contactDetailsPath);
                        string prefereredMobileNumber = GetXmlElementName("PrefereredMobileNumber", contactDetailsPath);
                        string homeTelNumberCode = GetXmlElementName("Country_Code_HomeTelNoNumber", contactDetailsPath);
                        string homeTelNumber = GetXmlElementName("ContactDetails_HomeTelNoNumber", contactDetailsPath);
                        string worktelNumberCode = GetXmlElementName("Country_Code_WorkTelNoNumber", contactDetailsPath);
                        string worktelNumber = GetXmlElementName("ContactDetails_WorkTelNoNumber", contactDetailsPath);
                        string faxNumberCode = GetXmlElementName("Country_Code_FaxNoFaxNumber", contactDetailsPath);
                        string faxNumber = GetXmlElementName("ContactDetails_FaxNoFaxNumber", contactDetailsPath);
                        string emailAddress = GetXmlElementName("ContactDetails_EmailAddress", contactDetailsPath);
                        string preferredCommunicationLanguage = GetXmlElementName("ContactDetails_PreferredCommunicationLanguage", contactDetailsPath);
                        //Tax Details
                        string taxDetailsPath = "/XML-Field-Lookups/COMMON/Tax-Details";
                        string taxDetails = GetLookupCode("Tax Details", taxDetailsPath);
                        string payDefenceTax = GetXmlElementName("PayDefenceTax", taxDetailsPath);
                        string countryOfTaxResidency = GetXmlElementName("TaxDetails_CountryOfTaxResidency", taxDetailsPath);
                        string taxIdentificationNumber = GetXmlElementName("TaxDetails_TaxIdentificationNumber", taxDetailsPath);
                        string tinUnavailableReason = GetXmlElementName("TaxDetails_TinUnavailableReason", taxDetailsPath);
                        string justificationForTinUnavalability = GetXmlElementName("TaxDetails_JustificationForTinUnavalability", taxDetailsPath);
                        //Employement Details
                        string employmentDetailspath = "/XML-Field-Lookups/INDIVIDUAL/Employement-Details";
                        string employmentDetails = GetLookupCode("Employement Details", employmentDetailspath);
                        string employmentStatus = GetXmlElementName("EmploymentStatus", employmentDetailspath);
                        string profession = GetXmlElementName("Profession", employmentDetailspath);
                        string yearInBusiness = GetXmlElementName("YearsInBusiness", employmentDetailspath);
                        string employersName = GetXmlElementName("EmployersName", employmentDetailspath);
                        string employersBusiness = GetXmlElementName("EmployersBusiness", employmentDetailspath);
                        //Foremer Details
                        string formerPath = "/XML-Field-Lookups/INDIVIDUAL/Former-Employer";
                        string formerEmployer = GetLookupCode("Former Employer", formerPath);
                        string formerEmployersName = GetXmlElementName("FormerEmployersName", formerPath);
                        string formerEmployersBusiness = GetXmlElementName("FormerEmployersBusiness", formerPath);
                        string formerCountryOfEmployment = GetXmlElementName("FormerCountryOfEmployment", formerPath);
                        string formerProfession = GetXmlElementName("FormerProfession", formerPath);
                        string grossAnnualSalary = GetXmlElementName("GrossAnnualSalary", formerPath);
                        string totalOtherIncome = GetXmlElementName("TotalOtherIncome", formerPath);
                        //Origin Of Income
                        string originOfIncomePath = "/XML-Field-Lookups/COMMON/Origin-Of-Income";
                        string originOfIncome = GetLookupCode("Origin Of Income", originOfIncomePath);
                        string sourceOfAnnualIncome = GetXmlElementName("SourceOfAnnualIncome", originOfIncomePath);
                        string specifyOtherSource = GetXmlElementName("SpecifyOtherSource", originOfIncomePath);
                        string amountOfIncome = GetXmlElementName("AmountOfIncome", originOfIncomePath);
                        //Origin Of Total Assets
                        string originOfAssetspath = "/XML-Field-Lookups/COMMON/Origin-Of-Total-Assets";
                        string originOfTotalAssests = GetLookupCode("Origin Of Total Assets", originOfAssetspath);
                        string originOfTotalAssest = GetXmlElementName("OriginOfTotalAssets", originOfAssetspath);
                        string specifyOtherOrigin = GetXmlElementName("SpecifyOtherOrigin", originOfAssetspath);
                        string amountOfTotalAsset = GetXmlElementName("AmountOfTotalAsset", originOfAssetspath);
                        //PEP Details
                        string pepApplicantPath = "/XML-Field-Lookups/COMMON/PEP-Applicant";
                        string isApplicantAPep = GetXmlElementName("IsApplicantaPep", pepApplicantPath);
                        string pepApplicants = GetLookupCode("PEP Applicant", pepApplicantPath);
                        string positionOrganisation = GetXmlElementName("PepApplicant_PositionOrganization", pepApplicantPath);
                        string pepApplicantCountry = GetXmlElementName("PepApplicant_Country", pepApplicantPath);
                        string pepApplicantSince = GetXmlElementName("PepApplicant_Since", pepApplicantPath);
                        string pepApplicantUntill = GetXmlElementName("PepApplicant_Untill", pepApplicantPath);
                        //PEP Associates
                        string associatesPath = "/XML-Field-Lookups/COMMON/PEP-Family-Associates";
                        string isFamilyMemberAssociatePep = GetXmlElementName("IsFamilyMemberAssociatePep", associatesPath);
                        string familyAssociates = GetLookupCode("PEP Family Associates", associatesPath);
                        string associatesFirstName = GetXmlElementName("PepAssociates_FirstName", associatesPath);
                        string associatesSurname = GetXmlElementName("PepAssociates_Surname", associatesPath);
                        string associatesRelationship = GetXmlElementName("PepAssociates_Relationship", associatesPath);
                        string associatesPositionOrganisation = GetXmlElementName("PepAssociates_PositionOrganization", associatesPath);
                        string associatesCountry = GetXmlElementName("PepAssociates_Country", associatesPath);
                        string associatesSince = GetXmlElementName("PepAssociates_Since", associatesPath);
                        string associatesUntil = GetXmlElementName("PepAssociates_Until", associatesPath);
                        //Bank Relationship
                        string bankRelationshipPath = "/XML-Field-Lookups/COMMON/Bank-Relationship";
                        string bankRealtionship = GetLookupCode("Bank Relationship", bankRelationshipPath);
                        string hasAccountInOtherBank = GetXmlElementName("HasAccountInOtherBank", bankRelationshipPath);
                        string nameOfBankingInstitution = GetXmlElementName("NameOfBankingInstitution", bankRelationshipPath);
                        string countryOfBankingInstitution = GetXmlElementName("CountryOfBankingInstitution", bankRelationshipPath);
                        //Consents
                        string consentsPath = "/XML-Field-Lookups/INDIVIDUAL/Consents";
                        string consents = GetLookupCode("Consents", consentsPath);
                        string consentForFormarMarketingPurposes = GetXmlElementName("ConsentForFormarMarketingPurposes", consentsPath);
                        //Card Details
                        string cardPath = "/XML-Field-Lookups/COMMON/Card-Details";
                        var cardholderName = CommonProcess.GetCardHolderNameIndividual(application.ApplicationDetails.ApplicationDetails_ApplicationNumber);
                        var debitCardAccounts = AccountsProcess.GetDebitCardAccountsByApplicationID(application.ApplicationDetails.ApplicationDetailsID);
                        var alldispatchInd = ServiceHelper.GetDispatchMethodIndividual();
                        string cardDetails = GetLookupCode("Card Details", cardPath);
                        string debitCardType = GetXmlElementName("DebitCardDetails_CardType", cardPath);
                        string cardAssociatedAccount = GetXmlElementName("AssociatedAccount", cardPath);
                        string cardDetailsFullName = GetXmlElementName("DebitCardDetails_FullName", cardPath);
                        string cardDetailsMobileNumberCode = GetXmlElementName("Country_Code", cardPath);
                        string cardDetailsMobileNumber = GetXmlElementName("DebitCardDetails_MobileNumber", cardPath);
                        string cardDetailsDispatchMethod = GetXmlElementName("DebitCardDetails_DispatchMethod", cardPath);
                        string cardDetailsDeliveryAddress = GetXmlElementName("DebitCardDetails_DeliveryAddress", cardPath);
                        string cardDetailsOtherDeliveryAddress = GetXmlElementName("DebitCardDetails_OtherDeliveryAddress", cardPath);
                        string cardDetailsCollectedBy = GetXmlElementName("DebitCardDetails_CollectedBy", cardPath);
                        string cardDetailsIndentityNumber = GetXmlElementName("DebitCardDetails_IdentityNumber", cardPath);
                        string cardDetailsDeliveryDetails = GetXmlElementName("DebitCardDetails_DeliveryDetails", cardPath);
                        //Start Applicant
                        writer.WriteStartElement("applicants");
                        foreach (var item in application.Applicants)
                        {
                            writer.WriteStartElement("applicant");
                            //Customer Check
                            writer.WriteStartElement(customerCheck);
                            writer.WriteElementString(customerExists, "NO");
                            writer.WriteElementString(customerCode, item.PersonalDetails.PersonalDetails_CustomerCIF);
                            writer.WriteEndElement();
                            writer.WriteElementString(partyreference, item.PersonalDetails.Id.ToString());
                            writer.WriteElementString(duediligenceType, "ENHANCED");
                            writer.WriteElementString(introducerCIF, application.ApplicationDetails.ApplicationDetails_IntroducerCIF.Trim().ToUpper());
                            writer.WriteElementString(introducerName, application.ApplicationDetails.ApplicationDetails_IntroducerName.Trim().ToUpper());
                            //writer.WriteElementString(GetXmlElementName("IntroducedBank", customerCheckPath), application.ApplicationDetails.ApplicationDetails_IntroducerName);
                            //string ResponsibleBranchText = ServiceHelper.GetName(ValidationHelper.GetString(application.ApplicationDetails.ApplicationDetails_ResponsibleBankingCenter, ""), Constants.Bank_Units);
                            string Bankunitcode = string.Empty;
                            BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(application.ApplicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
                            if (bankUnit != null)
                            {
                                Bankunitcode = bankUnit.BankUnitCode;
                            }
                            writer.WriteElementString(responsibleBranch, Bankunitcode.Trim().ToUpper());

                            string applicantName = item.PersonalDetails.FirstName + " " + item.PersonalDetails.LastName;
                            //Personal Details
                            if (item.PersonalDetails != null)
                            {
                                string personTitleText = ServiceHelper.GetName(ValidationHelper.GetString(item.PersonalDetails.Title, ""), Constants.TITLES);
                                string personGenderText = ServiceHelper.GetName(ValidationHelper.GetString(item.PersonalDetails.Gender, ""), Constants.GENDER);
                                writer.WriteStartElement(personalDetails);
                                string countryofBirthName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBirth != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString()).Text : string.Empty;
                                string educationLevelName = (educationLevel != null && educationLevel.Count > 0 && item.PersonalDetails.EducationLevel != null && educationLevel.Any(f => f.Value == item.PersonalDetails.EducationLevel.ToString())) ? educationLevel.FirstOrDefault(f => f.Value == item.PersonalDetails.EducationLevel.ToString()).Text : string.Empty;
                                writer.WriteElementString(title, personTitleText.Trim().ToUpper());
                                writer.WriteElementString(title + "_code", GetLookUpItemCode(personTitleText, Constants.TITLES)?.Trim().ToUpper());
                                writer.WriteElementString(firstName, item.PersonalDetails.FirstName.Trim().ToUpper());
                                writer.WriteElementString(lastName, item.PersonalDetails.LastName.Trim().ToUpper());
                                writer.WriteElementString(fathersName, item.PersonalDetails.FathersName.Trim().ToUpper());
                                writer.WriteElementString(gender, personGenderText.Trim().ToUpper());
                                writer.WriteElementString(gender + "_code", GetLookUpItemCode(personGenderText, Constants.GENDER)?.Trim().ToUpper());
                                writer.WriteElementString(dateOfBirth, Convert.ToDateTime(item.PersonalDetails.DateOfBirth).ToString("yyyy-MM-dd"));
                                writer.WriteElementString(placeOfBirth, item.PersonalDetails.PlaceOfBirth.Trim().ToUpper());
                                writer.WriteElementString(countryOfBirth, countryofBirthName.Trim().ToUpper());
                                writer.WriteElementString(countryOfBirth + "_code", ServiceHelper.GetCountryTwoletterCode(countryofBirthName)?.Trim().ToUpper());
                                writer.WriteElementString(educationLevelEliment, educationLevelName.Trim().ToUpper());
                                writer.WriteElementString(educationLevelEliment + "_code", GetLookUpItemCode(educationLevelName, Constants.Education)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Purpose and Activity
                            if (application.ApplicationDetails != null)
                            {
                                writer.WriteStartElement(purposeAndActivity);
                                if (application.PurposeAndActivity != null)
                                {
                                    writer.WriteStartElement(reasonForTheOpeningAccounts);
                                    foreach (var reason in application.PurposeAndActivity.ReasonForOpeningTheAccountGroup.Items)
                                    {
                                        var selectedValue = application.PurposeAndActivity.ReasonForOpeningTheAccountGroup.MultiSelectValue;
                                        if (selectedValue != null)
                                        {
                                            if (selectedValue.Contains(reason.Value))
                                            {
                                                writer.WriteElementString(reasonForTheOpeningAccount, reason.Text.Trim().ToUpper());
                                                writer.WriteElementString(reasonForTheOpeningAccount + "_code", GetLookUpItemCode(reason.Text, Constants.PURPOSE_AND_REASON_FOR_OPENING_THE_ACCOUNT)?.Trim().ToUpper());
                                            }
                                        }
                                    }
                                    writer.WriteEndElement();
                                    writer.WriteStartElement("expectednaturesoftransactions");
                                    foreach (var nature in application.PurposeAndActivity.ExpectedNatureOfInAndOutTransactionGroup.Items)
                                    {
                                        var selectedValue = application.PurposeAndActivity.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue;
                                        if (selectedValue != null)
                                        {
                                            if (selectedValue.Contains(nature.Value))
                                            {
                                                writer.WriteElementString(NatureInAndOutTransactions, nature.Text.Trim().ToUpper());
                                                writer.WriteElementString(NatureInAndOutTransactions + "_code", GetLookUpItemCode(nature.Text, Constants.EXPECTED_NATURE_INCOMING_OUTGOING_TRANSACTION)?.Trim().ToUpper());
                                            }
                                        }
                                    }
                                    writer.WriteEndElement();
                                    writer.WriteStartElement(expectedFrequencyOfIncomingOutgoingTransaction);
                                    foreach (var frequency in application.PurposeAndActivity.ExpectedFrequencyOfInAndOutTransactionGroup.Items)
                                    {
                                        var selectedValue = application.PurposeAndActivity.ExpectedFrequencyOfInAndOutTransactionGroup.RadioGroupValue;
                                        if (selectedValue != null)
                                        {
                                            if (selectedValue.Contains(frequency.Value))
                                            {
                                                writer.WriteElementString(expectedFrequencyOfInAndOuttransactions, frequency.Label.Trim().ToUpper());
                                                writer.WriteElementString(expectedFrequencyOfInAndOuttransactions + "_code", GetLookUpItemCode(frequency.Label, Constants.EXPECTED_FREQUENCY_INCOMING_OUTGOING_TRANSACTION)?.Trim().ToUpper());
                                            }
                                        }
                                    }
                                    writer.WriteEndElement();
                                    writer.WriteElementString(expectedIncomingAmount, application.PurposeAndActivity.ExpectedIncomingAmount?.ToString());
                                    writer.WriteElementString(newExpectedIncomingAmount, application.PurposeAndActivity.ExpectedIncomingAmount?.ToString());
                                }
                                //Source Of Incoming Transaction

                                if (application.SourceOfIncomingTransactions != null)
                                {
                                    writer.WriteStartElement(sourceOfIncomingTransaction);
                                    foreach (var sot in application.SourceOfIncomingTransactions)
                                    {
                                        writer.WriteStartElement("sourceofincomingtxns");
                                        writer.WriteElementString(nameOfRemitter, sot.SourceOfIncomingTransactions_NameOfRemitter.Trim().ToUpper());
                                        writer.WriteElementString(countryOfRemitter, sot.SourceOfIncomingTransactions_CountryOfRemitter.Trim().ToUpper());
                                        writer.WriteElementString(countryOfRemitter + "_code", ServiceHelper.GetCountryTwoletterCode(sot.SourceOfIncomingTransactions_CountryOfRemitter)?.Trim().ToUpper());
                                        writer.WriteElementString(countryOfRemitterBank, sot.SourceOfIncomingTransactions_CountryOfRemitterBank.Trim().ToUpper());
                                        writer.WriteElementString(countryOfRemitterBank + "_code", ServiceHelper.GetCountryTwoletterCode(sot.SourceOfIncomingTransactions_CountryOfRemitterBank)?.Trim().ToUpper());
                                        writer.WriteEndElement();

                                    }
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                //Source Of Outgoing Transaction
                                if (application.SourceOfOutgoingTransactions != null)
                                {
                                    writer.WriteStartElement(sourceOfOutgoingTransaction);
                                    foreach (var outgoing in application.SourceOfOutgoingTransactions)
                                    {
                                        writer.WriteStartElement("sourceofoutgoingtxns");
                                        writer.WriteElementString(nameOfBeneficiary, outgoing.NameOfBeneficiary.Trim().ToUpper());
                                        writer.WriteElementString(countryOfBeneficiary, outgoing.CountryOfBeneficiary.Trim().ToUpper());
                                        writer.WriteElementString(countryOfBeneficiary + "_code", ServiceHelper.GetCountryTwoletterCode(outgoing.CountryOfBeneficiary)?.Trim().ToUpper());
                                        writer.WriteElementString(countryOfBeneficiaryBank, outgoing.CountryOfBeneficiaryBank.Trim().ToUpper());
                                        writer.WriteElementString(countryOfBeneficiaryBank + "_code", ServiceHelper.GetCountryTwoletterCode(outgoing.CountryOfBeneficiaryBank)?.Trim().ToUpper());
                                        writer.WriteEndElement();

                                    }
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }

                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Identification
                            if (item._lst_IdentificationDetails != null)
                            {
                                writer.WriteStartElement(indentification);
                                foreach (var identification in item._lst_IdentificationDetails)
                                {
                                    writer.WriteStartElement("Identification");
                                    writer.WriteElementString(citizenship, identification.IdentificationDetails_CitizenshipName.Trim().ToUpper());
                                    writer.WriteElementString(citizenship + "_code", ServiceHelper.GetCountryTwoletterCode(identification.IdentificationDetails_CitizenshipName)?.Trim().ToUpper());
                                    writer.WriteElementString(typeOfIdentification, identification.IdentificationDetails_TypeOfIdentificationName.Trim().ToUpper());
                                    writer.WriteElementString(typeOfIdentification + "_code", GetLookUpItemCode(identification.IdentificationDetails_TypeOfIdentificationName, Constants.IDENTIFICATION_TYPE)?.Trim().ToUpper());
                                    writer.WriteElementString(identificationNumber, identification.IdentificationDetails_IdentificationNumber.Trim().ToUpper());
                                    writer.WriteElementString(countryOfIssue, identification.IdentificationDetails_CountryOfIssueName.Trim().ToUpper());
                                    writer.WriteElementString(countryOfIssue + "_code", ServiceHelper.GetCountryTwoletterCode(identification.IdentificationDetails_CountryOfIssueName)?.Trim().ToUpper());
                                    writer.WriteElementString(issueDate, Convert.ToDateTime(identification.IdentificationDetails_IssueDate).ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(expiryDate, Convert.ToDateTime(identification.IdentificationDetails_ExpiryDate).ToString("yyyy-MM-dd"));
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Address Details
                            if (item._lst_AddressDetails != null)
                            {
                                writer.WriteStartElement(addressDetails);
                                foreach (var address in item._lst_AddressDetails)
                                {
                                    writer.WriteStartElement("address");
                                    writer.WriteElementString(addressType, address.AddressTypeName.Trim().ToUpper());
                                    writer.WriteElementString(addressType + "_code", GetLookUpItemCode(address.AddressTypeName, Constants.ADDRESS_TYPE)?.Trim().ToUpper());
                                    //writer.WriteElementString(mainCorrespondenceAddress, address.AddressTypeName.Trim().ToUpper().Contains("RESIDENTIAL") ? "1" : "0");
                                    writer.WriteElementString(mainCorrespondenceAddress, address.MainCorrespondenceAddress == true ? "1" : "0");
                                    writer.WriteElementString(addressLine1, address.AddressLine1.Trim().ToUpper());
                                    writer.WriteElementString(addressLine2, address.AddressLine2.Trim().ToUpper());
                                    writer.WriteElementString(postalCode, address.PostalCode.Trim().ToUpper());
                                    writer.WriteElementString(poBox, address.POBox.ToString().Trim().ToUpper());
                                    writer.WriteElementString(city, address.City.ToString().Trim().ToUpper());
                                    writer.WriteElementString(countryName, address.CountryName.Trim().ToUpper());
                                    writer.WriteElementString(countryName + "_code", ServiceHelper.GetCountryTwoletterCode(address.CountryName)?.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Contcat Details

                            if (item.ContactDetails != null)
                            {
                                writer.WriteStartElement(contactDetails);
                                string preferredLanguageText = (preferredLanguage != null && preferredLanguage.Count > 0 && item.ContactDetails.ContactDetails_PreferredCommunicationLanguage != null && preferredLanguage.Any(f => f.Value == item.ContactDetails.ContactDetails_PreferredCommunicationLanguage.ToString())) ? preferredLanguage.FirstOrDefault(f => f.Value == item.ContactDetails.ContactDetails_PreferredCommunicationLanguage.ToString()).Text : string.Empty;
                                writer.WriteElementString(mobileNumberCode, string.IsNullOrEmpty(item.ContactDetails.Country_Code_MobileTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_MobileTelNoNumber);
                                writer.WriteElementString(mobileNumber, item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                //writer.WriteElementString(GetXmlElementName("PrefereredMobileNumber", contactDetailsPath), item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                writer.WriteElementString(prefereredMobileNumber, string.IsNullOrEmpty(item.ContactDetails.Country_Code_MobileTelNoNumber) ? "N" : "Y");
                                writer.WriteElementString(homeTelNumberCode, string.IsNullOrEmpty(item.ContactDetails.Country_Code_HomeTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_HomeTelNoNumber);
                                writer.WriteElementString(homeTelNumber, item.ContactDetails.ContactDetails_HomeTelNoNumber);
                                writer.WriteElementString(worktelNumberCode, string.IsNullOrEmpty(item.ContactDetails.Country_Code_WorkTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_WorkTelNoNumber);
                                writer.WriteElementString(worktelNumber, item.ContactDetails.ContactDetails_WorkTelNoNumber);
                                writer.WriteElementString(faxNumberCode, string.IsNullOrEmpty(item.ContactDetails.Country_Code_FaxNoFaxNumber) ? "" : "+" + item.ContactDetails.Country_Code_FaxNoFaxNumber);
                                writer.WriteElementString(faxNumber, item.ContactDetails.ContactDetails_FaxNoFaxNumber);
                                writer.WriteElementString(emailAddress, item.ContactDetails.ContactDetails_EmailAddress.Trim().ToUpper());
                                writer.WriteElementString(preferredCommunicationLanguage, GetLookUpItemCode(item.ContactDetails.ContactDetails_PreferredCommunicationLanguage, Constants.COMMUNICATION_LANGUAGE)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Tax Details
                            if (item._lst_TaxDetails != null)
                            {
                                writer.WriteStartElement(taxDetails);
                                writer.WriteElementString(payDefenceTax, string.Equals(item.PersonalDetails.IsLiableToPayDefenseTaxInCyprusName, "true", StringComparison.OrdinalIgnoreCase) ? "YES" : "NO");
                                foreach (var tax in item._lst_TaxDetails)
                                {
                                    writer.WriteStartElement("taxresidency");
                                    writer.WriteElementString(countryOfTaxResidency, tax.TaxDetails_CountryOfTaxResidencyName.Trim().ToUpper());
                                    writer.WriteElementString(countryOfTaxResidency + "_code", ServiceHelper.GetCountryTwoletterCode(tax.TaxDetails_CountryOfTaxResidencyName)?.Trim().ToUpper());
                                    writer.WriteElementString(taxIdentificationNumber, tax.TaxDetails_TaxIdentificationNumber.Trim().ToUpper());
                                    writer.WriteElementString(tinUnavailableReason, tax.TaxDetails_TinUnavailableReasonName.Trim().ToUpper());
                                    writer.WriteElementString(tinUnavailableReason + "_code", GetLookUpItemCode(tax.TaxDetails_TinUnavailableReasonName, Constants.TIN_UNAVAILABLE_REASON)?.Trim().ToUpper());
                                    writer.WriteElementString(justificationForTinUnavalability, tax.TaxDetails_JustificationForTinUnavalability.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Buiness Profile
                            writer.WriteStartElement("businessprofile");
                            //Employement Details
                            if (item.EmploymentDetails != null)
                            {
                                writer.WriteStartElement(employmentDetails);
                                //writer.WriteElementString(GetXmlElementName("EmploymentStatus", employmentDetailspath), item.EmploymentDetails.EmploymentStatusName);
                                writer.WriteElementString(employmentStatus, item.EmploymentDetails.EmploymentStatusName.Trim().ToUpper());
                                writer.WriteElementString(employmentStatus + "_code", GetLookUpItemCode(item.EmploymentDetails.EmploymentStatusName, Constants.EMPLOYMENT_STATUS)?.Trim().ToUpper());
                                writer.WriteElementString(profession, item.EmploymentDetails.ProfessionName.Trim().ToUpper());
                                writer.WriteElementString(profession + "_code", GetLookUpItemCode(item.EmploymentDetails.ProfessionName, Constants.PROFESSION)?.Trim().ToUpper());
                                writer.WriteElementString(yearInBusiness, item.EmploymentDetails.YearsInBusiness.Trim().ToUpper());
                                writer.WriteElementString(employersName, item.EmploymentDetails.EmployersName.Trim().ToUpper());
                                writer.WriteElementString(employersBusiness, item.EmploymentDetails.EmployersBusiness.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Foremer Details
                            if (item.EmploymentDetails != null)
                            {
                                string countryOfEmployment = (country != null && country.Count > 0 && item.EmploymentDetails.FormerCountryOfEmployment != null && country.Any(f => f.Value == item.EmploymentDetails.FormerCountryOfEmployment.ToString())) ? country.FirstOrDefault(f => f.Value == item.EmploymentDetails.FormerCountryOfEmployment.ToString()).Text : string.Empty;
                                writer.WriteStartElement(formerEmployer);
                                writer.WriteElementString(formerEmployersName, item.EmploymentDetails.FormerEmployersName.Trim().ToUpper());
                                writer.WriteElementString(formerEmployersBusiness, item.EmploymentDetails.FormerEmployersBusiness.Trim().ToUpper());
                                writer.WriteElementString(formerCountryOfEmployment, countryOfEmployment.Trim().ToUpper());
                                writer.WriteElementString(formerCountryOfEmployment + "_code", ServiceHelper.GetCountryTwoletterCode(countryOfEmployment)?.Trim().ToUpper());
                                writer.WriteElementString(formerProfession, item.EmploymentDetails.FormerProfessionName.Trim().ToUpper());
                                writer.WriteElementString(formerProfession + "_code", GetLookUpItemCode(item.EmploymentDetails.FormerProfessionName, Constants.PROFESSION)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                                if (item._lst_SourceOfIncomeModel != null && item._lst_SourceOfIncomeModel.Any(x => string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)))
                                {
                                    writer.WriteElementString(grossAnnualSalary, item._lst_SourceOfIncomeModel.Where(x => string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)).Select(x => x.AmountOfIncome.ToString()).FirstOrDefault());
                                }
                                else
                                {
                                    writer.WriteElementString(grossAnnualSalary, string.Empty);
                                }

                                if (item._lst_SourceOfIncomeModel != null && item._lst_SourceOfIncomeModel.Any(x => !string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)))
                                {
                                    writer.WriteElementString(totalOtherIncome, item._lst_SourceOfIncomeModel.Where(x => !string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.AmountOfIncome)).ToString());
                                }
                                else
                                {
                                    writer.WriteElementString(totalOtherIncome, string.Empty);
                                }
                            }
                            //Origin Of Income
                            if (item._lst_SourceOfIncomeModel != null)
                            {
                                if (!(item._lst_SourceOfIncomeModel.Count() == 1 && string.Equals(item._lst_SourceOfIncomeModel.FirstOrDefault().SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)))
                                {
                                    writer.WriteStartElement(originOfIncome);
                                    foreach (var income in item._lst_SourceOfIncomeModel)
                                    {
                                        if (!string.Equals(income.SourceOfAnnualIncomeName?.Trim(), "GROSS SALARY", StringComparison.OrdinalIgnoreCase))
                                        {
                                            writer.WriteStartElement("originofincome");
                                            writer.WriteElementString(sourceOfAnnualIncome, income.SourceOfAnnualIncomeName?.Trim().ToUpper());
                                            writer.WriteElementString(sourceOfAnnualIncome + "_code", GetLookUpItemCode(income.SourceOfAnnualIncomeName, Constants.SOURCE_OF_ANNUAL_INCOME)?.Trim().ToUpper());
                                            writer.WriteElementString(specifyOtherSource, income.SpecifyOtherSource?.Trim().ToUpper());
                                            writer.WriteElementString(amountOfIncome, income.AmountOfIncome.ToString());
                                            writer.WriteEndElement();
                                        }
                                    }
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                            }
                            //Origin Of Total Assets
                            if (item._lst_OriginOfTotalAssetsModel != null)
                            {
                                writer.WriteStartElement(originOfTotalAssests);
                                foreach (var assets in item._lst_OriginOfTotalAssetsModel)
                                {
                                    string originOfTotalAssetsText = ServiceHelper.GetName(ValidationHelper.GetString(assets.OriginOfTotalAssets, ""), Constants.ORIGIN_OF_TOTAL_ASSETS);
                                    writer.WriteStartElement(originOfTotalAssest);
                                    writer.WriteElementString(originOfTotalAssest, originOfTotalAssetsText.Trim().ToUpper());
                                    writer.WriteElementString(originOfTotalAssest + "_code", GetLookUpItemCode(originOfTotalAssetsText, Constants.ORIGIN_OF_TOTAL_ASSETS)?.Trim().ToUpper());
                                    writer.WriteElementString(specifyOtherOrigin, assets.SpecifyOtherOrigin.Trim().ToUpper());
                                    writer.WriteElementString(amountOfTotalAsset, assets.AmountOfTotalAsset.ToString());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            writer.WriteEndElement();
                            //PEP Details
                            writer.WriteStartElement("pepdetails");
                            //PEP Applicant
                            writer.WriteElementString(isApplicantAPep, item.PersonalDetails.IsPepName == "true" ? "YES" : "NO");
                            if (item._lst_PepApplicantViewModel != null && item.PersonalDetails.IsPepName == "true")
                            {
                                foreach (var pepApplicant in item._lst_PepApplicantViewModel)
                                {
                                    writer.WriteStartElement(pepApplicants);
                                    writer.WriteElementString(positionOrganisation, pepApplicant.PepApplicant_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(pepApplicantCountry, pepApplicant.PepApplicant_CountryName.Trim().ToUpper());
                                    writer.WriteElementString(pepApplicantCountry + "_code", ServiceHelper.GetCountryTwoletterCode(pepApplicant.PepApplicant_CountryName)?.Trim().ToUpper());
                                    writer.WriteElementString(pepApplicantSince, Convert.ToDateTime(pepApplicant.PepApplicant_Since).ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(pepApplicantUntill, Convert.ToDateTime(pepApplicant.PepApplicant_Untill).ToString("yyyy-MM-dd"));
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(pepApplicants);
                                writer.WriteElementString(positionOrganisation, string.Empty);
                                writer.WriteElementString(pepApplicantCountry, "");
                                writer.WriteElementString(pepApplicantCountry + "_code", "");
                                writer.WriteElementString(pepApplicantSince, "");
                                writer.WriteElementString(pepApplicantUntill, "");
                                writer.WriteEndElement();
                            }
                            //PEP Associates

                            writer.WriteElementString(isFamilyMemberAssociatePep, item.PersonalDetails.IsRelatedToPepName == "true" ? "YES" : "NO");
                            if (item._lst_PepAssociatesViewModel != null && item.PersonalDetails.IsRelatedToPepName == "true")
                            {
                                foreach (var pepAssociates in item._lst_PepAssociatesViewModel)
                                {
                                    writer.WriteStartElement(familyAssociates);
                                    writer.WriteElementString(associatesFirstName, pepAssociates.PepAssociates_FirstName.Trim().ToUpper());
                                    writer.WriteElementString(associatesSurname, pepAssociates.PepAssociates_Surname.Trim().ToUpper());
                                    writer.WriteElementString(associatesRelationship, pepAssociates.PepAssociates_RelationshipName.Trim().ToUpper());
                                    writer.WriteElementString(associatesRelationship + "_code", GetLookUpItemCode(pepAssociates.PepAssociates_RelationshipName, Constants.RELATIONSHIPS)?.Trim().ToUpper());
                                    writer.WriteElementString(associatesPositionOrganisation, pepAssociates.PepAssociates_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(associatesCountry, pepAssociates.PepAssociates_CountryName.Trim().ToUpper());
                                    writer.WriteElementString(associatesCountry + "_code", ServiceHelper.GetCountryTwoletterCode(pepAssociates.PepAssociates_CountryName)?.Trim().ToUpper());
                                    writer.WriteElementString(associatesSince, Convert.ToDateTime(pepAssociates.PepAssociates_Since).ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(associatesUntil, Convert.ToDateTime(pepAssociates.PepAssociates_Until).ToString("yyyy-MM-dd"));
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(familyAssociates);
                                writer.WriteElementString(associatesFirstName, "");
                                writer.WriteElementString(associatesSurname, "");
                                writer.WriteElementString(associatesRelationship, "");
                                writer.WriteElementString(associatesRelationship + "_code", "");
                                writer.WriteElementString(associatesPositionOrganisation, "");
                                writer.WriteElementString(associatesCountry, "");
                                writer.WriteElementString(associatesCountry + "_code", "");
                                writer.WriteElementString(associatesSince, "");
                                writer.WriteElementString(associatesUntil, "");
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.Flush();
                            //Bank Relationship
                            if (item.PersonalDetails != null)
                            {
                                string bankRelationshipcountryName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBankingInstitution != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBankingInstitution.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBankingInstitution.ToString()).Text : string.Empty;
                                writer.WriteStartElement(bankRealtionship);
                                writer.WriteElementString(hasAccountInOtherBank, item.PersonalDetails.HasAccountInOtherBankName == "true" ? "YES" : "NO");
                                writer.WriteElementString(hasAccountInOtherBank + "_code", item.PersonalDetails.HasAccountInOtherBankName == "true" ? "1" : "2");
                                writer.WriteElementString(nameOfBankingInstitution, item.PersonalDetails.NameOfBankingInstitution.Trim().ToUpper());
                                writer.WriteElementString(countryOfBankingInstitution, bankRelationshipcountryName.Trim().ToUpper());
                                writer.WriteElementString(countryOfBankingInstitution + "_code", ServiceHelper.GetCountryTwoletterCode(bankRelationshipcountryName)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Consents
                            writer.WriteStartElement(consents);
                            writer.WriteElementString(consentForFormarMarketingPurposes, "YES");
                            writer.WriteEndElement();
                            //Card Details
                            if (application.DebitCards != null)
                            {
                                writer.WriteStartElement("carddetails");
                                foreach (var card in application.DebitCards)
                                {
                                    string cardholder = cardholderName.FirstOrDefault(x => x.Value == card.DebitCardDetails_CardholderName)?.Text;
                                    if (string.Equals(applicantName, cardholder, StringComparison.OrdinalIgnoreCase))
                                    {

                                        string associatedAccountName = (debitCardAccounts != null && debitCardAccounts.Count > 0 && card.AssociatedAccount != null) ? debitCardAccounts.FirstOrDefault(f => f.Value == card.AssociatedAccount.ToString())?.Text : string.Empty;
                                        string associatedAccount = ServiceHelper.GetName(ValidationHelper.GetString(card.AssociatedAccount, ""), Constants.Associated_Account_Type);


                                        string dispatchMethod = (alldispatchInd != null && alldispatchInd.Count > 0 && card.DebitCardDetails_DispatchMethodName != null) ? alldispatchInd.FirstOrDefault(f => f.Value == card.DebitCardDetails_DispatchMethodName.ToString())?.Text : string.Empty;

                                        string deliveryAddress = string.Empty;

                                        var personDeliveryAdderss = AddressDetailsProcess.GetCardHolderAddresses(card.DebitCardDetails_CardholderName);
                                        if (personDeliveryAdderss != null && personDeliveryAdderss.Any() && personDeliveryAdderss.Count > 0)
                                        {
                                            personDeliveryAdderss = personDeliveryAdderss.Where(x => string.Equals(x.Value, card.DebitCardDetails_DeliveryAddress, StringComparison.OrdinalIgnoreCase)).ToList();
                                            if (personDeliveryAdderss != null && personDeliveryAdderss.Count > 0)
                                            {
                                                deliveryAddress = personDeliveryAdderss.Select(x => x.Text).FirstOrDefault();
                                            }
                                        }
                                        //string dispatchMethod = ServiceHelper.GetName(ValidationHelper.GetString(card.DebitCardDetails_DispatchMethodName, ""), Constants.DISPATCH_METHOD_INDIVIDUAL);
                                        //string deliveryAddress = ServiceHelper.GetName(ValidationHelper.GetString(card.DebitCardDetails_DeliveryAddress, ""), Constants.ADDRESS_TYPE);
                                        writer.WriteStartElement(cardDetails);
                                        writer.WriteElementString(debitCardType, card.DebitCardDetails_CardTypeName?.Trim().ToUpper());
                                        writer.WriteElementString(debitCardType + "_code", associatedAccountName != "" && associatedAccountName != null ? "CLASSIC.DEBIT." + associatedAccountName.Substring(Math.Max(0, associatedAccountName.Length - 3)).Trim().ToUpper() : ""); //GetLookUpItemCode(card.DebitCardDetails_CardTypeName, Constants.CARD_TYPE)?.Trim().ToUpper();
                                        writer.WriteElementString(cardAssociatedAccount, associatedAccountName != "" && associatedAccountName != null ? associatedAccountName.Substring(0, associatedAccountName.Length - 7)?.Trim().ToUpper() : "");
                                        //writer.WriteElementString(GetXmlElementName("AssociatedAccount", cardPath)+"_code", associatedAccountName != "" && associatedAccountName != null ? GetLookUpItemCode(associatedAccountName.Substring(0,associatedAccountName.Length-6), Constants.Accounts_AccountType)?.Trim().ToUpper() : "");
                                        if (string.Equals(application.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                                        {
                                            writer.WriteElementString(cardAssociatedAccount + "_code", associatedAccountName != "" && associatedAccountName != null ? "CURR.ACCT.NO.LIMIT.JOINT" : "");
                                        }
                                        else
                                        {
                                            writer.WriteElementString(cardAssociatedAccount + "_code", associatedAccountName != "" && associatedAccountName != null ? GetLookUpItemCode(associatedAccountName.Substring(0, associatedAccountName.Length - 6), Constants.Accounts_AccountType)?.Trim().ToUpper() : "");
                                        }
                                        writer.WriteElementString("associatedaccountccy", associatedAccountName != "" && associatedAccountName != null ? associatedAccountName.Substring(Math.Max(0, associatedAccountName.Length - 3))?.Trim().ToUpper() : "");
                                        writer.WriteElementString("associatedaccountccy" + "_code", associatedAccountName != "" && associatedAccountName != null ? GetLookUpItemCode(associatedAccountName.Substring(Math.Max(0, associatedAccountName.Length - 3)), Constants.Accounts_Currency)?.Trim().ToUpper() : "");
                                        //writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardholderName", cardPath), cardholder);
                                        writer.WriteElementString(cardDetailsFullName, card.DebitCardDetails_FullName?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsMobileNumberCode, string.IsNullOrEmpty(card.Country_Code) ? "" : "+" + card.Country_Code?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsMobileNumber, card.DebitCardDetails_MobileNumber?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsDispatchMethod, dispatchMethod?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsDispatchMethod + "_code", GetLookUpItemCode(dispatchMethod, Constants.DISPATCH_METHOD_INDIVIDUAL)?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsDeliveryAddress, deliveryAddress?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsOtherDeliveryAddress, deliveryAddress.ToUpper().Contains("OTHER") ? card.DebitCardDetails_OtherDeliveryAddress?.Trim().ToUpper() : "");
                                        writer.WriteElementString(cardDetailsCollectedBy, card.DebitCardDetails_CollectedBy?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsIndentityNumber, card.DebitCardDetails_IdentityNumber?.Trim().ToUpper());
                                        writer.WriteElementString(cardDetailsDeliveryDetails, card.DebitCardDetails_DeliveryDetails?.Trim().ToUpper());
                                        writer.WriteEndElement();
                                    }
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            writer.WriteEndElement();
                        }
                        //End Applicant 
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantPersonDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var educationLevel = ServiceHelper.GetEducationLevel();
            string personalDetailPath = "/XML-Field-Lookups/COMMON/Personal-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    writer.WriteStartElement(GetLookupCode("Personal Details", personalDetailPath));
                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {
                            if (item.PersonalDetails != null)
                            {
                                string countryofBirthName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBirth != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString()).Text : string.Empty;
                                string educationLevelName = (educationLevel != null && educationLevel.Count > 0 && item.PersonalDetails.EducationLevel != null && educationLevel.Any(f => f.Value == item.PersonalDetails.EducationLevel.ToString())) ? educationLevel.FirstOrDefault(f => f.Value == item.PersonalDetails.EducationLevel.ToString()).Text : string.Empty;
                                writer.WriteElementString(GetXmlElementName("Title", personalDetailPath), item.PersonalDetails.Title.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FirstName", personalDetailPath), item.PersonalDetails.FirstName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("LastName", personalDetailPath), item.PersonalDetails.LastName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FathersName", personalDetailPath), item.PersonalDetails.FathersName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Gender", personalDetailPath), item.PersonalDetails.Gender.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("DateOfBirth", personalDetailPath), item.PersonalDetails.DateOfBirth.ToString());
                                writer.WriteElementString(GetXmlElementName("PlaceOfBirth", personalDetailPath), item.PersonalDetails.PlaceOfBirth.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryOfBirth", personalDetailPath), countryofBirthName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EducationLevel", personalDetailPath), educationLevelName.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantIdentificationDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Identifications";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {
                            if (item._lst_IdentificationDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Identifications", path));
                                foreach (var identification in item._lst_IdentificationDetails)
                                {
                                    writer.WriteStartElement("Identification");
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_Citizenship", path), identification.IdentificationDetails_CitizenshipName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_TypeOfIdentification", path), identification.IdentificationDetails_TypeOfIdentificationName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_IdentificationNumber", path), identification.IdentificationDetails_IdentificationNumber.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_CountryOfIssue", path), identification.IdentificationDetails_CountryOfIssueName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_IssueDate", path), identification.IdentificationDetails_IssueDate.ToString());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_ExpiryDate", path), identification.IdentificationDetails_ExpiryDate.ToString());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }

                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantIAddressDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Address-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {

                        foreach (var item in application.Applicants)
                        {
                            if (item._lst_AddressDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Address Details", path));
                                foreach (var address in item._lst_AddressDetails)
                                {
                                    writer.WriteStartElement("address");
                                    writer.WriteElementString(GetXmlElementName("AddressType", path), address.AddressTypeName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("MainCorrespondenceAddress", path), address.MainCorrespondenceAddress == true ? "YES" : "NO");
                                    writer.WriteElementString(GetXmlElementName("AddressLine1", path), address.AddressLine1.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine2", path), address.AddressLine2.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PostalCode", path), address.PostalCode.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("POBox", path), address.POBox.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("City", path), address.City.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("country", path), address.CountryName.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }

                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantIContactDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;

            var preferredLanguage = ServiceHelper.GetCommunicationLanguage();
            string path = "/XML-Field-Lookups/COMMON/Contact-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {

                        foreach (var item in application.Applicants)
                        {
                            if (item.ContactDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Contact Details", path));
                                string preferredLanguageText = (preferredLanguage != null && preferredLanguage.Count > 0 && item.ContactDetails.ContactDetails_PreferredCommunicationLanguage != null && preferredLanguage.Any(f => f.Value == item.ContactDetails.ContactDetails_PreferredCommunicationLanguage.ToString())) ? preferredLanguage.FirstOrDefault(f => f.Value == item.ContactDetails.ContactDetails_PreferredCommunicationLanguage.ToString()).Text : string.Empty;
                                writer.WriteElementString(GetXmlElementName("Country_Code_MobileTelNoNumber", path), "+" + item.ContactDetails.Country_Code_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_MobileTelNoNumber", path), item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("PrefereredMobileNumber", path), item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_HomeTelNoNumber", path), item.ContactDetails.Country_Code_HomeTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_HomeTelNoNumber", path), item.ContactDetails.ContactDetails_HomeTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_WorkTelNoNumber", path), item.ContactDetails.Country_Code_WorkTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_WorkTelNoNumber", path), item.ContactDetails.ContactDetails_WorkTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_FaxNoFaxNumber", path), item.ContactDetails.Country_Code_FaxNoFaxNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_FaxNoFaxNumber", path), item.ContactDetails.ContactDetails_FaxNoFaxNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_EmailAddress", path), item.ContactDetails.ContactDetails_EmailAddress.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ContactDetails_PreferredCommunicationLanguage", path), item.ContactDetails.ContactDetails_PreferredCommunicationLanguage.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }

                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantTaxDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Tax-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {

                        foreach (var item in application.Applicants)
                        {

                            if (item._lst_TaxDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Tax Details", path));
                                writer.WriteElementString(GetXmlElementName("PayDefenceTax", path), item.PersonalDetails.IsLiableToPayDefenseTaxInCyprus == true ? "YES" : "NO");
                                foreach (var tax in item._lst_TaxDetails)
                                {
                                    writer.WriteStartElement("taxdresidency");
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_CountryOfTaxResidency", path), tax.TaxDetails_CountryOfTaxResidencyName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_TaxIdentificationNumber", path), tax.TaxDetails_TaxIdentificationNumber.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_TinUnavailableReason", path), tax.TaxDetails_TinUnavailableReasonName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_JustificationForTinUnavalability", path), tax.TaxDetails_JustificationForTinUnavalability.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }

                        }

                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantOriginOfIncomeXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Origin-Of-Income";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {

                        foreach (var item in application.Applicants)
                        {
                            if (item._lst_SourceOfIncomeModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Origin Of Income", path));
                                foreach (var income in item._lst_SourceOfIncomeModel)
                                {
                                    writer.WriteStartElement("originofincome");
                                    writer.WriteElementString(GetXmlElementName("SourceOfAnnualIncome", path), income.SourceOfAnnualIncomeName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("SpecifyOtherSource", path), income.SpecifyOtherSource.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AmountOfIncome", path), income.AmountOfIncome.ToString());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }

                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantOriginOfTotalAssestsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Origin-Of-Total-Assets";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {

                        foreach (var item in application.Applicants)
                        {
                            if (item._lst_OriginOfTotalAssetsModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Origin Of Total Assets", path));
                                foreach (var assets in item._lst_OriginOfTotalAssetsModel)
                                {
                                    writer.WriteStartElement(GetLookupCode("Origin Of Total Assets", path));
                                    writer.WriteElementString(GetXmlElementName("OriginOfTotalAssets", path), assets.OriginOfTotalAssets.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("SpecifyOtherOrigin", path), assets.SpecifyOtherOrigin.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AmountOfTotalAsset", path), assets.AmountOfTotalAsset.ToString());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }

                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantEmployementDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/Employement-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {
                            if (item.EmploymentDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Employement Details", path));
                                writer.WriteElementString(GetXmlElementName("EmploymentStatus", path), item.EmploymentDetails.EmploymentStatusName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Profession", path), item.EmploymentDetails.ProfessionName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("YearsInBusiness", path), item.EmploymentDetails.YearsInBusiness.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EmployersName", path), item.EmploymentDetails.EmployersName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EmployersBusiness", path), item.EmploymentDetails.EmployersBusiness.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantEmployementFormerDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/Former-Employer";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {
                            if (item.EmploymentDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Former Employer", path));
                                writer.WriteElementString(GetXmlElementName("FormerEmployersName", path), item.EmploymentDetails.FormerEmployersName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FormerEmployersBusiness", path), item.EmploymentDetails.FormerEmployersBusiness.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FormerCountryOfEmployment", path), item.EmploymentDetails.FormerCountryOfEmploymentName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FormerProfession", path), item.EmploymentDetails.FormerProfessionName.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantPepApplicantDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/PEP-Applicant";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {

                            if (item._lst_PepApplicantViewModel != null)
                            {
                                writer.WriteElementString(GetXmlElementName("IsApplicantaPep", path), item.PersonalDetails.IsPep == true ? "YES" : "NO");
                                foreach (var pepApplicant in item._lst_PepApplicantViewModel)
                                {
                                    writer.WriteStartElement(GetLookupCode("PEP Applicant", path));
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_PositionOrganization", path), pepApplicant.PepApplicant_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_Country", path), pepApplicant.PepApplicant_Country.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_Since", path), pepApplicant.PepApplicant_Since.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_Untill", path), pepApplicant.PepApplicant_Untill.ToString().Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantPepFamilyAssociatesDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/PEP-Family-Associates";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {

                            if (item._lst_PepAssociatesViewModel != null)
                            {
                                writer.WriteElementString(GetXmlElementName("IsFamilyMemberAssociatePep", path), item.PersonalDetails.IsRelatedToPep == true ? "YES" : "NO");
                                foreach (var pepAssociates in item._lst_PepAssociatesViewModel)
                                {
                                    writer.WriteStartElement(GetLookupCode("PEP Family Associates", path));
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_FirstName", path), pepAssociates.PepAssociates_FirstName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Surname", path), pepAssociates.PepAssociates_Surname.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", path), pepAssociates.PepAssociates_RelationshipName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_PositionOrganization", path), pepAssociates.PepAssociates_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Country", path), pepAssociates.PepAssociates_CountryName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Since", path), pepAssociates.PepAssociates_Since.ToString());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Until", path), pepAssociates.PepAssociates_Until.ToString());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantBankRelationshipXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            string path = "/XML-Field-Lookups/COMMON/Bank-Relationship";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {
                            if (item.PersonalDetails != null)
                            {
                                string countryName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBankingInstitution != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBankingInstitution.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBankingInstitution.ToString()).Text : string.Empty;
                                writer.WriteStartElement(GetLookupCode("Bank Relationship", path));
                                writer.WriteElementString(GetXmlElementName("HasAccountInOtherBank", path), item.PersonalDetails.HasAccountInOtherBank == true ? "YES" : "NO");
                                writer.WriteElementString(GetXmlElementName("NameOfBankingInstitution", path), item.PersonalDetails.NameOfBankingInstitution.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryOfBankingInstitution", path), countryName.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantConsentsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/Consents";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    writer.WriteStartElement(GetLookupCode("Consents", path));
                    writer.WriteElementString(GetXmlElementName("ConsentForFormarMarketingPurposes", path), "YES");
                    writer.WriteEndElement();
                    writer.Flush();
                }
                result = textWriter.ToString();
            }
            return result;
        }
        #endregion
        #region----RELATED PARTY----
        public static string GetRealtedPartyXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var educationLevel = ServiceHelper.GetEducationLevel();
            string personalDetailsPath = "/XML-Field-Lookups/COMMON/Personal-Details";
            string customerCheckPath = "/XML-Field-Lookups/COMMON/Customer-Check";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.RelatedParties != null)
                    {
                        writer.WriteStartElement("relatedparties");

                        foreach (var item in application.RelatedParties)
                        {
                            writer.WriteStartElement("relatedparty");
                            //Customer Check
                            writer.WriteStartElement(GetLookupCode("Customer Check", customerCheckPath));
                            writer.WriteElementString(GetXmlElementName("Partyreference", customerCheckPath), item.PersonalDetails.Id.ToString());
                            writer.WriteElementString(GetXmlElementName("PartyType", customerCheckPath), "3RDPARTY");
                            writer.WriteElementString(GetXmlElementName("CustomerType", customerCheckPath), "RETAIL");
                            writer.WriteElementString(GetXmlElementName("CustomerExists", customerCheckPath), "NO");
                            writer.WriteElementString(GetXmlElementName("CustomerCode", customerCheckPath), item.PersonalDetails.PersonalDetails_CustomerCIF);
                            writer.WriteEndElement();
                            //Personal Details
                            if (item.PersonalDetails != null)
                            {
                                string personTitleText = ServiceHelper.GetName(ValidationHelper.GetString(item.PersonalDetails.Title, ""), Constants.TITLES);
                                string personGenderText = ServiceHelper.GetName(ValidationHelper.GetString(item.PersonalDetails.Gender, ""), Constants.GENDER);
                                writer.WriteStartElement(GetLookupCode("Personal Details", personalDetailsPath));
                                string countryofBirthName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBirth != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString()).Text : string.Empty;
                                string educationLevelName = (educationLevel != null && educationLevel.Count > 0 && item.PersonalDetails.EducationLevel != null && educationLevel.Any(f => f.Value == item.PersonalDetails.EducationLevel.ToString())) ? educationLevel.FirstOrDefault(f => f.Value == item.PersonalDetails.EducationLevel.ToString()).Text : string.Empty;
                                writer.WriteElementString(GetXmlElementName("Title", personalDetailsPath), personTitleText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Title", personalDetailsPath) + "_code", GetLookUpItemCode(personTitleText, Constants.TITLES)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FirstName", personalDetailsPath), item.PersonalDetails.FirstName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("LastName", personalDetailsPath), item.PersonalDetails.LastName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FathersName", personalDetailsPath), item.PersonalDetails.FathersName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Gender", personalDetailsPath), personGenderText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Gender", personalDetailsPath) + "_code", GetLookUpItemCode(personGenderText, Constants.GENDER)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("DateOfBirth", personalDetailsPath), Convert.ToDateTime(item.PersonalDetails.DateOfBirth).ToString("yyyy-MM-dd"));
                                writer.WriteElementString(GetXmlElementName("PlaceOfBirth", personalDetailsPath), item.PersonalDetails.PlaceOfBirth.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryOfBirth", personalDetailsPath), countryofBirthName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryOfBirth", personalDetailsPath) + "_code", ServiceHelper.GetCountryTwoletterCode(countryofBirthName)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EducationLevel", personalDetailsPath), educationLevelName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EducationLevel", personalDetailsPath) + "_code", GetLookUpItemCode(educationLevelName, Constants.Education)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Identification Details
                            string relatedIdentificationpath = "/XML-Field-Lookups/COMMON/Identifications";
                            if (item._lst_IdentificationDetailsViewModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Identifications", relatedIdentificationpath));
                                foreach (var identification in item._lst_IdentificationDetailsViewModel)
                                {
                                    writer.WriteStartElement("Identification");
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_Citizenship", relatedIdentificationpath), identification.IdentificationDetails_CitizenshipName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_Citizenship", relatedIdentificationpath) + "_code", ServiceHelper.GetCountryTwoletterCode(identification.IdentificationDetails_CitizenshipName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_TypeOfIdentification", relatedIdentificationpath), identification.IdentificationDetails_TypeOfIdentificationName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_TypeOfIdentification", relatedIdentificationpath) + "_code", GetLookUpItemCode(identification.IdentificationDetails_TypeOfIdentificationName, Constants.IDENTIFICATION_TYPE)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_IdentificationNumber", relatedIdentificationpath), identification.IdentificationDetails_IdentificationNumber.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_CountryOfIssue", relatedIdentificationpath), identification.IdentificationDetails_CountryOfIssueName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_CountryOfIssue", relatedIdentificationpath) + "_code", ServiceHelper.GetCountryTwoletterCode(identification.IdentificationDetails_CountryOfIssueName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_IssueDate", relatedIdentificationpath), Convert.ToDateTime(identification.IdentificationDetails_IssueDate).ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_ExpiryDate", relatedIdentificationpath), Convert.ToDateTime(identification.IdentificationDetails_ExpiryDate).ToString("yyyy-MM-dd"));
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Address
                            string addresspath = "/XML-Field-Lookups/COMMON/Address-Details";
                            if (item._lst_AddressDetailsModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Address Details", addresspath));
                                foreach (var address in item._lst_AddressDetailsModel)
                                {
                                    string addressTypeText = ServiceHelper.GetName(ValidationHelper.GetString(address.AddressType, ""), Constants.Address_Type);
                                    writer.WriteStartElement("address");
                                    //writer.WriteElementString(GetXmlElementName("AddressType", addresspath), addressTypeText);
                                    writer.WriteElementString(GetXmlElementName("AddressType", addresspath), address.AddressTypeName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressType", addresspath) + "_code", GetLookUpItemCode(address.AddressTypeName, Constants.ADDRESS_TYPE)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine1", addresspath), address.AddressLine1.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine2", addresspath), address.AddressLine2.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PostalCode", addresspath), address.PostalCode.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("POBox", addresspath), address.POBox.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("City", addresspath), address.City.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Country", addresspath), address.CountryName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Country", addresspath) + "_code", ServiceHelper.GetCountryTwoletterCode(address.CountryName)?.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Contact Details
                            string contactPath = "/XML-Field-Lookups/COMMON/Contact-Details";
                            if (item.ContactDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Contact Details", contactPath));
                                writer.WriteElementString(GetXmlElementName("Country_Code_MobileTelNoNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_MobileTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_MobileTelNoNumber", contactPath), item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                //writer.WriteElementString(GetXmlElementName("PrefereredMobileNumber", contactPath), item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("PrefereredMobileNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_MobileTelNoNumber) ? "N" : "Y");
                                writer.WriteElementString(GetXmlElementName("Country_Code_HomeTelNoNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_HomeTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_HomeTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_HomeTelNoNumber", contactPath), item.ContactDetails.ContactDetails_HomeTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_WorkTelNoNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_WorkTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_WorkTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_WorkTelNoNumber", contactPath), item.ContactDetails.ContactDetails_WorkTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_FaxNoFaxNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_FaxNoFaxNumber) ? "" : "+" + item.ContactDetails.Country_Code_FaxNoFaxNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_FaxNoFaxNumber", contactPath), item.ContactDetails.ContactDetails_FaxNoFaxNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_EmailAddress", contactPath), item.ContactDetails.ContactDetails_EmailAddress.Trim().ToUpper());
                                //writer.WriteElementString(GetXmlElementName("ContactDetails_PreferredCommunicationLanguage", contactPath), item.ContactDetails.ContactDetails_PreferredCommunicationLanguage);
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //BusinessProfile

                            string employementPath = "/XML-Field-Lookups/INDIVIDUAL/Employement-Details";
                            if (item.EmploymentDetails != null)
                            {
                                writer.WriteStartElement("businessprofile");
                                writer.WriteStartElement(GetLookupCode("Employement Details", employementPath));
                                //writer.WriteElementString(GetXmlElementName("EmploymentStatus", employementPath), "EMPLOYED");
                                writer.WriteElementString(GetXmlElementName("EmploymentStatus", employementPath), item.EmploymentDetails.EmploymentStatusName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EmploymentStatus", employementPath) + "_code", GetLookUpItemCode(item.EmploymentDetails.EmploymentStatusName, Constants.EMPLOYMENT_STATUS)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Profession", employementPath), item.EmploymentDetails.ProfessionName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Profession", employementPath) + "_code", GetLookUpItemCode(item.EmploymentDetails.ProfessionName, Constants.PROFESSION)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EmployersName", employementPath), item.EmploymentDetails.EmployersName.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //PEP Details
                            ///Pep Related
                            string pepRelatedPath = "/XML-Field-Lookups/COMMON/PEP-Related";
                            writer.WriteStartElement(GetXmlElementName("PepDetailsRelated", pepRelatedPath));
                            writer.WriteElementString(GetXmlElementName("IsRelatedaPep", pepRelatedPath), item.PersonalDetails.IsPepName == "true" ? "YES" : "NO");
                            if (item._lst_PepApplicantViewModel != null && item.PersonalDetails.IsPepName == "true")
                            {
                                foreach (var pepRealted in item._lst_PepApplicantViewModel)
                                {
                                    writer.WriteStartElement(GetLookupCode("PEP Related Party", pepRelatedPath));
                                    writer.WriteElementString(GetXmlElementName("PositionOrganization", pepRelatedPath), pepRealted.PepApplicant_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Country", pepRelatedPath), pepRealted.PepApplicant_CountryName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Country", pepRelatedPath) + "_code", ServiceHelper.GetCountryTwoletterCode(pepRealted.PepApplicant_CountryName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("SinceWhen", pepRelatedPath), Convert.ToDateTime(pepRealted.PepApplicant_Since).ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(GetXmlElementName("UntilWhen", pepRelatedPath), Convert.ToDateTime(pepRealted.PepApplicant_Untill).ToString("yyyy-MM-dd"));
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(GetLookupCode("PEP Related Party", pepRelatedPath));
                                writer.WriteElementString(GetXmlElementName("PositionOrganization", pepRelatedPath), "");
                                writer.WriteElementString(GetXmlElementName("Country", pepRelatedPath), "");
                                writer.WriteElementString(GetXmlElementName("Country", pepRelatedPath) + "_code", "");
                                writer.WriteElementString(GetXmlElementName("SinceWhen", pepRelatedPath), "");
                                writer.WriteElementString(GetXmlElementName("UntilWhen", pepRelatedPath), "");
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            ///Pep Asssociates
                            string pepAssociatesPath = "/XML-Field-Lookups/COMMON/PEP-Family-Associates";
                            writer.WriteStartElement(GetXmlElementName("PepDetailsFamilyMembersAssociates", pepAssociatesPath));
                            writer.WriteElementString(GetXmlElementName("IsFamilyMemberAssociatePep", pepAssociatesPath), item.PersonalDetails.IsRelatedToPepName == "true" ? "YES" : "NO");
                            if (item._lst_PepAssociatesViewModel != null && item.PersonalDetails.IsRelatedToPepName == "true")
                            {
                                foreach (var pepAssociates in item._lst_PepAssociatesViewModel)
                                {
                                    writer.WriteStartElement(GetLookupCode("PEP Family Associates", pepAssociatesPath));
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_FirstName_RP", pepAssociatesPath), pepAssociates.PepAssociates_FirstName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Surname_RP", pepAssociatesPath), pepAssociates.PepAssociates_Surname.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", pepAssociatesPath), pepAssociates.PepAssociates_RelationshipName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", pepAssociatesPath) + "_code", GetLookUpItemCode(pepAssociates.PepAssociates_RelationshipName, Constants.RELATIONSHIPS)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_PositionOrganization", pepAssociatesPath), pepAssociates.PepAssociates_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Country", pepAssociatesPath), pepAssociates.PepAssociates_CountryName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Country", pepAssociatesPath) + "_code", ServiceHelper.GetCountryTwoletterCode(pepAssociates.PepAssociates_CountryName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Since", pepAssociatesPath), Convert.ToDateTime(pepAssociates.PepAssociates_Since).ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Until", pepAssociatesPath), Convert.ToDateTime(pepAssociates.PepAssociates_Until).ToString("yyyy-MM-dd"));
                                    writer.WriteEndElement();
                                }
                            }
                            else
                            {
                                writer.WriteStartElement(GetLookupCode("PEP Family Associates", pepAssociatesPath));
                                writer.WriteElementString(GetXmlElementName("PepAssociates_FirstName_RP", pepAssociatesPath), "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_Surname_RP", pepAssociatesPath), "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", pepAssociatesPath), "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", pepAssociatesPath) + "_code", "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_PositionOrganization", pepAssociatesPath), "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_Country", pepAssociatesPath), "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_Country", pepAssociatesPath) + "_code", "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_Since", pepAssociatesPath), "");
                                writer.WriteElementString(GetXmlElementName("PepAssociates_Until", pepAssociatesPath), "");
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.Flush();
                            //Roles
                            string rolesPath = "/XML-Field-Lookups/COMMON/Roles";
                            if (item.PartyRoles != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Roles", rolesPath));
                                if (item.PartyRoles.RelatedPartyRoles_HasPowerOfAttorney == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", rolesPath), "POWER OF ATTORNEY");
                                    writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode("POWER OF ATTORNEY", Constants.RELATED_PARTY_ROLE)?.Trim().ToUpper());
                                }
                                if (item.PartyRoles.RelatedPartyRoles_IsContactPerson == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", rolesPath), "CONTACT PERSON");
                                    writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode("AUTHORISED CONTACT PERSON", Constants.RELATED_PARTY_ROLE)?.Trim().ToUpper());
                                }
                                //if (item.PartyRoles.RelatedPartyRoles_IsEBankingUser == true)
                                //{
                                //    writer.WriteElementString(GetXmlElementName("Role", rolesPath), "EBANKING USER");
                                //    writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode("DESIGNATED E-BANKING USER", Constants.RELATED_PARTY_ROLE)?.Trim().ToUpper());
                                //}

                                writer.WriteEndElement();
                                writer.Flush();
                            }

                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyPersonDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var educationLevel = ServiceHelper.GetEducationLevel();
            string path = "/XML-Field-Lookups/COMMON/Personal-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    writer.WriteStartElement(GetLookupCode("Personal Details", path));
                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {

                            if (item.PersonalDetails != null)
                            {
                                string countryofBirthName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBirth != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString()).Text : string.Empty;
                                string educationLevelName = (educationLevel != null && educationLevel.Count > 0 && item.PersonalDetails.EducationLevel != null && educationLevel.Any(f => f.Value == item.PersonalDetails.EducationLevel.ToString())) ? educationLevel.FirstOrDefault(f => f.Value == item.PersonalDetails.EducationLevel.ToString()).Text : string.Empty;
                                writer.WriteElementString(GetXmlElementName("Title", path), item.PersonalDetails.Title.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FirstName", path), item.PersonalDetails.FirstName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("LastName", path), item.PersonalDetails.LastName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FathersName", path), item.PersonalDetails.FathersName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Gender", path), item.PersonalDetails.Gender.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("DateOfBirth", path), item.PersonalDetails.DateOfBirth.ToString());
                                writer.WriteElementString(GetXmlElementName("PlaceOfBirth", path), item.PersonalDetails.PlaceOfBirth.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryOfBirth", path), countryofBirthName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EducationLevel", path), educationLevelName.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyIdentificationDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Identifications";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {
                            if (item._lst_IdentificationDetailsViewModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Identifications", path));
                                foreach (var identification in item._lst_IdentificationDetailsViewModel)
                                {
                                    writer.WriteStartElement("Identification");
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_Citizenship", path), identification.IdentificationDetails_CitizenshipName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_TypeOfIdentification", path), identification.IdentificationDetails_TypeOfIdentificationName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_IdentificationNumber", path), identification.IdentificationDetails_IdentificationNumber.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_CountryOfIssue", path), identification.IdentificationDetails_CountryOfIssueName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_IssueDate", path), identification.IdentificationDetails_IssueDate.ToString());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_ExpiryDate", path), identification.IdentificationDetails_ExpiryDate.ToString());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }

                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyAddressDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Address-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {
                            if (item._lst_AddressDetailsModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Address Details", path));
                                foreach (var address in item._lst_AddressDetailsModel)
                                {
                                    writer.WriteStartElement("address");
                                    writer.WriteElementString(GetXmlElementName("AddressType", path), address.AddressTypeName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine1", path), address.AddressLine1.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine2", path), address.AddressLine2.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PostalCode", path), address.PostalCode.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("POBox", path), address.POBox.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("City", path), address.City.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("country", path), address.CountryName.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }

                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyIContactDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Contact-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {
                            if (item.ContactDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Contact Details", path));
                                writer.WriteElementString(GetXmlElementName("Country_Code_MobileTelNoNumber", path), "+" + item.ContactDetails.Country_Code_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_MobileTelNoNumber", path), item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("PrefereredMobileNumber", path), item.ContactDetails.ContactDetails_MobileTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_HomeTelNoNumber", path), item.ContactDetails.Country_Code_HomeTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_HomeTelNoNumber", path), item.ContactDetails.ContactDetails_HomeTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_WorkTelNoNumber", path), item.ContactDetails.Country_Code_WorkTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_WorkTelNoNumber", path), item.ContactDetails.ContactDetails_WorkTelNoNumber);
                                writer.WriteElementString(GetXmlElementName("Country_Code_FaxNoFaxNumber", path), item.ContactDetails.Country_Code_FaxNoFaxNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_FaxNoFaxNumber", path), item.ContactDetails.ContactDetails_FaxNoFaxNumber);
                                writer.WriteElementString(GetXmlElementName("ContactDetails_EmailAddress", path), item.ContactDetails.ContactDetails_EmailAddress.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ContactDetails_PreferredCommunicationLanguage", path), item.ContactDetails.ContactDetails_PreferredCommunicationLanguage);
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyEmployementDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/INDIVIDUAL/Employement-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {
                            if (item.EmploymentDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Employement Details", path));
                                writer.WriteElementString(GetXmlElementName("EmploymentStatus", path), "EMPLOYED");
                                writer.WriteElementString(GetXmlElementName("Profession", path), item.EmploymentDetails.ProfessionName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EmployersName", path), item.EmploymentDetails.EmployersName.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyPepRealtedDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/PEP-Related";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {

                            if (item._lst_PepApplicantViewModel != null)
                            {
                                writer.WriteStartElement(GetXmlElementName("PepDetailsRelated", path));
                                writer.WriteElementString(GetXmlElementName("IsRelatedaPep", path), item.PersonalDetails.IsPep == true ? "YES" : "NO");
                                foreach (var pepRealted in item._lst_PepApplicantViewModel)
                                {
                                    writer.WriteStartElement(GetLookupCode("PEP Related", path));
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_PositionOrganization", path), pepRealted.PepApplicant_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_Country", path), pepRealted.PepApplicant_Country.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_Since", path), pepRealted.PepApplicant_Since.ToString());
                                    writer.WriteElementString(GetXmlElementName("PepApplicant_Untill", path), pepRealted.PepApplicant_Untill.ToString());
                                    writer.WriteEndElement(); ;
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }

                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyPepFamilyAssociatesDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/PEP-Family-Associates";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {

                            if (item._lst_PepAssociatesViewModel != null)
                            {
                                writer.WriteStartElement(GetXmlElementName("PepDetailsFamilyMembersAssociates", path));
                                writer.WriteElementString(GetXmlElementName("IsFamilyMemberAssociatePep", path), item.PersonalDetails.IsRelatedToPep == true ? "YES" : "NO");
                                foreach (var pepAssociates in item._lst_PepAssociatesViewModel)
                                {
                                    writer.WriteStartElement(GetLookupCode("PEP Family Associates", path));
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_FirstName", path), pepAssociates.PepAssociates_FirstName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Surname", path), pepAssociates.PepAssociates_Surname.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", path), pepAssociates.PepAssociates_RelationshipName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_PositionOrganization", path), pepAssociates.PepAssociates_PositionOrganization.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Country", path), pepAssociates.PepAssociates_CountryName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Since", path), pepAssociates.PepAssociates_Since.ToString());
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Until", path), pepAssociates.PepAssociates_Until.ToString());
                                    writer.WriteEndElement(); ;
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }

                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyRolesXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/COMMON/Roles";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {

                    if (application.RelatedParties != null)
                    {
                        foreach (var item in application.RelatedParties)
                        {
                            if (item.PartyRoles != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Roles", path));
                                if (item.PartyRoles.RelatedPartyRoles_HasPowerOfAttorney == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "POWER OF ATTORNEY");
                                }
                                if (item.PartyRoles.RelatedPartyRoles_IsContactPerson == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "CONTACT PERSON");
                                }
                                if (item.PartyRoles.RelatedPartyRoles_IsEBankingUser == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "EBANKING USER");
                                }

                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }

        #endregion
        public static string GetXmlElementName(string elementName, string path)
        {
            string retVal = string.Empty;
            var result = LookupItemProvider.GetLookupItems().WhereContains("NodeAliasPath", path);
            if (result != null)
            {
                retVal = result.Where(x => x.DocumentName.ToUpper() == elementName.ToUpper() && x.GetValue("LookupItemCode") != null).Select(x => x.GetValue("LookupItemCode").ToString()).FirstOrDefault();
            }
            return retVal;
        }

        public static string GetLookUpItemCode(string elementName, string path)
        {
            string retVal = string.Empty;
            if (string.IsNullOrEmpty(elementName))
            {
                return retVal;
            }
            var result = LookupItemProvider.GetLookupItems().WhereContains("NodeAliasPath", path);
            if (result != null)
            {
                //retVal = result.Where(x => string.Equals(x.DocumentName,elementName,StringComparison.OrdinalIgnoreCase) && x.GetValue("LookupItemCode") != null).Select(x => x.GetValue("LookupItemCode").ToString()).FirstOrDefault();
                retVal = result.Where(x => string.Equals(x.GetValue("LookupItemName").ToString(), elementName, StringComparison.OrdinalIgnoreCase) && x.GetValue("LookupItemCode") != null).Select(x => x.GetValue("LookupItemCode").ToString()).FirstOrDefault();
            }
            if (string.IsNullOrEmpty(retVal))
            {
                //retVal = elementName;
                retVal = "MISSING.CODE";
            }
            return retVal;
        }

        public static string GetLookupCode(string elementName, string path)
        {
            string retVal = string.Empty;
            var result = LookupProvider.GetLookups().WhereContains("NodeAliasPath", path);
            if (result != null)
            {
                retVal = result.Where(x => x.DocumentName == elementName && x.GetValue("LookupCode") != null).Select(x => x.GetValue("LookupCode").ToString()).FirstOrDefault();
            }
            return retVal;
        }
        public static Root BindApplicationRootXML(ApplicationModel application)
        {
            Root retVal = new Root();
            if (application != null)
            {
                retVal.Application = new Models.XMLServiceModel.Individual.Application();
                retVal.Application.General = new General();
                //General
                retVal.Application.General.Applicationnumber = application.ApplicationNumber;
                retVal.Application.General.Applicationstatus = "";
                retVal.Application.General.Introducercif = 0;
                retVal.Application.General.Introducername = "";
                retVal.Application.General.SubmittedBy = application.ApplicationDetails.ApplicationDetails_SubmittedBy;
                retVal.Application.General.SubmittedOn = application.ApplicationDetails.ApplicationDetails_SubmittedOn;
                retVal.Application.General.ResponsibleOfficer = application.ApplicationDetails.ApplicationDetails_ResponsibleOfficer;
                //retVal.Application.General.Responsiblebranch =Convert.ToInt32( application.ApplicationDetails.ApplicationDetails_ResponsibleBankingCenter);
                //retVal.Application.General.Applicatonservices.Applicatonservice = null;
                //Purpose and Activity
                //retVal.Application.Purposeandactivity = new Purposeandactivity();
                //retVal.Application.Purposeandactivity.Reasonsforopeningtheaccount = null;
                //retVal.Application.Purposeandactivity.Expectednaturesofincomingandoutgoingtransactions = null;

                Purposeandactivity _Purposeandactivity = new Purposeandactivity();
                #region----- SOURCE OF INCOMING TRANSACTION------
                List<Sourceofincomingtxns> _lstSourceofincomingtxns = new List<Sourceofincomingtxns>();
                foreach (var soit in application.SourceOfIncomingTransactions)
                {
                    Sourceofincomingtxns _Sourceofincomingtxns = new Sourceofincomingtxns();
                    _Sourceofincomingtxns.Countryofremmiter = soit.SourceOfIncomingTransactions_CountryOfRemitter;
                    _Sourceofincomingtxns.Countryofremmiterbank = soit.SourceOfIncomingTransactions_CountryOfRemitterBank;
                    _Sourceofincomingtxns.Nameofremitter = soit.SourceOfIncomingTransactions_NameOfRemitter;
                    _Sourceofincomingtxns.Status = soit.SourceOfIncomingTransactions_Status_Name;
                    _lstSourceofincomingtxns.Add(_Sourceofincomingtxns);
                }

                Sourcesofincomingtxns _Sourcesofincomingtxns = new Sourcesofincomingtxns();
                _Sourcesofincomingtxns.Sourceofincomingtxns = _lstSourceofincomingtxns;
                _Purposeandactivity.Sourcesofincomingtxns = _Sourcesofincomingtxns;
                //retVal.Application.Purposeandactivity = _Purposeandactivity;
                #endregion
                #region----- SOURCE OF OUTGOING TRANSACTION------
                List<Sourceofoutgoingtxns> _lstSourceofoutgoingtxns = new List<Sourceofoutgoingtxns>();
                foreach (var outgoingTrans in application.SourceOfOutgoingTransactions)
                {
                    Sourceofoutgoingtxns _Sourceofoutgoingtxns = new Sourceofoutgoingtxns();
                    _Sourceofoutgoingtxns.Nameofbeneficiary = outgoingTrans.NameOfBeneficiary;
                    _Sourceofoutgoingtxns.Countryofbeneficiary = outgoingTrans.CountryOfBeneficiary;
                    _Sourceofoutgoingtxns.Countryofbeneficiarybank = outgoingTrans.CountryOfBeneficiaryBank;
                    _Sourceofoutgoingtxns.Status = outgoingTrans.StatusName;
                    _lstSourceofoutgoingtxns.Add(_Sourceofoutgoingtxns);
                }
                Sourcesofoutgoingtxns _Sourcesofoutgoingtxns = new Sourcesofoutgoingtxns();
                _Sourcesofoutgoingtxns.Sourceofoutgoingtxns = _lstSourceofoutgoingtxns;
                _Purposeandactivity.Sourcesofoutgoingtxns = _Sourcesofoutgoingtxns;
                retVal.Application.Purposeandactivity = _Purposeandactivity;
                #endregion
                #region------ACCOUNTS------
                List<Account> _lstAccount = new List<Account>();
                foreach (var accounts in application.Accounts)
                {
                    Account _Account = new Account();
                    _Account.Accounttype = accounts.Accounts_AccountTypeName;
                    _Account.Currency = accounts.Accounts_CurrencyName;
                    _Account.Statementoffrequence = accounts.Accounts_StatementFrequencyName;
                    _Account.Status = accounts.Account_Status_Name;
                    _lstAccount.Add(_Account);
                }
                Accountdetails _Accountdetails = new Accountdetails();
                _Accountdetails.Account = _lstAccount;
                retVal.Accountdetails = _Accountdetails;
                #endregion
                #region-----SIGNATURE MANDATE------
                List<Signaturemandate> _lstSignaturemandate = new List<Signaturemandate>();
                foreach (var signmandate in application.SignatureMandates)
                {
                    Signaturemandate _Signaturemandate = new Signaturemandate();
                    //List<Signatorypersons> _lstSignatorypersons = new List<Signatorypersons>();
                    //foreach (var item in signmandate.SignatoryPersonsList)
                    //{
                    //    Signatorypersons _Signatorypersons = new Signatorypersons();
                    //    _Signatorypersons.Signatoryperson = item;
                    //    _lstSignatorypersons.Add(_Signatorypersons);
                    //}
                    //_Signaturemandate.Signatorypersons = _lstSignatorypersons;
                    _Signaturemandate.Signatoryrights = signmandate.AccessRightsName;
                    _Signaturemandate.Totalsignatures = Convert.ToInt32(signmandate.NumberOfSignatures);
                    _Signaturemandate.Limitfrom = Convert.ToInt32(signmandate.AmountFrom);
                    _Signaturemandate.Limitto = Convert.ToInt32(signmandate.AmountTo);
                    _Signaturemandate.Status = signmandate.Status_Name;
                    _lstSignaturemandate.Add(_Signaturemandate);
                }
                Signaturemandates _Signaturemandates = new Signaturemandates();
                _Signaturemandates.Signaturemandate = _lstSignaturemandate;
                retVal.Signaturemandates = _Signaturemandates;
                #endregion
                #region------EBANKING SUBSCRIBER------
                List<Subscriber> _lstSubscriber = new List<Subscriber>();
                foreach (var subscriber in application.EBankingSubscribers)
                {
                    Subscriber _Subscriber = new Subscriber();
                    _Subscriber.Subscribername = subscriber.SubscriberName;
                    _Subscriber.Accesslevel = subscriber.AccessLevelName;
                    _Subscriber.Accesstoallpersonalaccounts = subscriber.AccessToAllPersonalAccountsValue;
                    _Subscriber.Automaticallyaddfuturepersonalaccounts = subscriber.AutomaticallyAddFuturePersonalAccountsValue;
                    _Subscriber.Status = subscriber.Status_Name;
                    _lstSubscriber.Add(_Subscriber);
                }

                Ebankingdetails _Ebankingdetails = new Ebankingdetails();
                _Ebankingdetails.Subscriber = _lstSubscriber;
                retVal.Ebankingdetails = _Ebankingdetails;
                #endregion
                #region----CARDS-------
                List<Card> _lstCard = new List<Card>();
                foreach (var card in application.DebitCards)
                {
                    Card _Card = new Card();
                    _Card.Visadebitcard = card.DebitCardDetails_CardTypeName;
                    _Card.Associatedaccount = card.AssociatedAccountName;
                    // _Card.Associatedaccountccy=
                    _Card.Associatedcardholder = card.DebitCardDetails_CardholderName;
                    _Card.Fullnameoncard = card.DebitCardDetails_FullName;
                    _Card.Mobileforalertscountry = Convert.ToInt32(card.Country_Code);
                    // _Card.Mobileforalertsphone = Convert.ToInt32(card.DebitCardDetails_MobileNumber);
                    _Card.Dispatchmethodofthecard = card.DebitCardDetails_DispatchMethodName;
                    _Card.Deliveryaddress = card.DebitCardDetails_DeliveryAddress;
                    _Card.DeliveryDetails = card.DebitCardDetails_DeliveryDetails;
                    _Card.Collectedbyname = card.DebitCardDetails_CollectedByName;
                    //_Card.Collectedbyid = Convert.ToInt32(card.DebitCardDetails_CollectedBy);
                    _Card.Status = card.DebitCardDetails_StatusName;
                    _lstCard.Add(_Card);
                }
                Carddetails _Carddetails = new Carddetails();
                _Carddetails.Card = _lstCard;
                retVal.Carddetails = _Carddetails;
                #endregion
                #region------EXPECTED DOCUMENTS-----
                List<Bankexpecteddoc> _lstBankexpecteddoc = new List<Bankexpecteddoc>();
                foreach (var doc in application.ExpectedDocuments)
                {
                    Bankexpecteddoc _Bankexpecteddoc = new Bankexpecteddoc();
                    _Bankexpecteddoc.Entitytypename = doc.EntityType;
                    _Bankexpecteddoc.Entitytype = doc.EntityType;
                    _Bankexpecteddoc.Entityrole = doc.EntityRole;
                    _Bankexpecteddoc.Documenttype = doc.DocumentType;
                    //_Bankexpecteddoc.Expecteddoccode = doc.;
                    //_Bankexpecteddoc.Expecteddocprovided = doc.;
                    _Bankexpecteddoc.Status = doc.Status;
                    _lstBankexpecteddoc.Add(_Bankexpecteddoc);
                }
                Bankexpecteddocs _Bankexpecteddocs = new Bankexpecteddocs();
                _Bankexpecteddocs.Bankexpecteddoc = _lstBankexpecteddoc;
                retVal.Bankexpecteddocs = _Bankexpecteddocs;
                #endregion

                #region----APPLICANT-----
                retVal.Applicants = BindApplicant(application);
                #endregion

                #region----RELATED PARTY-----
                retVal.Relatedparties = BindRelatedParty(application);
                #endregion

            }
            return retVal;
        }
        public static Applicants BindApplicant(ApplicationModel application)
        {
            Applicants retval = new Applicants();
            List<Applicant> _lstApplicants = new List<Applicant>();
            foreach (var item in application.Applicants)
            {
                Applicant _Applicant = new Applicant();
                //_Applicant.Customercheck.Customercode = "";
                //_Applicant.Customercheck.Customerexists = "";

                #region----PERSONAL DETAILS-----
                Personal _Personal = new Personal();
                _Personal.Title = item.PersonalDetails.Title;
                _Personal.Firstname = item.PersonalDetails.FirstName;
                _Personal.Lastname = item.PersonalDetails.LastName;
                _Personal.Fathername = item.PersonalDetails.FathersName;
                _Personal.Gender = item.PersonalDetails.Gender;
                _Personal.Dateofbirth = Convert.ToDateTime(item.PersonalDetails.DateOfBirth);
                _Personal.Placeofbirth = item.PersonalDetails.PlaceOfBirth;
                _Personal.Educationlevel = item.PersonalDetails.EducationLevel;
                _Applicant.Personal = _Personal;
                #endregion
                #region----IDENTIFICATION-----
                List<Identification> _lstIdentification = new List<Identification>();
                foreach (var identification in item._lst_IdentificationDetails)
                {
                    Identification _Identification = new Identification();
                    _Identification.Id = identification.IdentificationDetailsID;
                    _Identification.Citizenship = identification.IdentificationDetails_CitizenshipName;
                    _Identification.Typeofidentification = identification.IdentificationDetails_TypeOfIdentificationName;
                    _Identification.Identificationnumber = identification.IdentificationDetails_IdentificationNumber;
                    _Identification.Issuingcountry = identification.IdentificationDetails_CountryOfIssueName;
                    _Identification.Issuedate = Convert.ToDateTime(identification.IdentificationDetails_IssueDate);
                    _Identification.Expirydate = Convert.ToDateTime(identification.IdentificationDetails_ExpiryDate);
                    _lstIdentification.Add(_Identification);
                }
                Identifications _Identifications = new Identifications();
                _Identifications.Identification = _lstIdentification;
                _Applicant.Identifications = _Identifications;
                #endregion
                #region---ADDRESS----
                List<Address> _lstAddress = new List<Address>();
                foreach (var address in item._lst_AddressDetails)
                {
                    Address _Address = new Address();
                    _Address.Addresstype = address.AddressTypeName;
                    _Address.Addressline1 = address.AddressLine1;
                    _Address.Addressline2 = address.AddressLine2;
                    _Address.Postalcode = Convert.ToInt32(address.PostalCode);
                    _Address.Pobox = Convert.ToInt32(address.POBox);
                    _Address.City = address.City;
                    _Address.Country = address.CountryName;
                    _lstAddress.Add(_Address);
                }
                Addresses _Addresses = new Addresses();
                _Addresses.Address = _lstAddress;
                _Applicant.Addresses = _Addresses;
                #endregion
                #region----CONTCAT DETAILS-----
                Contactdetails _Contactdetails = new Contactdetails();
                _Contactdetails.Countrycodemobile = item.ContactDetails.Country_Code_MobileTelNoNumber;
                _Contactdetails.Mobilenumber = item.ContactDetails.ContactDetails_MobileTelNoNumber;
                _Contactdetails.Countrycodehome = item.ContactDetails.Country_Code_HomeTelNoNumber;
                _Contactdetails.Homenumber = item.ContactDetails.ContactDetails_HomeTelNoNumber;
                _Contactdetails.Countrycodework = item.ContactDetails.Country_Code_WorkTelNoNumber;
                _Contactdetails.Worknumber = item.ContactDetails.ContactDetails_WorkTelNoNumber;
                _Contactdetails.Countrycodefax = item.ContactDetails.Country_Code_FaxNoFaxNumber;
                _Contactdetails.Faxnumber = item.ContactDetails.ContactDetails_FaxNoFaxNumber;
                _Contactdetails.Emailaddress = item.ContactDetails.ContactDetails_EmailAddress;
                _Contactdetails.Correspondencelanguage = item.ContactDetails.ContactDetails_PreferredCommunicationLanguage;
                _Applicant.Contactdetails = _Contactdetails;
                #endregion
                #region-----TAX DETAILS-------
                List<Taxdresidency> _lstTaxdresidency = new List<Taxdresidency>();
                foreach (var tax in item._lst_TaxDetails)
                {
                    Taxdresidency _Taxdresidency = new Taxdresidency();
                    _Taxdresidency.Countryoftaxresidence = tax.TaxDetails_CountryOfTaxResidencyName;
                    _Taxdresidency.Taxidentificationnumber = Convert.ToInt32(tax.TaxDetails_TaxIdentificationNumber);
                    _Taxdresidency.Tinunavailablereason = tax.TaxDetails_TinUnavailableReasonName;
                    _Taxdresidency.Justificationfortin = tax.TaxDetails_JustificationForTinUnavalability;
                    _lstTaxdresidency.Add(_Taxdresidency);
                }

                Taxresidencies _Taxresidencies = new Taxresidencies();
                _Taxresidencies.Taxdresidency = _lstTaxdresidency;
                _Applicant.Taxresidencies = _Taxresidencies;
                #endregion
                #region------BUSINESS AND FINANCIAL PROFILE - EMPLOYMENT DETAILS-----
                Businessprofile _Businessprofile = new Businessprofile();
                //Employementdetails _Employementdetails = new Employementdetails();
                //_Employementdetails.Employmentstatus = item.EmploymentDetails.EmploymentStatusName;
                //_Employementdetails.Profession = item.EmploymentDetails.ProfessionName;
                //_Employementdetails.Yearsinbusiness = item.EmploymentDetails.YearsInBusiness;
                //_Employementdetails.Employername = item.EmploymentDetails.EmployersName;
                //_Employementdetails.Natureofbusiness = item.EmploymentDetails.EmployersBusiness;
                //_Businessprofile.Employementdetails = _Employementdetails;

                //Formeremployer _Formeremployer = new Formeremployer();
                //_Formeremployer.Formeremployername = item.EmploymentDetails.FormerEmployersName;
                //_Formeremployer.Formernatureofbusiness = item.EmploymentDetails.FormerEmployersBusiness;
                //_Formeremployer.Formercountryofemployment = item.EmploymentDetails.FormerCountryOfEmploymentName;
                //_Formeremployer.Formerprofession = item.EmploymentDetails.FormerProfessionName;
                //_Businessprofile.Formeremployer = _Formeremployer;

                //_Applicant.Businessprofile.Grossannualsalary=

                List<Originofincome> _lstOriginofincome = new List<Originofincome>();
                foreach (var originOfIncome in item._lst_SourceOfIncomeModel)
                {
                    Originofincome _Originofincome = new Originofincome();
                    _Originofincome.Id = originOfIncome.Id;
                    _Originofincome.Originofannualincome = originOfIncome.SourceOfAnnualIncomeName;
                    _Originofincome.Otheroriginofincome = originOfIncome.SpecifyOtherSource;
                    _Originofincome.Incomeamount = Convert.ToInt32(originOfIncome.AmountOfIncome);
                    _Originofincome.Status = originOfIncome.StatusName;
                    _lstOriginofincome.Add(_Originofincome);
                }
                Originsofincome _Originsofincome = new Originsofincome();
                _Originsofincome.Originofincome = _lstOriginofincome;
                _Businessprofile.Originsofincome = _Originsofincome;

                List<Originoftotalassets> _lstOriginoftotalassets = new List<Originoftotalassets>();
                foreach (var totalAssests in item._lst_OriginOfTotalAssetsModel)
                {
                    Originoftotalassets _Originoftotalassets = new Originoftotalassets();
                    _Originoftotalassets.Id = totalAssests.Id;
                    _Originoftotalassets.Originoftotalasset = totalAssests.OriginOfTotalAssetsName;
                    _Originoftotalassets.Otheroriginoftotalassets = totalAssests.SpecifyOtherOrigin;
                    _Originoftotalassets.Amountoftotalassets = Convert.ToInt32(totalAssests.AmountOfTotalAsset);
                    _Originoftotalassets.Status = totalAssests.StatusName;
                    _lstOriginoftotalassets.Add(_Originoftotalassets);
                }
                Originsoftotalassets _Originsoftotalassets = new Originsoftotalassets();
                _Originsoftotalassets.Originoftotalassets = _lstOriginoftotalassets;
                _Businessprofile.Originsoftotalassets = _Originsoftotalassets;
                _Applicant.Businessprofile = _Businessprofile;
                #endregion
                #region-----PEP DETAILS------
                Pepdetails _Pepdetails = new Pepdetails();
                _Pepdetails.Isapplicantapep = Convert.ToString(item.PersonalDetails.IsPep);

                List<Pepapplicant> _lstPepapplicant = new List<Pepapplicant>();
                foreach (var pepApplicant in item._lst_PepApplicantViewModel)
                {
                    Pepapplicant _Pepapplicant = new Pepapplicant();
                    _Pepapplicant.Id = pepApplicant.PepApplicantID;
                    _Pepapplicant.Positionorganization = pepApplicant.PepApplicant_PositionOrganization;
                    _Pepapplicant.Country = pepApplicant.PepApplicant_CountryName;
                    _Pepapplicant.Sincewhen = pepApplicant.PepApplicant_Since;
                    _Pepapplicant.Untilwhen = pepApplicant.PepApplicant_Untill;
                    _lstPepapplicant.Add(_Pepapplicant);
                }
                _Pepdetails.Pepapplicant = _lstPepapplicant;
                _Pepdetails.Isfamilymemberassociatepepe = Convert.ToString(item.PersonalDetails.IsRelatedToPep);

                List<Pepassociate> _lstPepassociate = new List<Pepassociate>();
                foreach (var pepAssociate in item._lst_PepAssociatesViewModel)
                {
                    Pepassociate _Pepassociate = new Pepassociate();
                    _Pepassociate.Id = pepAssociate.PepAssociatesID;
                    _Pepassociate.Nameofpep = pepAssociate.PepAssociates_FirstName;
                    _Pepassociate.Surnameofpep = pepAssociate.PepAssociates_Surname;
                    _Pepassociate.Relationship = pepAssociate.PepAssociates_RelationshipName;
                    _Pepassociate.Positionorganization = pepAssociate.PepAssociates_PositionOrganization;
                    _Pepassociate.Country = pepAssociate.PepAssociates_CountryName;
                    _Pepassociate.Sincewhen = pepAssociate.PepAssociates_Since;
                    _Pepassociate.Untilwhen = pepAssociate.PepAssociates_Until;
                    _Pepassociate.status = pepAssociate.StatusName;
                    _lstPepassociate.Add(_Pepassociate);
                }
                _Pepdetails.Pepassociate = _lstPepassociate;
                _Applicant.Pepdetails = _Pepdetails;
                #endregion
                #region----BANKING RELATIONSHIP-----
                Bankrelationship _Bankrelationship = new Bankrelationship();
                _Bankrelationship.Nameofeuropeanbankinginstitution = item.PersonalDetails.NameOfBankingInstitution;
                _Bankrelationship.Othereuropeanbankinginstitution = Convert.ToString(item.PersonalDetails.HasAccountInOtherBank);
                _Bankrelationship.Countryofeuropeanbankinginstitution = item.PersonalDetails.CountryOfBankingInstitution;
                _Applicant.Bankrelationship = _Bankrelationship;
                #endregion
                _lstApplicants.Add(_Applicant);
            }
            retval.Applicant = _lstApplicants;

            return retval;
        }

        public static Relatedparties BindRelatedParty(ApplicationModel application)
        {
            Relatedparties retval = new Relatedparties();
            List<Relatedparty> _lstRelatedparty = new List<Relatedparty>();
            foreach (var item in application.RelatedParties)
            {
                Relatedparty _Relatedparty = new Relatedparty();
                // _Relatedparty.Customercheck.Customerexists=item.cu
                //_Relatedparty.Customercheck.Customercode=
                Personal _Personal = new Personal();
                _Personal.Title = item.PersonalDetails.Title;
                _Personal.Firstname = item.PersonalDetails.FirstName;
                _Personal.Lastname = item.PersonalDetails.LastName;
                _Personal.Fathername = item.PersonalDetails.FathersName;
                _Personal.Gender = item.PersonalDetails.Gender;
                _Personal.Dateofbirth = Convert.ToDateTime(item.PersonalDetails.DateOfBirth);
                _Personal.Placeofbirth = item.PersonalDetails.PlaceOfBirth;
                _Personal.Countryofbirth = Convert.ToString(item.PersonalDetails.CountryOfBirth);
                _Personal.Educationlevel = item.PersonalDetails.EducationLevel;
                _Relatedparty.Personal = _Personal;
                #region--------IDENTIFICATION------
                List<Identification> _lstidentification = new List<Identification>();
                foreach (var identification in item._lst_IdentificationDetailsViewModel)
                {
                    Identification _Identification = new Identification();
                    _Identification.Id = identification.IdentificationDetailsID;
                    _Identification.Citizenship = identification.IdentificationDetails_CitizenshipName;
                    _Identification.Typeofidentification = identification.IdentificationDetails_TypeOfIdentificationName;
                    _Identification.Identificationnumber = identification.IdentificationDetails_IdentificationNumber;
                    _Identification.Issuingcountry = identification.IdentificationDetails_CountryOfIssueName;
                    _Identification.Issuedate = Convert.ToDateTime(identification.IdentificationDetails_IssueDate);
                    _Identification.Expirydate = Convert.ToDateTime(identification.IdentificationDetails_ExpiryDate);
                    _lstidentification.Add(_Identification);
                }
                Identifications _Identifications = new Identifications();
                _Identifications.Identification = _lstidentification;
                _Relatedparty.Identifications = _Identifications;
                #endregion

                #region--------ADDRESS------
                List<Address> _lstAddress = new List<Address>();
                foreach (var address in item._lst_AddressDetailsModel)
                {
                    Address _Address = new Address();
                    _Address.Addresstype = address.AddressTypeName;
                    _Address.Addressline1 = address.AddressLine1;
                    _Address.Addressline2 = address.AddressLine2;
                    _Address.Postalcode = Convert.ToInt32(address.PostalCode);
                    _Address.Pobox = Convert.ToInt32(address.POBox);
                    _Address.City = address.City;
                    _Address.Country = address.CountryName;
                    _lstAddress.Add(_Address);
                }
                Addresses _Addresses = new Addresses();
                _Addresses.Address = _lstAddress;
                _Relatedparty.Addresses = _Addresses;
                #endregion

                #region------CONTACT DETAILS------
                Contactdetails _Contactdetails = new Contactdetails();
                _Contactdetails.Countrycodemobile = item.ContactDetails.Country_Code_MobileTelNoNumber;
                _Contactdetails.Mobilenumber = item.ContactDetails.ContactDetails_MobileTelNoNumber;
                _Contactdetails.Countrycodehome = item.ContactDetails.Country_Code_HomeTelNoNumber;
                _Contactdetails.Homenumber = item.ContactDetails.ContactDetails_HomeTelNoNumber;
                _Contactdetails.Countrycodework = item.ContactDetails.Country_Code_WorkTelNoNumber;
                _Contactdetails.Worknumber = item.ContactDetails.ContactDetails_WorkTelNoNumber;
                _Contactdetails.Countrycodefax = item.ContactDetails.Country_Code_FaxNoFaxNumber;
                _Contactdetails.Faxnumber = item.ContactDetails.ContactDetails_FaxNoFaxNumber;
                _Contactdetails.Emailaddress = item.ContactDetails.ContactDetails_EmailAddress;
                _Contactdetails.Correspondencelanguage = item.ContactDetails.ContactDetails_PreferredCommunicationLanguage;
                _Relatedparty.Contactdetails = _Contactdetails;
                #endregion

                #region------BUSINESS AND FINANCIAL PROFILE - EMPLOYMENT DETAILS-----
                //_Relatedparty.Businessprofile.Employementdetails.Employmentstatus = item.EmploymentDetails.EmploymentStatusName;
                //_Relatedparty.Businessprofile.Employementdetails.Profession = item.EmploymentDetails.ProfessionName;
                //_Relatedparty.Businessprofile.Employementdetails.Employername = item.EmploymentDetails.EmployersName;


                //List<Originofincome> _lstOriginofincome = new List<Originofincome>();
                //foreach (var originOfIncome in item._lst_SourceOfIncomeModel)
                //{
                //    Originofincome _Originofincome = new Originofincome();
                //    _Originofincome.Id = originOfIncome.Id;
                //    _Originofincome.Originofannualincome = originOfIncome.SourceOfAnnualIncomeName;
                //    _Originofincome.Otheroriginofincome = originOfIncome.SpecifyOtherSource;
                //    _Originofincome.Incomeamount = Convert.ToInt32(originOfIncome.AmountOfIncome);
                //    _Originofincome.Status = originOfIncome.StatusName;
                //    _lstOriginofincome.Add(_Originofincome);
                //}
                //_Relatedparty.Businessprofile.Originsofincome.Originofincome = _lstOriginofincome;

                //List<Originoftotalassets> _lstOriginoftotalassets = new List<Originoftotalassets>();
                //foreach (var totalAssests in item._lst_OriginOfTotalAssetsModel)
                //{
                //    Originoftotalassets _Originoftotalassets = new Originoftotalassets();
                //    _Originoftotalassets.Id = totalAssests.Id;
                //    _Originoftotalassets.Originoftotalasset = totalAssests.OriginOfTotalAssetsName;
                //    _Originoftotalassets.Otheroriginoftotalassets = totalAssests.SpecifyOtherOrigin;
                //    _Originoftotalassets.Amountoftotalassets = Convert.ToInt32(totalAssests.AmountOfTotalAsset);
                //    _Originoftotalassets.Status = totalAssests.StatusName;
                //    _lstOriginoftotalassets.Add(_Originoftotalassets);
                //}
                //_Relatedparty.Businessprofile.Originsoftotalassets.Originoftotalassets = _lstOriginoftotalassets;
                #endregion

                #region-----PEP DETAILS------
                List<Pepapplicant> _lstPepapplicant = new List<Pepapplicant>();
                foreach (var pepApplicant in item._lst_PepApplicantViewModel)
                {
                    Pepapplicant _Pepapplicant = new Pepapplicant();
                    _Pepapplicant.Id = pepApplicant.PepApplicantID;
                    _Pepapplicant.Positionorganization = pepApplicant.PepApplicant_PositionOrganization;
                    _Pepapplicant.Country = pepApplicant.PepApplicant_CountryName;
                    _Pepapplicant.Sincewhen = pepApplicant.PepApplicant_Since;
                    _Pepapplicant.Untilwhen = pepApplicant.PepApplicant_Untill;
                    _lstPepapplicant.Add(_Pepapplicant);
                }
                Pepdetailsapplicants _Pepdetailsapplicants = new Pepdetailsapplicants();
                _Pepdetailsapplicants.Pepapplicant = _lstPepapplicant;
                _Relatedparty.Pepdetailsapplicants = _Pepdetailsapplicants;



                List<Pepfamilyassociate> _lstPepfamilyassociate = new List<Pepfamilyassociate>();
                foreach (var pepAssociate in item._lst_PepAssociatesViewModel)
                {
                    Pepfamilyassociate _Pepfamilyassociate = new Pepfamilyassociate();
                    _Pepfamilyassociate.Id = pepAssociate.PepAssociatesID;
                    _Pepfamilyassociate.Name = pepAssociate.PepAssociates_FirstName;
                    _Pepfamilyassociate.Surname = pepAssociate.PepAssociates_Surname;
                    _Pepfamilyassociate.Relationship = pepAssociate.PepAssociates_RelationshipName;
                    _Pepfamilyassociate.Positionorganization = pepAssociate.PepAssociates_PositionOrganization;
                    _Pepfamilyassociate.Country = pepAssociate.PepAssociates_CountryName;
                    _Pepfamilyassociate.Sincewhen = pepAssociate.PepAssociates_Since;
                    _Pepfamilyassociate.Untilwhen = pepAssociate.PepAssociates_Until;
                    _lstPepfamilyassociate.Add(_Pepfamilyassociate);
                }

                Pepdetailsfamilymembersassociates _Pepdetailsfamilymembersassociates = new Pepdetailsfamilymembersassociates();
                _Pepdetailsfamilymembersassociates.Pepfamilyassociate = _lstPepfamilyassociate;
                _Relatedparty.Pepdetailsfamilymembersassociates = _Pepdetailsfamilymembersassociates;
                #endregion

                #region-----ROLES------
                // _Relatedparty.Roles.Role = item.PartyRoles.RelatedPartyRoles_HasPowerOfAttorney;
                // List<string> mylist = new List<string>(new string[] { });
                #endregion

            }

            return retval;
        }
    }
}
