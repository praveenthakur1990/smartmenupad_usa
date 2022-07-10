function readImage(ele) {
    var _type = $(ele).attr('name');
    if (ele.files[0].type === "video/mp4") {
        //if (_type === "banner" || _type === "promotional") {

        //}
        //else {
        //    toastr.error("Only .png, .jpeg, .jpe files are allowed", "Error", { timeOut: 1000 });
        //    $(ele).val('');
        //    return false;
        //}

        toastr.error("Only .png, .jpeg, .jpg files are allowed", "Error", { timeOut: 1000 });
        $(ele).val('');
        return false;
    }
    if (ele.files && ele.files[0]) {
        const file = Math.round((ele.files[0].size / 1024));
        if (ele.files[0].type === "image/png" || ele.files[0].type === "image/jpeg" || ele.files[0].type === "image/jpg" || ele.files[0].type === "video/mp4") {
            // The size of the file. 
            if (ele.files[0].type === "video/mp4" && file > 5000) {
                toastr.error("Video too Big, please select a video less than 5 MB", "Error", { timeOut: 1000 });
                $(ele).val('');
                return false;
            }
            if ((ele.files[0].type === "image/png" || ele.files[0].type === "image/jpeg" || ele.files[0].type === "image/jpg") && file > 2000) {
                toastr.error("File too Big, please select a file less than 2 MB", "Error", { timeOut: 1000 });
                $(ele).val('');
                return false;
            }
            else {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var ext = ele.files[0].type;
                    ext = ext.split('/')[1];
                    if (ele.files[0].type === "image/png" || ele.files[0].type === "image/jpeg" || ele.files[0].type === "image/jpg") {
                        $(ele).parents("div.form-group").find('video#vdo').hide();
                        $(ele).parents("div.form-group").find('a#linkUploadFilePath').show();
                        $(ele).parents("div.form-group").find('a#linkUploadFilePath').attr('data-content', '.' + ext);
                        $(ele).parents("div.form-group").find('a#linkUploadFilePath').attr('href', e.target.result);
                        $(ele).parents("div.form-group").find('img').attr('src', e.target.result);
                    }
                    else {
                        $(ele).parents("div.form-group").find('a#linkUploadFilePath').hide();
                        $(ele).parents("div.form-group").find('a#linkUploadFilePath').attr('href', e.target.result);
                        $(ele).parents("div.form-group").find('a#linkUploadFilePath').attr('data-content', '.' + ext);
                        $(ele).parents("div.form-group").find('video#vdo').show();
                        $(ele).parents("div.form-group").find('video#vdo').attr('src', e.target.result);
                    }

                }
                reader.readAsDataURL(ele.files[0]);
            }
        }
        else {
            toastr.error("Only .png, .jpeg, .jpg, .mp4 files are allowed", "Error", { timeOut: 500 })
            $(ele).val('');
            return false;
        }
    }
    else {
        $(ele).val('');
        return false;
    }
}

function saveImages() {
    var imagesArr = [];
    $("form#frmAddUpdateImages").find("a#linkUploadFilePath").each(function () {
        var obj = {
            'Type': $(this).attr('name'),
            'ImagePath': $(this).attr('href'),
            'Ext': $(this).attr('data-content')
        }
        imagesArr.push(obj);
    })
    console.log(imagesArr)
    $.ajax({
        url: "/Admin/Dashboard/SaveImages",
        method: "POST",
        data: JSON.stringify(imagesArr),
        traditional: true,
        cache: false,
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            ajaxindicatorstart(returnLoadingText());
        },
        success: function (result) {
            ajaxindicatorstop();
            if (result === "1") {
                toastr.success("Images have been uploaded successfully !", "Success", { timeOut: 3000 });
            }
            else {
                toastr.error("An error occured while uploading images, please try again later", "Error", { timeOut: 3000 });
            }
        },
        error: function (jqXHR) {
            ajaxindicatorstop();
            toastr.error("An error occured while uploading images, please try again later", "Error", { timeOut: 3000 });
        }
    });
}