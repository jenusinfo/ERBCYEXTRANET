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

[assembly: RegisterDocumentType(SourceOfIncome.CLASS_NAME, typeof(SourceOfIncome))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type SourceOfIncome.
	/// </summary>
	public partial class SourceOfIncome : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.SourceOfIncome";


		/// <summary>
		/// The instance of the class that provides extended API for working with SourceOfIncome fields.
		/// </summary>
		private readonly SourceOfIncomeFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// SourceOfIncomeID.
		/// </summary>
		[DatabaseIDField]
		public int SourceOfIncomeID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SourceOfIncomeID"), 0);
			}
			set
			{
				SetValue("SourceOfIncomeID", value);
			}
		}


		/// <summary>
		/// Source of Annual Income.
		/// </summary>
		[DatabaseField]
		public Guid SourceOfIncome_SourceOfAnnualIncome
		{
			get
			{
				return ValidationHelper.GetGuid(GetValue("SourceOfIncome_SourceOfAnnualIncome"), Guid.Empty);
			}
			set
			{
				SetValue("SourceOfIncome_SourceOfAnnualIncome", value);
			}
		}


		/// <summary>
		/// Specify Other Source.
		/// </summary>
		[DatabaseField]
		public string SourceOfIncome_SpecifyOtherSource
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SourceOfIncome_SpecifyOtherSource"), @"");
			}
			set
			{
				SetValue("SourceOfIncome_SpecifyOtherSource", value);
			}
		}


		/// <summary>
		/// Amount of Income.
		/// </summary>
		[DatabaseField]
		public double SourceOfIncome_AmountOfIncome
		{
			get
			{
				return ValidationHelper.GetDouble(GetValue("SourceOfIncome_AmountOfIncome"), 0);
			}
			set
			{
				SetValue("SourceOfIncome_AmountOfIncome", value);
			}
		}


		/// <summary>
		/// Is Confirmed.
		/// </summary>
		[DatabaseField]
		public bool Status
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("Status"), false);
			}
			set
			{
				SetValue("Status", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with SourceOfIncome fields.
		/// </summary>
		[RegisterProperty]
		public SourceOfIncomeFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with SourceOfIncome fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class SourceOfIncomeFields : AbstractHierarchicalObject<SourceOfIncomeFields>
		{
			/// <summary>
			/// The content item of type SourceOfIncome that is a target of the extended API.
			/// </summary>
			private readonly SourceOfIncome mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="SourceOfIncomeFields" /> class with the specified content item of type SourceOfIncome.
			/// </summary>
			/// <param name="instance">The content item of type SourceOfIncome that is a target of the extended API.</param>
			public SourceOfIncomeFields(SourceOfIncome instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// SourceOfIncomeID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.SourceOfIncomeID;
				}
				set
				{
					mInstance.SourceOfIncomeID = value;
				}
			}


			/// <summary>
			/// Source of Annual Income.
			/// </summary>
			public Guid _SourceOfAnnualIncome
			{
				get
				{
					return mInstance.SourceOfIncome_SourceOfAnnualIncome;
				}
				set
				{
					mInstance.SourceOfIncome_SourceOfAnnualIncome = value;
				}
			}


			/// <summary>
			/// Specify Other Source.
			/// </summary>
			public string _SpecifyOtherSource
			{
				get
				{
					return mInstance.SourceOfIncome_SpecifyOtherSource;
				}
				set
				{
					mInstance.SourceOfIncome_SpecifyOtherSource = value;
				}
			}


			/// <summary>
			/// Amount of Income.
			/// </summary>
			public double _AmountOfIncome
			{
				get
				{
					return mInstance.SourceOfIncome_AmountOfIncome;
				}
				set
				{
					mInstance.SourceOfIncome_AmountOfIncome = value;
				}
			}


			/// <summary>
			/// Is Confirmed.
			/// </summary>
			public bool Status
			{
				get
				{
					return mInstance.Status;
				}
				set
				{
					mInstance.Status = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="SourceOfIncome" /> class.
		/// </summary>
		public SourceOfIncome() : base(CLASS_NAME)
		{
			mFields = new SourceOfIncomeFields(this);
		}

		#endregion
	}
}