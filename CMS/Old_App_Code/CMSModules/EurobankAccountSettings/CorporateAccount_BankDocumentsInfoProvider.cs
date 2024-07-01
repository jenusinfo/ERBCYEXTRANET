using CMS.DataEngine;

namespace EurobankAccountSettings
{
    /// <summary>
    /// Class providing <see cref="CorporateAccount_BankDocumentsInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(ICorporateAccount_BankDocumentsInfoProvider))]
    public partial class CorporateAccount_BankDocumentsInfoProvider : AbstractInfoProvider<CorporateAccount_BankDocumentsInfo, CorporateAccount_BankDocumentsInfoProvider>, ICorporateAccount_BankDocumentsInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorporateAccount_BankDocumentsInfoProvider"/> class.
        /// </summary>
        public CorporateAccount_BankDocumentsInfoProvider()
            : base(CorporateAccount_BankDocumentsInfo.TYPEINFO)
        {
        }
    }
}