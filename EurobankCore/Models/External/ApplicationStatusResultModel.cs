using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.External
{
	public class ApplicationStatusResultModel
	{
		public string ErrorCode { get; set; }

		public int StatusCode { get; set; }

		public string Message { get; set; }

		public bool IsSuccess { get; set; }
	}
}
