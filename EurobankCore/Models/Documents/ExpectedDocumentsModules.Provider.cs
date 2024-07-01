using CMS.DataEngine;

namespace EurobankAccountSettings
{
    /// <summary>
    /// Class providing <see cref="PersonalAndJointAccount_ExpectedDocumentInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IPersonalAndJointAccount_ExpectedDocumentInfoProvider))]
    public partial class PersonalAndJointAccount_ExpectedDocumentInfoProvider : AbstractInfoProvider<PersonalAndJointAccount_ExpectedDocumentInfo, PersonalAndJointAccount_ExpectedDocumentInfoProvider>, IPersonalAndJointAccount_ExpectedDocumentInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalAndJointAccount_ExpectedDocumentInfoProvider"/> class.
        /// </summary>
        public PersonalAndJointAccount_ExpectedDocumentInfoProvider()
            : base(PersonalAndJointAccount_ExpectedDocumentInfo.TYPEINFO)
        {
        }
    }
    public partial interface IPersonalAndJointAccount_ExpectedDocumentInfoProvider : IInfoProvider<PersonalAndJointAccount_ExpectedDocumentInfo>, IInfoByIdProvider<PersonalAndJointAccount_ExpectedDocumentInfo>
    {
    }
}