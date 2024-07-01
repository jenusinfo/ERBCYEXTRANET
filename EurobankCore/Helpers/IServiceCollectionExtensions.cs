using Eurobank.Infrastructure;
using Eurobank.Models;
using Eurobank.Models.FAQ;
using Eurobank.PageTemplates;
using Eurobank.Services;
using Eurobank.Models.UseFulLinks;
using Microsoft.Extensions.DependencyInjection;
using Eurobank.Models.WhatsNew;
using Eurobank.Models.Applications;
using Eurobank.Models.Applications.SourceofIncommingTransactions;
using Eurobank.Models.Application;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.Applications.Accounts;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.Documents;
using Eurobank.Models.Applications.DecisionHistory;
using Eurobank.Models.Applications.TaxDetails;
using Eurobank.Models.PEPDetails;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.Application.RelatedParty.PartyRoles;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.RelatedParty.PartyRolesLegal;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.TermsAndCondition;
using Eurobank.Models.Application.Applicant.LegalEntity.FATCACRS;
using Eurobank.Models.Application.Applicant.LegalEntity.CRS;
using Eurobank.Models.Application.Applicant.LegalEntity.ContactDetails;

namespace Eurobank
{
    public static class IServiceCollectionExtensions
    {
        public static void AddEurobankServices(this IServiceCollection services)
        {
            AddViewComponentServices(services);

            AddRepositories(services);

            services.AddSingleton<TypedProductViewModelFactory>();
            services.AddSingleton<TypedSearchItemViewModelFactory>();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<ICheckoutService, CheckoutService>();
            services.AddSingleton<RepositoryCacheHelper>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<RegistriesRepository>();
            services.AddSingleton<ArticleRepository>();
            services.AddSingleton<CafeRepository>();
            services.AddSingleton<ContactRepository>();
            services.AddSingleton<CountryRepository>();
            services.AddSingleton<NavigationRepository>();
            services.AddSingleton<SocialLinkRepository>();
            services.AddSingleton<BrewerRepository>();
            services.AddSingleton<CoffeeRepository>();
            services.AddSingleton<ManufacturerRepository>();
            services.AddSingleton<PublicStatusRepository>();
            services.AddSingleton<ProductRepository>();
            services.AddSingleton<VariantRepository>();
            services.AddSingleton<HotTipsRepository>();
            services.AddSingleton<CustomerAddressRepository>();
            services.AddSingleton<ShippingOptionRepository>();
            services.AddSingleton<PaymentMethodRepository>();
            services.AddSingleton<AboutUsRepository>();
            services.AddSingleton<MediaFileRepository>();
            services.AddSingleton<ReferenceRepository>();
            services.AddSingleton<HomeRepository>();
            services.AddSingleton<OrderRepository>();
            services.AddSingleton<FAQItemRepository>();
            services.AddSingleton<UseFulLinksRepository>();
            services.AddSingleton<NewsRepository>();
            services.AddSingleton<NewsSectionRepository>();
            services.AddSingleton<WhatsNewRepository>();
            services.AddSingleton<FAQRepository>();
            services.AddSingleton<ApplicationsRepository>();
            services.AddSingleton<SourceOfIncomingTransactionsRepository>();
            services.AddSingleton<SourceOfOutgoingTransactionsRepository>();
            services.AddSingleton<IdentificationDetailsRepository>();
            services.AddSingleton<AccountsRepository>();
            services.AddSingleton<EBankingSubscriberDetailsRepository>();
            services.AddSingleton<NoteDetailsRepository>();
            services.AddSingleton<SignatureMandateIndividualRepository>();


            



            services.AddSingleton<DebitCardDetailsRepository>();
            services.AddSingleton<BankDocumentsRepository>();
            services.AddSingleton<ExpectedDocumentsRepository>();
            services.AddSingleton<DecisionHistoryRepository>();
            services.AddSingleton<TaxDetailsRepository>();
            services.AddSingleton<PepApplicantRepository>();
            services.AddSingleton<PepAssociatesRepository>();
            services.AddSingleton<AddressDetailsRepository>();
            services.AddSingleton<SourceOfIncomeRepository>();
            services.AddSingleton<OriginOfTotalAssetsRepository>();
            services.AddSingleton<CompanyGroupStructureRepository>();
            services.AddSingleton<SignatureMandateCompanyRepository>();
            services.AddSingleton<SignatoryGroupRepository>();
            services.AddSingleton<CompanyDetailsRelatedPartyRepository>();




            services.AddSingleton<PersonalDetailsRepository>();
            services.AddSingleton<ContactDetailsRepository>();
            services.AddSingleton<PartyRolesRepository>();
            services.AddSingleton<PartyRolesLegalRepository>();
            services.AddSingleton<WhatsNewSectionRepository>();
            services.AddSingleton<TermsAndConditionRepository>();

            services.AddSingleton<CompanyDetailsRepository>();
            services.AddSingleton<FATCACRSDetailsRepository>();
            services.AddSingleton<CRSDetailsRepository>();
            services.AddSingleton<ContactDetailsLegalRepository>();

        }

        private static void AddViewComponentServices(IServiceCollection services)
        {
            services.AddSingleton<ArticleWithSidebarPageTemplateService>();
            services.AddSingleton<ArticlePageTemplateService>();
        }
    }
}
