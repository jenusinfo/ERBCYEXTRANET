using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Polls
{
	public class PollsAnswerViewModel
	{
		public string AnswerID{ get; set; }
		public string AnswerText { get; set; }
		public int AnswerOrder { get; set; }
		public int AnswerCount { get; set; }
		public bool AnswerEnabled { get; set; }
		public string AnswerPollID { get; set; }
		public int VotePercentage { get; set; }
	}
}
