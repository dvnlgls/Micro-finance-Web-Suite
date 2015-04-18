<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true"
    CodeBehind="report.aspx.cs" Inherits="MfiWebSuite.UsrForms.report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.mtz.monthpicker.js" type="text/javascript"></script>
    <script src="../Scripts/report.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                <span id="uxReportTitle" runat="server"></span>
            </div>
            <div id="locNotHO">
            </div>
            <div class="report-filter-wrap">
                <ul class="report-filter-row">
                    <li runat="server" class="span1" id="uxLblBranch"><strong>Branch</strong></li>
                    <li id="uxBranchWrap" runat="server"></li>
                    <li runat="server" class="span1" id="uxLblVillage"><strong>Village</strong></li>
                    <li id="uxVillageWrap" runat="server"></li>
                    <li runat="server" class="span1" id="uxLblCenter"><strong>Center</strong></li>
                    <li id="uxCenterWrap" runat="server"></li>
                    <li runat="server" class="span1" id="uxLblFe"><strong>FE</strong></li>
                    <li id="uxFEWrap" runat="server"></li>
                    <li runat="server" class="span1" id="uxLblLP"><strong>Loan Product</strong></li>
                    <li id="uxLPWrap" runat="server"></li>
                </ul>
                <ul class="report-filter-row">
                    <li runat="server" class="span1" id="uxLblAsOn"><strong>As On</strong></li>
                    <li id="uxAsOnWrap" runat="server">
                        <input id="uxAsOnDate" readonly='readonly' type='text' class='span2' />
                    </li>
                    <li runat="server" class="span1" id="uxLblFrom"><strong>From Date</strong></li>
                    <li id="uxFromWrap" runat="server">
                        <input id="uxFromDate" readonly='readonly' type='text' class='span2' />
                    </li>
                    <li runat="server" class="span1" id="uxLblTo"><strong>To Date</strong></li>
                    <li id="uxToWrap" runat="server">
                        <input id="uxToDate" readonly='readonly' type='text' class='span2' />
                    </li>
                    <li runat="server" class="span1" id="uxLblMonth"><strong>Month</strong></li>
                    <li id="uxMonthPickerWrap" runat="server">
                        <input id="uxMonthPicker" readonly='readonly' type='text' class='span2' />
                    </li>
                    <li class="btnWrap"><a id="btnShowReport" class="btn btn-primary">Show Report</a>
                    </li>
                </ul>
            </div>
            <div class="report-wrap" id="uxReportTblWrap">
            </div>
        </div>
    </div>
    <input type="hidden" id="hdnRID" runat="server" />
</asp:Content>
