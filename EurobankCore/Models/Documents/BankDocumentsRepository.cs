using CMS.DocumentEngine;
using EurobankAccountSettings;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.Documents
{
	public class BankDocumentsRepository
	{
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="RegistriesRepository"/> class that returns home page sections. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public BankDocumentsRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.BankDocuments>> GetBankDocumentsAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.BankDocuments>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetBankDocumentsAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.BankDocuments> GetBankDocuments(int apllicationID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                     .OrderBy("NodeOrder")
                     .WhereEquals("ApplicationDetailsID", apllicationID)
                ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.BankDocuments>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetBankDocuments)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }

        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>

        public CMS.DocumentEngine.Types.Eurobank.BankDocuments GetBankDocumentsByID(int bankDocumentsID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.BankDocuments>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("BankDocumentsID", bankDocumentsID)
               ).FirstOrDefault();
        }
        public IEnumerable<PersonalAndJointAccount_BankDocumentInfo> GetBankDocumentsModules()
        {
            IEnumerable<PersonalAndJointAccount_BankDocumentInfo> items = PersonalAndJointAccount_BankDocumentInfoProvider.ProviderObject.Get();
            return items;


        }
        public IEnumerable<CorporateAccount_BankDocumentInfo> GetCorporateDocumentsModules()
        {
            IEnumerable<CorporateAccount_BankDocumentInfo> items = CorporateAccount_BankDocumentInfoProvider.ProviderObject.Get();
            return items;


        }
        public IEnumerable<PersonalAndJointAccount_BankDocumentInfo> GetBankDocumentsModulesExpectEntity(string entityType)
        {
            IEnumerable<PersonalAndJointAccount_BankDocumentInfo> items = PersonalAndJointAccount_BankDocumentInfoProvider.ProviderObject.Get().WhereNotEquals("PersonalAndJointAccount_BankDocument_PersonType", entityType);
            return items;


        }
    }
}
