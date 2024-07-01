using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using EurobankAccountSettings;

[assembly: RegisterObjectType(typeof(CorporateAccount_ExpectedDocumentInfo), CorporateAccount_ExpectedDocumentInfo.OBJECT_TYPE)]

namespace EurobankAccountSettings
{
    /// <summary>
    /// Data container class for <see cref="CorporateAccount_ExpectedDocumentInfo"/>.
    /// </summary>
    [Serializable]
    public partial class CorporateAccount_ExpectedDocumentInfo : AbstractInfo<CorporateAccount_ExpectedDocumentInfo, ICorporateAccount_ExpectedDocumentInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "eurobankaccountsettings.corporateaccount_expecteddocument";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(CorporateAccount_ExpectedDocumentInfoProvider), OBJECT_TYPE, "EurobankAccountSettings.CorporateAccount_ExpectedDocument", "CorporateAccount_ExpectedDocumentID", "CorporateAccount_ExpectedDocumentLastModified", "CorporateAccount_ExpectedDocumentGuid", null, null, null, null, null, null)
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
        /// Corporate account expected document ID.
        /// </summary>
        [DatabaseField]
        public virtual int CorporateAccount_ExpectedDocumentID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CorporateAccount_ExpectedDocumentID"), 0);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocumentID", value);
            }
        }


        /// <summary>
        /// Corporate account expected document person type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_ExpectedDocument_PersonType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_ExpectedDocument_PersonType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocument_PersonType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account expected document person role.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_ExpectedDocument_PersonRole
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_ExpectedDocument_PersonRole"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocument_PersonRole", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account expected document jurisdiction.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_ExpectedDocument_Jurisdiction
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_ExpectedDocument_Jurisdiction"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocument_Jurisdiction", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account expected document expected document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_ExpectedDocument_ExpectedDocumentType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_ExpectedDocument_ExpectedDocumentType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocument_ExpectedDocumentType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account expected document mandatory optional conditional.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_ExpectedDocument_MandatoryOptionalConditional
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_ExpectedDocument_MandatoryOptionalConditional"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocument_MandatoryOptionalConditional", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account expected document guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_ExpectedDocumentGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_ExpectedDocumentGuid"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocumentGuid", value);
            }
        }


        /// <summary>
        /// Corporate account expected document last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime CorporateAccount_ExpectedDocumentLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("CorporateAccount_ExpectedDocumentLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("CorporateAccount_ExpectedDocumentLastModified", value);
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
        protected CorporateAccount_ExpectedDocumentInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="CorporateAccount_ExpectedDocumentInfo"/> class.
        /// </summary>
        public CorporateAccount_ExpectedDocumentInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="CorporateAccount_ExpectedDocumentInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public CorporateAccount_ExpectedDocumentInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}