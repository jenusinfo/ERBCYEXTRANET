using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
    public class PollsWidgetsProperties : IWidgetProperties
    {
        
       // [EditingComponent(DropDownComponent.IDENTIFIER, Order = 3, Label = "Polls")]

        //[EditingComponentProperty("DataSource", "1;Polls\r\n2;Polls test")]
        [EditingComponent("CustomDropDownComponent")]
        public string PollsID { get; set; }


    }
}
