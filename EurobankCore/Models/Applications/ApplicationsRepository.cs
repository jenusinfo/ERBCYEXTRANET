using CMS.DataEngine;
using CMS.DocumentEngine;
using DocumentFormat.OpenXml.Wordprocessing;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Common;
using Kendo.Mvc.Extensions;
using Kentico.Content.Web.Mvc;
using Lucene.Net.Search;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications
{
    public class ApplicationsRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="RegistriesRepository"/> class that returns home page sections. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public ApplicationsRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.PersonsRegistry>> GetRegistriesAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.PersonsRegistry>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetRegistriesAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }


        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetails()
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children)
                     .OrderBy("NodeOrder")
                );
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsNormal(int userId)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                     .OrderBy("NodeOrder")
                );
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsPower(string companyId)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", companyId)
                     .OrderBy("NodeOrder")
                );
        }

        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsPowerForBranch(string bankBranchId)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_ResponsibleBankingCenter", bankBranchId)
                     .OrderBy("NodeOrder")
                );
        }

        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForInternalUserNormal(List<Guid> bankBranches, int pageNumber, int pageSize, out int totalCount, string sortMember, string sortDirection, string txtSearch, string filterColumn, string stausFilter)
        {

            if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "DRAFT")
            {
                //string DraftGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                //totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                //         query => query
                //          .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                //          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                //          .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)).Count();
                //if (sortMember != "" && sortDirection != "")
                //{
                //    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                //     query => query
                //      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                //      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                //      .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                //         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                //         .Page(pageNumber - 1, pageSize)
                //    );
                //}
                //else
                //{
                //    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                //     query => query
                //      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                //      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                //      .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                //      .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                //    );
                //}
                totalCount = 0;
                return null;
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "COMPLETED")
            {
                string CompletedGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.COMPLETED.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                       .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "PENDING")
            {
                List<SelectListItem> pendingGuids = ServiceHelper.GetApplicationStatuses().Where(x => x.Text == "PENDING INITIATOR" || x.Text == "PENDING SIGNATURES" || x.Text == "PENDING CHECKER" || x.Text == "PENDING VERIFICATION" || x.Text == "PENDING EXECUTION" || x.Text == "PENDING DOCUMENTS" || x.Text == "PENDING OMMISSIONS" || x.Text == "PENDING COMPLETION").ToList();
                List<string> pendingGuidslist = pendingGuids.Select(x => x.Value).ToList();
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                      .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else //(string.IsNullOrEmpty(stausFilter))
            {
                string DraftGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                           .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                       .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                      .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForInternalUserNormalByApplicationDetailsID(List<Guid> bankBranches, int ApplicationDetailsID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                  .WhereEquals("ApplicationDetailsID", ApplicationDetailsID)
                );
        }

        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForInternalUserPowerByApplicationDetailsID(List<Guid> bankBranches, int ApplicationDetailsID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                  .WhereEquals("ApplicationDetailsID", ApplicationDetailsID)
                );
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForInternalUserPower(List<Guid> bankBranches, int pageNumber, int pageSize, out int totalCount, string sortMember, string sortDirection, string txtSearch, string filterColumn, string stausFilter)
        {
            if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "DRAFT")
            {
                //string DraftGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                //totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                //         query => query
                //          .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                //          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                //          .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)).Count();
                //if (sortMember != "" && sortDirection != "")
                //{
                //    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                //     query => query
                //      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                //      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                //      .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                //         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                //         .Page(pageNumber - 1, pageSize)
                //    );
                //}
                //else
                //{
                //    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                //     query => query
                //      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                //      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                //      .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                //         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                //    );
                //}
                totalCount = 0;
                return null;
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "COMPLETED")
            {
                string CompletedGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.COMPLETED.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "PENDING")
            {
                List<SelectListItem> pendingGuids = ServiceHelper.GetApplicationStatuses().Where(x => x.Text == "PENDING INITIATOR" || x.Text == "PENDING SIGNATURES" || x.Text == "PENDING CHECKER" || x.Text == "PENDING VERIFICATION" || x.Text == "PENDING EXECUTION" || x.Text == "PENDING DOCUMENTS" || x.Text == "PENDING OMMISSIONS" || x.Text == "PENDING COMPLETION").ToList();
                List<string> pendingGuidslist = pendingGuids.Select(x => x.Value).ToList();
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                      .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else
            {
                string DraftGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                       .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                       .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                      .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                      .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                       .WhereNotEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                      .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
        }

        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForIntroducerNormal(int userId, int pageNumber, int pageSize, out int totalCount, string sortMember, string sortDirection, string txtSearch, string filterColumn, string stausFilter)
        {
            if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "DRAFT")
            {
                string DraftGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "COMPLETED")
            {
                string CompletedGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.COMPLETED.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "PENDING")
            {
                List<SelectListItem> pendingGuids = ServiceHelper.GetApplicationStatuses().Where(x => x.Text == "PENDING INITIATOR" || x.Text == "PENDING SIGNATURES" || x.Text == "PENDING CHECKER" || x.Text == "PENDING VERIFICATION" || x.Text == "PENDING EXECUTION" || x.Text == "PENDING DOCUMENTS" || x.Text == "PENDING OMMISSIONS" || x.Text == "PENDING COMPLETION").ToList();
                List<string> pendingGuidslist = pendingGuids.Select(x => x.Value).ToList();
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else
            {
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      ).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                     .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                     .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                    query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                        );
                }
            }
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForIntroducerNormalByApplicationDetailsID(int userId, int ApplicationDetailsID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId)
                 .WhereEquals("ApplicationDetailsID", ApplicationDetailsID)
                );
        }

        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForIntroducerPower(string organization, int pageNumber, int pageSize, out int totalCount, string sortMember, string sortDirection, string txtSearch, string filterColumn, string stausFilter)
        {
            if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "DRAFT")
            {
                string DraftGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", DraftGuid)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "COMPLETED")
            {
                string CompletedGuid = ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.COMPLETED.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereEquals("ApplicationDetails_ApplicationStatus", CompletedGuid)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else if (!string.IsNullOrEmpty(stausFilter) && stausFilter.ToUpper() == "PENDING")
            {
                List<SelectListItem> pendingGuids = ServiceHelper.GetApplicationStatuses().Where(x => x.Text == "PENDING INITIATOR" || x.Text == "PENDING SIGNATURES" || x.Text == "PENDING CHECKER" || x.Text == "PENDING VERIFICATION" || x.Text == "PENDING EXECUTION" || x.Text == "PENDING DOCUMENTS" || x.Text == "PENDING OMMISSIONS" || x.Text == "PENDING COMPLETION").ToList();
                List<string> pendingGuidslist = pendingGuids.Select(x => x.Value).ToList();
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                         query => query
                          .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                          .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                          .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)).Count();
                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                      .WhereIn("ApplicationDetails_ApplicationStatus", pendingGuidslist)
                         .OrderByDescending("Search_Created_On").Page(pageNumber - 1, pageSize)
                    );
                }
            }
            else
            {
                totalCount = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                             query => query
                              .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                              .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                              ).Count();

                if (sortMember != "" && sortDirection != "")
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                         .OrderBy((sortDirection == "Ascending" ? OrderDirection.Ascending : OrderDirection.Descending), sortMember)
                         .Page(pageNumber - 1, pageSize)
                    );
                }
                else ////////////////////////////////////////////////////////////////////////////////////////////
                {
                    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                     query => query
                      .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                      .WhereContains(string.IsNullOrEmpty(txtSearch) ? "ApplicationDetails_ApplicationNumber" : filterColumn, string.IsNullOrEmpty(txtSearch) ? "" : txtSearch)
                         .OrderByDescending("Search_Created_On")
                         .Page(pageNumber - 1, pageSize)
                    );
                }
            }
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetApplicationDetailsForIntroducerPowerByApplicationDetailsID(string organization, int ApplicationDetailsID)
        {

            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children).WhereEquals("ApplicationDetails_UserOrganisation", organization)
                  .WhereEquals("ApplicationDetailsID", ApplicationDetailsID)
                );
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> GetAllApplicationDetails()
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                 query => query
                  .Path("/Applications-(1)", PathTypeEnum.Children)
                     .OrderBy("NodeOrder")
                );
        }

        public CMS.DocumentEngine.Types.Eurobank.ApplicationDetails GetApplicationDetailsByID(int applicationDetailsID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("ApplicationDetailsID", applicationDetailsID)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.Intermediary GetIntermediary(string NodeGuid)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.Intermediary>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeGUID", NodeGuid)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.ApplicationDetails GetApplicationDetailsByApplicationNumber(string applicationNumber)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("ApplicationDetails_ApplicationNumber", applicationNumber)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.ApplicationDetails GetApplicationDetailsByNodeGUID(string applicationNodeGUID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.ApplicationDetails>(
                query => query
                .Path("/Applications-(1)", PathTypeEnum.Children)
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeGUID", applicationNodeGUID)
               ).FirstOrDefault();
        }
    }
}
