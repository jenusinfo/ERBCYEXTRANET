using CMS.DocumentEngine.Types.Eurobank;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;
using CodeBeautify;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.XMLServiceModel.Individual;
using MaxMind.GeoIP2.Model;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eurobank.Helpers.External.SSP.Application.Legal
{
    public class ApplicationLegalService
    {
        public static string GetApplicationLegalElement(int applicationId)
        {
            string result = string.Empty;
            ApplicationModel application = ApplicationsProcess.GetApplicationModelByApplicationIdExtented(applicationId);

            StringBuilder writer = new StringBuilder();
            writer.Append("<root>");

            string applicationDetails = GetApplicationXML(application);
            writer.Append(applicationDetails);
            //string applicationService = GetApplicationServiceXML(application); 
            //writer.Append(applicationService);
            string applicant = GetApplicantXML(application);
            writer.Append(applicant);
            string accountDetails = GetAccountDetailsXML(application);
            writer.Append(accountDetails);
            string eBankingValue = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), null, '\0').Items.Where(x => string.Equals(x.Label, "DIGITAL BANKING", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
            if (application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Contains(eBankingValue))
            {
                string Ebankingdetails = GetEbankingdetailsXML(application);
                writer.Append(Ebankingdetails);
            }
            string EBankingSignatureMandate = GetEBankingSignatureMandateXML(application);
            writer.Append(EBankingSignatureMandate);
            string signatoryGroups = GetSignatoryGroupXML(application);
            writer.Append(signatoryGroups);
            string signatureMandate = GetSignatureMandateXML(application);
            writer.Append(signatureMandate);
            string relatedPartyIndividual = GetRealtedPartyindividualXML(application);
            writer.Append(relatedPartyIndividual);
			//GetRealtedPartyLegalXML code is merged in GetRealtedPartyindividualXML 
            //now there is only one <relatedparties> tag and all individual and legal related party comes under <relatedparty> tag
            //issue number 695
            //Date : 20-July-2023
			//string relatedPartyLegal = GetRealtedPartyLegalXML(application);
			//writer.Append(relatedPartyLegal);

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
            string path = "/XML-Field-Lookups/LEGAL/Application";
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
                        string applicationstatusname = ServiceHelper.GetName(ValidationHelper.GetString(application.ApplicationDetails.ApplicationDetails_ApplicationStatus, ""), Constants.APPLICATION_STATUS);
                        writer.WriteStartElement(GetLookupCode("Application", path));
                        writer.WriteStartElement(GetXmlElementName("General", path));
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationNumber", path), application.ApplicationDetails.ApplicationDetails_ApplicationNumber.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationStatusName", path), applicationstatusname.Trim().ToUpper());
                        //writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationStatusName", path) + "_code", GetLookUpItemCode(applicationstatusname, Constants.APPLICATION_STATUS)?.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationStatusName", path) + "_code", "1");
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_SubmittedBy", path), application.ApplicationDetails.ApplicationDetails_SubmittedBy.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_SubmittedOn", path), Convert.ToDateTime(application.ApplicationDetails.ApplicationDetails_SubmittedOn).ToString("yyyy-MM-dd HH:mm:ss").Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ResponsibleOfficer", path), responsibleofficeName.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicationTypeName", path), "CORPORATE");//application.ApplicationDetails.ApplicationDetails_ApplicationTypeName

                        //Application Service
                        string applicationServicePath = "/XML-Field-Lookups/LEGAL/Application-Service";
                        if (application.ApplicationDetails != null && application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup != null)
                        {
                            writer.WriteStartElement(GetLookupCode("Application Service", applicationServicePath));
                            foreach (var applicationService in application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items)
                            {
                                var selectedValue = application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue;
                                if (selectedValue != null)
                                {
                                    if (selectedValue.Contains(applicationService.Value))
                                    {
                                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicatonServices", applicationServicePath), applicationService.Label?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("ApplicationDetails_ApplicatonServices", applicationServicePath) + "_code", GetLookUpItemCode(applicationService.Label, Constants.APPLICATION_SERVICES)?.Trim().ToUpper());
                                    }
                                }
                            }
                            writer.WriteEndElement();
                            writer.Flush();
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
                        if (application.PurposeAndActivity != null)
                        {
                            writer.WriteStartElement(GetLookupCode("Purpose and Activity", path));
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
                            writer.WriteEndElement();
                            writer.Flush();
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicationServiceXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/Application-Service";
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
        public static string GetAccountDetailsXML(ApplicationModel application)
        {
            string path = "/XML-Field-Lookups/LEGAL/Account-Details";
            string result = string.Empty;
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
                            writer.WriteStartElement(GetXmlElementName("Account", path));


                            writer.WriteElementString(GetXmlElementName("Accounts_AccountType", path), item.Accounts_AccountTypeName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Accounts_AccountType", path) + "_code", GetXmlElementName(item.Accounts_AccountTypeName, Constants.Accounts_AccountType)?.Trim().ToUpper());


                            writer.WriteElementString(GetXmlElementName("Accounts_Currency", path), item.Accounts_CurrencyName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Accounts_Currency", path) + "_code", GetLookUpItemCode(item.Accounts_CurrencyName, Constants.Accounts_Currency)?.Trim().ToUpper());

                            //writer.WriteElementString(GetXmlElementName("Accounts_StatementFrequency", path), item.Accounts_StatementFrequencyName);
                            writer.WriteEndElement();
                            writer.Flush();
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetEbankingdetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/EBankingDetails";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    writer.WriteStartElement(GetLookupCode("EBankingDetails", path));
                    if (application.EBankingSubscribers != null)
                    {
                        foreach (var item in application.EBankingSubscribers)
                        {
                            //string test = item.Id.ToString();
                            writer.WriteStartElement(GetXmlElementName("Subscriber", path));
                            writer.WriteElementString(GetXmlElementName("PartyReference", path), item.PartyReferenceId.ToString()); //item.Id.ToString()
							writer.WriteElementString(GetXmlElementName("SubscriberName", path), item.SubscriberName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("AccessLevel", path), item.AccessLevelName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("AccessLevel", path) + "_code", GetLookUpItemCode(item.AccessLevelName, Constants.Access_Level)?.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("SignatoryGroup", path), item.SignatoryGroup.Trim().ToUpper());
							//writer.WriteElementString(GetXmlElementName("SignatoryGroup", path) + "_code", "GROUPB");
							writer.WriteElementString(GetXmlElementName("SignatoryGroup", path) + "_code", GetLookUpItemCode(item.SignatoryGroup, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
							writer.WriteElementString(GetXmlElementName("AccessToAllPersonalAccounts", path), item.AccessToAllPersonalAccounts == true ? "YES" : "NO");
                            writer.WriteElementString(GetXmlElementName("AutomaticallyAddFuturePersonalAccounts", path), "YES");
                            writer.WriteElementString(GetXmlElementName("AccountProduct", path), "");
                            writer.WriteElementString(GetXmlElementName("AccountCurrency", path), "");
                            writer.WriteElementString(GetXmlElementName("LimitPerTransactionType", path), "ALL");
                            writer.WriteElementString(GetXmlElementName("TypeOfLimit", path), "2");
                            writer.WriteElementString(GetXmlElementName("LimitAmount", path), item.LimitAmountName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("LimitAmount", path) + "_code", GetLookUpItemCode(item.LimitAmountName, Constants.LIMIT_AMOUNT)?.Trim().ToUpper());
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

        public static string GetEBankingSignatureMandateXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/EBankingSignatureMandate";
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
                    writer.WriteStartElement("ebankingsignaturemandates");
                    //if (application.EBankingSubscribers != null)
                    foreach (var itemEB in application.EBankingSubscribers)
                    {
                        if (string.Equals(itemEB.AccessLevelName,"FULL",StringComparison.OrdinalIgnoreCase) || string.Equals(itemEB.AccessLevelName, "AUTHORISE", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var itemSIGN in application.SignatureMandateCompany)
                            {
                                if (string.Equals(itemEB.SignatoryGroup, itemSIGN.AuthorizedSignatoryGroupName, StringComparison.OrdinalIgnoreCase) && string.Equals(itemSIGN.MandateTypeName, "FOR OPERATIONS AND EBANKING", StringComparison.OrdinalIgnoreCase))
                                {
                                    writer.WriteStartElement(GetLookupCode("EBankingSignatureMandate", path));

                                    writer.WriteElementString(GetXmlElementName("PartyReference", path), string.Empty);
                                    writer.WriteElementString(GetXmlElementName("MandateType", path), itemSIGN.MandateTypeName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("MandateType", path) + "_code", GetLookUpItemCode(itemSIGN.MandateTypeName, Constants.MANDATE_TYPE)?.Trim().ToUpper());
                                    //writer.WriteElementString(GetXmlElementName("TransactionType", path), "");
                                    writer.WriteElementString(GetXmlElementName("Description", path), "FOR ALL ACCOUNTS"); //"MANDATE FOR " + itemSIGN.AuthorizedSignatoryGroupName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("LimitFrom", path), itemSIGN.LimitFrom.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("LimitTo", path), itemSIGN.LimitTo.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TotalSignatures", path), itemSIGN.TotalNumberofSignature.ToString().Trim().ToUpper());
                                    //foreach (var item in application.EBankingSubscribers)

                                    if(itemSIGN.AuthorizedSignatoryGroupList != null)
                                    {
                                        foreach (var group in itemSIGN.AuthorizedSignatoryGroupList)
                                        {
                                            writer.WriteStartElement("signatorysets");
                                            writer.WriteStartElement(GetXmlElementName("SignatorySet", path));

                                            writer.WriteStartElement(GetXmlElementName("SignatoryGroups", path));
                                            writer.WriteElementString(GetXmlElementName("SignatoryGroup", path), authorisedGroup.FirstOrDefault(x => x.Value == group).Text.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("SignatoryGroup", path) + "_code", GetLookUpItemCode(authorisedGroup.FirstOrDefault(x => x.Value == group).Text, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("MinnumSignatures", path), itemSIGN.NumberofSignatures.ToString().Trim().ToUpper());
                                            string rightsText = ServiceHelper.GetName(ValidationHelper.GetString(itemSIGN.Rights, ""), Constants.SIGNATURE_RIGHTS);
                                            writer.WriteElementString(GetXmlElementName("Rights", path), rightsText.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("Rights", path) + "_code", GetLookUpItemCode(rightsText, Constants.SIGNATURE_RIGHTS)?.Trim().ToUpper());
                                            writer.WriteEndElement();
                                            if (itemSIGN.AuthorizedSignatoryGroup1List != null)
                                            {
                                                foreach (var group1 in itemSIGN.AuthorizedSignatoryGroup1List)
                                                {
                                                    writer.WriteStartElement(GetXmlElementName("SignatoryGroups", path));
                                                    writer.WriteElementString(GetXmlElementName("SignatoryGroup", path), authorisedGroup.FirstOrDefault(x => x.Value == group1).Text.Trim().ToUpper());
                                                    writer.WriteElementString(GetXmlElementName("SignatoryGroup", path) + "_code", GetLookUpItemCode(authorisedGroup.FirstOrDefault(x => x.Value == group1).Text, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
                                                    writer.WriteElementString(GetXmlElementName("MinnumSignatures", path), itemSIGN.NumberofSignatures1.ToString().Trim().ToUpper());
                                                    writer.WriteEndElement();  
                                                }
                                            }

                                            writer.WriteEndElement();
                                            writer.WriteEndElement(); 
                                        }
                                    }

                                    writer.WriteEndElement(); 
                                } 
                            }
                        }
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetSignatoryGroupXML(ApplicationModel application)
        {
            string result = string.Empty;
            var signatoryPerson = CommonProcess.GetSignatoryPersonLegal(application.ApplicationDetails.ApplicationDetails_ApplicationNumber);
            //var signatoryPerson = CommonProcess.GetCardHolderNameLegal(application.ApplicationDetails.ApplicationDetails_ApplicationNumber);
            string path = "/XML-Field-Lookups/LEGAL/Signatory-Groups";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    writer.WriteStartElement(GetLookupCode("Signatory Groups", path));
                    if (application.SignatoryGroup != null)
                    {
                        foreach (var item in application.SignatoryGroup)
                        {
                            writer.WriteStartElement(GetXmlElementName("SignatoryGroup", path));
                            writer.WriteElementString(GetXmlElementName("SignatoryGroupName", path), item.SignatoryGroup.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("SignatoryGroupName", path) + "_code", GetLookUpItemCode(item.SignatoryGroup, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
                            
                            if (item.SignatoryPersonsList != null)
                            {
                                foreach (var person in item.SignatoryPersonsList)
                                {                                    
                                    string personName = (signatoryPerson != null && signatoryPerson.Count > 0 && person != null && signatoryPerson.Any(f => f.Value == person.ToString())) ? signatoryPerson.FirstOrDefault(f => f.Value == person.ToString()).Text : string.Empty;
                                    writer.WriteStartElement(GetXmlElementName("SignatoryPersons", path));
                                    writer.WriteElementString(GetXmlElementName("PartyReference", path), PersonalDetailsProcess.GetPersonDetailsIdByGuid(person.ToString()).ToString()); //refId.ToString().Trim().ToUpper()
									writer.WriteElementString(GetXmlElementName("SignatoryPerson", path), personName.Trim().ToUpper()); //PersonalDetailsProcess.GetPersonDetailsNameByGuid(person.ToString())
                                    writer.WriteElementString(GetXmlElementName("StartDate", path), DateTime.Now.ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(GetXmlElementName("EndDate", path), "2099-01-01");
                                    writer.WriteEndElement();
                                }
                            }
                            writer.WriteElementString(GetXmlElementName("Description", path), "FOR ALL ACCOUNTS");//"SIGNATORY GROUP FOR " + item.SignatoryGroup.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Description", path) + "_code", GetLookUpItemCode(item.SignatoryGroup, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
                            writer.WriteEndElement();
                            writer.Flush();
                        }
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetSignatureMandateXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/Signature-Mandates";
            var authorisedGroup = SignatoryGroupProcess.GetDDLSignatoryGroupsByApplicationId(application.ApplicationDetails.ApplicationDetailsID);

			XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.SignatureMandateCompany != null)
                    {
                        writer.WriteStartElement(GetLookupCode("Signature Mandates", path));
                        foreach (var item in application.SignatureMandateCompany)
                        {
                            writer.WriteElementString(GetXmlElementName("MandateType", path), item.MandateTypeName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("MandateType", path) + "_code", GetLookUpItemCode(item.MandateTypeName, Constants.MANDATE_TYPE)?.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Description", path), "FOR ALL ACCOUNTS"); // "MANDATE FOR " + item.AuthorizedSignatoryGroupName.Trim().ToUpper());

                            writer.WriteStartElement(GetXmlElementName("SignatureMandate", path));
                            writer.WriteElementString(GetXmlElementName("LimitFrom", path), item.LimitFrom.ToString().Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("LimitTo", path), item.LimitTo.ToString().Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("TotalSignatures", path), item.TotalNumberofSignature.ToString().Trim().ToUpper());

                            writer.WriteStartElement(GetXmlElementName("SignatorySet", path));
                            if (item.AuthorizedSignatoryGroupList != null)
                            {
                                foreach (var group in item.AuthorizedSignatoryGroupList)
                                {
                                    writer.WriteStartElement(GetXmlElementName("SignatoryGroups", path));
                                    string authorisedGroupText = authorisedGroup.FirstOrDefault(x => x.Value == group) != null ? authorisedGroup.FirstOrDefault(x => x.Value == group).Text : string.Empty;
                                    
                                    if(!string.IsNullOrEmpty(authorisedGroupText))
                                    {
										writer.WriteElementString(GetXmlElementName("SignatoryGroup", path), authorisedGroupText.Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("SignatoryGroup", path) + "_code", GetLookUpItemCode(authorisedGroupText, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
									}
									
                                    writer.WriteElementString(GetXmlElementName("MinnumSignatures", path), item.NumberofSignatures.ToString().Trim().ToUpper());
                                    string rightsText = ServiceHelper.GetName(ValidationHelper.GetString(item.Rights, ""), Constants.SIGNATURE_RIGHTS);
                                    writer.WriteElementString(GetXmlElementName("Rights", path), rightsText.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Rights", path) + "_code", GetLookUpItemCode(rightsText, Constants.SIGNATURE_RIGHTS)?.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                            }
                            if (item.AuthorizedSignatoryGroup1List != null)
                            {
                                foreach (var group1 in item.AuthorizedSignatoryGroup1List)
                                {
                                    writer.WriteStartElement(GetXmlElementName("SignatoryGroups", path));
                                    writer.WriteElementString(GetXmlElementName("SignatoryGroup", path), authorisedGroup.FirstOrDefault(x => x.Value == group1).Text.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("SignatoryGroup", path) + "_code", GetLookUpItemCode(authorisedGroup.FirstOrDefault(x => x.Value == group1).Text, Constants.SIGNATORY_GROUP)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("MinnumSignatures", path), item.NumberofSignatures1.ToString().Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                            }
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
        public static string GetGroupStructureXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/Group-Structure";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    if (application.GroupStructureLegalParent != null)
                    {
                        writer.WriteStartElement(GetLookupCode("Group Structure", path));
                        writer.WriteElementString(GetXmlElementName("DoesTheEntityBelongToAGroup", path), application.GroupStructureLegalParent.DoesTheEntityBelongToAGroup == true ? "YES" : "NO");
                        writer.WriteElementString(GetXmlElementName("GroupName", path), application.GroupStructureLegalParent.GroupName.Trim().ToUpper());
                        writer.WriteElementString(GetXmlElementName("GroupActivities", path), application.GroupStructureLegalParent.GroupActivities.Trim().ToUpper());
                        if (application.GroupStructure != null)
                        {
                            foreach (var item in application.GroupStructure)
                            {
                                writer.WriteStartElement(GetXmlElementName("EntityAssociations", path));
                                writer.WriteStartElement(GetXmlElementName("EntityAssociation", path));
                                writer.WriteElementString(GetXmlElementName("EntityName", path), item.EntityName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EntityType", path), item.EntityTypeName.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.WriteEndElement();
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
                    writer.WriteStartElement(GetLookupCode("Source Of Incoming Transaction", path));
                    if (application.SourceOfIncomingTransactions != null)
                    {
                        foreach (var item in application.SourceOfIncomingTransactions)
                        {
                            writer.WriteStartElement(GetLookupCode("Source Of Incoming Transaction", path));
                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_NameOfRemitter", path), item.SourceOfIncomingTransactions_NameOfRemitter.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitter", path), item.SourceOfIncomingTransactions_CountryOfRemitter.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitterBank", path), item.SourceOfIncomingTransactions_CountryOfRemitterBank.Trim().ToUpper());
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
                    writer.WriteStartElement(GetLookupCode("Source Of Outgoing Transaction", path));
                    if (application.SourceOfOutgoingTransactions != null)
                    {
                        foreach (var item in application.SourceOfOutgoingTransactions)
                        {
                            writer.WriteStartElement(GetLookupCode("Source Of Outgoing Transaction", path));
                            writer.WriteElementString(GetXmlElementName("NameOfBeneficiary", path), item.NameOfBeneficiary.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiary", path), item.CountryOfBeneficiary.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiaryBank", path), item.CountryOfBeneficiaryBank.Trim().ToUpper());
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
                        foreach (var item in application.DebitCards)
                        {
                            writer.WriteStartElement(GetLookupCode("Card Details", path));
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardType", path), item.DebitCardDetails_CardTypeName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("AssociatedAccount", path), item.AssociatedAccountName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardholderName", path), item.DebitCardDetails_CardholderName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_FullName", path), item.DebitCardDetails_FullName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("Country_Code", path), item.Country_Code.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_MobileNumber", path), item.DebitCardDetails_MobileNumber.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_DispatchMethod", path), item.DebitCardDetails_DispatchMethodName.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_DeliveryAddress", path), item.DebitCardDetails_DeliveryAddress.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_CollectedBy", path), item.DebitCardDetails_CollectedBy.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_IdentityNumber", path), item.DebitCardDetails_IdentityNumber.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("DebitCardDetails_DeliveryDetails", path), item.DebitCardDetails_DeliveryDetails.Trim().ToUpper());
                            writer.WriteElementString(GetXmlElementName("EmbossingCompanyname", path), string.Empty);
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
        #endregion
        #region---APPLICANT-----
        public static string GetApplicantXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var entityType = ServiceHelper.GetCompanyEntityTypes();
            var listingStatus = ServiceHelper.GetListingStatuses();
            string corporatePath = "/XML-Field-Lookups/COMMON/Company-Details";
            string customerCheckPath = "/XML-Field-Lookups/COMMON/Customer-Check";
            var responsibleBranch = ServiceHelper.GetBankingService();
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
                        writer.WriteStartElement("applicants");
                        foreach (var item in application.Applicants)
                        {
                            writer.WriteStartElement("applicant");
                            if (item.CompanyDetails != null)
                            {
                                //Customer Check

                                writer.WriteStartElement(GetLookupCode("Customer Check", customerCheckPath));
                                writer.WriteElementString(GetXmlElementName("CustomerExists", customerCheckPath), "NO");
                                writer.WriteElementString(GetXmlElementName("CustomerCode", customerCheckPath), item.CompanyDetails.CustomerCIF);
                                writer.WriteEndElement();
                                writer.WriteElementString(GetXmlElementName("Partyreference", customerCheckPath), item.CompanyDetails.Id.ToString());
                                writer.WriteElementString(GetXmlElementName("Introducer CIF", customerCheckPath), application.ApplicationDetails.ApplicationDetails_IntroducerCIF.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("IntroducerName", customerCheckPath), application.ApplicationDetails.ApplicationDetails_IntroducerName.Trim().ToUpper());
                                string Bankunitcode = string.Empty;
                                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(application.ApplicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
                                if (bankUnit != null)
                                {
                                    Bankunitcode = bankUnit.BankUnitCode;
                                }
                                writer.WriteElementString(GetXmlElementName("ResponsibleBranch", customerCheckPath), Bankunitcode);
                                //Company Details
                                writer.WriteStartElement(GetLookupCode("Company Details", corporatePath));
                                string countryofIncorporation = (country != null && country.Count > 0 && item.CompanyDetails.CountryofIncorporation != null && country.Any(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString()).Text : string.Empty;
                                string companyEntityType = (entityType != null && entityType.Count > 0 && item.CompanyDetails.EntityType != null && entityType.Any(f => f.Value == item.CompanyDetails.EntityType.ToString())) ? entityType.FirstOrDefault(f => f.Value == item.CompanyDetails.EntityType.ToString()).Text : string.Empty;
                                string companylistingStatus = (listingStatus != null && listingStatus.Count > 0 && item.CompanyDetails.ListingStatus != null && listingStatus.Any(f => f.Value == item.CompanyDetails.ListingStatus.ToString())) ? listingStatus.FirstOrDefault(f => f.Value == item.CompanyDetails.ListingStatus.ToString()).Text : string.Empty;
                                writer.WriteElementString(GetXmlElementName("RegisteredName", corporatePath), item.CompanyDetails.RegisteredName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("TradingName", corporatePath), item.CompanyDetails.TradingName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EntityType", corporatePath), companyEntityType.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EntityType", corporatePath) + "_code", GetLookUpItemCode(companyEntityType, Constants.COMPANY_ENTITY_TYPE)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryofIncorporation", corporatePath), countryofIncorporation.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryofIncorporation", corporatePath) + "_code", ServiceHelper.GetCountryTwoletterCode(countryofIncorporation)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("RegistrationNumber", corporatePath), item.CompanyDetails.RegistrationNumber.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("DateofIncorporation", corporatePath), Convert.ToDateTime(item.CompanyDetails.DateofIncorporation).ToString("yyyy-MM-dd"));
                                writer.WriteElementString(GetXmlElementName("ListingStatus", corporatePath), companylistingStatus.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ListingStatus", corporatePath) + "_code", GetLookUpItemCode(companylistingStatus, Constants.LISTING_STATUS)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CorporationSharesIssuedToTheBearer", corporatePath), item.CompanyDetails.CorporationSharesIssuedToTheBearer == true ? "YES" : "NO");
                                writer.WriteElementString(GetXmlElementName("IstheEntitylocatedandoperatesanofficeinCyprus", corporatePath), item.CompanyDetails.IsOfficeinCyprusName);
                                writer.WriteElementString(GetXmlElementName("IstheEntitylocatedandoperatesanofficeinCyprus", corporatePath) + "_code", item.CompanyDetails.IsOfficeinCyprusName == "YES" ? "1" : "2");
                                //writer.WriteElementString(GetXmlElementName("CorporationSharesIssuedToTheBearer", corporatePath), item.CompanyDetails.SharesIssuedToTheBearerName);
                                //writer.WriteElementString(GetXmlElementName("IstheEntitylocatedandoperatesanofficeinCyprus", corporatePath), item.CompanyDetails.IsOfficeinCyprusName);
                                writer.WriteEndElement();
                                writer.Flush();
                            }

                            //Group Structure
                            string groupStructurePath = "/XML-Field-Lookups/LEGAL/Group-Structure";
                            if (application.GroupStructureLegalParent != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Group Structure", groupStructurePath));
                                writer.WriteElementString(GetXmlElementName("DoesTheEntityBelongToAGroup", groupStructurePath), application.GroupStructureLegalParent.DoesTheEntityBelongToAGroupName == "true" ? "YES" : "NO");
                                writer.WriteElementString(GetXmlElementName("GroupName", groupStructurePath), application.GroupStructureLegalParent.GroupName == null ? "" : application.GroupStructureLegalParent.GroupName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("GroupActivities", groupStructurePath), application.GroupStructureLegalParent.GroupActivities == null ? "" : application.GroupStructureLegalParent.GroupActivities.Trim().ToUpper());
                                if (application.GroupStructure != null)
                                {
                                    foreach (var structure in application.GroupStructure)
                                    {
                                        writer.WriteStartElement(GetXmlElementName("EntityAssociations", groupStructurePath));
                                        writer.WriteStartElement(GetXmlElementName("EntityAssociation", groupStructurePath));
                                        writer.WriteElementString(GetXmlElementName("EntityName", groupStructurePath), structure.EntityName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("EntityType", groupStructurePath), structure.EntityTypeName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("EntityType", groupStructurePath) + "_code", GetLookUpItemCode(structure.EntityTypeName, Constants.Entity_TYPE_Group_Structure)?.Trim().ToUpper());
                                        writer.WriteEndElement();
                                        writer.WriteEndElement();
                                    }
                                }
                                else
                                {
                                    writer.WriteStartElement(GetXmlElementName("EntityAssociations", groupStructurePath));
                                    writer.WriteStartElement(GetXmlElementName("EntityAssociation", groupStructurePath));
                                    writer.WriteElementString(GetXmlElementName("EntityName", groupStructurePath), "");
                                    writer.WriteElementString(GetXmlElementName("EntityType", groupStructurePath), "");
                                    writer.WriteElementString(GetXmlElementName("EntityType", groupStructurePath) + "_code", "");
                                    writer.WriteEndElement();
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Purpose And Activity
                            string puposeAndActivityPath = "/XML-Field-Lookups/COMMON/Purpose-and-Activity";
                            if (application.ApplicationDetails != null)
                            {
                                if (application.PurposeAndActivity != null)
                                {
                                    writer.WriteStartElement(GetLookupCode("Purpose and Activity", puposeAndActivityPath));
                                    writer.WriteStartElement(GetXmlElementName("ReasonsforOpeningtheAccounts", puposeAndActivityPath));
                                    foreach (var reason in application.PurposeAndActivity.ReasonForOpeningTheAccountGroup.Items)
                                    {
                                        var selectedValue = application.PurposeAndActivity.ReasonForOpeningTheAccountGroup.MultiSelectValue;
                                        if (selectedValue != null)
                                        {
                                            if (selectedValue.Contains(reason.Value))
                                            {
                                                writer.WriteElementString(GetXmlElementName("ReasonsForOpeningTheAccount", puposeAndActivityPath), reason.Text.Trim().ToUpper());
                                                writer.WriteElementString(GetXmlElementName("ReasonsForOpeningTheAccount", puposeAndActivityPath) + "_code", GetLookUpItemCode(reason.Text, Constants.PURPOSE_AND_REASON_FOR_OPENING_THE_ACCOUNT)?.Trim().ToUpper());
                                            }
                                        }
                                    }
                                    writer.WriteEndElement();
                                    writer.WriteStartElement(GetXmlElementName("ExpectedNaturesOfIncomingAndOutgoingTransactions", puposeAndActivityPath));
                                    foreach (var nature in application.PurposeAndActivity.ExpectedNatureOfInAndOutTransactionGroup.Items)
                                    {
                                        var selectedValue = application.PurposeAndActivity.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue;
                                        if (selectedValue != null)
                                        {
                                            if (selectedValue.Contains(nature.Value))
                                            {
                                                writer.WriteElementString(GetXmlElementName("NatureInAndOutTransactions", puposeAndActivityPath), nature.Text.Trim().ToUpper());
                                                writer.WriteElementString(GetXmlElementName("NatureInAndOutTransactions", puposeAndActivityPath) + "_code", GetLookUpItemCode(nature.Text, Constants.EXPECTED_NATURE_INCOMING_OUTGOING_TRANSACTION)?.Trim().ToUpper());
                                            }
                                        }
                                    }
                                    writer.WriteEndElement();
                                    writer.WriteStartElement(GetXmlElementName("ExpectedFrequencyOfIncomingAndOutgoingTransactions", puposeAndActivityPath));
                                    foreach (var frequency in application.PurposeAndActivity.ExpectedFrequencyOfInAndOutTransactionGroup.Items)
                                    {
                                        var selectedValue = application.PurposeAndActivity.ExpectedFrequencyOfInAndOutTransactionGroup.RadioGroupValue;
                                        if (selectedValue != null)
                                        {
                                            if (selectedValue.Contains(frequency.Value))
                                            {
                                                writer.WriteElementString(GetXmlElementName("ExpectedFrequencyOfInAndOuttransactions", puposeAndActivityPath), frequency.Label.Trim().ToUpper());
                                                writer.WriteElementString(GetXmlElementName("ExpectedFrequencyOfInAndOuttransactions", puposeAndActivityPath) + "_code", GetLookUpItemCode(frequency.Label, Constants.EXPECTED_FREQUENCY_INCOMING_OUTGOING_TRANSACTION)?.Trim().ToUpper());
                                            }
                                        }
                                    }
                                    writer.WriteEndElement();
                                    if(!string.IsNullOrEmpty(application.PurposeAndActivity.ExpectedIncomingAmount))
                                    {
                                        writer.WriteElementString(GetXmlElementName("ExpectedIncomingAmount", puposeAndActivityPath), application.PurposeAndActivity.ExpectedIncomingAmount.ToString().Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("NewExpectedIncomingAmount", puposeAndActivityPath), application.PurposeAndActivity.ExpectedIncomingAmount.ToString().Trim().ToUpper());
                                    }

                                    string srcIncTransPath = "/XML-Field-Lookups/COMMON/Source-Of-Incoming-Transaction";
                                    writer.WriteStartElement(GetLookupCode("Source Of Incoming Transaction", srcIncTransPath));
                                    if (application.SourceOfIncomingTransactions != null)
                                    {
                                        foreach (var itemInTran in application.SourceOfIncomingTransactions)
                                        {
                                            writer.WriteStartElement(GetLookupCode("Source Of Incoming Transaction", srcIncTransPath));
                                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_NameOfRemitter", srcIncTransPath), itemInTran.SourceOfIncomingTransactions_NameOfRemitter.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitter", srcIncTransPath), itemInTran.SourceOfIncomingTransactions_CountryOfRemitter.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitter", srcIncTransPath) + "_code", ServiceHelper.GetCountryTwoletterCode(itemInTran.SourceOfIncomingTransactions_CountryOfRemitter)?.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitterBank", srcIncTransPath), itemInTran.SourceOfIncomingTransactions_CountryOfRemitterBank.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("SourceOfIncomingTransactions_CountryOfRemitterBank", srcIncTransPath) + "_code", ServiceHelper.GetCountryTwoletterCode(itemInTran.SourceOfIncomingTransactions_CountryOfRemitterBank)?.Trim().ToUpper());
                                            writer.WriteEndElement();

                                        }
                                    }
                                    writer.WriteEndElement();

                                    string srcOutTransPath = "/XML-Field-Lookups/COMMON/Source-Of-Outgoing-Transaction";
                                    writer.WriteStartElement(GetLookupCode("Source Of Outgoing Transaction", srcOutTransPath));
                                    if (application.SourceOfOutgoingTransactions != null)
                                    {
                                        foreach (var itemOutTran in application.SourceOfOutgoingTransactions)
                                        {
                                            writer.WriteStartElement(GetLookupCode("Source Of Outgoing Transaction", srcOutTransPath));
                                            writer.WriteElementString(GetXmlElementName("NameOfBeneficiary", srcOutTransPath), itemOutTran.NameOfBeneficiary.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiary", srcOutTransPath), itemOutTran.CountryOfBeneficiary.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiary", srcOutTransPath) + "_code", ServiceHelper.GetCountryTwoletterCode(itemOutTran.CountryOfBeneficiary)?.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiaryBank", srcOutTransPath), itemOutTran.CountryOfBeneficiaryBank.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("CountryOfBeneficiaryBank", srcOutTransPath) + "_code", ServiceHelper.GetCountryTwoletterCode(itemOutTran.CountryOfBeneficiaryBank)?.Trim().ToUpper());
                                            writer.WriteEndElement();

                                        }
                                    }
                                    writer.WriteEndElement();


                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                            }
                            //Address
                            string addressPath = "/XML-Field-Lookups/COMMON/Address-Details";
                            if (item._lst_AddressDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Address Details", addressPath));
                                foreach (var address in item._lst_AddressDetails)
                                {
                                    writer.WriteStartElement("address");
                                    writer.WriteElementString(GetXmlElementName("AddressType", addressPath), address.AddressTypeName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressType", addressPath) + "_code", GetLookUpItemCode(address.AddressTypeName, Constants.Address_Type)?.Trim().ToUpper());
                                    //if (address.AddressTypeName.Trim().ToUpper().Contains("REGISTERED"))
                                    //{
                                    //    writer.WriteElementString(GetXmlElementName("MainCorrespondenceAddress", addressPath), "1");
                                    //}
                                    //else
                                    //{
                                    //    writer.WriteElementString(GetXmlElementName("MainCorrespondenceAddress", addressPath), "0");
                                    //}
                                    writer.WriteElementString(GetXmlElementName("MainCorrespondenceAddress", addressPath), address.MainCorrespondenceAddress == true ? "1" : "2");
                                    writer.WriteElementString(GetXmlElementName("AddressLine1", addressPath), address.AddressLine1.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine2", addressPath), address.AddressLine2.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PostalCode", addressPath), address.PostalCode.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("POBox", addressPath), address.POBox.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("City", addressPath), address.City.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Country", addressPath), address.CountryName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Country", addressPath) + "_code", ServiceHelper.GetCountryTwoletterCode(address.CountryName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("CountryCodeWork", addressPath), string.IsNullOrEmpty(address.CountryCode_PhoneNo) ? "" : "+" + address.CountryCode_PhoneNo.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("WorkNumber", addressPath), address.PhoneNo.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("COUNTRYCODEFAX", addressPath), string.IsNullOrEmpty(address.CountryCode_FaxNo) ? "" : "+" + address.CountryCode_FaxNo.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("FAXNUMBER", addressPath), address.FaxNo.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("EmailAddress", addressPath), address.Email.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("NoOfStaff", addressPath), address.NumberOfStaffEmployed.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Fax number
                            if (item._lst_AddressDetails != null)
                            {
                                writer.WriteStartElement(GetXmlElementName("FaxNumbers_Tag", addressPath));
                                foreach (var address in item._lst_AddressDetails)
                                {
                                    if (address.FaxNo != "")
                                    {
                                        writer.WriteStartElement(GetXmlElementName("FaxNumber", addressPath));
                                        writer.WriteElementString(GetXmlElementName("CountryCodeFax", addressPath), string.IsNullOrEmpty(address.CountryCode_FaxNo) ? "" : "+" + address.CountryCode_FaxNo);
                                        writer.WriteElementString(GetXmlElementName("FaxNumber", addressPath), address.FaxNo);
                                        if (address.AddressTypeName.Trim().ToUpper().Contains("REGISTERED"))
                                        {
                                            writer.WriteElementString(GetXmlElementName("FaxType", addressPath), "REGISTER");
                                        }
                                        else if (address.AddressTypeName.Trim().ToUpper().Contains("HEAD OFFICE"))
                                        {
                                            writer.WriteElementString(GetXmlElementName("FaxType", addressPath), "HEADQUARTERS");
                                        }
                                        else if (address.AddressTypeName.Trim().ToUpper().Contains("PRINCIPAL TRADING"))
                                        {
                                            writer.WriteElementString(GetXmlElementName("FaxType", addressPath), "BRANCH");
                                        }
                                        else if (address.AddressTypeName.Trim().ToUpper().Contains("OFFICE IN CYPRUS"))
                                        {
                                            writer.WriteElementString(GetXmlElementName("FaxType", addressPath), "CYPRUSOFFICE");
                                            //writer.WriteElementString(GetXmlElementName("CyprusOffice", addressPath), "OFF.CYP.ADDR");
                                        }
                                        else
                                        {
                                            writer.WriteElementString(GetXmlElementName("FaxType", addressPath), "OTHER");
                                        }

                                        writer.WriteEndElement();
                                    }
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Contact Details
                            var mailingAddress = ServiceHelper.GetPreferred_Mailing_Address();
                            var preferredLanguage = ServiceHelper.GetCommunicationLanguage();
                            string contactDetailspath = "/XML-Field-Lookups/LEGAL/Contact-Details";
                            if (item.ContactDetailsLegal != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Contact Details", contactDetailspath));
                                string mailingAddressText = (mailingAddress != null && mailingAddress.Count > 0 && item.ContactDetailsLegal.ContactDetailsLegal_PreferredMailingAddress != null && mailingAddress.Any(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredMailingAddress.ToString())) ? mailingAddress.FirstOrDefault(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredMailingAddress.ToString()).Text : string.Empty;
                                string preferredLanguageText = (preferredLanguage != null && preferredLanguage.Count > 0 && item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage != null && preferredLanguage.Any(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage.ToString())) ? preferredLanguage.FirstOrDefault(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage.ToString()).Text : string.Empty;
                                writer.WriteStartElement(GetXmlElementName("EmailAddresseDetails", contactDetailspath));
                                writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_EmailAddressForSendingAlerts", contactDetailspath), item.ContactDetailsLegal.ContactDetailsLegal_EmailAddressForSendingAlerts.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_PreferredMailingAddress", contactDetailspath), "Y"); //item.ContactDetailsLegal.ContactDetailsLegal_PreferredMailingAddress
                                writer.WriteEndElement();

                                if (item._lst_AddressDetails != null)
                                {
                                    foreach (var address in item._lst_AddressDetails)
                                    {
                                        if (address.Email.ToString() != "")
                                        {
                                            writer.WriteStartElement(GetXmlElementName("EmailAddresseDetails", contactDetailspath));
                                            writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_EmailAddressForSendingAlerts", contactDetailspath), address.Email.Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_PreferredMailingAddress", contactDetailspath), "N");
                                            writer.WriteEndElement();
                                        }
                                    }
                                }

                                writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_PreferredCommunicationLanguage", contactDetailspath), item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_PreferredCommunicationLanguage", contactDetailspath) + "_code", GetLookUpItemCode(item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage, Constants.COMMUNICATION_LANGUAGE)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Business Profile
                            string businessProfilePath = "/XML-Field-Lookups/LEGAL/Business-Profile";
                            if (item.CompanyBusinessProfile != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Business Profile", businessProfilePath));
                                string countryofOriginofWealthActivities = (country != null && country.Count > 0 && item.CompanyBusinessProfile.CountryofOriginofWealthActivities != null && country.Any(f => f.Value == item.CompanyBusinessProfile.CountryofOriginofWealthActivities.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyBusinessProfile.CountryofOriginofWealthActivities.ToString()).Text : string.Empty;

                                writer.WriteElementString(GetXmlElementName("MainBusinessActivities", businessProfilePath), item.CompanyBusinessProfile.MainBusinessActivities.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("NumberofYearsinOperation", businessProfilePath), item.CompanyBusinessProfile.NumberofYearsinOperation.ToString().Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("NumberofEmployes", businessProfilePath), item.CompanyBusinessProfile.NumberofEmployes.ToString().Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("WebsiteAddress", businessProfilePath), item.CompanyBusinessProfile.WebsiteAddress.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CorporationIsengagedInTheProvision", businessProfilePath), string.Equals(item.CompanyBusinessProfile.CorporationIsengagedInTheProvisionName,"true", StringComparison.OrdinalIgnoreCase) ? "YES" : "NO");
                                writer.WriteElementString(GetXmlElementName("IssuingAuthority", businessProfilePath), item.CompanyBusinessProfile.IssuingAuthority.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("BusinessLegalID", businessProfilePath), "");// item.CompanyDetails.RegistrationNumber.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EconomicSectorIndustry", businessProfilePath), item.CompanyBusinessProfile.EconomicSectorIndustryName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EconomicSectorIndustry", businessProfilePath) + "_code", GetLookUpItemCode(item.CompanyBusinessProfile.EconomicSectorIndustryName, Constants.ECONOMIC_SECTOR_INDUSTRIES)?.Trim().ToUpper());
                                writer.WriteStartElement(GetXmlElementName("CountriesofOriginofWealthActivities", businessProfilePath));
                                if (item.CompanyBusinessProfile.CountryofOriginofWealthActivitiesValues != null && item.CompanyBusinessProfile.CountryofOriginofWealthActivitiesValues.Any())
                                {                                    
                                    foreach (var countrycode in item.CompanyBusinessProfile.CountryofOriginofWealthActivitiesValues)
                                    {
                                        writer.WriteElementString(GetXmlElementName("CountryofOriginofWealthActivities", businessProfilePath), ServiceHelper.GetCountryNameById(Convert.ToInt32(countrycode))?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("CountryofOriginofWealthActivities", businessProfilePath) + "_code", ServiceHelper.GetCountryTwoletterCode(ServiceHelper.GetCountryNameById(Convert.ToInt32(countrycode)))?.Trim().ToUpper());                                        
                                    }                                    
                                }
                                else
                                {
                                    writer.WriteElementString(GetXmlElementName("CountryofOriginofWealthActivities", businessProfilePath), "");
                                    writer.WriteElementString(GetXmlElementName("CountryofOriginofWealthActivities", businessProfilePath) + "_code", "");
                                }
                                writer.WriteEndElement();

                                writer.WriteElementString(GetXmlElementName("SupplierofGovernmentalBodies", businessProfilePath), "");
                                writer.WriteElementString(GetXmlElementName("GovernmentalEntity", businessProfilePath), "");
                                writer.WriteElementString(GetXmlElementName("SponsoringEntityName", businessProfilePath), "");
                                writer.WriteElementString(GetXmlElementName("SponsoringEntityBusiness", businessProfilePath), "");
                                writer.WriteElementString(GetXmlElementName("SponsoringEntityWebsite", businessProfilePath), "");
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Financial Profile
                            string financialProfilePath = "/XML-Field-Lookups/LEGAL/Financial-Profile";

                            writer.WriteStartElement(GetLookupCode("Financial Profile", financialProfilePath));
                            if (item.CompanyFinancialInformation != null)
                            {
                                writer.WriteElementString(GetXmlElementName("Turnover", financialProfilePath), item.CompanyFinancialInformation.Turnover.ToString().Trim());
                                writer.WriteElementString(GetXmlElementName("NetProfitAndLoss", financialProfilePath), item.CompanyFinancialInformation.NetProfitLoss.ToString().Trim());
                                writer.WriteElementString(GetXmlElementName("TotalAssets", financialProfilePath), item.CompanyFinancialInformation.TotalAssets.ToString().Trim());
                            }
                            string assetsPath = "/XML-Field-Lookups/COMMON/Origin-Of-Total-Assets";
                            if (item._lst_OriginOfTotalAssetsModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Origin Of Total Assets", assetsPath));
                                foreach (var assets in item._lst_OriginOfTotalAssetsModel)
                                {
                                    string originOfTotalAssetsText = ServiceHelper.GetName(ValidationHelper.GetString(assets.OriginOfTotalAssets, ""), Constants.ORIGIN_OF_TOTAL_ASSETS);
                                    writer.WriteStartElement(GetLookupCode("Origin Of Total Assets", assetsPath));
                                    writer.WriteElementString(GetXmlElementName("OriginOfTotalAssets", assetsPath), originOfTotalAssetsText.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("OriginOfTotalAssets", assetsPath) + "_code", GetLookUpItemCode(originOfTotalAssetsText, Constants.ORIGIN_OF_TOTAL_ASSETS)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("SpecifyOtherOrigin", assetsPath), assets.SpecifyOtherOrigin.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AmountOfTotalAsset", assetsPath), assets.AmountOfTotalAsset.ToString());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.Flush();
                            //Financial Profile END
                            //Tax Details
                            string taxDetailsPath = "/XML-Field-Lookups/COMMON/Tax-Details";
                            if (item._lst_TaxDetails != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Tax Details", taxDetailsPath));
                                writer.WriteElementString(GetXmlElementName("PayDefenceTax", taxDetailsPath), string.Equals(item.CompanyDetails.IsLiableToPayDefenseTaxInCyprusName, "true", StringComparison.OrdinalIgnoreCase) ? "YES" : "NO");
                                foreach (var tax in item._lst_TaxDetails)
                                {
                                    writer.WriteStartElement("taxresidency");
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_CountryOfTaxResidency", taxDetailsPath), tax.TaxDetails_CountryOfTaxResidencyName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_CountryOfTaxResidency", taxDetailsPath) + "_code", ServiceHelper.GetCountryTwoletterCode(tax.TaxDetails_CountryOfTaxResidencyName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_TaxIdentificationNumber", taxDetailsPath), tax.TaxDetails_TaxIdentificationNumber.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_TinUnavailableReason", taxDetailsPath), tax.TaxDetails_TinUnavailableReasonName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_TinUnavailableReason", taxDetailsPath) + "_code", GetLookUpItemCode(tax.TaxDetails_TinUnavailableReasonName, Constants.TIN_UNAVAILABLE_REASON)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("TaxDetails_JustificationForTinUnavalability", taxDetailsPath), tax.TaxDetails_JustificationForTinUnavalability.Trim().ToUpper());
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //FATCA Details

                            string fatcaPath = "/XML-Field-Lookups/LEGAL/Fatca-Details";
                            if (item.FATCACRSDetails != null)
                            {
                                string oafatcaclassification = "";
                                if(!string.IsNullOrEmpty(item.FATCACRSDetails.FATCADetails_USEtityType))
                                {
                                    oafatcaclassification = GetLookUpItemCode(item.FATCACRSDetails.FATCADetails_USEtityType, Constants.US_ENTITY_TYPE)?.Trim().ToUpper();
                                }
                                else if (!string.IsNullOrEmpty(item.FATCACRSDetails.FATCADetails_TypeofForeignFinancialInstitution))
                                {
                                    oafatcaclassification = GetLookUpItemCode(item.FATCACRSDetails.FATCADetails_TypeofForeignFinancialInstitution, Constants.TYPE_OF_FOREIGN_FINANCIAL_INSTITUTION)?.Trim().ToUpper();
                                }
                                else if(!string.IsNullOrEmpty(item.FATCACRSDetails.FATCADetails_TypeofNonFinancialForeignEntity))
                                {
                                    oafatcaclassification = GetLookUpItemCode(item.FATCACRSDetails.FATCADetails_TypeofNonFinancialForeignEntity, Constants.TYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE)?.Trim().ToUpper();
                                }
                                writer.WriteStartElement(GetLookupCode("Fatca Details", fatcaPath));
                                writer.WriteElementString(GetXmlElementName("FATCADetails_FATCAClassification", fatcaPath), item.FATCACRSDetails.FATCADetails_FATCAClassification.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_FATCAClassification", fatcaPath) + "_code", GetLookUpItemCode(item.FATCACRSDetails.FATCADetails_FATCAClassification, Constants.FATCA_CLASSIFICATION)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_USEtityType", fatcaPath), item.FATCACRSDetails.FATCADetails_USEtityType.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_USEtityType", fatcaPath) + "_code", GetLookUpItemCode(item.FATCACRSDetails.FATCADetails_USEtityType, Constants.US_ENTITY_TYPE)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_TypeofForeignFinancialInstitution", fatcaPath), item.FATCACRSDetails.FATCADetails_TypeofForeignFinancialInstitution.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_TypeofForeignFinancialInstitution", fatcaPath) + "_code", GetLookUpItemCode(item.FATCACRSDetails.FATCADetails_TypeofForeignFinancialInstitution, Constants.TYPE_OF_FOREIGN_FINANCIAL_INSTITUTION)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_TypeofNonFinancialForeignEntity", fatcaPath), item.FATCACRSDetails.FATCADetails_TypeofNonFinancialForeignEntity.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_TypeofNonFinancialForeignEntity", fatcaPath) + "_code", GetLookUpItemCode(item.FATCACRSDetails.FATCADetails_TypeofNonFinancialForeignEntity, Constants.TYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADETAILS_OAFatcaClassification", fatcaPath) + "_code", oafatcaclassification);
                                writer.WriteElementString(GetXmlElementName("FATCADetails_GlobalIntermediaryIdentificationNumber", fatcaPath), item.FATCACRSDetails.FATCADetails_GlobalIntermediaryIdentificationNumber.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ExemptionReason", fatcaPath), item.FATCACRSDetails.ExemptionReason.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //CRS Details
                            string crsPath = "/XML-Field-Lookups/LEGAL/Crs-Details";
                            if (item.CRSDetails != null)
                            {
                                var allTypeOfFinancialInstitution = ServiceHelper.GetTypeofFinancialInstitution();
                                string typeOfFinancialInstitution = (allTypeOfFinancialInstitution != null && allTypeOfFinancialInstitution.Count > 0 && !string.IsNullOrEmpty(item.CRSDetails.CompanyCRSDetails_TypeofFinancialInstitution)) ? allTypeOfFinancialInstitution.FirstOrDefault(f => f.Text.Contains(item.CRSDetails.CompanyCRSDetails_TypeofFinancialInstitution.ToUpper()))?.Text : string.Empty;
                                writer.WriteStartElement(GetLookupCode("Crs Details", crsPath));
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_CRSClassification", crsPath), item.CRSDetails.CompanyCRSDetails_CRSClassification.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_CRSClassification", crsPath) + "_code", GetLookUpItemCode(item.CRSDetails.CompanyCRSDetails_CRSClassification, Constants.CRS_CLASSIFICATION)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_TypeofActiveNonFinancialEntity", crsPath), item.CRSDetails.CompanyCRSDetails_TypeofActiveNonFinancialEntity.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_TypeofActiveNonFinancialEntity", crsPath) + "_code", GetLookUpItemCode(item.CRSDetails.CompanyCRSDetails_TypeofActiveNonFinancialEntity, Constants.TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE)?.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_NameofEstablishedSecuritiesMarket", crsPath), item.CRSDetails.CompanyCRSDetails_NameofEstablishedSecuritiesMarket.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_TypeofFinancialInstitution", crsPath), typeOfFinancialInstitution.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_TypeofFinancialInstitution", crsPath) + "_code", GetLookUpItemCode(typeOfFinancialInstitution, Constants.TYPE_OF_FINANCIAL_INSITUTION)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Bank Relationship
                            string bankRealtionshipPath = "/XML-Field-Lookups/COMMON/Bank-Relationship";
                            if (item.CompanyDetails != null)
                            {
                                string countryName = (country != null && country.Count > 0 && item.CompanyDetails.CountryOfBankingInstitution != null && country.Any(f => f.Value == item.CompanyDetails.CountryOfBankingInstitution.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyDetails.CountryOfBankingInstitution.ToString()).Text : string.Empty;
                                writer.WriteStartElement(GetLookupCode("Bank Relationship", bankRealtionshipPath));
                                writer.WriteElementString(GetXmlElementName("HasAccountInOtherBank", bankRealtionshipPath), item.CompanyDetails.HasAccountInOtherBankName == "true" ? "YES" : "NO");
                                writer.WriteElementString(GetXmlElementName("HasAccountInOtherBank", bankRealtionshipPath) + "_code", item.CompanyDetails.HasAccountInOtherBankName == "true" ? "1" : "2");
                                writer.WriteElementString(GetXmlElementName("NameOfBankingInstitution", bankRealtionshipPath), item.CompanyDetails.NameOfBankingInstitution.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryOfBankingInstitution", bankRealtionshipPath), countryName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryOfBankingInstitution", bankRealtionshipPath) + "_code", ServiceHelper.GetCountryTwoletterCode(countryName)?.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            //Card Details
                            string cardValue = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), null, '\0').Items.Where(x => string.Equals(x.Label, "CARD", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
                            if (application.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Contains(cardValue))
                            {
                                string cardPath = "/XML-Field-Lookups/COMMON/Card-Details";
                                if (application.DebitCards != null)
                                {
                                    writer.WriteStartElement("carddetails");
                                    foreach (var card in application.DebitCards)
                                    {
                                        var debitCardAccounts = AccountsProcess.GetDebitCardAccountsByApplicationID(application.ApplicationDetails.ApplicationDetailsID);
                                        string associatedAccountName = string.Empty;
                                        if (debitCardAccounts != null && debitCardAccounts.Count > 0)
                                        {
                                            var accountName = debitCardAccounts.Where(x => x.Value == card.AssociatedAccount).FirstOrDefault();
                                            if (accountName != null)
                                            {
                                                associatedAccountName = accountName.Text;
                                            }
                                        }
                                        //string associatedAccountName = (debitCardAccounts != null && debitCardAccounts.Count > 0 && card.AssociatedAccount != null) ? debitCardAccounts.FirstOrDefault(f => f.Value == card.AssociatedAccount.ToString()).Text : string.Empty;
                                        string associatedAccount = ServiceHelper.GetName(ValidationHelper.GetString(card.AssociatedAccount, ""), Constants.Associated_Account_Type);
                                        var alldispatchLegal = ServiceHelper.GetDispatchMethod();
                                        string dispatchMethod = (alldispatchLegal != null && alldispatchLegal.Count > 0 && card.DebitCardDetails_DispatchMethodName != null && alldispatchLegal.Any(f => f.Value == card.DebitCardDetails_DispatchMethodName.ToString())) ? alldispatchLegal.FirstOrDefault(f => f.Value == card.DebitCardDetails_DispatchMethodName.ToString()).Text : string.Empty;
										//string dispatchMethod = ServiceHelper.GetName(ValidationHelper.GetString(card.DebitCardDetails_DispatchMethodName, ""), Constants.DISPATCH_METHOD);
										//string deliveryAddress = ServiceHelper.GetName(ValidationHelper.GetString(card.DebitCardDetails_DeliveryAddress, ""), Constants.ADDRESS_TYPE);
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
										writer.WriteStartElement(GetLookupCode("Card Details", cardPath));
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardType", cardPath), card.DebitCardDetails_CardTypeName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardType", cardPath) + "_code", associatedAccountName != "" && associatedAccountName != null ? "CLASSIC.DEBIT." + associatedAccountName.Substring(Math.Max(0, associatedAccountName.Length - 3)).Trim().ToUpper() : ""); //GetLookUpItemCode(card.DebitCardDetails_CardTypeName, Constants.CARD_TYPE)?.Trim().ToUpper()
                                        if (!string.IsNullOrEmpty(associatedAccountName))
                                        {
                                            writer.WriteElementString(GetXmlElementName("AssociatedAccount", cardPath), associatedAccountName.Substring(0, associatedAccountName.Length - 7).Trim().ToUpper());
                                            writer.WriteElementString(GetXmlElementName("AssociatedAccount", cardPath) + "_code", GetLookUpItemCode(associatedAccountName.Substring(0, associatedAccountName.Length - 6), Constants.Accounts_AccountType)?.Trim().ToUpper());
                                        }
                                        else
                                        {
                                            writer.WriteElementString(GetXmlElementName("AssociatedAccount", cardPath),"");
                                            writer.WriteElementString(GetXmlElementName("AssociatedAccount", cardPath) + "_code", "");
                                        }
                                        
                                        writer.WriteElementString("associatedaccountccy", associatedAccountName != "" && associatedAccountName != null ? associatedAccountName.Substring(Math.Max(0, associatedAccountName.Length - 3)).Trim().ToUpper() : "");
                                        writer.WriteElementString("associatedaccountccy" + "_code", associatedAccountName != "" && associatedAccountName != null ? GetLookUpItemCode(associatedAccountName.Substring(Math.Max(0, associatedAccountName.Length - 3)), Constants.Accounts_Currency)?.Trim().ToUpper() : "");
                                        //writer.WriteElementString(GetXmlElementName("DebitCardDetails_CardholderName", cardPath), card.DebitCardDetails_CardholderName););
                                        writer.WriteElementString(GetXmlElementName("PartyReference", cardPath), PersonalDetailsProcess.GetPersonDetailsIdByGuid(card.DebitCardDetails_CardholderName).ToString());
                                        writer.WriteElementString(GetXmlElementName("AssociatedCardHolder", cardPath), ServiceHelper.GetName(card.DebitCardDetails_CardholderName));
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_FullName", cardPath), card.DebitCardDetails_FullName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("Country_Code", cardPath), string.IsNullOrEmpty(card.Country_Code) ? "" : "+" + card.Country_Code.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_MobileNumber", cardPath), card.DebitCardDetails_MobileNumber.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_DispatchMethod", cardPath), dispatchMethod.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_DispatchMethod", cardPath) + "_code", GetLookUpItemCode(dispatchMethod, Constants.DISPATCH_METHOD)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_DeliveryAddress", cardPath), deliveryAddress.Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("DebitCardDetails_OtherDeliveryAddress", cardPath), deliveryAddress.ToUpper().Contains("OTHER") ? card.DebitCardDetails_OtherDeliveryAddress?.Trim().ToUpper() : "");
										writer.WriteElementString(GetXmlElementName("DebitCardDetails_CollectedBy", cardPath), card.DebitCardDetails_CollectedBy.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_IdentityNumber", cardPath), card.DebitCardDetails_IdentityNumber.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("DebitCardDetails_DeliveryDetails", cardPath), card.DebitCardDetails_DeliveryDetails.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("EmbossingCompanyname", cardPath), card.DebitCardDetails_CompanyNameAppearOnCard.Trim().ToUpper());
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                            }
                            //E-banking Details

                            //End of applicant
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetApplicantCompanyDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var entityType = ServiceHelper.GetCompanyEntityTypes();
            var listingStatus = ServiceHelper.GetListingStatuses();
            string path = "/XML-Field-Lookups/COMMON/Company-Details";
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter, writerSettings))
                {
                    writer.WriteStartElement(GetLookupCode("corporatedetails", path));
                    if (application.Applicants != null)
                    {
                        foreach (var item in application.Applicants)
                        {
                            if (item.CompanyDetails != null)
                            {
                                string countryofIncorporation = (country != null && country.Count > 0 && item.CompanyDetails.CountryofIncorporation != null && country.Any(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString()).Text : string.Empty;
                                string companyEntityType = (entityType != null && entityType.Count > 0 && item.CompanyDetails.EntityType != null && entityType.Any(f => f.Value == item.CompanyDetails.EntityType.ToString())) ? entityType.FirstOrDefault(f => f.Value == item.CompanyDetails.EntityType.ToString()).Text : string.Empty;
                                string companylistingStatus = (listingStatus != null && listingStatus.Count > 0 && item.CompanyDetails.ListingStatus != null && listingStatus.Any(f => f.Value == item.CompanyDetails.ListingStatus.ToString())) ? listingStatus.FirstOrDefault(f => f.Value == item.CompanyDetails.ListingStatus.ToString()).Text : string.Empty;
                                writer.WriteElementString(GetXmlElementName("RegisteredName", path), item.CompanyDetails.RegisteredName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("TradingName", path), item.CompanyDetails.TradingName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EntityType", path), companyEntityType.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryofIncorporation", path), countryofIncorporation.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("RegistrationNumber", path), item.CompanyDetails.RegistrationNumber.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("DateofIncorporation", path), Convert.ToDateTime(item.CompanyDetails.DateofIncorporation).ToString("yyyy-MM-dd"));
                                writer.WriteElementString(GetXmlElementName("ListingStatus", path), companylistingStatus.Trim().ToUpper());
                                //writer.WriteElementString(GetXmlElementName("CorporationSharesIssuedToTheBearer", path), item.CompanyDetails.CorporationSharesIssuedToTheBearer == true ? "YES" : "NO");
                                //writer.WriteElementString(GetXmlElementName("IstheEntitylocatedandoperatesanofficeinCyprus", path), item.CompanyDetails.IstheEntitylocatedandoperatesanofficeinCyprus == true ? "YES" : "NO");
                                writer.WriteElementString(GetXmlElementName("CorporationSharesIssuedToTheBearer", path), item.CompanyDetails.SharesIssuedToTheBearerName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("IstheEntitylocatedandoperatesanofficeinCyprus", path), item.CompanyDetails.IsOfficeinCyprusName.Trim().ToUpper());
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
                                    //writer.WriteElementString(GetXmlElementName("MainCorrespondenceAddress", path), address.MainCorrespondenceAddress == true ? "YES" : "NO");
                                    writer.WriteElementString(GetXmlElementName("MainCorrespondenceAddress", path), address.MainCorrespondenceAddress == true ? "1" : "2");
                                    writer.WriteElementString(GetXmlElementName("AddressLine1", path), address.AddressLine1.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine2", path), address.AddressLine2.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("PostalCode", path), address.PostalCode.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("POBox", path), address.POBox.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("City", path), address.City.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("country", path), address.CountryName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("CountryCodeWork", path), address.CountryCode_PhoneNo.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("WorkNumber", path), address.PhoneNo);
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
        public static string GetApplicantIAddressFaxDetailsXML(ApplicationModel application)
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
                                writer.WriteStartElement(GetXmlElementName("FaxNumbers_Tag", path));
                                foreach (var address in item._lst_AddressDetails)
                                {
                                    writer.WriteStartElement(GetXmlElementName("FaxNumber", path));
                                    writer.WriteElementString(GetXmlElementName("CountryCodeFax", path), address.CountryCode_FaxNo);
                                    writer.WriteElementString(GetXmlElementName("FaxNumber", path), address.FaxNo);
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
            var mailingAddress = ServiceHelper.GetPreferred_Mailing_Address();
            var preferredLanguage = ServiceHelper.GetCommunicationLanguage();
            string path = "/XML-Field-Lookups/LEGAL/Contact-Details";
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
                            if (item.ContactDetailsLegal != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Contact Details", path));
                                string mailingAddressText = (mailingAddress != null && mailingAddress.Count > 0 && item.ContactDetailsLegal.ContactDetailsLegal_PreferredMailingAddress != null && mailingAddress.Any(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredMailingAddress.ToString())) ? mailingAddress.FirstOrDefault(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredMailingAddress.ToString()).Text : string.Empty;
                                string preferredLanguageText = (preferredLanguage != null && preferredLanguage.Count > 0 && item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage != null && preferredLanguage.Any(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage.ToString())) ? preferredLanguage.FirstOrDefault(f => f.Value == item.ContactDetailsLegal.ContactDetailsLegal_PreferredCommunicationLanguage.ToString()).Text : string.Empty;
                                writer.WriteStartElement(GetXmlElementName("EmailAddresseDetails", path));
                                writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_EmailAddressForSendingAlerts", path), item.ContactDetailsLegal.ContactDetailsLegal_EmailAddressForSendingAlerts.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_PreferredMailingAddress", path), mailingAddressText.Trim().ToUpper());
                                writer.WriteEndElement();
                                writer.WriteElementString(GetXmlElementName("ContactDetailsLegal_PreferredCommunicationLanguage", path), preferredLanguageText.Trim().ToUpper());
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
        public static string GetApplicantBusinessProfileXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();

            string path = "/XML-Field-Lookups/LEGAL/Business-Profile";
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
                            if (item.CompanyBusinessProfile != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Business Profile", path));
                                string countryofOriginofWealthActivities = (country != null && country.Count > 0 && item.CompanyBusinessProfile.CountryofOriginofWealthActivities != null && country.Any(f => f.Value == item.CompanyBusinessProfile.CountryofOriginofWealthActivities.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyBusinessProfile.CountryofOriginofWealthActivities.ToString()).Text : string.Empty;

                                writer.WriteElementString(GetXmlElementName("MainBusinessActivities", path), item.CompanyBusinessProfile.MainBusinessActivities.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("NumberofYearsinOperation", path), item.CompanyBusinessProfile.NumberofYearsinOperation.ToString().Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("NumberofEmployes", path), item.CompanyBusinessProfile.NumberofEmployes.ToString().Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("WebsiteAddress", path), item.CompanyBusinessProfile.WebsiteAddress.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CorporationIsengagedInTheProvision", path), item.CompanyBusinessProfile.CorporationIsengagedInTheProvision == true ? "YES" : "NO");
                                writer.WriteElementString(GetXmlElementName("IssuingAuthority", path), item.CompanyBusinessProfile.IssuingAuthority.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EconomicSectorIndustry", path), item.CompanyBusinessProfile.EconomicSectorIndustryName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryofOriginofWealthActivities", path), countryofOriginofWealthActivities.Trim().ToUpper());
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
        public static string GetApplicantFinancialInformationXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/Financial-Profile";
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
                            writer.WriteStartElement(GetLookupCode("Financial Profile", path));
                            if (item.CompanyFinancialInformation != null)
                            {
                                writer.WriteElementString(GetXmlElementName("Turnover", path), item.CompanyFinancialInformation.Turnover.ToString().Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("NetProfitAndLoss", path), item.CompanyFinancialInformation.NetProfitLoss.ToString().Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("TotalAssets", path), item.CompanyFinancialInformation.TotalAssets.ToString().Trim().ToUpper());
                            }
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
                            }
                            writer.WriteEndElement();
                            writer.Flush();
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
        public static string GetApplicantFatcaDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            var classification = ServiceHelper.GetFATCA_CLASSIFICATION();
            var usEntityType = ServiceHelper.GetUS_ENTITY_TYPE();
            var financialInstitution = ServiceHelper.GetTYPE_OF_FOREIGN_FINANCIAL_INSTITUTION();
            var financialForeignEntity = ServiceHelper.GetTYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE();
            var identificationNumber = ServiceHelper.GetGLOBAL_INTERMEDIARY_IDENTIFICATION_NUMBER_GIIN();
            var exemptionReason = ServiceHelper.GetEXEMPTION_REASON();
            string path = "/XML-Field-Lookups/LEGAL/Fatca-Details";
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
                            if (item.FATCACRSDetails != null)
                            {
                                string classificationText = (classification != null && classification.Count > 0 && item.FATCACRSDetails.FATCADetails_FATCAClassification != null && classification.Any(f => f.Value == item.FATCACRSDetails.FATCADetails_FATCAClassification.ToString())) ? classification.FirstOrDefault(f => f.Value == item.FATCACRSDetails.FATCADetails_FATCAClassification.ToString()).Text : string.Empty;
                                string usEntityTypeText = (usEntityType != null && usEntityType.Count > 0 && item.FATCACRSDetails.FATCADetails_USEtityType != null && usEntityType.Any(f => f.Value == item.FATCACRSDetails.FATCADetails_USEtityType.ToString())) ? usEntityType.FirstOrDefault(f => f.Value == item.FATCACRSDetails.FATCADetails_USEtityType.ToString()).Text : string.Empty;
                                string financialInstitutionText = (financialInstitution != null && financialInstitution.Count > 0 && item.FATCACRSDetails.FATCADetails_TypeofForeignFinancialInstitution != null && financialInstitution.Any(f => f.Value == item.FATCACRSDetails.FATCADetails_TypeofForeignFinancialInstitution.ToString())) ? financialInstitution.FirstOrDefault(f => f.Value == item.FATCACRSDetails.FATCADetails_TypeofForeignFinancialInstitution.ToString()).Text : string.Empty;
                                string financialForeignEntityText = (financialForeignEntity != null && financialForeignEntity.Count > 0 && item.FATCACRSDetails.FATCADetails_TypeofNonFinancialForeignEntity != null && financialForeignEntity.Any(f => f.Value == item.FATCACRSDetails.FATCADetails_TypeofNonFinancialForeignEntity.ToString())) ? financialForeignEntity.FirstOrDefault(f => f.Value == item.FATCACRSDetails.FATCADetails_TypeofNonFinancialForeignEntity.ToString()).Text : string.Empty;
                                string identificationNumberText = (identificationNumber != null && identificationNumber.Count > 0 && item.FATCACRSDetails.FATCADetails_GlobalIntermediaryIdentificationNumber != null && identificationNumber.Any(f => f.Value == item.FATCACRSDetails.FATCADetails_GlobalIntermediaryIdentificationNumber.ToString())) ? identificationNumber.FirstOrDefault(f => f.Value == item.FATCACRSDetails.FATCADetails_GlobalIntermediaryIdentificationNumber.ToString()).Text : string.Empty;
                                string exemptionReasonText = (exemptionReason != null && exemptionReason.Count > 0 && item.FATCACRSDetails.ExemptionReason != null && exemptionReason.Any(f => f.Value == item.FATCACRSDetails.ExemptionReason.ToString())) ? exemptionReason.FirstOrDefault(f => f.Value == item.FATCACRSDetails.ExemptionReason.ToString()).Text : string.Empty;
                                writer.WriteStartElement(GetLookupCode("Fatca Details", path));
                                writer.WriteElementString(GetXmlElementName("FATCADetails_FATCAClassification", path), classificationText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_USEtityType", path), usEntityTypeText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_TypeofForeignFinancialInstitution", path), financialInstitutionText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_TypeofNonFinancialForeignEntity", path), financialForeignEntityText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FATCADetails_GlobalIntermediaryIdentificationNumber", path), item.FATCACRSDetails.FATCADetails_GlobalIntermediaryIdentificationNumber.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("ExemptionReason", path), item.FATCACRSDetails.ExemptionReason);
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
        public static string GetApplicantCrsDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            var classification = ServiceHelper.GetCRS_CLASSIFICATION();
            var finiancialEntity = ServiceHelper.GetTYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE();
            var securitiesMarket = ServiceHelper.GetNAME_OF_ESTABLISHED_SECURITES_MARKET();
            var financialInstitution = ServiceHelper.GetTYPE_OF_FINANCIAL_INSITUTION();

            string path = "/XML-Field-Lookups/LEGAL/Crs-Details";
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
                            if (item.CRSDetails != null)
                            {
                                string classificationText = (classification != null && classification.Count > 0 && item.CRSDetails.CompanyCRSDetails_CRSClassification != null && classification.Any(f => f.Value == item.CRSDetails.CompanyCRSDetails_CRSClassification.ToString())) ? classification.FirstOrDefault(f => f.Value == item.CRSDetails.CompanyCRSDetails_CRSClassification.ToString()).Text : string.Empty;
                                string finiancialEntityText = (finiancialEntity != null && finiancialEntity.Count > 0 && item.CRSDetails.CompanyCRSDetails_TypeofActiveNonFinancialEntity != null && finiancialEntity.Any(f => f.Value == item.CRSDetails.CompanyCRSDetails_TypeofActiveNonFinancialEntity.ToString())) ? finiancialEntity.FirstOrDefault(f => f.Value == item.CRSDetails.CompanyCRSDetails_TypeofActiveNonFinancialEntity.ToString()).Text : string.Empty;
                                string securitiesMarketText = (securitiesMarket != null && securitiesMarket.Count > 0 && item.CRSDetails.CompanyCRSDetails_NameofEstablishedSecuritiesMarket != null && securitiesMarket.Any(f => f.Value == item.CRSDetails.CompanyCRSDetails_NameofEstablishedSecuritiesMarket.ToString())) ? securitiesMarket.FirstOrDefault(f => f.Value == item.CRSDetails.CompanyCRSDetails_NameofEstablishedSecuritiesMarket.ToString()).Text : string.Empty;
                                string financialInstitutionText = (financialInstitution != null && financialInstitution.Count > 0 && item.CRSDetails.CompanyCRSDetails_TypeofFinancialInstitution != null && financialInstitution.Any(f => f.Value == item.CRSDetails.CompanyCRSDetails_TypeofFinancialInstitution.ToString())) ? financialInstitution.FirstOrDefault(f => f.Value == item.CRSDetails.CompanyCRSDetails_TypeofFinancialInstitution.ToString()).Text : string.Empty;

                                writer.WriteStartElement(GetLookupCode("Crs Details", path));
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_CRSClassification", path), classificationText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_TypeofActiveNonFinancialEntity", path), finiancialEntityText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_NameofEstablishedSecuritiesMarket", path), securitiesMarketText.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CompanyCRSDetails_TypeofFinancialInstitution", path), financialInstitutionText.Trim().ToUpper());
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
        #endregion
        #region----RELATED PARTY-----

        public static string GetRealtedPartyindividualXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
			var entityType = ServiceHelper.GetCompanyEntityTypes();
			var educationLevel = ServiceHelper.GetEducationLevel();
            string personPath = "/XML-Field-Lookups/COMMON/Personal-Details";
            string customerCheckPath = "/XML-Field-Lookups/COMMON/Customer-Check";
			string companyDetailsPath = "/XML-Field-Lookups/LEGAL/Company-Details(Related-Party)";
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
                            if (string.Equals(item.Type, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                            {
                                writer.WriteStartElement("relatedparty");
                                //Customer Check
                                writer.WriteStartElement(GetLookupCode("Customer Check", customerCheckPath));
                                writer.WriteElementString(GetXmlElementName("Partyreference", customerCheckPath), item.PersonalDetails.Id.ToString());
                                writer.WriteElementString(GetXmlElementName("PartyType", customerCheckPath), item.PersonalDetails.IsRelatedPartyUBO == true ? "UBO" : "3RDPARTY");
                                writer.WriteElementString(GetXmlElementName("CustomerType", customerCheckPath), "RETAIL");
                                writer.WriteElementString(GetXmlElementName("CustomerExists", customerCheckPath), "NO");
                                writer.WriteElementString(GetXmlElementName("CustomerCode", customerCheckPath), item.PersonalDetails.PersonalDetails_CustomerCIF);
                                writer.WriteEndElement();
                                if (item.PersonalDetails != null)
                                {
                                    string personTitleText = ServiceHelper.GetName(ValidationHelper.GetString(item.PersonalDetails.Title, ""), Constants.TITLES);
                                    string personGenderText = ServiceHelper.GetName(ValidationHelper.GetString(item.PersonalDetails.Gender, ""), Constants.GENDER);
                                    writer.WriteStartElement(GetLookupCode("Personal Details", personPath));
                                    string countryofBirthName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBirth != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString()).Text : string.Empty;
                                    string educationLevelName = (educationLevel != null && educationLevel.Count > 0 && item.PersonalDetails.EducationLevel != null && educationLevel.Any(f => f.Value == item.PersonalDetails.EducationLevel.ToString())) ? educationLevel.FirstOrDefault(f => f.Value == item.PersonalDetails.EducationLevel.ToString()).Text : string.Empty;
                                    writer.WriteElementString(GetXmlElementName("Title", personPath), personTitleText.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Title", personPath) + "_code", GetLookUpItemCode(personTitleText, Constants.TITLES)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("FirstName", personPath), item.PersonalDetails.FirstName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("LastName", personPath), item.PersonalDetails.LastName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("FathersName", personPath), item.PersonalDetails.FathersName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Gender", personPath), personGenderText.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Gender", personPath) + "_code", GetLookUpItemCode(personGenderText, Constants.GENDER)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("DateOfBirth", personPath), Convert.ToDateTime(item.PersonalDetails.DateOfBirth).ToString("yyyy-MM-dd"));
                                    writer.WriteElementString(GetXmlElementName("PlaceOfBirth", personPath), item.PersonalDetails.PlaceOfBirth.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("CountryOfBirth", personPath), countryofBirthName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("CountryOfBirth", personPath) + "_code",  ServiceHelper.GetCountryTwoletterCode(countryofBirthName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("EducationLevel", personPath), educationLevelName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("EducationLevel", personPath) + "_code", GetLookUpItemCode(educationLevelName, Constants.Education)?.Trim().ToUpper());
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                //Identification
                                string identificationPath = "/XML-Field-Lookups/COMMON/Identifications";
                                if (item._lst_IdentificationDetailsViewModel != null)
                                {
                                    writer.WriteStartElement(GetLookupCode("Identifications", identificationPath));
                                    foreach (var identification in item._lst_IdentificationDetailsViewModel)
                                    {
                                        writer.WriteStartElement("Identification");
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_Citizenship", identificationPath), identification.IdentificationDetails_CitizenshipName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_Citizenship", identificationPath) + "_code", ServiceHelper.GetCountryTwoletterCode(identification.IdentificationDetails_CitizenshipName)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_TypeOfIdentification", identificationPath), identification.IdentificationDetails_TypeOfIdentificationName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_TypeOfIdentification", identificationPath) + "_code", GetLookUpItemCode(identification.IdentificationDetails_TypeOfIdentificationName, Constants.IDENTIFICATION_TYPE)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_IdentificationNumber", identificationPath), identification.IdentificationDetails_IdentificationNumber.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_CountryOfIssue", identificationPath), identification.IdentificationDetails_CountryOfIssueName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_CountryOfIssue", identificationPath) + "_code", ServiceHelper.GetCountryTwoletterCode(identification.IdentificationDetails_CountryOfIssueName)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_IssueDate", identificationPath), Convert.ToDateTime(identification.IdentificationDetails_IssueDate).ToString("yyyy-MM-dd"));
                                        writer.WriteElementString(GetXmlElementName("IdentificationDetails_ExpiryDate", identificationPath), Convert.ToDateTime(identification.IdentificationDetails_ExpiryDate).ToString("yyyy-MM-dd"));
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
                                        writer.WriteStartElement("address");
                                        writer.WriteElementString(GetXmlElementName("AddressType", addresspath), address.AddressTypeName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("AddressType", addresspath) + "_code", GetLookUpItemCode(address.AddressTypeName, Constants.ADDRESS_TYPE)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("AddressLine1", addresspath), address.AddressLine1.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("AddressLine2", addresspath), address.AddressLine2.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PostalCode", addresspath), address.PostalCode.Trim().ToUpper());
                                        writer.WriteElementString("pobox", address.POBox.ToString()); //pobox
                                        //writer.WriteElementString(GetXmlElementName("POBox", addresspath), address.POBox.ToString()); //poboxnumber
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
                                    writer.WriteElementString(GetXmlElementName("PrefereredMobileNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.ContactDetails_MobileTelNoNumber) ? "N" : "Y");
                                    writer.WriteElementString(GetXmlElementName("Country_Code_HomeTelNoNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_HomeTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_HomeTelNoNumber);
                                    writer.WriteElementString(GetXmlElementName("ContactDetails_HomeTelNoNumber", contactPath), item.ContactDetails.ContactDetails_HomeTelNoNumber);
                                    writer.WriteElementString(GetXmlElementName("Country_Code_WorkTelNoNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_WorkTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_WorkTelNoNumber);
                                    writer.WriteElementString(GetXmlElementName("ContactDetails_WorkTelNoNumber", contactPath), item.ContactDetails.ContactDetails_WorkTelNoNumber);
                                    writer.WriteElementString(GetXmlElementName("Country_Code_FaxNoFaxNumber", contactPath), string.IsNullOrEmpty(item.ContactDetails.Country_Code_FaxNoFaxNumber) ? "" : "+" + item.ContactDetails.Country_Code_FaxNoFaxNumber);
                                    writer.WriteElementString(GetXmlElementName("ContactDetails_FaxNoFaxNumber", contactPath), item.ContactDetails.ContactDetails_FaxNoFaxNumber);
                                    writer.WriteElementString(GetXmlElementName("ContactDetails_EmailAddress", contactPath), item.ContactDetails.ContactDetails_EmailAddress.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("ContactDetails_PreferredCommunicationLanguage", contactPath), GetLookUpItemCode(item.ContactDetails.ContactDetails_PreferredCommunicationLanguage,Constants.COMMUNICATION_LANGUAGE)?.Trim().ToUpper());
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                //Business Profile
                                string employmentPath = "/XML-Field-Lookups/INDIVIDUAL/Employement-Details";
                                writer.WriteStartElement("businessprofile");
                                if (item.EmploymentDetails != null)
                                {
                                    writer.WriteStartElement(GetLookupCode("Employement Details", employmentPath));
                                    //writer.WriteElementString(GetXmlElementName("EmploymentStatus", employmentPath), "EMPLOYED");
                                    writer.WriteElementString(GetXmlElementName("EmploymentStatus", employmentPath), item.EmploymentDetails.EmploymentStatusName);
                                    writer.WriteElementString(GetXmlElementName("EmploymentStatus", employmentPath) + "_code", GetLookUpItemCode(item.EmploymentDetails.EmploymentStatusName, Constants.EMPLOYMENT_STATUS)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Profession", employmentPath), item.EmploymentDetails.ProfessionName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("Profession", employmentPath) + "_code", GetLookUpItemCode(item.EmploymentDetails.ProfessionName, Constants.PROFESSION)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("EmployersName", employmentPath), item.EmploymentDetails.EmployersName.Trim().ToUpper());
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                writer.WriteEndElement();

                                if (item._lst_SourceOfIncomeModel != null && item._lst_SourceOfIncomeModel.Any(x => string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)))
                                {
                                    writer.WriteElementString("grossannualsalary", item._lst_SourceOfIncomeModel.Where(x => string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)).Select(x => x.AmountOfIncome.ToString()).FirstOrDefault());
                                }
                                else
                                {
                                    writer.WriteElementString("grossannualsalary", string.Empty);
                                }
                                if (item._lst_SourceOfIncomeModel != null && item._lst_SourceOfIncomeModel.Any(x => !string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)))
                                {
                                    writer.WriteElementString("totalotherincome", item._lst_SourceOfIncomeModel.Where(x => !string.Equals(x.SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.AmountOfIncome)).ToString());
                                }
                                else
                                {
                                    writer.WriteElementString("totalotherincome", string.Empty);
                                }
                                //writer.WriteStartElement("grossannualsalary");
                                //writer.WriteEndElement();
                                //writer.Flush();
                                //writer.WriteStartElement("totalotherincome");
                                //writer.WriteEndElement();
                                //writer.Flush();

                                //Origin Of Annual Income
                                string originOfIncomePath = "/XML-Field-Lookups/LEGAL/Origin-Of-Income";
                                if (item._lst_SourceOfIncomeModel != null)
                                {
                                    if (!(item._lst_SourceOfIncomeModel.Count() == 1 && string.Equals(item._lst_SourceOfIncomeModel.FirstOrDefault().SourceOfAnnualIncomeName, "GROSS SALARY", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        writer.WriteStartElement(GetLookupCode("Origin Of Income", originOfIncomePath));
                                        foreach (var income in item._lst_SourceOfIncomeModel)
                                        {
                                            if (!string.Equals(income.SourceOfAnnualIncomeName?.Trim(), "GROSS SALARY", StringComparison.OrdinalIgnoreCase))
                                            {
                                                writer.WriteStartElement("originofincome");
                                                writer.WriteElementString(GetXmlElementName("SourceOfAnnualIncome", originOfIncomePath), income.SourceOfAnnualIncomeName.Trim().ToUpper());
                                                writer.WriteElementString(GetXmlElementName("SourceOfAnnualIncome", originOfIncomePath)+"_code", GetLookUpItemCode(income.SourceOfAnnualIncomeName,Constants.SOURCE_OF_ANNUAL_INCOME).Trim().ToUpper());
                                                writer.WriteElementString(GetXmlElementName("SpecifyOtherSource", originOfIncomePath), income.SpecifyOtherSource.Trim().ToUpper());
                                                writer.WriteElementString(GetXmlElementName("AmountOfIncome", originOfIncomePath), income.AmountOfIncome.ToString().Trim().ToUpper());
                                                writer.WriteEndElement(); 
                                            }
                                        }
                                        writer.WriteEndElement();
                                        writer.Flush(); 
                                    }
                                }
                                //Origin Of Assets
                                string assetsPath = "/XML-Field-Lookups/LEGAL/Origin-Of-Assets";
                                if (item._lst_OriginOfTotalAssetsModel != null)
                                {

                                    foreach (var assets in item._lst_OriginOfTotalAssetsModel)
                                    {
                                        string originOfTotalAssetsText = ServiceHelper.GetName(ValidationHelper.GetString(assets.OriginOfTotalAssets, ""), Constants.ORIGIN_OF_TOTAL_ASSETS);
                                        writer.WriteStartElement(GetLookupCode("Origin Of Assets", assetsPath));
                                        writer.WriteElementString(GetXmlElementName("OriginOfTotalAssets", assetsPath), originOfTotalAssetsText.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("OriginOfTotalAssets", assetsPath) + "_code", GetLookUpItemCode(originOfTotalAssetsText,Constants.ORIGIN_OF_TOTAL_ASSETS).Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("SpecifyOtherOrigin", assetsPath), assets.SpecifyOtherOrigin.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("AmountOfTotalAsset", assetsPath), assets.AmountOfTotalAsset.ToString());
                                        writer.WriteEndElement();
                                    }
                                }
                                //Pep Related
                                string pepRelatedPath = "/XML-Field-Lookups/COMMON/PEP-Related";
                                writer.WriteStartElement(GetXmlElementName("PepDetailsRelated", pepRelatedPath));
                                writer.WriteElementString(GetXmlElementName("IsRelatedaPep", pepRelatedPath), item.PersonalDetails.IsPepName.ToString().Trim().ToUpper() == "TRUE" ? "YES" : "NO");
                                if (item._lst_PepApplicantViewModel != null && item.PersonalDetails.IsPepName.ToString().Trim().ToUpper() == "TRUE")
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
                                writer.Flush();

                                //Pep Family Associates
                                string familyAssociatesPath = "/XML-Field-Lookups/COMMON/PEP-Family-Associates";
                                writer.WriteStartElement(GetXmlElementName("PepDetailsFamilyMembersAssociates", familyAssociatesPath));
                                writer.WriteElementString(GetXmlElementName("IsFamilyMemberAssociatePep", familyAssociatesPath), item.PersonalDetails.IsRelatedToPepName.ToString().Trim().ToUpper() == "TRUE" ? "YES" : "NO");
                                if (item._lst_PepAssociatesViewModel != null && item.PersonalDetails.IsRelatedToPepName.ToString().Trim().ToUpper() == "TRUE")
                                {
                                    foreach (var pepAssociates in item._lst_PepAssociatesViewModel)
                                    {
                                        writer.WriteStartElement(GetLookupCode("PEP Family Associates", familyAssociatesPath));
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_FirstName", familyAssociatesPath), pepAssociates.PepAssociates_FirstName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_Surname", familyAssociatesPath), pepAssociates.PepAssociates_Surname.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", familyAssociatesPath), pepAssociates.PepAssociates_RelationshipName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", familyAssociatesPath) + "_code", GetLookUpItemCode(pepAssociates.PepAssociates_RelationshipName, Constants.RELATIONSHIPS)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_PositionOrganization", familyAssociatesPath), pepAssociates.PepAssociates_PositionOrganization.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_Country", familyAssociatesPath), pepAssociates.PepAssociates_CountryName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_Country", familyAssociatesPath) + "_code", ServiceHelper.GetCountryTwoletterCode(pepAssociates.PepAssociates_CountryName)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_Since", familyAssociatesPath), Convert.ToDateTime(pepAssociates.PepAssociates_Since).ToString("yyyy-MM-dd"));
                                        writer.WriteElementString(GetXmlElementName("PepAssociates_Until", familyAssociatesPath), Convert.ToDateTime(pepAssociates.PepAssociates_Until).ToString("yyyy-MM-dd"));
                                        writer.WriteEndElement();
                                    }
                                }
                                else
                                {
                                    writer.WriteStartElement(GetLookupCode("PEP Family Associates", familyAssociatesPath));
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_FirstName", familyAssociatesPath), "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Surname", familyAssociatesPath), "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", familyAssociatesPath), "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Relationship", familyAssociatesPath) + "_code", "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_PositionOrganization", familyAssociatesPath), "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Country", familyAssociatesPath), "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Country", familyAssociatesPath) + "_code", "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Since", familyAssociatesPath), "");
                                    writer.WriteElementString(GetXmlElementName("PepAssociates_Until", familyAssociatesPath), "");
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                writer.Flush();
                                //Roles
                                string rolesPath = "/XML-Field-Lookups/COMMON/Roles";
                                if (item.PartyRolesLegal != null)
                                {
                                    writer.WriteStartElement(GetLookupCode("Roles", rolesPath));
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsDirector == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "DIRECTOR");                                        
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9eedc210-da6e-442f-9ca2-85a867e402f6"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9eedc210-da6e-442f-9ca2-85a867e402f6"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAlternativeDirector == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "ALTERNATIVE DIRECTOR");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("599bae16-5050-483a-84cf-1cb5d94e8690"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("599bae16-5050-483a-84cf-1cb5d94e8690"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretary == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "SECRETARY");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("274593c6-4983-4c0c-99f6-23c0c61a79f5"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("274593c6-4983-4c0c-99f6-23c0c61a79f5"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsShareholder == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "SHAREHOLDER");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9ba0a626-6671-43ae-83cc-25bd3bcdcc96"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9ba0a626-6671-43ae-83cc-25bd3bcdcc96"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsUltimateBeneficiaryOwner == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "ULTIMATE BENEFICIAL OWNER");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("b032e3f3-eaeb-40ab-b964-4e4c042a241d"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("b032e3f3-eaeb-40ab-b964-4e4c042a241d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    //if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedSignatory == true)
                                    //{
                                    //    //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED SIGNATORY");
                                    //    writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e4816d88-c1f2-4c85-b16d-867c0fac8d3e"));
                                    //    writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e4816d88-c1f2-4c85-b16d-867c0fac8d3e"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    //}
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedPerson == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED PERSON");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("6cb82fc1-afcb-4b54-8e2c-d7d51890edec"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("6cb82fc1-afcb-4b54-8e2c-d7d51890edec"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    //if (item.PartyRolesLegal.RelatedPartyRoles_IsDesignatedEBankingUser == true)
                                    //{
                                    //    //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "DESIGNATED E-BANKING USER");
                                    //    writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("5b062e5e-0695-4e61-8677-8e46d01e4b9c"));
                                    //    writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("5b062e5e-0695-4e61-8677-8e46d01e4b9c"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    //}
                                    //if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedCardholder == true)
                                    //{
                                    //    //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED CARDHOLDER");
                                    //    writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9ce397e7-3e83-4c0e-9e28-5887613a565d"));
                                    //    writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9ce397e7-3e83-4c0e-9e28-5887613a565d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    //}
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedContactPerson == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("bb661c47-560e-4558-89c2-a8ca8879ceeb")); //AUTHORISED CONTACT PERSON
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("bb661c47-560e-4558-89c2-a8ca8879ceeb"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }

                                    //New added role

                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsPartner == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("171afa93-7de1-4ea4-bcb3-a58b1e72573a")); //PARTNER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("171afa93-7de1-4ea4-bcb3-a58b1e72573a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_GeneralPartner == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3f62c060-177c-4cde-bca0-08ca417531a4")); //GENERAL PARTNER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3f62c060-177c-4cde-bca0-08ca417531a4"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_LimitedPartner == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("921b8240-b277-45f4-a270-400dcb027f10")); //LIMITED PARTNER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("921b8240-b277-45f4-a270-400dcb027f10"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsPresidentOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3b253076-d913-472f-8239-cf688145dbdf")); //PRESIDENT OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3b253076-d913-472f-8239-cf688145dbdf"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsVicePresidentOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("8e0c526d-7fd9-4902-b412-555e336065ac")); //VICE-PRESIDENT OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("8e0c526d-7fd9-4902-b412-555e336065ac"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretaryOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("94b1e8ca-b76f-413a-8683-a672d1602360")); //SECRETARY OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("94b1e8ca-b76f-413a-8683-a672d1602360"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsTreasurerOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("4d9d57a7-165d-464b-9083-e95bd924b0f1")); //TREASURE OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("4d9d57a7-165d-464b-9083-e95bd924b0f1"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsMemeberOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("23b70b2c-8d1e-4f61-9159-34d0daaebec2")); //MEMBER OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("23b70b2c-8d1e-4f61-9159-34d0daaebec2"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsTrustee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e1aa7816-14bc-4717-9db5-4e1cd328fc03")); //TRUSTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e1aa7816-14bc-4717-9db5-4e1cd328fc03"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSettlor == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("dac9e60c-b377-4102-bcff-18c10fbd93b7")); //SETTLOR
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("dac9e60c-b377-4102-bcff-18c10fbd93b7"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsProtector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("745f3db5-7c09-48ab-a1de-2683df720c1c")); //PROTECTOR
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("745f3db5-7c09-48ab-a1de-2683df720c1c"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("5295a919-1f4a-4b07-b2b2-b8fb2cac7702")); //CHAIRMAN OF THE BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("5295a919-1f4a-4b07-b2b2-b8fb2cac7702"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("2cca749e-971e-472c-b235-5eff64e00606")); //VICE-CHAIRMAN OF THE BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("2cca749e-971e-472c-b235-5eff64e00606"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e7211284-7011-4f32-8382-2bbf78d88267")); //SECRETARY OF THE BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e7211284-7011-4f32-8382-2bbf78d88267"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("14a2c9b1-e8e2-4e9c-a420-5973b156cf85")); //TREASURER OF BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("14a2c9b1-e8e2-4e9c-a420-5973b156cf85"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsMemeberOfBoardOfDirectors == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("459a0f8a-1942-44f4-97ad-4aa744dba64a")); //MEMBER OF BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("459a0f8a-1942-44f4-97ad-4aa744dba64a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsFundMlco == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("df98720a-f8e4-47bc-b74c-bbaa68a7a81a")); //FUND MLCO
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("df98720a-f8e4-47bc-b74c-bbaa68a7a81a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsFundAdministrator == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("352f0599-80d7-4f3e-a1d8-13a5faa10638")); //FUND ADMINISTRATOR
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("352f0599-80d7-4f3e-a1d8-13a5faa10638"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsManagementCompany == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3fe7f9b2-b814-40cb-8e83-e362642a3899")); //MANAGEMENT COMPANY
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3fe7f9b2-b814-40cb-8e83-e362642a3899"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsHolderOfManagementShares == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("8b8b6ee6-80e9-43a1-824b-a5a45100485d")); //HOLDER OF MANAGEMENT SHARES
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("8b8b6ee6-80e9-43a1-824b-a5a45100485d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsFounder == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("dfd27518-f3c2-491d-8171-ad7637be490e")); //FOUNDER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("dfd27518-f3c2-491d-8171-ad7637be490e"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsBenificiary == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("b0cac0b1-9516-446f-9456-9f2814438bfd")); //BENEFICIARY
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("b0cac0b1-9516-446f-9456-9f2814438bfd"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }

                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                writer.WriteEndElement();

                            }
                        }
						foreach (var item in application.RelatedParties)
						{
							if (string.Equals(item.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
							{
								writer.WriteStartElement("relatedparty");
								//Customer Check
								writer.WriteStartElement(GetLookupCode("Customer Check", customerCheckPath));
								writer.WriteElementString(GetXmlElementName("Partyreference", customerCheckPath), item.CompanyDetails.Id.ToString());
                                //writer.WriteElementString(GetXmlElementName("PartyType", customerCheckPath), item.CompanyDetails.IsRelatedPartyUBO == true ? "UBOCORP" : "UBO");
                                writer.WriteElementString(GetXmlElementName("PartyType", customerCheckPath), item.CompanyDetails.IsRelatedPartyUBO == true ? "UBO" : "3RDPARTY");
                                writer.WriteElementString(GetXmlElementName("CustomerType", customerCheckPath), "CORPORATE");
								writer.WriteElementString(GetXmlElementName("CustomerExists", customerCheckPath), "NO");
								writer.WriteElementString(GetXmlElementName("CustomerCode", customerCheckPath), item.CompanyDetails.CustomerCIF);
								writer.WriteEndElement();
								if (item.CompanyDetails != null)
								{
									writer.WriteStartElement(GetLookupCode("Company Details(Related Party)", companyDetailsPath));
									string countryofIncorporationName = (country != null && country.Count > 0 && item.CompanyDetails.CountryofIncorporation != null && country.Any(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString()).Text : string.Empty;
									string entityTypeName = (entityType != null && entityType.Count > 0 && item.CompanyDetails.EntityType != null && entityType.Any(f => f.Value == item.CompanyDetails.EntityType.ToString())) ? entityType.FirstOrDefault(f => f.Value == item.CompanyDetails.EntityType.ToString()).Text : string.Empty;

									writer.WriteElementString(GetXmlElementName("RegisteredName", companyDetailsPath), item.CompanyDetails.RegisteredName.Trim().ToUpper());
									writer.WriteElementString(GetXmlElementName("EntityType", companyDetailsPath), entityTypeName.Trim().ToUpper());
									writer.WriteElementString(GetXmlElementName("EntityType", companyDetailsPath) + "_code", GetLookUpItemCode(entityTypeName, Constants.COMPANY_ENTITY_TYPE)?.Trim().ToUpper());
									writer.WriteElementString(GetXmlElementName("CountryofIncorporation", companyDetailsPath), countryofIncorporationName.Trim().ToUpper());
									writer.WriteElementString(GetXmlElementName("CountryofIncorporation", companyDetailsPath) + "_code", ServiceHelper.GetCountryTwoletterCode(countryofIncorporationName)?.Trim().ToUpper());
									writer.WriteElementString(GetXmlElementName("RegistrationNumber", companyDetailsPath), item.CompanyDetails.RegistrationNumber.Trim().ToUpper());
									writer.WriteElementString(GetXmlElementName("DateofIncorporation", companyDetailsPath), Convert.ToDateTime(item.CompanyDetails.DateofIncorporation).ToString("yyyy-MM-dd"));
									writer.WriteEndElement();
									writer.Flush();
								}
								//Address Details
								string addresspath = "/XML-Field-Lookups/COMMON/Address-Details";
								if (item._lst_AddressDetailsModel != null)
								{
									writer.WriteStartElement(GetLookupCode("Address Details", addresspath));
									foreach (var address in item._lst_AddressDetailsModel)
									{
										writer.WriteStartElement("address");
										writer.WriteElementString(GetXmlElementName("AddressType", addresspath), address.AddressTypeName.Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("AddressType", addresspath) + "_code", GetLookUpItemCode(address.AddressTypeName, Constants.Address_Type)?.Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("AddressLine1", addresspath), address.AddressLine1.Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("AddressLine2", addresspath), address.AddressLine2.Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("PostalCode", addresspath), address.PostalCode.Trim().ToUpper());
										writer.WriteElementString("pobox", address.POBox.ToString());
										//writer.WriteElementString(GetXmlElementName("POBox", addresspath), address.POBox.ToString());
										writer.WriteElementString(GetXmlElementName("City", addresspath), address.City.ToString().Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("Country", addresspath), address.CountryName.Trim().ToUpper());
										writer.WriteElementString(GetXmlElementName("Country", addresspath) + "_code", ServiceHelper.GetCountryTwoletterCode(address.CountryName)?.Trim().ToUpper());
										writer.WriteEndElement();
									}
									writer.WriteEndElement();
									writer.Flush();
								}
								//Party Roles
								string rolesPath = "/XML-Field-Lookups/COMMON/Roles";
								if (item.PartyRolesLegal != null)
								{
									writer.WriteStartElement(GetLookupCode("Roles", rolesPath));
									if (item.PartyRolesLegal.RelatedPartyRoles_IsDirector == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "DIRECTOR");
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9eedc210-da6e-442f-9ca2-85a867e402f6"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9eedc210-da6e-442f-9ca2-85a867e402f6"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsAlternativeDirector == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "ALTERNATIVE DIRECTOR");
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("599bae16-5050-483a-84cf-1cb5d94e8690"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("599bae16-5050-483a-84cf-1cb5d94e8690"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretary == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "SECRETARY");
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("274593c6-4983-4c0c-99f6-23c0c61a79f5"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("274593c6-4983-4c0c-99f6-23c0c61a79f5"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsShareholder == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "SHAREHOLDER");
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9ba0a626-6671-43ae-83cc-25bd3bcdcc96"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9ba0a626-6671-43ae-83cc-25bd3bcdcc96"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsUltimateBeneficiaryOwner == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "ULTIMATE BENEFICIARY OWNER");
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("b032e3f3-eaeb-40ab-b964-4e4c042a241d"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("b032e3f3-eaeb-40ab-b964-4e4c042a241d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedSignatory == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED SIGNATORY");
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e4816d88-c1f2-4c85-b16d-867c0fac8d3e"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e4816d88-c1f2-4c85-b16d-867c0fac8d3e"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedPerson == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED PERSON");
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("6cb82fc1-afcb-4b54-8e2c-d7d51890edec"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("6cb82fc1-afcb-4b54-8e2c-d7d51890edec"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									//if (item.PartyRolesLegal.RelatedPartyRoles_IsDesignatedEBankingUser == true)
									//{
									//	//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "DESIGNATED E-BANKING USER");
									//	writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("5b062e5e-0695-4e61-8677-8e46d01e4b9c"));
									//	writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("5b062e5e-0695-4e61-8677-8e46d01e4b9c"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									//}
									//if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedCardholder == true)
									//{
									//	//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED CARDHOLDER");
									//	writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9ce397e7-3e83-4c0e-9e28-5887613a565d"));
									//	writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9ce397e7-3e83-4c0e-9e28-5887613a565d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									//}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedContactPerson == true)
									{
										//writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED CONTACT PERSON"); ;
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("bb661c47-560e-4558-89c2-a8ca8879ceeb"));
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("bb661c47-560e-4558-89c2-a8ca8879ceeb"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}

									//New added role

									if (item.PartyRolesLegal.RelatedPartyRoles_IsPartner == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("171afa93-7de1-4ea4-bcb3-a58b1e72573a")); //PARTNER
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("171afa93-7de1-4ea4-bcb3-a58b1e72573a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_GeneralPartner == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3f62c060-177c-4cde-bca0-08ca417531a4")); //GENERAL PARTNER
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3f62c060-177c-4cde-bca0-08ca417531a4"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_LimitedPartner == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("921b8240-b277-45f4-a270-400dcb027f10")); //LIMITED PARTNER
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("921b8240-b277-45f4-a270-400dcb027f10"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsPresidentOfCommittee == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3b253076-d913-472f-8239-cf688145dbdf")); //PRESIDENT OF COMMITTEE
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3b253076-d913-472f-8239-cf688145dbdf"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsVicePresidentOfCommittee == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("8e0c526d-7fd9-4902-b412-555e336065ac")); //VICE-PRESIDENT OF COMMITTEE
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("8e0c526d-7fd9-4902-b412-555e336065ac"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretaryOfCommittee == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("94b1e8ca-b76f-413a-8683-a672d1602360")); //SECRETARY OF COMMITTEE
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("94b1e8ca-b76f-413a-8683-a672d1602360"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsTreasurerOfCommittee == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("4d9d57a7-165d-464b-9083-e95bd924b0f1")); //TREASURE OF COMMITTEE
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("4d9d57a7-165d-464b-9083-e95bd924b0f1"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsMemeberOfCommittee == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("23b70b2c-8d1e-4f61-9159-34d0daaebec2")); //MEMBER OF COMMITTEE
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("23b70b2c-8d1e-4f61-9159-34d0daaebec2"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsTrustee == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e1aa7816-14bc-4717-9db5-4e1cd328fc03")); //TRUSTEE
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e1aa7816-14bc-4717-9db5-4e1cd328fc03"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsSettlor == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("dac9e60c-b377-4102-bcff-18c10fbd93b7")); //SETTLOR
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("dac9e60c-b377-4102-bcff-18c10fbd93b7"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsProtector == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("745f3db5-7c09-48ab-a1de-2683df720c1c")); //PROTECTOR
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("745f3db5-7c09-48ab-a1de-2683df720c1c"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("5295a919-1f4a-4b07-b2b2-b8fb2cac7702")); //CHAIRMAN OF THE BOARD OF DIRECTORS
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("5295a919-1f4a-4b07-b2b2-b8fb2cac7702"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("2cca749e-971e-472c-b235-5eff64e00606")); //VICE-CHAIRMAN OF THE BOARD OF DIRECTORS
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("2cca749e-971e-472c-b235-5eff64e00606"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e7211284-7011-4f32-8382-2bbf78d88267")); //SECRETARY OF THE BOARD OF DIRECTORS
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e7211284-7011-4f32-8382-2bbf78d88267"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("14a2c9b1-e8e2-4e9c-a420-5973b156cf85")); //TREASURER OF BOARD OF DIRECTORS
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("14a2c9b1-e8e2-4e9c-a420-5973b156cf85"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsMemeberOfBoardOfDirectors == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("459a0f8a-1942-44f4-97ad-4aa744dba64a")); //MEMBER OF BOARD OF DIRECTORS
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("459a0f8a-1942-44f4-97ad-4aa744dba64a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsFundMlco == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("df98720a-f8e4-47bc-b74c-bbaa68a7a81a")); //FUND MLCO
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("df98720a-f8e4-47bc-b74c-bbaa68a7a81a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsFundAdministrator == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("352f0599-80d7-4f3e-a1d8-13a5faa10638")); //FUND ADMINISTRATOR
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("352f0599-80d7-4f3e-a1d8-13a5faa10638"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsManagementCompany == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3fe7f9b2-b814-40cb-8e83-e362642a3899")); //MANAGEMENT COMPANY
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3fe7f9b2-b814-40cb-8e83-e362642a3899"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsHolderOfManagementShares == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("8b8b6ee6-80e9-43a1-824b-a5a45100485d")); //HOLDER OF MANAGEMENT SHARES
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("8b8b6ee6-80e9-43a1-824b-a5a45100485d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsFounder == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("dfd27518-f3c2-491d-8171-ad7637be490e")); //FOUNDER
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("dfd27518-f3c2-491d-8171-ad7637be490e"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									if (item.PartyRolesLegal.RelatedPartyRoles_IsBenificiary == true)
									{
										writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("b0cac0b1-9516-446f-9456-9f2814438bfd")); //BENEFICIARY
										writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("b0cac0b1-9516-446f-9456-9f2814438bfd"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
									}
									writer.WriteEndElement();
									writer.Flush();
								}
								writer.WriteEndElement();
							}
						}
						writer.WriteEndElement();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }
        public static string GetRealtedPartyLegalXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var entityType = ServiceHelper.GetCompanyEntityTypes();
            string companyDetailsPath = "/XML-Field-Lookups/LEGAL/Company-Details(Related-Party)";
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
                            if (string.Equals(item.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                            {
                                writer.WriteStartElement("relatedparty");
                                //Customer Check
                                writer.WriteStartElement(GetLookupCode("Customer Check", customerCheckPath));
                                writer.WriteElementString(GetXmlElementName("Partyreference", customerCheckPath), item.CompanyDetails.Id.ToString());
                                writer.WriteElementString(GetXmlElementName("PartyType", customerCheckPath), item.CompanyDetails.IsRelatedPartyUBO == true ? "UBO" : "3RDPARTY");
                                writer.WriteElementString(GetXmlElementName("CustomerType", customerCheckPath), "CORPORATE");
                                writer.WriteElementString(GetXmlElementName("CustomerExists", customerCheckPath), "NO");
                                writer.WriteElementString(GetXmlElementName("CustomerCode", customerCheckPath), string.Empty);
                                writer.WriteEndElement();
                                if (item.CompanyDetails != null)
                                {
                                    writer.WriteStartElement(GetLookupCode("Company Details(Related Party)", companyDetailsPath));
                                    string countryofIncorporationName = (country != null && country.Count > 0 && item.CompanyDetails.CountryofIncorporation != null && country.Any(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString()).Text : string.Empty;
                                    string entityTypeName = (entityType != null && entityType.Count > 0 && item.CompanyDetails.EntityType != null && entityType.Any(f => f.Value == item.CompanyDetails.EntityType.ToString())) ? entityType.FirstOrDefault(f => f.Value == item.CompanyDetails.EntityType.ToString()).Text : string.Empty;

                                    writer.WriteElementString(GetXmlElementName("RegisteredName", companyDetailsPath), item.CompanyDetails.RegisteredName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("EntityType", companyDetailsPath), entityTypeName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("EntityType", companyDetailsPath) + "_code", GetLookUpItemCode(entityTypeName, Constants.COMPANY_ENTITY_TYPE)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("CountryofIncorporation", companyDetailsPath), countryofIncorporationName.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("CountryofIncorporation", companyDetailsPath) + "_code", ServiceHelper.GetCountryTwoletterCode(countryofIncorporationName)?.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("RegistrationNumber", companyDetailsPath), item.CompanyDetails.RegistrationNumber.Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("DateofIncorporation", companyDetailsPath), Convert.ToDateTime(item.CompanyDetails.DateofIncorporation).ToString("yyyy-MM-dd"));
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                //Address Details
                                string addresspath = "/XML-Field-Lookups/COMMON/Address-Details";
                                if (item._lst_AddressDetailsModel != null)
                                {
                                    writer.WriteStartElement(GetLookupCode("Address Details", addresspath));
                                    foreach (var address in item._lst_AddressDetailsModel)
                                    {
                                        writer.WriteStartElement("address");
                                        writer.WriteElementString(GetXmlElementName("AddressType", addresspath), address.AddressTypeName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("AddressType", addresspath) + "_code", GetLookUpItemCode(address.AddressTypeName, Constants.Address_Type)?.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("AddressLine1", addresspath), address.AddressLine1.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("AddressLine2", addresspath), address.AddressLine2.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("PostalCode", addresspath), address.PostalCode.Trim().ToUpper());
                                        writer.WriteElementString("pobox", address.POBox.ToString());
                                        //writer.WriteElementString(GetXmlElementName("POBox", addresspath), address.POBox.ToString());
                                        writer.WriteElementString(GetXmlElementName("City", addresspath), address.City.ToString().Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("Country", addresspath), address.CountryName.Trim().ToUpper());
                                        writer.WriteElementString(GetXmlElementName("Country", addresspath) + "_code", ServiceHelper.GetCountryTwoletterCode(address.CountryName)?.Trim().ToUpper());
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                //Party Roles
                                string rolesPath = "/XML-Field-Lookups/COMMON/Roles";
                                if (item.PartyRolesLegal != null)
                                {
                                    writer.WriteStartElement(GetLookupCode("Roles", rolesPath));
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsDirector == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "DIRECTOR");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9eedc210-da6e-442f-9ca2-85a867e402f6"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9eedc210-da6e-442f-9ca2-85a867e402f6"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAlternativeDirector == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "ALTERNATIVE DIRECTOR");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("599bae16-5050-483a-84cf-1cb5d94e8690"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("599bae16-5050-483a-84cf-1cb5d94e8690"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretary == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "SECRETARY");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("274593c6-4983-4c0c-99f6-23c0c61a79f5"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("274593c6-4983-4c0c-99f6-23c0c61a79f5"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsShareholder == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "SHAREHOLDER");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9ba0a626-6671-43ae-83cc-25bd3bcdcc96"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9ba0a626-6671-43ae-83cc-25bd3bcdcc96"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsUltimateBeneficiaryOwner == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "ULTIMATE BENEFICIARY OWNER");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("b032e3f3-eaeb-40ab-b964-4e4c042a241d"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("b032e3f3-eaeb-40ab-b964-4e4c042a241d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedSignatory == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED SIGNATORY");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e4816d88-c1f2-4c85-b16d-867c0fac8d3e"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e4816d88-c1f2-4c85-b16d-867c0fac8d3e"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedPerson == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED PERSON");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("6cb82fc1-afcb-4b54-8e2c-d7d51890edec"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("6cb82fc1-afcb-4b54-8e2c-d7d51890edec"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsDesignatedEBankingUser == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "DESIGNATED E-BANKING USER");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("5b062e5e-0695-4e61-8677-8e46d01e4b9c"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("5b062e5e-0695-4e61-8677-8e46d01e4b9c"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedCardholder == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED CARDHOLDER");
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("9ce397e7-3e83-4c0e-9e28-5887613a565d"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("9ce397e7-3e83-4c0e-9e28-5887613a565d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedContactPerson == true)
                                    {
                                        //writer.WriteElementString(GetXmlElementName("Role", rolesPath), "AUTHORISED CONTACT PERSON"); ;
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("bb661c47-560e-4558-89c2-a8ca8879ceeb"));
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("bb661c47-560e-4558-89c2-a8ca8879ceeb"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }

                                    //New added role

                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsPartner == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("171afa93-7de1-4ea4-bcb3-a58b1e72573a")); //PARTNER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("171afa93-7de1-4ea4-bcb3-a58b1e72573a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_GeneralPartner == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3f62c060-177c-4cde-bca0-08ca417531a4")); //GENERAL PARTNER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3f62c060-177c-4cde-bca0-08ca417531a4"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_LimitedPartner == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("921b8240-b277-45f4-a270-400dcb027f10")); //LIMITED PARTNER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("921b8240-b277-45f4-a270-400dcb027f10"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsPresidentOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3b253076-d913-472f-8239-cf688145dbdf")); //PRESIDENT OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3b253076-d913-472f-8239-cf688145dbdf"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsVicePresidentOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("8e0c526d-7fd9-4902-b412-555e336065ac")); //VICE-PRESIDENT OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("8e0c526d-7fd9-4902-b412-555e336065ac"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretaryOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("94b1e8ca-b76f-413a-8683-a672d1602360")); //SECRETARY OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("94b1e8ca-b76f-413a-8683-a672d1602360"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsTreasurerOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("4d9d57a7-165d-464b-9083-e95bd924b0f1")); //TREASURE OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("4d9d57a7-165d-464b-9083-e95bd924b0f1"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsMemeberOfCommittee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("23b70b2c-8d1e-4f61-9159-34d0daaebec2")); //MEMBER OF COMMITTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("23b70b2c-8d1e-4f61-9159-34d0daaebec2"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsTrustee == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e1aa7816-14bc-4717-9db5-4e1cd328fc03")); //TRUSTEE
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e1aa7816-14bc-4717-9db5-4e1cd328fc03"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSettlor == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("dac9e60c-b377-4102-bcff-18c10fbd93b7")); //SETTLOR
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("dac9e60c-b377-4102-bcff-18c10fbd93b7"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsProtector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("745f3db5-7c09-48ab-a1de-2683df720c1c")); //PROTECTOR
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("745f3db5-7c09-48ab-a1de-2683df720c1c"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("5295a919-1f4a-4b07-b2b2-b8fb2cac7702")); //CHAIRMAN OF THE BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("5295a919-1f4a-4b07-b2b2-b8fb2cac7702"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("2cca749e-971e-472c-b235-5eff64e00606")); //VICE-CHAIRMAN OF THE BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("2cca749e-971e-472c-b235-5eff64e00606"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("e7211284-7011-4f32-8382-2bbf78d88267")); //SECRETARY OF THE BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("e7211284-7011-4f32-8382-2bbf78d88267"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("14a2c9b1-e8e2-4e9c-a420-5973b156cf85")); //TREASURER OF BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("14a2c9b1-e8e2-4e9c-a420-5973b156cf85"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsMemeberOfBoardOfDirectors == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("459a0f8a-1942-44f4-97ad-4aa744dba64a")); //MEMBER OF BOARD OF DIRECTORS
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("459a0f8a-1942-44f4-97ad-4aa744dba64a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsFundMlco == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("df98720a-f8e4-47bc-b74c-bbaa68a7a81a")); //FUND MLCO
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("df98720a-f8e4-47bc-b74c-bbaa68a7a81a"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsFundAdministrator == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("352f0599-80d7-4f3e-a1d8-13a5faa10638")); //FUND ADMINISTRATOR
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("352f0599-80d7-4f3e-a1d8-13a5faa10638"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsManagementCompany == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("3fe7f9b2-b814-40cb-8e83-e362642a3899")); //MANAGEMENT COMPANY
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("3fe7f9b2-b814-40cb-8e83-e362642a3899"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsHolderOfManagementShares == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("8b8b6ee6-80e9-43a1-824b-a5a45100485d")); //HOLDER OF MANAGEMENT SHARES
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("8b8b6ee6-80e9-43a1-824b-a5a45100485d"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsFounder == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("dfd27518-f3c2-491d-8171-ad7637be490e")); //FOUNDER
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("dfd27518-f3c2-491d-8171-ad7637be490e"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    if (item.PartyRolesLegal.RelatedPartyRoles_IsBenificiary == true)
                                    {
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath), GetRoleName("b0cac0b1-9516-446f-9456-9f2814438bfd")); //BENEFICIARY
                                        writer.WriteElementString(GetXmlElementName("Role", rolesPath) + "_code", GetLookUpItemCode(GetRoleName("b0cac0b1-9516-446f-9456-9f2814438bfd"), Constants.Roles_Related_To_Entity)?.Trim().ToUpper());
                                    }
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }
                }
                result = textWriter.ToString();
            }
            return result;
        }


        public static string GetRealtedPartyindividualPersonDetailsXML(ApplicationModel application)
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
                            if (item.PersonalDetails != null && string.Equals(item.Type, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                            {
                                string countryofBirthName = (country != null && country.Count > 0 && item.PersonalDetails.CountryOfBirth != null && country.Any(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString())) ? country.FirstOrDefault(f => f.Value == item.PersonalDetails.CountryOfBirth.ToString()).Text : string.Empty;
                                string educationLevelName = (educationLevel != null && educationLevel.Count > 0 && item.PersonalDetails.EducationLevel != null && educationLevel.Any(f => f.Value == item.PersonalDetails.EducationLevel.ToString())) ? educationLevel.FirstOrDefault(f => f.Value == item.PersonalDetails.EducationLevel.ToString()).Text : string.Empty;
                                writer.WriteElementString(GetXmlElementName("Title", path), item.PersonalDetails.Title.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FirstName", path), item.PersonalDetails.FirstName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("LastName", path), item.PersonalDetails.LastName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("FathersName", path), item.PersonalDetails.FathersName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("Gender", path), item.PersonalDetails.Gender.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("DateOfBirth", path), item.PersonalDetails.DateOfBirth.ToString().Trim().ToUpper());
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
        public static string GetRealtedPartyPersonDetailsXML(ApplicationModel application)
        {
            string result = string.Empty;
            var country = ServiceHelper.GetCountriesWithID();
            var entityType = ServiceHelper.GetCompanyEntityTypes();
            string path = "/XML-Field-Lookups/LEGAL/Company-Details(Related-Party)";
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

                            if (item.CompanyDetails != null && string.Equals(item.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                            {
                                writer.WriteStartElement(GetLookupCode("Company Details(Related Party)", path));
                                string countryofIncorporationName = (country != null && country.Count > 0 && item.CompanyDetails.CountryofIncorporation != null && country.Any(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString())) ? country.FirstOrDefault(f => f.Value == item.CompanyDetails.CountryofIncorporation.ToString()).Text : string.Empty;
                                string entityTypeName = (entityType != null && entityType.Count > 0 && item.CompanyDetails.EntityType != null && entityType.Any(f => f.Value == item.CompanyDetails.EntityType.ToString())) ? entityType.FirstOrDefault(f => f.Value == item.CompanyDetails.EntityType.ToString()).Text : string.Empty;

                                writer.WriteElementString(GetXmlElementName("RegisteredName", path), item.CompanyDetails.RegisteredName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("EntityType", path), entityTypeName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("CountryofIncorporation", path), countryofIncorporationName.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("RegistrationNumber", path), item.CompanyDetails.RegistrationNumber.Trim().ToUpper());
                                writer.WriteElementString(GetXmlElementName("DateofIncorporation", path), item.CompanyDetails.DateofIncorporation.ToString().Trim().ToUpper());
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
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_IssueDate", path), identification.IdentificationDetails_IssueDate.ToString().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("IdentificationDetails_ExpiryDate", path), identification.IdentificationDetails_ExpiryDate.ToString().Trim().ToUpper());
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
                                    writer.WriteElementString(GetXmlElementName("AddressLine1", path), address.AddressLine1.Trim().ToUpper().Trim().ToUpper());
                                    writer.WriteElementString(GetXmlElementName("AddressLine2", path), address.AddressLine2);
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
        public static string GetRealtedPartyContactDetailsXML(ApplicationModel application)
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
                                writer.WriteElementString(GetXmlElementName("Country_Code_MobileTelNoNumber", path), string.IsNullOrEmpty(item.ContactDetails.Country_Code_MobileTelNoNumber) ? "" : "+" + item.ContactDetails.Country_Code_MobileTelNoNumber);
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
        public static string GetRealtedPartyOriginOfIncomeXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/Origin-Of-Income";
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
        public static string GetRealtedPartyOriginOfTotalAssestsXML(ApplicationModel application)
        {
            string result = string.Empty;
            string path = "/XML-Field-Lookups/LEGAL/Origin-Of-Assets";
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

                        foreach (var item in application.RelatedParties)
                        {
                            if (item._lst_OriginOfTotalAssetsModel != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Origin Of Assets", path));
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
                            if (item.PartyRolesLegal != null)
                            {
                                writer.WriteStartElement(GetLookupCode("Roles", path));
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsDirector == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "DIRECTOR");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsAlternativeDirector == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "ALTERNATE DIRECTOR");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsSecretary == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "SECRETARY");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsShareholder == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "SHAREHOLDER");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsUltimateBeneficiaryOwner == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "ULTIMATE BENEFICIARY OWNER");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedSignatory == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "AUTHORISED SIGNATORY");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedPerson == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "AUTHORISED PERSON");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsDesignatedEBankingUser == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "DESIGNATED E-BANKING USER");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedCardholder == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "AUTHORISED CARDHOLDER");
                                }
                                if (item.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedContactPerson == true)
                                {
                                    writer.WriteElementString(GetXmlElementName("Role", path), "AUTHORISED CONTACT PERSON"); ;
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
                //retVal = result.Where(x => x.DocumentName == elementName && x.GetValue("LookupItemCode") != null).Select(x => x.GetValue("LookupItemCode").ToString()).FirstOrDefault();
                retVal = result.Where(x => string.Equals(x.GetValue("LookupItemName").ToString(), elementName,StringComparison.OrdinalIgnoreCase) && x.GetValue("LookupItemCode") != null).Select(x => x.GetValue("LookupItemCode").ToString()).FirstOrDefault();
            }
			if (string.IsNullOrEmpty(retVal))
			{
				//retVal = elementName;
				retVal = "MISSING.CODE";
			}
			return retVal;
        }

        public static string GetRoleName(string guid)
        {
            LookupItem role = LookupItemProvider.GetLookupItem(new Guid(guid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
            return role.LookupItemName;
        }
    }
}
