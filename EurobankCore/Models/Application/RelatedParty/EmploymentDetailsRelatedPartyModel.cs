using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.RelatedParty
{
	public class EmploymentDetailsRelatedPartyModel
	{
		public int Id { get; set; }

		public string EmploymentStatus { get; set; }
		public string EmploymentStatusName { get; set; }
		public string Profession { get; set; }

		public string ProfessionName { get; set; }

		public string EmployersName { get; set; }

	}
}
