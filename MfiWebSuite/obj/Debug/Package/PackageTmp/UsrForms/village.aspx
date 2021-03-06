﻿<%@ Page Title="Village" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="village.aspx.cs" Inherits="MfiWebSuite.UsrForms.village" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../DataTable/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <style type="text/css">
        div.dataTables_length label
        {
            float: left;
            text-align: left;
        }
        
        div.dataTables_length select
        {
            width: 75px;
        }
        
        div.dataTables_filter label
        {
            float: right;
        }
        
        div.dataTables_info
        {
            padding-top: 8px;
            float: left;
        }
        
        div.dataTables_paginate
        {
            float: right;
            margin: 0;
        }
        
        table.display
        {
            clear: both;
            float: left;
            margin-bottom: 33px;
            margin-top: 30px;
        }
        
        table.table thead .sorting, table.table thead .sorting_asc, table.table thead .sorting_desc, table.table thead .sorting_asc_disabled, table.table thead .sorting_desc_disabled
        {
            cursor: pointer;
        }
        table.table thead .sorting
        {
            background: url('/DataTable/images/sort_both.png') no-repeat center right;
        }
        table.table thead .sorting_asc
        {
            background: url('/DataTable/images/sort_asc.png') no-repeat center right;
        }
        table.table thead .sorting_desc
        {
            background: url('/DataTable/images/sort_desc.png') no-repeat center right;
        }
        
        table.table thead .sorting_asc_disabled
        {
            background: url('/DataTable/images/sort_asc_disabled.png') no-repeat center right;
        }
        table.table thead .sorting_desc_disabled
        {
            background: url('/DataTable/images/sort_desc_disabled.png') no-repeat center right;
        }
    </style>
    <script type="text/javascript">
        /*  Default class modification */
        $.extend($.fn.dataTableExt.oStdClasses, {
            "sWrapper": "dataTables_wrapper form-inline"
        });

        /* API method to get paging information */
        $.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
            return {
                "iStart": oSettings._iDisplayStart,
                "iEnd": oSettings.fnDisplayEnd(),
                "iLength": oSettings._iDisplayLength,
                "iTotal": oSettings.fnRecordsTotal(),
                "iFilteredTotal": oSettings.fnRecordsDisplay(),
                "iPage": Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
                "iTotalPages": Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
            };
        }

        /* Bootstrap style pagination control */
        $.extend($.fn.dataTableExt.oPagination, {
            "bootstrap": {
                "fnInit": function (oSettings, nPaging, fnDraw) {
                    var oLang = oSettings.oLanguage.oPaginate;
                    var fnClickHandler = function (e) {
                        e.preventDefault();
                        if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                            fnDraw(oSettings);
                        }
                    };

                    $(nPaging).addClass('pagination').append(
				'<ul>' +
					'<li class="prev disabled"><a href="#">&larr; ' + oLang.sPrevious + '</a></li>' +
					'<li class="next disabled"><a href="#">' + oLang.sNext + ' &rarr; </a></li>' +
				'</ul>'
			);
                    var els = $('a', nPaging);
                    $(els[0]).bind('click.DT', { action: "previous" }, fnClickHandler);
                    $(els[1]).bind('click.DT', { action: "next" }, fnClickHandler);
                },

                "fnUpdate": function (oSettings, fnDraw) {
                    var iListLength = 5;
                    var oPaging = oSettings.oInstance.fnPagingInfo();
                    var an = oSettings.aanFeatures.p;
                    var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                    if (oPaging.iTotalPages < iListLength) {
                        iStart = 1;
                        iEnd = oPaging.iTotalPages;
                    }
                    else if (oPaging.iPage <= iHalf) {
                        iStart = 1;
                        iEnd = iListLength;
                    } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                        iStart = oPaging.iTotalPages - iListLength + 1;
                        iEnd = oPaging.iTotalPages;
                    } else {
                        iStart = oPaging.iPage - iHalf + 1;
                        iEnd = iStart + iListLength - 1;
                    }

                    for (i = 0, iLen = an.length; i < iLen; i++) {
                        // Remove the middle elements
                        $('li:gt(0)', an[i]).filter(':not(:last)').remove();

                        // Add the new list items and their event handlers
                        for (j = iStart; j <= iEnd; j++) {
                            sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                            $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
						.insertBefore($('li:last', an[i])[0])
						.bind('click', function (e) {
						    e.preventDefault();
						    oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
						    fnDraw(oSettings);
						});
                        }

                        // Add / remove disabled classes from the static elements
                        if (oPaging.iPage === 0) {
                            $('li:first', an[i]).addClass('disabled');
                        } else {
                            $('li:first', an[i]).removeClass('disabled');
                        }

                        if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                            $('li:last', an[i]).addClass('disabled');
                        } else {
                            $('li:last', an[i]).removeClass('disabled');
                        }
                    }
                }
            }
        });

        $(function () {
            if (jQuery("#tblCenWrap").find("#tblCenters").length == 1) {

                var ColCtr = $("#tblCenters thead tr th").length;

                if (ColCtr == 4) {
                    $('#tblCenters').dataTable({
                        "sPaginationType": "bootstrap",
                        "oLanguage": {
                            "sLengthMenu": "_MENU_ records per page"
                        }
                    });
                }
                else {
                    $('#tblCenters').dataTable({
                        "sPaginationType": "bootstrap",
                        "oLanguage": {
                            "sLengthMenu": "_MENU_ records per page"
                        },
                        "aoColumnDefs": [{ "bSortable": false, "aTargets": [4]}]
                    });
                }
            }
        });
    </script>
    <script type="text/javascript">
        $(function () {

            $("#form1").validate({
                ignore: "",
                rules: {
                    txtName: {
                        required: true,
                        maxlength: 50
                    }

                }, //rules

                errorPlacement: function (error, element) {
                    if (element.attr("name") == "txtName")
                        error.appendTo("#errName");

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


            $("#modalAddCenter").modal({
                keyboard: false,
                backdrop: 'static',
                show: false
            });

            $('#modalAddCenter').on('shown', function () {
                $("#txtCenterLocation").focus();
            });

            $("#btnAddCenter").live("click", function () {

                $("#modalNot").html("");

                $("#txtCenterLocation").val("").removeAttr("disabled").show();
                $("#uxMeetingTime").val("").removeAttr("disabled").show();
                $("#drpFE").val("0").removeAttr("disabled");

                $("#btnCreateCenter").removeClass("disabled").show();
                $("#btnCancelCenter").removeClass("disabled").text("Cancel").show();

                $('#modalAddCenter').modal('show');

            });


            $("#btnCreateCenter").click(function () {
                if (!$(this).hasClass('disabled')) {

                    var errors = fnCenterVal();

                    if (errors.length == 0) {
                        //val passed
                        $("#txtCenterLocation").attr("disabled", "disabled");
                        $("#uxMeetingTime").attr("disabled", "disabled");
                        $("#drpFE").attr("disabled", "disabled");

                        $("#btnCreateCenter").addClass("disabled");
                        $("#btnCancelCenter").addClass("disabled");

                        var meetingLoc = $("#txtCenterLocation").val();
                        var meetingTime = $("#uxMeetingTime").val();
                        var feID = $("#drpFE option:selected").val();
                        var feName = $("#drpFE option:selected").text();
                        var villageID = $("#ContentPlaceHolder1_hdnOid").val();

                        $.ajax({
                            type: "POST",
                            url: "/Ajax/offices.aspx",
                            data: "AjaxMethod=AddCenter" +
                                    "&VillageID=" + villageID +
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

                                    if (responseStatus == "Error") {
                                        if (responseMsg == "Default") {
                                            $("#modalNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                                            $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");

                                            $("#btnCreateCenter").removeClass("disabled");
                                            $("#btnCancelCenter").removeClass("disabled");
                                        }
                                    }
                                    else if (responseStatus == "Success") {
                                        $("#modalNot").html("<div id='frmAlert' class='alert alert-block alert-success'></div>");
                                        $("#frmAlert").append("<strong>Success!</strong> <a href='#'>Center " + responseMsg + "</a> has been created.");


                                        var tblCen = jQuery("#ContentPlaceHolder1_uxCenterList").find("#tblCenters");

                                        if (tblCen.length == 1) {
                                            //table is present

                                            var firstRow = $("#tblCenters tbody tr:eq(0)");

                                            var newCenter = "<tr class='newCen'>";
                                            newCenter += "<td><a href='/" + responseMsg + "/center'>Center " + responseMsg + "</a></td>";
                                            newCenter += "<td>" + feName + "</td>";
                                            newCenter += "<td>" + meetingLoc + "</td>";
                                            newCenter += "<td>" + meetingTime + "</td>";

                                            var today = new Date();
                                            var dd = today.getDate();
                                            var mm = today.getMonth() + 1; //January is 0!

                                            if (dd < 10) { dd = '0' + dd }

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

                                            mm = month[mm];

                                            var yy = (today.getFullYear()) % 100;

                                            var createdDate = dd + '-' + mm + '-' + yy;

                                            newCenter += "<td>" + createdDate + "</td>";
                                            if ($("#ContentPlaceHolder1_hdnAcD").val() == "1") {
                                                newCenter += "<td><a id='btnDel_" + responseMsg + "' class='btn btn-danger btn-mini'><i class='icon-white icon-trash'></i>&nbsp;Remove Center</a></td>";
                                            }

                                            newCenter += "</tr>";

                                            $(newCenter).insertBefore(firstRow);
                                        }
                                        else {
                                            //first center of this village. reload to check permissions and gen tbl
                                            location.reload();
                                        }

                                        $("#btnCreateCenter").removeClass("disabled").hide();
                                        $("#btnCancelCenter").removeClass("disabled").text("Ok");
                                    }

                                    $("#txtCenterLocation").val("").removeAttr("disabled");
                                    $("#uxMeetingTime").val("").removeAttr("disabled");
                                    $("#drpFE").removeAttr("disabled");
                                }
                            }
                        }); //ajax
                    } //if
                    else {
                        $("#modalNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                        $("#frmAlert").append("<a class='close' data-dismiss='alert' href='#'>×</a>");
                        $("#frmAlert").append("<h4 class='alert-heading'>Please correct the below mistake(s) to continue:</h4>");

                        for (var i = 0; i < errors.length; i++) {
                            $("#frmAlert").append("<h5><i class='icon-hand-right'></i>&nbsp; " + errors[i] + "</h5>");
                        }

                    } //else

                } //not disabled
            }); //create center


            $("#btnCancelCenter").click(function () {
                if (!$(this).hasClass('disabled')) {
                    $('#modalAddCenter').modal('hide');
                }
            });


            //delete center
            $("[ID^='btnDel_']").live("click", function () {

                $("#hdnCID").val("");
                $("#hdnCID").val($(this).attr("id").split('_')[1]);

                $("#spnDelTxt").text("Do you really want to delete this Center?");
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
                        data: "AjaxMethod=DeleteCenter" +
                        "&CID=" + $("#hdnCID").val(),
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
                                    $("#spnDelTxt").text("This center has groups in it. Please delete the groups before deleting this center.");
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
                        $("#tblCenters #trCen_" + $("#hdnCID").val()).remove();
                        $("#hdnCID").val("");
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

        });                          //doc ready

        function fnCenterVal() {

            var locationOk = false;
            var timeOK = false;
            var feOk = false;

            var errorMsgs = new Array();

            if ($("#txtCenterLocation").val().trim() != "") {
                locationOk = true;
            }

            if ($("#uxMeetingTime").val().trim() != "") {
                timeOK = true;
            }

            if ($("#drpFE option:selected").val() != "0") {
                feOk = true;
            }

            //consolidate errors
            if (!locationOk) {
                errorMsgs.push("Please enter a Meeting Location");
            }
            if (!timeOK) {
                errorMsgs.push("Please enter a Meeting Time");
            }
            if (!feOk) {
                errorMsgs.push("Please select a Field Executive");
            }

            return errorMsgs;
        }

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
                        "&Addr=" +
                        "&Ph1=" +
                        "&Ph2=" +
                        "&Fax=" +
                        "&Web=" +
                        "&Email=",

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content" id="uxOffcontent" runat="server">
            <div class="gen-title">
                Village</div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="vil-det-wrap">
                <div class="ho-details" id="viewRO" runat="server">
                    <div class="ho-title">
                        Village Details
                    </div>
                    <table class="table ho-tbl">
                        <tr>
                            <td class="lbl">
                                Name
                            </td>
                            <td>
                                <span class="orig" id="spnName" runat="server"></span>
                                <input type="text" id="txtName" name="txtName" class="span3 edit" />
                                <span class="gen-err-1" id="errName"></span>
                            </td>
                        </tr>
                    </table>
                    <div id="editOff" runat="server">
                        <div class="ho-btn-wrap">
                            <a class="btn btn-primary" id="btnEdit"><i class="icon-pencil icon-white"></i>&nbsp;
                                Edit</a> <a class="btn btn-success gen-nodisplay" id="btnSave"><i class="icon-ok icon-white">
                                </i>Save </a><a class="btn gen-nodisplay" id="btnCancel">Cancel</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="vil-tbl-wrap">
                <div class="vil-cent-lst" id="viewSubOff" runat="server">
                    <div class="ho-title">
                        Centers under this office
                    </div>
                    <div class="" id="uxCenterList" runat="server">
                    </div>
                </div>
            </div>
        </div>
        <div class="modal hide" id="modalAddCenter">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">
                    ×</button>
                <h3>
                    Create New Center
                </h3>
            </div>
            <div class="modal-body of-modal-body">
                <div class="of-modal-body-not" id="modalNot">
                </div>
                <div id="divAddOfc" class="of-add-wrap">
                    <table class="table ho-add-tbl">
                        <tr>
                            <td class="lbl">
                                Meeting Location
                            </td>
                            <td>
                                <input type="text" id="txtCenterLocation" name="txtName" class="span3 edit" />
                                <span class="gen-err-1" id="Span1"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">
                                Meeting Time
                            </td>
                            <td>
                                <input type="text" id="uxMeetingTime" class="input-mini" />
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">
                                Field Executive
                            </td>
                            <td id="FE" runat="server">
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="modal-footer">
                <div>
                    <a class="btn btn-success" id="btnCreateCenter"><i class="icon-ok icon-white"></i>&nbsp;Create
                    </a><a class="btn" id="btnCancelCenter">Cancel</a>
                </div>
            </div>
        </div>
        <div class="modal hide" id="modalDel">
            <div class="modal-footer">
                <div class="of-del-modal">
                    <table>
                        <tr>
                            <td class="of-del-modal-txt">
                                <span id="spnDelTxt">Do you really want to delete this Center?</span>
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
        <input type="hidden" id="hdnOid" runat="server" />
        <input type="hidden" id="hdnAcD" runat="server" />
        <input type="hidden" id="hdnCID" />
    </div>
</asp:Content>
