using CMS.DataEngine;

namespace EurobankAccountSettings
{
    /// <summary>
    /// Declares members for <see cref="CorporateAccountInfo"/> management.
    /// </summary>
    public partial interface ICorporateAccountInfoProvider : IInfoProvider<CorporateAccountInfo>, IInfoByIdProvider<CorporateAccountInfo>
    {
    }
}