$(document).ready(function () {
    if ($("body#OnlineOffline").attr('data-bind') == "True") {
        $("#offlineModal").modal({ backdrop: 'static', keyboard: false });
        $("#offlineModal").modal('show');
    }
    $("#myNavbar ul li:first a").addClass("active");
    $('body').scrollspy({ target: ".navScroll", offset: 50 });
    $("#myNavbar a").on('click', function (event) {
        // Make sure this.hash has a value before overriding default behavior
        if (this.hash !== "") {
            // Prevent default anchor click behavior
            event.preventDefault();
            // Store hash
            var hash = this.hash;
            // Using jQuery's animate() method to add smooth page scroll
            // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 200, function () {
                // Add hash (#) to URL when done scrolling (default click behavior)
                window.location.hash = hash;
            });
        }  // End if

        $("a").removeClass("active");
        // $(".tab").addClass("active"); // instead of this do the below 
        $(this).addClass("active");
    });

    $('.number-format').usPhoneFormat({
        format: '(xxx) xxx-xxxx',
    });
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
    $('#frmContactUs').validate({ // initialize the plugin
        rules: {
            emailAddress: {
                required: true,
                maxlength: 50
            }
        }
    });
    $("#frmGenerateOTP").validate({
        rules: {
            mobileNumber: {
                required: true
            },
        }
    });
    $('#frmOrderInfo').validate({ // initialize the plugin
        rules: {
            txtOrderNo: {
                required: true,
                maxlength: 10
            }
        }
    });
});

function calculateTotalCartAmount() {
    var totalAmount = 0;
    totalAmount += parseFloat($("span.span_price").attr('data-bind'));
    if ($("#hdnIsMultipleSize").val() == "true") {
        if ($("ul.menuItemOption").find('input[name="itemSize"]:checked').length > 0) {
            totalAmount += parseFloat($("ul.menuItemOption").find('input[name="itemSize"]:checked').attr('data-content'));
        }
        else {
            totalAmount += $("#hdnFinalPrice").val();
        }
    }

    $("ul.menuItemOption").find('input[type="checkbox"]:checked').each(function () {
        if ($(this).val() != '') {
            totalAmount = parseFloat(totalAmount) + parseFloat($(this).val());
        }
    })
    totalAmount = parseInt($("#hdnQty").val()) * parseFloat(totalAmount);
}

function openAddToCartModal(index, menuId, type) {
    $.ajax({
        url: "/Menu/GetMenuDetail",
        method: "GET",
        data: { 'index': index, 'menuId': menuId, 'type': type },
        beforeSend: function () {
            ajaxindicatorstart(returnLoadingText());
        },
        success: function (result) {
            ajaxindicatorstop();
            if (result.IsClosed == true) {
                $("#offlineModal").modal('show');
            }
            else {
                $("#addToCartModal").modal('show');
                $("#addToCartModal div.modal-body").html('');
                $("#addToCartModal div.modal-body").html(result);
                $('#frmAddToCart').validate({
                    errorPlacement: function (error, element) {
                        $("span.error").empty();
                        if ($(element).parents('ul').hasClass('required')) {
                            $(element).parents('ul').find('span.error').text(error[0].innerHTML);
                        }
                    }
                });
            }
        },
        error: function (jqXHR) {
            ajaxindicatorstop();
            toastr.error("An error occured while reterving menu info, please try again later", "Error", { timeOut: 3000 });
        }
    });
}

function addToCartSubmit(menuId, name, price, qty, description, imagePath, categoryImagePath) {
    $('ul.addOnsSection').each(function () {
        var minVal = $(this).find('input#hdnMin').val();
        var maxVal = $(this).find('input#hdnMax').val();
        var IsRequired = $(this).find('input#hdnIsRequired').val();
        if ($(this).hasClass('required')) {
            $(this).find('input').each(function () {
                $(this).rules("add",
                    {
                        required: (IsRequired == "true" ? true : false),
                        maxlength: (IsRequired == "true" && parseInt(maxVal) > 0 ? parseInt(maxVal) : false),
                        minlength: (IsRequired == "true" ? parseInt(minVal) : false),
                        messages: {
                            required: "Required",
                            maxlength: "Maximum {0} selection required",
                            minlength: "Minimum {0} selection required"
                        }
                    })
            })
        }
    })
    if ($("#frmAddToCart").valid()) {
        var multipleSizesItemArr = [];
        var addOnschoicesItemArr = [];
        if ($("#hdnIsMultipleSize").val() == "true") {
            $('ul.menuItemOption').each(function () {
                if ($(this).find('input[name="itemSize"]:checked').length > 0) {
                    $(this).find('input[type="radio"]:checked').each(function () {
                        var objItemSize = {
                            'Name': $(this).val(),
                            'Price': $(this).attr('data-content')
                        }
                        multipleSizesItemArr.push(objItemSize);
                    })

                }
            })
        }
        $("ul.addOnsSection").each(function () {
            var objAddOnsItemArr = [];
            $(this).find('input[type="checkbox"]:checked').each(function () {
                var objAddonsItem = {
                    'Name': $(this).attr('data-content'),
                    'Price': $(this).val()
                }
                objAddOnsItemArr.push(objAddonsItem);
            })

            var objAddOns = {
                'Title': $(this).find('input#hdnTitle').val(),
                'IsRequired': $(this).find('input#hdnIsRequired').val(),
                'Min': $(this).find('input#hdnMin').val(),
                'Max': $(this).find('input#hdnMax').val(),
                'AddOnChoiceItems': objAddOnsItemArr
            }
            addOnschoicesItemArr.push(objAddOns);
        })
        var token = $('input[name="__RequestVerificationToken"]', $("#frmAddToCart")).val();
        var params = {
            'Id': menuId,
            'Name': name,
            'Description': description,
            'Price': price,
            'ImagePath': imagePath,
            'CategoryImagePath': categoryImagePath,
            'Qty': qty,
            "IsMultipleSize": $("#hdnIsMultipleSize").val() === "true" ? true : false,
            'ItemSizeList': multipleSizesItemArr,
            'AddOnsItemList': addOnschoicesItemArr,
            'ActionType': $("#frmAddToCart").find("input#hdnActionType").val(),
            'RowIndex': $("#hdnRowIndex").val(),
            '__RequestVerificationToken': token
        }
        $.ajax({
            url: "/Menu/AddToCart",
            method: "POST",
            data: params,
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (result) {
                ajaxindicatorstop();
                $("#addToCartModal").modal('hide');
                $("div.cartView").html('');
                $("div.cartView").html(result.htmlStr);
                getTotalAddedCounter(menuId);
                $("button.proceedBtn").html("Proceed to checkout<span>" + $("#hdnCurrencySymbol").val() + result.totalAmt.toFixed(2) + "</span>");
                if (parseFloat(result.totalAmt) > 0) {
                    $("button.proceedBtn").css('display', 'block');
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                if (jqXHR.status == "500") {
                    toastr.error("Inernal server error occured, please try again later", "Error", { timeOut: 3000 });
                }
                else {
                    toastr.error("An error occured while adding add to cart, please try again later", "Error", { timeOut: 3000 });
                }

            }
        });
    }
}

function removeFromCart(index, id) {
    $.ajax({
        url: "/Menu/RemoveCartItem",
        method: "POST",
        data: { 'index': index, 'id': id },
        beforeSend: function () {
            ajaxindicatorstart(returnLoadingText());
        },
        success: function (result) {
            ajaxindicatorstop();
            $("div.cartView").html('');
            $("div.cartView").html(result.htmlStr);
            getTotalAddedCounter(id);
            $("button.proceedBtn").html("Proceed to checkout<span>$" + result.totalAmt + "</span>");
            if (parseFloat(result.totalAmt) > 0) {
                $("button.proceedBtn").css('display', 'block');
            }
            else {
                $("button.proceedBtn").css('display', 'none');
            }
        },
        error: function (jqXHR) {
            ajaxindicatorstop();
            toastr.error("An error occured while adding item to cart, please try again later", "Error", { timeOut: 3000 });
        }
    });
}

function getQty(index, menuId, ele) {
    if ($(ele).val() == 0) {
        removeFromCart(index, menuId);
    }
    else {
        $.ajax({
            url: "/Menu/EditCartQty",
            method: "POST",
            data: { 'index': index, 'newQty': $(ele).val() },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (result) {
                ajaxindicatorstop();
                $("div.cartView").html('');
                $("div.cartView").html(result.htmlStr);
                getTotalAddedCounter(menuId);
                if (parseFloat(result.totalAmt) > 0) {
                    $("button.proceedBtn").html("Proceed to checkout<span>$" + result.totalAmt + "</span>");
                }
                else {
                    $("button.proceedBtn").css('display', 'none');
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while reterving updating cart info, please try again later", "Error", { timeOut: 3000 });
            }
        });
    }

}

function btnSaveUserInfo() {
    if ($("#frmPlaceOrder").valid()) {
        var params = {
            'CustomerId': $("#Id").val(),
            'PhoneNumber': $("#mobileNumber").val(),
            'FirstName': $("#firstName").val(),
            'LastName': $("#lastName").val(),
            'EmailAddress': $("#emailAddress").val(),
            'Address1': $("#address1").val(),
            'Address2': $("#address2").val(),
            'State': $("#state").val(),
            'City': $("#city").val(),
            'ZipCode': $("#zipCode").val(),
            'Latitude': $("#hdnLatitude").val(),
            'Longitude': $("#hdnLongitude").val()
        }
        $.ajax({
            url: '/Menu/SaveUserInfo',
            method: 'POST',
            data: params,
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (response) {
                ajaxindicatorstop();
                if (response.result === "1") {
                    if (response.type == 'payment') {
                        $("div.paymentSection").html('');
                        $("div.paymentSection").html(response.htmlStr);
                        $("body").find('input#hdnCustomerId').val(response.CustomerId);
                        $(function () {
                            $('#frmPickup').validate({ // initialize the plugin
                                rules: {
                                    pickupDateTime: {
                                        required: true
                                    },
                                    pickupTime: {
                                        required: true
                                    }
                                }
                            });
                        })
                    }
                    else {
                        $("div#userInfoDIv").html('');
                        $("div#userInfoDIv").html(response.htmlStr);
                    }
                }
                if (response.result === "-2") {
                    toastr.warning("No item added in cart, please add some item", "Error", { timeOut: 1000 });
                    window.location.href = '/Menu/GetMenu';
                }
                if (response.result === "-1") {
                    toastr.error("An error ocuured while palacing order, please try again later", "Error", { timeOut: 1000 });
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

function sendOTP() {
    var mobileNumber = $("#mobileNumber").val();
    if ($('#frmGenerateOTP').valid()) {
        $.ajax({
            url: "/Menu/SendOtp",
            method: "POST",
            data: { 'mobileNumber': mobileNumber },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (result) {
                ajaxindicatorstop();
                if (result == "1") {
                    $("div#enterOtpDiv").show();
                    $("#btnSendOTP").hide();
                    $("#btnVerify").show();
                    $("#mobileNumber").attr('disabled', true);
                    $("#changeNumber").show();
                    toastr.success("Otp sent on your mobile number, please verify it.", "Success", { timeOut: 3000 });
                }
                else {
                    toastr.error("An error occured while sending OTP, please try again later", "Error", { timeOut: 3000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while sending OTP, please try again later", "Error", { timeOut: 3000 });
            }
        });
    }
}

function verifyOTP() {
    var mobileNumber = $("#mobileNumber").val();
    $("input[name='Otp']").rules("add",
        {
            required: true,
            maxlength: 4
        })
    if ($('#frmGenerateOTP').valid()) {
        $.ajax({
            url: "/Menu/VerifyOtp_v1",
            method: "POST",
            data: { 'mobileNumber': mobileNumber, 'otp': $("input[name='Otp']").val() },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (res) {
                ajaxindicatorstop();
                if (res.result === "1") {
                    $("#mobileNumber").attr('disabled', true);
                    $("div#userInfoDIv").html('');
                    $("div#userInfoDIv").html(res.htmlStr);
                    $("#enterOtpDiv").hide();
                    $("#btnOTP").hide();
                    $(function () {
                        $('#frmPlaceOrder').validate({ // initialize the plugin
                            rules: {
                                firstName: {
                                    required: true,
                                    maxlength: 50
                                },
                                lastName: {
                                    required: true,
                                    maxlength: 50
                                },
                                emailAddress: {
                                    required: true,
                                    maxlength: 50
                                },
                                address1: {
                                    required: true,
                                    maxlength: 200
                                },
                                address2: {
                                    required: false,
                                    maxlength: 50
                                },
                                state: {
                                    required: true,
                                },
                                city: {
                                    required: true,
                                    maxlength: 50
                                },
                                zipCode: {
                                    required: true,
                                    maxlength: 6
                                },
                                paymentMethod: {
                                    required: true
                                }
                            }
                        });
                    })
                    $(function () {
                        var options = {
                            //types: ['(cities)'],
                            componentRestrictions: { country: ["US", "IN", "ES"]}
                        };
                        var places = new google.maps.places.Autocomplete(document.getElementById('address1'), options);
                        google.maps.event.addListener(places, 'place_changed', function () {
                            var result = places.getPlace();
                            var state_shortname = "N/A";
                            var state_longname = "N/A";
                            var city = "N/A";
                            var zipCode = "N/A";
                            var address_components = result.address_components;
                            $.each(address_components, function (index, component) {
                                var types = component.types;
                                $.each(types, function (index, type) {
                                    if (type == 'locality') {
                                        city = component.long_name;
                                    }
                                    if (type == 'administrative_area_level_1') {
                                        state_shortname = component.short_name;
                                        state_longname = component.long_name;
                                    }
                                    if (type == 'postal_code') {
                                        zipCode = component.long_name;
                                    }
                                });
                            });
                            $("#city").val(city);
                            $("#state").val(state_longname);
                            $("#zipCode").val(zipCode);
                            $("#hdnLatitude").val(result.geometry.location.lat());
                            $("#hdnLongitude").val(result.geometry.location.lng());
                            validateOrder(result.geometry.location.lat(), result.geometry.location.lng())
                        });
                    })
                }
                else {
                    $("div#userInfoDIv").html('');
                    toastr.error("Invalid OTP, please try with correct OTP", "Error", { timeOut: 3000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while verifying OTP, please try again later", "Error", { timeOut: 3000 });
            }
        });
    }
}

function changeNumber() {
    window.location.reload();
}

function btnProceedForPayment() {
    var paymentMethod = $('input[name="paymentMethod"]:checked').val();
    var orderType = $('input[name="pickupDeliveryOption"]:checked').val();
    if (paymentMethod === 'cash') {
        $.ajax({
            url: "/Menu/PlaceOrder",
            method: "POST",
            data: {
                'customerIdStr': $("#hdnCustomerId").val(),
                'specialInstruction': $("#txtSpecialInstruction").val(),
                'orderType': orderType
            },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (res) {
                ajaxindicatorstop();
                if (res.result === -1) {
                    toastr.error("Your cart is empty please add atleast 1 item into cart", "Success", { timeOut: 2000 })
                }
                else {
                    toastr.success("Your order have been placed successfully", "Success", { timeOut: 2000 })
                    window.location.href = '/Menu/PaymentResponse?orderId=' + res.result;
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while placing order, try again later", "Error", { timeOut: 3000 });
            }
        });
    }
    if (paymentMethod === 'payWithCard') {
        $("#hdnSpecialInstruction").val($("#txtSpecialInstruction").val())
        $("#hdnOrderType").val(orderType);
        $("#hdnPickUpDateTime").val($("#pickupDateTime option:selected").text() + ' ' + $("#pickupTime option:selected").text());
        $("#hdnPickUpAddress").val($("span.selectedAddress").text());
        $("button.stripe-button-el").trigger('click');
        //ajaxindicatorstart("Placing Order..Please don't try to refresh the page and wait for the confirmation.")
    }
}

function getTotalAddedCounter(menuId) {
    var counter = 0;
    $("div.cartItemRow").each(function () {
        if ($(this).attr('id') === menuId) {
            counter += parseInt($(this).find('select.qty option:selected').val())
        }
    });
    if (counter > 0) {
        $("span.itemAddedCounter_" + menuId).text(counter).addClass('itemCounter');
    }
    else {
        $("span.itemAddedCounter_" + menuId).text('').removeClass('itemCounter');
    }
}

function showHideAddressPanel(type, ele) {
    if (type === 'add') {
        $("#addDeliveryAddressSection").css('display', 'block');
        $("#editDeliveryAddressSection").css('display', 'none');
        $("div.paymentSection").html('');
        $('body').find("div#addressespanel").css('display', 'none');
    }
    else if (type === 'edit') {
        $("#editDeliveryAddressSection").css('display', 'block');
        $("#addDeliveryAddressSection").css('display', 'none');
        $.ajax({
            url: '/Menu/SaveUserInfo',
            method: 'POST',
            data: { 'CustomerAddressId': $(ele).val(), 'CustomerId': $(ele).attr('data-content') },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (response) {
                ajaxindicatorstop();
                if (response.result === "1") {
                    $("div.paymentSection").html('');
                    $("div.paymentSection").html(response.htmlStr);
                    $("body").find('input#hdnCustomerId').val(response.CustomerId);
                    $(function () {
                        $('#frmPickup').validate({ // initialize the plugin
                            rules: {
                                pickupDateTime: {
                                    required: true
                                }
                            }
                        });
                    })
                }
                if (response.result === "-2") {
                    toastr.warning("No item added in cart, please add some item", "Error", { timeOut: 1000 });
                    window.location.href = '/Menu/GetMenu';
                }
                if (response.IsFarAway === true) {
                    $('input:radio[name=addressRadio]').each(function () { $(this).prop('checked', false); });
                    $("#locationFarAwayModal").modal('show');
                }
                else if (response.IsMinOrderAmtvalid === false) {
                    $('input:radio[name=addressRadio]').each(function () { $(this).prop('checked', false); });
                    $("#minimumOrderAmtModal").modal('show');
                }
                if (response.result === "-1") {
                    toastr.error("An error ocuured while palacing order, please try again later", "Error", { timeOut: 1000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
            }
        });
    }
    else {
        $("#editDeliveryAddressSection").css('display', 'block');
        $("#addDeliveryAddressSection").css('display', 'none');
        $('input:radio[name=addressRadio]').each(function () { $(this).prop('checked', false); });
        $('body').find("div#addressespanel").css('display', 'block');
        //$("body").find('input[name=addressRadio]').each(function () {
        //    debugger
        //    $(this).attr('checked',false);
        //})
    }
}

function validateOrder(destinationLatitude, destinationLongitude) {
    $.ajax({
        url: '/Menu/ValidateOrderDistance',
        method: 'POST',
        data: { 'destinationLatitude': destinationLatitude, 'destinationLongitude': destinationLongitude },
        beforeSend: function () {
            ajaxindicatorstart(returnLoadingText());
        },
        success: function (response) {
            ajaxindicatorstop();
            if (response.IsFarAway === true) {
                $("#address1").val('');
                $("#city").val('');
                $("#state").val('');
                $("#zipCode").val('');
                $("#hdnLatitude").val('');
                $("#hdnLongitude").val('');
                $("#locationFarAwayModal").modal('show');
            }
            else if (response.IsMinOrderAmtvalid === false) {
                $("#address1").val('');
                $("#city").val('');
                $("#state").val('');
                $("#zipCode").val('');
                $("#hdnLatitude").val('');
                $("#hdnLongitude").val('');
                $("#minimumOrderAmtModal").modal('show');
                $("#minimumOrderAmtModal").find('span#minOrder').text(response.MinDeliveryOrderedAmt);
            }
        },
        error: function (jqXHR) {
            ajaxindicatorstop();
            toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
        }
    });
}

function btnSaveContactUs() {
    if ($("#frmContactUs").valid()) {
        $.ajax({
            url: '/Menu/SendContactUs',
            method: 'POST',
            data: { 'emailAddress': $("#frmContactUs").find('input#emailAddress').val() },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (res) {
                ajaxindicatorstop();
                if (res.response === 1) {
                    $("#frmContactUs").find('input#emailAddress').val('')
                    toastr.success("You request have been successfully submitted", "Success", { timeOut: 1000 });
                }
                else {
                    toastr.error("An error ocuured while sending request, please try again later", "Error", { timeOut: 1000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
            }
        });
    }
}

function showHidePickUpDelivery(ele) {
    var addressId = $('input[name="addressRadio"]:checked').val();
    var mobileNumber = $("#mobileNumber").val();
    $.ajax({
        url: '/Menu/loadPaymentOption',
        method: 'GET',
        data: { 'type': ele, 'customerAddressId': addressId, mobileNumber: mobileNumber },
        beforeSend: function () {
            ajaxindicatorstart(returnLoadingText());
        },
        success: function (response) {
            ajaxindicatorstop();
            if (response.result == "1") {
                $("div#paymentMethod").html('');
                $("div#paymentMethod").html(response.htmlStr);
                if (ele === 'PickUp') {
                    $("div#pickUpSection").css('display', 'block');
                    $("div#paymentMethod").css('display', 'none');
                }
                else {
                    $("div#pickUpSection").css('display', 'none');
                    $("div#paymentMethod").css('display', 'block');
                }

            }
            else {
                if (response.IsFarAway === true) {
                    $('input:radio[name=addressRadio]').each(function () { $(this).prop('checked', false); });
                    $("#locationFarAwayModal").modal('show');
                }
                else if (response.IsMinOrderAmtvalid === false) {
                    $('input:radio[name=addressRadio]').each(function () { $(this).prop('checked', false); });
                    $("#minimumOrderAmtModal").modal('show');
                }
                $("div.paymentSection").html('');
            }

        },
        error: function (jqXHR) {
            ajaxindicatorstop();
            toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
        }
    });
}

function continuePickup() {
    console.log($("#pickupDateTime option:selected").text() + ' ' + $("#pickupTime option:selected").text())
    var pickupdateTime = $("#pickupDateTime option:selected").text() + ' ' + $("#pickupTime option:selected").text();
    if ($("#frmPickup").valid()) {
        $.ajax({
            url: '/Menu/ValidatePickupDateTime',
            method: 'POST',
            data: {
                'pickupDateTime': pickupdateTime
            },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (response) {
                ajaxindicatorstop();
                if (response.result === "1") {
                    $("div#paymentMethod").html('');
                    $("div#paymentMethod").html(response.htmlStr);
                    $("div#paymentMethod").css('display', 'block');
                    $("form.editPickUp").hide();
                    $("div#viewPickup").show();
                    $("div#viewPickup").find('span.selectedAddress').text($('input[name="pickupAddress"]:checked').val());
                    $("div#viewPickup").find('span#pickUpdateTime').text(pickupdateTime)
                }
                else {
                    $("div#userInfoDIv").html('');
                    toastr.error("Pickup is not available in mentioned datetime..Please select other datetime", "Error", { timeOut: 1000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
            }
        });
    }
}

function editPickupDateTime() {
    $("div#viewPickup").hide();
    $("form.editPickUp").show();
    $("div#paymentMethod").html('');

}

function validateOrderAmt() {
    $.ajax({
        url: '/Menu/ValidateOrderAmount',
        method: 'POST',
        beforeSend: function () {
            ajaxindicatorstart(returnLoadingText());
        },
        success: function (response) {
            ajaxindicatorstop();
            if (response.IsMinOrderAmtvalid === false) {
                $("#minimumOrderAmtModal").modal('show');
                $("#minimumOrderAmtModal").find('span#minOrder').text(response.MinDeliveryOrderedAmt);
                return false;
            }
            else {
                window.location.href = '/Menu/ProceedForCheckout';
            }
        },
        error: function (jqXHR) {
            ajaxindicatorstop();
            toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
        }
    });
}

function validateOrderInfo() {
    if ($("#frmOrderInfo").valid()) {
        $.ajax({
            url: '/Menu/GetOrderInfo',
            method: 'POST',
            data: { 'orderNo': $("#frmOrderInfo").find('input#txtOrderNo').val() },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (res) {
                ajaxindicatorstop();
                if (res.result === "1") {
                    $('div#orderInfo').html(res.htmlStr);
                }
                else {
                    toastr.error("An error occured while getting order info, please try again later", "Error", { timeOut: 1000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error(jqXHR.responseJSON.error, "Error", { timeOut: 1000 })
            }
        });
    }
}

function openPageContent(ele) {
    $("#pageContentModel").modal('show');
    $("#pageContentModel").find('.modal-title').text($(ele).attr('data-bind'));
    $("#pageContentModel").find('div.modal-body').html('');
    $("#pageContentModel").find('div.modal-body').html($(ele).attr('data-content'));
}

function getPickUpTime(ele) {
    $("#pickupTime").empty();
    $("#pickupTime").append("<option val=''>Select</option>");
    if ($(ele).val() != '') {
        $.ajax({
            url: "/Menu/GetPickUpTime",
            data: {
                'WeekDayId': parseInt($(ele).val())
            },
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (res) {
                ajaxindicatorstop();
                console.log(res)
                if (res.result.length > 0) {
                    //$("#pickupTime").append($("<option></option>").val("").html("Select"));
                    $.each(res.result, function (data, value) {
                        $("#pickupTime").append($("<option></option>").val(value.PickUpTime).html(value.PickUpTime));
                    })
                }
                else {
                    $("#pickupTime").append("<option val=''>Not Available</option>");
                }

            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while placing order, try again later", "Error", { timeOut: 3000 });
            }
        });
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
