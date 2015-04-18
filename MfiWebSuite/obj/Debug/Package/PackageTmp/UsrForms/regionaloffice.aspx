<%@ Page Title="Regional Office" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="regionaloffice.aspx.cs" Inherits="MfiWebSuite.UsrForms.regionaloffice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(function () {

        $("#form1").validate({
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


    });         //doc ready

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
                        "&Addr=" + $("#txtAddr").val() +
                        "&Ph1=" + $("#txtPh1").val() +
                        "&Ph2=" + $("#txtPh2").val() +
                        "&Fax=" + $("#txtFax").val() +
                        "&Web=" +
                        "&Email=" + $("#txtMail").val(),

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
                Regional Office</div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="ho-details" id="viewRO" runat="server">
                <div class="ho-title">
                    Office Details
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
                    <tr>
                        <td class="lbl">
                            Address
                        </td>
                        <td>
                            <span id="spnAddr" runat="server" class="orig"></span>
                            <textarea id="txtAddr" name="txtAddr" rows="3" cols="2" class="edit"></textarea>
                            <span class="gen-err-1" id="errAddr"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">
                            Phone 1
                        </td>
                        <td>
                            <span id="spnPh1" runat="server" class="orig"></span>
                            <input type="text" id="txtPh1" name="txtPh1" class="span3 edit" />
                            <span class="gen-err-1" id="errPh1"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">
                            Phone 2
                        </td>
                        <td>
                            <span id="spnPh2" runat="server" class="orig"></span>
                            <input type="text" id="txtPh2" name="txtPh2" class="span3 edit" />
                            <span class="gen-err-1" id="errPh2"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">
                            Fax
                        </td>
                        <td>
                            <span id="spnFax" runat="server" class="orig"></span>
                            <input type="text" id="txtFax" name="txtFax" class="span3 edit" />
                            <span class="gen-err-1" id="errFax"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">
                            Email-ID
                        </td>
                        <td>
                            <span id="spnMail" runat="server" class="orig"></span>
                            <input type="text" id="txtMail" name="txtMail" class="span3 edit" />
                            <span class="gen-err-1" id="errMail"></span>
                        </td>
                    </tr>
                </table>
                
                <div id="editHO" runat="server">
                    <div class="ho-btn-wrap">
                        <a class="btn btn-primary" id="btnEdit">
                            <i class="icon-pencil icon-white"></i>&nbsp; Edit</a>
                        <a class="btn btn-success gen-nodisplay" id="btnSave">
                            <i class="icon-ok icon-white"></i>Save
                        </a>
                        <a class="btn gen-nodisplay" id="btnCancel">
                            Cancel</a>
                    </div>
                </div>
            </div>
            <div class="ho-off" id="viewAO" runat="server">
                <div class="ho-title">
                    Area Offices under this office
                </div>
                <table class="table table-bordered ho-off-tbl">
                    <thead>
                        <tr>
                            <th class="off">
                                Area Office
                            </th>
                            <th class="addr">
                                Address
                            </th>
                            <th class="ph">
                                Phone
                            </th>
                        </tr>
                    </thead>
                    <tbody id="uxSubOfficeTblBody" runat="server">
                    </tbody>
                </table>
            </div>
        </div>
        <input type="hidden" id="hdnOid" runat="server" />
    </div>

</asp:Content>
