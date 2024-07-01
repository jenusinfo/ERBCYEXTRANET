using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models
{
	public class PollsViewModel
	{
		public string PollQuestion { get; set; }
		public string PollQuestionID { get; set; }
		public string PollResponseMessage { get; set; }
		public DateTime PollOpenFrom { get; set; }
		public DateTime PollOpenTO { get; set; }
		public string Answer { get; set; }
		public bool Isactive { get; set; }
	}
}
