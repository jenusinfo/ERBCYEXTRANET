//--------------------------------------------------------------------------------------------------
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

[assembly: RegisterDocumentType(ManufacturerSection.CLASS_NAME, typeof(ManufacturerSection))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type ManufacturerSection.
	/// </summary>
	public partial class ManufacturerSection : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.ManufacturerSection";


		/// <summary>
		/// The instance of the class that provides extended API for working with ManufacturerSection fields.
		/// </summary>
		private readonly ManufacturerSectionFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// Gets an object that provides extended API for working with ManufacturerSection fields.
		/// </summary>
		[RegisterProperty]
		public ManufacturerSectionFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with ManufacturerSection fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class ManufacturerSectionFields : AbstractHierarchicalObject<ManufacturerSectionFields>
		{
			/// <summary>
			/// The content item of type ManufacturerSection that is a target of the extended API.
			/// </summary>
			private readonly ManufacturerSection mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="ManufacturerSectionFields" /> class with the specified content item of type ManufacturerSection.
			/// </summary>
			/// <param name="instance">The content item of type ManufacturerSection that is a target of the extended API.</param>
			public ManufacturerSectionFields(ManufacturerSection instance)
			{
				mInstance = instance;
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="ManufacturerSection" /> class.
		/// </summary>
		public ManufacturerSection() : base(CLASS_NAME)
		{
			mFields = new ManufacturerSectionFields(this);
		}

		#endregion
	}
}