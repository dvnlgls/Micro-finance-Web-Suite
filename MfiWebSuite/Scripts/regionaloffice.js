
$(function () {

    $("#form1").validate({
        ignore: "",
        rules: {
            txtName: {
                required: true,
                maxlength: 50
            },
            txtAddr: {
                maxlength: 500
            },
            txtPh1: {
                digits: true,
                maxlength: 20
            },
            txtPh2: {

                digits: true,
                maxlength: 20
            },
            txtFax: {

                digits: true,
                maxlength: 20
            },
            txtMail: {

                email: true,
                maxlength: 50
            }

        }, //rules

        errorPlacement: function (error, element) {
            if (element.attr("name") == "txtName")
                error.appendTo("#errName");

            if (element.attr("name") == "txtAddr")
                error.appendTo("#errAddr");

            if (element.attr("name") == "txtPh1")
                error.appendTo("#errPh1");

            if (element.attr("name") == "txtPh2")
                error.appendTo("#errPh2");

            if (element.attr("name") == "txtFax")
                error.appendTo("#errFax");

            if (element.attr("name") == "txtMail")
                error.appendTo("#errMail");
        },
        submitHandler: function (form) {

        },

        invalidHandler: function (form, validator) {

        }
    }); //validate()

    $("#btnEdit").click(function () {
        if (!$(this).hasClass('disabled')) {

            $(".orig").each(function () {
                var origTxt = $(this).text();
                var nextID = $(this).next().attr('id');
                $("#" + nextID).val(origTxt);
            });

            $(".orig").hide();
            $(".edit").show();
            $(this).hide();
            $("#btnSave, #btnCancel").show();
        }
    });

    $("#btnCancel").click(function () {
        if (!$(this).hasClass('disabled')) {
            fnResetUI();
            $("#locNotHO").html("");
        }

    });

    $("#btnSave:not(.disabled)").click(function () {
        if (!$(this).hasClass('disabled')) {

            $("#locNotHO").html("");

            if (fnVal()) {
                $(".edit").addClass("disabled").attr("disabled", "");
                $("[id^='btn']").addClass("disabled");
                fnUpdateOffice();
            }
        }
    });


});         //doc ready

function fnVal() {

    if ($("#form1").valid()) {
        return true;
    }
    else {
        return false;
    }
}

function fnResetUI() {
    $(".edit").removeClass("disabled").removeAttr("disabled").hide();
    $(".orig").show();
    $("[id^='btn']").removeClass("disabled");
    $("#btnSave, #btnCancel").hide();
    $("#btnEdit").show();
    $("html, body").animate({ scrollTop: 0 }, "fast");
}

function fnUpdateOffice() {

    $.ajax({
        type: "POST",
        url: "/Ajax/offices.aspx",
        data: "AjaxMethod=SaveOffDetails" +
                    "&OID=" + $("#ContentPlaceHolder1_hdnOid").val() +
                    "&Name=" + $("#txtName").val() +
                    "&Addr=" + $("#txtAddr").val() +
                    "&Ph1=" + $("#txtPh1").val() +
                    "&Ph2=" + $("#txtPh2").val() +
                    "&Fax=" + $("#txtFax").val() +
                    "&Web=" +
                    "&Email=" + $("#txtMail").val(),

        success: function (returnResponse) {
            var responseStatus = returnResponse.split('_')[0];
            var responseMsg = returnResponse.split('_')[1];

            if (responseStatus == "Success") {

                //change values
                $(".edit").each(function () {
                    var origTxt = $(this).val();
                    var prevID = $(this).prev().attr('id');
                    $("#" + prevID).text(origTxt);
                });

                //reset ui
                fnResetUI();

                //disp notif
                $("#locNotHO").html("<div class='alert alert-success'><button class='close' data-dismiss='alert' type='button'>×</button><strong>Success!</strong> Details have been edited.</div>");

            }
            if (responseStatus == "Error") {

                //ui
                $("button[id^='btn']").removeClass("disabled");
                $(".edit").removeClass("disabled").removeAttr("disabled");

                if (responseMsg == "Default") {
                    $("#locNotHO").html("<div class='alert alert-error'><button class='close' data-dismiss='alert' type='button'>×</button>Sorry, encountered an unexpected error. Please try again.</div>");
                    $("html, body").animate({ scrollTop: 0 }, "fast");
                }
                else if (responseMsg == "SessionExpired") {
                    window.location = "/login?rr=se";
                }
            }
        }
    });         //ajax
}
