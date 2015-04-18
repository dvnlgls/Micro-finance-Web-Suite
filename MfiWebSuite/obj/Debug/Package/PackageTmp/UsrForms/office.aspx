<%@ Page Title="Offices & Villages" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="office.aspx.cs" Inherits="MfiWebSuite.UsrForms.office" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="../DataTable/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <style type="text/css">
        
        div.dataTables_wrapper
        {
        	width:600px;
        }
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
        
        table#tblVillage
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
        /* Default class modification */
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
            if (jQuery("#ContentPlaceHolder1_tabContent").find("#tblVillage").length == 1) {

                var ColCtr = $("#tblVillage thead tr th").length;                

                if (ColCtr == 2) {
                    $('#tblVillage').dataTable({                        
                        "sPaginationType": "bootstrap",
                        "oLanguage": {
                            "sLengthMenu": "_MENU_ records per page"
                        }
                    });
                }
                else {
                    $('#tblVillage').dataTable({
                        "sPaginationType": "bootstrap",
                        "oLanguage": {
                            "sLengthMenu": "_MENU_ records per page"
                        },
                        "aoColumnDefs": [{ "bSortable": false, "aTargets": [2]}]
                    });
                }
            }
        });
    </script>
    <script type="text/javascript">

        $(function () {

            $('#modalAdd').modal({
                keyboard: false,
                backdrop: 'static',
                show: false
            });

            $('#modalDel').modal({
                keyboard: false,
                backdrop: 'static',
                show: false
            });


            var validator = $("#form1").validate({
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

            $('#modalAdd').on('shown', function () {
                $("#txtName").focus();
            });


            $("[ID^='btnDel_']").live("click", function () {

                $("#hdnOID").val("");
                $("#hdnOID").val($(this).attr("id").split('_')[1]);

                $("#spnDelTxt").text("Do you really want to delete?");
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
                        url: "/Ajax/offices.aspx",
                        data: "AjaxMethod=DeleteOffice" +
                        "&OID=" + $("#hdnOID").val(),
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
                                    $("#spnDelTxt").text("You cannot delete an office when it has sub-offices.");
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
                        location.reload();
                    }
                    else {
                        $('#modalDel').modal('hide');
                    }
                }
            });

            $("[ID^='btnAdd_']").live("click", function () {

                $("#spnEntityName").text($("#ContentPlaceHolder1_tab li.active a").text());
                //modal reset
                validator.resetForm();

                $("#hdnOTID").val("");

                $("#modalNot").html("");
                $("#drpParentMenu").html("<img src='/Images/Resources/ajax-loader.gif' />");

                $("#divAddOfc").show();

                $("#btnSave").addClass("disabled").show();
                $("#btnCancel").addClass("disabled").show();

                $(".notvil").show();

                $("#divAddOfc input[ID^='txt']").val("");
                $("#txtAddr").val("");
                $(".edit").removeClass("disabled").removeAttr("disabled");
                $("#drpParent").removeAttr("disabled");

                var typeID = $(this).attr("id").split('_')[1];
                $("#hdnOTID").val(typeID);

                if (typeID == "5") {
                    $(".notvil").hide();
                }

                $.ajax({
                    type: "POST",
                    url: "/Ajax/offices.aspx",
                    data: "AjaxMethod=FetchParent" +
                        "&OTID=" + typeID,
                    success: function (returnResponse) {

                        var element = jQuery(returnResponse).find("#hdnLUK");

                        if (element.length == 1) {
                            window.location = "/login?rr=se";
                        }
                        else {

                            if (returnResponse == "Error_Default") {
                            }
                            else if (returnResponse == "Error_SessionExpired") {
                                window.location = "/login?rr=se";
                            }
                            else {
                                $("#drpParentMenu").html(returnResponse);
                                $("#btnSave").removeClass("disabled");
                                $("#btnCancel").removeClass("disabled");
                            }
                        }
                    }
                }); //ajax

                $('#modalAdd').modal('show');

            });

            $("#btnSave").click(function () {
                if (!$(this).hasClass('disabled')) {

                    $("#modalNot").html("");

                    if ($("#form1").valid()) {

                        $("#btnSave").addClass("disabled");
                        $("#btnCancel").addClass("disabled");
                        $(".edit").addClass("disabled").attr("disabled", "");
                        $("#drpParent").attr("disabled", "disabled");

                        //
                        $.ajax({
                            type: "POST",
                            url: "/Ajax/offices.aspx",
                            data: "AjaxMethod=AddOffice" +
                                "&PID=" + $("#drpParent option:selected").attr("id").split('_')[1] +
                                "&OTID=" + $("#hdnOTID").val() +
                                "&Name=" + $("#txtName").val() +
                                "&Addr=" + $("#txtAddr").val() +
                                "&Ph1=" + $("#txtPh1").val() +
                                "&Ph2=" + $("#txtPh2").val() +
                                "&Fax=" + $("#txtFax").val() +
                                "&Web=" + $("#txtWeb").val() +
                                "&Email=" + $("#txtMail").val(),

                            success: function (returnResponse) {

                                $("#btnSave").hide();
                                $("#btnCancel").hide();

                                var element = jQuery(returnResponse).find("#hdnLUK");

                                if (element.length == 1) {
                                    window.location = "/login?rr=se";
                                }
                                else {
                                    if (returnResponse == "Error_Default") {
                                        $("#divAddOfc").hide();
                                        $("#modalNot").html("<div class='alert alert-error'>Sorry, encountered an error. Please try again. <a href='#' class='btn' data-dismiss='modal'>Ok</a></div>");
                                    }
                                    else if (returnResponse == "Error_SessionExpired") {
                                        window.location = "/login?rr=se";
                                    }
                                    else if (returnResponse == "Success_") {

                                        $("#divAddOfc").hide();
                                        $("#modalNot").html("<div class='alert alert-success'>Successfully Created! <a href='#' id='btnAdded' class='btn'>Ok</a></div>");

                                    }
                                }
                            }
                        }); //ajax

                    } //frm valid
                }
            }); //save

            $("#btnCancel").click(function () {
                if (!$(this).hasClass('disabled')) {
                    $('#modalAdd').modal('hide');
                }

            });

            $("#btnAdded").live('click', function () {
                //$('#modalAdd').modal('hide');
                location.reload();
            });

        });

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content">        
            <ul class="nav nav-tabs" id="tab" runat="server">
                <li class="active"><a href="#HO" data-toggle="tab">Head Office</a></li>
                <li><a href="#RO" data-toggle="tab">Regional Office</a></li>
                <li><a href="#AO" data-toggle="tab">Area Office</a></li>
                <li><a href="#BO" data-toggle="tab">Branch Office</a></li>
                <li><a href="#VIL" data-toggle="tab">Village</a></li>
            </ul>
            <div class="tab-content" id="tabContent" runat="server">
            </div>
            <div class="modal hide" id="modalAdd">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        ×</button>
                    <h3>
                        Create New <span id="spnEntityName"></span>
                    </h3>
                </div>
                <div class="modal-body of-modal-body">
                    <div class="of-modal-body-not" id="modalNot">
                    </div>
                    <div id="divAddOfc" class="of-add-wrap">
                        
                        <table class="table ho-add-tbl">
                            <tr>
                                <td class="lbl">
                                    Name
                                </td>
                                <td>
                                    <input type="text" id="txtName" name="txtName" class="span3 edit" />
                                    <span class="gen-err-1" id="errName"></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="lbl">
                                    Parent Office
                                </td>
                                <td id="drpParentMenu">
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">
                                    Address
                                </td>
                                <td>
                                    <textarea id="txtAddr" name="txtAddr" rows="3" cols="2" class="edit"></textarea>
                                    <span class="gen-err-1" id="errAddr"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">
                                    Phone 1
                                </td>
                                <td>
                                    <input type="text" id="txtPh1" name="txtPh1" class="span3 edit" />
                                    <span class="gen-err-1" id="errPh1"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">
                                    Phone 2
                                </td>
                                <td>
                                    <input type="text" id="txtPh2" name="txtPh2" class="span3 edit" />
                                    <span class="gen-err-1" id="errPh2"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">
                                    Fax
                                </td>
                                <td>
                                    <input type="text" id="txtFax" name="txtFax" class="span3 edit" />
                                    <span class="gen-err-1" id="errFax"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">
                                    Email-ID
                                </td>
                                <td>
                                    <input type="text" id="txtMail" name="txtMail" class="span3 edit" />
                                    <span class="gen-err-1" id="errMail"></span>
                                </td>
                            </tr>
                        </table>
                        
                    </div>
                </div>
                <div class="modal-footer">
                    <div>
                        <a class="btn btn-success" id="btnSave">
                            <i class="icon-ok icon-white"></i>&nbsp;Create
                        </a>
                        <a class="btn" id="btnCancel">
                            Cancel</a>
                    </div>
                </div>
            </div>
            <div class="modal hide" id="modalDel">
                <div class="modal-footer">
                    <div class="of-del-modal">
                        <table>
                            <tr>
                                <td class="of-del-modal-txt"><span id="spnDelTxt">Do you really want to delete?</span>
                                </td>
                                <td class="of-del-btn">
                                    <a class="btn btn-danger btn-large" id="btnDelYes">
                                        Yes
                                    </a>
                                </td>
                                <td>
                                    <a class="btn btn-large" id="btnDelNo">
                                        No</a>
                                </td>
                            </tr>
                        </table>                        
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnOTID" />
        <input type="hidden" id="hdnOID" />
    </div>
</asp:Content>


