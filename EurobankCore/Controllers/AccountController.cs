using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Io;
using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.UploadExtensions;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Helpers.Process;
using Eurobank.Models;
using Eurobank.Models.Documents;
using Eurobank.Models.User;
using Eurobank.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Membership;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Eurobank.Controllers
{
    //[AuthorizeRoles(Role.PowerUser, Role.NormalUser)]
    public class AccountController : Controller
    {
        private readonly IMembershipActivityLogger membershipActivitiesLogger;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly IEventLogService eventLogService;
        private readonly IAvatarService avatarService;
        private readonly ApplicationUserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ISiteService siteService;
        private readonly IMessageService emailService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private static AccountService _AccountService = new AccountService();
        public AccountController(ApplicationUserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IMessageService emailService,
                                 ISiteService siteService,
                                 IAvatarService avatarService,
                                 IMembershipActivityLogger membershipActivitiesLogger,
                                 IStringLocalizer<SharedResources> localizer,
                                 IEventLogService eventLogService,
                                 IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.siteService = siteService;
            this.avatarService = avatarService;
            this.membershipActivitiesLogger = membershipActivitiesLogger;
            this.localizer = localizer;
            this.eventLogService = eventLogService;
            this.httpContextAccessor = httpContextAccessor;

        }
       
        // GET: Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            TempData["UserLoginFailed"] = "";
            ViewBag.LoginActive = "active";
            ViewBag.LoginShow = "show";
            ViewBag.RegisterActive = "";
            ViewBag.RegisterShow = "";
            LoginRegisterViewModel _LoginRegisterViewModel = new LoginRegisterViewModel();
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            _LoginRegisterViewModel.IpAddress = remoteIpAddress.ToString();
            return View("Login", _LoginRegisterViewModel);
        }


        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken] //commented becasue one user one login feature is not working correctly, ajax from login page is not able to call this actionmethod sometime
        //[IgnoreAntiforgeryToken]
        public async Task<ActionResult> Login(LoginRegisterViewModel model, string returnUrl)
        {
            eventLogService.LogInformation("AccountController.Login(Post).Start", "CustomLogInfo1", "User login method start with " +
                            "\r\n Username : " + ValidationHelper.GetString(model.Login.UserName, "not found") +
                            "\r\n IsPreviousLogout : " + ValidationHelper.GetString(model.IsPreviousLogout, "not found") +
                            "\r\n UserBrowserName : " + ValidationHelper.GetString(model.BrowserName, "not found") +
                            "\r\n UserIPAddress : " + ValidationHelper.GetString(model.IpAddress, "not found") + "");
            String iSCuncurrentLoginActive = ResHelper.GetString("EuroBank.Settings.CuncurrentLogin.Active");
            var NuberOfFailedAttempt = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["NuberOfFailedAttempt"];
            ViewBag.LoginActive = "active";
            ViewBag.LoginShow = "show";
            ViewBag.RegisterActive = "";
            ViewBag.RegisterShow = "";

            if (!ModelState.IsValid)
            {
                return View("Login", new LoginRegisterViewModel() { Login = model.Login });
            }

            var signInResult = SignInResult.Failed;
            UserInfo user = UserInfoProvider.GetUserInfo(model.Login.UserName);

            try
            {
                signInResult = await signInManager.PasswordSignInAsync(model.Login.UserName, model.Login.Password, false, false);
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "Login", ex);
            }

            if (signInResult.Succeeded)
            {
                if (String.Equals(iSCuncurrentLoginActive, "TRUE", StringComparison.OrdinalIgnoreCase))
                {
                    bool UserIsLoggedIn = ValidationHelper.GetBoolean(user.UserSettings.GetValue("Eurobank_UserIsLoggedIn"), false);
                    DateTime UserLastLoggedIn = ValidationHelper.GetDateTime(user.UserSettings.GetValue("Eurobank_UserLastLoggedIn"), System.DateTime.UtcNow);
                    string UserBrowserName = ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserBrowserName"), "not found");
                    string UserIPAddress = ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserIPAddress"), "not found");
                    string UserSessionID = ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserSessionID"), "");
                    var NewSessionID = Guid.NewGuid();
                    int diffInSeconds = Convert.ToInt32((System.DateTime.UtcNow - UserLastLoggedIn).TotalSeconds);
                    int sessionTimeOut = SettingsKeyInfoProvider.GetIntValue(SiteContext.CurrentSiteName + ".Session_Timeout_key") * 60;
                    if (UserIsLoggedIn == true && diffInSeconds < sessionTimeOut && model.IsPreviousLogout == false)
                    {
                        eventLogService.LogInformation("AccountController.Login(Post)", "CustomLogInfo1", "User Already loggedin " +
                            "\r\n Condition Checked : [UserIsLoggedIn == true && Time-difference < Session TimeOut && IsPreviousLogout == false]" +
                            "\r\n Username : " + ValidationHelper.GetString(model.Login.UserName, "not found") +
                            "\r\n UserIsLoggedIn : " + ValidationHelper.GetString(UserIsLoggedIn, "not found") +
                            "\r\n UserLastLoggedIn(UTC) : " + ValidationHelper.GetString(UserLastLoggedIn, "not found") +
                            "\r\n IsPreviousLogout : " + ValidationHelper.GetString(model.IsPreviousLogout, "not found") +
                            "\r\n UserBrowserName : " + ValidationHelper.GetString(UserBrowserName, "not found") +
                            "\r\n UserIPAddress : " + ValidationHelper.GetString(UserIPAddress, "not found") +
                            "\r\n UserSessionID : " + ValidationHelper.GetString(UserSessionID, "not found") +
                            "\r\n Session TimeOut(in seconds) : " + sessionTimeOut +
                            "\r\n Time difference of current UTC and UserLastLoggedIn(in seconds) : " + diffInSeconds + "");
                        TempData["UserLoginFailed"] = "You have already logged in through " + UserBrowserName + " IP: " + UserIPAddress;
                        TempData["UserPassword"] = model.Login.Password;
                        return View("Login", new LoginRegisterViewModel() { Login = model.Login });
                    }
                    user.UserSettings.SetValue("Eurobank_UserIsLoggedIn", true);
                    user.UserSettings.SetValue("Eurobank_UserLastLoggedIn", System.DateTime.UtcNow);
                    user.UserSettings.SetValue("Eurobank_UserBrowserName", model.BrowserName);
                    user.UserSettings.SetValue("Eurobank_UserIPAddress", model.IpAddress);
                    user.UserSettings.SetValue("Eurobank_UserSessionID", NewSessionID.ToString());
                    user.UserSettings.Update();
                    user.UserInvalidLogOnAttempts = 0;
                    user.Update();
                    HttpContext.Session.SetString("UserSessionID", NewSessionID.ToString());
                    HttpContext.Session.SetString("UserSessionName", model.Login.UserName);

                    eventLogService.LogInformation("AccountController.Login(Post)", "CustomLogInfo1", "User Data updated on database of " +
                            "\r\n Username : " + ValidationHelper.GetString(model.Login.UserName, "not found") +
                            "\r\n UserIsLoggedIn : true" +
                            "\r\n UserLastLoggedIn(UTC) : " + ValidationHelper.GetString(System.DateTime.UtcNow, "not found") +
                            "\r\n UserBrowserName : " + ValidationHelper.GetString(model.BrowserName, "not found") +
                            "\r\n UserIPAddress : " + ValidationHelper.GetString(model.IpAddress, "not found") +
                            "\r\n UserSessionID : " + ValidationHelper.GetString(NewSessionID.ToString(), "not found") +
                            "\r\n" +
                            "\r\n" +
                            "User Data updated on Browser " +
                            "\r\n UserSessionID : " + ValidationHelper.GetString(NewSessionID.ToString(), "not found") +
                            "\r\n UserSessionName : " + ValidationHelper.GetString(model.Login.UserName, "not found") + "");
                }
                else
                {
                    var NewSessionID = Guid.NewGuid();
                    HttpContext.Session.SetString("UserSessionID", NewSessionID.ToString());
                    HttpContext.Session.SetString("UserSessionName", model.Login.UserName);


                    user.UserSettings.SetValue("Eurobank_UserSessionID", NewSessionID.ToString());
                    user.UserSettings.Update();

                    user.UserInvalidLogOnAttempts = 0;
                    user.Update();
                    TempData["iSCuncurrentLoginActive"] = iSCuncurrentLoginActive;
                }
                UserSettingsInfo userSettingsInfo = UserInfoProvider.GetUserInfo(model.Login.UserName).UserSettings;
                RoleInfo role = null;
                if (user != null)
                {
                    // Gets the user's roles
                    var userRoleIDs = UserRoleInfoProvider.GetUserRoles().Column("RoleID").WhereEquals("UserID", user.UserID);
                    role = RoleInfoProvider.GetRoles().WhereIn("RoleID", userRoleIDs).FirstOrDefault();

                }
                var userClaims = new List<Claim>()
                {
                    new Claim("UserName",model.Login.UserName),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role,role!=null? role.RoleName:""),
                    new Claim(ClaimTypes.GivenName, Convert.ToString(userSettingsInfo.GetValue("Eurobank_UserOrganisation")))
                 };

                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                HttpContext.SignInAsync(userPrincipal);

                var decodedReturnUrl = WebUtility.UrlDecode(returnUrl);
                if (!string.IsNullOrEmpty(decodedReturnUrl) && Url.IsLocalUrl(decodedReturnUrl))
                {
                    return Redirect(decodedReturnUrl);
                }

                return Redirect(Url.Kentico().PageUrl(ContentItemIdentifiers.HOME));
            }
            else if (!signInResult.Succeeded)
            {
                if (signInResult.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, localizer["Your account requires activation before logging in."]);
                }
                else
                {
                    if (user != null)
                    {
                        if (user.UserInvalidLogOnAttempts >= Convert.ToInt32(NuberOfFailedAttempt))
                        {
                            user.Enabled = false;
                            user.Update();
                            ModelState.AddModelError(string.Empty, localizer["Your account is locked out"]);
                        }
                        else
                        {
                            user.UserInvalidLogOnAttempts = user.UserInvalidLogOnAttempts + 1;
                            user.Update();
                            ModelState.AddModelError(string.Empty, localizer["Your sign-in attempt was not successful. After " + Convert.ToInt32(NuberOfFailedAttempt) + " unsuccessful attempts your account will be locked"].ToString());
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, localizer["Invalid user name or password"].ToString());
                    }
                }
            }
            return View("Login", new LoginRegisterViewModel() { Login = model.Login });
        }


        // POST: Account/Logout
        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            String iSCuncurrentLoginActive = ResHelper.GetString("EuroBank.Settings.CuncurrentLogin.Active");
            if (String.Equals(iSCuncurrentLoginActive, "TRUE", StringComparison.OrdinalIgnoreCase))
            {
                UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
                user.UserSettings.SetValue("Eurobank_UserIsLoggedIn", false);
                user.UserSettings.Update();
            }
            signInManager.SignOutAsync();
            return Redirect(ContentItemIdentifiers.LOGIN);
        }


        // GET: Account/Register
        //public ActionResult Register()
        //{
        //    return View();
        //}


        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(LoginRegisterViewModel model)
        {
            ViewBag.LoginActive = "";
            ViewBag.LoginShow = "";
            ViewBag.RegisterActive = "active";
            ViewBag.RegisterShow = "show";

            if (!ModelState.IsValid)
            {
                return View("Login", new LoginRegisterViewModel() { Register = model.Register });
            }

            //if (!IsReCaptchValid())
            //{
            //    return View("Login", new LoginRegisterViewModel() { Register = model.Register });
            //}

            var user = new ApplicationUser
            {
                UserName = model.Register.Email,
                FirstName = model.Register.FirstName,
                LastName = model.Register.LastName,
                Email = model.Register.Email,
                FullName = UserInfoProvider.GetFullName(model.Register.FirstName, null, model.Register.LastName),
                Enabled = false,
                WaitingForApproval = true
            };

            var registerResult = new IdentityResult();

            try
            {
                registerResult = await userManager.CreateAsync(user, model.Register.RegPassword);
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "Register", ex);
                ModelState.AddModelError(string.Empty, localizer["Your registration was not successful."]);
            }

            if (registerResult.Succeeded)
            {
                membershipActivitiesLogger.LogRegistration(model.Register.Email);

                var signInResult = await signInManager.PasswordSignInAsync(user, model.Register.RegPassword, true, false);

                if (signInResult.Succeeded)
                {
                    ContactManagementContext.UpdateUserLoginContact(model.Register.Email);

                    //SendRegistrationSuccessfulEmail(user.Email);
                    _AccountService.SendRegistrationSuccessfulMail(user.Email);
                    membershipActivitiesLogger.LogLogin(model.Register.Email);

                    return Redirect(Url.Kentico().PageUrl(ContentItemIdentifiers.HOME));
                }

                if (signInResult.IsNotAllowed)
                {
                    if (user.WaitingForApproval)
                    {
                        //SendWaitForApprovalEmail(user.Email);
                        _AccountService.SendWaitForApprovalMail(user.Email);
                        _AccountService.NewUserApprovalMailsend(user.FirstName, user.LastName, _AccountService.GetUserEmailofRole("Extranet_UserAdmin"), user.Email);
                        //send mail to admin
                    }
                    ModelState.AddModelError(string.Empty, "Your account requires activation before logging in.");
                    return View("Login", new LoginRegisterViewModel() { Register = model.Register });
                    //return RedirectToAction(nameof(RequireConfirmedAccount));
                }
            }

            foreach (var error in registerResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Login", new LoginRegisterViewModel() { Register = model.Register });
        }


        // GET: Account/RetrievePassword
        public ActionResult RetrievePassword()
        {
            return PartialView("_RetrievePassword");
        }

        public string GetCurrentHost()
        {
            var request = httpContextAccessor.HttpContext.Request;
            //string currentUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            string currentUrl = $"{request.Scheme}://{request.Host}";
            return currentUrl;
        }
        // POST: Account/RetrievePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RetrievePassword(RetrievePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_RetrievePassword", model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                token = _AccountService.Encrypt(token);
                //string siteURL = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".Site_URL");
                //string siteURL = SiteContext.CurrentSite.SitePresentationURL;
                string siteURL = GetCurrentHost();
                string resetURL = siteURL + "/Account/ResetPassword?userId=" + user.Id + "&token=" + token;
                //var url = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, token }, RequestContext.URL.Scheme);

                var result = _AccountService.ForgotPassword(user.FirstName, user.Email, resetURL);
                //await emailService.SendEmailAsync(user.Email, localizer["Request for changing your password"],
                //	string.Format(localizer["You have submitted a request to change your password. " +
                //	"Please click <a href=\"{0}\">this link</a> to set a new password.<br/><br/> " +
                //	"If you did not submit the request please let us know."], url));
            }

            return Content(localizer["If the email address is known to us, we'll send a password recovery link in a few minutes."]);
        }


        // GET: Account/ResetPassword
        [HttpGet]
        public async Task<ActionResult> ResetPassword(int userId, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var model = new ResetPasswordViewModel()
            {
                UserId = user.Id,
                Token = token
            };

            return View(model);
        }


        // POST: Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
            {
                return NotFound();
            }
            string token = _AccountService.Decrypt(model.Token);
            var resetResult = await userManager.ResetPasswordAsync(user, token, model.Password);

            if (resetResult.Succeeded)
            {
                return View("ResetPasswordSucceeded");
            }

            foreach (var error in resetResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            //         if(!PasswordHistoryProcess.IsFoundPasswordHistories(model.UserId, model.Password))
            //{
            //             var resetResult = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

            //             if(resetResult.Succeeded)
            //             {
            //                 return View("ResetPasswordSucceeded");
            //             }

            //             foreach(var error in resetResult.Errors)
            //             {
            //                 ModelState.AddModelError(string.Empty, error.Description);
            //             }
            //         }
            //else
            //{
            //             ModelState.AddModelError("Password", "Password sholud be different from last 3");
            //         }



            return View(model);
        }


        // GET: Account/YourAccount
        [Authorize]
        public async Task<ActionResult> YourAccount(bool avatarUpdateFailed = false)
        {
            var model = new YourAccountViewModel
            {
                User = await userManager.FindByNameAsync(User.Identity.Name),
                AvatarUpdateFailed = avatarUpdateFailed
            };

            if (model != null && model.User != null)
            {
                if (string.IsNullOrEmpty(model.User.FullName))
                {
                    model.User.FullName = model.User.FirstName + " " + model.User.LastName;
                }

                // Gets the user
                UserInfo user = UserInfo.Provider.Get(model.User.GUID);
                if (user != null)
                {
                    // Attempts to retrieve a value from a custom text field named 'CustomField'
                    model.UserOrganisation = user.GetValue("Eurobank_UserOrganisation", "");
                    var lookupAddressType = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Culture("en-us")
                    .PublishedVersion().WhereEquals("NodeGUID", ValidationHelper.GetGuid(user.GetValue("Eurobank_UserOrganisation", ""), new Guid())).FirstOrDefault();
                    model.UserOrganisation = lookupAddressType.NodeName;
                }
            }

            return View(model);
        }


        // GET: Account/Edit
        [Authorize]
        public async Task<ActionResult> Edit()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var model = new PersonalDetailsViewModel(user);
            return View(model);
        }


        // POST: Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonalDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.UserName = User.Identity.Name;
                return View(model);
            }

            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                // Set full name only if it was automatically generated
                if (user.FullName == UserInfoProvider.GetFullName(user.FirstName, null, user.LastName))
                {
                    user.FullName = UserInfoProvider.GetFullName(model.FirstName, null, model.LastName);
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                await userManager.UpdateAsync(user);

                return RedirectToAction(nameof(YourAccount));
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AccountController", "Edit", ex);
                ModelState.AddModelError(string.Empty, localizer["Personal details save failed"]);

                model.UserName = User.Identity.Name;
                return View(model);
            }
        }


        // POST: Account/ChangeAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangeAvatar(IFormFile avatarUpload)
        {
            object routeValues = null;

            if (avatarUpload != null && avatarUpload.Length > 0)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (!avatarService.UpdateAvatar(avatarUpload.ToUploadedFile(), user.Id, siteService.CurrentSite.SiteName))
                {
                    routeValues = new { avatarUpdateFailed = true };
                }
            }
            else
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                avatarService.DeleteAvatar(user.Id);
            }

            return RedirectToAction(nameof(YourAccount), routeValues);
        }


        // GET: Account/RequireConfirmedAccount
        [HttpGet]
        public ActionResult RequireConfirmedAccount()
        {
            return View();
        }


        private void SendRegistrationSuccessfulEmail(string email)
        {
            var subject = localizer["Registration information"];
            var body = localizer["Thank you for registering at our site."];

            emailService.SendEmailAsync(email, subject, body);
        }


        private void SendWaitForApprovalEmail(string email)
        {
            var subject = localizer["Your registration must be approved"];
            var body = string.Format(localizer["Thank you for registering at our site {0}. Your registration must be approved by administrator."], siteService.CurrentSite.DisplayName);

            emailService.SendEmailAsync(email, subject, body);
        }

        private bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];

            var secretKey = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSReCaptchaPrivateKey");

            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }
        [HttpGet]
        public ActionResult UserAccessDenied()
        {
            return View();
        }


    }
}