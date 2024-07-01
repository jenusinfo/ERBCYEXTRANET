function ShowRespectedField() {
	var SourceOfAnnualIncomeText = $("#SourceOfAnnualIncome").data("kendoDropDownList").text();
	if (SourceOfAnnualIncomeText == "OTHER") {
		$("#SpecifyOtherSourceID").css("display", "block");
	}
	else {
		$("#SpecifyOtherSourceID").css("display", "none");
		$("#SpecifyOtherSource").val('').change().focus().focusout();
	}
}