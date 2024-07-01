using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class ValidationResultModel
	{
		public List<ValidationError> Errors { get; set; }
		public List<ValidationInfo> Infos { get; set; }
		public ApplicationModule ApplicationModuleName { get; set; }
		public bool IsValid { get; set; }
	}
	public class ValidationError
	{
		public string ErrorMessage { get; set; }
		public string PropertyName { get; set; }
	}
	public class ValidationInfo
	{
		public string InfoMessage { get; set; }
		public string PropertyName { get; set; }
	}
}
