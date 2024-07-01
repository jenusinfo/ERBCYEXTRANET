using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Api
{
	public class ApplicationStatusRequest
	{
		public string ApplicationNumber { get; set; }

		public string Status { get; set; }

		public string Decision { get; set; }

		public string Comments { get; set; }

		public string CallerId { get; set; }
	}
}
