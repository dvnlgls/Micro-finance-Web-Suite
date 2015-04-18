<%@ Page Title="Loan Application Form" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="loanappform.aspx.cs" Inherits="MfiWebSuite.UsrForms.loanappform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ui-autocomplete
        {
            max-height: 150px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            padding-right: 20px;
        }
        .ui-combobox
        {
            position: relative;
            display: inline-block;
        }
        .ui-combobox-toggle
        {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
            height: 26px;
            top: 0px;
        }
        
        #tblClients .ui-combobox-toggle
        {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
            height: 26px;
            top: 0px;
        }
        
        .ui-combobox-input
        {
            margin: 0;
            padding: 0.3em;
        }
        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default
        {
            background: none;
        }
    </style>
    <script src="../Scripts/json2.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function ($) {
            fnClearInputFields();

            $.widget("ui.combobox", {
                _create: function () {
                    var input,
					self = this,
					select = this.element.hide(),
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "",
					wrapper = this.wrapper = $("<span>")
						.addClass("ui-combobox")
						.insertAfter(select);

                    input = $("<input>")
					.appendTo(wrapper)
					.val(value)
					.addClass("ui-state-default ui-combobox-input")
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: function (request, response) {
					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function () {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));
					    },
					    select: function (event, ui) {
					        ui.item.option.selected = true;
					        self._trigger("selected", event, {
					            item: ui.item.option
					        });
					    },
					    change: function (event, ui) {
					        if (!ui.item) {
					            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
									valid = false;
					            select.children("option").each(function () {
					                if ($(this).text().match(matcher)) {
					                    this.selected = valid = true;
					                    return false;
					                }
					            });
					            if (!valid) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					                input.data("autocomplete").term = "";
					                return false;
					            }
					        }
					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left");

                    input.data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a>" + item.label + "</a>")
						.appendTo(ul);
                    };

                    $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.appendTo(wrapper)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-combobox-toggle")
					.click(function () {
					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return;
					    }

					    // work around a bug (likely same cause as #5265)
					    $(this).blur();

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();
					});
                },

                destroy: function () {
                    this.wrapper.remove();
                    this.element.show();
                    $.Widget.prototype.destroy.call(this);
                }
            });
        })(jQuery);

        $(function () {

            $('#modalLAF').modal({
                keyboard: false,
                backdrop: 'static',
                show: false
            });

            $(".gloAmnt").keyup(function () {
                $(".ldisb-amnt").val($(this).val());
            });

            $(".loanPurpose").html($("#drpLoanPurpose").html());


            $("#drpCenter option").hide();
            $("#drpCenter option:eq(0)").attr('selected', true);

            $("#drpFE option").hide();
            $("#drpFE option:eq(0)").attr('selected', true);

            $(".custName").combobox();
            $(".loanPurpose").combobox();


            $("#drpLP").change(function () {
                var LPID = $("#drpLP option:selected").val();
                if (LPID != "0") {
                    $("#lpDetails").show();
                    $("#ContentPlaceHolder1_lpDetailsBody tr").show().not("#" + LPID).hide();

                    $("#lpCycles").show();
                    $("#ContentPlaceHolder1_lcBody tr").show().not(".LC_" + LPID).hide();


                }
                else {
                    $("#lpDetails").hide();
                    $("#lpCycles").hide();
                }
            });

            $("#drpVillage").change(function () {
                var VillageID = $("#drpVillage option:selected").val();

                //center
                $("#drpCenter option").show().not("." + VillageID).hide();
                $("#drpCenter option:eq(0)").attr('selected', true);

                //fe
                var ParentID = $("#drpVillage option:selected").attr('class');
                $("#drpFE option").show().not("." + ParentID).hide();
                $("#drpFE option:eq(0)").attr('selected', true);

                //clients                

                //clear values
                $('#tblClients .ui-autocomplete-input').focus().val('').autocomplete('close');

                $('.custName').html($('#drpClients option.' + VillageID).clone());


            });


            $("#btnAddClient").click(function () {
                //*************************************************
                //stmnt order is very important. don't change willy nilly!
                //*************************************************

                var htmlStr = $("#template").html();
                $("<tr id='tmp'>" + htmlStr + "</tr>").insertBefore("#clientLast").effect("highlight", {}, 6000);

                $("#tmp td#ContentPlaceHolder1_drpCliWrap").removeAttr("id");

                var VillageID = $("#drpVillage option:selected").val();
                $('#drpClients').html($('#drpClients option.' + VillageID).clone());

                $("#tmp #drpClients").removeAttr("id");
                $("#tmp #drpLoanPurpose").removeAttr("id");

                $("#tmp .name").combobox().addClass('custName');
                $("#tmp .lp").combobox().addClass('loanPurpose');

                $('#tmp .ui-autocomplete-input').focus().val('').autocomplete('close');
                $("#tmp").removeAttr("id");

                $('.sno').each(function (index) {
                    $(this).html(index + 1);
                });
            }); //add cli


            $("#btnSaveLAF").click(function () {
                if (!$(this).hasClass('disabled')) {

                    $("#btnSaveLAF").addClass("disabled");
                    $("#btnCancel").addClass("disabled");

                    fnSaveLAF();

                }
            });

            $("#btnCancel").click(function () {
                if (!$(this).hasClass('disabled')) {
                    $('#modalLAF').modal('hide');
                }
            });


            $("#btnSendApproval").click(function () {

                $("#locNotHO").html("");

                var errors = fnVal();

                if (errors.length == 0) {

                    //val passed
                    $("#modalNot").html("<p>Are you sure you want to save this Loan Application Form?</p> <p>Please check the details before submitting the form.</p>"); //remove modal msg
                    $("#btnSaveLAF").removeClass("disabled").show();
                    $("#btnCancel").removeClass("disabled").text("Cancel").show();

                    $('#modalLAF').modal('show');

                } //if no error
                else {
                    $("#locNotHO").append("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                    $("#frmAlert").append("<a class='close' data-dismiss='alert' href='#'>×</a>");
                    $("#frmAlert").append("<h4 class='alert-heading'>Please correct the below mistake(s) to continue:</h4><br/>");

                    for (var i = 0; i < errors.length; i++) {
                        $("#frmAlert").append("<h5><i class='icon-hand-right'></i>&nbsp; " + errors[i] + "</h5>");
                    }

                    $("html, body").animate({ scrollTop: 0 }, "fast");
                }
            }); //btnSendApproval

            //to reset the ui after page refresh
            fnClearInputFields();

            $("#drpVillage").val("0");
            $("#drpLP").val("0");

        });       //doc ready

        function fnResetUI() {

            var cusLen = $('#tblClients').find(".custName").length;

            //get all valid values
            for (ctr = 0; ctr < cusLen; ctr++) {

                var cli = $("#tblClients tbody tr:eq(" + ctr + ")  td:eq(1)").find(".ui-autocomplete-input").val();
                var clival = $("#tblClients tbody tr:eq(" + ctr + ") .custName option:contains(" + cli + ")").val();


                if (clival > 0) {

                    //delete all used client values from template
                    $("#template #drpClients option[value='" + clival + "']").remove();

                    //delete all used client values from template
                    $("#tblClients .custName option[value='" + clival + "']").remove();

                }

            } //for

            //clear input fields                
            fnClearInputFields();

        }

        function fnSaveLAF() {

            $("#modalNot").html("Saving form&nbsp;<img src='/Images/Resources/ajax-loader.gif' />");

            //declare json array
            var ClientData = {
                clients: [] //don't change the name. coupled with class
            };

            var cusLen = $('#tblClients').find(".custName").length;
            
            //get all valid values
            for (ctr = 1; ctr <= cusLen; ctr++) {

                var cli = $("#tblClients tbody tr:eq(" + ctr + ")  td:eq(1)").find(".ui-autocomplete-input").val();
                var clival = $("#tblClients tbody tr:eq(" + ctr + ") .custName option:contains(" + cli + ")").val();
                
                if (clival > 0) {

                    var amnt = $("#tblClients tbody tr:eq(" + ctr + ")").find(".amount").val();

                    var lp = $("#tblClients tbody tr:eq(" + ctr + ") td:eq(3)").find(".ui-autocomplete-input").val();
                    var lpval = $("#tblClients tbody tr:eq(" + ctr + ") .loanPurpose option:contains(" + lp + ")").val();

                    var lc = $("#tblClients tbody tr:eq(" + ctr + ")").find(".cycle").val();

                    ClientData.clients.push({
                        "CliID": clival,
                        "Amnt": amnt,
                        "PurpID": lpval,
                        "Cycle": lc
                    });

                }

            } //FOR
            
            $.ajax({
                type: "POST",
                url: "/Ajax/loan.aspx",
                data: "AjaxMethod=laf" +
                        "&Vil=" + $("#drpVillage option:selected").val() +
                        "&Cen=" + $("#drpCenter option:selected").val() +
                        "&FE=" + $("#drpFE option:selected").val() +
                        "&LP=" + $("#drpLP option:selected").val() +
                        "&Client=" + JSON.stringify(ClientData),
                success: function (returnResponse) {

                    var element = jQuery(returnResponse).find("#hdnLUK");

                    if (element.length == 1) {
                        window.location = "/login?rr=se";
                    }
                    else {

                        var responseStatus = returnResponse.split('_')[0];
                        var responseMsg = returnResponse.split('_')[1];
                        var errorID = returnResponse.split('_')[2];

                        $("#btnSaveLAF").removeClass("disabled").hide();
                        $("#btnCancel").removeClass("disabled").text("Ok");

                        if (responseStatus == "Error") {

                            if (responseMsg == "Default") {

                                $("#modalNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                                $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");
                            }
                            else if (responseMsg == "Process") {
                                $("#modalNot").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                                $("#frmAlert").append("Sorry, encountered an error while processing. Please contact system administrator with the Error Code: #0" + errorID);
                            }
                        }
                        else if (responseStatus == "Success") {
                            $("#modalNot").html("<div id='frmAlert' class='alert alert-block alert-success'></div>");
                            $("#frmAlert").append("<strong>Success!</strong>Loan Application form has been saved. The clients have been put in a new <a href='/groups?gid=" + responseMsg + "'>Group " + responseMsg + "</a>");

                            fnResetUI();
                        }

                        $("html, body").animate({ scrollTop: 0 }, "fast");
                    }
                }
            }); //ajax
        }

        function fnClearInputFields() {
            $("#tblClients tbody").find(".ui-autocomplete-input").val("");
            $("#tblClients tbody .amount").val("");
            $("#tblClients tbody .cycle").val("");
            $(".gloAmnt").val("");
        }

        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        function fnVal() {
            var vilSel = false;
            var cenSel = false;
            var feSel = false;
            var lpSel = false;

            var clientSelected = false;
            var amountCorrect = false;
            var loanPurpCorrect = false;
            var uniqueClients = false;

            var LeaderSel = false; // tp check if group leader has been selected.

            var errorMsgs = new Array();


            //check for filters
            if ($("#drpVillage option:selected").val() > 0)
                vilSel = true;

            if ($("#drpCenter option:selected").val() > 0)
                cenSel = true;

            if ($("#drpFE option:selected").val() > 0)
                feSel = true;

            if ($("#drpLP option:selected").val() > 0)
                lpSel = true;


            //check for clients
            var cliCtr = 0;
            var amntCtr = 0;
            var lpCtr = 0;

            var clients = new Array();

            var cusLen = $('#tblClients').find(".custName").length;

            for (ctr = 1; ctr <= cusLen; ctr++) {

                var cli = $("#tblClients tbody tr:eq(" + ctr + ")  td:eq(1)").find(".ui-autocomplete-input").val();
                var clival = $("#tblClients tbody tr:eq(" + ctr + ") .custName option:contains(" + cli + ")").val();

                if (ctr == 1 && clival > 0) {
                    LeaderSel = true;
                }

                if (clival > 0) {
                    clients.push(clival);
                    cliCtr++;

                    var amnt = $("#tblClients tbody tr:eq(" + ctr + ")").find(".amount").val();
                    if (isNumber(amnt) && amnt > 0)
                        amntCtr++;

                    var lp = $("#tblClients tbody tr:eq(" + ctr + ") td:eq(3)").find(".ui-autocomplete-input").val();
                    var lpval = $("#tblClients tbody tr:eq(" + ctr + ") .loanPurpose option:contains(" + lp + ")").val();

                    if (lpval > 0)
                        lpCtr++;
                }

            }

            if (cliCtr > 0) {
                //some clients have been selected. so check for other val criteria
                clientSelected = true;

                if (cliCtr == amntCtr)
                    amountCorrect = true;

                if (cliCtr == lpCtr)
                    loanPurpCorrect = true;


                var sorted_arr = clients.sort();
                var duplicates = [];
                for (var i = 0; i < clients.length - 1; i++) {
                    if (sorted_arr[i + 1] == sorted_arr[i]) {
                        duplicates.push(sorted_arr[i]);
                    }
                }

                if (duplicates.length == 0)
                    uniqueClients = true;
            }

            //consolidate error msgs
            if (!vilSel)
                errorMsgs.push("Please select a Village");
            if (!cenSel)
                errorMsgs.push("Please select a Center");
            if (!feSel)
                errorMsgs.push("Please select a Field Executive");
            if (!lpSel)
                errorMsgs.push("Please select a Loan Product");

            if (!clientSelected)
                errorMsgs.push("Please select some clients");
            if (!amountCorrect)
                errorMsgs.push("Please check if the loan amount is valid for all selected clients");
            if (!loanPurpCorrect)
                errorMsgs.push("Please select loan purpose for all clients");
            if (!uniqueClients)
                errorMsgs.push("Please select unique clients. You have selected some clients more than once.");

            if (!LeaderSel)
                errorMsgs.push("You must select a Group Leader.");

            return errorMsgs;

        }
	</script>
    <script type="text/javascript" src="/JQuery/jquery-ui-1.8.21.custom/development-bundle/ui/jquery-ui-1.8.21.custom.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Loan Application Form</div>
            <div id="locNotHO" class="span12">
            </div>
            <div class="span12 laf-filt-wrap">
                <table class="laf-tbl">
                    <tr>
                        <td class="lbl">
                            Select Village:
                        </td>
                        <td class="vil" id="drpVillageWrap" runat="server">
                        </td>
                        <td class="lbl">
                            Select Center:
                        </td>
                        <td class="drpCenter" id="drpCenterWrap" runat="server">
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">
                            Field Executive:
                        </td>
                        <td class="drpFE" id="drpFeWrap" runat="server">
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">
                            Loan Product:
                        </td>
                        <td class="drpProd" id="drpLpWrap" runat="server">
                        </td>
                        <td colspan="2">
                            <table class="table table-condensed table-bordered lp-tbl" id="lpDetails">
                                <thead>
                                    <tr>
                                        <th>
                                            Loan Product
                                        </th>
                                        <th>
                                            Max Amount (Rs)
                                        </th>
                                        <th>
                                            Interest (%)
                                        </th>
                                        <th>
                                            Tenure (months)
                                        </th>
                                    </tr>
                                </thead>
                                <tbody id="lpDetailsBody" runat="server">
                                </tbody>
                            </table>
                            <table class="table table-condensed table-bordered lc-tbl" id="lpCycles">
                                <thead>
                                    <tr>
                                        <th>
                                            Loan Cycle
                                        </th>
                                        <th>
                                            Min Amount (Rs)
                                        </th>
                                        <th>
                                            Max Amount (Rs)
                                        </th>
                                    </tr>
                                </thead>
                                <tbody id="lcBody" runat="server">
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="span12">
                <table class="table table-bordered laf-tbl-cli" id="tblClients">
                    <thead>
                        <tr>
                            <th>
                                S.No
                            </th>
                            <th>
                                Client
                            </th>
                            <th>
                                Loan Amount (Rs)
                            </th>
                            <th style="width: 200px;">
                                Loan Purpose
                            </th>
                            <%--<th>
                                Loan Cycle
                            </th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="laf-template">
                            <td colspan="2" class='laf-template-info'>
                                Amount entered in this row will be copied to all clients&nbsp; <i class='icon-hand-right'>
                                </i>
                            </td>
                            <td><input type='text' class='ldisb-amnt gloAmnt' /></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="sno">
                                1
                            </td>
                            <td>
                                <div class="laf-gl">
                                    Group Leader:</div>
                                <select class="custName name">
                                </select>
                            </td>
                            <td>
                                <div class="laf-gl-fx">
                                    &nbsp;</div>
                                <input type="text" class="span2 amount ldisb-amnt" />
                            </td>
                            <td class="drpPurpose">
                                <div class="laf-gl-fx">
                                    &nbsp;</div>
                                <select class="loanPurpose lp">
                                </select>
                            </td>
                            <%--<td>
                                <div class="laf-gl-fx">
                                    &nbsp;</div>
                                <input type="text" class=" span1 cycle" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="sno">
                                2
                            </td>
                            <td>
                                <select class="custName name">
                                </select>
                            </td>
                            <td>
                                <input type="text" class="span2 amount ldisb-amnt" />
                            </td>
                            <td class="drpPurpose">
                                <select class="loanPurpose lp">
                                </select>
                            </td>
                            <%--                            <td>
                                <input type="text" class=" span1 cycle" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="sno">
                                3
                            </td>
                            <td>
                                <select class="custName name">
                                </select>
                            </td>
                            <td>
                                <input type="text" class="span2 amount ldisb-amnt" />
                            </td>
                            <td class="drpPurpose">
                                <select class="loanPurpose lp">
                                </select>
                            </td>
                            <%--<td>
                                <input type="text" class=" span1 cycle" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="sno">
                                4
                            </td>
                            <td>
                                <select class="custName name">
                                </select>
                            </td>
                            <td>
                                <input type="text" class="span2 amount ldisb-amnt" />
                            </td>
                            <td class="drpPurpose">
                                <select class="loanPurpose lp">
                                </select>
                            </td>
                            <%--<td>
                                <input type="text" class=" span1 cycle" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="sno">
                                5
                            </td>
                            <td>
                                <select class="custName name">
                                </select>
                            </td>
                            <td>
                                <input type="text" class="span2 amount ldisb-amnt" />
                            </td>
                            <td class="drpPurpose">
                                <select class="loanPurpose lp">
                                </select>
                            </td>
                            <%--<td>
                                <input type="text" class=" span1 cycle" />
                            </td>--%>
                        </tr>
                        <tr id="clientLast">
                            <td colspan="5">
                                <a class="btn btn-primary btn-small pull-right" id="btnAddClient" onclick="return false;">
                                    <i class="icon-plus icon-white"></i>&nbsp; Add Another Client</a>
                            </td>
                        </tr>
                        <tr id="template" style="display: none">
                            <td class="sno">
                            </td>
                            <td id="drpCliWrap" runat="server">
                            </td>
                            <td>
                                <input type="text" class="span2 amount ldisb-amnt" />
                            </td>
                            <td class="drpPurpose" id="drpLoanPurposeWrap" runat="server">
                            </td>
                            <%--<td>
                                <input type="text" class="span1 cycle" />
                            </td>--%>
                        </tr>
                    </tbody>
                </table>
                <div class="laf-save-btn">
                    <a id="btnSendApproval" class="btn btn-large btn-success pull-right"><i class="icon-white icon-check">
                    </i>&nbsp; Send For Approval </a>
                </div>
            </div>
        </div>
        <div class="modal hide" id="modalLAF">
            <div class="modal-header">
                <%--<button type="button" class="close" data-dismiss="modal">
                    ×</button>--%>
                <h3>
                    Loan Application Form
                </h3>
            </div>
            <div class="modal-body of-modal-body">
                <div class="laf-mod-bod" id="modalNot">
                </div>
            </div>
            <div class="modal-footer">
                <div>
                    <a class="btn btn-success" id="btnSaveLAF"><i class="icon-ok icon-white"></i>&nbsp;Yes
                    </a><a class="btn" id="btnCancel">Cancel</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
