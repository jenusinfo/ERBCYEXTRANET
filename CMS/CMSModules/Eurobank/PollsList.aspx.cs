using CMS.CustomTables;
using CMS.DataEngine;
using CMS.UIControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CMSModules_Eurobank_PollsList : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string customTableClassName = "Eurobank.Polls_Poll";
        // Gets the custom table
        DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
        if (customTable != null)
        {
            DataSet dataSet = CustomTableItemProvider.GetItems(customTableClassName);
            // Loads a string value from the 'ItemText' field of the 'item1' custom table record
            if (dataSet != null)
            {
                PollsList.DataSource = dataSet;
                PollsList.DataBind();
                PollsList.ReloadData(); ;
                PollsList.OnAction += PollsList_OnAction;
                PollsList.OnExternalDataBound += PollsList_OnExternalDataBound;

            }


        }
    }
    protected object PollsList_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        if (sourceName== "PollOpenFrom")
        {
            DateTime dt = (DateTime)parameter;
          return  dt.ToString("MM/dd/yyyy");
        }
       else if (sourceName == "PollOpenTo")
        {
            DateTime dt = (DateTime)parameter;
            return dt.ToString("MM/dd/yyyy");
        }
        else
        {
            return parameter;
        }

        
    }

    protected void PollsList_OnAction(string actionName, object actionArgument)
    {
        // Implements the logic of the view action
        if (actionName == "edit")
        {
            Response.Redirect("~/CMSModules/Eurobank/PollsAddEdit.aspx?PollsID=" + actionArgument);
        }
        if (actionName == "report")
        {
            Response.Redirect("~/CMSModules/Eurobank/PollsReport.aspx?PollsID=" + actionArgument);
        }
        if (actionName == "deleteaction")
        {
            string customTableClassName = "Eurobank.Polls_Poll";

            // Gets the custom table
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (customTable != null)
            {
                // Gets the first custom table record whose value in the 'ItemText' starts with 'New text'
                CustomTableItem item = CustomTableItemProvider.GetItems(customTableClassName)
                                                                    .WhereEquals("ItemID", actionArgument)
                                                                    .FirstOrDefault();

                if (item != null)
                {
                    // Deletes the custom table record from the database
                    item.Delete();
                }
            }
            Response.Redirect("~/CMSModules/Eurobank/PollsList.aspx");
        }
    }

    protected void btnNewPolls_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/CMSModules/Eurobank/PollsAddEdit.aspx");
    }

    protected void PollsList_OnExternalDataBound1(object sender, string sourceName, object parameter)
    {

    }
}
