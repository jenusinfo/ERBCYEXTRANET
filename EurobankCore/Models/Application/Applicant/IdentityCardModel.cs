using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant
{
	public class IdentityCardModel
	{
		public string IdentityNumber { get; set; }

		public int CountryOfIssue { get; set; }

		public string CountryOfIssueName { get; set; }
	}
}
