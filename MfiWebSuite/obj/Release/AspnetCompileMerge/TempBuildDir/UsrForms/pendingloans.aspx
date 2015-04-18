<%@ Page Title="Pending Loans" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true"
    CodeBehind="pendingloans.aspx.cs" Inherits="MfiWebSuite.UsrForms.pendingloans" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../DataTable/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <link href="../Styles/pendingloans.css" rel="stylesheet" />
    <script src="../Scripts/pendingloans.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Loans Pending Disbursement &nbsp;<span class="badge badge-important lpen-count" id="LoanCount"
                    runat="server">0</span>
            </div>
            <div id="locNotHO" class="span12">
            </div>
            <div class="lpen-tbl" id="PendingLoans" runat="server">
            </div>
        </div>
    </div>
</asp:Content>
