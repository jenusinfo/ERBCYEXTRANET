using CMS.DataEngine;

namespace EurobankAccountSettings
{
    /// <summary>
    /// Class providing <see cref="CorporateAccount_ExpectedDocumentInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(ICorporateAccount_ExpectedDocumentInfoProvider))]
    public partial class CorporateAccount_ExpectedDocumentInfoProvider : AbstractInfoProvider<CorporateAccount_ExpectedDocumentInfo, CorporateAccount_ExpectedDocumentInfoProvider>, ICorporateAccount_ExpectedDocumentInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorporateAccount_ExpectedDocumentInfoProvider"/> class.
        /// </summary>
        public CorporateAccount_ExpectedDocumentInfoProvider()
            : base(CorporateAccount_ExpectedDocumentInfo.TYPEINFO)
        {
        }
    }
    public partial interface ICorporateAccount_ExpectedDocumentInfoProvider : IInfoProvider<CorporateAccount_ExpectedDocumentInfo>, IInfoByIdProvider<CorporateAccount_ExpectedDocumentInfo>
    {
    }
}