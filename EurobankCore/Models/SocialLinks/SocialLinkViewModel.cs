using CMS.DocumentEngine.Types.Eurobank;

using Kentico.Content.Web.Mvc;

namespace Eurobank.Models
{
    public class SocialLinkViewModel
    {
        public string IconPath { get; set; }


        public string Title { get; set; }


        public string Url { get; set; }


        public static SocialLinkViewModel GetViewModel(SocialLink socialLink, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new SocialLinkViewModel
            {
                Title = socialLink.Fields.Title,
                Url = socialLink.Fields.Url,
                IconPath = attachmentUrlRetriever.Retrieve(socialLink.Fields.Icon).RelativePath
            };
        }
    }
}