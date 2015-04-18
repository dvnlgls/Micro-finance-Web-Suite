<%@ Page Title="Center Collection Sheet" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true" CodeBehind="centercollecreport.aspx.cs" Inherits="MfiWebSuite.UsrForms.centercollecreport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(function () {

        $("#drpCenter").change(function () {
            var CenterID = $("#drpCenter option:selected").val();
            $('#uxMeetingDate').html('');

            if (CenterID != "0") {
                $('#uxMeetingDate').attr('disabled', 'disabled');
                fnGetMeetingDate(CenterID);
            }
        });

    
        $("#drpCenter option:eq(0)").attr('selected', true);

        $("#btnShowRepaymentForm").click(function () {
            if (!$(this).hasClass('disabled')) {

                $('#uxRepBody').html("");
                               
                var repDt = $("#uxMeetingDate option:selected").val();

                if (repDt) {
                    var CenterID = $("#drpCenter option:selected").val();

                    $(this).addClass('disabled');
                    fnGetRepData(CenterID, repDt);
                                    
                }
                else {
                    alert("Please select a meeting date");
                }
            }
        });

    });         //doc ready

    function fnGetRepData(centerID, plannedDate) {
        $.ajax({
            type: "POST",
            url: "/Ajax/loan.aspx",
            data: "AjaxMethod=GetRepaymentData" +
                        "&CenterID=" + centerID +
                        "&PlannedDate=" + plannedDate +
                        "&IsReport=1",
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Center Collection Sheet
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
                            <td>
                                <a id="btnShowRepaymentForm" class="btn btn-primary pull-left">Show Report</a>
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
</asp:Content>
