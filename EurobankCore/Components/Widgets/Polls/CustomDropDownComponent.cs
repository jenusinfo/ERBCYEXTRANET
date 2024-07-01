using System.Linq;
using System.Collections.Generic;

using CMS.DocumentEngine;

using Kentico.Forms.Web.Mvc;
using Kentico.Web.Mvc;

using Eurobank.Components.Widgets.Polls;
using CMS.DataEngine;
using CMS.CustomTables;
using System;

[assembly: RegisterFormComponent(CustomDropDownComponent.IDENTIFIER, typeof(CustomDropDownComponent), "Drop-down with custom data", IconClass = "icon-menu")]

namespace Eurobank.Components.Widgets.Polls
{
    public class CustomDropDownComponent : SelectorFormComponent<CustomDropDownComponentProperties>
    {
        public const string IDENTIFIER = "CustomDropDownComponent";


        // Retrieves data to be displayed in the selector
        protected override IEnumerable<HtmlOptionItem> GetHtmlOptions()
        {
            // Perform data retrieval operations here
            // The following example retrieves all pages of the 'DancingGoatMvc.Article' page type
            // located under the 'Articles' section of the Dancing Goat sample website

            string customTableClassName = "Eurobank.Polls_Poll";
            // Gets the custom table
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            
                var question = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("Isactive", true).ToList();
                // Loads a string value from the 'ItemText' field of the 'item1' custom table record
                
            
          

            var sampleData = question.ToList().Select(x => new {
                Name = Convert.ToString( x.GetValue("PollName")),
                Guid = x.ItemID.ToString()
            });

            // Iterates over retrieved data and transforms it into SelectListItems
            foreach(var item in sampleData)
            {
                var listItem = new HtmlOptionItem()
                {
                    Value = item.Guid,
                    Text = item.Name
                };

                yield return listItem;
            }
        }
    }
}