using Kentico.Membership;

namespace Eurobank.Models
{
    public class YourAccountViewModel
    {
        public ApplicationUser User { get; set; }

        public string UserOrganisation { get; set; }

		public bool AvatarUpdateFailed { get; set; }
    }
}