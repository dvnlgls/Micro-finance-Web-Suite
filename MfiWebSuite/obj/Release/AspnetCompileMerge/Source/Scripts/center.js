
$(function () {
    $(".cmGrpStat").tooltip();

    $("#btnEdit").click(function () {
        if (!$(this).hasClass('disabled')) {

            $("#hdnCL").val($("#ContentPlaceHolder1_uxMeetingLoc").val());
            $("#hdnCT").val($("#ContentPlaceHolder1_uxMeetingTime").val());
            $("#hdnFE").val($("#drpFE option:selected").val());

            $(".cmEditable").removeAttr("disabled");
            $(this).hide();
            $("#btnSave, #btnCancel").show();
        }
    }); //btn edit

    $("#btnSave").click(function () {
        if (!$(this).hasClass('disabled')) {

            $("#locNotHO").html("");

            if (fnVal().length == 0) {
                $(".cmEditable").attr("disabled", "");
                $("[id^='btn']").addClass("disabled");
                fnUpdateCenter();
            }
        }
    }); //btn

    $("#btnCancel").click(function () {
        if (!$(this).hasClass('disabled')) {
            fnResetUI();

            $("#ContentPlaceHolder1_uxMeetingLoc").val($("#hdnCL").val());
            $("#ContentPlaceHolder1_uxMeetingTime").val($("#hdnCT").val());
            $("#drpFE").val($("#hdnFE").val());

            $("#locNotHO").html("");
        }

    });

});

function fnResetUI() {
    $(".cmEditable").attr("disabled", "disabled");

    $("[id^='btn']").removeClass("disabled");
    $("#btnSave, #btnCancel").hide();
    $("#btnEdit").show();
    //$("html, body").animate({ scrollTop: 0 }, "fast");
}

function fnVal() {

    var locationOk = false;
    var timeOK = false;
    //var feOk = false;

    var errorMsgs = new Array();

    if ($("#ContentPlaceHolder1_uxMeetingLoc").val().trim() != "") {
        locationOk = true;
    }

    if ($("#ContentPlaceHolder1_uxMeetingTime").val().trim() != "") {
        timeOK = true;
    }

    //            if ($("#drpFE option:selected").val() != "0") {
    //                feOk = true;
    //            }

    //consolidate errors
    if (!locationOk) {
        errorMsgs.push("Please enter a Meeting Location");
    }
    if (!timeOK) {
        errorMsgs.push("Please enter a Meeting Time");
    }
    //            if (!feOk) {
    //                errorMsgs.push("Please select a Field Executive");
    //            }

    return errorMsgs;
}

function fnUpdateCenter() {

    var meetingLoc = $("#ContentPlaceHolder1_uxMeetingLoc").val();
    var meetingTime = $("#ContentPlaceHolder1_uxMeetingTime").val();
    var feID = $("#drpFE option:selected").val();
    var centerID = $("#ContentPlaceHolder1_hdnOid").val();

    $.ajax({
        type: "POST",
        url: "/Ajax/offices.aspx",
        data: "AjaxMethod=EditCenter" +
                "&CenterID=" + centerID +
                "&Location=" + meetingLoc +
                "&Time=" + meetingTime +
                "&FE=" + feID,

        success: function (returnResponse) {
            var element = jQuery(returnResponse).find("#hdnLUK");

            if (element.length == 1) {
                window.location = "/login?rr=se";
            }
            else {
                var responseStatus = returnResponse.split('_')[0];
                var responseMsg = returnResponse.split('_')[1];

                if (responseStatus == "Success") {

                    //reset ui
                    fnResetUI();

                    //disp notif
                    $("#locNotHO").html("<div class='alert alert-success'><button class='close' data-dismiss='alert' type='button'>×</button><strong>Success!</strong> Details have been edited.</div>");

                }
                else if (responseStatus == "Error") {
                    //ui
                    $(".cmEditable").removeAttr("disabled");
                    $("[id^='btn']").removeClass("disabled");
                    $("#locNotHO").html("<div class='alert alert-error'><button class='close' data-dismiss='alert' type='button'>×</button>Sorry, encountered an unexpected error. Please try again.</div>");

                }
                $("html, body").animate({ scrollTop: 0 }, "fast");
            }
        }
    }); //ajax
}
