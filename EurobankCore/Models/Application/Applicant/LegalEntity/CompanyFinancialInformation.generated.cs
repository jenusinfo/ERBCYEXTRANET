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

[assembly: RegisterDocumentType(CompanyFinancialInformation.CLASS_NAME, typeof(CompanyFinancialInformation))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type CompanyFinancialInformation.
	/// </summary>
	public partial class CompanyFinancialInformation : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.CompanyFinancialInformation";


		/// <summary>
		/// The instance of the class that provides extended API for working with CompanyFinancialInformation fields.
		/// </summary>
		private readonly CompanyFinancialInformationFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// FinancialInformationID.
		/// </summary>
		[DatabaseIDField]
		public int CompanyFinancialInformationID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("CompanyFinancialInformationID"), 0);
			}
			set
			{
				SetValue("CompanyFinancialInformationID", value);
			}
		}


		/// <summary>
		/// Annual Turnover (€) .
		/// </summary>
		[DatabaseField]
		public decimal FinancialInformation_Turnover
		{
			get
			{
				return ValidationHelper.GetDecimal(GetValue("FinancialInformation_Turnover"), 0);
			}
			set
			{
				SetValue("FinancialInformation_Turnover", value);
			}
		}


		/// <summary>
		/// Net Profit & Loss(€) .
		/// </summary>
		[DatabaseField]
		public decimal FinancialInformation_NetProfitAndLoss
		{
			get
			{
				return ValidationHelper.GetDecimal(GetValue("FinancialInformation_NetProfitAndLoss"), 0);
			}
			set
			{
				SetValue("FinancialInformation_NetProfitAndLoss", value);
			}
		}


		/// <summary>
		/// Total Assets(€) .
		/// </summary>
		[DatabaseField]
		public decimal FinancialInformation_TotalAssets
		{
			get
			{
				return ValidationHelper.GetDecimal(GetValue("FinancialInformation_TotalAssets"), 0);
			}
			set
			{
				SetValue("FinancialInformation_TotalAssets", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with CompanyFinancialInformation fields.
		/// </summary>
		[RegisterProperty]
		public CompanyFinancialInformationFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with CompanyFinancialInformation fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class CompanyFinancialInformationFields : AbstractHierarchicalObject<CompanyFinancialInformationFields>
		{
			/// <summary>
			/// The content item of type CompanyFinancialInformation that is a target of the extended API.
			/// </summary>
			private readonly CompanyFinancialInformation mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="CompanyFinancialInformationFields" /> class with the specified content item of type CompanyFinancialInformation.
			/// </summary>
			/// <param name="instance">The content item of type CompanyFinancialInformation that is a target of the extended API.</param>
			public CompanyFinancialInformationFields(CompanyFinancialInformation instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// FinancialInformationID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.CompanyFinancialInformationID;
				}
				set
				{
					mInstance.CompanyFinancialInformationID = value;
				}
			}


			/// <summary>
			/// Annual Turnover (€) .
			/// </summary>
			public decimal FinancialInformation_Turnover
			{
				get
				{
					return mInstance.FinancialInformation_Turnover;
				}
				set
				{
					mInstance.FinancialInformation_Turnover = value;
				}
			}


			/// <summary>
			/// Net Profit & Loss(€) .
			/// </summary>
			public decimal FinancialInformation_NetProfitAndLoss
			{
				get
				{
					return mInstance.FinancialInformation_NetProfitAndLoss;
				}
				set
				{
					mInstance.FinancialInformation_NetProfitAndLoss = value;
				}
			}


			/// <summary>
			/// Total Assets(€) .
			/// </summary>
			public decimal FinancialInformation_TotalAssets
			{
				get
				{
					return mInstance.FinancialInformation_TotalAssets;
				}
				set
				{
					mInstance.FinancialInformation_TotalAssets = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyFinancialInformation" /> class.
		/// </summary>
		public CompanyFinancialInformation() : base(CLASS_NAME)
		{
			mFields = new CompanyFinancialInformationFields(this);
		}

		#endregion
	}
}