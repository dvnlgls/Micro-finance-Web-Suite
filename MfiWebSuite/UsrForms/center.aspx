<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true"
    CodeBehind="center.aspx.cs" Inherits="MfiWebSuite.UsrForms.center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/center.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Center
            </div>
            <div id="locNotHO" class="span12">
            </div>
            <div id="uxCenterWrap" runat="server" class="span12">
                <div class="ho-details">
                    <div class="ho-title">
                        Center Details
                    </div>
                    <table class="table ho-tbl">
                        <tr>
                            <td class="lbl">Name
                            </td>
                            <td>
                                <span id="uxCenterName" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Meeting Location
                            </td>
                            <td>
                                <input type="text" class="cmEditable" id="uxMeetingLoc" runat="server" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Meeting Time
                            </td>
                            <td>
                                <input type="text" id="uxMeetingTime" class="cmEditable input-mini" runat="server"
                                    disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Field Executive
                            </td>
                            <td id="uxFE" runat="server"></td>
                        </tr>
                        <tr>
                            <td class="lbl">Created On
                            </td>
                            <td>
                                <span id="uxCreatedOn" runat="server"></span>
                            </td>
                        </tr>
                    </table>
                    <div id="uxEditCenter" runat="server">
                        <div class="ho-btn-wrap">
                            <a class="btn btn-primary" id="btnEdit"><i class="icon-pencil icon-white"></i>&nbsp;
                                Edit</a> <a class="btn btn-success gen-nodisplay" id="btnSave"><i class="icon-ok icon-white"></i>Save </a><a class="btn gen-nodisplay" id="btnCancel">Cancel</a>
                        </div>
                    </div>
                </div>
                <div class="cent-grp-det" id="uxGroups" runat="server">
                    <div class="ho-title">
                        Groups under this Center
                    </div>
                    <table class="table table-bordered ho-off-tbl">
                        <thead>
                            <tr>
                                <th>Group
                                </th>
                                <th>Group Status
                                </th>
                                <th>Created By
                                </th>
                                <th>Created Date
                                </th>
                            </tr>
                        </thead>
                        <tbody id="uxGroupBody" runat="server">
                        </tbody>
                    </table>
                </div>
            </div>
            <input type="hidden" id="hdnOid" runat="server" />
            <input type="hidden" id="hdnCL" />
            <input type="hidden" id="hdnCT" />
            <input type="hidden" id="hdnFE" />
        </div>
    </div>
</asp:Content>
