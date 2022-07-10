$(document).ready(function () {
    $('#frmRegister').validate({ // initialize the plugin
        rules: {
            email: {
                required: true,
                customEmail: true
            },
            password: {
                required: true
                //passwordCheck: true
            },
            confirmPassword: {
                required: true,
                equalTo: "#password"
            },
            terms: {
                required: true
            }
        },
        messages: {
            ConfirmPassword: {
                equalTo: "Password and Confirm Password should be same"
            },
        }
    });

    jQuery.validator.addMethod("customEmail", function (value, element) {
        // allow any non-whitespace characters as the host part
        var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
        return this.optional(element) || pattern.test(value);
    }, 'Please enter a valid email address.');
});
$("#email").keypress(function (e) {
    if (e.which === 13) {
        register();
    }
});

$("#password").keypress(function (e) {
    if (e.which === 13) {
        register();
    }
});

$("#btnRegister").click(function () {
    register();
});

function register() {
    var token = $('input[name="__RequestVerificationToken"]', $("#frmRegister")).val();
    if ($("#frmRegister").valid()) {
        $.ajax({
            url: '/SecurePanel/Account/Register',
            method: 'POST',
            data: {
                email: $("#email").val(),
                password: $("#password").val(),
                __RequestVerificationToken: token,
            },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (response) {
                ajaxindicatorstop();
                if (response === "2") {
                    toastr.error("Your account have been lockout..Please contact site adminstrator", "Error", { timeOut: 1000 })
                }
                else if (response === "-1") {
                    toastr.error("Your account have not verified yet. Please verify it.", "Error", { timeOut: 1000 })
                }
                else if (response === "3") {
                    toastr.error("Sign In failed", "Error", { timeOut: 1000 })
                }
                else if (response === "4") {
                    toastr.error("Invalid login attempt", "Error", { timeOut: 1000 })
                }
                else if (response == "Admin") {
                    toastr.info("Redirecting to Home Page !", "Success", { timeOut: 3000 })
                    window.location.href = '/SecurePanel/Index';
                }
                else if (response == "Company") {
                    toastr.info("Redirecting to Home Page !", "Success", { timeOut: 3000 })
                    window.location.href = '/Portal/Index';
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
            }
        });
    }
    else {
        return false;
    }
}


function returnLoadingText() {
    return "Please wait...";
}

function ajaxindicatorstart(text) {
    if (jQuery('body').find('#resultLoading').attr('id') != 'resultLoading') {
        jQuery('body').append('<div id="resultLoading"><div><img src="/Content/img/rotating-balls-spinner.svg"><div>' + text + '</div></div><div class="bg"></div></div>');
    }
    jQuery('#resultLoading').css({
        'width': '100%',
        'height': '100%',
        'position': 'fixed',
        'z-index': '10000000',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto'
    });

    jQuery('#resultLoading .bg').css({
        'background': '#000000',
        'opacity': '0.7',
        'width': '100%',
        'height': '100%',
        'position': 'absolute',
        'top': '0'
    });

    jQuery('#resultLoading>div:first').css({
        'width': '250px',
        'height': '75px',
        'text-align': 'center',
        'position': 'fixed',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto',
        'font-size': '16px',
        'z-index': '10',
        'color': '#ffffff'

    });

    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeIn(300);
    jQuery('body').css('cursor', 'wait');
}

function ajaxindicatorstop() {
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeOut(300);
    jQuery('body').css('cursor', 'default');
}
