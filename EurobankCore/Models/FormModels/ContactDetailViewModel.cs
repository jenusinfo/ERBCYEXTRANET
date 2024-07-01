using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models
{
	public class ContactDetailViewModel
	{
		[Display(Name = "Title")]
		[Required(ErrorMessage = "Title is Required")]
		public string Title { get; set; }
		[Display(Name = "First Name")]
		[Required(ErrorMessage = "First name is Required")]
		public string FirstName { get; set; }
		[Display(Name = "Last Name")]
		[Required(ErrorMessage = "Last name is Required")]
		public string LastName { get; set; }
		[Display(Name = "Father Name")]
		[Required(ErrorMessage = "Father name is Required")]
		public string FatherName { get; set; }
		[Display(Name = "Date of Birth")]
		public string DateofBirth { get; set; }
		public string PlaceofBirth { get; set; }
		public string Gender { get; set; }
		public string ApplicationID { get; set; }
		[Display(Name = "Active")]
		public string Isactive { get; set; }
	}
}
