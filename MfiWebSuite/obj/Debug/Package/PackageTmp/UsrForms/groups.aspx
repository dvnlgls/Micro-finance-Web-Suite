<%@ Page Title="Groups" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="groups.aspx.cs" Inherits="MfiWebSuite.UsrForms.groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">

        $.fn.scrollView = function () {
            return this.each(function () {
                $('html, body').animate({
                    scrollTop: $(this).offset().top
                }, 1000);
            });
        }

        

        function fnGetQueryParam(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.search);
            if (results == null)
                return "";
            else
                return decodeURIComponent(results[1].replace(/\+/g, " "));
        }


        $(function () {
           
            $(".cmGrpStat").tooltip();

            $("#drpBranch option:eq(0)").attr('selected', true);
            $("#drpVillage option:eq(0)").attr('selected', true);
            $("#drpCenter option:eq(0)").attr('selected', true);

            $("#drpBranch").keypress(function () {
                $(this).trigger("change");
            });

            $("#drpVillage").keypress(function () {
                $(this).trigger("change");
            });

            $("#drpCenter").keypress(function () {
                $(this).trigger("change");
            });

            $("#drpBranch").change(function () {

                var BranchID = $("#drpBranch option:selected").val();

                $("#drpVillage option").show();
                $("#drpVillage option:eq(0)").attr('selected', true);

                if (BranchID != "0") {
                    $("#drpVillage option").show().not("." + BranchID).hide();
                    $("#drpVillage option:eq(0)").show().attr('selected', true);
                }

                $("#drpVillage").trigger("change");
            });



            $("#drpVillage").change(function () {

                var VillageID = $("#drpVillage option:selected").val();

                $("#drpCenter option").show();
                $("#drpCenter option:eq(0)").attr('selected', true);

                if (VillageID != "0") {

                    $("#drpCenter option").show().not("." + VillageID).hide();
                    $("#drpCenter option:eq(0)").show().attr('selected', true);
                }

                $("#drpCenter").trigger("change");

            });

            $("#drpCenter").change(function () {
                var CenterID = $("#drpCenter option:selected").val();
                var VillageID = $("#drpVillage option:selected").val();
                var BranchID = $("#drpBranch option:selected").val();


                $("#ContentPlaceHolder1_uxGroupBody tr").show();

                if (BranchID != "0") {
                    $("#ContentPlaceHolder1_uxGroupBody tr").not(".cmBranch" + BranchID).hide();
                }

                if (VillageID != "0") {
                    $("#ContentPlaceHolder1_uxGroupBody tr").not(".cmVillage" + VillageID).hide();
                }

                if (CenterID != "0") {
                    $("#ContentPlaceHolder1_uxGroupBody tr").not(".cmCenter" + CenterID).hide();
                }

                //  fnRedrawTable();

            });

            $("[ID^='ancGrp_']").toggle(function () {

                var GrpID = $(this).attr("id").split('_')[1];

                if ($("#ContentPlaceHolder1_uxGroupBody #tdClientDetail_" + GrpID).length == 0) {

                    $("#trGrp_" + GrpID).after("<tr id='trClientDetail_" + GrpID + "'><td id='tdClientDetail_" + GrpID + "' colspan='9'></td></tr>");
                    $("#tdClientDetail_" + GrpID).append("<table class='table table-bordered table-condensed grp-gc-tbl'><thead><tr><th>Client</th><th>Loan</th><th>Loan Status</th></tr></thead><tbody></tbody></table>");
                    $("#ContentPlaceHolder1_uxGCBody .trGC_" + GrpID).clone().appendTo("#tdClientDetail_" + GrpID + " tbody");
                }

                $("#trClientDetail_" + GrpID).slideDown('fast');
            }, function () {
                var GrpID = $(this).attr("id").split('_')[1];
                $("#trClientDetail_" + GrpID).slideUp('fast');
            }
            );


            //delete group
            $("[ID^='btnDel_']").live("click", function () {

                $("#hdnGID").val("");
                $("#hdnGID").val($(this).attr("id").split('_')[1]);

                $("#spnDelTxt").text("Do you really want to delete this Group?");
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
                        url: "/Ajax/offices.aspx",
                        data: "AjaxMethod=DeleteGroup" +
                        "&GID=" + $("#hdnGID").val(),
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
                                else if (returnResponse == "HasSubOffice") {
                                    $("#spnDelTxt").text("Sorry, you can't delete an active group.");
                                    $("#btnDelYes").removeClass("disabled").hide();
                                    $("#btnDelNo").removeClass("disabled").text("Close");
                                }

                                else if (returnResponse == "Error_SessionExpired") {
                                    window.location = "/login?rr=se";
                                }
                                else {
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
                        $("#uxGroupTbl #trGrp_" + $("#hdnGID").val()).remove();
                        $("#uxGroupTbl #trClientDetail_" + $("#hdnGID").val()).remove();
                        $("#hdnGID").val("");
                        $('#modalDel').modal('hide');
                    }
                    else {
                        $('#modalDel').modal('hide');
                    }
                }
            });

            $('#modalDel').modal({
                keyboard: false,
                backdrop: 'static',
                show: false
            });

            var GC = fnGetQueryParam("gid");
            if (GC > 0) {
                $("#ancGrp_" + GC).click().scrollView();
                
            }

        }); //doc ready

        //        function fnRedrawTable() {
        //            //alert("called");
        //            var oTable = $('#uxGroupTbl').dataTable();
        //            oTable.fnPageChange(1);
        //        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content" id="uxCentContent" runat="server">
            <div class="gen-title">
                Groups</div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="grp-filters">
                <table class="table table-bordered grp-tbl">
                    <tbody>
                        <tr>
                            <td class="lbl">
                                Branch
                            </td>
                            <td id="uxDrpBranchWrap" runat="server">
                            </td>
                            <td class="lbl">
                                Village
                            </td>
                            <td id="uxDrpVillageWrap" runat="server">
                            </td>
                            <td class="lbl">
                                Center
                            </td>
                            <td id="uxDrpCenterWrap" runat="server">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="grp-list" id="uxGroups" runat="server">
                <table class="table table-bordered ho-off-tbl" id="uxGroupTbl">
                    <thead>
                        <tr>
                            <th>
                                Group
                            </th>
                            <th>
                                Group Status
                            </th>
                            <th>
                                Group Leader
                            </th>
                            <th>
                                Created By
                            </th>
                            <th>
                                Created Date
                            </th>
                            <th>
                                Center
                            </th>
                            <th>
                                Village
                            </th>
                            <th>
                                Branch
                            </th>
                            <th>
                                <i class='icon-cog'></i>&nbsp;Options
                            </th>
                        </tr>
                    </thead>
                    <tbody id="uxGroupBody" runat="server">
                    </tbody>
                </table>
            </div>
            <div class="modal hide" id="modalDel">
                <div class="modal-footer">
                    <div class="of-del-modal">
                        <table>
                            <tr>
                                <td class="of-del-modal-txt">
                                    <span id="spnDelTxt">Do you really want to delete this Group?</span>
                                </td>
                                <td class="of-del-btn">
                                    <a class="btn btn-danger btn-large" id="btnDelYes">Yes </a>
                                </td>
                                <td>
                                    <a class="btn btn-large" id="btnDelNo">No</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <table class="hide">
                <tbody id="uxGCBody" runat="server">
                </tbody>
            </table>
        </div>
        <input type="hidden" id="hdnGID" />
    </div>
</asp:Content>
