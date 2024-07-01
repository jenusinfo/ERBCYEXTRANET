using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.PEPDetails
{
	public class PepApplicantRepository
	{
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        public PepApplicantRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.PepApplicant>> GetPepApplicantAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.PepApplicant>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(PepApplicantRepository)}|{nameof(GetPepApplicantAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }


        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.PepApplicant> GetPepApplicants(int applicantID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonalDetails>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("PersonalDetailsID", applicantID)
               ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PepApplicant>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(PepApplicantRepository)}|{nameof(GetPepApplicants)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.PepApplicant> GetPepApplicantsLegal(int applicantID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.CompanyDetails>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("CompanyDetailsID", applicantID)
               ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PepApplicant>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(PepApplicantRepository)}|{nameof(GetPepApplicants)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.PepApplicant> GetPepApplicantsLegalRealtedParty(int applicantID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.CompanyDetailsRelatedParty>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("CompanyDetailsRelatedPartyID", applicantID)
               ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PepApplicant>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(PepApplicantRepository)}|{nameof(GetPepApplicants)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        public CMS.DocumentEngine.Types.Eurobank.PepApplicant GetPepApplicantByID(int applicantID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PepApplicant>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("PepApplicantID", applicantID)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.PersonalDetails GetPersonalDetailsByID(int applicantID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonalDetails>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("PersonalDetailsID", applicantID)
               ).FirstOrDefault();
        }
    }
}
