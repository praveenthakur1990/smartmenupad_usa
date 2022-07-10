var i = 1, j = 1, addons = 1, n = 0, objOptionalArr = [], objRecommendationArr = [], IsPublish = true, objSelectedMonthArr = [];
$(document).ready(function () {
    var selectedDaysArr = [];
    if ($("#hdnSelectedDays").val() !== '') {
        var daysArr = $("#hdnSelectedDays").val().split(",");
        for (var i = 0; i < daysArr.length; i++) {
            selectedDaysArr.push(parseInt(daysArr[i]));
        }
        console.log(selectedDaysArr)
        selectedDaysArr.push(selectedDaysArr)
    }
    else {
        //for (var m = 0; m < 7; i++) {
        //    selectedDaysArr.push(parseInt(m));
        //}
        //selectedDaysArr.push(selectedDaysArr)
        //$('ul.weekdays-list li').each(function () {
        //    $(this).attr('selected', false);
        //})
    }
    $('#weekdays').weekdays({
        listClass: 'weekdays-list',
        itemClass: 'weekdays-day',
        itemSelectedClass: 'weekday-selected',
        selectedIndexes: selectedDaysArr
    });

    $('.multiselect-ui').multiselect({
        includeSelectAllOption: false,
        enableFiltering: false,
        maxHeight: 200,
        onChange: function (element, checked) {
            var ele = {};
            if (element.context.parentElement.id == "SeasonalMonth") {
                objSelectedMonthArr = [];
                ele = $('#SeasonalMonth option:selected');
                $(ele).each(function (index, brand) {
                    objSelectedMonthArr.push([$(this).val()]);
                });
            }
            else {
                objRecommendationArr = [];
                ele = $('#RecommendedItems option:selected');
                $(ele).each(function (index, brand) {
                    objRecommendationArr.push([$(this).val()]);
                });
            }

        }
    });
    $("input.switch").bootstrapSwitch({
        onText: 'Yes',
        offText: 'No',
        onColor: 'success',
        offColor: 'danger',
        size: 'small'
    });
    $('#frmAddUpdateMenu').validate({ // initialize the plugin
        rules: {
            name: {
                required: true,
                maxlength: 50
            },
            CategoryId: {
                required: true
            },
            description: {
                maxlength: 250
            },
            VegNonVeg: {
                required: false
            }
        }
    });

    $('input[name="IsPublish"]').on('switchChange.bootstrapSwitch', function (e, data) {
        IsPublish = data;
    });

    if ($("#Id").val() > 0) {
        var hidValue = $("#hdnRecommendationItem").val();
        var RecommendationSelectedOptions = hidValue.split(",");
        objRecommendationArr = [];
        for (var i in RecommendationSelectedOptions) {
            objRecommendationArr.push($.trim(RecommendationSelectedOptions[i]));
            $("#RecommendedItems").find("option[value=" + $.trim(RecommendationSelectedOptions[i] + "]")).prop("selected", "selected");
            $("#RecommendedItems").multiselect("refresh");
        }

        var hdnSeasonalMonth = $("#hdnSeasonalMonth").val();
        var SeasonalMonthselectedOptions = hdnSeasonalMonth.split(",");
        objSelectedMonthArr = [];
        for (var i in SeasonalMonthselectedOptions) {
            objSelectedMonthArr.push($.trim(SeasonalMonthselectedOptions[i]));
            $("#SeasonalMonth").find("option[value=" + $.trim(SeasonalMonthselectedOptions[i] + "]")).prop("selected", "selected");
            $("#SeasonalMonth").multiselect("refresh");
        }
    }
});

function addMoreSizes(ele) {
    i = $(ele).attr('data-content');
    i++;
    //if (i <= 4) {
    $("table#tblMultipleSizes tbody").append('<tr><td><input type="text" class="tdInput validate" name="name_' + i + '" maxlength="50"/></td><td><input type="text" class="tdInput numbers-only validate" name="price_' + i + '" maxlength="20"/></td><td class="text-center"><a href="javascript:void(0);" class="deleteRow" onclick="removeMultipleSizeRow(this)"><i class="sm-delete"></i></a></td></tr>');
    $(ele).attr('data-content', i);
    // }
}

function removeMultipleSizeRow(ele) {
    $(ele).closest('tr').remove();
    i--;
}

function setIsMutipleSizeItem(ele) {
    if (ele.value === 'true') {
        $("div#mutipleSizeDiv").show();
        $("div#SingleSizeDiv").hide();
    }
    else {
        $("div#SingleSizeDiv").show();
        $("div#mutipleSizeDiv").hide();
    }
}

function setIsAddOnsChoicesItem(ele) {
    if (ele.value === 'true') {
        $("div#AddonsChoicesDiv").show();
    }
    else {
        $("div#AddonsChoicesDiv").hide();
    }
}

function setIsOnSelectedDaysItem(ele) {
    if (ele.value === 'true') {
        $("div#SelectedDaysDiv").show();
    }
    else {
        $("div#SelectedDaysDiv").hide();
    }
}

function setIsSeasonalItem(ele) {
    if (ele.value === 'true') {
        $("div#SeasonalItemDiv").show();
    }
    else {
        $("div#SeasonalItemDiv").hide();
    }
}

function addMoreAddOns(ele) {
    addons = $(ele).attr('data-content');
    addons++
    $(ele).attr('data-content', addons);
    var content = '<div class="addChoicesSec" id="addon_' + addons + '"><table class="table table-responsive" id = "tblAddOnsChoices_' + addons + '"><thead><tr><th>Title</th><th>Is Required</th><th>Min</th><th>Max</th><th></th></tr></thead><tbody><tr><td><input type="text" name="title_' + addons + '" class="form-control validate" placeholder="Title" maxlength="50" autocomplete="off" value="" /></td><td><select class="form-control validate" name="IsRequired_addon_' + addons + '" onchange="showMinMax(this)"><option value="">Select</option><option value="true">Yes</option><option value="false">No</option></select></td><td><input type="text" name="min_' + addons + '" class="form-control validate allow-numeric" placeholder="Min" maxlength="2" autocomplete="off" value=""/></td><td><input type="text" name="max_' + addons + '" class="form-control validate allow-numeric" placeholder="Max" maxlength="2" autocomplete="off" value="" /></td><td><a href="javascript:void(0);" class="deleteRow" onclick="removeAddOnsChoicesRow(this)"><i class="sm-delete"></i></a></td></tr></tbody></table><div class="row" id="AddonsChoicesItemDiv_' + addons + '" style="display:none"><div class="col-lg-12"><table class="table table-responsive" id="tblAddOnsChoicesItem_' + addons + '"><thead><tr><th>Name</th><th>Price</th><th class="text-center">Action</th></tr></thead><tbody></tbody></table></div></div><div class="col-lg-12"><button type="button" class="addOptionSetBtn btn btn-primary" onclick="addMoreAddOnsItem(' + addons + ')">(+) Add Options</button></div></div>';
    $('body').find(".copyDiv").append(content);
    $(function () {
        $(".allow-numeric").on("keypress keyup blur", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });
    })
}

function removeAddOnsChoicesRow(ele) {
    $(ele).closest('div').remove();
    addons--;
}

function addMoreAddOnsItem(n) {
    if ($("table#tblAddOnsChoices_" + n + " tbody tr").length > 0) {
        $("table#tblAddOnsChoices_" + n + " tbody tr td input.validate, select.validate").each(function () {
            $(this).rules("add",
                {
                    required: true,
                    maxlength: 200
                })
        });
    }
    $("div#AddonsChoicesItemDiv_" + n).css('display', 'block');
    //if (j <= 9) {
    var endPrefixStr = n + "_" + j;
    $("div#AddonsChoicesItemDiv_" + n + " table#tblAddOnsChoicesItem_" + n + " tbody").append('<tr><td><input type="text" class="form-control validate" name="CustomizationItem_' + endPrefixStr + '" maxlength="200" autocomplete="off"/></td><td><input type="text" class="form-control numbers-only" name="CustomizationItemPrice_' + endPrefixStr + '" maxlength="20" autocomplete="off" value="0.00"/></td><td class="text-center"><a href="javascript:void(0);" class="deleteRow" onclick="removeAddOnsItemRow(this)"><i class="sm-delete"></i></a></td></tr>');
    j++;
    //}
}

function removeAddOnsItemRow(ele) {
    $(ele).closest('tr').remove();
    n--;
}

function readImage(ele) {
    if (ele.files && ele.files[0]) {
        const file = Math.round((ele.files[0].size / 1024));
        if (ele.files[0].type === "image/png" || ele.files[0].type === "image/jpeg" || ele.files[0].type === "image/jpg") {
            // The size of the file. 
            if (file > 2000) {
                toastr.error("File too Big, please select a file less than 2 MB", "Error", { timeOut: 1000 });
                $("#imagePath").val('');
                $("div.preview-image-FSSAI").css('display', 'none');
            }
            else {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#linkUploadFilePath').find('img').attr('src', e.target.result);
                }
                reader.readAsDataURL(ele.files[0]);
                $("div.preview-image-FSSAI").css('display', 'block');
            }
        }
        else {
            toastr.error("Only .png, .jpeg, .jpg files are allowed", "Error", { timeOut: 500 })
            $("#imagePath").val('');
            $("div.preview-image-FSSAI").css('display', 'none');
        }
    }
    else {
        $("#imagePath").val('');
        $("div.preview-image-FSSAI").css('display', 'none');
    }
}

function addUpdateMenu() {
    var IsMultipleSizeItem = $('input[name="IsMultipleSizeItem"]:checked').val();
    var IsAddOnsChoices = $('input[name="IsAddOnsChoices"]:checked').val();
    var IsSelectedDays = $('input[name="IsSelectedDays"]:checked').val();
    var objAddOnsChoicesItemArr = [];
    var selectedWeekDaysArr = [];
    if (IsSelectedDays === "true") {
        $("ul.weekdays-list li").each(function () {
            if ($(this).hasClass('weekday-selected')) {
                selectedWeekDaysArr.push($(this).attr('data-day'))
            }
        })
    }
    var IsSeasonalItem = $('input[name="IsSeasonalItem"]:checked').val();
    if ($('#frmAddUpdateMenu').valid()) {
        if (IsMultipleSizeItem === "true") {
            if ($("table#tblMultipleSizes tbody tr").length > 0) {
                $("table#tblMultipleSizes tbody tr td input.validate").each(function () {
                    $(this).rules("add",
                        {
                            required: true,
                            maxlength: 200
                        })
                });
            }
        }
        else {
            $("div#SingleSizeDiv input[name='price']").rules("add",
                {
                    required: true,
                    maxlength: 200
                })
        }

        if (IsAddOnsChoices === "true") {
            $("div.copyDiv div table").each(function () {
                if ($(this).find('tbody tr').length > 0) {
                    $(this).find('td').find('input.validate, select.validate').each(function () {
                        $(this).rules("add",
                            {
                                required: true,
                                maxlength: 200
                            })
                    })
                }
            })
        }
    }
    if ($("#frmAddUpdateMenu").valid()) {
        var objMultipleSizeArr = [];
        var objAddOnsChoicesArr = [];
        if (IsMultipleSizeItem === "true") {
            $("table#tblMultipleSizes tbody tr").each(function () {
                var objMultipleSize = {
                    'Name': $(this).find('td').eq(0).find('input').val(),
                    'Price': ($(this).find('td').eq(1).find('input').val() == '' ? 0 : $(this).find('td').eq(1).find('input').val())
                }
                objMultipleSizeArr.push(objMultipleSize);
            });
        }
        if (IsAddOnsChoices === "true") {
            $("div.copyDiv div.addChoicesSec").each(function () {
                objAddOnsChoicesItemArr = [];
                var arrId = $(this).attr('id').split('_')[1];
                if ($(this).find('div table#tblAddOnsChoicesItem_' + arrId + ' tbody tr').length > 0) {
                    $(this).find('div table#tblAddOnsChoicesItem_' + arrId + ' tbody tr').each(function () {
                        console.log('div table#tblAddOnsChoicesItem_' + arrId + ' tbody tr')
                        var objAddOnItem = {
                            'Name': $(this).find('td').eq(0).find('input').val(),
                            'Price': ($(this).find('td').eq(1).find('input').val() == '' ? 0 : $(this).find('td').eq(1).find('input').val())
                        }
                        objAddOnsChoicesItemArr.push(objAddOnItem);
                    })
                }

                if ($(this).find('table#tblAddOnsChoices_' + arrId + ' tbody tr').length > 0) {
                    $(this).find('table#tblAddOnsChoices_' + arrId + ' tbody tr').each(function () {
                        var objAddOn = {
                            'Title': $(this).find('td').eq(0).find('input').val(),
                            'IsRequired': $(this).find('td').eq(1).find('select').val(),
                            'Min': ($(this).find('td').eq(2).find('input').val() == '' ? 0 : $(this).find('td').eq(2).find('input').val()),
                            'Max': ($(this).find('td').eq(3).find('input').val() == '' ? 0 : $(this).find('td').eq(3).find('input').val()),
                            'AddOnChoiceItems': objAddOnsChoicesItemArr
                        }
                        objAddOnsChoicesArr.push(objAddOn);
                    })

                }
            })
        }
        if (IsMultipleSizeItem == 'false') {
            if ($("#price").val() == 0 || $("#price").val() == '') {
                if (IsAddOnsChoices == 'false') {
                    toastr.error("Please add item Price or Addons/Choices", '', { timeOut: 2000 });
                    return false;
                }
                if (IsAddOnsChoices == 'true' && objAddOnsChoicesItemArr.length == 0) {
                    toastr.error("Please add an item in Addons/Choices", '', { timeOut: 2000 });
                    return false;
                }
            }
        }

        if (IsMultipleSizeItem == 'false' && parseInt($("#price").val()) == 0) {
            if (objAddOnsChoicesArr.length > 0) {
                var IsAddonsMarkAsRequired = true, IsPriceMarkAsRequired = true;
                for (var i = 0; i < objAddOnsChoicesArr.length; i++) {
                    if (objAddOnsChoicesArr[i].IsRequired == 'true') {
                        IsAddonsMarkAsRequired = false;
                    }
                    for (var j = 0; j < objAddOnsChoicesArr[i].AddOnChoiceItems.length; j++) {
                        if (parseInt(objAddOnsChoicesArr[i].AddOnChoiceItems[j].Price) > 0) {
                            IsPriceMarkAsRequired = false;
                        }
                    }

                }
            }
            if (IsAddonsMarkAsRequired) {
                toastr.error("Please mark as required atleast one AddOns/Choices", '', { timeOut: 2000 });
                return false;
            }

            if (IsPriceMarkAsRequired) {
                toastr.error("Please add atleast one AddOns/Choices item Price", '', { timeOut: 2000 });
                return false;
            }
        }

        if (objAddOnsChoicesArr.length > 0) {
            for (var k = 0; k < objAddOnsChoicesArr.length; k++) {
                var addonTitle = objAddOnsChoicesArr[k].Name;
                if (objAddOnsChoicesArr[k].AddOnChoiceItems.length == 0) {
                    toastr.error("Please add atleast one item in" + addonTitle + "AddOns/Choices", '', { timeOut: 2000 });
                    return false;
                }
            }
        }
        var Imagefile = $("#linkUploadFilePath").find('img').attr('src');
        var formData = new FormData();
        formData.append("Id", $("#Id").val())
        formData.append("Name", $("#name").val());
        formData.append("CategoryId", $("#CategoryId").val());
        formData.append("ImagePath", Imagefile);

        formData.append("itemAs", $("#itemAs").val());
        formData.append("IsPublish", IsPublish);
        formData.append("Price", $("#price").val());
        formData.append("VegNonVeg", $("#VegNonVeg").val());
        formData.append("Description", $("#description").val());
        formData.append("RecommendedItems", (objRecommendationArr.length > 0 ? objRecommendationArr.join(", ") : ""));
        formData.append("IsMultipleSizeItem", IsMultipleSizeItem);
        formData.append("MultipleSizeJsonStr", (objMultipleSizeArr.length > 0 ? JSON.stringify(objMultipleSizeArr) : ""));
        formData.append("IsAddOnsChoices", IsAddOnsChoices);
        formData.append("AddOnsChoicesItemJsonStr", (objAddOnsChoicesArr.length > 0 ? JSON.stringify(objAddOnsChoicesArr) : ""));
        formData.append("IsSelectedDays", IsSelectedDays);
        formData.append("SelectedWeekdays", (selectedWeekDaysArr.length > 0 ? selectedWeekDaysArr.join(", ") : ""));
        formData.append("IsSeasonalItem", IsSeasonalItem);
        formData.append("SelectedMonths", (objSelectedMonthArr.length > 0 ? objSelectedMonthArr.join(", ") : ""));
        formData.append("hdnUploadFile", $("#hdnImagePath").val());
        console.log(formData)
        if (IsSeasonalItem == "true" && objSelectedMonthArr.join(", ") == '') {
            toastr.error("Select a month", "", { timeOut: 2000 });
            return false;
        }

        if (IsSelectedDays == "true" && selectedWeekDaysArr.length == 0) {
            toastr.error("Select week days", "", { timeOut: 2000 });
            return false;
        }
        $.ajax({
            url: "/Admin/Menu/AddUpdateMenu",
            method: "POST",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                ajaxindicatorstart(returnLoadingText());
            },
            success: function (result) {
                ajaxindicatorstop();
                var location = '/Admin/Menu/Index';
                if (parseInt(result) === 1) {
                    toastr.success("Menu have been created successfully !", "Success", { timeOut: 3000 });
                    refreshPageCustom(location, 1000);
                }
                else if (parseInt(result) === 2) {
                    toastr.success("Menu have been updated successfully !", "Success", { timeOut: 3000 });
                    refreshPageCustom(location, 1000);
                }
                else {
                    toastr.error("An error occured while saving Menu, please try again later", "Error", { timeOut: 3000 });
                }
            },
            error: function (jqXHR) {
                ajaxindicatorstop();
                toastr.error("An error occured while saving Menu, please try again later", "Error", { timeOut: 3000 });
            }
        });
    }
}

function markAsDeleted(menuId) {
    swal({
        title: "",
        text: "Are you sure you want to delete?",
        icon: "warning",
        buttons: ["No", "Yes"],
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Admin/Menu/MarkAsDeleted",
                    method: "POST",
                    data: { 'id': menuId },
                    beforeSend: function () {
                        ajaxindicatorstart(returnLoadingText());
                    },
                    success: function (result) {
                        ajaxindicatorstop();
                        var location = '/Admin/Menu/Index';
                        if (parseInt(result) === 1) {
                            toastr.success("Menu have been deleted successfully !", "Success", { timeOut: 3000 });
                            refreshPageCustom(location, 1000);
                        }
                        else {
                            toastr.error("An error occured while deleting Menu, please try again later", "Error", { timeOut: 3000 });
                        }
                    },
                    error: function (jqXHR) {
                        ajaxindicatorstop();
                        toastr.error("An error occured while deleting Menu, please try again later", "Error", { timeOut: 3000 });
                    }
                });
            } else {
                return false;
            }
        });
}

$('body').delegate("a.deleteCustomizationRow", 'click', function () {
    $(this).closest('tr').remove()
})

function showMinMax(ele) {
    if ($(ele).val() == 'true') {
        $(ele).closest('table').find('thead tr').find('th').eq(2).show()
        $(ele).closest('table').find('thead tr').find('th').eq(3).show()

        $(ele).closest('td').parent('tr').find('td').eq(2).show();
        $(ele).closest('td').parent('tr').find('td').eq(3).show();
    }
    else if ($(ele).val() == 'false') {
        $(ele).closest('table').find('thead tr').find('th').eq(2).hide()
        $(ele).closest('table').find('thead tr').find('th').eq(3).hide()

        $(ele).closest('td').parent('tr').find('td').eq(2).hide();
        $(ele).closest('td').parent('tr').find('td').eq(3).hide();
    }
    else {
        $(ele).closest('table').find('thead tr').find('th').eq(2).show()
        $(ele).closest('table').find('thead tr').find('th').eq(3).show()

        $(ele).closest('td').parent('tr').find('td').eq(2).show();
        $(ele).closest('td').parent('tr').find('td').eq(3).show();
    }
}


