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

[assembly: RegisterDocumentType(SourceOfIncomingTransactions.CLASS_NAME, typeof(SourceOfIncomingTransactions))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type SourceOfIncomingTransactions.
	/// </summary>
	public partial class SourceOfIncomingTransactions : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.SourceOfIncomingTransactions";


		/// <summary>
		/// The instance of the class that provides extended API for working with SourceOfIncomingTransactions fields.
		/// </summary>
		private readonly SourceOfIncomingTransactionsFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// SourceOfIncomingTransactionsID.
		/// </summary>
		[DatabaseIDField]
		public int SourceOfIncomingTransactionsID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SourceOfIncomingTransactionsID"), 0);
			}
			set
			{
				SetValue("SourceOfIncomingTransactionsID", value);
			}
		}


		/// <summary>
		/// Name of Remitter.
		/// </summary>
		[DatabaseField]
		public string SourceOfIncomingTransactions_NameOfRemitter
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SourceOfIncomingTransactions_NameOfRemitter"), @"");
			}
			set
			{
				SetValue("SourceOfIncomingTransactions_NameOfRemitter", value);
			}
		}


		/// <summary>
		/// Country of Remitter.
		/// </summary>
		[DatabaseField]
		public string SourceOfIncomingTransactions_CountryOfRemitter
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SourceOfIncomingTransactions_CountryOfRemitter"), @"");
			}
			set
			{
				SetValue("SourceOfIncomingTransactions_CountryOfRemitter", value);
			}
		}


		/// <summary>
		/// Country of Remitter's Bank.
		/// </summary>
		[DatabaseField]
		public string SourceOfIncomingTransactions_CountryOfRemitterBank
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SourceOfIncomingTransactions_CountryOfRemitterBank"), @"");
			}
			set
			{
				SetValue("SourceOfIncomingTransactions_CountryOfRemitterBank", value);
			}
		}


		/// <summary>
		/// Source Of IncomingTransactions Status.
		/// </summary>
		[DatabaseField]
		public bool SourceOfIncomingTransactions_Status
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("SourceOfIncomingTransactions_Status"), false);
			}
			set
			{
				SetValue("SourceOfIncomingTransactions_Status", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with SourceOfIncomingTransactions fields.
		/// </summary>
		[RegisterProperty]
		public SourceOfIncomingTransactionsFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with SourceOfIncomingTransactions fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class SourceOfIncomingTransactionsFields : AbstractHierarchicalObject<SourceOfIncomingTransactionsFields>
		{
			/// <summary>
			/// The content item of type SourceOfIncomingTransactions that is a target of the extended API.
			/// </summary>
			private readonly SourceOfIncomingTransactions mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="SourceOfIncomingTransactionsFields" /> class with the specified content item of type SourceOfIncomingTransactions.
			/// </summary>
			/// <param name="instance">The content item of type SourceOfIncomingTransactions that is a target of the extended API.</param>
			public SourceOfIncomingTransactionsFields(SourceOfIncomingTransactions instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// SourceOfIncomingTransactionsID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.SourceOfIncomingTransactionsID;
				}
				set
				{
					mInstance.SourceOfIncomingTransactionsID = value;
				}
			}


			/// <summary>
			/// Name of Remitter.
			/// </summary>
			public string _NameOfRemitter
			{
				get
				{
					return mInstance.SourceOfIncomingTransactions_NameOfRemitter;
				}
				set
				{
					mInstance.SourceOfIncomingTransactions_NameOfRemitter = value;
				}
			}


			/// <summary>
			/// Country of Remitter.
			/// </summary>
			public string _CountryOfRemitter
			{
				get
				{
					return mInstance.SourceOfIncomingTransactions_CountryOfRemitter;
				}
				set
				{
					mInstance.SourceOfIncomingTransactions_CountryOfRemitter = value;
				}
			}


			/// <summary>
			/// Country of Remitter's Bank.
			/// </summary>
			public string _CountryOfRemitterBank
			{
				get
				{
					return mInstance.SourceOfIncomingTransactions_CountryOfRemitterBank;
				}
				set
				{
					mInstance.SourceOfIncomingTransactions_CountryOfRemitterBank = value;
				}
			}


			/// <summary>
			/// Source Of IncomingTransactions Status.
			/// </summary>
			public bool _Status
			{
				get
				{
					return mInstance.SourceOfIncomingTransactions_Status;
				}
				set
				{
					mInstance.SourceOfIncomingTransactions_Status = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="SourceOfIncomingTransactions" /> class.
		/// </summary>
		public SourceOfIncomingTransactions() : base(CLASS_NAME)
		{
			mFields = new SourceOfIncomingTransactionsFields(this);
		}

		#endregion
	}
}