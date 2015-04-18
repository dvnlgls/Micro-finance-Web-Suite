<%@ Page Title="Offices & Villages" Language="C#" MasterPageFile="~/MasterPages/default.Master"
    AutoEventWireup="true" CodeBehind="office.aspx.cs" Inherits="MfiWebSuite.UsrForms.office" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../DataTable/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <link href="../Styles/office.css" rel="stylesheet" />
    <script src="../Scripts/office.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="" id="gloErrHO" runat="server">
        </div>
        <div class="ho-content">
            <ul class="nav nav-tabs" id="tab" runat="server">
                <li class="active"><a href="#HO" data-toggle="tab">Head Office</a></li>
                <li><a href="#RO" data-toggle="tab">Regional Office</a></li>
                <li><a href="#AO" data-toggle="tab">Area Office</a></li>
                <li><a href="#BO" data-toggle="tab">Branch Office</a></li>
                <li><a href="#VIL" data-toggle="tab">Village</a></li>
            </ul>
            <div class="tab-content" id="tabContent" runat="server">
            </div>
            <div class="modal hide" id="modalAdd">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        ×</button>
                    <h3>Create New <span id="spnEntityName"></span>
                    </h3>
                </div>
                <div class="modal-body of-modal-body">
                    <div class="of-modal-body-not" id="modalNot">
                    </div>
                    <div id="divAddOfc" class="of-add-wrap">

                        <table class="table ho-add-tbl">
                            <tr>
                                <td class="lbl">Name
                                </td>
                                <td>
                                    <input type="text" id="txtName" name="txtName" class="span3 edit" />
                                    <span class="gen-err-1" id="errName"></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="lbl">Parent Office
                                </td>
                                <td id="drpParentMenu"></td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">Address
                                </td>
                                <td>
                                    <textarea id="txtAddr" name="txtAddr" rows="3" cols="2" class="edit"></textarea>
                                    <span class="gen-err-1" id="errAddr"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">Phone 1
                                </td>
                                <td>
                                    <input type="text" id="txtPh1" name="txtPh1" class="span3 edit" />
                                    <span class="gen-err-1" id="errPh1"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">Phone 2
                                </td>
                                <td>
                                    <input type="text" id="txtPh2" name="txtPh2" class="span3 edit" />
                                    <span class="gen-err-1" id="errPh2"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">Fax
                                </td>
                                <td>
                                    <input type="text" id="txtFax" name="txtFax" class="span3 edit" />
                                    <span class="gen-err-1" id="errFax"></span>
                                </td>
                            </tr>
                            <tr class="notvil">
                                <td class="lbl">Email-ID
                                </td>
                                <td>
                                    <input type="text" id="txtMail" name="txtMail" class="span3 edit" />
                                    <span class="gen-err-1" id="errMail"></span>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
                <div class="modal-footer">
                    <div>
                        <a class="btn btn-success" id="btnSave">
                            <i class="icon-ok icon-white"></i>&nbsp;Create
                        </a>
                        <a class="btn" id="btnCancel">Cancel</a>
                    </div>
                </div>
            </div>
            <div class="modal hide" id="modalDel">
                <div class="modal-footer">
                    <div class="of-del-modal">
                        <table>
                            <tr>
                                <td class="of-del-modal-txt"><span id="spnDelTxt">Do you really want to delete?</span>
                                </td>
                                <td class="of-del-btn">
                                    <a class="btn btn-danger btn-large" id="btnDelYes">Yes
                                    </a>
                                </td>
                                <td>
                                    <a class="btn btn-large" id="btnDelNo">No</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnOTID" />
        <input type="hidden" id="hdnOID" />
    </div>
</asp:Content>


