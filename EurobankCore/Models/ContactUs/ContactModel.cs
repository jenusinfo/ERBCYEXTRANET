using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.ContactUs
{
	public class ContactModel
	{
        [Required]
        public string FName { get; set; }

        [Required]
        public string LName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Massage { get; set; }

        [Required]
        public string Query { get; set; }


    }
}
