using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications.DecisionHistory
{
	public class DecisionHistoryViewModel
	{
		public int DecisionHistoryID { get; set; }
		[Display(Name = "When")]
		public string DecisionHistory_When { get; set; }
		[Display(Name = "Who")]
		public string DecisionHistory_Who { get; set; }
		//[Required(ErrorMessage = "Please enter Stage.")]
		[Display(Name = "Next Stage")]
		public string DecisionHistory_Stage { get; set; }
		//[Required(ErrorMessage = "Please enter Decision.")]
		[Display(Name = "Decision")]
		public string DecisionHistory_Decision { get; set; }
		[Display(Name = "Decision")]
		public string DecisionHistory_DecisionName { get; set; }
		[Display(Name = "Comments")]
		public string DecisionHistory_Comments { get; set; }
		[Display(Name = "Escalate To")]
		public string DecisionHistory_EscalateTo { get; set; }
	}
}
