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

[assembly: RegisterDocumentType(SignatureMandateIndividual.CLASS_NAME, typeof(SignatureMandateIndividual))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type SignatureMandateIndividual.
	/// </summary>
	public partial class SignatureMandateIndividual : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.SignatureMandateIndividual";


		/// <summary>
		/// The instance of the class that provides extended API for working with SignatureMandateIndividual fields.
		/// </summary>
		private readonly SignatureMandateIndividualFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// SignatureMandateIndividualID.
		/// </summary>
		[DatabaseIDField]
		public int SignatureMandateIndividualID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SignatureMandateIndividualID"), 0);
			}
			set
			{
				SetValue("SignatureMandateIndividualID", value);
			}
		}


		/// <summary>
		/// Signatory Persons.
		/// </summary>
		[DatabaseField]
		public string SignatureMandateIndividual_SignatoryPersons
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SignatureMandateIndividual_SignatoryPersons"), @"");
			}
			set
			{
				SetValue("SignatureMandateIndividual_SignatoryPersons", value);
			}
		}


		/// <summary>
		/// Number of Signatures.
		/// </summary>
		[DatabaseField]
		public int SignatureMandateIndividual_NumberOfSignatures
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SignatureMandateIndividual_NumberOfSignatures"), 0);
			}
			set
			{
				SetValue("SignatureMandateIndividual_NumberOfSignatures", value);
			}
		}


		/// <summary>
		/// SignatureMandateIndividual Status.
		/// </summary>
		[DatabaseField]
		public bool SignatureMandateIndividual_Status
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("SignatureMandateIndividual_Status"), false);
			}
			set
			{
				SetValue("SignatureMandateIndividual_Status", value);
			}
		}


		/// <summary>
		/// Access Rights.
		/// </summary>
		[DatabaseField]
		public Guid SignatureMandateIndividual_AccessRights
		{
			get
			{
				return ValidationHelper.GetGuid(GetValue("SignatureMandateIndividual_AccessRights"), Guid.Empty);
			}
			set
			{
				SetValue("SignatureMandateIndividual_AccessRights", value);
			}
		}


		/// <summary>
		/// Amount From.
		/// </summary>
		[DatabaseField]
		public decimal SignatureMandateIndividual_AmountFrom
		{
			get
			{
				return ValidationHelper.GetDecimal(GetValue("SignatureMandateIndividual_AmountFrom"), 0);
			}
			set
			{
				SetValue("SignatureMandateIndividual_AmountFrom", value);
			}
		}


		/// <summary>
		/// Amount To.
		/// </summary>
		[DatabaseField]
		public decimal SignatureMandateIndividual_AmountTo
		{
			get
			{
				return ValidationHelper.GetDecimal(GetValue("SignatureMandateIndividual_AmountTo"), 0);
			}
			set
			{
				SetValue("SignatureMandateIndividual_AmountTo", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with SignatureMandateIndividual fields.
		/// </summary>
		[RegisterProperty]
		public SignatureMandateIndividualFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with SignatureMandateIndividual fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class SignatureMandateIndividualFields : AbstractHierarchicalObject<SignatureMandateIndividualFields>
		{
			/// <summary>
			/// The content item of type SignatureMandateIndividual that is a target of the extended API.
			/// </summary>
			private readonly SignatureMandateIndividual mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="SignatureMandateIndividualFields" /> class with the specified content item of type SignatureMandateIndividual.
			/// </summary>
			/// <param name="instance">The content item of type SignatureMandateIndividual that is a target of the extended API.</param>
			public SignatureMandateIndividualFields(SignatureMandateIndividual instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// SignatureMandateIndividualID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.SignatureMandateIndividualID;
				}
				set
				{
					mInstance.SignatureMandateIndividualID = value;
				}
			}


			/// <summary>
			/// Signatory Persons.
			/// </summary>
			public string _SignatoryPersons
			{
				get
				{
					return mInstance.SignatureMandateIndividual_SignatoryPersons;
				}
				set
				{
					mInstance.SignatureMandateIndividual_SignatoryPersons = value;
				}
			}


			/// <summary>
			/// Number of Signatures.
			/// </summary>
			public int _NumberOfSignatures
			{
				get
				{
					return mInstance.SignatureMandateIndividual_NumberOfSignatures;
				}
				set
				{
					mInstance.SignatureMandateIndividual_NumberOfSignatures = value;
				}
			}


			/// <summary>
			/// SignatureMandateIndividual Status.
			/// </summary>
			public bool _Status
			{
				get
				{
					return mInstance.SignatureMandateIndividual_Status;
				}
				set
				{
					mInstance.SignatureMandateIndividual_Status = value;
				}
			}


			/// <summary>
			/// Access Rights.
			/// </summary>
			public Guid _AccessRights
			{
				get
				{
					return mInstance.SignatureMandateIndividual_AccessRights;
				}
				set
				{
					mInstance.SignatureMandateIndividual_AccessRights = value;
				}
			}


			/// <summary>
			/// Amount From.
			/// </summary>
			public decimal _AmountFrom
			{
				get
				{
					return mInstance.SignatureMandateIndividual_AmountFrom;
				}
				set
				{
					mInstance.SignatureMandateIndividual_AmountFrom = value;
				}
			}


			/// <summary>
			/// Amount To.
			/// </summary>
			public decimal _AmountTo
			{
				get
				{
					return mInstance.SignatureMandateIndividual_AmountTo;
				}
				set
				{
					mInstance.SignatureMandateIndividual_AmountTo = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="SignatureMandateIndividual" /> class.
		/// </summary>
		public SignatureMandateIndividual() : base(CLASS_NAME)
		{
			mFields = new SignatureMandateIndividualFields(this);
		}

		#endregion
	}
}