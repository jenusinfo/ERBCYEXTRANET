using CMS.Membership;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using CMS.Core;
using CMS.EventLog;
using CMS.Helpers;

namespace Eurobank.Services
{
    public class SessionAuthorization : Attribute, IAuthorizationFilter
    {
        IEventLogService eventLog = Service.Resolve<IEventLogService>();
        String iSCuncurrentLoginActive = ResHelper.GetString("EuroBank.Settings.CuncurrentLogin.Active");
        public void OnAuthorization(AuthorizationFilterContext filterContext)  
       {
            //if (String.Equals(iSCuncurrentLoginActive, "TRUE", StringComparison.OrdinalIgnoreCase))
            //{
                string username = filterContext.HttpContext.Session.GetString("UserSessionName");
                string sessionid = filterContext.HttpContext.Session.GetString("UserSessionID");

                if (username != null)
                {
                    UserInfo user = UserInfoProvider.GetUserInfo(username);
                    //string sessioniddb = Convert.ToString(user.UserSettings.GetValue("Eurobank_UserSessionID"));
                    if (user != null)
                    {
                        if (sessionid != Convert.ToString(user.UserSettings.GetValue("Eurobank_UserSessionID")))
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account" }));
                            eventLog.LogInformation(filterContext.ActionDescriptor.DisplayName, "CustomLogInfo1", "user session on browser (" + sessionid + ") not matched with Database session (" + Convert.ToString(user.UserSettings.GetValue("Eurobank_UserSessionID")) + ") ");
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account" }));
                        eventLog.LogInformation(filterContext.ActionDescriptor.DisplayName, "CustomLogInfo1", "user not found of username : " + username);
                    }

                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account" }));
                    eventLog.LogInformation(filterContext.ActionDescriptor.DisplayName, "CustomLogInfo1", "user session not found in browser");
                } 
            //}
           
        }  
    }
}
