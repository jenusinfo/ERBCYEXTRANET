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

[assembly: RegisterDocumentType(ContactDetailsLegal.CLASS_NAME, typeof(ContactDetailsLegal))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type ContactDetailsLegal.
	/// </summary>
	public partial class ContactDetailsLegal : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.ContactDetailsLegal";


		/// <summary>
		/// The instance of the class that provides extended API for working with ContactDetailsLegal fields.
		/// </summary>
		private readonly ContactDetailsLegalFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// ContactDetailsLegalID.
		/// </summary>
		[DatabaseIDField]
		public int ContactDetailsLegalID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("ContactDetailsLegalID"), 0);
			}
			set
			{
				SetValue("ContactDetailsLegalID", value);
			}
		}


		/// <summary>
		/// Preferred Mailing Address.
		/// </summary>
		[DatabaseField]
		public Guid ContactDetailsLegal_PreferredMailingAddress
		{
			get
			{
				return ValidationHelper.GetGuid(GetValue("ContactDetailsLegal_PreferredMailingAddress"), Guid.Empty);
			}
			set
			{
				SetValue("ContactDetailsLegal_PreferredMailingAddress", value);
			}
		}


		/// <summary>
		/// Email Address for sendig Alerts.
		/// </summary>
		[DatabaseField]
		public string ContactDetailsLegal_EmailAddressForSendingAlerts
		{
			get
			{
				return ValidationHelper.GetString(GetValue("ContactDetailsLegal_EmailAddressForSendingAlerts"), @"");
			}
			set
			{
				SetValue("ContactDetailsLegal_EmailAddressForSendingAlerts", value);
			}
		}


		/// <summary>
		/// Preferred Communication Language.
		/// </summary>
		[DatabaseField]
		public Guid ContactDetailsLegal_PreferredCommunicationLanguage
		{
			get
			{
				return ValidationHelper.GetGuid(GetValue("ContactDetailsLegal_PreferredCommunicationLanguage"), Guid.Empty);
			}
			set
			{
				SetValue("ContactDetailsLegal_PreferredCommunicationLanguage", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with ContactDetailsLegal fields.
		/// </summary>
		[RegisterProperty]
		public ContactDetailsLegalFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with ContactDetailsLegal fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class ContactDetailsLegalFields : AbstractHierarchicalObject<ContactDetailsLegalFields>
		{
			/// <summary>
			/// The content item of type ContactDetailsLegal that is a target of the extended API.
			/// </summary>
			private readonly ContactDetailsLegal mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="ContactDetailsLegalFields" /> class with the specified content item of type ContactDetailsLegal.
			/// </summary>
			/// <param name="instance">The content item of type ContactDetailsLegal that is a target of the extended API.</param>
			public ContactDetailsLegalFields(ContactDetailsLegal instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// ContactDetailsLegalID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.ContactDetailsLegalID;
				}
				set
				{
					mInstance.ContactDetailsLegalID = value;
				}
			}


			/// <summary>
			/// Preferred Mailing Address.
			/// </summary>
			public Guid _PreferredMailingAddress
			{
				get
				{
					return mInstance.ContactDetailsLegal_PreferredMailingAddress;
				}
				set
				{
					mInstance.ContactDetailsLegal_PreferredMailingAddress = value;
				}
			}


			/// <summary>
			/// Email Address for sendig Alerts.
			/// </summary>
			public string _EmailAddressForSendingAlerts
			{
				get
				{
					return mInstance.ContactDetailsLegal_EmailAddressForSendingAlerts;
				}
				set
				{
					mInstance.ContactDetailsLegal_EmailAddressForSendingAlerts = value;
				}
			}


			/// <summary>
			/// Preferred Communication Language.
			/// </summary>
			public Guid _PreferredCommunicationLanguage
			{
				get
				{
					return mInstance.ContactDetailsLegal_PreferredCommunicationLanguage;
				}
				set
				{
					mInstance.ContactDetailsLegal_PreferredCommunicationLanguage = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="ContactDetailsLegal" /> class.
		/// </summary>
		public ContactDetailsLegal() : base(CLASS_NAME)
		{
			mFields = new ContactDetailsLegalFields(this);
		}

		#endregion
	}
}