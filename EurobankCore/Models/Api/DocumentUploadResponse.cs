using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Api
{
	public class DocumentUploadResponse
	{
		public string ErrorMessage { get; set; }

		public int ResultCode { get; set; }

		public bool IsSuccess { get; set; }

		public string FileGuid { get; set; }
	}
}
