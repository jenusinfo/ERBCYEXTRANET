using CMS.Membership;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.User
{
	public class UserModel
	{
		public UserInfo UserInformation { get; set; }

		//public ApplicationUserType UserType { get; set; }

		public string UserType { get; set; }

		public string UserRole { get; set; }
		public bool IsLegalOnly { get; set; }
		public IntroducerUserModel IntroducerUser { get; set; }

		public InternalUserModel InternalUser { get; set; }

		//public List<ApplicationDetailsModelView> Applications { get; set; }

		//public List<UserApplicationModel> UserApplications { get; set; }
	}
}
