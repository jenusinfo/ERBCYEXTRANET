using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Eurobank.Models
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email")]
        [DisplayName("Email*")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [DisplayName("Password*")]
        [Required(ErrorMessage = "Please enter your password")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string RegPassword { get; set; }


        [DataType(DataType.Password)]
        [DisplayName("Confirm Password*")]
        [Required(ErrorMessage = "Please confirm your password")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        [Compare("RegPassword", ErrorMessage = "Password does not match the confirmation password")]
        public string RegPasswordConfirmation { get; set; }


        [DisplayName("First Name*")]
        [Required(ErrorMessage = "Please enter your first name")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string FirstName { get; set; }


        [DisplayName("Last Name*")]
        [Required(ErrorMessage = "Please enter your last name")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string LastName { get; set; }
    }
}