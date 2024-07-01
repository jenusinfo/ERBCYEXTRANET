using System.Collections.Generic;

namespace Eurobank.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<HomeSectionViewModel> HomeSections { get; set; }

        public ReferenceViewModel Reference { get; set; }

        public int ApplicationTypeIndividualCount { get; set; }
        public int ApplicationTypeJointCount { get; set; }
        public int ApplicationTypeCompanyCount { get; set; }

        public int ApplicationStatusCountCancelled { get; set; }
        public int ApplicationStatusCountCompleted { get; set; }
        public int ApplicationStatusCountDraft { get; set; }
        public int ApplicationStatusCountPendingChecker { get; set; }
        public int ApplicationStatusCountPendingCompletion { get; set; }
        public int ApplicationStatusCountPendingDocuments { get; set; }
        public int ApplicationStatusCountPendingExecution { get; set; }
        public int ApplicationStatusCountPendingInitiator { get; set; }
        public int ApplicationStatusCountPendingOmmissions { get; set; }
        public int ApplicationStatusCountPendingVerification { get; set; }
        public int ApplicationStatusCountPendingSignatures { get; set; }
        public int ApplicationStatusCountWithdrawn { get; set; }

    }
}