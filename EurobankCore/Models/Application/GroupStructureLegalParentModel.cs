using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application
{
	public class GroupStructureLegalParentModel
	{
		public int Id { get; set; }

		public bool DoesTheEntityBelongToAGroup { get; set; }

		public string DoesTheEntityBelongToAGroupName { get; set; }

		public string GroupName { get; set; }

		public string GroupActivities { get; set; }


	}
}
