using CMS.DataEngine;

namespace EurobankAccountSettings
{
    /// <summary>
    /// Class providing <see cref="CorporateAccount_BankDocumentInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(ICorporateAccount_BankDocumentInfoProvider))]
    public partial class CorporateAccount_BankDocumentInfoProvider : AbstractInfoProvider<CorporateAccount_BankDocumentInfo, CorporateAccount_BankDocumentInfoProvider>, ICorporateAccount_BankDocumentInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorporateAccount_BankDocumentInfoProvider"/> class.
        /// </summary>
        public CorporateAccount_BankDocumentInfoProvider()
            : base(CorporateAccount_BankDocumentInfo.TYPEINFO)
        {
        }
    }
    /// </summary>
    public partial interface ICorporateAccount_BankDocumentInfoProvider : IInfoProvider<CorporateAccount_BankDocumentInfo>, IInfoByIdProvider<CorporateAccount_BankDocumentInfo>
    {
    }
}