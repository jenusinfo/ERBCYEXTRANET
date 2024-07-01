using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;

using Kentico.Content.Web.Mvc;

namespace Eurobank.Models
{
    /// <summary>
    /// Represents a collection of home page sections.
    /// </summary>
    public class RegistriesRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="RegistriesRepository"/> class that returns home page sections. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public RegistriesRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
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
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.PersonsRegistry> GetRegistries(string nodeAliasPath)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonsRegistry>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetRegistries)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.CompanyRegistry> GetCompanyRegistries(string nodeAliasPath)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.CompanyRegistry>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetCompanyRegistries)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.AddressRegistry> GetAddressRegistries(string nodeAliasPath)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.AddressRegistry>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetAddressRegistries)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }

        public TreeNode GetNodeByNodeAliasPath(string nodeAliasPath,string objectType)
        {
           

            return pageRetriever.Retrieve<TreeNode>(
                query => query
                    .WhereEquals("NodeSKUID", nodeAliasPath),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetNodeByNodeAliasPath)}|{nodeAliasPath}")
                    .Dependencies((_, builder) => builder.ObjectType(objectType)))
                .FirstOrDefault() as TreeNode;
        }
        public CMS.DocumentEngine.Types.Eurobank.Registries GetRegistriesAsync(string nodeAliasPath, string NewsAlias)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.Registries>(
                query => query
                  .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeAlias", NewsAlias)
               ).First();
        }
		public CMS.DocumentEngine.Types.Eurobank.PersonsRegistryUser GetRegistryUserAsync(string nodeAliasPath, string NewsAlias)
		{
			return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonsRegistryUser>(
				query => query
				  .Path(nodeAliasPath, PathTypeEnum.Children)
					.OrderBy("NodeOrder")
					.WhereEquals("NodeAlias", NewsAlias)
			   ).First();
		}
        public CMS.DocumentEngine.Types.Eurobank.PersonsRegistryUser GetRegistryUserByName(string userName)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonsRegistryUser>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("DocumentName", userName)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.PersonsRegistry GetRegistryUserByNodeGUID(string nodeGUID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonsRegistry>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeGUID", nodeGUID)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.CompanyRegistry GetCompanyRegistryUserByNodeGUID(string nodeGUID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.CompanyRegistry>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeGUID", nodeGUID)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.PersonsRegistry GetRegistryUserById(int personRegistryId)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonsRegistry>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("PersonsRegistryID", personRegistryId)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.CompanyRegistry GetCompanyRegistryUserById(int companyRegistryID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.CompanyRegistry>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("CompanyRegistryID", companyRegistryID)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.AddressRegistry GetAddressRegistryUserByNodeGUID(string nodeGUID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.AddressRegistry>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeGUID", nodeGUID)
               ).FirstOrDefault();
        }

        public CMS.DocumentEngine.Types.Eurobank.AddressRegistry GetAddressRegistryById(int addressRegistryId)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.AddressRegistry>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("AddressRegistryID", addressRegistryId)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.PersonsRegistry GetRegistryFolder(string name)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PersonsRegistry>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("DocumentName", name)
               ).FirstOrDefault();
        }
    }
}