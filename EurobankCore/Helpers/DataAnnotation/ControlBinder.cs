using CMS.DocumentEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using Eurobank.Models.Applications;
using Eurobank.Models.KendoExtention;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Eurobank.Helpers.DataAnnotation
{
	public class ControlBinder
	{
        #region Common Control Binding

        public static CheckBoxGroupViewModel BindCheckBoxGroupItems(List<IInputGroupItem> inputGroupItems, string selectedValues, char seperater)
        {
            CheckBoxGroupViewModel retVal = new CheckBoxGroupViewModel();

            if(inputGroupItems != null && inputGroupItems.Count > 0)
            {
                retVal.Items = inputGroupItems;
                if(!string.IsNullOrEmpty(selectedValues) && seperater != '\0')
				{
                    string[] selectedValueList = selectedValues.Split(new char[] { seperater }, StringSplitOptions.RemoveEmptyEntries);
                    retVal.CheckBoxGroupValue = selectedValueList;
                }
            }

            return retVal;
        }

        public static RadioGroupViewModel BindRadioButtonGroupItems(List<IInputGroupItem> inputGroupItems, string selectedValue)
        {
            RadioGroupViewModel retVal = new RadioGroupViewModel();

            if(inputGroupItems != null && inputGroupItems.Count > 0)
            {
                retVal.Items = inputGroupItems;
                if(!string.IsNullOrEmpty(selectedValue))
                {
                    retVal.RadioGroupValue = selectedValue;
                }
            }

            return retVal;
        }

        public static MultiselectDropDownViewModel BindMultiselectDropdownItems(List<SelectListItem> inputGroupItems, string selectedValues, char seperater)
        {
            MultiselectDropDownViewModel retVal = new MultiselectDropDownViewModel();

            if(inputGroupItems != null && inputGroupItems.Count > 0)
            {
                retVal.Items = inputGroupItems;
                if(!string.IsNullOrEmpty(selectedValues) && seperater != '\0')
                {
                    string[] selectedValueList = selectedValues.Split(new char[] { seperater }, StringSplitOptions.RemoveEmptyEntries);
                    retVal.MultiSelectValue = selectedValueList;
                }
            }

            return retVal;
        }

        #endregion
    }
}
