using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using EurobankAccountSettings;

[assembly: RegisterObjectType(typeof(CorporateAccount_BankDocumentsInfo), CorporateAccount_BankDocumentsInfo.OBJECT_TYPE)]

namespace EurobankAccountSettings
{
    /// <summary>
    /// Data container class for <see cref="CorporateAccount_BankDocumentsInfo"/>.
    /// </summary>
    [Serializable]
    public partial class CorporateAccount_BankDocumentsInfo : AbstractInfo<CorporateAccount_BankDocumentsInfo, ICorporateAccount_BankDocumentsInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "eurobankaccountsettings.corporateaccount_bankdocuments";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(CorporateAccount_BankDocumentsInfoProvider), OBJECT_TYPE, "EurobankAccountSettings.CorporateAccount_BankDocuments", "CorporateAccount_BankDocumentsID", "CorporateAccount_BankDocumentsLastModified", "CorporateAcoountPersonType", null, null, null, null, null, null)
        {
            ModuleName = "EurobankAccountSettings",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Corporate account bank documents ID.
        /// </summary>
        [DatabaseField]
        public virtual int CorporateAccount_BankDocumentsID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CorporateAccount_BankDocumentsID"), 0);
            }
            set
            {
                SetValue("CorporateAccount_BankDocumentsID", value);
            }
        }


        /// <summary>
        /// Corporate acoount person type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAcoountPersonType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAcoountPersonType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAcoountPersonType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate acoount expected document type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAcoountExpectedDocumentType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAcoountExpectedDocumentType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAcoountExpectedDocumentType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate acoount mandatory optional conditional.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAcoountMandatoryOptionalConditional
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAcoountMandatoryOptionalConditional"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAcoountMandatoryOptionalConditional", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate acoount person role.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAcoountPersonRole
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAcoountPersonRole"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAcoountPersonRole", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate acoount account type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAcoountAccountType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAcoountAccountType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAcoountAccountType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate acoount sub type.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAcoountSubType
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAcoountSubType"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAcoountSubType", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate acoount jurisdiction.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAcoountJurisdiction
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAcoountJurisdiction"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAcoountJurisdiction", value, Guid.Empty);
            }
        }


        /// <summary>
        /// Corporate account bank documents guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid CorporateAccount_BankDocumentsGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CorporateAccount_BankDocumentsGuid"), Guid.Empty);
            }
            set
            {
                SetValue("CorporateAccount_BankDocumentsGuid", value);
            }
        }


        /// <summary>
        /// Corporate account bank documents last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime CorporateAccount_BankDocumentsLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("CorporateAccount_BankDocumentsLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("CorporateAccount_BankDocumentsLastModified", value);
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
        protected CorporateAccount_BankDocumentsInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="CorporateAccount_BankDocumentsInfo"/> class.
        /// </summary>
        public CorporateAccount_BankDocumentsInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="CorporateAccount_BankDocumentsInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public CorporateAccount_BankDocumentsInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}