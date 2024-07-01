function CopyCardHolderName() {
	//debugger;
	var name = $("#DebitCardDetails_CardholderName").data("kendoDropDownList").text().toUpperCase();
	$("#DebitCardDetails_FullName").val(name).change().focus().focusout();
	FillIdentificationIndividual();
}
function FillIdentificationIndividual() {
	GetCardHolderAddresss();
}