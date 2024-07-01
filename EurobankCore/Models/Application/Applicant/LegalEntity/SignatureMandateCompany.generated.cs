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

[assembly: RegisterDocumentType(SignatureMandateCompany.CLASS_NAME, typeof(SignatureMandateCompany))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type SignatureMandateCompany.
	/// </summary>
	public partial class SignatureMandateCompany : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.SignatureMandateCompany";


		/// <summary>
		/// The instance of the class that provides extended API for working with SignatureMandateCompany fields.
		/// </summary>
		private readonly SignatureMandateCompanyFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// SignatureMandateCompanyID.
		/// </summary>
		[DatabaseIDField]
		public int SignatureMandateCompanyID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SignatureMandateCompanyID"), 0);
			}
			set
			{
				SetValue("SignatureMandateCompanyID", value);
			}
		}


		/// <summary>
		/// Name.
		/// </summary>
		[DatabaseField]
		public string Name
		{
			get
			{
				return ValidationHelper.GetString(GetValue("Name"), @"");
			}
			set
			{
				SetValue("Name", value);
			}
		}


		/// <summary>
		/// Limit From.
		/// </summary>
		[DatabaseField]
		public decimal SignatureMandateCompany_LimitFrom
		{
			get
			{
				return ValidationHelper.GetDecimal(GetValue("SignatureMandateCompany_LimitFrom"), 0);
			}
			set
			{
				SetValue("SignatureMandateCompany_LimitFrom", value);
			}
		}


		/// <summary>
		/// Limit To.
		/// </summary>
		[DatabaseField]
		public decimal SignatureMandateCompany_LimitTo
		{
			get
			{
				return ValidationHelper.GetDecimal(GetValue("SignatureMandateCompany_LimitTo"), 0);
			}
			set
			{
				SetValue("SignatureMandateCompany_LimitTo", value);
			}
		}


		/// <summary>
		/// Total Number of Signature.
		/// </summary>
		[DatabaseField]
		public int SignatureMandateCompany_TotalNumberofSignature
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SignatureMandateCompany_TotalNumberofSignature"), 0);
			}
			set
			{
				SetValue("SignatureMandateCompany_TotalNumberofSignature", value);
			}
		}


		/// <summary>
		/// Authorized Signatory Group.
		/// </summary>
		[DatabaseField]
		public string SignatureMandateCompany_AuthorizedSignatoryGroup
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SignatureMandateCompany_AuthorizedSignatoryGroup"), @"");
			}
			set
			{
				SetValue("SignatureMandateCompany_AuthorizedSignatoryGroup", value);
			}
		}


		/// <summary>
		/// Number of Signatures.
		/// </summary>
		[DatabaseField]
		public int SignatureMandateCompany_NumberofSignatures
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SignatureMandateCompany_NumberofSignatures"), 0);
			}
			set
			{
				SetValue("SignatureMandateCompany_NumberofSignatures", value);
			}
		}


		/// <summary>
		/// Rights.
		/// </summary>
		[DatabaseField]
		public string SignatureMandateCompany_Rights
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SignatureMandateCompany_Rights"), @"");
			}
			set
			{
				SetValue("SignatureMandateCompany_Rights", value);
			}
		}


		/// <summary>
		/// Authorized Signatory Group.
		/// </summary>
		[DatabaseField]
		public string SignatureMandateCompany_AuthorizedSignatoryGroup1
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SignatureMandateCompany_AuthorizedSignatoryGroup1"), @"");
			}
			set
			{
				SetValue("SignatureMandateCompany_AuthorizedSignatoryGroup1", value);
			}
		}


		/// <summary>
		/// Number of Signatures.
		/// </summary>
		[DatabaseField]
		public int SignatureMandateCompany_NumberofSignatures1
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("SignatureMandateCompany_NumberofSignatures1"), 0);
			}
			set
			{
				SetValue("SignatureMandateCompany_NumberofSignatures1", value);
			}
		}


		/// <summary>
		/// Mandate Type.
		/// </summary>
		[DatabaseField]
		public string SignatureMandateCompany_Mandatetype
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SignatureMandateCompany_Mandatetype"), @"");
			}
			set
			{
				SetValue("SignatureMandateCompany_Mandatetype", value);
			}
		}


		/// <summary>
		/// Description.
		/// </summary>
		[DatabaseField]
		public string SignatureMandateCompany_Description
		{
			get
			{
				return ValidationHelper.GetString(GetValue("SignatureMandateCompany_Description"), @"");
			}
			set
			{
				SetValue("SignatureMandateCompany_Description", value);
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
		/// Gets an object that provides extended API for working with SignatureMandateCompany fields.
		/// </summary>
		[RegisterProperty]
		public SignatureMandateCompanyFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with SignatureMandateCompany fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class SignatureMandateCompanyFields : AbstractHierarchicalObject<SignatureMandateCompanyFields>
		{
			/// <summary>
			/// The content item of type SignatureMandateCompany that is a target of the extended API.
			/// </summary>
			private readonly SignatureMandateCompany mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="SignatureMandateCompanyFields" /> class with the specified content item of type SignatureMandateCompany.
			/// </summary>
			/// <param name="instance">The content item of type SignatureMandateCompany that is a target of the extended API.</param>
			public SignatureMandateCompanyFields(SignatureMandateCompany instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// SignatureMandateCompanyID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.SignatureMandateCompanyID;
				}
				set
				{
					mInstance.SignatureMandateCompanyID = value;
				}
			}


			/// <summary>
			/// Name.
			/// </summary>
			public string Name
			{
				get
				{
					return mInstance.Name;
				}
				set
				{
					mInstance.Name = value;
				}
			}


			/// <summary>
			/// Limit From.
			/// </summary>
			public decimal _LimitFrom
			{
				get
				{
					return mInstance.SignatureMandateCompany_LimitFrom;
				}
				set
				{
					mInstance.SignatureMandateCompany_LimitFrom = value;
				}
			}


			/// <summary>
			/// Limit To.
			/// </summary>
			public decimal _LimitTo
			{
				get
				{
					return mInstance.SignatureMandateCompany_LimitTo;
				}
				set
				{
					mInstance.SignatureMandateCompany_LimitTo = value;
				}
			}


			/// <summary>
			/// Total Number of Signature.
			/// </summary>
			public int _TotalNumberofSignature
			{
				get
				{
					return mInstance.SignatureMandateCompany_TotalNumberofSignature;
				}
				set
				{
					mInstance.SignatureMandateCompany_TotalNumberofSignature = value;
				}
			}


			/// <summary>
			/// Authorized Signatory Group.
			/// </summary>
			public string _AuthorizedSignatoryGroup
			{
				get
				{
					return mInstance.SignatureMandateCompany_AuthorizedSignatoryGroup;
				}
				set
				{
					mInstance.SignatureMandateCompany_AuthorizedSignatoryGroup = value;
				}
			}


			/// <summary>
			/// Number of Signatures.
			/// </summary>
			public int _NumberofSignatures
			{
				get
				{
					return mInstance.SignatureMandateCompany_NumberofSignatures;
				}
				set
				{
					mInstance.SignatureMandateCompany_NumberofSignatures = value;
				}
			}


			/// <summary>
			/// Rights.
			/// </summary>
			public string _Rights
			{
				get
				{
					return mInstance.SignatureMandateCompany_Rights;
				}
				set
				{
					mInstance.SignatureMandateCompany_Rights = value;
				}
			}


			/// <summary>
			/// Authorized Signatory Group.
			/// </summary>
			public string _AuthorizedSignatoryGroup1
			{
				get
				{
					return mInstance.SignatureMandateCompany_AuthorizedSignatoryGroup1;
				}
				set
				{
					mInstance.SignatureMandateCompany_AuthorizedSignatoryGroup1 = value;
				}
			}


			/// <summary>
			/// Number of Signatures.
			/// </summary>
			public int _NumberofSignatures1
			{
				get
				{
					return mInstance.SignatureMandateCompany_NumberofSignatures1;
				}
				set
				{
					mInstance.SignatureMandateCompany_NumberofSignatures1 = value;
				}
			}


			/// <summary>
			/// Mandate Type.
			/// </summary>
			public string _Mandatetype
			{
				get
				{
					return mInstance.SignatureMandateCompany_Mandatetype;
				}
				set
				{
					mInstance.SignatureMandateCompany_Mandatetype = value;
				}
			}


			/// <summary>
			/// Description.
			/// </summary>
			public string _Description
			{
				get
				{
					return mInstance.SignatureMandateCompany_Description;
				}
				set
				{
					mInstance.SignatureMandateCompany_Description = value;
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
		/// Initializes a new instance of the <see cref="SignatureMandateCompany" /> class.
		/// </summary>
		public SignatureMandateCompany() : base(CLASS_NAME)
		{
			mFields = new SignatureMandateCompanyFields(this);
		}

		#endregion
	}
}