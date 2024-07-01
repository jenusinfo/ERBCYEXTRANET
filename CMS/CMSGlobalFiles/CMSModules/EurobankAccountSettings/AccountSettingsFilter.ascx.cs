using CMS.DocumentEngine.Types.Eurobank;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CMSGlobalFiles_CMSModules_EurobankAccountSettings_AccountSettingsFilter : FormEngineUserControl
{
    /// <summary>
    /// Gets or sets the value selected within the filter.
    /// </summary>
    public override object Value
    {
        get
        {
            return ddlLookupItems.SelectedValue;
        }
        set
        {
            ddlLookupItems.SelectedValue = ValidationHelper.GetString(value, "");
        }
    }


    /// <summary>
    /// Indicates the NodeAliasPath of field data source name.
    /// </summary>
    public string NodeAliasPath
    {
        get
        {
            // Gets the value from the matching <filterparameter> element in the UniGrid's XML definition
            object parameterValue = GetValue("nodealiaspath");
            return ValidationHelper.GetString(parameterValue, "");
        }
        set
        {
            SetValue("nodealiaspath", value);
        }
    }


    /// <summary>
    /// Indicates the Source field name.
    /// </summary>
    public string Source
    {
        get
        {
            // Gets the value from the matching <filterparameter> element in the UniGrid's XML definition
            object parameterValue = GetValue("source");
            return ValidationHelper.GetString(parameterValue, "");
        }
        set
        {
            SetValue("source", value);
        }
    }


    /// <summary>
    /// Loads the filtering options during the initialization of the control.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Only initializes the culture options on the first page load
        if(IsPostBack == false)
        {
            // Prepares a where condition for loading cultures based on the filter's parameters
            string where = String.Empty;

            if(NodeAliasPath.Equals("")) return;

            // Loads the cultures from the Xperience database based on the where condition
            IEnumerable<LookupItem> lookupItems = LookupItemProvider.GetLookupItems()
                .Path(NodeAliasPath, CMS.DocumentEngine.PathTypeEnum.Children);

            // Binds the loaded cultures to the filter's drop-down list
            ddlLookupItems.DataSource = lookupItems;
            ddlLookupItems.DataBind();

            // Adds the '(any)' filtering options
            ddlLookupItems.Items.Insert(0, new ListItem("(any)", "_any"));
        }
    }


    /// <summary>
    /// Generates the SQL Where condition used to limit the data displayed in the connected UniGrid.
    /// </summary>
    public override string GetWhereCondition()
    {
        string filterPersonType = Value as string;

        // Returns an empty condition if the special (any) option is selected in the filter
        if(filterPersonType.Equals("_any") || Source.Equals(""))
        {
            return String.Empty;
        }

        // Returns a condition for loading users whose preferred content culture matches the filter's value.
        return $"({Source} = '{Value}')";
    }
}
