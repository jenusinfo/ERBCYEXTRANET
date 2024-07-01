using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity
{
	public class SignatureMandateCompanyRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureMandateCompanyRepository"/> class that returns home page sections. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public SignatureMandateCompanyRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany>> GetSignatureMandateCompanyAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(SignatureMandateCompany)}|{nameof(GetSignatureMandateCompanyAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany> GetSignatureMandateCompanies(int apllicationID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany>(
                 query => query
                     .OrderBy("NodeOrder")
                     .WhereEquals("ApplicationDetailsID", apllicationID)
                ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(SignatureMandateCompanyRepository)}|{nameof(SignatureMandateCompany)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany>GetSignatureMandateLegal(int apllicationID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                     .OrderBy("NodeOrder")
                     .WhereEquals("ApplicationDetailsID", apllicationID)
                ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(SignatureMandateCompanyRepository)}|{nameof(GetSignatureMandateLegal)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>

        public CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany GetSignatureMandateCompany(int SignatureMandateCompanyID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.SignatureMandateCompany>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("SignatureMandateCompanyID", SignatureMandateCompanyID)
               ).FirstOrDefault();
        }
    }
}
