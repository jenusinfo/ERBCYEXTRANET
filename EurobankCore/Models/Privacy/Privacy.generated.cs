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

using CMS;
using CMS.Base;
using CMS.DocumentEngine.Types.Eurobank;

[assembly: RegisterDocumentType(Privacy.CLASS_NAME, typeof(Privacy))]

namespace CMS.DocumentEngine.Types.Eurobank
{
    /// <summary>
    /// Represents a content item of type Privacy.
    /// </summary>
    public partial class Privacy : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.Privacy";


		/// <summary>
		/// The instance of the class that provides extended API for working with Privacy fields.
		/// </summary>
		private readonly PrivacyFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// Gets an object that provides extended API for working with Privacy fields.
		/// </summary>
		[RegisterProperty]
		public PrivacyFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with Privacy fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class PrivacyFields : AbstractHierarchicalObject<PrivacyFields>
		{
			/// <summary>
			/// The content item of type Privacy that is a target of the extended API.
			/// </summary>
			private readonly Privacy mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="PrivacyFields" /> class with the specified content item of type Privacy.
			/// </summary>
			/// <param name="instance">The content item of type Privacy that is a target of the extended API.</param>
			public PrivacyFields(Privacy instance)
			{
				mInstance = instance;
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="Privacy" /> class.
		/// </summary>
		public Privacy() : base(CLASS_NAME)
		{
			mFields = new PrivacyFields(this);
		}

		#endregion
	}
}