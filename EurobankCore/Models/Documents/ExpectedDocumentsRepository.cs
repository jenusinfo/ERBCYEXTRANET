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
	public class ExpectedDocumentsRepository
	{
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="RegistriesRepository"/> class that returns home page sections. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public ExpectedDocumentsRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.ExpectedDocuments>> GetExpectedDocumentsAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.ExpectedDocuments>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetExpectedDocumentsAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ExpectedDocuments> GetExpectedDocuments(int apllicationID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                     .OrderBy("NodeOrder")
                     .WhereEquals("ApplicationDetailsID", apllicationID)
                ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ExpectedDocuments>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetExpectedDocuments)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }

        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>

        public CMS.DocumentEngine.Types.Eurobank.ExpectedDocuments GetExpectedDocumentsByID(int expectedDocumentsID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ExpectedDocuments>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("ExpectedDocumentsID", expectedDocumentsID)
               ).FirstOrDefault();
        }
        public IEnumerable<PersonalAndJointAccount_ExpectedDocumentInfo> GetExpectedDocumentsModules()
        {
            IEnumerable<PersonalAndJointAccount_ExpectedDocumentInfo> items = PersonalAndJointAccount_ExpectedDocumentInfoProvider.ProviderObject.Get();
            return items;


        }
        public IEnumerable<CorporateAccount_ExpectedDocumentInfo> GetExpectedCorporateDocumentsModules()
        {
            IEnumerable<CorporateAccount_ExpectedDocumentInfo> items = CorporateAccount_ExpectedDocumentInfoProvider.ProviderObject.Get();
            return items;


        }
    }
}
