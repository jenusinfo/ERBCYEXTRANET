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

[assembly: RegisterDocumentType(GroupStructureLegalParent.CLASS_NAME, typeof(GroupStructureLegalParent))]

namespace CMS.DocumentEngine.Types.Eurobank
{
	/// <summary>
	/// Represents a content item of type GroupStructureLegalParent.
	/// </summary>
	public partial class GroupStructureLegalParent : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Eurobank.GroupStructureLegalParent";


		/// <summary>
		/// The instance of the class that provides extended API for working with GroupStructureLegalParent fields.
		/// </summary>
		private readonly GroupStructureLegalParentFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// GroupStructureLegalParentID.
		/// </summary>
		[DatabaseIDField]
		public int GroupStructureLegalParentID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("GroupStructureLegalParentID"), 0);
			}
			set
			{
				SetValue("GroupStructureLegalParentID", value);
			}
		}


		/// <summary>
		/// Does the entity belong to a group.
		/// </summary>
		[DatabaseField]
		public string DoesTheEntityBelongToAGroup
		{
			get
			{
				return ValidationHelper.GetString(GetValue("DoesTheEntityBelongToAGroup"), @"");
			}
			set
			{
				SetValue("DoesTheEntityBelongToAGroup", value);
			}
		}


		/// <summary>
		/// Group Name.
		/// </summary>
		[DatabaseField]
		public string GroupName
		{
			get
			{
				return ValidationHelper.GetString(GetValue("GroupName"), @"");
			}
			set
			{
				SetValue("GroupName", value);
			}
		}


		/// <summary>
		/// Group Activities.
		/// </summary>
		[DatabaseField]
		public string GroupActivities
		{
			get
			{
				return ValidationHelper.GetString(GetValue("GroupActivities"), @"");
			}
			set
			{
				SetValue("GroupActivities", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with GroupStructureLegalParent fields.
		/// </summary>
		[RegisterProperty]
		public GroupStructureLegalParentFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with GroupStructureLegalParent fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class GroupStructureLegalParentFields : AbstractHierarchicalObject<GroupStructureLegalParentFields>
		{
			/// <summary>
			/// The content item of type GroupStructureLegalParent that is a target of the extended API.
			/// </summary>
			private readonly GroupStructureLegalParent mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="GroupStructureLegalParentFields" /> class with the specified content item of type GroupStructureLegalParent.
			/// </summary>
			/// <param name="instance">The content item of type GroupStructureLegalParent that is a target of the extended API.</param>
			public GroupStructureLegalParentFields(GroupStructureLegalParent instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// GroupStructureLegalParentID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.GroupStructureLegalParentID;
				}
				set
				{
					mInstance.GroupStructureLegalParentID = value;
				}
			}


			/// <summary>
			/// Does the entity belong to a group.
			/// </summary>
			public string DoesTheEntityBelongToAGroup
			{
				get
				{
					return mInstance.DoesTheEntityBelongToAGroup;
				}
				set
				{
					mInstance.DoesTheEntityBelongToAGroup = value;
				}
			}


			/// <summary>
			/// Group Name.
			/// </summary>
			public string GroupName
			{
				get
				{
					return mInstance.GroupName;
				}
				set
				{
					mInstance.GroupName = value;
				}
			}


			/// <summary>
			/// Group Activities.
			/// </summary>
			public string GroupActivities
			{
				get
				{
					return mInstance.GroupActivities;
				}
				set
				{
					mInstance.GroupActivities = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="GroupStructureLegalParent" /> class.
		/// </summary>
		public GroupStructureLegalParent() : base(CLASS_NAME)
		{
			mFields = new GroupStructureLegalParentFields(this);
		}

		#endregion
	}
}