$(document).ready(function () {
    $('#frmRequestDemo').validate({
        rules: {
            FullName: {
                required: true,
                maxlength: 50
            },
            PhoneNumber: {
                required: true,
                maxlength: 10,
                number: true
            },
            EmailAddress: {
                required: true,
                maxlength: 100
            },
            RestaurantName: {
                required: true,
                maxlength: 50
            },
            NoOfLocation: {
                required: true
            }
        }
    });

    $('#frmTalkToExpert').validate({
        rules: {
            FirstName: {
                required: true,
                maxlength: 50
            },
            LastName: {
                required: true,
                maxlength: 50
            },
            ChoosePlan: {
                required: true
            },
            EmailAddress: {
                required: true,
                maxlength: 100
            },
            PhoneNumber: {
                required: true,
                maxlength: 10,
                number: true
            }
        }
    });

    $('#frmNewsLetter').validate({
        rules: {
            EmailAddress: {
                required: true,
                maxlength: 100
            }
        }
    });

});

function btnRequestDemo() {
    if ($('#frmRequestDemo').valid()) {
        $.ajax({
            url: "/Home/ScheduleDemo",
            method: "POST",
            data: $('#frmRequestDemo').serialize(),
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (result) {
                ajaxindicatorstop();
                $("#requestForDemoModal").modal('hide');
                toastr.success("Your request for demo have been submiited successfully", "success", { timeOut: 2000 });
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while submitting Demo form, please try again later", "Error", { timeOut: 3000 });
            }
        });
    }
}

function btnTalkExpert() {
    if ($('#frmTalkToExpert').valid()) {
        $.ajax({
            url: "/Home/TalkToExpert",
            method: "POST",
            data: $('#frmTalkToExpert').serialize(),
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (result) {
                ajaxindicatorstop();
                $("#talkExpertModal").modal('hide');
                toastr.success("Your request for Talk to expert have been submiited successfully", "success", { timeOut: 2000 });
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while submitting form, please try again later", "Error", { timeOut: 3000 });
            }
        });
    }
}

function btnNewsLetterSubscriber() {
    if ($('#frmNewsLetter').valid()) {
        $.ajax({
            url: "/Home/NewsLetterSubscriber",
            method: "POST",
            data: { 'emailAddress': $('#frmNewsLetter').find('input#EmailAddress').val() },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (result) {
                ajaxindicatorstop();
                $('#frmNewsLetter').find('input#EmailAddress').val('');
                if (result.res > 1) {
                    toastr.success("Your request have been submiited successfully", "success", { timeOut: 2000 });
                }
                if (result.res == -2) {
                    toastr.success("You have already subscribe for Newsletter.", "success", { timeOut: 2000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while submitting form, please try again later", "Error", { timeOut: 3000 });
            }
        });
    }
}

$('body').delegate('.numbers-only', 'keypress', function (e) {
    if (e.which == 46) {
        if ($(this).val().indexOf('.') != -1) {
            return false;
        }
    }
    if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
        return false;
    }
})

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

