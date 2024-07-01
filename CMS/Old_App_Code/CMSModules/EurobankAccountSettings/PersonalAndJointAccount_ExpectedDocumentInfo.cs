using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using EurobankAccountSettings;

[assembly: RegisterObjectType(typeof(PersonalAndJointAccount_ExpectedDocumentInfo), PersonalAndJointAccount_ExpectedDocumentInfo.OBJECT_TYPE)]

namespace EurobankAccountSettings
{
    /// <summary>
    /// Data container class for <see cref="PersonalAndJointAccount_ExpectedDocumentInfo"/>.
    /// </summary>
    [Serializable]
    public partial class PersonalAndJointAccount_ExpectedDocumentInfo : AbstractInfo<PersonalAndJointAccount_ExpectedDocumentInfo, IPersonalAndJointAccount_ExpectedDocumentInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "eurobankaccountsettings.personalandjointaccount_expecteddocument";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(PersonalAndJointAccount_ExpectedDocumentInfoProvider), OBJECT_TYPE, "EurobankAccountSettings.PersonalAndJointAccount_ExpectedDocument", "PersonalAndJointAccount_ExpectedDocumentID", "PersonalAndJointAccount_ExpectedDocumentLastModified", "PersonalAndJointAccount_ExpectedDocumentGuid", null, null, null, null, null, null)
        {
            ModuleName = "EurobankAccountSettings",
            TouchCacheDependencies = true,
            ImportExportSettings =
            {
                IsExportable = true, // Makes the data of the custom Office class exportable
				AllowSingleExport = true, // Allows export of single office objects from the office listing page
				ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
					// Creates a new category in the global objects export interface
					new ObjectTreeLocation(GLOBAL, "EurobankAccountSettings")
                }
            },
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.LogSynchronization, // Enables logging of staging tasks for changes made to Office objects
				ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
					// Creates a new category in the 'Global objects' section of the staging object tree
					new ObjectTreeLocation(GLOBAL, "EurobankAccountSettings")
                }
            }
        };


        /// <summary>
        /// Personal and joint account expected document ID.
        /// </summary>
        [DatabaseField]
        public virtual int PersonalAndJointAccount_ExpectedDocumentID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PersonalAndJointAccount_ExpectedDocumentID"), 0);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocumentID", value);
            }
        }


        /// <summary>
        /// Personal and joint account expected document person type.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_ExpectedDocument_PersonType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_ExpectedDocument_PersonType"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocument_PersonType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account expected document person role.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_ExpectedDocument_PersonRole
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_ExpectedDocument_PersonRole"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocument_PersonRole", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account expected document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_ExpectedDocument_Type
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_ExpectedDocument_Type"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocument_Type", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account expected document expected document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account expected document mandatory optional conditional.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_ExpectedDocument_MandatoryOptionalConditional
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_ExpectedDocument_MandatoryOptionalConditional"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocument_MandatoryOptionalConditional", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account expected document guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_ExpectedDocumentGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_ExpectedDocumentGuid"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocumentGuid", value);
            }
        }


        /// <summary>
        /// Personal and joint account expected document last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PersonalAndJointAccount_ExpectedDocumentLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PersonalAndJointAccount_ExpectedDocumentLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PersonalAndJointAccount_ExpectedDocumentLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected PersonalAndJointAccount_ExpectedDocumentInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="PersonalAndJointAccount_ExpectedDocumentInfo"/> class.
        /// </summary>
        public PersonalAndJointAccount_ExpectedDocumentInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="PersonalAndJointAccount_ExpectedDocumentInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public PersonalAndJointAccount_ExpectedDocumentInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}