using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
	public enum ApplicationWorkflowDecisionType
	{
		NONE,
		SUBMIT,
		WITHDRAW,
		RESUBMIT,
		CHECKED,
		RETURN,
		RETURN_TO_INITIATOR,
		RETURN_TO_VERIFICATION,
		VERIFIED,
		CANCEL,
		EXECUTED,
		ESCALATE,
		SEND_FOR_SIGNATURES,
		SIGNATURES_COMPLETED,
		DOCUMENTS_RECEIVED,
		COMPLETE,
		DOCUMENTS_PENDING,
		OMMISSIONS_COMPLETED
	}
}
