using CMS.DocumentEngine.Types.Eurobank;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.FAQ
{
    public class FaqViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactUs { get; set; }
        public string VisitUs { get; set; }
        //public List<FaqDetails> FaqList { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public string NodeId { get; set; }
        public static FaqViewModel GetViewModel(FAQItem faqItem, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new FaqViewModel
            {

                Question = faqItem.Questions,
                Answer = faqItem.Answers,
                NodeId= faqItem.NodeID.ToString()
            };
        }
        //public static FaqViewModel GetFAQPageViewModel(Faq faq, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        //{
        //    return new FaqViewModel
        //    {
        //        Name= faq.Name,
        //        Email = faq.EmailUs,
        //        ContactUs = faq.CallUs,
        //        VisitUs = faq.VisitUs
               
        //    };
        //}
    }

    public class FaqDetails
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public string NodeId { get; set; }
    }
}
