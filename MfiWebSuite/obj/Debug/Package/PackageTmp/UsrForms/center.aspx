<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true"
    CodeBehind="center.aspx.cs" Inherits="MfiWebSuite.UsrForms.center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Center
            </div>
            <div id="locNotHO" class="span12">
            </div>
            <div id="uxCenterWrap" runat="server" class="span12">
                <div class="ho-details">
                    <div class="ho-title">
                        Center Details
                    </div>
                    <table class="table ho-tbl">
                        <tr>
                            <td class="lbl">
                                Name
                            </td>
                            <td>
                                <span id="uxCenterName" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">
                                Meeting Location
                            </td>
                            <td>
                                <input type="text" class="cmEditable" id="uxMeetingLoc" runat="server" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">
                                Meeting Time
                            </td>
                            <td>
                                <input type="text" id="uxMeetingTime" class="cmEditable input-mini" runat="server"
                                    disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">
                                Field Executive
                            </td>
                            <td id="uxFE" runat="server">
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">
                                Created On
                            </td>
                            <td>
                                <span id="uxCreatedOn" runat="server"></span>
                            </td>
                        </tr>
                    </table>
                    <div id="uxEditCenter" runat="server">
                        <div class="ho-btn-wrap">
                            <a class="btn btn-primary" id="btnEdit"><i class="icon-pencil icon-white"></i>&nbsp;
                                Edit</a> <a class="btn btn-success gen-nodisplay" id="btnSave"><i class="icon-ok icon-white">
                                </i>Save </a><a class="btn gen-nodisplay" id="btnCancel">Cancel</a>
                        </div>
                    </div>
                </div>
                <div class="cent-grp-det" id="uxGroups" runat="server">
                    <div class="ho-title">
                        Groups under this Center
                    </div>
                    <table class="table table-bordered ho-off-tbl">
                        <thead>
                            <tr>
                                <th>
                                    Group
                                </th>
                                <th>
                                    Group Status
                                </th>
                                <th>
                                    Created By
                                </th>
                                <th>
                                    Created Date
                                </th>
                            </tr>
                        </thead>
                        <tbody id="uxGroupBody" runat="server">
                        </tbody>
                    </table>
                </div>
            </div>
            <input type="hidden" id="hdnOid" runat="server" />
            <input type="hidden" id="hdnCL" />
            <input type="hidden" id="hdnCT" />
            <input type="hidden" id="hdnFE" />
        </div>
    </div>
</asp:Content>
