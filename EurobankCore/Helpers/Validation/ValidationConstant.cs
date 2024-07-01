using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class ValidationConstant
    {
        #region---Applicant Individual--------
        #region-----PEP Applicant------
        public const string PepApplicant_PositionOrganization = "Please enter the Position of PEP";
        public const string PepApplicant_Country = "Please select the Country of PEP";
        public const string PepApplicant_Since = "Please enter a valid date";
        public const string PepApplicant_Since_Validate = "Since Date must be smaller to today's date.";
        public const string PepApplicant_Untill = "Please enter a valid date";
        public const string PepApplicant_Untill_Validate = "Untill Date must be higher than Since Date";
        #endregion

        #region----PEP Family/Associates-----
        public const string PepAssociates_FirstName = "Please enter Name of PEP";
        public const string PepAssociates_Surname = "Please enter Surname of PEP";
        public const string PepAssociates_Relationship = "Please select Relation to PEP";
        public const string PepAssociates_PositionOrganization = "Please enter the Position of PEP";
        public const string PepAssociates_Country = "Please select the Country of PEP";
        public const string PepAssociates_Since = "Please enter a valid date";
        public const string PepAssociates_Since_Validate = "Since Date must be smaller to today's date.";
        public const string PepAssociates_Until = "Please enter a valid date";
        public const string PepAssociates_Until_Validate = "Untill Date must be higher than Since Date";
        #endregion
        #region----TAX Details-----
        public const string TaxDetails_CountryOfTaxResidency = "Please enter Country of Tax Residence";
        public const string TaxDetails_LiableToPayCyprus = "Please tell us if the applicant is liable for defence tax";
        public const string TaxDetails_TaxIdentificationNumber = "Please enter TIN Number";
        public const string TaxDetails_TinUnavailableReason = "Please select a reason for not providing TIN Number";
        public const string TaxDetails_JustificationForTinUnavalability = "Please explain why you are unable to provide a tax identificaiton number";
        public const string TaxDetails_TinAndIdentificationNumber = "Both TIN Number  and TIN  Unavailable Reason have a value. Please correct either one of the fields.";
        #endregion
        #region---Identification Details----
        public const string IdentificationDetails_Citizenship = "Please select Citizenship";
        public const string IdentificationDetails_TypeOfIdentification = "Please select type of identification";
        public const string IdentificationDetails_TypeOfIdentification_Selected_ID = "Citizenship is Cyprus hence an ID is required.";
        public const string IdentificationDetails_TypeOfIdentification_Selected_Passport = "Citizenship is a Third Country, hence a Passport is required.";
        public const string IdentificationDetails_IdentificationNumber = "Please enter an Identification number";
        public const string CountryOfIssueValue = "Please select country of issue";
        public const string IdentificationDetails_IssueDate = "Please enter a  valid Issue Date";
        public const string IdentificationDetails_IssueDate_Val = "Issue Date must be smaller to today's date.";
        public const string IdentificationDetails_ExpiryDate = "Please enter a  valid Expiry Date";
        public const string IdentificationDetails_ExpiryDate_High = "Expiry Date must be higher than today";
        public const string IdentificationDetails_ExpiryDate_Val = "Expiry Date must be higher than Issue date.";
        public const string IdentificationDetails_CitizenshipIssueCountry = "Issuing country should match citizenship country.";
        #endregion
        #region----Personal Details-----
        public const string PersonType = "Please select a Person Type";
        public const string Title = "Please select a Title";
        public const string FirstName = "First Name can't be blank";
        public const string LastName = "Last Name can't be blank";
        public const string FathersName = "Please enter Fathers Name";
        public const string Gender = "Please select a Gender";
        public const string IsPep = "Please select Is Applicant a PEP";
        public const string IsPepFamily = "Please select Is Applicant a Is any of Applicant's Family Member or Close Associate a PEP";
        public const string GenderMale = "Title selected is xx hence Gender should be xx.";
        public const string DateOfBirth = "Please enter a valid Date of Birth";
        public const string DateofIncorporation = "Please enter a valid Date of Incorporation";
        public const string DateOfBirthGreater = "Date of Birth is higher than today’s date";
        public const string DateOfBirthInvalid = "Please check if Date of Birth is correct";
        public const string DateOfBirthMinor = "Customer is a Minor and as such non-eligible for opening accounts through Extranet";
        public const string PlaceOfBirth = "Please enter  a Place of Birth";
        public const string CountryOfBirth = "Please select a Country of Birth";
        public const string EducationLevel = "Please select  Education Level";
        public const string InvitePerson = "Please select  Invite person for online ID verification";
        #endregion
        #region----Business and Financial Profile----
        public const string EmploymentStatus = "Please select an Employment status";
        public const string Profession = "Please select a Profession";
        public const string YearsInBusiness = "Please enter Years in Business";
        public const string EmployersName = "Please tell us the Employer's Name";
        public const string EmployersBusiness = "Please tell us the Employer's Nature of Business";

        //Retired Status
        public const string FormerProfession = "Please enter Former Profession";
        public const string FormerCountryOfEmployment = "Please enter Former Country of Employment";
        public const string FormerEmployersName = "Please enter Former Employer's Name";
        public const string FormerEmployersBusiness = "Please enter Former Employer's Business";

        #endregion
        #region----Contact Details----
        public const string Country_Code_MobileTelNoNumber = "Please select Country code of Mobile Tel. No";
        public const string Country_Code_HomeTelNo = "Please select Country code of Home Tel. No";
        public const string Country_Code_WorkTelNo = "Please select Country code of Work Tel. No";
        public const string Country_Code_FaxNo = "Please select Country code of Fax No";
        public const string ContactDetails_MobileTelNoNumber = "Please enter mobile number. This Mobile Number will be the preferred method of communication with the Bank for receiving ERB alerts.";//"This Mobile Number will be the preferred method of communication with the Bank for receiving SMS alerts.";
        public const string ContactDetails_MobileTelNoNumberRelatedParty = "Please enter mobile number.";
        public const string ContactDetails_EmailAddress = "Please enter email address. This email will be used to contact you about your current application.";//"This email will be used to contact you about your current application";
        public const string ContactDetails_EmailAddressRelatedParty = "Please enter email address.";
        public const string ContactDetails_EmailAddressForSendingAlerts = "Please enter a valid email address. This email address will be  the preferred method of communication with the Bank for receiving ERB alerts.";//"This email will be used to contact you about your current application";
        public const string ContactDetails_InvalidEmailAddress = "Email address is invalid.";
        public const string ContactDetails_PreferredCommunicationLanguage = "Please select Correspondence Language";
        public const string ContactDetails_HomeTelNoNumber = "Please enter home number.";
        public const string ContactDetails_WorkTelNoNumber = "Please enter work number.";
        public const string ContactDetails_FaxNoFaxNumber = "Please enter fax number.";

        #endregion
        #region----Source of Income----
        public const string SourceOfAnnualIncome = "Please select Origin of Annual Income";
        public const string SpecifyOtherSource = "Please provide description of Other income";
        public const string AmountOfIncome = "Please enter Income Amount";
        public const string Amount_Max = "Max limit reached - (19,2)";
        #endregion
        #region----Source of Income----
        public const string OriginOfTotalAssets = "Please select Origin of Total Assets";
        public const string SpecifyOtherOrigin = "Please provide description for Other Total Assets";
        public const string AmountOfTotalAsset = "Plese enter Total Assets Amount";
        #endregion
        #region----Address Details----
        public const string AddressType = "Please select Address Type";
        public const string LocationName = "Please enter registry name";
        public const string AddressLine1 = "Please enter Street Name and Details:  No, Flat No";
        public const string MaxLength_YearsInBusiness_Exceeded = "Max length exceeded, max length is 5";
        public const string MaxLength_Exceeded = "Max length exceeded, max length is 35 characters";
        public const string MaxLength_NameBankingInstitute = "Name of Banking Institution: max length is 50 characters";
        public const string MaxLength_65_Exceeded = "Max length exceeded, max length is 35 characters";
        public const string MaxLength_26_Exceeded = "Max length exceeded, max length is 26 characters";
        public const string MaxLength_50_Exceeded = "Max length exceeded, max length is 50 characters";
        public const string MaxLength_FirstName_Exceeded = "First Name: Max length exceeded, max length is 50 characters";
        public const string MaxLength_LastName_Exceeded = "Last Name: Max length exceeded, max length is 50 characters";
        public const string MaxLength_FatherName_Exceeded = "Father's Name: Max length exceeded, max length is 50 characters";

        public const string PostalCode = "Please enter a Postal Code";
        public const string City = "Please enter a City";
        public const string City_Special = "No number or special character is allowed in City";
        public const string RegisteredName_Special = "No number or special character is allowed in Registered Name";
        public const string City_Cyprus = "If Country is Cyprus the allowed cities are Nicosia, Limassol, Paphos, Larnaca and Famagusta";
        public const string Country = "Please select Country";
        public const string Work_Address = "Work address is required.";
        public const string Phone_No = "Phone no is required.";
        public const string Fax_No = "Fax no is required.";
        public const string Fax_No_Enter = "Please enter Fax No.";
        public const string Email = "Email is required.";
        public const string NumberOfStaffEmployed = "Number Of Staff Employed is required.";
        public const string SaveinRegistryLocation = "Please enter location.";
        public const string PhoneCountryCode = "Please select country code.";

        #endregion
        #endregion

        #region Applicant Common

        public const string NoOfYesrsOpeMaxLength_4_Exceeded = "Number of years in operation: max length is 4 digit";
        public const string NoOfEmployesMaxLength_4_Exceeded = "Number of Employees: max length is 4 digit";
        
        public const string NoOfYearsOpe_Negetive = "Number of years in operation: can not be negative";
        public const string Turnover_Negetive = "Turnover: can not be negative";
        public const string AmountOfTotalAsset_Negetive = "Amount of Total Asset: can not be negative";
        public const string TotalAssets_Negetive = "Total Assets: can not be negative";
        public const string NumberofEmployes_Negetive = "Number of Employees: can not be negative";
        public const string ExpectedIncomingAmount_Negetive = "Anticipated amount of incoming transactions to pass through the account(s) on an annual basis: can not be negative";
        

        #region Bank Relationship
        public const string BankRelationship_HasAccountInOtherBank = "Existing Banking relationship details missing";
        public const string BankRelationship_NameOfBankingInstitution = "Please provide name of  Banking Institution";
        public const string BankRelationship_CountryOfBankingInstitution = "Please select country of Banking Institution";
        #endregion

        #endregion

        #region---Applicant Legal---
        #region-----Contact Details---
        public const string ContactDetailsLegal_PreferredMailingAddress = "Please select a Preferred Mailing address";
        #endregion
        #region-----FATCA Details---
        public const string FATCADetails_FATCAClassification = "Please select  FATCA Classification";
        public const string FATCADetails_USEtityType = "Please select  US Entity Type";
        public const string FATCADetails_TypeofForeignFinancialInstitution = "Please select Type of Foreign Financial Institution";
        public const string FATCADetails_TypeofNonFinancialForeignEntity = "Please select Type of Non-Financial Foreign Entity (NFFE)";
        public const string FATCADetails_GlobalIntermediaryIdentificationNumber = "GIIN Number is required";
        public const string ExemptionReason = "Exemption Reason is required";
        #endregion
        #region---CRS Details---
        public const string CompanyCRSDetails_CRSClassification = "Please select CSR Classification";
        public const string CompanyCRSDetails_TypeofActiveNonFinancialEntity = "Please select Type of Active Non-Financial Entity (NFE)";
        public const string CompanyCRSDetails_NameofEstablishedSecuritiesMarket = "Please input Name of Established Securities Market";
        public const string CompanyCRSDetails_TypeofFinancialInstitution = "Please select Type of Financial Institution";
        public const string CompanyCRSDetails_TypeofFinancialInstitutionValid = "only one option can be select between Type of Active Non-Financial Entity (NFE) and Type of Financial Institution";
        #endregion
        #region Compnay Details
        public const string CompanyDetails_EntityType = "Please select entity type.";
        public const string CompanyDetails_CountryofIncorporation = "Please select Country of Incorporation.";
        
        public const string CompanyDetails_RegisteredName = "Registered name can't be blank";
        public const string CompanyDetails_RegistrationNumber = "Please enter registration number";
        public const string CompanyDetails_RegisteredName_Special = "Symbols/special characters are not allowed in registered name";
        public const string CompanyDetails_CountryOfIncorporation = "Please enter country of Incorporation";
        public const string CompanyDetails_RegitrationNumber = "Please enter registration number";
        public const string CompanyDetails_SharesIssuedBearer = "Please select corporation shares issued to the bearer";
        public const string CompanyDetails_ListingStatus = "Please select listing status";
        public const string CompanyDetails_OfficeInCyprus = "Please select the Entity located and operates an office in cyprus";
        public const string CompanyDetails_DateofIncorporation = "Please select date of incorporation";
        public const string CompanyDetails_RegistrationDateFuture = "No future date than today's date should be allowed.";
        
        #endregion
        #region Business Profile
        public const string BusinessProfile_MainBusinessActivities = "Legal Entity Activity: INPUT MISSING ";
        public const string BusinessProfile_NumberofYearsinOperation = "No. of Years in Operation: INPUT MISSING ";
        public const string BusinessProfile_NumberofEmployes = "No. of Employees: INPUT MISSING";
        public const string BusinessProfile_NumberofMembers = "No. of Members: INPUT MISSING";
        public const string BusinessProfile_CorporationIsengagedInTheProvision = "";
        public const string BusinessProfile_EconomicSectorIndustry = "Economic Sector/Industry: INPUT MISSING";
        public const string BusinessProfile_IssuingAuthority = "Please enter name of Issuing Authority";
        public const string BusinessProfile_CountryofOriginofWealthActivities = "Country of Activity: INPUT MISSING";
        public const string BusinessProfile_SponsoringEntityName = "Sponsoring Entity Name: INPUT MISSING";
        public const string BusinessProfile_LineOfBusinessOfTheSponsoringEntity = "Line Of Business Of The Sponsoring Entity: INPUT MISSING";
        public const string BusinessProfile_ProvisionOfFinancialandInvestmentServices = "Provision of Financial and Investment Services : INPUT MISSING";
        #endregion
        #region Finacial Profile

        public const string FinancialProfile_Turnover = "Annual Turnover (EUR): INPUT MISSING";
        public const string FinancialProfile_NetProfitLoss = "Net Profit And Loss (EUR): INPUT MISSING";        
        public const string FinancialProfile_TotalAssets = "Total Assets (EUR): INPUT MISSING";

        public const string FinancialProfilePositiveNumeric_19_2_RegexTotalAssets = "Total Assets (EUR): The value should be positive and in numeric (19,2) format !";
        public const string FinancialProfilePositiveNumeric_19_2_RegexTurnover = "Annual Turnover (EUR): The value should be positive and in numeric (19,2) format !";
        public const string FinancialProfileAllNumeric_19_2_RegexNetProfitLoss = "Net Profit And Loss (EUR): The value should be in numeric (19,2) format !";


        #endregion
        #endregion
        #region Application Section
        #region Section Validation Message

        public const string Applicant_Count_CanNotBeZero = "Applicant can't be Zero";
        public const string Applicant_Already_Created = "Applicant already created";
        public const string Applicant_Individual_Count_NotMoreThanOne = "Applicant can't be more than one for Individual Application";
        public const string Applicant_Legal_Count_NotMoreThanOne = "Applicant can't be more than one for Legal Entity Application";
        public const string Applicant_Joint_Count_ShouldMoreThanOne = "Applicant should be more than one for Joint Individual Application";
        public const string Application_GroupStructure_Count_NotLessThanTwo = "If Application belongs to a group the group structures can't be lesss than two";
        public const string RelatedParty_Legal_Count_CanNotBeZero = "Related Party can't be Zero";
        public const string Accounts_Count_CanNotBeZero = "Accounts can't be Zero";
        public const string DebitCard_Count_CanNotBeZero = "Debit Card can't be Zero";
        public const string EBankingSubscriber_Count_CanNotBeZero = "E-Banking subscriber can't be Zero";
        public const string SignatureMandate_Individual_Count_ShouldBeZero = "Signature Madate should be Zero for Individual Application";
        public const string SignatureMandate_JointIndividual_Count_ShouldBeTwo = "Signature Madate should be Two for Joint Individual Application";
        public const string SignatureMandate_Legal_Count_NotBeZero = "Signature Madate can't be Zero for Legal Entity Application";

        #endregion
        #region----Account Details-----
        public const string Accounts_AccountType = "Please select at least one Type of Account";
        public const string Accounts_Currency = "Please select a Currency";
        public const string Accounts_StatementFrequency = "Please select a statement frequency";
        #endregion
        #region----Purpose And Activity-----

        public const string ReasonForOpeningTheAccountGroup = "Please select the main reasons for opening an account";
        public const string ExpectedNatureOfInAndOutTransactionGroup = "Please select the nature of incoming and outgoing transactions";
        public const string ExpectedFrequencyOfInAndOutTransactionGroup = "Please select the frequency of incoming and outgoing transactions";
        public const string ExpectedIncomingAmount = "Please enter the Expected Incoming Amount to pass through the account on a calendar basis";

        #endregion
        #region---Source Of Incoming Transaction---
        public const string SourceOfIncomingTransactions_NameOfRemitter = "At least one Remitter must be entered.";
        public const string SourceOfIncomingTransactions_CountryOfRemitter = "Please select the Country of the Remitter";
        public const string SourceOfIncomingTransactions_CountryOfRemitterBank = "Please select the Country of Remmiter's Bank";
        #endregion
        #region---Source Of Outgoing Transaction---
        public const string NameOfBeneficiary = "At least one Beneficiary must be entered.";
        public const string CountryOfBeneficiary = "Please select the Country of the Beneficiary";
        public const string CountryOfBeneficiaryBank = "Please select the Country of the Beneficiary's Bank";

        #endregion

        #region----Debit card Details----
        public const string DebitCardDetails_CardType = "Please select card type";
        public const string DebitCardDetails_CardholderName = "Please select card holder name";
        public const string DebitCardDetails_CardHolderCountryOfIssue = "Please select country of issue";
        public const string AssociatedAccount = "Please select associated account";
        public const string DebitCardDetails_FullName_Regex = "Only Latin capital letters, numbers and symbols , & - . * / “ are allowed";
        public const string DebitCardDetails_CompanyNameAppearOnCard_Regex = "Only Latin capital letters, numbers and symbols , & - . * / “ are allowed";
        public const string DebitCardDetails_AssociatedAccount = "Please select the account that the card should be linked to";
        public const string Country_Code = "Please select country code";
        public const string DebitCardDetails_MobileNumber = "Please enter mobile number";
        public const string DebitCardDetails_MobileNumber_MaxLength = "Mobile number: max length is 8 digit";
        public const string DebitCardDetails_MobileNumber_CyprusLength = "Mobile number should be 8 digit";
        public const string DebitCardDetails_CardHolderIdentityNumber = "Please enter Identity / Passport Number";
        public const string DebitCardDetails_CompanyNameAppearOnCard = "Please enter the Company's Full Name as it will appear on the Card";
        public const string DebitCardDetails_FullName = "Please enter the Full Name as it will appear on the Card";
        public const string DebitCardDetails_DispatchMethod = "Please select dispatch method";
        public const string DebitCardDetails_IdentityNumber = "Please enter Identity / Passport Number";
        public const string DebitCardDetails_CollectedBy = "Please select collected by";
        public const string DebitCardDetails_CollectedBy_Individual = "Please enter collected by";
        public const string DebitCardDetails_DeliveryDetails = "Please enter delivery details";
        public const string DebitCardDetails_DeliveryAddress = "Please select delivery address";
        public const string DebitCardDetails_OtherDeliveryAddress = "Please enter other delivery address";
        public const string DebitCardDetails_Unique = "CardHolder details already exists";
        #endregion
        #region---EBanking Subscriber--------

        public const string Subscriber = "Please selct Subscriber";
        public const string IdentityPassportNumber = "Please enter Identity / Passport Number";
        public const string CountryOfIssue = "Please select country of issue";
        public const string AccessLevel = "Please select access level";
        #endregion
        #region---Signature Mandate----
        public const string LimitFrom = "Please enter limit from";
        public const string LimitTo = "Please enter limit to";
        public const string TotalNumberofSignature = "Please enter total number of signature";
        public const string CalculateTotalNumberOfSignature = "Sum of number of signature must be equal to Total number of Signature";
        public const string CalculateTotalNumberOfSignatureORright = "Number of signature must be equal to Total number of Signature";
        public const string NumberofSignature = "Please enter number of signature";
        public const string DifferentAuthorizedSignatoryGroup = "Please select different Authorized Signatory Group, Same group can not be in both Authorized Signatory Group";
        public const string AuthorizedSignatoryGroup = "Please select Authorized Signatory Group (s)";
        public const string AuthorizeMandateType = "Only one signature mandate For Authoised Persons is allowed. Edit existing if you need to modify it";
        public const string AuthorizePersonCumpulsory = "Please create a signature mandate for AUTHORISED PERSONS";
        #endregion
        #region---Signature Mandate Individual----
        public const string AmountFrom = "Please enter amount from";
        public const string AmountTo = "Please enter amount to";
        public const string NumberOfSignatures = "Please enter number of signature";
        public const string AccessRights = "Please select access rights";
        public const string SignatoryPersonsList = "Please select signatory person";
        public const string SignatoryGroupList = "Should have at least one Signature Mandate with each Signatory Group";
        #endregion
        #region----Signatory Group-----
        public const string SignatoryGroupName = "Please select signatory group name";
        public const string SignatoryGroupSignatureRights = "Please select signature rights";
        public const string SignatoryPersons = "Please select signatory persons";
        public const string SignatoryPersonAlreadyAdded = "Signatory person already added to another Signatory Group";
        public const string SignatoryPersonAlreadyAddedAuth = "Signatory person already added to another Signatory Group of AUTHORISED PERSONS";
        public const string SignatoryGroupWithAuthorizePerson = "All signatory persons must belong to at least one signatory group";
        public const string StartDate = "Please select start date";
        public const string StartDateValidate = "Start date should be smaller or equals to today's date";
        public const string EndDate = "Please select end date";
        public const string EndDateValidate = "End date must be higher to start date";
        public const string SignatoryGroupNameSequence = "Please create Signatory Group sequence wise(like GROUP A,GROUP B...)";
        public const string SignatoryGroupNameDeleteSequence = "Please Delete Signatory Group sequence wise(like GROUP D,GROUP C,GROUP B,GROUP A)";
        public const string SignatoryGroupAuthorizeUnique = "Only one signature group for Authorised Persons is allowed. Edit existing if you need to modify it";

        #endregion
        #region---Note Details----
        public const string NoteDetailsType = "Please select a type";
        public const string Subject = "Please enter subject";
        public const string Details = "Please enter details";
        public const string PendingOn = "Please select pending on";
        public const string ExpectedDate = "Please select expected date";

        #endregion
        #region----Bank Documents/Expected Documents---
        public const string Entity = "Please select a entity";
        public const string EntityType = "Please select a entity type";
        public const string DocumentType = "Please select a document type";
        #endregion
       
        #endregion

        #region Grid Validation

        public const string Applicant_Grid = "Applicant is required";
        public const string Applicant_Joint_Grid_Count = "More than one applicant is required";
        public const string Applicant_Individual_Grid_Count = "Applicant can only be one";
        public const string Relatedparty_Grid = "Related party is required";
        public const string Accounts_Grid = "Account is required";
        public const string Debit_Card_Grid = "Debit card is required";
        public const string Source_Of_Incoming_Grid = "Counterparty of Incoming Transaction is required";
        public const string Ebanking_Subscriber_Grid = "Ebanking subscriber is required";
        public const string Source_Of_Outgoing_Grid = "Counterparty of Outgoing Transaction is required";
        public const string Decision_History = "Decision History is required";
        public const string Signature_Mandate_Grid = "Signature Mandate is required";
        public const string Signatory_Group_Grid = "Signatory Group is required";
        //public const string Group_Structure_Grid = "If group structure is yes then \"group name\" and \"group activities\" should be completed";
        public const string Group_Structure_Grid = "Group structure is missing";
        public const string Group_Structure_GroupActivities = "Group activities is missing";
        public const string Group_Structure_GroupName = "Group name is missing";
        public const string Applicant_Tax_Grid_Invalid = "Record(s) is in Pending Status";
        public const string Applicant_PepDetailsApplicant_Grid_Invalid = "Record(s) is in Pending Status";
        public const string Applicant_PepDetailsFamily_Grid_Invalid = "Record(s) is in Pending Status";
        public const string Applicant_Identification_Grid_Invalid = "Record(s) is in Pending Status";
        public const string Applicant_SourceOfIncome_Grid_Invalid = "Record(s) is in Pending Status";
        public const string Applicant_OriginOfTotalAssets_Grid_Invalid = "Record(s) is in Pending Status";

        public const string Applicant_Address = "At least one address is required.";
        public const string Applicant_Identification = "At least one identification is required.";
        public const string Applicant_OriginOfAssets = "At least one origin of total assets is required";
        public const string Applicant_PepDetailsApplicant = "At least one pep details required.";
        public const string Applicant_PepDetailsFamily = "At least one pep family details is required.";
        public const string Applicant_SourceOfIncome = "At least one source of income is required";
        public const string Applicant_Tax = "At least one tax details is required.";
        public const string Applicant_AddressResMail = "Atleast one RESIDENTIAL ADDRESS and one MAILING ADDRESS is required";
        public const string Applicant_PassportMandetoryforNONCY = "At least one Passport is required for NON-CY citizens.";
        public const string Applicant_IDmandatoryforCY = "At least one ID is mandatory for CY citizens.";
        public const string Applicant_WorkAdderssMandetory = "Work address type is mandatory in case of choosen EMPLOYMENT STATUS";
        public const string Applicant_SingleMailAddress = "Applicant should have only one MAILING ADDRESS.";
        public const string Applicant_OneResidentialAddress = "At least one RESIDENTIAL ADDRESS is required";
        public const string Applicant_OneMailingAddress = "At least one MAILING ADDRESS is required";
        public const string Applicant_IdentificationDuplicate = "Identification contains duplicate records";
        public const string Applicant_AnnualIncomeGrossSalaryInvalid = "Gross Salary is not applicable for the selected employment status";

        //Applicant Legal
        public const string Address_Details = @"One address is required with address type ""Office in Cyprus"" and Country ""Cyprus"".";
        public const string Address_Details_Phone_No = @"Address type is ""Office in Cyprus"" so phone number most be from ""Cyprus"".";
        public const string Tax_Details_Country = @"Country of Corporation is ""Cyprus"" so a tax details is required from ""Cyprus""";
        public const string Tax_Details_Country_Info = @"Country of Incorporation is ""Cyprus"", there wasn't any tax information provided from Cyprus, if this is intended proceed with the application";
        public const string Address_Details_RegMail = "At least one REGISTERED OFFICE and one MAILING ADDRESS is required";
        public const string Address_Details_MailAddress = "At least one MAILING ADDRESS is required";
        public const string Address_Details_RegisteredAddress = "At least one REGISTERED OFFICE ADDRESS is required";
        public const string Address_RegOffice_Details = "REGISTERED OFFICE details required";
        public const string Applicant_Tax_Details_isApplicantLiableToPayDefenceTaxInCyprus = @"Please select ""Is the applicant liable to pay defence tax in Cyprus?""";
        #endregion

        #region----Related Party------
        //Individual
        public const string RelatedPartyRoles_HasPowerOfAttorney = "Please select at least one party role";
        public const string RelatedPartyRoles_HasAuthorisedPerson = "Please select atleast one Related Party with Authorised Person role";
        //Legal
        public const string RelatedPartyRoles_IsDirector = "Please select at least one party role";
        public const string RelatedPartyAddressRegistered = "Only one REGISTERED OFFICE details allowed";
        #endregion

        #region Regex Constants

        public const string RegexNameOnDebitCard = @"^[A-Z][A-Z0-9 ,&-.*/""]*$";
        public const string RegexPositiveNumeric_19_2 = @"^\d{0,17}(\.\d{1,2})?$";
        public const string RegexAllNumeric_19_2 = @"^-?\d{0,17}(\.\d{1,2})?$";

        #endregion
    }
}
