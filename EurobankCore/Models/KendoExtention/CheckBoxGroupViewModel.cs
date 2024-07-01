using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eurobank.Models.KendoExtention
{
    public class CheckBoxGroupViewModel
    {
        public List<IInputGroupItem> Items { get; set; }

        [Required]
        public string[] CheckBoxGroupValue { get; set; }
    }
}
