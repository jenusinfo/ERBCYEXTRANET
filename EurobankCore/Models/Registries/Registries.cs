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

[assembly: RegisterDocumentType(Registries.CLASS_NAME, typeof(Registries))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type Registries.
	/// </summary>
	public partial class Registries : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.Registries";


		/// <summary>
		/// The instance of the class that provides extended API for working with Registries fields.
		/// </summary>
		private readonly RegistriesFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// RegistriesID.
		/// </summary>
		[DatabaseIDField]
		public int RegistriesID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("RegistriesID"), 0);
			}
			set
			{
				SetValue("RegistriesID", value);
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
		/// Gets an object that provides extended API for working with Registries fields.
		/// </summary>
		[RegisterProperty]
		public RegistriesFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with Registries fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class RegistriesFields : AbstractHierarchicalObject<RegistriesFields>
		{
			/// <summary>
			/// The content item of type Registries that is a target of the extended API.
			/// </summary>
			private readonly Registries mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="RegistriesFields" /> class with the specified content item of type Registries.
			/// </summary>
			/// <param name="instance">The content item of type Registries that is a target of the extended API.</param>
			public RegistriesFields(Registries instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// RegistriesID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.RegistriesID;
				}
				set
				{
					mInstance.RegistriesID = value;
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
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="Registries" /> class.
		/// </summary>
		public Registries() : base(CLASS_NAME)
		{
			mFields = new RegistriesFields(this);
		}

		#endregion
	}
}