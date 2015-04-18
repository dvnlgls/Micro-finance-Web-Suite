<%@ Page Title="MWS - Dev" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="dev.aspx.cs" Inherits="MfiWebSuite.Dev.dev" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(function () {

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row span6">
        <div class="span6">
            <label>
                Email</label>
            <input type="text" id="txtEmailID" runat="server" />
        </div>
        <div class="span6">
            <label>
                Password</label>
            <input type="text" id="txtPwd" runat="server" />
        </div>
        <div class="span6">
            <input type="button" id="btnAccount" class="btn btn-primary" value="Create Account"
                runat="server" onserverclick="btnAccCreate_Click" />
        </div>
        <div class="span6">
            <h3>
                Salt:</h3>
            <div id="spnSalt" runat="server" style="width:1000px; word-wrap:break-word;background-color: #B94A48; color:White; padding:10px;"></div>
            <br />
            <br />
            <h3>Password:</h3>
            <div id="spnPwd" runat="server" style="width:1000px; word-wrap:break-word;background-color: #B94A48;color:White; padding:10px;"></div>
            
        </div>
    </div>
    
</asp:Content>
