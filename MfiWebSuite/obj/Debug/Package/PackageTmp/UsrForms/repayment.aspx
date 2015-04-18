<%@ Page Title="Loan Repayment" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="repayment.aspx.cs" Inherits="MfiWebSuite.UsrForms.repayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script type="text/javascript">
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Loan Repayment Form
            </div>
            <div id="locNotHO">
            </div>
            <div class="grp-filters">
                <table class="table table-bordered rep-tbl">
                    <tbody>
                        <tr>
                            <td class="lbl">
                                Center
                            </td>
                            <td id="uxDrpCenterWrap" runat="server">
                            </td>
                            <td class="lbl">
                                Planned Meeting Date
                            </td>
                            <td>
                                <select class='span2' id="uxMeetingDate">
                                </select>
                            </td>
                            <td class="lbl">
                                Collection Date
                            </td>
                            <td>
                                <input id="uxCollectionDate" readonly='readonly' type='text' class='span2' />
                            </td>
                            <td>
                                <a id="btnShowRepaymentForm" class="btn btn-success pull-left">Show Repayment Form</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="repfrm-tbl" id="uxRepayWrap" runat="server">
                <table class="table table-condensed table-bordered">
                    <thead>
                        <tr>
                            <th rowspan="3">
                                Loan ID
                            </th>
                            <th rowspan="3">
                                Client ID
                            </th>
                            <th rowspan="3">
                                Client Name
                            </th>
                            <th rowspan="3">
                                Loan Product
                            </th>
                            <th rowspan="3">
                                Inst. No
                            </th>
                            <th colspan="4" class="gen-center-txt">
                                Demand
                            </th>
                            <th rowspan="3">
                                Total Due
                            </th>
                            <th colspan="4" class="gen-center-txt">
                                Collection
                            </th>
                            <th rowspan="3">
                                Total Collection
                            </th>
                            <th rowspan="3">
                                Receipt No.
                            </th>
                        </tr>
                        <tr>
                            <th colspan="2" class="gen-center-txt">
                                Over Due
                            </th>
                            <th colspan="2" class="gen-center-txt">
                                Due This Month
                            </th>                            
                            <th colspan="2" class="gen-center-txt">
                                Over Due
                            </th>
                            <th colspan="2" class="gen-center-txt">
                                Due This Month
                            </th>
                        </tr>
                        <tr>
                            <th>
                                Interest
                            </th>
                            <th>
                                Principal
                            </th>
                            <th>
                                Interest
                            </th>
                            <th>
                                Principal
                            </th>
                            
                            <th>
                                Interest
                            </th>
                            <th>
                                Principal
                            </th>
                            <th>
                                Interest
                            </th>
                            <th>
                                Principal
                            </th>
                        </tr>
                    </thead>
                    <tbody id="uxRepBody">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="modal hide" id="modalRep">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal">
                ×</button>
            <h3>
                Save Repayment Form
            </h3>
        </div>
        <div class="modal-body of-modal-body">
            <div class="laf-mod-bod" id="modalRepNot">
            </div>
        </div>
        <div class="modal-footer">
            <div>
                <a class="btn btn-success" id="btnOkRep"><i class="icon-ok icon-white"></i>&nbsp;Yes
                </a><a class="btn" id="btnCancelRep">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>
