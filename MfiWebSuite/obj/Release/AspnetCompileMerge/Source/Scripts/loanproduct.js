﻿
$(function () {

    $("#modalDel").modal({
        keyboard: false,
        backdrop: 'static',
        show: false
    });

    $("[ID^='btnDel_']").click(function () {

        $("#hdnLPID").val("");
        $("#hdnLPID").val($(this).attr("id").split('_')[1]);

        $("#spnDelTxt").text("Do you really want to delete this Loan Product? You won't be able to delete a product when it is linked to active Loans");
        $("#btnDelYes").removeClass("disabled").show();
        $("#btnDelNo").removeClass("disabled").show().text("No");

        $('#modalDel').modal('show');
    }); //del

    $("#btnDelYes").click(function () {
        if (!$(this).hasClass('disabled')) {

            $("#btnDelYes").addClass("disabled");
            $("#btnDelNo").addClass("disabled");

            $.ajax({
                type: "POST",
                url: "/Ajax/loan.aspx",
                data: "AjaxMethod=DeleteLP" +
                "&LPID=" + $("#hdnLPID").val(),
                success: function (returnResponse) {

                    var element = jQuery(returnResponse).find("#hdnLUK");

                    if (element.length == 1) {
                        window.location = "/login?rr=se";
                    }
                    else {

                        var responseStatus = returnResponse.split('_')[0];
                        var responseMsg = returnResponse.split('_')[1];

                        if (responseStatus == "Error") {
                            $("#spnDelTxt").text("Sorry, encountered an error. Please try again.");
                            $("#btnDelYes").removeClass("disabled");
                            $("#btnDelNo").removeClass("disabled");
                        }
                        else if (responseStatus == "LinkedToLoan") {
                            $("#spnDelTxt").text("Sorry, you can't delete this product as it is being used by active loans.");
                            $("#btnDelYes").removeClass("disabled").hide();
                            $("#btnDelNo").removeClass("disabled").text("Close");
                        }

                        else if (responseStatus == "Success") {
                            $("#spnDelTxt").text("Successfully Deleted");
                            $("#btnDelNo").removeClass("disabled").text("Ok");

                        }
                    }
                }
            }); //ajax
        }
    });

    $("#btnDelNo").click(function () {
        if (!$(this).hasClass('disabled')) {

            if ($(this).text() == "Ok") {
                //location.reload();
                $("#uxGroupTbl #LP_" + $("#hdnLPID").val()).remove();
                $("#hdnGID").val("");
                $('#modalDel').modal('hide');
            }
            else {
                $('#modalDel').modal('hide');
            }
        }
    });

    $("#modalAddProduct").modal({
        keyboard: false,
        backdrop: 'static',
        show: false
    });

    $('#modalAddProduct').on('shown', function () {
        $("#uxProductName").focus();
    });

    $("#btnAddProduct").click(function () {

        $("#modalNot").html("");

        $("#divAddOfc input[type='text']").val("");

        $("#btnAddOK").removeClass("disabled").show();
        $("#btnCancelAddProduct").removeClass("disabled").text("Cancel").show();

        $('#modalAddProduct').modal('show');

    });

    $("#btnAddOK").click(function () {
        if (!$(this).hasClass('disabled')) {

            $("#modalNot").html("");

            var errors = fnVal();

            if (errors.length == 0) {

                $("#btnAddOK").addClass("disabled");
                $("#btnCancelAddProduct").addClass("disabled");

                var name = $("#uxProductName").val();
                var amount = $("#uxMaxAmnt").val();
                var interest = $("#uxIntRate").val();
                var tenure = $("#uxTenure").val();
                var fs = $("#drpFS option:selected").val();
                var repType = $('input[name=uxReptype]:checked').val();
                var repDays = $("#uxRepDays").val();
                var range = new Array();

                $("[ID^='LcRange_']").each(function () {
                    var rangeID = $(this).attr("id").split('_')[1];

                    var curMin = $("#uxLcMin_" + rangeID).val();
                    var curMax = $("#uxLcMax_" + rangeID).val();

                    if (curMin != "" && curMax != "") {
                        range.push(rangeID + "_" + curMin + "_" + curMax);
                    }
                });

                range = range.join('~');

                $.ajax({
                    type: "POST",
                    url: "/Ajax/loan.aspx",
                    data: "AjaxMethod=AddLP" +
                        "&Name=" + name +
                        "&Amount=" + amount +
                        "&Interest=" + interest +
                        "&Tenure=" + tenure +
                        "&FS=" + fs +
                        "&RepType=" + repType +
                        "&RepDays=" + repDays +
                        "&CycleRange=" + range,

                    success: function (returnResponse) {

                        var element = jQuery(returnResponse).find("#hdnLUK");

                        if (element.length == 1) {
                            window.location = "/login?rr=se";
                        }
                        else {

                            var responseStatus = returnResponse.split('_')[0];
                            var responseMsg = returnResponse.split('_')[1];

                            if (responseStatus == "Error") {
                                $("#spnDelTxt").text("Sorry, encountered an error. Please try again.");
                                $("#btnDelYes").removeClass("disabled");
                                $("#btnDelNo").removeClass("disabled");
                            }

                            if (responseStatus == "Success") {
                                $("#modalNot").append("<div id='frmAlert' class='alert alert-block alert-success'></div>");
                                $("#frmAlert").append("<strong>Success!</strong> Loan Product has been created.");
                                $(".modal-body, #modalNot").animate({ scrollTop: 0 }, "fast");

                                $("#btnCancelAddProduct").removeClass("disabled").text("Ok");
                            }
                        }
                    }
                }); //ajax
            }
            else {
                $("#modalNot").append("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                $("#frmAlert").append("<h4 class='alert-heading'>Please correct the below mistake(s) to continue:</h4><br/>");

                for (var i = 0; i < errors.length; i++) {
                    $("#frmAlert").append("<h5><i class='icon-hand-right'></i>&nbsp; " + errors[i] + "</h5>");
                }

                $(".modal-body, #modalNot").animate({ scrollTop: 0 }, "fast");
            }
        }
    });

    $("#btnCancelAddProduct").click(function () {
        if (!$(this).hasClass('disabled')) {

            if ($(this).text() == "Ok") {
                location.reload();
            }
            else {
                $('#modalAddProduct').modal('hide');
            }
        }
    });


});             //doc ready

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function isInt(value) {
    return !isNaN(parseInt(value)) && (parseFloat(value) == parseInt(value));
}


function fnVal() {
    var nameOk = false;
    var nameUniqueOK = false;
    var amountOk = false;
    var intOK = false;
    var tenureOK = false;
    var FSOK = false;
    var RepTypeOk = false;
    var RepDaysOk = false;

    var rangeOK = true;

    var errorMsgs = new Array();

    var name = $("#uxProductName").val();
    var amount = $("#uxMaxAmnt").val();
    var interest = $("#uxIntRate").val();
    var tenure = $("#uxTenure").val();
    var fs = $("#drpFS option:selected").val();
    var repType = $('input[name=uxReptype]:checked').val();
    var repDays = $("#uxRepDays").val();


    if (name != "")
        nameOk = true;

    $("tr[ID^='LP_']").each(function () {
        var LPID = $(this).attr("id");

        var lpName = $("#" + LPID + " td:eq(0)").html();

        if (lpName == name) {
            nameUniqueOK = false;
            return false;
        }
        else {
            nameUniqueOK = true;
        }
    });

    if (!nameUniqueOK)
        errorMsgs.push("The loan product name already exists. Please enter a different name.");

    if (isNumber(amount))
        amountOk = true;

    if (isNumber(interest))
        intOK = true;

    if (isInt(tenure))
        tenureOK = true;

    if (fs != "0")
        FSOK = true;

    if (repType == "1" || repType == "2")
        RepTypeOk = true;

    if (isInt(repDays) && (parseInt(repDays) > 0))
        RepDaysOk = true;


    $("[ID^='LcRange_']").each(function () {
        var rangeID = $(this).attr("id").split('_')[1];

        var curMin = $("#uxLcMin_" + rangeID).val();
        var curMax = $("#uxLcMax_" + rangeID).val();

        if (curMin != "" && curMax != "") {

            if (isNumber(curMin) && parseFloat(curMin) > 0 && parseFloat(curMin) <= parseFloat(amount) && isNumber(curMax) && parseFloat(curMax) > 0 && parseFloat(curMax) <= parseFloat(amount) && parseFloat(curMin) < parseFloat(curMax)) {
                rangeOK = true;
            }
            else {
                rangeOK = false;
                return false;
            }

        }

    });


    //gather err msgs
    if (!nameOk)
        errorMsgs.push("Product name cannot be empty");

    if (!amountOk)
        errorMsgs.push("Please enter a valid max. amount");

    if (!intOK)
        errorMsgs.push("Please enter a valid interest");

    if (!tenureOK)
        errorMsgs.push("Please enter a valid tenure.");

    if (!FSOK)
        errorMsgs.push("Please select a Fund Source");

    if (!RepTypeOk)
        errorMsgs.push("Please select a repayment type");

    if (repType == "2" && (!RepDaysOk))
        errorMsgs.push("Please enter a valid integer for days");

    if (!rangeOK)
        errorMsgs.push("Check your Loan Cycle range amount values.");

    return errorMsgs;
}
