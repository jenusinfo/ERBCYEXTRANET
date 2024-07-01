using Eurobank.Models.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application
{
	public class UserApplicationModel
	{
		public ApplicationDetailsModelView ApplicationModel { get; set; }

		public List<ApplicationPermission> ApplicationPermissions { get; set; }
	}
}
