using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
	public class ApplicationUserModel
	{
		public ApplicationUserType UserType { get; set; }

		public ApplicationUserRole UserRole { get; set; }
	}
}
