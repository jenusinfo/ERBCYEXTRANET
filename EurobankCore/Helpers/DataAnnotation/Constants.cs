using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.DataAnnotation
{
	public class Constants
	{
		public const string Accounts_AccountType = "/Lookups/General/TYPE-OF-ACCOUNTS";
		public const string Accounts_Currency = "/Lookups/General/Currency";
		public const string Accounts_StatementFrequency = "/Lookups/General/STATEMENT-FREQUENCY";
		public const string Bank_Units = "/Bank-Units";
		public const string IDENTIFICATION_TYPE = "/Lookups/General/IDENTIFICATION-TYPE";
		public const string APPLICATION_TYPE = "/Lookups/General/APPLICATION-TYPE";
		public const string COMMUNICATION_LANGUAGE = "/Lookups/General/COMMUNICATION-LANGUAGE";
		public const string Education = "/Lookups/General/Education";
		public const string TITLES = "/Lookups/General/TITLES";
		public const string GENDER = "/Lookups/General/GENDER";
		public const string Address_Type = "/Lookups/General/Address-Type";
		public const string APPLICATION_SERVICES = "/Lookups/General/APPLICATION-SERVICES";
		public const string PURPOSE_AND_REASON_FOR_OPENING_THE_ACCOUNT = "/Lookups/General/PURPOSE-AND-REASON-FOR-OPENING-THE-ACCOUNT";
		public const string EXPECTED_NATURE_INCOMING_OUTGOING_TRANSACTION = "/Lookups/General/EXPECTED-NATURE-INCOMING-OUTGOING-TRANSACTION";
		public const string EXPECTED_FREQUENCY_INCOMING_OUTGOING_TRANSACTION = "/Lookups/General/EXPECTED-FREQUENCY-INCOMING-OUTGOING-TRANSACTION";
		public const string Access_Level = "/Lookups/General/ACCESS-LEVEL";
		public const string Access_Level_Individual = "/Lookups/General/ACCESS-LEVEL-INDIVIDUAL";
		public const string Document_Type = "/Lookups/General/DOCUMENT-TYPES";
		public const string Document_Subject = "/Lookups/General/SUBJECTS";
		public const string CARD_TYPE = "/Lookups/General/CARD-TYPE";
		public const string DISPATCH_METHOD = "/Lookups/General/DISPATCH-METHOD";
		public const string DISPATCH_METHOD_INDIVIDUAL = "/Lookups/General/DISPATCH-METHOD-INDIVIDUAL";
		public const string ADDRESS_TYPE = "/Lookups/General/ADDRESS-TYPE-(Physical-Person)";
		public const string PERSON_TYPE = "/Lookups/Personal-Joint-Account/PERSON-TYPE";
		public const string LEGALPERSON_TYPE = "/Lookups/Corporate-Account/PERSON-TYPE";
		public const string CORPORATEACCOUNT_TYPE = "/Lookups/Corporate-Account/TYPE";
		public const string Entity_TYPE = "/Lookups/Personal-Joint-Account/TYPE";
		public const string HID = "/Lookups/Personal-Joint-Account/HID";
		public const string Roles_Related_To_Entity = "/Lookups/Corporate-Account/DIRECTOR";
		//public const string Entity_TYPE_Group_Structure = "/Lookups/General/ENTITY-TYPE(Group-structure)";
		public const string Entity_TYPE_Group_Structure = "/Lookups/General/GROUP-STRUCTURE-TYPE";
		public const string DECISION_TYPE = "/Lookups/General/DECISION";
		public const string Corporate_Entity_TYPE = "/Lookups/Corporate-Account/TYPE";
		public const string Corporate_Entity_SUB_TYPE = "/Lookups/Corporate-Account/SUB-TYPE";
		public const string Corporate_JURISDICTION = "/Lookups/Corporate-Account/JURISDICTION";
		public const string Corporate_EXPECTED_DOCUMENT_TYPE = "/Lookups/Corporate-Account/EXPECTED-DOCUMENT-TYPE";
		public const string Corporate_PERSON_TYPE = "/Lookups/Corporate-Account/PERSON-TYPE";
		public const string IndividualJoint_PERSON_TYPE = "/Lookups/Personal-Joint-Account/PERSON-TYPE";
		public const string PERSON_ROLE = "/Lookups/Personal-Joint-Account/PERSON-ROLE";
		public const string LEGALPERSON_ROLE = "/Lookups/Corporate-Account/PERSON-ROLE";
		public const string JURISDICTION = "/Lookups/Corporate-Account/JURISDICTION";
		public const string BANK_DOCUMENT_TYPE = "/Lookups/Personal-Joint-Account/BANK-DOCUMENT-TYPE";
		public const string LegalBANK_DOCUMENT_TYPE = "/Lookups/Corporate-Account/BANK-DOCUMENT-TYPE";
		public const string Corporate_BANK_DOCUMENT_TYPE = "/Lookups/Corporate-Account/BANK-DOCUMENT-TYPE";
		public const string EXPECTED_DOCUMENT_TYPE = "/Lookups/Personal-Joint-Account/EXPECTED-DOCUMENT-TYPE";
		public const string LEGALEXPECTED_DOCUMENT_TYPE = "/Lookups/Corporate-Account/EXPECTED-DOCUMENT-TYPE";
		public const string TIN_UNAVAILABLE_REASON = "/Lookups/General/TIN-UNAVAILABLE-REASON";
		public const string Access_Right = "/Lookups/General/ACCESS-RIGHTS";
		public const string RELATIONSHIPS = "/Lookups/General/RELATIONSHIPS";
		public const string EMPLOYMENT_STATUS = "/Lookups/General/EMPLOYMENT-STATUS";
		public const string PROFESSION = "/Lookups/General/PROFESSION";
		public const string DefaultDate = "1/1/0001 12:00:00 AM";
		public const string SOURCE_OF_ANNUAL_INCOME = "/Lookups/General/SOURCE-OF-ANNUAL-INCOME";
		public const string ORIGIN_OF_TOTAL_ASSETS = "/Lookups/General/ORIGIN-OF-TOTAL-ASSETS";
		public const string LISTING_STATUS = "/Lookups/General/LISTING-STATUS";
		public const string COMPANY_ENTITY_TYPE = "/Lookups/General/Entity-Type";
		public const string SIGNATURE_RIGHTS = "/Lookups/General/SIGNATURE-RIGHTS";
		public const string LIMIT_AMOUNT = "/Lookups/General/LIMIT-AMOUNT";
		public const string LegalEntity = "LEGAL-ENTITY";
		public const string COUNTRY_CODE_PREFIX = "/Lookups/General/Country-Code-Prefix";
		public const string ECONOMIC_SECTOR_INDUSTRIES = "/Lookups/General/ECONIMIC-SECTOR-INDUSTRY";

		public const string FATCA_CLASSIFICATION= "/Lookups/General/FATCA-CLASSIFICATION";
		public const string US_ENTITY_TYPE = "/Lookups/General/US-ENTITY-TYPE";
		public const string TYPE_OF_FOREIGN_FINANCIAL_INSTITUTION = "/Lookups/General/TYPE-OF-FOREIGN-FINANCIAL-INSTITUTION";
		public const string TYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE = "/Lookups/General/TYPE-OF-NON-FINANCIAL-FOREIGN-ENTITY-(NFFE)";
		public const string GLOBAL_INTERMEDIARY_IDENTIFICATION_NUMBER_GIIN = "/Lookups/General/GLOBAL-INTERMEDIARY-IDENTIFICATION-NUMBER(GIIN)";
		public const string EXEMPTION_REASON = "/Lookups/General/EXEMPTION-REASON";

		public const string CRS_CLASSIFICATION = "/Lookups/General/CRS-CLASSIFICATION";
		public const string TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE = "/Lookups/General/TYPE-OF-ACTIVE-NON-FINANCIAL-ENTITY-(NFE)";
		public const string NAME_OF_ESTABLISHED_SECURITES_MARKET = "/Lookups/General/NAME-OF-ESTABLISHED-SECURITES-MARKET";
		public const string TYPE_OF_FINANCIAL_INSITUTION = "/Lookups/General/TYPE-OF-FINANCIAL-INSITUTION";
		public const string PREFERRED_MAILING_ADDRESS = "/Lookups/General/Preferred-Mailing-Address";
		public const int COUNTRY_CODE_PREFIX_NODE_PARENT_ID = 1330;
		public const string Associated_Account_Type = "/Lookups/General/ASSOCIATED-ACCOUNT";
		public const string SIGNATURE_MANDATE_TYPE = "/Lookups/General/Signature-Mandate-Type";
		public const string APPLICATION_STATUS = "/Lookups/General/APPLICATION-WORKFLOW-STATUS";

		public const string Address_Type_Physical = "/Lookups/General/ADDRESS-TYPE-(Physical-Person)";
		public const string SIGNATORY_GROUP = "/Lookups/General/SIGNATORY-GROUP";

		public const string MANDATE_TYPE = "/Lookups/General/MANDATE-TYPE";
		//Mail Template
		//Trigger-1
		public const string New_Application = "Eurobank-NewApplication";
		//Trigger-2
		public const string New_ApplicationChecker = "Eurobank-NewApplicationChecker";
		//Trigger-3
		public const string ResubmitApplication = "Eurobank-ResubmitApplication";
		//Trigger-4
		public const string ReturnedApplication = "Eurobank-ReturnedApplication";
		//Trigger-5
		public const string WithdrawnApplication = "Eurobank-WithdrawnApplication";
		//Trigger-6
		public const string ApplicationReviewed = "Eurobank-ApplicationReviewed";
		//Trigger-7
		public const string ApplicationCancelled = "Eurobank-ApplicationCancelled";
		//Trigger-8
		public const string ApplicationExecuted = "Eurobank-ApplicationExecuted";
		//Trigger-9//Trigger-12
		public const string ApplicationExecution = "Eurobank-ApplicationExecution";
		//Trigger-10
		public const string EscaltedForReview = "Eurobank-EscaltedForReview";
		//Trigger-11
		public const string DocumentsPrepared = "Eurobank-DocumentsPrepared";
		//Trigger-13
		public const string DocumentsSigned = "Eurobank-DocumentsSigned";
		//Trigger-14
		public const string DocumentPreparation = "Eurobank-DocumentPreparation";
		//Trigger-15
		public const string ApplicationCompleted = "Eurobank-Completed";
		//Trigger-16
		public const string DocumentSignatures = "Eurobank-DocumentSignatures";
		//Trigger-17
		public const string OmmissionsCompleted = "Eurobank-OmmissionsCompleted";
		//Trigger-18
		public const string PendingCompletion = "Eurobank-PendingCompletion";
		//Trigger-19
		public const string ResubmitApplicationChecker = "Eurobank-ResubmitApplicationChecker";
		//Trigger-20
		public const string ResubmitApplicationExecution = "Eurobank-ResubmitApplicationExecution";
		//Trigger-21
		public const string ResubmitApplicationBankDocuments = "Eurobank-ResubmitApplicationBankDocuments";
		//Trigger-22
		public const string DocumentsPendingOmmisions = "Eurobank-DocumentsPendingOmmissions";
		//Forgot Password
		public const string ForgotPassword = "Eurobank-ForgotPassword";
		//New User Approval
		public const string NewUserApproval = "Eurobank-NewUserApproval";
		//Registration Approval Required
		public const string RegistrationApprovalRequired = "Eurobank-RegistrationApprovalRequired";
        //Registration Successful Email
        public const string RegistrationSuccessfulEmail = "Eurobank-RegistrationSuccessfulEmail";


        public const string RELATED_PARTY_ROLE = "/Lookups/Personal-Joint-Account/RELATED-PARTY-ROLE";

		//Invite
		public const string Pending = "PENDING";
		public const string Invited = "INVITED";
		public const string Skipped = "SKIPPED";
		public const string NA = "N/A";

		public const string BlankGuid = "00000000-0000-0000-0000-000000000000";
		public const string PersonTypeGuidIndividual = "d8e8ef77-f5c1-40f7-95e3-05b248b983a4";

	}
	public enum EntityType
	{
		LEGALENTITY = 1,
		INDIVIDUAL = 2,
	}

	public enum SignatoryGroupType
    {
		GROUPA = 1,
		GROUPB = 2,
		GROUPC = 3,
		GROUPD = 4,
    }
	
}
