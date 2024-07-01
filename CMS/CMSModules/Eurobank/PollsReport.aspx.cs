using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.UIControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CMSModules_Eurobank_PollsReport : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int pollsID = QueryHelper.GetInteger("PollsID", 0);
        GetQuestion(pollsID);
        

        string customTableClassName = "Eurobank.Polls_PollAnswer";
        // Gets the custom table
        DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
        if (customTable != null)
        {
            DataSet dataSet = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("AnswerPollID", pollsID);
            // Loads a string value from the 'ItemText' field of the 'item1' custom table record
            RepterReportDetails.DataSource = dataSet;
            RepterReportDetails.DataBind();

        }
    }

    private string GetQuestion(int pollsID)
    {
        string name = string.Empty;
        string customTableClassName = "Eurobank.Polls_Poll";
        // Gets the custom table
        DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
        if (customTable != null)
        {
            var polls = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("ItemID", pollsID).FirstOrDefault() ;
            // Loads a string value from the 'ItemText' field of the 'item1' custom table record
            lblQuestion.Text = ValidationHelper.GetString( polls.GetValue("PollQuestion"),"");
            lblDisplayname.Text= ValidationHelper.GetString(polls.GetValue("PollName"), "");

        }
        return name;
    }

    public int CheckVote(object PollsID, object AnswerID)
    {
        int vote = 0;
        try
        {
            string customTableClassName = "Eurobank.Polls_PollAnswer";
            // Gets the custom table
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (customTable != null)
            {
                var eachpollcount = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("AnswerPollID", PollsID).WhereEquals("ItemID", AnswerID).FirstOrDefault(); ;
                var totalPollsCount = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("AnswerPollID", PollsID);
                // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                if (totalPollsCount.Count>0)
                {

                    int sum = totalPollsCount.Sum(i =>Convert.ToInt32( i.GetValue("AnswerCount")));
                    int count = Convert.ToInt32(eachpollcount.GetValue("AnswerCount"));
                    vote = (100 / sum) * count;
                }



            }
            //Check if it is null or empty
            return vote;
        }
        catch
        {
            //If any kind of error occurred don't display it
            return vote;
        }
    }

}
