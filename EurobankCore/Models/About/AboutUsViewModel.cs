﻿using System.Collections.Generic;

namespace Eurobank.Models
{
    public class AboutUsViewModel
    {
        public List<AboutUsSectionViewModel> Sections { get; set; }

        public ReferenceViewModel Reference {get; set;}
    }
}