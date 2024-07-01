using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class NewsProcess
    {
        public static News Add(string title)
        {
            News _news = new News();

            try
            {
                TreeNode parentPage = new DocumentQuery<TreeNode>()
                                    .Path("/News", PathTypeEnum.Single)
                                    .OnSite("Eurobank")
                                    .Culture("en-us")
                                    .TopN(1)
                                    .FirstOrDefault();

                if (parentPage != null)
                {
                    // Creates a new page of the custom page type
                    TreeNode newPage = TreeNode.New("Eurobank.News");

                    // Sets the properties of the new page
                    newPage.DocumentName = title;
                    newPage.DocumentCulture = "en-us";
                    newPage.SetValue("NewsTitle", ValidationHelper.GetString(title, ""));
                    newPage.SetValue("NewsSummary", ValidationHelper.GetString(title, ""));
                    newPage.SetValue("NewsLongDescription", ValidationHelper.GetString(title, ""));
                    // Inserts the new page as a child of the parent page
                    newPage.Insert(parentPage);
                    _news.Sucess = true;
                }

            }
            catch (Exception)
            {
                _news.Sucess = false;
                return _news;
            }
            return _news;
        }
        public class News
        {
            public bool Sucess { get; set; }
        }
    }
}
