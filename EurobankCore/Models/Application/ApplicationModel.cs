using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.Applications;
using Eurobank.Models.Applications.Accounts;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.Applications.DecisionHistory;
using Eurobank.Models.Applications.SourceofIncommingTransactions;
using Eurobank.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application
{
	public class ApplicationModel
	{
		public int Id { get; set; }
		public string ApplicationNumber { get; set; }
		public string UserNodeGUID { get; set; }
		public string EntityType { get; set; }
		public string EntityTypeCode { get; set; }
		public string UserFullName { get; set; }
		public ApplicationDetailsModelView ApplicationDetails { get; set; }
		public PurposeAndActivityModel PurposeAndActivity { get; set; }
		public DecisionHistoryViewModel DecisionHistoryViewModel { get; set; }
		public List<DecisionHistoryViewModel> DecisionHistoryViewModels { get; set; }
		public List<ApplicantModel> Applicants { get; set; }
		public List<RelatedPartyModel> RelatedParties { get; set; }
		public List<AccountsDetailsViewModel> Accounts { get; set; }
		public List<SignatureMandateIndividualModel> SignatureMandates { get; set; }
		public List<EBankingSubscriberDetailsModel> EBankingSubscribers { get; set; }
		public List<SourceOfIncomingTransactionsViewModel> SourceOfIncomingTransactions { get; set; }
		public List<SourceOfOutgoingTransactionsModel> SourceOfOutgoingTransactions { get; set; }
		public List<DebitCardDetailsViewModel> DebitCards { get; set; }
		public List<DocumentsViewModel> BankDocuments { get; set; }
		public List<DocumentsViewModel> ExpectedDocuments { get; set; }
		public List<NoteDetailsModel> Notes { get; set; }
		public List<SignatoryGroupModel> SignatoryGroup { get; set; }
		public List<SignatureMandateCompanyModel> SignatureMandateCompany { get; set; }
		public GroupStructureLegalParentModel GroupStructureLegalParent { get; set; }
		public List<CompanyGroupStructureModel> GroupStructure { get; set; }

	}
}
