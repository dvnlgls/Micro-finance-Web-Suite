<%@ Page Title="Loan Application Form" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="loanappform.aspx.cs" Inherits="MfiWebSuite.UsrForms.loanappform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/loanappform.css" rel="stylesheet" />
    <script src="../Scripts/json2.js" type="text/javascript"></script>
    <script src="../Scripts/loanappform.js"></script>
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
