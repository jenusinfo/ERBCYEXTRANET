using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using EurobankAccountSettings;

[assembly: RegisterObjectType(typeof(CorporateAccount_BankDocumentInfo), CorporateAccount_BankDocumentInfo.OBJECT_TYPE)]

namespace EurobankAccountSettings
{
    /// <summary>
    /// Data container class for <see cref="CorporateAccount_BankDocumentInfo"/>.
    /// </summary>
    [Serializable]
    public partial class CorporateAccount_BankDocumentInfo : AbstractInfo<CorporateAccount_BankDocumentInfo, ICorporateAccount_BankDocumentInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "eurobankaccountsettings.corporateaccount_bankdocument";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(CorporateAccount_BankDocumentInfoProvider), OBJECT_TYPE, "EurobankAccountSettings.CorporateAccount_BankDocument", "CorporateAccount_BankDocumentID", "CorporateAccount_BankDocumentLastModified", "CorporateAccount_BankDocumentGuid", null, null, null, null, null, null)
        {
            ModuleName = "EurobankAccountSettings",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Corporate account bank document ID.
        /// </summary>
        [DatabaseField]
        public virtual int CorporateAccount_BankDocumentID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CorporateAccount_BankDocumentID"), 0);
            }
            set
            {
                SetValue("CorporateAccount_BankDocumentID", value);
            }
        }


        /// <summary>
        /// Corporate account bank document jurisdiction.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_Jurisdiction
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_Jurisdiction"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_Jurisdiction", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank document entity role.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_EntityRole
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_EntityRole"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_EntityRole", value, Guid.Empty);
            }
        }


        
        /// <summary>
        /// Corporate account bank document person type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_PersonType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_PersonType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_PersonType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank document person role.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_PersonRole
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_PersonRole"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_PersonRole", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_Type
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_Type"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_Type", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank document sub type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_SubType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_SubType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_SubType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank document bank document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_BankDocumentType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_BankDocumentType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_BankDocumentType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank document mandatory optional conditional.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocument_MandatoryOptionalConditional
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocument_MandatoryOptionalConditional"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_MandatoryOptionalConditional", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank document is active.
        /// </summary>
        [DatabaseField]
        public virtual bool CorporateAccount_BankDocument_IsActive
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("CorporateAccount_BankDocument_IsActive"), true);
            }
            set
            {
                SetValue("CorporateAccount_BankDocument_IsActive", value);
            }
        }


        /// <summary>
        /// Corporate account bank document guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocumentGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocumentGuid"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocumentGuid", value);
            }
        }


        /// <summary>
        /// Corporate account bank document last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime CorporateAccount_BankDocumentLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("CorporateAccount_BankDocumentLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("CorporateAccount_BankDocumentLastModified", value);
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
        protected CorporateAccount_BankDocumentInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="CorporateAccount_BankDocumentInfo"/> class.
        /// </summary>
        public CorporateAccount_BankDocumentInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="CorporateAccount_BankDocumentInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public CorporateAccount_BankDocumentInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}