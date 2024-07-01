$(document).ready(function () {
    $(".sidebarscroll").niceScroll({
        cursorborder: "",
        cursorcolor: "#0F0",
        cursoropacitymax: 0.7,
        boxzoom: false
    });
});
function addConfirmButton(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    var model = e.model;
    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>Confirm</a>').insertAfter(".k-grid-update");
    $(".custom").click(function (e) {
        model.Status = true;
    });
}
function ClearIdentificationNumber() {
    $("#IdentificationDetails_IdentificationNumber").val('').focus();
}