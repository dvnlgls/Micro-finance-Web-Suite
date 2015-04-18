<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true"
    CodeBehind="test.aspx.cs" Inherits="MfiWebSuite.test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    wlecome!!
    <tr>
        <td class="lbl">
            Loan Cycle 5
        </td>
        <td>
            Min(Rs)
            <input type="text" class="input-mini" id="uxLcMin5" />&nbsp;&nbsp; Max(Rs)
            <input type="text" class="input-mini" id="uxLcMax5" />
        </td>
    </tr>
</asp:Content>
