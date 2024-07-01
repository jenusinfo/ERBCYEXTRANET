using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.RelatedParty.ContactDetails
{
    public class ContactDetailsRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="RegistriesRepository"/> class that returns home page sections. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public ContactDetailsRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.ContactDetails>> GetContactDetailsAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.ContactDetails>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(ContactDetailsRepository)}|{nameof(GetContactDetailsAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }


        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ContactDetails> GetContactDetails(string nodeAliasPath)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ContactDetails>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(ContactDetailsRepository)}|{nameof(GetContactDetails)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        public CMS.DocumentEngine.Types.Eurobank.ContactDetails GetContactDetailsAsync(string nodeAliasPath, string NewsAlias)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ContactDetails>(
                query => query
                  .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeAlias", NewsAlias)
               ).First();
        }
        public CMS.DocumentEngine.Types.Eurobank.ContactDetails GetContactDetailsByID(int ContactDetailsID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ContactDetails>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("ContactDetailsID", ContactDetailsID)
               ).FirstOrDefault();
        }
    }
}
