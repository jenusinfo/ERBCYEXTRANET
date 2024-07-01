using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;
using Eurobank.Components.Widgets.Polls;
using Eurobank.Models.Polls;
using Eurobank.Models.WhatsNew;
using Eurobank.Widgets;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[assembly: RegisterWidget(PollsWidgetViewComponent.IDENTIFIER, typeof(PollsWidgetViewComponent), "Polls", typeof(PollsWidgetsProperties), Description = "Displays the Polls from the Eurobank site.", IconClass = "icon-l-list-article")]


namespace Eurobank.Components.Widgets.Polls
{
	
    public class PollsWidgetViewComponent : ViewComponent
    {

        public const string IDENTIFIER = "Polls";


        private readonly WhatsNewRepository repository;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        public PollsWidgetViewComponent(WhatsNewRepository repository, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.repository = repository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }

        public ViewViewComponentResult Invoke(ComponentViewModel<PollsWidgetsProperties> viewModel)
        {
            if(viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }
            PollsWidgetViewModel pollsWidgetViewModel = new PollsWidgetViewModel();
            //pollsWidgetViewModel.Isactive = UserPoll(ValidationHelper.GetInteger(viewModel.Properties.PollsID,0));
            string customTableClassName = "Eurobank.Polls_Poll";
            // Gets the custom table
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if(customTable != null)
            {
                var question = CustomTableItemProvider.GetItems(customTableClassName)
                    //.WhereEquals("ItemID", viewModel.Properties.PollsID)
                    .WhereEquals("Isactive",true)
                    .WhereGreaterOrEquals("PollOpenTO", DateTime.Today).WhereLessOrEquals("PollOpenFrom", DateTime.Today)
					.FirstOrDefault();
                // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                if(question!=null)
                {
					pollsWidgetViewModel.Isactive = UserPoll(ValidationHelper.GetInteger(question.GetValue("ItemID"), 0));
					pollsWidgetViewModel.PollQuestion = ValidationHelper.GetString(question.GetValue("PollQuestion"), "");
                    pollsWidgetViewModel.PollQuestionID = ValidationHelper.GetString(question.GetValue("ItemID"), "");
                    pollsWidgetViewModel.PollResponseMessage= ValidationHelper.GetString(question.GetValue("PollResponseMessage"), "");
                }
            }
            pollsWidgetViewModel.AnswerList = GetAnswerList(ValidationHelper.GetInteger(pollsWidgetViewModel.PollQuestionID, 0));
            return View("~/Components/Widgets/Polls/_PollsWidgets.cshtml", pollsWidgetViewModel);
        }

		private bool UserPoll(int pollsID)
		{
            bool success = false;
            string customTableClassName = "Eurobank.Polls_Users";

            // Gets the custom table
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if(customTable != null)
            {

                // Gets the first custom table record whose value in the 'ItemName' field is equal to "SampleName"
                var answerList = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("PollsID", pollsID).WhereEquals("UserID",User.Identity.Name).ToList();

                // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                if(answerList.Count > 0)
                {
                  success = true;
                }
            }
            return success;
        }

		private List<PollsAnswerViewModel> GetAnswerList(int PollsID)
		{
            List<PollsAnswerViewModel> pollsAnswerViewModels = new List<PollsAnswerViewModel>();

            // Prepares the code name (class name) of the custom table
            string customTableClassName = "Eurobank.Polls_PollAnswer";

            // Gets the custom table
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if(customTable != null)
            {

                // Gets the first custom table record whose value in the 'ItemName' field is equal to "SampleName"
                var answerList = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("AnswerPollID", PollsID).OrderByAscending("AnswerOrder").ToList();

                // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                if(answerList.Count > 0)
                {
                    foreach(var item in answerList)
                    {


                        PollsAnswerViewModel pollsAnswerViewModel = new PollsAnswerViewModel();
                        pollsAnswerViewModel.AnswerText = ValidationHelper.GetString(item.GetValue("AnswerText"), "");
                        pollsAnswerViewModel.AnswerID = ValidationHelper.GetString(item.GetValue("ItemID"), "");
                        pollsAnswerViewModel.VotePercentage = GetVotePercentage(ValidationHelper.GetInteger(item.GetValue("ItemID"), 0), ValidationHelper.GetInteger(item.GetValue("AnswerPollID"), 0));
                        pollsAnswerViewModels.Add(pollsAnswerViewModel);
                    }
                }
            }
            return pollsAnswerViewModels;

        }

		private int GetVotePercentage(int AnswerID, int PollsID)
		{
            int vote = 0;
            try
            {
                string customTableClassName = "Eurobank.Polls_PollAnswer";
                // Gets the custom table
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if(customTable != null)
                {
                    var eachpollcount = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("AnswerPollID", PollsID).WhereEquals("ItemID", AnswerID).FirstOrDefault(); ;
                    var totalPollsCount = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("AnswerPollID", PollsID);
                    // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                    if(totalPollsCount.Count > 0)
                    {

                        int sum = totalPollsCount.Sum(i => Convert.ToInt32(i.GetValue("AnswerCount")));
                        int count = Convert.ToInt32(eachpollcount.GetValue("AnswerCount"));
                        vote = (100 / sum) * count;
                    }



                }
                //Check if it is null or empty
                return vote;
            }
            catch
            {
                //If any kind of error occurred don't display it
                return vote;
            }
        }
	}
}
