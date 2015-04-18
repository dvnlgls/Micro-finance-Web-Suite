<%@ Page Title="Client Form" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true"
    CodeBehind="clientinfoform.aspx.cs" Inherits="MfiWebSuite.UsrForms.clientinfoform"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/clientinfoform.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">        
        <div class="ho-content">
            <div class="gen-title">
                Client Information Form</div>
            <div class="cif-glo-alrt" id="gloErrHO" runat="server">
        </div>
            <div id="formContents" runat="server">
                <div class="span7">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="2">
                                    Client Details
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="gen-bold span2">
                                    Name
                                </td>
                                <td class="span3">
                                    <input type="text" id="uxClientName" name="uxClientName" runat="server" class="span3" />
                                    <span class="gen-err-1" id="errClientName"></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Resident Village
                                </td>
                                <td id="uxClientVill" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Gender
                                </td>
                                <td>
                                    <select id="uxClientGender" class="span2" runat="server">
                                        <option value="0">Female</option>
                                        <option value="1">Male</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    S/o/ W/o/ D/o
                                </td>
                                <td>
                                    <input type="text" id="uxClientXof" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Age
                                </td>
                                <td>
                                    <input type="text" id="uxClientAge" runat="server" class="span1" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span12">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="7">
                                    Family Details
                                </th>
                            </tr>
                            <tr>
                                <th class="span1">
                                    S.No
                                </th>
                                <th>
                                    Name
                                </th>
                                <th>
                                    Relationship
                                </th>
                                <th>
                                    Sex
                                </th>
                                <th>
                                    Age
                                </th>
                                <th>
                                    Occupation
                                </th>
                                <th>
                                    Educational Status
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    1
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyName1" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyRel1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <select id="uxFamilyGender1" class="span2" runat="server">
                                        <option value="0">Female</option>
                                        <option value="1">Male</option>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyAge1" runat="server" class="span1" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyOccu1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyEdu1" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    2
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyName2" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyRel2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <select id="uxFamilyGender2" class="span2" runat="server">
                                        <option value="0">Female</option>
                                        <option value="1">Male</option>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyAge2" runat="server" class="span1" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyOccu2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyEdu2" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    3
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyName3" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyRel3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <select id="uxFamilyGender3" class="span2" runat="server">
                                        <option value="0">Female</option>
                                        <option value="1">Male</option>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyAge3" runat="server" class="span1" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyOccu3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyEdu3" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    4
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyName4" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyRel4" runat="server" class="span2" />
                                </td>
                                <td>
                                    <select id="uxFamilyGender4" class="span2" runat="server">
                                        <option value="0">Female</option>
                                        <option value="1">Male</option>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyAge4" runat="server" class="span1" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyOccu4" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyEdu4" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    5
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyName5" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyRel5" runat="server" class="span2" />
                                </td>
                                <td>
                                    <select id="uxFamilyGender5" class="span2" runat="server">
                                        <option value="0">Female</option>
                                        <option value="1">Male</option>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyAge5" runat="server" class="span1" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyOccu5" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxFamilyEdu5" runat="server" class="span2" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span12">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="6">
                                    Present Sources of Earnings of the Client or Other Members of Family
                                </th>
                            </tr>
                            <tr>
                                <th>
                                </th>
                                <th>
                                    Occupation/Activity
                                </th>
                                <th>
                                    Nature
                                </th>
                                <th>
                                    Annual Revenue from Activity (Rs.)
                                </th>
                                <th>
                                    Annual Expd. in Activity (Rs.)
                                </th>
                                <th>
                                    Annual Surplus from Activity (Rs.)
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    A
                                </td>
                                <td class="gen-bold">
                                    Primary Occupation
                                </td>
                                <td>
                                    <input type="text" id="uxEarningNature1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningREvenue1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningExpen1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningSurplus1" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    B
                                </td>
                                <td class="gen-bold">
                                    Other Activity 1
                                </td>
                                <td>
                                    <input type="text" id="uxEarningNature2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningREvenue2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningExpen2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningSurplus2" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    C
                                </td>
                                <td class="gen-bold">
                                    Other Activity 2
                                </td>
                                <td>
                                    <input type="text" id="uxEarningNature3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningREvenue3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningExpen3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningSurplus3" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    D
                                </td>
                                <td class="gen-bold">
                                    Activity of other Family Members
                                </td>
                                <td>
                                    <input type="text" id="uxEarningNature4" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningREvenue4" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningExpen4" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxEarningSurplus4" runat="server" class="span2" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span6">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="2">
                                    Other Details
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="gen-bold span2">
                                    Caste
                                </td>
                                <td>
                                    <select id="uxCaste" runat="server" class="span2">
                                        <option value="SC">SC</option>
                                        <option value="ST">ST</option>
                                        <option value="OBC">OBC</option>
                                        <option value="Minority">Minority</option>
                                        <option value="General">General</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Full Address
                                </td>
                                <td>
                                    <textarea rows="3" cols="5" id="uxClientAddress" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    PIN
                                </td>
                                <td>
                                    <input type="text" id="uxClientPIN" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Telephone No.
                                </td>
                                <td>
                                    <input type="text" id="uxClientPhone" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Does the client have Bank saving A/c?
                                </td>
                                <td>
                                    <select id="uxClientBankAcc" runat="server" class="span2">
                                        <option value="1">Yes</option>
                                        <option value="0">No</option>
                                    </select>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span6">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="2">
                                    Consumption Expenditure
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    Regular Monthly Household Expenditure (Rs.)
                                </th>
                                <th>
                                    Annual Spending on Festive Seasons (Rs.)
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <input type="text" id="uxconsExpen" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxConsFestive" runat="server" class="span2" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span12">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="6">
                                    Particulars of Assets owned:
                                </th>
                            </tr>
                            <tr>
                                <th colspan="6">
                                    Residential Lands & Housing
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    Village
                                </th>
                                <th>
                                    Plot No.
                                </th>
                                <th>
                                    Extent
                                </th>
                                <th>
                                    No. of rooms
                                </th>
                                <th>
                                    Type of Roof
                                </th>
                                <th>
                                    Market Value (Rs.)
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <input type="text" id="uxAssetVillage" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetPlotNo" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetExtent" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetRooms" runat="server" class="span2" />
                                </td>
                                <td>
                                    <select id="uxAssetRoof" runat="server" class="span2">
                                        <option value="Thatched">Thatched</option>
                                        <option value="Tiles">Tiles</option>
                                        <option value="Stones">Stones</option>
                                        <option value="Sheets">Sheets</option>
                                        <option value="RCC">RCC</option>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" id="uxAssetValue" runat="server" class="span2" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span12">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>
                                    Type of Asset
                                </th>
                                <th>
                                    Description
                                </th>
                                <th>
                                    Approx. Mkt. Value(Rs.)
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="gen-bold">
                                    Agricultural Land
                                </td>
                                <td>
                                    <input type="text" id="uxAssetDesc1" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetMkVal1" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Livestock
                                </td>
                                <td>
                                    <input type="text" id="uxAssetDesc2" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetMkVal2" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Agr. Implements
                                </td>
                                <td>
                                    <input type="text" id="uxAssetDesc3" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetMkVal3" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Vehicles
                                </td>
                                <td>
                                    <input type="text" id="uxAssetDesc4" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetMkVal4" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    TV
                                </td>
                                <td>
                                    <input type="text" id="uxAssetDesc5" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetMkVal5" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Any Other
                                </td>
                                <td>
                                    <input type="text" id="uxAssetDesc6" runat="server" class="span3" />
                                </td>
                                <td>
                                    <input type="text" id="uxAssetMkVal6" runat="server" class="span3" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span12">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="6">
                                    Present Source of Credit (Banks, Finance Companies, Traders, Relatives etc.)
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    Source
                                </th>
                                <th>
                                    Rate of Interest
                                </th>
                                <th>
                                    Period of Credit
                                </th>
                                <th>
                                    Purpose
                                </th>
                                <th>
                                    Amount Borrowed (Rs.)
                                </th>
                                <th>
                                    Amount Due
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <input type="text" id="uxCreditsource1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditROI1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditPeriod1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditPurpose1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditBorrowed1" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditDue1" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" id="uxCreditsource2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditROI2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditPeriod2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditPurpose2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditBorrowed2" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditDue2" runat="server" class="span2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" id="uxCreditsource3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditROI3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditPeriod3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditPurpose3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditBorrowed3" runat="server" class="span2" />
                                </td>
                                <td>
                                    <input type="text" id="uxCreditDue3" runat="server" class="span2" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="span6">
                    <table class="table table-bordered">
                        <thead>
                            <tr class="cif-tbl-clr">
                                <th colspan="2" class="span2">
                                    Nomination Details:
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="gen-bold">
                                    Nominee Name
                                </td>
                                <td>
                                    <input type="text" id="uxNomineeName" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Relationship
                                </td>
                                <td>
                                    <input type="text" id="uxNomineeRel" runat="server" class="span3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Age
                                </td>
                                <td>
                                    <input type="text" id="uxNomineeAge" runat="server" class="span1" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gen-bold">
                                    Resident of
                                </td>
                                <td>
                                    <input type="text" id="uxNomineePlace" runat="server" class="span1" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="cif-save-btn">
                <button type="submit" class="btn btn-primary btn-large" id="btnSave" onclick="if (fnVal())"
                    runat="server" onserverclick="btnSave_Click">
                    Save Client Form</button>
            </div>
        </div>
        <input type="hidden" id="hdnOID" runat="server" />
    </div>
</asp:Content>
