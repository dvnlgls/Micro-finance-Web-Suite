<%@ Page Title="Loan Product" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="loanproduct.aspx.cs" Inherits="MfiWebSuite.UsrForms.loanproduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/loanproduct.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content" id="uxLpContent" runat="server">
            <div class="gen-title">
                Loan Product(s)</div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="lp-add-wrap" runat="server" id="uxAddProduct">
                <a id="btnAddProduct" class="btn btn-primary"><i class="icon-white icon-plus"></i>&nbsp;Create
                    New Loan Product </a>
            </div>
            <div class="lp-list" id="uxLP" runat="server">
                <table class="table table-bordered lp-product-tbl" id="uxGroupTbl">
                    <thead>
                        <tr>
                            <th>
                                Product Name
                            </th>
                            <th>
                                Max.Amount (Rs.)
                            </th>
                            <th>
                                Interest Rate (%)
                            </th>
                            <th>
                                Tenure (months)
                            </th>
                            <th>
                                Fund Source
                            </th>
                            <th>
                                Repayment Type
                            </th>
                            <th>
                                Created On
                            </th>
                            <th class="options">
                                <i class='icon-cog'></i>&nbsp;Options
                            </th>
                        </tr>
                    </thead>
                    <tbody id="uxLpBody" runat="server">
                    </tbody>
                </table>
            </div>
            <div class="modal hide" id="modalDel">
                <div class="modal-footer">
                    <div class="of-del-modal">
                        <table>
                            <tr>
                                <td class="of-del-modal-txt">
                                    <span id="spnDelTxt"></span>
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
            <div class="modal hide" id="modalAddProduct">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        ×</button>
                    <h3>
                        Create New Loan Product
                    </h3>
                </div>
                <div class="modal-body of-modal-body">
                    <div class="of-modal-body-not" id="modalNot">
                    </div>
                    <div id="divAddOfc" class="lp-add-tbl-wrap">
                        <table class="table table-bordered lp-add-tbl">
                            <tr>
                                <td class="lbl">
                                    Product Name
                                </td>
                                <td>
                                    <input type="text" id="uxProductName" name="txtName" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td class="lbl">
                                    Max. Amount (Rs.)
                                </td>
                                <td>
                                    <input type="text" id="uxMaxAmnt" class="span2 ldisb-amnt" />
                                </td>
                            </tr>
                            <tr>
                                <td class="lbl">
                                    Interest Rate (%)
                                </td>
                                <td>
                                    <input type="text" id="uxIntRate" class="input-mini" />
                                </td>
                            </tr>
                            <tr>
                                <td class="lbl">
                                    Tenure (months)
                                </td>
                                <td>
                                    <input type="text" id="uxTenure" class="input-mini" />
                                </td>
                            </tr>
                            <tr>
                                <td class="lbl">
                                    Fund Source
                                </td>
                                <td id="uxFundSource" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td class="lbl">
                                    Repayment Type
                                </td>
                                <td>
                                    <input type="radio" name="uxReptype" value="1" />
                                    Monthly&nbsp;&nbsp;
                                    <input type="radio" name="uxReptype" value="2" />
                                    <input type="text" class="span1" id="uxRepDays" />
                                    Days
                                </td>
                            </tr>
                            <tr id="LcRange_1">
                                <td class="lbl">
                                    Loan Cycle 1
                                </td>
                                <td>
                                    Min(Rs)
                                    <input type="text" class="span1" id="uxLcMin_1" />&nbsp;&nbsp; Max(Rs)
                                    <input type="text" class="span1" id="uxLcMax_1" />
                                </td>
                            </tr>
                            <tr id="LcRange_2">
                                <td class="lbl">
                                    Loan Cycle 2
                                </td>
                                <td>
                                    Min(Rs)
                                    <input type="text" class="span1" id="uxLcMin_2" />&nbsp;&nbsp; Max(Rs)
                                    <input type="text" class="span1" id="uxLcMax_2" />
                                </td>
                            </tr>
                            <tr id="LcRange_3">
                                <td class="lbl">
                                    Loan Cycle 3
                                </td>
                                <td>
                                    Min(Rs)
                                    <input type="text" class="span1" id="uxLcMin_3" />&nbsp;&nbsp; Max(Rs)
                                    <input type="text" class="span1" id="uxLcMax_3" />
                                </td>
                            </tr>
                            <tr id="LcRange_4">
                                <td class="lbl">
                                    Loan Cycle 4
                                </td>
                                <td>
                                    Min(Rs)
                                    <input type="text" class="span1" id="uxLcMin_4" />&nbsp;&nbsp; Max(Rs)
                                    <input type="text" class="span1" id="uxLcMax_4" />
                                </td>
                            </tr>
                            <tr id="LcRange_5">
                                <td class="lbl">
                                    Loan Cycle 5
                                </td>
                                <td>
                                    Min(Rs)
                                    <input type="text" class="span1" id="uxLcMin_5" />&nbsp;&nbsp; Max(Rs)
                                    <input type="text" class="span1" id="uxLcMax_5" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <div>
                        <a class="btn btn-success" id="btnAddOK"><i class="icon-ok icon-white"></i>&nbsp;Create
                        </a><a class="btn" id="btnCancelAddProduct">Cancel</a>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnLPID" />
    </div>
</asp:Content>
