using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.External
{
	public enum ApplicationChangeStatusCode
	{
		SUCCESS = 200,
		STATUS_INVALID = 401,
		APPLICATION_NUMBER_INVALID = 402,
		DECISION_INVALID = 403,
		USER_NAME_INVALID = 404,
		ACCESS_DENIED = 405
	}

	public class ApplicationChangeStatusResposeMsg
	{
		public const string SUCCESS_MSG = "Status Changed Successfully";
		public const string STATUS_INVALID_MSG = "Status is invalid";
		public const string APPLICATION_NUMBER_INVALID_MSG = "Application Number is invalid";
		public const string DECISION_INVALID_MSG = "Decision is invalid";
		public const string USER_NAME_INVALID_MSG = "User Id is invalid";
		public const string ACCESS_DENIED_MSG = "Access Denied";
	}
}
