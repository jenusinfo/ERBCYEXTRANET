using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Kentico.Membership;

namespace Eurobank.Models
{
    public class PersonalDetailsViewModel
    {
        [DisplayName("Email / User name")]
        public string UserName { get; set; }


        [DisplayName("First name")]
        [Required(ErrorMessage = "Please enter your first name")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        [URLCheck(ErrorMessage ="URL is not allowed")]
        [RegularExpression("^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "Html tags are not allowed.")]
        public string FirstName { get; set; }


        [DisplayName("Last name")]
        [Required(ErrorMessage = "Please enter your last name")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        [URLCheck(ErrorMessage = "URL is not allowed")]
        [RegularExpression("^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "Html tags are not allowed.")]
        public string LastName { get; set; }


        public PersonalDetailsViewModel()
        {
        }


        public PersonalDetailsViewModel(ApplicationUser user)
        {
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
        
        public class URLCheck : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                bool isValid = !reg.IsMatch(value.ToString());
                return isValid;
            }
        }
    }
}