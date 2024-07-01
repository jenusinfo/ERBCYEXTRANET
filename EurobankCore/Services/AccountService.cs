using CMS.EmailEngine;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.Common.Communication;
using Eurobank.Helpers.DataAnnotation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eurobank.Services
{
    public class AccountService
    {
        public bool ForgotPassword(string userName, string receipentEmail, string url)
        {
            bool retval = true;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ForgotPassword, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("name", userName);
            resolver.SetNamedSourceData("resetURL", url);
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = receipentEmail;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, true);
            return retval;
        }
        public string Decrypt(string cipherText)
        {
            string Salt = "Zn@78gTXP6";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Salt, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }



        public string Encrypt(string clearText)
        {
            string Salt = "Zn@78gTXP6";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Salt, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;

        }

        public bool NewUserApprovalMailsend(string first_name, string last_name, string receipentEmails, string email)
        {
            bool retval = true;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.NewUserApproval, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("first_name", first_name);
            resolver.SetNamedSourceData("last_name", last_name);
            resolver.SetNamedSourceData("email", email);
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = receipentEmails;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, true);
            return retval;
        }

        public string GetUserEmailofRole(string rolename)
        {
            string retval = string.Empty;
            RoleInfo role = RoleInfoProvider.GetRoleInfo(rolename, SiteContext.CurrentSiteName);

            if (role != null)
            {
                // Gets the role's users
                var roleUserIDs = UserRoleInfoProvider.GetUserRoles().Column("UserID").WhereEquals("RoleID", role.RoleID);
                var users = UserInfoProvider.GetUsers().WhereIn("UserID", roleUserIDs);

                retval = string.Join(";", users.Where(p => !string.IsNullOrEmpty(p.Email)).Select(k => k.Email));
                // Loops through the users
                //foreach (UserInfo user in users)
                //{

                //}
            }
            return retval;
        }

        public bool SendWaitForApprovalMail(string receipentEmails)
        {
            bool retval = true;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.RegistrationApprovalRequired, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = receipentEmails;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, true);
            return retval;
        }

        public bool SendRegistrationSuccessfulMail(string receipentEmails)
        {
            bool retval = true;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.RegistrationSuccessfulEmail, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = receipentEmails;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, true);
            return retval;
        }

    }
}
