
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function fnVal() {

    var disbAmntOK = 0; //check if valid number
    var disbAmntAllowed = 0; //check if within limit
    var disbDateCtr = 0;

    var repDateCtr = 0;
    var repDateOK = 0;

    var emiCtr = 0;
    var emiOK = 0;

    var errorMsgs = new Array();


    $("[ID^='LN_']").each(function () {
        var lnID = $(this).attr("id");

        var disbAmnt = $("#" + lnID + " td .cmDisbAmnt").val();

        //disb amount
        if (isNumber(disbAmnt) && parseFloat(disbAmnt) > 0) {
            disbAmntOK++;
        }

        //amount within limits
        if (parseInt(disbAmnt) <= parseInt($("#ContentPlaceHolder1_uxLpamnt").html())) {
            disbAmntAllowed++;
        }

        //get disb date count
        if ($("#" + lnID + " .cmDisbDate").datepicker("getDate") != null) {
            disbDateCtr++;
        }

        //get repayment date count
        if ($("#" + lnID + " .cmFRDate").datepicker("getDate") != null) {
            repDateCtr++;

            //check if the date is greater than disb date
            var disbDt = $("#" + lnID + " .cmDisbDate").datepicker("getDate");
            var repDt = $("#" + lnID + " .cmFRDate").datepicker("getDate");

            if (repDt > disbDt) {
                repDateOK++;
            }
        }

        //emi checks
        var emi = $("#" + lnID + " .cmEMI").val();

        if (emi != "") {
            emiCtr++;

            if (isNumber(emi) && (parseFloat(emi) > 0) && (parseFloat(emi) < disbAmnt)) {
                emiOK++;
            }
        }
    });     //tr ln each

    var cliCtr = $("[ID^='LN_']").length;


    if (cliCtr != disbAmntOK) {
        errorMsgs.push("Please enter a valid disbursement amount.");
    }

    if (cliCtr != disbAmntAllowed) {
        errorMsgs.push("Disbursement amount cannot be more than the amount set in Loan Product. Reduce the disbursement amount or increase the value in Loan Product");
    }

    if (cliCtr != disbDateCtr) {
        errorMsgs.push("Disbursement date cannot be empty");
    }

    if (repDateCtr > 0 && repDateCtr != repDateOK) {
        errorMsgs.push("First Repayment date, if present, must be greater than disbursement date");
    }

    if (emiCtr > 0 && emiCtr != emiOK) {
        errorMsgs.push("Please check your EMI value. Only valid numbers are allowed and they cannot be more than disbursement amount.");
    }

    return errorMsgs;


} //fnval


function fnValByID(lnID) {

    var disbAmntOK = 0; //check if valid number
    var disbAmntAllowed = 0; //check if within limit
    var disbDateCtr = 0;

    var repDateCtr = 0;
    var repDateOK = 0;

    var emiCtr = 0;
    var emiOK = 0;

    var errorMsgs = new Array();

    var disbAmnt = $("#" + lnID + " td .cmDisbAmnt").val();

    //disb amount
    if (isNumber(disbAmnt) && parseFloat(disbAmnt) > 0) {
        disbAmntOK++;
    }

    //amount within limits
    if (parseInt(disbAmnt) <= parseInt($("#ContentPlaceHolder1_uxLpamnt").html())) {
        disbAmntAllowed++;
    }

    //get disb date count
    if ($("#" + lnID + " .cmDisbDate").datepicker("getDate") != null) {
        disbDateCtr++;
    }

    //get repayment date count
    if ($("#" + lnID + " .cmFRDate").datepicker("getDate") != null) {
        repDateCtr++;

        //check if the date is greater than disb date
        var disbDt = $("#" + lnID + " .cmDisbDate").datepicker("getDate");
        var repDt = $("#" + lnID + " .cmFRDate").datepicker("getDate");

        if (repDt > disbDt) {
            repDateOK++;
        }
    }

    //emi checks
    var emi = $("#" + lnID + " .cmEMI").val();

    if (emi != "") {
        emiCtr++;

        if (isNumber(emi) && (parseFloat(emi) > 0) && (parseFloat(emi) < disbAmnt)) {
            emiOK++;
        }
    }


    var cliCtr = 1; // $("[ID^='LN_']").length;


    if (cliCtr != disbAmntOK) {
        errorMsgs.push("Please enter a valid disbursement amount.");
    }

    if (cliCtr != disbAmntAllowed) {
        errorMsgs.push("Disbursement amount cannot be more than the amount set in Loan Product. Reduce the disbursement amount or increase the value in Loan Product");
    }

    if (cliCtr != disbDateCtr) {
        errorMsgs.push("Disbursement date cannot be empty");
    }

    if (repDateCtr > 0 && repDateCtr != repDateOK) {
        errorMsgs.push("First Repayment date, if present, must be greater than disbursement date");
    }

    if (emiCtr > 0 && emiCtr != emiOK) {
        errorMsgs.push("Please check your EMI value. Only valid numbers are allowed and they cannot be more than disbursement amount.");
    }

    return errorMsgs;


} //fnval individual

function fnEmiFunctionalVal(lnID) {

    var errInvalidEMI = 0;

    var prinOut = $("#LN_" + lnID + " td .cmDisbAmnt").val();
    var intRate = $("#ContentPlaceHolder1_uxLpInt").html() / 100;
    var tenure = parseInt($("#ContentPlaceHolder1_uxLpTenure").html());

    var CollectionTypeID = parseInt($("#ContentPlaceHolder1_hdnCFTID").val());
    var CollectionFreq = parseInt($("#ContentPlaceHolder1_hdnCFV").val());

    var totalInstallments;

    if (CollectionTypeID == 1) {
        totalInstallments = tenure;
    }
    else if (CollectionTypeID == 2) {
        totalInstallments = Math.round(((tenure / 12) * 365) / CollectionFreq);
    }

    var startDate = $("#LN_" + lnID + " .cmDisbDate").datepicker("getDate");
    var endDate;
    var emi;

    //get first disbursement date, if present
    if ($("#LN_" + lnID + " .cmFRDate").datepicker("getDate") != null) {
        endDate = $("#LN_" + lnID + " .cmFRDate").datepicker("getDate");

        if (endDate.getDay() == 0) {
            endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() - 1);
        }
    }
    else {
        if (CollectionTypeID == 1) {
            endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());
        }
        else if (CollectionTypeID == 2) {
            endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + CollectionFreq);
        }

        if (endDate.getDay() == 0) {
            endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() - 1);
        }
    }

    if ($("#LN_" + lnID + " .cmEMI").val() != "") {
        emi = $("#LN_" + lnID + " .cmEMI").val();
    }
    else {
        emi = Math.round(prinOut * (1 + (intRate * 0.555)) / totalInstallments);
    }

    for (var ctr = 1; ctr <= totalInstallments; ctr++) {

        var daysDifference = Math.floor((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24));
        var interest = Math.round((prinOut * intRate * daysDifference) / 365);
        var principalComponent = emi - interest;

        prinOut = prinOut - principalComponent;

        if (ctr == totalInstallments) {
            emi = parseFloat(emi) + parseFloat(prinOut);
            principalComponent = parseFloat(emi) - parseFloat(interest);
            prinOut = 0;
        }
        if ((parseFloat(principalComponent) < 0) || (parseFloat(interest) < 0)) {
            errInvalidEMI++;
        }
        startDate = endDate;

        if (CollectionTypeID == 1) {
            endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());
        }
        else if (CollectionTypeID == 2) {
            endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + CollectionFreq);
        }
        //endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());

        if (endDate.getDay() == 0) {
            endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() - 1);
        }
    }

    if (errInvalidEMI > 0) {
        return false;
    }
    else
        return true;

} //fn emi func val

function fnDisb() {

    //ui 
    $("#modalDisbNot").html("Disbursing...&nbsp;<img src='/Images/Resources/ajax-loader.gif' />");
    $("#btnDisbOk").addClass("disabled");
    $("#btnCancelDisb").addClass("disabled");

    var LoanData = {
        loan: [] //don't change the name. coupled with class
    };

    //common values
    var intRate = $("#ContentPlaceHolder1_uxLpInt").html();

    var tenure = parseInt($("#ContentPlaceHolder1_uxLpTenure").html());

    var CollectionTypeID = parseInt($("#ContentPlaceHolder1_hdnCFTID").val());
    var CollectionFreq = parseInt($("#ContentPlaceHolder1_hdnCFV").val());

    var totalInstallments;

    if (CollectionTypeID == 1) {
        totalInstallments = tenure;
    }
    else if (CollectionTypeID == 2) {
        totalInstallments = Math.round(((tenure / 12) * 365) / CollectionFreq);
    }

    $("[ID^='LN_']").each(function () {

        var lnID = $(this).attr("id").split('_')[1];
        var disbAmnt = $("#LN_" + lnID + " td .cmDisbAmnt").val();

        var disbDate = $("#LN_" + lnID + " .cmDisbDate").datepicker("getDate");
        disbDate = disbDate.getFullYear() + "-" + (disbDate.getMonth() + 1) + "-" + disbDate.getDate();

        var frDate = "";

        if ($("#LN_" + lnID + " .cmFRDate").datepicker("getDate") != null) {
            frDate = $("#LN_" + lnID + " .cmFRDate").datepicker("getDate");
            frDate = frDate.getFullYear() + "-" + (frDate.getMonth() + 1) + "-" + frDate.getDate();
        }

        var emi = "";
        if ($("#LN_" + lnID + " .cmEMI").val() != "") {
            emi = $("#LN_" + lnID + " .cmEMI").val();
        }


        //put values in json

        LoanData.loan.push({
            "LoanID": lnID,
            "Amnt": disbAmnt,
            "DisbDate": disbDate,
            "FrDate": frDate,
            "Emi": emi
        });

    }); //each loan row

    //ajax
    //alert(JSON.stringify(LoanData));
    $.ajax({
        type: "POST",
        url: "/Ajax/loan.aspx",
        data: "AjaxMethod=disburse" +
                "&GID=" + $("#ContentPlaceHolder1_hdnGID").val() +
                "&Int=" + intRate +
                "&Inst=" + totalInstallments +
                "&LoanData=" + JSON.stringify(LoanData),

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

                        $("#modalDisbNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                        $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");
                    }
                    else if (responseMsg == "Process") {
                        $("#modalDisbNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                        $("#frmAlert").append("Sorry, encountered an error while processing. Please contact system administrator with the Error Code: #0" + errorID);
                    }

                    $("#btnDisbOk").removeClass("disabled");
                    $("#btnCancelDisb").removeClass("disabled");
                }
                else if (responseStatus == "Success") {
                    $("#divBtnSec").remove();
                    $(".ldisb-amnt").attr("disabled", "disabled");
                    $(".gloEMI").attr("disabled", "disabled");
                    $(".cmEMI").attr("disabled", "disabled");

                    $("#locNotHO").html("<div class='alert alert-block alert-success'><strong>Success!</strong> Disbursement has been completed.</div>");


                    $('#modalDisb').modal('hide');
                    $("html, body").animate({ scrollTop: 0 }, "fast");
                }

            }
        }, //success
        error: function (xhr) {
            $("#modalDisbNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
            $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");

            $("#btnDisbOk").removeClass("disabled");
            $("#btnCancelDisb").removeClass("disabled");
        }
    });   //ajax

} //fn disb

$(function () {

    $('#modalDisb').modal({
        keyboard: false,
        backdrop: 'static',
        show: false
    });

    $('#modalDel').modal({
        keyboard: false,
        backdrop: 'static',
        show: false
    });

    //reject loan
    $("#btnRejectLoan").click(function () {
        $("#spnDelTxt").text("Do you really want to reject this loan? This will also delete the group.");
        $("#btnDelYes").removeClass("disabled").show();
        $("#btnDelNo").removeClass("disabled").show().text("No");

        $('#modalDel').modal('show');
    });


    $("#btnDelYes").click(function () {
        if (!$(this).hasClass('disabled')) {

            $("#btnDelYes").addClass("disabled");
            $("#btnDelNo").addClass("disabled");

            $.ajax({
                type: "POST",
                url: "/Ajax/loan.aspx",
                data: "AjaxMethod=RejectLoan" +
                "&GID=" + $("#ContentPlaceHolder1_hdnGID").val(),
                success: function (returnResponse) {

                    var element = jQuery(returnResponse).find("#hdnLUK");

                    if (element.length == 1) {
                        window.location = "/login?rr=se";
                    }
                    else {

                        if (returnResponse == "Error") {
                            $("#spnDelTxt").text("Sorry, encountered an error. Please try again.");
                            $("#btnDelYes").removeClass("disabled");
                            $("#btnDelNo").removeClass("disabled");
                        }

                        else if (returnResponse == "Error_SessionExpired") {
                            window.location = "/login?rr=se";
                        }
                        else {
                            $("#spnDelTxt").text("Loan has been successfully rejected and group removed. You can apply for new loans for these clients.");
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
                $("#divBtnSec").remove();
                window.location = "/pending-loans";
            }
            else {
                $('#modalDel').modal('hide');
            }
        }
    });

    //disburse
    $("#btnDisbOk").click(function () {
        if (!$(this).hasClass('disabled')) {
            fnDisb();
        }
    });

    $("#btnCancelDisb").click(function () {
        if (!$(this).hasClass('disabled')) {

            if ($(this).text() == "Ok") {
                //location.reload();
                //window.location = "/pending-loans";

            }
            else {
                $('#modalDisb').modal('hide');
            }
        }
    });

    $("#btnDisburse").click(function () {
        //ui resets
        $("#locNotHO").html("");

        var errors = fnVal();

        var emiCtr = 0; //total valid emi's

        $("[ID^='LN_']").each(function () {
            var lnID = $(this).attr("id").split('_')[1];

            if (fnEmiFunctionalVal(lnID)) {
                emiCtr++;
            }
            else {
                errors.push("EMI for client " + $("#LN_" + lnID + " td:eq(0) a").html() + " will result in negative principal/interest values. Please correct it.");
            }
        });


        if (errors.length == 0 && ($("[ID^='LN_']").length == emiCtr)) {

            $("#modalDisbNot").html("<p>Are you sure you want to disburse?</p> <p>Please check the details before confirming.</p>"); //remove modal msg
            $("#btnDisbOk").removeClass("disabled").show();
            $("#btnCancelDisb").removeClass("disabled").text("Cancel").show();

            $("#modalDisb").modal('show');
        }
        else {
            $("#locNotHO").append("<div id='frmAlert' class='alert alert-block alert-error'></div>");
            $("#frmAlert").append("<a class='close' data-dismiss='alert' href='#'>×</a>");
            $("#frmAlert").append("<h4 class='alert-heading'>Please correct the below mistake(s) to continue:</h4><br/>");

            for (var i = 0; i < errors.length; i++) {
                $("#frmAlert").append("<h5><i class='icon-hand-right'></i>&nbsp; " + errors[i] + "</h5>");
            }

            $("html, body").animate({ scrollTop: 0 }, "fast");
        }
    });


    $('#modalRPS').modal({
        keyboard: false,
        backdrop: 'static',
        show: false
    });

    $("[ID^='RPS_']").click(function () {

        $(this).attr("disabled", "disabled");
        $("#modalNot").html("");
        $("#uxRPS").hide();
        $("#uxRPS tbody").html("");

        var lnID = $(this).attr("id").split('_')[1];

        var errors = fnValByID("LN_" + lnID);
        var errInvalidEMI = 0;

        if (errors.length == 0) {

            var prinOut = $("#LN_" + lnID + " td .cmDisbAmnt").val();
            var intRate = $("#ContentPlaceHolder1_uxLpInt").html() / 100;

            var tenure = parseInt($("#ContentPlaceHolder1_uxLpTenure").html());

            var CollectionTypeID = parseInt($("#ContentPlaceHolder1_hdnCFTID").val());
            var CollectionFreq = parseInt($("#ContentPlaceHolder1_hdnCFV").val());

            var totalInstallments;

            if (CollectionTypeID == 1) {
                totalInstallments = tenure;
            }
            else if (CollectionTypeID == 2) {
                totalInstallments = Math.round(((tenure / 12) * 365) / CollectionFreq);
            }

            var startDate = $("#LN_" + lnID + " .cmDisbDate").datepicker("getDate");
            var endDate;
            var emi;

            var month = new Array();
            month[0] = "Jan";
            month[1] = "Feb";
            month[2] = "Mar";
            month[3] = "Apr";
            month[4] = "May";
            month[5] = "Jun";
            month[6] = "Jul";
            month[7] = "Aug";
            month[8] = "Sep";
            month[9] = "Oct";
            month[10] = "Nov";
            month[11] = "Dec";

            var weekday = new Array(7);
            weekday[0] = "Sun";
            weekday[1] = "Mon";
            weekday[2] = "Tue";
            weekday[3] = "Wed";
            weekday[4] = "Thu";
            weekday[5] = "Fri";
            weekday[6] = "Sat";

            //get first disbursement date, if present
            if ($("#LN_" + lnID + " .cmFRDate").datepicker("getDate") != null) {

                endDate = $("#LN_" + lnID + " .cmFRDate").datepicker("getDate");

                if (endDate.getDay() == 0) {
                    endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() - 1);
                }
            }
            else {

                if (CollectionTypeID == 1) {
                    endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());
                }
                else if (CollectionTypeID == 2) {
                    endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + CollectionFreq);
                }

                //endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());

                if (endDate.getDay() == 0) {
                    endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() - 1);
                }
            }

            //get emi if present
            if ($("#LN_" + lnID + " .cmEMI").val() != "") {
                emi = $("#LN_" + lnID + " .cmEMI").val();
            }
            else {
                emi = Math.round(prinOut * (1 + (intRate * 0.555)) / totalInstallments);
            }


            for (var ctr = 1; ctr <= totalInstallments; ctr++) {

                var daysDifference = Math.floor((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24));

                var interest = Math.round((prinOut * intRate * daysDifference) / 365);

                var principalComponent = emi - interest;

                prinOut = prinOut - principalComponent;


                if (ctr == totalInstallments) {
                    emi = parseFloat(emi) + parseFloat(prinOut);

                    principalComponent = parseFloat(emi) - parseFloat(interest);
                    prinOut = 0;
                }

                if ((parseFloat(principalComponent) < 0) || (parseFloat(interest) < 0)) {
                    errInvalidEMI++;
                }

                var prettyDate = endDate.getDate() + "-" + month[endDate.getMonth()] + "-" + (endDate.getFullYear()) % 100 + " (" + weekday[endDate.getDay()] + ")";
                $("#uxRPS tbody").append("<tr><td>" + ctr + "</td><td>" + prettyDate + "</td><td>" + principalComponent + "</td><td>" + interest + "</td><td>" + (interest + principalComponent) + "</td></tr>");

                startDate = endDate;
                //endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());
                if (CollectionTypeID == 1) {
                    endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());
                }
                else if (CollectionTypeID == 2) {
                    endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + CollectionFreq);
                }

                if (endDate.getDay() == 0) {
                    endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() - 1);
                }
            }

            if (errInvalidEMI > 0) {
                $("#modalNot").append("<div id='modAlert' class='alert alert-block alert-error'></div>");
                $("#modAlert").append("<h4 class='alert-heading'>Please enter a valid EMI to view schedule. The EMI, if specified, must not result in negative principal and/or interest components.</h4><br/>");
            }
            else {
                $("#uxRPS").show();
            }

        }
        else {
            $("#modalNot").append("<div id='modAlert' class='alert alert-block alert-error'></div>");
            $("#modAlert").append("<h4 class='alert-heading'>Please correct the below mistake(s) to continue:</h4><br/>");

            for (var i = 0; i < errors.length; i++) {
                $("#modAlert").append("<h5><i class='icon-hand-right'></i>&nbsp; " + errors[i] + "</h5>");
            }
        }


        $('#modalRPS').modal('show');
        $(this).removeAttr("disabled");

    });



    $(".GroupLeader").tooltip();

    $("#btnViewRPS").popover({
        placement: "top"
    });

    $(".cmDisbDate").datepicker();
    $(".cmDisbDate").datepicker("option", "dateFormat", "d M y");
    $(".cmDisbDate").datepicker('setDate', new Date());

    $(".cmFRDate").datepicker();
    $(".cmFRDate").datepicker("option", "dateFormat", "d M y");

    $(".gloAmnt").keyup(function () {
        $(".cmDisbAmnt").val($(this).val());
    });

    $(".gloDate").change(function () {
        $(".cmDisbDate").datepicker('setDate', $(".gloDate").datepicker("getDate"));
    });

    $(".gloFrDate").change(function () {
        $(".cmFRDate").datepicker('setDate', $(".gloFrDate").datepicker("getDate"));
    });

    $(".gloEMI").keyup(function () {
        $(".cmEMI").val($(this).val());
    });


});
