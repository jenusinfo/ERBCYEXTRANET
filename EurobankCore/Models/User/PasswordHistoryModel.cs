using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.User
{
	public class PasswordHistoryModel
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public string Password { get; set; }

		public DateTime CreatedDateTime { get; set; }
	}
}
