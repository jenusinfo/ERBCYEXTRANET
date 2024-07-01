using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using EurobankAccountSettings;

[assembly: RegisterObjectType(typeof(PersonalAndJointAccount_BankDocumentInfo), PersonalAndJointAccount_BankDocumentInfo.OBJECT_TYPE)]

namespace EurobankAccountSettings
{
    /// <summary>
    /// Data container class for <see cref="PersonalAndJointAccount_BankDocumentInfo"/>.
    /// </summary>
    [Serializable]
    public partial class PersonalAndJointAccount_BankDocumentInfo : AbstractInfo<PersonalAndJointAccount_BankDocumentInfo, IPersonalAndJointAccount_BankDocumentInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "eurobankaccountsettings.personalandjointaccount_bankdocument";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(PersonalAndJointAccount_BankDocumentInfoProvider), OBJECT_TYPE, "EurobankAccountSettings.PersonalAndJointAccount_BankDocument", "PersonalAndJointAccount_BankDocumentID", "PersonalAndJointAccount_BankDocumentLastModified", "PersonalAndJointAccount_BankDocumentGuid", null, null, null, null, null, null)
        {
            ModuleName = "EurobankAccountSettings",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Personal and joint account bank document ID.
        /// </summary>
        [DatabaseField]
        public virtual int PersonalAndJointAccount_BankDocumentID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PersonalAndJointAccount_BankDocumentID"), 0);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocumentID", value);
            }
        }


        /// <summary>
        /// Personal and joint account bank document person type.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_BankDocument_PersonType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_BankDocument_PersonType"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocument_PersonType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account bank document entity role.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_BankDocument_EntityRole
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_BankDocument_EntityRole"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocument_EntityRole", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account bank document person role.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_BankDocument_PersonRole
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_BankDocument_PersonRole"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocument_PersonRole", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account bank document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_BankDocument_Type
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_BankDocument_Type"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocument_Type", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account bank document bank document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_BankDocument_BankDocumentType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_BankDocument_BankDocumentType"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocument_BankDocumentType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account bank document mandatory optional conditional.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_BankDocument_MandatoryOptionalConditional
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_BankDocument_MandatoryOptionalConditional"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocument_MandatoryOptionalConditional", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Personal and joint account bank document is active.
        /// </summary>
        [DatabaseField]
        public virtual bool PersonalAndJointAccount_BankDocument_IsActive
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("PersonalAndJointAccount_BankDocument_IsActive"), true);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocument_IsActive", value);
            }
        }


        /// <summary>
        /// Personal and joint account bank document guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid PersonalAndJointAccount_BankDocumentGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PersonalAndJointAccount_BankDocumentGuid"), Guid.Empty);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocumentGuid", value);
            }
        }


        /// <summary>
        /// Personal and joint account bank document last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PersonalAndJointAccount_BankDocumentLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PersonalAndJointAccount_BankDocumentLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PersonalAndJointAccount_BankDocumentLastModified", value);
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
        protected PersonalAndJointAccount_BankDocumentInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="PersonalAndJointAccount_BankDocumentInfo"/> class.
        /// </summary>
        public PersonalAndJointAccount_BankDocumentInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="PersonalAndJointAccount_BankDocumentInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public PersonalAndJointAccount_BankDocumentInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}