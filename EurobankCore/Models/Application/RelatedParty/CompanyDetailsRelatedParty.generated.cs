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

[assembly: RegisterDocumentType(CompanyDetailsRelatedParty.CLASS_NAME, typeof(CompanyDetailsRelatedParty))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type CompanyDetailsRelatedParty.
	/// </summary>
	public partial class CompanyDetailsRelatedParty : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.CompanyDetailsRelatedParty";


		/// <summary>
		/// The instance of the class that provides extended API for working with CompanyDetailsRelatedParty fields.
		/// </summary>
		private readonly CompanyDetailsRelatedPartyFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// CompanyDetailsID.
		/// </summary>
		[DatabaseIDField]
		public int CompanyDetailsRelatedPartyID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("CompanyDetailsRelatedPartyID"), 0);
			}
			set
			{
				SetValue("CompanyDetailsRelatedPartyID", value);
			}
		}


		/// <summary>
		/// Registry Id.
		/// </summary>
		[DatabaseField]
		public int PersonsRegistryID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("PersonsRegistryID"), 0);
			}
			set
			{
				SetValue("PersonsRegistryID", value);
			}
		}


		/// <summary>
		/// CustomerCIF.
		/// </summary>
		[DatabaseField]
		public string CustomerCIF
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CustomerCIF"), @"");
			}
			set
			{
				SetValue("CustomerCIF", value);
			}
		}


		/// <summary>
		/// Is Related Party a UBO?.
		/// </summary>
		[DatabaseField]
		public bool CompanyDetails_IsRelatedPartyUBO
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CompanyDetails_IsRelatedPartyUBO"), false);
			}
			set
			{
				SetValue("CompanyDetails_IsRelatedPartyUBO", value);
			}
		}


		/// <summary>
		/// ID Verified.
		/// </summary>
		[DatabaseField]
		public bool CompanyDetails_IdVerified
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CompanyDetails_IdVerified"), false);
			}
			set
			{
				SetValue("CompanyDetails_IdVerified", value);
			}
		}


		/// <summary>
		/// Invited.
		/// </summary>
		[DatabaseField]
		public int CompanyDetails_Invited
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("CompanyDetails_Invited"), 0);
			}
			set
			{
				SetValue("CompanyDetails_Invited", value);
			}
		}


		/// <summary>
		/// Application Type.
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_Type
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_Type"), @"");
			}
			set
			{
				SetValue("CompanyDetails_Type", value);
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
		/// Is he/she a PEP?.
		/// </summary>
		[DatabaseField]
		public bool CompanyDetails_IsPep
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CompanyDetails_IsPep"), false);
			}
			set
			{
				SetValue("CompanyDetails_IsPep", value);
			}
		}


		/// <summary>
		/// Is he/she an immediate family member or close associate a PEP?.
		/// </summary>
		[DatabaseField]
		public bool CompanyDetails_IsRelatedToPep
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CompanyDetails_IsRelatedToPep"), false);
			}
			set
			{
				SetValue("CompanyDetails_IsRelatedToPep", value);
			}
		}


		/// <summary>
		/// Status.
		/// </summary>
		[DatabaseField]
		public string CompanyDetails_Status
		{
			get
			{
				return ValidationHelper.GetString(GetValue("CompanyDetails_Status"), @"");
			}
			set
			{
				SetValue("CompanyDetails_Status", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with CompanyDetailsRelatedParty fields.
		/// </summary>
		[RegisterProperty]
		public CompanyDetailsRelatedPartyFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with CompanyDetailsRelatedParty fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class CompanyDetailsRelatedPartyFields : AbstractHierarchicalObject<CompanyDetailsRelatedPartyFields>
		{
			/// <summary>
			/// The content item of type CompanyDetailsRelatedParty that is a target of the extended API.
			/// </summary>
			private readonly CompanyDetailsRelatedParty mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="CompanyDetailsRelatedPartyFields" /> class with the specified content item of type CompanyDetailsRelatedParty.
			/// </summary>
			/// <param name="instance">The content item of type CompanyDetailsRelatedParty that is a target of the extended API.</param>
			public CompanyDetailsRelatedPartyFields(CompanyDetailsRelatedParty instance)
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
					return mInstance.CompanyDetailsRelatedPartyID;
				}
				set
				{
					mInstance.CompanyDetailsRelatedPartyID = value;
				}
			}


			/// <summary>
			/// Registry Id.
			/// </summary>
			public int PersonsRegistryID
			{
				get
				{
					return mInstance.PersonsRegistryID;
				}
				set
				{
					mInstance.PersonsRegistryID = value;
				}
			}


			/// <summary>
			/// CustomerCIF.
			/// </summary>
			public string CustomerCIF
			{
				get
				{
					return mInstance.CustomerCIF;
				}
				set
				{
					mInstance.CustomerCIF = value;
				}
			}


			/// <summary>
			/// Is Related Party a UBO?.
			/// </summary>
			public bool CompanyDetails_IsRelatedPartyUBO
			{
				get
				{
					return mInstance.CompanyDetails_IsRelatedPartyUBO;
				}
				set
				{
					mInstance.CompanyDetails_IsRelatedPartyUBO = value;
				}
			}


			/// <summary>
			/// ID Verified.
			/// </summary>
			public bool CompanyDetails_IdVerified
			{
				get
				{
					return mInstance.CompanyDetails_IdVerified;
				}
				set
				{
					mInstance.CompanyDetails_IdVerified = value;
				}
			}


			/// <summary>
			/// Invited.
			/// </summary>
			public int CompanyDetails_Invited
			{
				get
				{
					return mInstance.CompanyDetails_Invited;
				}
				set
				{
					mInstance.CompanyDetails_Invited = value;
				}
			}


			/// <summary>
			/// Application Type.
			/// </summary>
			public string CompanyDetails_Type
			{
				get
				{
					return mInstance.CompanyDetails_Type;
				}
				set
				{
					mInstance.CompanyDetails_Type = value;
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
			/// Is he/she a PEP?.
			/// </summary>
			public bool CompanyDetails_IsPep
			{
				get
				{
					return mInstance.CompanyDetails_IsPep;
				}
				set
				{
					mInstance.CompanyDetails_IsPep = value;
				}
			}


			/// <summary>
			/// Is he/she an immediate family member or close associate a PEP?.
			/// </summary>
			public bool CompanyDetails_IsRelatedToPep
			{
				get
				{
					return mInstance.CompanyDetails_IsRelatedToPep;
				}
				set
				{
					mInstance.CompanyDetails_IsRelatedToPep = value;
				}
			}


			/// <summary>
			/// Status.
			/// </summary>
			public string CompanyDetails_Status
			{
				get
				{
					return mInstance.CompanyDetails_Status;
				}
				set
				{
					mInstance.CompanyDetails_Status = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyDetailsRelatedParty" /> class.
		/// </summary>
		public CompanyDetailsRelatedParty() : base(CLASS_NAME)
		{
			mFields = new CompanyDetailsRelatedPartyFields(this);
		}

		#endregion
	}
}