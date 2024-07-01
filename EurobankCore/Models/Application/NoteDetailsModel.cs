using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application
{
	public class NoteDetailsModel
	{
		public int rowID { get; set; }
		public int Id{ get; set; }
		//[Required(ErrorMessageResourceName = "NoteDetailsType", ErrorMessageResourceType = typeof(NoteDetailsModelErrorMessage))]
		public string NoteDetailsType { get; set; }
		public string NoteDetailsTypeName { get; set; }

		public string Subject { get; set; }

		public string SubjectName { get; set; }

		public string Details { get; set; }

		public string PendingOn { get; set; }

		public string PendingOnName { get; set; }

		public Nullable<DateTime> ExpectedDate { get; set; }

		public bool NoteDetails_Status { get; set; }
		public string Status_Name { get; set; }
	}
	public class NoteDetailsModelErrorMessage
	{
		public static string NoteDetailsType
		{
			get
			{
				return ResHelper.GetString("Eurobank.NoteDetails.Error.NoteDetailsType");
			}
		}
		public const string SubjectError = "Eurobank.NoteDetails.Error.Subject";
		public const string DetailsError = "Eurobank.NoteDetails.Error.Details";
		//public const string PendingOnError = "Eurobank.NoteDetails.Error.PendingOn";
		//public const string ExpectedDateError = "Eurobank.NoteDetails.Error.ExpectedDate";
	}
}
