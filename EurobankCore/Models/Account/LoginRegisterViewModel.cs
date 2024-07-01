using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel Login { get; set; }

        public RegisterViewModel Register { get; set; }
        public string BrowserName { get; set; }
        public string IpAddress { get; set; }
        public bool IsPreviousLogout { get; set; }
    }
}
