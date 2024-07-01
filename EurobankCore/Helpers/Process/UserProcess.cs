using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Common;
using Eurobank.Models.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class UserProcess
    {
        public static UserModel GetUser(string userName)
        {
            UserModel retVal = null;

            if (!string.IsNullOrEmpty(userName))
            {
                retVal = new UserModel();
                UserInfo user = UserInfoProvider.GetUserInfo(userName);
                if (user != null)
                {
                    retVal.UserInformation = user;
                    string userType = ServiceHelper.GetName(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserType"), ""), "/Lookups/General/ENTITIES");
                    if (!string.IsNullOrEmpty(userType))
                    {
                        retVal.UserType = userType;
                    }
                    if (user.IsInRole(Role.PowerUser, SiteContext.CurrentSiteName))
                    {
                        retVal.UserRole = ApplicationUserRole.POWER.ToString();
                    }
                    else if (user.IsInRole(Role.NormalUser, SiteContext.CurrentSiteName))
                    {
                        retVal.UserRole = ApplicationUserRole.NORMAL.ToString();
                    }
                    if (string.Equals(userType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        string introducerOrganization = ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                        if (!string.IsNullOrEmpty(introducerOrganization))
                        {
                            retVal.IntroducerUser = new IntroducerUserModel();
                            retVal.IntroducerUser.Introducer = IntermediaryProvider.GetIntermediary(new Guid(introducerOrganization), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
                        }
                    }
                    else if (string.Equals(userType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        List<Guid> bankBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                        retVal.InternalUser = new InternalUserModel();
                        retVal.InternalUser.AssignBankBranches = bankBranches;
                    }
                }
            }

            return retVal;
        }

        public static List<UserInfo> GetPowerUsersByIntroducerCompany(string company)
        {
            List<UserInfo> retVal = null;

            if (!string.IsNullOrEmpty(company))
            {
                retVal = new List<UserInfo>();
                var users = UserInfoProvider.GetUsers();
                if (users != null && users.Count > 0)
                {
                    retVal = users.Where(y => y.UserSettings.GetValue("Eurobank_UserOrganisation") != null && string.Equals(y.UserSettings.GetValue("Eurobank_UserOrganisation").ToString(), company) && y.IsInRole(Role.PowerUser, SiteContext.CurrentSiteName)).ToList();
                }
            }

            return retVal;
        }

        public static List<SelectListItem> GetNotePendingUserDDL(string bankingCenterGUID,string company)
        {
            List<SelectListItem> retVal = null;


            retVal = new List<SelectListItem>();
            var users = UserInfoProvider.GetUsers();
            if (users != null)
            {
                foreach (var user in users.AsEnumerable())
                {
                    string userType = ServiceHelper.GetName(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserType"), ""), "/Lookups/General/ENTITIES");
                    
                    if (string.Equals(userType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        string introducerOrganization = ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                        if ((!string.IsNullOrEmpty(introducerOrganization) && string.Equals(introducerOrganization, company,StringComparison.OrdinalIgnoreCase)))
                        {
                            retVal.Add(new SelectListItem { Value = user.UserGUID.ToString(), Text = user.FullName.ToUpper() });
                        }
                    }
                    else if (string.Equals(userType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        List<Guid> bankBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                        if (bankBranches != null && bankBranches.Count > 0 && bankBranches.Any(x=>string.Equals(bankingCenterGUID, x.ToString(),StringComparison.OrdinalIgnoreCase)))
                        {
                            retVal.Add(new SelectListItem { Value = user.UserGUID.ToString(), Text = user.FullName.ToUpper() });
                        }
                        
                    }
                }

            }
            return retVal;
        }

        public static List<SelectListItem> GetEscalateToUsers(string bankingCenterGUID)
        {
            List<SelectListItem> retVal = null;


            retVal = new List<SelectListItem>();
            var users = UserInfoProvider.GetUsers();
            if(users != null)
            {
                foreach(var user in users.AsEnumerable())
                {
                    string userType = ServiceHelper.GetName(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserType"), ""), "/Lookups/General/ENTITIES");

                    if(string.Equals(userType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        List<Guid> bankBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                        if(bankBranches != null && bankBranches.Count > 0 && bankBranches.Any(x => string.Equals(bankingCenterGUID, x.ToString(), StringComparison.OrdinalIgnoreCase)))
                        {
                            retVal.Add(new SelectListItem { Value = user.UserGUID.ToString(), Text = user.FullName.ToUpper() });
                        }

                    }
                }

            }
            return retVal;
        }
    }
}
