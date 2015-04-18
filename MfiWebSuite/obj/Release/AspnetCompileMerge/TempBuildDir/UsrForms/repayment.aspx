<%@ Page Title="Loan Repayment" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="repayment.aspx.cs" Inherits="MfiWebSuite.UsrForms.repayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="../Scripts/repayment.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Loan Repayment Form
            </div>
            <div id="locNotHO">
            </div>
            <div class="grp-filters">
                <table class="table table-bordered rep-tbl">
                    <tbody>
                        <tr>
                            <td class="lbl">Center
                            </td>
                            <td id="uxDrpCenterWrap" runat="server"></td>
                            <td class="lbl">Planned Meeting Date
                            </td>
                            <td>
                                <select class='span2' id="uxMeetingDate">
                                </select>
                            </td>
                            <td class="lbl">Collection Date
                            </td>
                            <td>
                                <input id="uxCollectionDate" readonly='readonly' type='text' class='span2' />
                            </td>
                            <td>
                                <a id="btnShowRepaymentForm" class="btn btn-success pull-left">Show Repayment Form</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="repfrm-tbl" id="uxRepayWrap" runat="server">
                <table class="table table-condensed table-bordered">
                    <thead>
                        <tr>
                            <th rowspan="3">Loan ID
                            </th>
                            <th rowspan="3">Client ID
                            </th>
                            <th rowspan="3">Client Name
                            </th>
                            <th rowspan="3">Loan Product
                            </th>
                            <th rowspan="3">Inst. No
                            </th>
                            <th colspan="4" class="gen-center-txt">Demand
                            </th>
                            <th rowspan="3">Total Due
                            </th>
                            <th colspan="4" class="gen-center-txt">Collection
                            </th>
                            <th rowspan="3">Total Collection
                            </th>
                            <th rowspan="3">Receipt No.
                            </th>
                        </tr>
                        <tr>
                            <th colspan="2" class="gen-center-txt">Over Due
                            </th>
                            <th colspan="2" class="gen-center-txt">Due This Month
                            </th>
                            <th colspan="2" class="gen-center-txt">Over Due
                            </th>
                            <th colspan="2" class="gen-center-txt">Due This Month
                            </th>
                        </tr>
                        <tr>
                            <th>Interest
                            </th>
                            <th>Principal
                            </th>
                            <th>Interest
                            </th>
                            <th>Principal
                            </th>

                            <th>Interest
                            </th>
                            <th>Principal
                            </th>
                            <th>Interest
                            </th>
                            <th>Principal
                            </th>
                        </tr>
                    </thead>
                    <tbody id="uxRepBody">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="modal hide" id="modalRep">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal">
                ×</button>
            <h3>Save Repayment Form
            </h3>
        </div>
        <div class="modal-body of-modal-body">
            <div class="laf-mod-bod" id="modalRepNot">
            </div>
        </div>
        <div class="modal-footer">
            <div>
                <a class="btn btn-success" id="btnOkRep"><i class="icon-ok icon-white"></i>&nbsp;Yes
                </a><a class="btn" id="btnCancelRep">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>
