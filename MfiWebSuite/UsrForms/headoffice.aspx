<%@ Page Title="Head Office" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="headoffice.aspx.cs" Inherits="MfiWebSuite.UsrForms.headoffice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/headoffice.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content">
            <div class="gen-title">
                Head Office
            </div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="ho-details" id="viewHO" runat="server">
                <div class="ho-title">
                    Office Details
                </div>

                <table class="table ho-tbl">
                    <tr>
                        <td class="lbl">Name
                        </td>
                        <td>
                            <span class="orig" id="spnName" runat="server"></span>
                            <input type="text" id="txtName" name="txtName" class="span3 edit" />
                            <span class="gen-err-1" id="errName"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">Address
                        </td>
                        <td>
                            <span id="spnAddr" runat="server" class="orig"></span>
                            <textarea id="txtAddr" name="txtAddr" rows="3" cols="2" class="edit"></textarea>
                            <span class="gen-err-1" id="errAddr"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">Phone 1
                        </td>
                        <td>
                            <span id="spnPh1" runat="server" class="orig"></span>
                            <input type="text" id="txtPh1" name="txtPh1" class="span3 edit" />
                            <span class="gen-err-1" id="errPh1"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">Phone 2
                        </td>
                        <td>
                            <span id="spnPh2" runat="server" class="orig"></span>
                            <input type="text" id="txtPh2" name="txtPh2" class="span3 edit" />
                            <span class="gen-err-1" id="errPh2"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">Fax
                        </td>
                        <td>
                            <span id="spnFax" runat="server" class="orig"></span>
                            <input type="text" id="txtFax" name="txtFax" class="span3 edit" />
                            <span class="gen-err-1" id="errFax"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">Website
                        </td>
                        <td>
                            <span id="spnWeb" runat="server" class="orig"></span>
                            <input type="text" id="txtWeb" name="txtWeb" class="span3 edit" />
                            <span class="gen-err-1" id="errWeb"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lbl">Email-ID
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
                        <a class="btn btn-primary" id="btnEdit" onclick="return false;">
                            <i class="icon-pencil icon-white"></i>&nbsp; Edit</a>
                        <a class="btn btn-success gen-nodisplay" id="btnSave">
                            <i class="icon-ok icon-white"></i>&nbsp;Save
                        </a>
                        <a class="btn gen-nodisplay" id="btnCancel" onclick="return false;">Cancel</a>
                    </div>
                </div>
            </div>
            <div class="ho-off" id="viewRO" runat="server">
                <div class="ho-title">
                    Regional Offices under this office
                </div>
                <table class="table table-bordered ho-off-tbl">
                    <thead>
                        <tr>
                            <th class="off">Regional Office
                            </th>
                            <th class="addr">Address
                            </th>
                            <th class="ph">Phone
                            </th>
                        </tr>
                    </thead>
                    <tbody id="uxSubOfficeTblBody" runat="server">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
