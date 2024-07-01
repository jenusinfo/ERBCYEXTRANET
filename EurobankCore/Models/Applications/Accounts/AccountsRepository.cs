using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications.Accounts
{
	public class AccountsRepository
	{
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        public AccountsRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        public Task<IEnumerable<CMS.DocumentEngine.Types.Eurobank.Accounts>> GetAccountsAsync(string nodeAliasPath, CancellationToken cancellationToken)
        {
            return pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Eurobank.Accounts>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetAccountsAsync)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }


        /// <summary>
        /// Returns an enumerable collection of home page sections ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the home in the content tree.</param>
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.Accounts> GetAccounts(string nodeAliasPath)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.Accounts>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(RegistriesRepository)}|{nameof(GetAccounts)}|{nodeAliasPath}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()));
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
        public CMS.DocumentEngine.Types.Eurobank.Accounts GetAccountsDetailsByID(int accountID)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.Accounts>(
                query => query
                    .OrderBy("NodeOrder")
                    .WhereEquals("AccountsID", accountID)
               ).FirstOrDefault();
        }
        public CMS.DocumentEngine.Types.Eurobank.Accounts CheckAccount(string nodeAliasPath, string currency,string account)
        {
           
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.Accounts>(
                query => query
                  .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder").WhereEquals("Accounts_Currency", currency).WhereEquals("Accounts_AccountType", account)
               ).FirstOrDefault();
        }
    }
}
