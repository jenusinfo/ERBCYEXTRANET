using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using CMS.Helpers;
using Eurobank.Helpers.Common.Authorization;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.External.Application;
using Eurobank.Helpers.External.SSP.Application.Individual;
using Eurobank.Helpers.External.SSP.Application.Legal;
using Eurobank.Models.Api;
using Eurobank.Models.Applications;
using Eurobank.Models.External;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Eurobank.Helpers.Process;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eurobank.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ApplicationStatusController : ControllerBase
    {
        private readonly ApplicationsRepository applicationsRepository;
        public ApplicationStatusController(ApplicationsRepository applicationsRepository)
        {
            this.applicationsRepository = applicationsRepository;
        }

        // GET: api/<ApplicationSatusController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //	return new string[] { "value1", "value2" };
        //}

        // GET api/<ApplicationSatusController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //	return "value";
        //}

        //// POST api/<ApplicationSatusController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // POST api/<ApplicationSatusController>
        //[BasicAuthenticationAttribute()]
        ////[BasicAuthenticationAttribute("EuroIntA_Normal2", "C1tytech", BasicRealm = "your-realm")]
        //[Route("api/changestatus")]
        //[HttpGet()]
        //public IActionResult ChangeStatus(string applicationNumber, string status, string decision)
        //{
        //	var req = HttpContext.Request;
        //	var auth = req.Headers["Authorization"];
        //	if(!String.IsNullOrEmpty(auth))
        //	{
        //		var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.ToString().Substring(6))).Split(':');
        //		var user = new { Name = cred[0], Pass = cred[1] };
        //	}
        //	var result = ApplicationApiProcess.ChangeApplicationStatus(applicationNumber, status, decision);
        //	return Ok(result);
        //}

        //[BasicAuthenticationAttribute()]
        //[Route("api/changedecision")]
        //[HttpGet]
        //public IActionResult ChangeDecision(string applicationNumber, string status, string decision)
        //{
        //	string userName = string.Empty;
        //	ApplicationStatusResultModel result = new ApplicationStatusResultModel()
        //	{
        //		ErrorCode = "600",
        //		IsSuccess = false
        //	};

        //	var req = HttpContext.Request;
        //	var auth = req.Headers["Authorization"];
        //	if(!String.IsNullOrEmpty(auth))
        //	{
        //		var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.ToString().Substring(6))).Split(':');
        //		var user = new { Name = cred[0], Pass = cred[1] };
        //		result = ApplicationApiProcess.ExecuteApplicationDecision(user.Name, applicationNumber, decision);
        //	}

        //	return Ok(result);
        //}

        [Route("api/updatestatus")]
        [HttpPost]
        public IActionResult UpdateStatus(ApplicationStatusRequest applicationStatusRequest)
        {
            string userName = string.Empty;
            ApplicationStatusResultModel result = new ApplicationStatusResultModel()
            {
                ErrorCode = "600",
                IsSuccess = false
            };


            //Uncomment if you need authorization
            //var req = HttpContext.Request;
            //var auth = req.Headers["Authorization"];
            //if(!String.IsNullOrEmpty(auth))
            //{
            //	var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.ToString().Substring(6))).Split(':');
            //	var user = new { Name = cred[0], Pass = cred[1] };
            //	result = ApplicationApiProcess.ExecuteApplicationDecision(user.Name, applicationStatusRequest.ApplicationNumber, applicationStatusRequest.Decision, applicationStatusRequest.Comments);
            //}

            //result = ApplicationApiProcess.ExecuteApplicationDecision(applicationStatusRequest.CallerId, applicationStatusRequest.ApplicationNumber, applicationStatusRequest.Decision, applicationStatusRequest.Comments);

            result = ApplicationApiProcess.ChangeApplicationStatus(applicationStatusRequest.CallerId, applicationStatusRequest.ApplicationNumber, applicationStatusRequest.Status, applicationStatusRequest.Decision, applicationStatusRequest.Comments);

            return Ok(result);
        }

        [Route("Api/UpdateApplicationStatus")]
        [HttpPost]
        public IActionResult UpdateApplicationStatus(ApplicationStatusRequest applicationStatusRequest)
        {
            ApplicationStatusResultModel result = new ApplicationStatusResultModel()
            {
                ErrorCode = "600",
                IsSuccess = false
            };

            result = ApplicationApiProcess.ExecuteApplicationDecision(applicationStatusRequest.CallerId, applicationStatusRequest.ApplicationNumber, applicationStatusRequest.Decision, applicationStatusRequest.Comments);

            return Ok(result);
        }

        //      [Route("api/getxml")]
        //      [HttpPost]
        //      public IActionResult GetXML(int applicationId)
        //{
        //          var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
        //          string applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetails.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");
        //          string xml = string.Empty;
        //          if (string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
        //          {
        //              xml = ApplicationLegalService.GetApplicationLegalElement(applicationId);
        //          }
        //          else
        //          {
        //              xml = ApplicationIndividualService.GetApplicationIndividualElement(applicationId);
        //          }

        //          return Ok(xml);
        //}

        [Route("api/getxmlbyappid")]
        [HttpGet]
        public ContentResult GetXML(int applicationId)
        {
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            if (applicationDetails == null)
            {
                return new ContentResult
                {
                    ContentType = "application/xml",
                    Content = "",
                    StatusCode = 401
                };
            }
            string applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetails.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");
            string xml = string.Empty;
            if (string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
            {
                xml = ApplicationLegalService.GetApplicationLegalElement(applicationId);
            }
            else
            {
                xml = ApplicationIndividualService.GetApplicationIndividualElement(applicationId);
            }

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = xml,
                StatusCode = 200
            };
        }

        [Route("api/getXML")]
        [HttpGet]
        public ContentResult GetXMLbyApplicationNumber(string applicationNumber)
        {
            int applicationId = ApplicationsProcess.GetApplicationId(applicationNumber);
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            if (applicationDetails == null)
            {
                return new ContentResult
                {
                    ContentType = "application/xml",
                    Content = "",
                    StatusCode = 401
                };
            }
            string applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetails.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");
            string xml = string.Empty;
            if (string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
            {
                xml = ApplicationLegalService.GetApplicationLegalElement(applicationId);
            }
            else
            {
                xml = ApplicationIndividualService.GetApplicationIndividualElement(applicationId);
            }

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = xml,
                StatusCode = 200
            };
        }

        //[Route("api/updatestatus")]
        //[HttpPost]
        //public IActionResult UpdateStatus(ApplicationStatusRequest applicationStatusRequest)
        //{
        //	string userName = string.Empty;
        //	ApplicationStatusResultModel result = new ApplicationStatusResultModel()
        //	{
        //		ErrorCode = "600",
        //		IsSuccess = false
        //	};


        //	//Uncomment if you need authorization
        //	//var req = HttpContext.Request;
        //	//var auth = req.Headers["Authorization"];
        //	//if(!String.IsNullOrEmpty(auth))
        //	//{
        //	//	var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.ToString().Substring(6))).Split(':');
        //	//	var user = new { Name = cred[0], Pass = cred[1] };
        //	//	result = ApplicationApiProcess.ExecuteApplicationDecision(user.Name, applicationStatusRequest.ApplicationNumber, applicationStatusRequest.Decision, applicationStatusRequest.Comments);
        //	//}

        //	//result = ApplicationApiProcess.ExecuteApplicationDecision(applicationStatusRequest.CallerId, applicationStatusRequest.ApplicationNumber, applicationStatusRequest.Decision, applicationStatusRequest.Comments);

        //	result = ApplicationApiProcess.ChangeApplicationStatus(applicationStatusRequest.CallerId, applicationStatusRequest.ApplicationNumber, applicationStatusRequest.Status, applicationStatusRequest.Decision);

        //	return Ok(result);
        //}

        //[Route("api/useraccessdenied")]
        //public IActionResult AccessDenied(string applicationNumber, string status, string decision)
        //{
        //	ApplicationStatusResultModel retval = new ApplicationStatusResultModel()
        //	{
        //		StatusCode = (int)ApplicationChangeStatusCode.ACCESS_DENIED,
        //		IsSuccess = false,
        //		ErrorMessage = "Access Denied"
        //	};
        //	return Ok(retval);
        //}

        //// PUT api/<ApplicationSatusController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ApplicationSatusController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}


        #region-------For Test By SB--------------
        [Route("api/CreateNews")]
        [HttpPost]
        public IActionResult CreateNews(string title)
        {
            var result = NewsProcess.Add(title);
            return Ok(result);
        }
        
        #endregion
    }
}
