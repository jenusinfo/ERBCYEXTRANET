using Eurobank.Models.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
	
    public class PollsWidgetViewModel
    {
        public IEnumerable<PollsAnswerViewModel> AnswerList { get; set; }


        /// <summary>
        /// Number of articles to show.
        /// </summary>
        public string PollQuestion { get; set; }
        public string PollQuestionID { get; set; }
        public string PollResponseMessage { get; set; }
        public string Answer { get; set; }
        public DateTime PollOpenFrom { get; set; }
        public DateTime PollOpenTO { get; set; }
        public bool Isactive { get; set; }
        public string AnswerCheckedID { get; set; }

        public string NodeAliasPath { get; set; }
    }
}
