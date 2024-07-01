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


public partial class CMSModules_Eurobank_AnswerAddEdit : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int pollsID = QueryHelper.GetInteger("PollsID", 0);
        int answerID = QueryHelper.GetInteger("AnswerID", 0);
        if (!IsPostBack)
        {
            hdnPollsID.Value = ValidationHelper.GetString(pollsID, "");
            if (answerID > 0)
            {
                string customTableClassName = "Eurobank.Polls_PollAnswer";
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    var pollsData = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("ItemID", answerID).WhereEquals("AnswerPollID", pollsID).FirstOrDefault();
                    // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                    if (pollsData != null)
                    {

                        hdnAnsID.Value = ValidationHelper.GetString(pollsData.GetValue("ItemID"), "");
                        txtAnswer.Text = ValidationHelper.GetString(pollsData.GetValue("AnswerText"), "");
                        txtlavel.Text = ValidationHelper.GetString(pollsData.GetValue("AnswerOrder"), "");

                    }


                }
            }


        }

    }

    

    protected void btnSubmitAnswer_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {

        
        int pollsID = QueryHelper.GetInteger("PollsID", 0);
        string customTableClassName = "Eurobank.Polls_PollAnswer";

        // Gets the custom table
        DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
        if (customTable != null)
        {
            if (ValidationHelper.GetInteger(hdnAnsID.Value, 0) > 0)
            {
                var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
                                                        .WhereEquals("ItemID", hdnAnsID.Value).WhereEquals("AnswerPollID", hdnPollsID.Value);

                // Loops through individual custom table records
                foreach (CustomTableItem item in customTableData)
                {
                    item.SetValue("AnswerText", txtAnswer.Text.Trim());
                    item.SetValue("AnswerOrder", txtlavel.Text.Trim());
                        item.SetValue("AnswerEnabled", true);

                        // Saves the changes to the database
                        item.Update();
                }
            }
            else
            {
                CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);

                // Sets the values for the fields of the custom table (ItemText in this case)
                newCustomTableItem.SetValue("AnswerText", txtAnswer.Text.Trim());
                newCustomTableItem.SetValue("AnswerPollID", pollsID);
                newCustomTableItem.SetValue("AnswerOrder", txtlavel.Text.Trim());
                    newCustomTableItem.SetValue("AnswerEnabled", true);
                    // Save the new custom table record into the database
                    newCustomTableItem.Insert();
            }
        }
        
        // Creates a new custom table item

        Response.Redirect("~/CMSModules/Eurobank/PollsAddEdit.aspx?PollsID=" + hdnPollsID.Value);
        }
    }


}
  

