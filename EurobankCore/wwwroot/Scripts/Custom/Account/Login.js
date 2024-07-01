$(function () {
    $('#btnRegistrationSubmit').prop('disabled', true);
});

$('#chkAcceptTermsAndConditions, #chkAcceptPrivacyPolicy').click(function () {
    //if ($('#chkAcceptTermsAndConditions').is(':checked') && $('#chkAcceptPrivacyPolicy').is(':checked')) {
    //    $('#btnRegistrationSubmit').prop('disabled', false);
    //}
    //else {
    //    $('#btnRegistrationSubmit').prop('disabled', true);
    //}
    if ($('#chkAcceptTermsAndConditions').is(':checked')) {
        $('#btnRegistrationSubmit').prop('disabled', false);
    }
    else {
        $('#btnRegistrationSubmit').prop('disabled', true);
    }
});

function btnRegistrationSubmit_OnClick() {
    if (typeof (grecaptcha) != 'undefined') {
        var response = grecaptcha.getResponse();
        if (response.length == 0) {
            $('#Register_ReCaptcha_Error').html('Please validate captcha');
            return false;
        }
    }
}

function btnContinue_OnClick() {
    //debugger;
    $('#btnContinue').attr('disabled', 'disabled');
    $('#continueMsg').show();
    $('#IsPreviousLogout').val("true");
    $('#Login_Password').val($("#UserPassword").val());
    var model = {
        Login: {
            UserName: $('#Login_UserName').val(),
            Password: $('#Login_Password').val()
        },
        BrowserName: $('#BrowserName').val(),
        IpAddress: $('#IpAddress').val(),
        IsPreviousLogout: $('#IsPreviousLogout').val()
    }
    var returnUrl = $("#ReturnUrl").val();
    $.ajax({
        async: false,
        type: "POST",
        url: $("#LoginUrl").val(),
        cache: false,
        data: { "model": model, "returnUrl": returnUrl },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function () {
            $('#loginModal').modal('hide');
            window.location.href = $("#PageUrl").val();
        }
    })
}
function getBrowserName(userAgent) {
    if (userAgent.includes("Firefox")) {
        return "Mozilla Firefox";
    } else if (userAgent.includes("SamsungBrowser")) {
        return "Samsung Internet";
    } else if (userAgent.includes("Opera") || userAgent.includes("OPR")) {
        return "Opera";
    } else if (userAgent.includes("Trident")) {
        return "Microsoft Internet Explorer";
    } else if (userAgent.includes("Edge")) {
        return "Microsoft Edge (Legacy)";
    } else if (userAgent.includes("Edg")) {
        return "Microsoft Edge (Chromium)";
    } else if (userAgent.includes("Chrome")) {
        return "Google Chrome or Chromium";
    } else if (userAgent.includes("Safari")) {
        return "Apple Safari";
    } else {
        return "unknown";
    }
}
