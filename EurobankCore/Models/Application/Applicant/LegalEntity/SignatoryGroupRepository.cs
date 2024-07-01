﻿using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity
{
	public class SignatoryGroupRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="SignatoryGroupRepository"/> class that returns home page sections. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public SignatoryGroupRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.SignatoryGroup>> GetSignatoryGroupAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.SignatoryGroup>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(SignatoryGroup)}|{nameof(GetSignatoryGroupAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.SignatoryGroup> GetSignatoryGroups(int apllicationID)
        {
            var apllicationDetails = pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.SignatoryGroup>(
                 query => query
                     .OrderBy("NodeOrder")
                     .WhereEquals("SignatoryGroupID", apllicationID)
                ).FirstOrDefault();
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.SignatoryGroup>(
                query => query
                    .Path(apllicationDetails.NodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(SignatoryGroupRepository)}|{nameof(SignatoryGroup)}|{apllicationDetails.NodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(apllicationDetails.NodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }

        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>

        public CMS.DocumentEngine.Types.Eurobank.SignatoryGroup GetSignatoryGroup(int SignatoryGroupID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.SignatoryGroup>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("SignatoryGroupID", SignatoryGroupID)
               ).FirstOrDefault();
        }
    }
}
