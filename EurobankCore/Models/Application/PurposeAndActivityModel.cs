using Eurobank.Models.KendoExtention;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Eurobank.Models.Application
{
	public class PurposeAndActivityModel
	{
        //[DisplayName("Eurobank.Application.PurposeAndActivity.Label.ReasonForOpeningTheAccount")]
        //[DataType(DataType.Text)]
        //[MaxLength(200, ErrorMessage = "Eurobank.Application.PurposeAndActivity.RequiredValidationErrorMsg.ReasonForOpeningTheAccount")]
        //public string ReasonForOpeningTheAccount
        //{
        //    get;
        //    set;
        //}

        //[DisplayName("Eurobank.Application.PurposeAndActivity.Label.ExpectedNatureOfInAndOutTransaction")]
        //[DataType(DataType.Text)]
        //[MaxLength(400, ErrorMessage = "Eurobank.Application.PurposeAndActivity.RequiredValidationErrorMsg.ExpectedNatureOfInAndOutTransaction")]
        //public string ExpectedNatureOfInAndOutTransaction
        //{
        //    get;
        //    set;
        //}

        //[DisplayName("Eurobank.Application.PurposeAndActivity.Label.ExpectedFrequencyOfInAndOutTransaction")]
        //[DataType(DataType.Text)]
        //[MaxLength(200, ErrorMessage = "Eurobank.Application.PurposeAndActivity.RequiredValidationErrorMsg.ExpectedFrequencyOfInAndOutTransaction")]
        //public string ExpectedFrequencyOfInAndOutTransaction
        //{
        //    get;
        //    set;
        //}

        [DisplayName("Eurobank.Application.PurposeAndActivity.Label.ExpectedIncomingAmount")]
       
        [DataType(DataType.Text)]
        //[MaxLength(200, ErrorMessage = "Eurobank.Application.PurposeAndActivity.RequiredValidationErrorMsg.ExpectedIncomingAmount")]
        public string ExpectedIncomingAmount
        {
            get;
            set;
        }

        [DisplayName("Eurobank.Application.PurposeAndActivity.Label.ReasonForOpeningTheAccount")]
        //public CheckBoxGroupViewModel ReasonForOpeningTheAccountGroup { get; set; }

        public MultiselectDropDownViewModel ReasonForOpeningTheAccountGroup { get; set; }

        [DisplayName("Eurobank.Application.PurposeAndActivity.Label.ExpectedNatureOfInAndOutTransaction")]
        //public CheckBoxGroupViewModel ExpectedNatureOfInAndOutTransactionGroup { get; set; }

        public MultiselectDropDownViewModel ExpectedNatureOfInAndOutTransactionGroup { get; set; }
        

        [DisplayName("Eurobank.Application.PurposeAndActivity.Label.ExpectedFrequencyOfInAndOutTransaction")]
        public RadioGroupViewModel ExpectedFrequencyOfInAndOutTransactionGroup { get; set; }

        [DisplayName("Eurobank.Application.PurposeAndActivity.Label.SignatureMandateType")]
        public RadioGroupViewModel SignatureMandateTypeGroup { get; set; }
    }
}
