using CMS.DataEngine;

namespace EurobankAccountSettings
{
    /// <summary>
    /// Class providing <see cref="PersonalAndJointAccount_BankDocumentInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IPersonalAndJointAccount_BankDocumentInfoProvider))]
    public partial class PersonalAndJointAccount_BankDocumentInfoProvider : AbstractInfoProvider<PersonalAndJointAccount_BankDocumentInfo, PersonalAndJointAccount_BankDocumentInfoProvider>, IPersonalAndJointAccount_BankDocumentInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalAndJointAccount_BankDocumentInfoProvider"/> class.
        /// </summary>
        public PersonalAndJointAccount_BankDocumentInfoProvider()
            : base(PersonalAndJointAccount_BankDocumentInfo.TYPEINFO)
        {
        }
    }
}