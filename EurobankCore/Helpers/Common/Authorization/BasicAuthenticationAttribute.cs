using CMS.Membership;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Eurobank.Helpers.Common.Authorization
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        public string BasicRealm { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }

		//public BasicAuthenticationAttribute(string username, string password)
		//{
		//	this.Username = username;
		//	this.Password = password;
		//}

		//public BasicAuthenticationAttribute()
		//{
		//}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if(!String.IsNullOrEmpty(auth))
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.ToString().Substring(6))).Split(':');
                var user = new { Name = cred[0], Pass = cred[1] };
                var signInResult = SignInResult.Failed;

                try
                {
                    UserInfo userInfo = UserInfoProvider.GetUserInfo(user.Name);
                    if(userInfo != null)
					{
                        bool isValidUser = UserInfoProvider.ValidateUserPassword(userInfo, user.Pass);
                        if(isValidUser)
                            return;
                    }
                }
                catch(Exception ex)
                {

                }
    //            if(user.Name == Username && user.Pass == Password)
				//{

    //                return;
				//}
            }
            var res = filterContext.HttpContext.Response;
            res.StatusCode = 401;
            res.Headers.Add("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", BasicRealm ?? "Ryadel"));
            //res.CompleteAsync();
            //res.Redirect("api/useraccessdenied");
            filterContext.Result = new UnauthorizedResult();
        }
    }
}
