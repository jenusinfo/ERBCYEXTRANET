using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications;
using Eurobank.Models.Applications.DecisionHistory;
using Eurobank.Models.Applications.SourceofIncommingTransactions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application
{
	public class ApplicationViewModel
	{
		public int Id { get; set; }
		public string Application_NodeGUID { get; set; }
		public string ApplicationNumber { get; set; }
		public string UserNodeGUID { get; set; }
		public string EntityType { get; set; }
		public string EntityTypeCode { get; set; }
		public string UserFullName { get; set; }
		public ApplicationDetailsModelView ApplicationDetails{ get; set;}

		public PurposeAndActivityModel PurposeAndActivity { get; set; }
		public DecisionHistoryViewModel DecisionHistoryViewModel { get; set; }
		public List<DecisionHistoryViewModel> _lst_DecisionHistoryViewModel { get; set; }
		//public List<StepperStep> LeftMenuApplicant { get; set; }
		public LeftMenuCommon LeftMenuApplicant { get; set; }

		public GroupStructureLegalParentModel GroupStructureLegalParent { get; set; }

		//public List<ApplicationWorkflowButton> ApplicationWorkflowButtons { get; set; }

		public bool IsCardShow { get; set; }
		public bool IsEbankingShow { get; set; }
		public bool IsCardNew { get; set; }
		public string ApplicantEntityType { get; set; }
	}
}
