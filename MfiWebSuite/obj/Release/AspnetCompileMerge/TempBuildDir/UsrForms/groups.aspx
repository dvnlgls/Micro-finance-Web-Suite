<%@ Page Title="Groups" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="groups.aspx.cs" Inherits="MfiWebSuite.UsrForms.groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/groups.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content" id="uxCentContent" runat="server">
            <div class="gen-title">
                Groups
            </div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="grp-filters">
                <table class="table table-bordered grp-tbl">
                    <tbody>
                        <tr>
                            <td class="lbl">Branch
                            </td>
                            <td id="uxDrpBranchWrap" runat="server"></td>
                            <td class="lbl">Village
                            </td>
                            <td id="uxDrpVillageWrap" runat="server"></td>
                            <td class="lbl">Center
                            </td>
                            <td id="uxDrpCenterWrap" runat="server"></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="grp-list" id="uxGroups" runat="server">
                <table class="table table-bordered ho-off-tbl" id="uxGroupTbl">
                    <thead>
                        <tr>
                            <th>Group
                            </th>
                            <th>Group Status
                            </th>
                            <th>Group Leader
                            </th>
                            <th>Created By
                            </th>
                            <th>Created Date
                            </th>
                            <th>Center
                            </th>
                            <th>Village
                            </th>
                            <th>Branch
                            </th>
                            <th>
                                <i class='icon-cog'></i>&nbsp;Options
                            </th>
                        </tr>
                    </thead>
                    <tbody id="uxGroupBody" runat="server">
                    </tbody>
                </table>
            </div>
            <div class="modal hide" id="modalDel">
                <div class="modal-footer">
                    <div class="of-del-modal">
                        <table>
                            <tr>
                                <td class="of-del-modal-txt">
                                    <span id="spnDelTxt">Do you really want to delete this Group?</span>
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
            <table class="hide">
                <tbody id="uxGCBody" runat="server">
                </tbody>
            </table>
        </div>
        <input type="hidden" id="hdnGID" />
    </div>
</asp:Content>
