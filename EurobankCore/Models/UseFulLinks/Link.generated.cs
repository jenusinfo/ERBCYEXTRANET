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

[assembly: RegisterDocumentType(Link.CLASS_NAME, typeof(Link))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type Link.
	/// </summary>
	public partial class Link : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.Link";


		/// <summary>
		/// The instance of the class that provides extended API for working with Link fields.
		/// </summary>
		private readonly LinkFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// LinkID.
		/// </summary>
		[DatabaseIDField]
		public int LinkID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("LinkID"), 0);
			}
			set
			{
				SetValue("LinkID", value);
			}
		}


		/// <summary>
		/// Text.
		/// </summary>
		[DatabaseField]
		public string LinkText
		{
			get
			{
				return ValidationHelper.GetString(GetValue("LinkText"), @"");
			}
			set
			{
				SetValue("LinkText", value);
			}
		}


		/// <summary>
		/// URL.
		/// </summary>
		[DatabaseField]
		public string LinkURL
		{
			get
			{
				return ValidationHelper.GetString(GetValue("LinkURL"), @"");
			}
			set
			{
				SetValue("LinkURL", value);
			}
		}


		/// <summary>
		/// Open link in new tab.
		/// </summary>
		[DatabaseField]
		public bool LinkOpenInNewTab
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("LinkOpenInNewTab"), false);
			}
			set
			{
				SetValue("LinkOpenInNewTab", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with Link fields.
		/// </summary>
		[RegisterProperty]
		public LinkFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with Link fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class LinkFields : AbstractHierarchicalObject<LinkFields>
		{
			/// <summary>
			/// The content item of type Link that is a target of the extended API.
			/// </summary>
			private readonly Link mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="LinkFields" /> class with the specified content item of type Link.
			/// </summary>
			/// <param name="instance">The content item of type Link that is a target of the extended API.</param>
			public LinkFields(Link instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// LinkID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.LinkID;
				}
				set
				{
					mInstance.LinkID = value;
				}
			}


			/// <summary>
			/// Text.
			/// </summary>
			public string Text
			{
				get
				{
					return mInstance.LinkText;
				}
				set
				{
					mInstance.LinkText = value;
				}
			}


			/// <summary>
			/// URL.
			/// </summary>
			public string URL
			{
				get
				{
					return mInstance.LinkURL;
				}
				set
				{
					mInstance.LinkURL = value;
				}
			}


			/// <summary>
			/// Open link in new tab.
			/// </summary>
			public bool OpenInNewTab
			{
				get
				{
					return mInstance.LinkOpenInNewTab;
				}
				set
				{
					mInstance.LinkOpenInNewTab = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="Link" /> class.
		/// </summary>
		public Link() : base(CLASS_NAME)
		{
			mFields = new LinkFields(this);
		}

		#endregion
	}
}