$(document).ready(function () {
    $('#frmLogin').validate({ // initialize the plugin
        rules: {
            email: {
                required: true
            },
            password: {
                required: true,
                minlength: 6
            }
        }
    });

});

$("#email").keypress(function (e) {
    if (e.which === 13) {
        login();
    }
});

$("#password").keypress(function (e) {
    if (e.which === 13) {
        login();
    }
});

$("#btnLogin").click(function () {
    login();
});

function login() {
    var token = $('input[name="__RequestVerificationToken"]', $("#frmLogin")).val();
    if ($("#frmLogin").valid()) {
        $.ajax({
            url: '/SecurePanel/Account/Login',
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
                console.log(response)
                if (response === "superadmin" || response === "partner") {
                    toastr.info("Redirecting to Home Page !", "Success", { timeOut: 3000 })
                    window.location.href = '/SecurePanel/Home/Index';
                }
                else if (response === "admin") {
                    toastr.info("Redirecting to Home Page !", "Success", { timeOut: 3000 })
                    window.location.href = '/Admin/Dashboard/Index';
                }
                else {
                    toastr.error(response, "Error", { timeOut: 1000 })
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
