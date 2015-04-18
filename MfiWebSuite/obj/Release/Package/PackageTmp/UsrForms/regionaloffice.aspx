<%@ Page Title="Regional Office" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="regionaloffice.aspx.cs" Inherits="MfiWebSuite.UsrForms.regionaloffice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/regionaloffice.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content" id="uxOffcontent" runat="server">
            <div class="gen-title">
                Regional Office
            </div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="ho-details" id="viewRO" runat="server">
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
                        <a class="btn btn-primary" id="btnEdit">
                            <i class="icon-pencil icon-white"></i>&nbsp; Edit</a>
                        <a class="btn btn-success gen-nodisplay" id="btnSave">
                            <i class="icon-ok icon-white"></i>Save
                        </a>
                        <a class="btn gen-nodisplay" id="btnCancel">Cancel</a>
                    </div>
                </div>
            </div>
            <div class="ho-off" id="viewAO" runat="server">
                <div class="ho-title">
                    Area Offices under this office
                </div>
                <table class="table table-bordered ho-off-tbl">
                    <thead>
                        <tr>
                            <th class="off">Area Office
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
        <input type="hidden" id="hdnOid" runat="server" />
    </div>

</asp:Content>
