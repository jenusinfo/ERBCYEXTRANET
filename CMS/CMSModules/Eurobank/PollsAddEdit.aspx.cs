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


public partial class CMSModules_Eurobank_PollsAddEdit : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int pollsID = QueryHelper.GetInteger("PollsID", 0);
        if (!IsPostBack)
        {
            if (pollsID > 0)
            {
                GetAllAnswer(pollsID);
                string customTableClassName = "Eurobank.Polls_Poll";
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    var pollsData = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("ItemID", pollsID).FirstOrDefault();
                    // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                    if (pollsData != null)
                    {
                        hdnPollsID.Value = ValidationHelper.GetString(pollsData.GetValue("ItemID"), "");
                        txtName.Text = ValidationHelper.GetString(pollsData.GetValue("PollName"), "");
                        txtQuestion.Text = ValidationHelper.GetString(pollsData.GetValue("PollQuestion"), "");
                        txtMessage.Text = ValidationHelper.GetString(pollsData.GetValue("PollResponseMessage"), "");
                        txtOpenDate.Text = Convert.ToDateTime(pollsData.GetValue("PollOpenFrom")).ToString("MM/dd/yyyy");
                        txtToDate.Text = Convert.ToDateTime(pollsData.GetValue("PollOpenTO")).ToString("MM/dd/yyyy");
                        cbActive.Checked= ValidationHelper.GetBoolean(pollsData.GetValue("Isactive"), false);
                    }
                   

                }
               
            }
            else
            {
                pnlAnswer.Visible = false;
            }
           
        }
        PollsAnsList.OnAction += PollsAnsList_OnAction;

    }

    private void GetAllAnswer(int pollsID)
    {
        string customTableClassName = "Eurobank.Polls_PollAnswer";

        // Gets the custom table
        DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
        if (customTable != null)
        {
            // Gets the first custom table record whose value in the 'ItemName' field is equal to "SampleName"
            DataSet answerList = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("AnswerPollID", pollsID).OrderByDescending("AnswerOrder");

            // Loads a string value from the 'ItemText' field of the 'item1' custom table record
            if (answerList!=null)
            {
               
                    PollsAnsList.DataSource = answerList;
                    PollsAnsList.DataBind();
                    PollsAnsList.ReloadData(); ;
                    

                
            }
        }
    }

    protected void PollsAnsList_OnAction(string actionName, object actionArgument)
    {
        int pollsID = QueryHelper.GetInteger("PollsID", 0);
        // Implements the logic of the view action
        if (actionName == "edit")
        {
            Response.Redirect("~/CMSModules/Eurobank/AnswerAddEdit.aspx?PollsID=" + pollsID + "&AnswerID=" + Convert.ToString(actionArgument));
        }
        if (actionName == "deleteaction")
        {
            string customTableClassName = "Eurobank.Polls_PollAnswer";

            // Gets the custom table
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (customTable != null)
            {
                // Gets the first custom table record whose value in the 'ItemText' starts with 'New text'
                CustomTableItem item = CustomTableItemProvider.GetItems(customTableClassName)
                                                                    .WhereEquals("ItemID", actionArgument).WhereEquals("AnswerPollID", pollsID)
                                                                    .FirstOrDefault();

                if (item != null)
                {
                    // Deletes the custom table record from the database
                    item.Delete();
                }
            }
            Response.Redirect("~/CMSModules/Eurobank/PollsAddEdit.aspx?PollsID=" + pollsID);
        }
    }

    protected void btnPolls_Click(object sender, EventArgs e)
    {
        string customTableClassName = "Eurobank.Polls_Poll";

        // Gets the custom table
        DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
        if (customTable != null)
        {
            if (ValidationHelper.GetInteger(hdnPollsID.Value, 0) > 0)
            {
                var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
                                                        .WhereEquals("ItemID", hdnPollsID.Value);

                // Loops through individual custom table records
                foreach (CustomTableItem item in customTableData)
                {
                    item.SetValue("PollName", txtName.Text.Trim());
                    item.SetValue("PollQuestion", txtQuestion.Text.Trim());
                    item.SetValue("PollResponseMessage", txtMessage.Text.Trim());
                    if (!string.IsNullOrEmpty(txtOpenDate.Text.Trim()))
                    {
                        item.SetValue("PollOpenFrom", txtOpenDate.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
                    {
                        item.SetValue("PollOpenTO", txtToDate.Text.Trim());
                    }
                    item.SetValue("Isactive", cbActive.Checked);
                    // Saves the changes to the database
                    item.Update();
                }
            }
            else
            {
                CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);

                // Sets the values for the fields of the custom table (ItemText in this case)
                newCustomTableItem.SetValue("PollName", txtName.Text.Trim());
                newCustomTableItem.SetValue("PollQuestion", txtQuestion.Text.Trim());
                newCustomTableItem.SetValue("PollResponseMessage", txtMessage.Text.Trim());
                if (!string.IsNullOrEmpty (txtOpenDate.Text.Trim()))
                {
                    newCustomTableItem.SetValue("PollOpenFrom", txtOpenDate.Text.Trim());
                }
                if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
                {
                    newCustomTableItem.SetValue("PollOpenTO", txtToDate.Text.Trim());
                }
                newCustomTableItem.SetValue("Isactive", cbActive.Checked);
                // Save the new custom table record into the database
                newCustomTableItem.Insert();
            }
        }
        
        // Creates a new custom table item
        
        Response.Redirect("~/CMSModules/Eurobank/PollsList.aspx");
    }

    protected void btnPollsAns_Click(object sender, EventArgs e)
    {
        
            int pollsID = QueryHelper.GetInteger("PollsID", 0);
            Response.Redirect("~/CMSModules/Eurobank/AnswerAddEdit.aspx?PollsID=" + pollsID);
        
       
       
    }
}

