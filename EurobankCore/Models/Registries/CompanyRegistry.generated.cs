﻿//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;

[assembly: RegisterDocumentType(CompanyRegistry.CLASS_NAME, typeof(CompanyRegistry))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type CompanyRegistry.
	/// </summary>
	public partial class CompanyRegistry : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.CompanyRegistry";


		/// <summary>
		/// The instance of the class that provides extended API for working with CompanyRegistry fields.
		/// </summary>
		private readonly CompanyRegistryFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// CompanyDetailsID.
		/// </summary>
		[DatabaseIDField]
		public int CompanyRegistryID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("CompanyRegistryID"), 0);
			}
			set
			{
				SetValue("CompanyRegistryID", value);
			}
		}


		/// <summary>
		/// Does the Applicant maintain Bank Account(s) with another Banking Institution?.
		/// </summary>
		[DatabaseField]
		public bool CompanyDetails_HasAccountInOtherBank
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CompanyDetails_HasAccountInOtherBank"), false);
			}
			set
			{
				SetValue("CompanyDetails_HasAccountInOtherBank", value);
			}
		}


		/// <summary>
		/// Name of Banking Institution.
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_NameOfBankingInstitution
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_NameOfBankingInstitution"), @"");
			}
			set
			{
				SetValue("CompanyDetails_NameOfBankingInstitution", value);
			}
		}


		/// <summary>
		/// Country of Banking Institution.
		/// </summary>
		[DatabaseField]
		public int CompanyDetails_CountryOfBankingInstitution
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("CompanyDetails_CountryOfBankingInstitution"), 0);
			}
			set
			{
				SetValue("CompanyDetails_CountryOfBankingInstitution", value);
			}
		}


		/// <summary>
		/// Registered Name.
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_RegisteredName
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_RegisteredName"), @"");
			}
			set
			{
				SetValue("CompanyDetails_RegisteredName", value);
			}
		}


		/// <summary>
		/// Trading Name.
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_TradingName
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_TradingName"), @"");
			}
			set
			{
				SetValue("CompanyDetails_TradingName", value);
			}
		}


		/// <summary>
		/// Based on Entity Type, this field should be visible and mandatory as well. There are types that this field should not be visible. Only one value is allowed to be selected . If the selected value at field “SECTOR” is Public Limited Company or Public Insurance Company then field “Listing Status” should be mandatory for completion          .
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_EntityType
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_EntityType"), @"");
			}
			set
			{
				SetValue("CompanyDetails_EntityType", value);
			}
		}


		/// <summary>
		/// 1. Country of Incorporation should be the same as the Country of type Registered Address. If there is a mismatch then an error message should  should be generated.
		/// 2. If the selected value at fields "Tax Country” and “Country of Incorporation”differ then a warning message should be generated
		/// .
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_CountryofIncorporation
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_CountryofIncorporation"), @"");
			}
			set
			{
				SetValue("CompanyDetails_CountryofIncorporation", value);
			}
		}


		/// <summary>
		/// Registration Number .
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_RegistrationNumber
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_RegistrationNumber"), @"");
			}
			set
			{
				SetValue("CompanyDetails_RegistrationNumber", value);
			}
		}


		/// <summary>
		/// Date of Incorporation.
		/// </summary>
		[DatabaseField]
		public DateTime CompanyDetails_DateofIncorporation
		{
			get
			{
				return ValidationHelper.GetDateTime(GetValue("CompanyDetails_DateofIncorporation"), DateTimeHelper.ZERO_TIME);
			}
			set
			{
				SetValue("CompanyDetails_DateofIncorporation", value);
			}
		}


		/// <summary>
		/// Based on Entity Type, this field should be visible and mandatory as well. There are types that this field should not be visible. Only one value is allowed to be selected .
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_ListingStatus
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_ListingStatus"), @"");
			}
			set
			{
				SetValue("CompanyDetails_ListingStatus", value);
			}
		}


		/// <summary>
		/// Corporation shares  issued to the bearer.
		/// </summary>
		[DatabaseField]
		public bool CompanyDetails_CorporationSharesIssuedToTheBearer
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CompanyDetails_CorporationSharesIssuedToTheBearer"), false);
			}
			set
			{
				SetValue("CompanyDetails_CorporationSharesIssuedToTheBearer", value);
			}
		}


		/// <summary>
		/// Is the Entity located and operates an office in Cyprus?.
		/// </summary>
		[DatabaseField]
		public bool CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus"), false);
			}
			set
			{
				SetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus", value);
			}
		}


		/// <summary>
		/// Preferred Mailing Address.
		/// </summary>
		[DatabaseField]
		public Guid CompanyDetails_PreferredMailingAddress
		{
			get
			{
				return ValidationHelper.GetGuid(GetValue("CompanyDetails_PreferredMailingAddress"), Guid.Empty);
			}
			set
			{
				SetValue("CompanyDetails_PreferredMailingAddress", value);
			}
		}


		/// <summary>
		/// Email Address for sending Alerts.
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_EmailAddressForSendingAlerts
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_EmailAddressForSendingAlerts"), @"");
			}
			set
			{
				SetValue("CompanyDetails_EmailAddressForSendingAlerts", value);
			}
		}


		/// <summary>
		/// Preferred Communication Language.
		/// </summary>
		[DatabaseField]
		public Guid CompanyDetails_PreferredCommunicationLanguage
		{
			get
			{
				return ValidationHelper.GetGuid(GetValue("CompanyDetails_PreferredCommunicationLanguage"), Guid.Empty);
			}
			set
			{
				SetValue("CompanyDetails_PreferredCommunicationLanguage", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with CompanyRegistry fields.
		/// </summary>
		[RegisterProperty]
		public CompanyRegistryFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with CompanyRegistry fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class CompanyRegistryFields : AbstractHierarchicalObject<CompanyRegistryFields>
		{
			/// <summary>
			/// The content item of type CompanyRegistry that is a target of the extended API.
			/// </summary>
			private readonly CompanyRegistry mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="CompanyRegistryFields" /> class with the specified content item of type CompanyRegistry.
			/// </summary>
			/// <param name="instance">The content item of type CompanyRegistry that is a target of the extended API.</param>
			public CompanyRegistryFields(CompanyRegistry instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// CompanyDetailsID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.CompanyRegistryID;
				}
				set
				{
					mInstance.CompanyRegistryID = value;
				}
			}


			/// <summary>
			/// Does the Applicant maintain Bank Account(s) with another Banking Institution?.
			/// </summary>
			public bool CompanyDetails_HasAccountInOtherBank
			{
				get
				{
					return mInstance.CompanyDetails_HasAccountInOtherBank;
				}
				set
				{
					mInstance.CompanyDetails_HasAccountInOtherBank = value;
				}
			}


			/// <summary>
			/// Name of Banking Institution.
			/// </summary>
			public string CompanyDetails_NameOfBankingInstitution
			{
				get
				{
					return mInstance.CompanyDetails_NameOfBankingInstitution;
				}
				set
				{
					mInstance.CompanyDetails_NameOfBankingInstitution = value;
				}
			}


			/// <summary>
			/// Country of Banking Institution.
			/// </summary>
			public int CompanyDetails_CountryOfBankingInstitution
			{
				get
				{
					return mInstance.CompanyDetails_CountryOfBankingInstitution;
				}
				set
				{
					mInstance.CompanyDetails_CountryOfBankingInstitution = value;
				}
			}


			/// <summary>
			/// Registered Name.
			/// </summary>
			public string CompanyDetails_RegisteredName
			{
				get
				{
					return mInstance.CompanyDetails_RegisteredName;
				}
				set
				{
					mInstance.CompanyDetails_RegisteredName = value;
				}
			}


			/// <summary>
			/// Trading Name.
			/// </summary>
			public string CompanyDetails_TradingName
			{
				get
				{
					return mInstance.CompanyDetails_TradingName;
				}
				set
				{
					mInstance.CompanyDetails_TradingName = value;
				}
			}


			/// <summary>
			/// Based on Entity Type, this field should be visible and mandatory as well. There are types that this field should not be visible. Only one value is allowed to be selected . If the selected value at field “SECTOR” is Public Limited Company or Public Insurance Company then field “Listing Status” should be mandatory for completion          .
			/// </summary>
			public string CompanyDetails_EntityType
			{
				get
				{
					return mInstance.CompanyDetails_EntityType;
				}
				set
				{
					mInstance.CompanyDetails_EntityType = value;
				}
			}


			/// <summary>
			/// 1. Country of Incorporation should be the same as the Country of type Registered Address. If there is a mismatch then an error message should  should be generated.
			/// 2. If the selected value at fields "Tax Country” and “Country of Incorporation”differ then a warning message should be generated
			/// .
			/// </summary>
			public string CompanyDetails_CountryofIncorporation
			{
				get
				{
					return mInstance.CompanyDetails_CountryofIncorporation;
				}
				set
				{
					mInstance.CompanyDetails_CountryofIncorporation = value;
				}
			}


			/// <summary>
			/// Registration Number .
			/// </summary>
			public string CompanyDetails_RegistrationNumber
			{
				get
				{
					return mInstance.CompanyDetails_RegistrationNumber;
				}
				set
				{
					mInstance.CompanyDetails_RegistrationNumber = value;
				}
			}


			/// <summary>
			/// Date of Incorporation.
			/// </summary>
			public DateTime CompanyDetails_DateofIncorporation
			{
				get
				{
					return mInstance.CompanyDetails_DateofIncorporation;
				}
				set
				{
					mInstance.CompanyDetails_DateofIncorporation = value;
				}
			}


			/// <summary>
			/// Based on Entity Type, this field should be visible and mandatory as well. There are types that this field should not be visible. Only one value is allowed to be selected .
			/// </summary>
			public string CompanyDetails_ListingStatus
			{
				get
				{
					return mInstance.CompanyDetails_ListingStatus;
				}
				set
				{
					mInstance.CompanyDetails_ListingStatus = value;
				}
			}


			/// <summary>
			/// Corporation shares  issued to the bearer.
			/// </summary>
			public bool CompanyDetails_CorporationSharesIssuedToTheBearer
			{
				get
				{
					return mInstance.CompanyDetails_CorporationSharesIssuedToTheBearer;
				}
				set
				{
					mInstance.CompanyDetails_CorporationSharesIssuedToTheBearer = value;
				}
			}


			/// <summary>
			/// Is the Entity located and operates an office in Cyprus?.
			/// </summary>
			public bool CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus
			{
				get
				{
					return mInstance.CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus;
				}
				set
				{
					mInstance.CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus = value;
				}
			}


			/// <summary>
			/// Preferred Mailing Address.
			/// </summary>
			public Guid CompanyDetails_PreferredMailingAddress
			{
				get
				{
					return mInstance.CompanyDetails_PreferredMailingAddress;
				}
				set
				{
					mInstance.CompanyDetails_PreferredMailingAddress = value;
				}
			}


			/// <summary>
			/// Email Address for sending Alerts.
			/// </summary>
			public string CompanyDetails_EmailAddressForSendingAlerts
			{
				get
				{
					return mInstance.CompanyDetails_EmailAddressForSendingAlerts;
				}
				set
				{
					mInstance.CompanyDetails_EmailAddressForSendingAlerts = value;
				}
			}


			/// <summary>
			/// Preferred Communication Language.
			/// </summary>
			public Guid CompanyDetails_PreferredCommunicationLanguage
			{
				get
				{
					return mInstance.CompanyDetails_PreferredCommunicationLanguage;
				}
				set
				{
					mInstance.CompanyDetails_PreferredCommunicationLanguage = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyRegistry" /> class.
		/// </summary>
		public CompanyRegistry() : base(CLASS_NAME)
		{
			mFields = new CompanyRegistryFields(this);
		}

		#endregion
	}
}