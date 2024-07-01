﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.KendoExtention
{
	public class MultiselectDropDownViewModel
    {
        public List<SelectListItem> Items { get; set; }

        [Required]
        public string[] MultiSelectValue { get; set; }
    }
}
