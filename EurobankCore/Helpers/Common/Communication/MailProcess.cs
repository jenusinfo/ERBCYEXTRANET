using CMS.EmailEngine;
using CMS.MacroEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Common.Communication
{
	public class MailProcess
	{
		public static bool SendEmail(EmailMessage message, string templateName, MacroResolver resolver, bool isSendImmediately)
		{
			bool isSuccess = true;
            try
            {
				if(string.IsNullOrEmpty(message.From))
				{
                    message.From = "dummy@tmail.com";
                }
                EmailSender.SendEmail("", message, templateName, resolver, isSendImmediately);
               
            }
            catch (Exception ex)
            {
                isSuccess = false;
                //throw;
            }
            return isSuccess;
		}
        public static bool SendEmailWithTemplate(EmailMessage message, EmailTemplateInfo eti, MacroResolver resolver, bool isSendImmediately)
        {
            bool isSuccess = true;
            try
            {
                
                EmailSender.SendEmailWithTemplateText("", message, eti, resolver, isSendImmediately);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                //throw;
            }
            return isSuccess;
        }
    }
}
