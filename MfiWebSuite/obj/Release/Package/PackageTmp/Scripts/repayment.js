
$(function () {

    $("#uxRepBody input:text").live('focus', function () {
        $(this).select();
    });

    $('#modalRep').modal({
        keyboard: false,
        //backdrop: 'static',
        show: false
    });

    $("#btnSaveRep").live('click', function () {

        $("#locNotHO").html("");
        $("[ID^='TR_']").css('background-color', '#FFFFFF');

        var errors = fnVal();

        if (errors.length == 0) {

            $("#modalRepNot").html("<p>Are you sure you want to save the details?</p> <p>Please check the data before confirming.</p>"); //remove modal msg
            $("#btnRepOk").removeClass("disabled").show();
            $("#btnCancelRep").removeClass("disabled").text("Cancel").show();

            $("#modalRep").modal('show');
        }
        else {
            $("#locNotHO").append("<div id='frmAlert' class='alert alert-block alert-error'></div>");
            $("#frmAlert").append("<a class='close' data-dismiss='alert' href='#'>×</a>");
            $("#frmAlert").append("<h4 class='alert-heading'>Please enter valid number in total collection field. Rows with error are highlighted in red.</h4><br/>");

            for (var i = 0; i < errors.length; i++) {
                var RowID = errors[i];
                $("#TR_" + RowID).css('background-color', '#FF5454');
            }

            $("html, body").animate({ scrollTop: 0 }, "fast");
        }
    });

    $("#btnOkRep").click(function () {
        if (!$(this).hasClass('disabled')) {
            fnRep();
        }
    });

    $("#btnCancelRep").click(function () {
        if (!$(this).hasClass('disabled')) {

            if ($(this).text() == "Ok") {
                //location.reload();
                //window.location = "/pending-loans";

            }
            else {
                $('#modalRep').modal('hide');
            }
        }
    });



    $("#drpCenter").change(function () {
        var CenterID = $("#drpCenter option:selected").val();
        $('#uxMeetingDate').html('');

        if (CenterID != "0") {
            $('#uxMeetingDate').attr('disabled', 'disabled');
            fnGetMeetingDate(CenterID);
        }
    });

    $("#uxCollectionDate").datepicker();
    $("#uxCollectionDate").datepicker("option", "dateFormat", "d M y");
    $("#uxCollectionDate").datepicker('setDate', new Date());

    $("#drpCenter option:eq(0)").attr('selected', true);

    $("#btnShowRepaymentForm").click(function () {
        if (!$(this).hasClass('disabled')) {

            $('#uxRepBody').html("");

            var collecDt = $("#uxCollectionDate").datepicker("getDate");
            var repDt = $("#uxMeetingDate option:selected").val();

            if (repDt) {

                if (Date.parse(collecDt) >= Date.parse(repDt)) {
                    var CenterID = $("#drpCenter option:selected").val();

                    $(this).addClass('disabled');
                    fnGetRepData(CenterID, repDt);

                }

                else {
                    alert("Collection date must be greater than or equal to planned meeting date");
                }

            }
            else {
                alert("Please select a meeting date");
            }
        }
    });

});          //doc ready

function fnGetRepData(centerID, plannedDate) {
    $.ajax({
        type: "POST",
        url: "/Ajax/loan.aspx",
        data: "AjaxMethod=GetRepaymentData" +
                "&CenterID=" + centerID +
                "&PlannedDate=" + plannedDate,
        success: function (returnResponse) {
            var element = jQuery(returnResponse).find("#hdnLUK");

            if (element.length == 1) {
                window.location = "/login?rr=se";
            }
            else {
                $('#btnShowRepaymentForm').removeClass('disabled');
                if (returnResponse != 'Error_Default')
                    $('#uxRepBody').html(returnResponse);
            }
        },
        error: function (xhr) {
            $('#btnShowRepaymentForm').removeClass('disabled');
        }
    });
}

function fnGetMeetingDate(centerID) {
    $.ajax({
        type: "POST",
        url: "/Ajax/loan.aspx",
        data: "AjaxMethod=GetMeetingDatesForCenter" +
                "&CenterID=" + centerID,
        success: function (returnResponse) {
            var element = jQuery(returnResponse).find("#hdnLUK");

            if (element.length == 1) {
                window.location = "/login?rr=se";
            }
            else {
                $('#uxMeetingDate').removeAttr('disabled');
                if (returnResponse != 'Error_Default')
                    $('#uxMeetingDate').html(returnResponse);

            }
        },
        error: function (xhr) {
            $('#uxMeetingDate').removeAttr('disabled');
        }
    });
}

function fnVal() {

    var TotalCollecOk = 0;
    var errorMsgs = new Array();

    $("[ID^='TD_']").each(function () {

        var DueID = $(this).attr("id").split('_')[1];

        var TotalDueAmnt = $("#TD_" + DueID).val();
        //                alert(TotalDueAmnt);
        if (isNumber(TotalDueAmnt) && parseFloat(TotalDueAmnt) > -1) {
            TotalCollecOk++;
        }
        else {
            errorMsgs.push(DueID);
        }
    });

    return errorMsgs;
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


function fnRep() {

    //ui 
    $("#modalRepNot").html("Saving...&nbsp;<img src='/Images/Resources/ajax-loader.gif' />");
    $("#btnOkRep").addClass("disabled");
    $("#btnCancelRep").addClass("disabled");

    var RepData = {
        repayment: [] //don't change the name. coupled with class
    };

    $("[ID^='TD_']").each(function () {

        var DueID = $(this).attr("id").split('_')[1];
        var TotalDueAmnt = $("#TD_" + DueID).val();
        var Receipt = $("#RNO_" + DueID).val();

        RepData.repayment.push({
            "LoanInstID": DueID,
            "CollectedAmnt": TotalDueAmnt,
            "ReceiptNo": Receipt
        });
    });

    var collectionDate = $("#uxCollectionDate").datepicker("getDate");
    collectionDate = collectionDate.getFullYear() + "-" + (collectionDate.getMonth() + 1) + "-" + collectionDate.getDate();

    $.ajax({
        type: "POST",
        url: "/Ajax/loan.aspx",
        data: "AjaxMethod=repayment" +
                "&CollecDate=" + collectionDate +
                "&RepData=" + JSON.stringify(RepData),

        success: function (returnResponse) {

            var element = jQuery(returnResponse).find("#hdnLUK");

            if (element.length == 1) {
                window.location = "/login?rr=se";
            }
            else {

                var responseStatus = returnResponse.split('_')[0];
                var responseMsg = returnResponse.split('_')[1];
                var errorID = returnResponse.split('_')[2];

                if (responseStatus == "Error") {

                    if (responseMsg == "Default") {

                        $("#modalRepNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                        $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");
                    }
                    else if (responseMsg == "Process") {
                        $("#modalRepNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                        $("#frmAlert").append("Sorry, encountered an error while processing. Please contact system administrator with the Error Code: #0" + errorID);
                    }

                    $("#btnOkRep").removeClass("disabled");
                    $("#btnCancelRep").removeClass("disabled");
                }
                else if (responseStatus == "success") {

                    $('#uxRepBody').html('');
                    $("#btnOkRep").removeClass("disabled");
                    $("#btnCancelRep").removeClass("disabled");

                    $("#uxMeetingDate option:eq(0)").remove();
                    if ($("#uxMeetingDate option").length > 0) {
                        $("#uxMeetingDate option:eq(0)").removeAttr('disabled').css('color', '#333');
                    }

                    $("#locNotHO").html("<div class='alert alert-block alert-success'><strong>Success!</strong> Repayment form saved.</div>");
                    $('#modalRep').modal('hide');
                    $("html, body").animate({ scrollTop: 0 }, "fast");
                }

            }
        }, //success
        error: function (xhr) {
            $("#modalDisbNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
            $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");

            $("#btnOkRep").removeClass("disabled");
            $("#btnCancelRep").removeClass("disabled");
        }
    });      //ajax
}