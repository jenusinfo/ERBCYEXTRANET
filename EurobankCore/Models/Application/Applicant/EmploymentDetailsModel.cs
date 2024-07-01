using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant
{
	public class EmploymentDetailsModel
	{
		public int Id { get; set; }

		public string EmploymentStatus { get; set; }

		public string EmploymentStatusName { get; set; }

		public string Profession { get; set; }

		public string ProfessionName { get; set; }


		public string YearsInBusiness { get; set; }

		public string EmployersName { get; set; }

		public string EmployersBusiness { get; set; }

		public string FormerProfession { get; set; }

		public string FormerProfessionName { get; set; }

		public int FormerCountryOfEmployment { get; set; }

		public string FormerCountryOfEmploymentName { get; set; }

		public string FormerEmployersName { get; set; }

		public string FormerEmployersBusiness { get; set; }
	}
}
