<%@ Page Title="Disburse Loan" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="disburseloan.aspx.cs" Inherits="MfiWebSuite.UsrForms.disburseloan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/json2.js" type="text/javascript"></script>
    <script src="../Scripts/disburseloan.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Loan Disbursement
            </div>
            <div id="locNotHO" class="span12">
            </div>
            <div class="span12" id="DisburseLoan" runat="server">
                <div class="ldisb-info">
                    <table class="table table-bordered ldisb-inf-tbl">
                        <tr>
                            <td class="lbl">Group
                            </td>
                            <td>
                                <span id="uxGroupID" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Applied On
                            </td>
                            <td>
                                <span id="uxAppliedOn" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Field Executive
                            </td>
                            <td>
                                <span id="uxFE" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Group Hierarchy
                            </td>
                            <td>
                                <span id="uxHierarchy" runat="server"></span>
                            </td>
                        </tr>
                    </table>
                    <table class="table table-condensed table-bordered ldisb-lp-tbl" id="uxLpDetails">
                        <thead>
                            <tr>
                                <th>Loan Product
                                </th>
                                <th>Max Amount (Rs)
                                </th>
                                <th>Interest (%)
                                </th>
                                <th>Tenure (months)
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <span id="uxLP" runat="server"></span>
                                </td>
                                <td>
                                    <span id="uxLpamnt" runat="server"></span>
                                </td>
                                <td>
                                    <span id="uxLpInt" runat="server"></span>
                                </td>
                                <td>
                                    <span id="uxLpTenure" runat="server"></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="ldisb-clients">
                    <table class="table table-bordered ldisb-cli-tbl">
                        <thead>
                            <tr>
                                <th class="client">Client
                                </th>
                                <th class="lp">Loan Purpose
                                </th>
                                <th class="amnt">Amount Applied (Rs)
                                </th>
                                <th class="disb">Amount to Disburse (Rs)
                                </th>
                                <th class="date">Disbursement Date
                                </th>
                                <th class="date">1<sup>st</sup> Repayment Date
                                    <h5>(Optional)</h5>
                                </th>
                                <th class="emi">EMI (Rs)
                                    <h5>(Optional)</h5>
                                </th>
                                <th>Repayment Schedule
                                </th>
                            </tr>
                        </thead>
                        <tbody id="uxClientList" runat="server">
                        </tbody>
                    </table>
                </div>
                <div class="ldisb-btn-sec" id="divBtnSec">
                    <a id="btnDisburse" class="btn btn-large btn-success pull-right"><i class="icon-white icon-check"></i>&nbsp; Sanction & Disburse </a>
                    <%--<a id="btnViewRPS" title="View Repayment Schedule"
                        data-content="Use this feature to view Repayment Schedule of clients before disbursing loans and to adjust EMI if needed."
                        class="btn btn-large pull-right vrps"><i class="icon-calendar"></i>&nbsp; View RPS
                    </a>--%>
                    <a id="btnRejectLoan" class="btn btn-danger pull-left"><i class="icon-white icon-trash"></i>&nbsp; Reject Loan</a>
                </div>
            </div>
            <div class="modal hide" id="modalRPS">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        ×</button>
                    <h3>Loan Repayment Schedule
                    </h3>
                </div>
                <div class="modal-body">
                    <div class="" id="modalNot">
                    </div>
                    <div>
                        <table class="table table-bordered table-condensed ldisb-rps-tbl" id="uxRPS">
                            <thead>
                                <tr>
                                    <th>Inst. No
                                    </th>
                                    <th>Due Date
                                    </th>
                                    <th>Principal (Rs)
                                    </th>
                                    <th>Interest (Rs)
                                    </th>
                                    <th>Total (Rs)
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
                <%--                <div class="modal-footer">
                    <div>
                        <a class="btn btn-success" id="btnSaveLAF"><i class="icon-ok icon-white"></i>&nbsp;Yes
                        </a><a class="btn" id="btnCancel">Cancel</a>
                    </div>
                </div>--%>
            </div>
            <div class="modal hide" id="modalDisb">
                <div class="modal-header">
                    <%--<button type="button" class="close" data-dismiss="modal">
                    ×</button>--%>
                    <h3>Loan Disbursement
                    </h3>
                </div>
                <div class="modal-body of-modal-body">
                    <div class="laf-mod-bod" id="modalDisbNot">
                    </div>
                </div>
                <div class="modal-footer">
                    <div>
                        <a class="btn btn-success" id="btnDisbOk"><i class="icon-ok icon-white"></i>&nbsp;Yes
                        </a><a class="btn" id="btnCancelDisb">Cancel</a>
                    </div>
                </div>
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
            <input type="hidden" id="hdnGID" runat="server" />
            <input type="hidden" id="hdnCFTID" runat="server" />
            <input type="hidden" id="hdnCFV" runat="server" />
        </div>
    </div>
</asp:Content>
