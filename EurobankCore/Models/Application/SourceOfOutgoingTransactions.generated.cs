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

[assembly: RegisterDocumentType(SourceOfOutgoingTransactions.CLASS_NAME, typeof(SourceOfOutgoingTransactions))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type SourceOfOutgoingTransactions.
	/// </summary>
	public partial class SourceOfOutgoingTransactions : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.SourceOfOutgoingTransactions";


		/// <summary>
		/// The instance of the class that provides extended API for working with SourceOfOutgoingTransactions fields.
		/// </summary>
		private readonly SourceOfOutgoingTransactionsFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// SourceOfOutgoingTransactionsID.
		/// </summary>
		[DatabaseIDField]
		public int SourceOfOutgoingTransactionsID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SourceOfOutgoingTransactionsID"), 0);
			}
			set
			{
				SetValue("SourceOfOutgoingTransactionsID", value);
			}
		}


		/// <summary>
		/// Name of Beneficiary.
		/// </summary>
		[DatabaseField]
		public string SourceOfOutgoingTransactions_NameOfBeneficiary
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SourceOfOutgoingTransactions_NameOfBeneficiary"), @"");
			}
			set
			{
				SetValue("SourceOfOutgoingTransactions_NameOfBeneficiary", value);
			}
		}


		/// <summary>
		/// Source Of Outgoing Transactions Status.
		/// </summary>
		[DatabaseField]
		public bool SourceOfOutgoingTransactions_Status
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("SourceOfOutgoingTransactions_Status"), false);
			}
			set
			{
				SetValue("SourceOfOutgoingTransactions_Status", value);
			}
		}


		/// <summary>
		/// Country of Beneficiary.
		/// </summary>
		[DatabaseField]
		public string SourceOfOutgoingTransactions_CountryOfBeneficiary
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SourceOfOutgoingTransactions_CountryOfBeneficiary"), @"");
			}
			set
			{
				SetValue("SourceOfOutgoingTransactions_CountryOfBeneficiary", value);
			}
		}


		/// <summary>
		/// Country of Beneficiary's Bank.
		/// </summary>
		[DatabaseField]
		public string SourceOfOutgoingTransactions_CountryOfBeneficiaryBank
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SourceOfOutgoingTransactions_CountryOfBeneficiaryBank"), @"");
			}
			set
			{
				SetValue("SourceOfOutgoingTransactions_CountryOfBeneficiaryBank", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with SourceOfOutgoingTransactions fields.
		/// </summary>
		[RegisterProperty]
		public SourceOfOutgoingTransactionsFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with SourceOfOutgoingTransactions fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class SourceOfOutgoingTransactionsFields : AbstractHierarchicalObject<SourceOfOutgoingTransactionsFields>
		{
			/// <summary>
			/// The content item of type SourceOfOutgoingTransactions that is a target of the extended API.
			/// </summary>
			private readonly SourceOfOutgoingTransactions mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="SourceOfOutgoingTransactionsFields" /> class with the specified content item of type SourceOfOutgoingTransactions.
			/// </summary>
			/// <param name="instance">The content item of type SourceOfOutgoingTransactions that is a target of the extended API.</param>
			public SourceOfOutgoingTransactionsFields(SourceOfOutgoingTransactions instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// SourceOfOutgoingTransactionsID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.SourceOfOutgoingTransactionsID;
				}
				set
				{
					mInstance.SourceOfOutgoingTransactionsID = value;
				}
			}


			/// <summary>
			/// Name of Beneficiary.
			/// </summary>
			public string _NameOfBeneficiary
			{
				get
				{
					return mInstance.SourceOfOutgoingTransactions_NameOfBeneficiary;
				}
				set
				{
					mInstance.SourceOfOutgoingTransactions_NameOfBeneficiary = value;
				}
			}


			/// <summary>
			/// Source Of Outgoing Transactions Status.
			/// </summary>
			public bool _Status
			{
				get
				{
					return mInstance.SourceOfOutgoingTransactions_Status;
				}
				set
				{
					mInstance.SourceOfOutgoingTransactions_Status = value;
				}
			}


			/// <summary>
			/// Country of Beneficiary.
			/// </summary>
			public string _CountryOfBeneficiary
			{
				get
				{
					return mInstance.SourceOfOutgoingTransactions_CountryOfBeneficiary;
				}
				set
				{
					mInstance.SourceOfOutgoingTransactions_CountryOfBeneficiary = value;
				}
			}


			/// <summary>
			/// Country of Beneficiary's Bank.
			/// </summary>
			public string _CountryOfBeneficiaryBank
			{
				get
				{
					return mInstance.SourceOfOutgoingTransactions_CountryOfBeneficiaryBank;
				}
				set
				{
					mInstance.SourceOfOutgoingTransactions_CountryOfBeneficiaryBank = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="SourceOfOutgoingTransactions" /> class.
		/// </summary>
		public SourceOfOutgoingTransactions() : base(CLASS_NAME)
		{
			mFields = new SourceOfOutgoingTransactionsFields(this);
		}

		#endregion
	}
}