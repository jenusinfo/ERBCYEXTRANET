using CMS.Base.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.SiteProvider;
using CMS.DataEngine;

namespace ClassLibrary2
{
    public class CustomWebAPIController : ApiController
    {
        public HttpResponseMessage Get(String applicationNumber)
        {
            string Efs_Url = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".Print_Summary_URL");
            var sessionXML = Getxml(applicationNumber);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage response = client.PostAsync(Efs_Url, new StringContent(sessionXML, UTF8Encoding.UTF8, "application/XML")).Result;
            return response;
        }

        private string Getxml(string applicationNumber)
        {
            var frontEndURL = SiteContext.CurrentSite.SitePresentationURL; //http://localhost:61384

            string FinalUrl = frontEndURL + "/api/getXML?applicationNumber=" + applicationNumber;

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(30);
            Task<HttpResponseMessage> httpResponse = client.GetAsync(FinalUrl);
            HttpResponseMessage response = httpResponse.Result;

            HttpStatusCode statuscode = response.StatusCode;

            HttpContent content = response.Content;
            Task<string> responseData = content.ReadAsStringAsync();
            return responseData.Result;
        }
    }
}
