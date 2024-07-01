using CMS.DataEngine;

namespace EurobankAccountSettings
{
    /// <summary>
    /// Class providing <see cref="CorporateAccountInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(ICorporateAccountInfoProvider))]
    public partial class CorporateAccountInfoProvider : AbstractInfoProvider<CorporateAccountInfo, CorporateAccountInfoProvider>, ICorporateAccountInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorporateAccountInfoProvider"/> class.
        /// </summary>
        public CorporateAccountInfoProvider()
            : base(CorporateAccountInfo.TYPEINFO)
        {
        }
    }
}