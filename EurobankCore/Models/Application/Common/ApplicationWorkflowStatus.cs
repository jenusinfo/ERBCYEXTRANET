using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
	public enum ApplicationWorkflowStatus
	{
		NONE,
		DRAFT,
		PENDING_INITIATOR,
		PENDING_SIGNATURES,
PENDING_CHECKER,
		PENDING_VERIFICATION,
		PENDING_EXECUTION,
		PENDING_BANK_DOCUMENTS,
		PENDING_OMMISSIONS,
		PENDING_COMPLETION,
		COMPLETED,
		WITHDRAWN,
		CANCELLED
	}
}
