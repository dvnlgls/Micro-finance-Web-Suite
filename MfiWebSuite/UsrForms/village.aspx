<%@ Page Title="Village" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="village.aspx.cs" Inherits="MfiWebSuite.UsrForms.village" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../DataTable/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <link href="../Styles/village.css" rel="stylesheet" />
    <script src="../Scripts/village.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content" id="uxOffcontent" runat="server">
            <div class="gen-title">
                Village
            </div>
            <div id="locNotHO" class="gen-not-1">
            </div>
            <div class="vil-det-wrap">
                <div class="ho-details" id="viewRO" runat="server">
                    <div class="ho-title">
                        Village Details
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
                    </table>
                    <div id="editOff" runat="server">
                        <div class="ho-btn-wrap">
                            <a class="btn btn-primary" id="btnEdit"><i class="icon-pencil icon-white"></i>&nbsp;
                                Edit</a> <a class="btn btn-success gen-nodisplay" id="btnSave"><i class="icon-ok icon-white"></i>Save </a><a class="btn gen-nodisplay" id="btnCancel">Cancel</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="vil-tbl-wrap">
                <div class="vil-cent-lst" id="viewSubOff" runat="server">
                    <div class="ho-title">
                        Centers under this office
                    </div>
                    <div class="" id="uxCenterList" runat="server">
                    </div>
                </div>
            </div>
        </div>
        <div class="modal hide" id="modalAddCenter">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">
                    ×</button>
                <h3>Create New Center
                </h3>
            </div>
            <div class="modal-body of-modal-body">
                <div class="of-modal-body-not" id="modalNot">
                </div>
                <div id="divAddOfc" class="of-add-wrap">
                    <table class="table ho-add-tbl">
                        <tr>
                            <td class="lbl">Meeting Location
                            </td>
                            <td>
                                <input type="text" id="txtCenterLocation" name="txtName" class="span3 edit" />
                                <span class="gen-err-1" id="Span1"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Meeting Time
                            </td>
                            <td>
                                <input type="text" id="uxMeetingTime" class="input-mini" />
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl">Field Executive
                            </td>
                            <td id="FE" runat="server"></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="modal-footer">
                <div>
                    <a class="btn btn-success" id="btnCreateCenter"><i class="icon-ok icon-white"></i>&nbsp;Create
                    </a><a class="btn" id="btnCancelCenter">Cancel</a>
                </div>
            </div>
        </div>
        <div class="modal hide" id="modalDel">
            <div class="modal-footer">
                <div class="of-del-modal">
                    <table>
                        <tr>
                            <td class="of-del-modal-txt">
                                <span id="spnDelTxt">Do you really want to delete this Center?</span>
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
        <input type="hidden" id="hdnOid" runat="server" />
        <input type="hidden" id="hdnAcD" runat="server" />
        <input type="hidden" id="hdnCID" />
    </div>
</asp:Content>
