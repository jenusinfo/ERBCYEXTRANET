using CMS.UIControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Net.Http;
using PayPal.Api;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;

public partial class CMSModules_Eurobank_TestXML : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnXML_Click(object sender, EventArgs e)
    {
		try
		{
            //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Code Behind called');", true);
            //lblMessage.Text = "Code Behind called";
            Getxml(txtApplicationNumber.Text.Trim());
        }
		catch (Exception)
		{

			throw;
		}
    }
    private string Getxml(string applicationNumber)
    {
        //string apiUrl = "http://localhost:61384/api/getXML";
        string apiUrl = ConfigurationManager.AppSettings["GetXMLurl"];

        string FinalUrl = apiUrl + "?applicationNumber=" + applicationNumber;

        HttpClient client = new HttpClient();
        //HttpRequestHeaders requestHeaders = client.DefaultRequestHeaders;

        //requestHeaders.Add("Accept", "application/xml");
        Task<HttpResponseMessage> httpResponse = client.GetAsync(FinalUrl);
        HttpResponseMessage response = httpResponse.Result;
        lblMessage.Text = response.ToString();////////////////////

        HttpStatusCode statuscode = response.StatusCode;
        lblcode.Text = statuscode.ToString();//////////////////

        HttpContent content = response.Content; 
        Task<string> responseData = content.ReadAsStringAsync();
        txtarea.Text = responseData.Result;/////////////////////

        return responseData.Result; 
    }
}
