using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.UserClass;
using System.Data;
using MfiWebSuite.BL.MfiClass;
using MfiWebSuite.BL.Utilities;
using System.Text;

namespace MfiWebSuite.UsrForms
{
    public partial class groups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string PageID = DbSettings.Menus.Groups;

            Users objUsr = new Users();
            UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

            string UserID = objUsrDet.UserID;
            string UserStatusID = objUsrDet.StatusID;
            string UserRoleID = objUsrDet.RoleID;

            string UserOfficeID = objUsrDet.OfficeID;
            string UserOfficeTypeID = objUsrDet.OfficeTypeID;

            if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
            {
                CustEnum.PageAccess Access = objUsr.CheckPageAccess(UserRoleID, PageID);

                if (Access == CustEnum.PageAccess.Yes)
                {
                    //check for privileges

                    DataSet dsAction = objUsr.GetMenusActionsForRoleID(UserRoleID);
                    List<string> MenuActions = dsAction.Tables[0].AsEnumerable().Select(r => r[0].ToString()).ToList();
                    bool DeleteGroup = MenuActions.Contains(DbSettings.MenusAction.DeleteGroup);

                    #region generate filters

                    Office objOff = new Office();
                    bool BranchGenerated = false;

                    //branch
                    StringBuilder sbrBranch = new StringBuilder();

                    if (int.Parse(UserOfficeTypeID) < int.Parse(DbSettings.OfficeType.BranchOffice))
                    {
                        //user belongs to a higher office type, so the sp will fetch branches
                        DataSet dsBranch = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.BranchOffice);

                        sbrBranch.Append("<select class='span2' id='drpBranch'>");
                        sbrBranch.Append("<option value='0'>All Branches</option>");

                        foreach (DataRow drBranch in dsBranch.Tables[0].Rows)
                        {
                            string BranchID = drBranch["OfficeID"].ToString();
                            string BranchName = drBranch["OfficeName"].ToString();

                            sbrBranch.Append("<option value='" + BranchID + "'>" + BranchName + "</option>");
                        }
                        sbrBranch.Append("</select>");
                        uxDrpBranchWrap.InnerHtml = sbrBranch.ToString();
                        BranchGenerated = true;

                    }
                    else if (UserOfficeTypeID == DbSettings.OfficeType.BranchOffice)
                    {
                        //user belongs to a branch
                        DataSet dsBranchDetails = objOff.GetOfficeDetails(UserOfficeID);

                        if (!DataUtils.IsDataSetNull(dsBranchDetails, 0))
                        {
                            sbrBranch.Append("<select disabled='disabled' class='span2' id='drpBranch'>");

                            string BranchID = dsBranchDetails.Tables[0].Rows[0]["OfficeID"].ToString();
                            string BranchName = dsBranchDetails.Tables[0].Rows[0]["OfficeName"].ToString();

                            sbrBranch.Append("<option value='" + BranchID + "'>" + BranchName + "</option>");

                            sbrBranch.Append("</select>");
                            uxDrpBranchWrap.InnerHtml = sbrBranch.ToString();
                            BranchGenerated = true;
                        }
                    }

                    if (BranchGenerated)
                    {
                        //villages
                        DataSet dsVillage = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.Village);
                        StringBuilder sbrVillage = new StringBuilder();

                        if (!DataUtils.IsDataSetNull(dsVillage, 0))
                        {
                            sbrVillage.Append("<select class='span2' id='drpVillage'>");
                            sbrVillage.Append("<option class='0' value='0'>All Villages</option>");

                            foreach (DataRow drVill in dsVillage.Tables[0].Rows)
                            {
                                string VillaID = drVill["OfficeID"].ToString();
                                string VillName = drVill["OfficeName"].ToString();
                                string BranchID = drVill["ParentOfficeID"].ToString();

                                sbrVillage.Append("<option class='" + BranchID + "' value='" + VillaID + "'>" + VillName + "</option>");
                            }
                            sbrVillage.Append("</select>");
                            uxDrpVillageWrap.InnerHtml = sbrVillage.ToString();


                            //centers

                            DataSet dsCen = objOff.GetAllCentersForUserID(UserID);
                            StringBuilder sbrCenter = new StringBuilder();

                            if (!DataUtils.IsDataSetNull(dsCen, 0))
                            {
                                sbrCenter.Append("<select class='span2' id='drpCenter'>");
                                sbrCenter.Append("<option class='0' value='0'>All Centers</option>");

                                foreach (DataRow dr in dsCen.Tables[0].Rows)
                                {
                                    string CenterID = dr["CenterID"].ToString();
                                    string VillageID = dr["ParentOfficeID"].ToString();

                                    sbrCenter.Append("<option class='" + VillageID + "' value='" + CenterID + "'>Center " + CenterID + "</option>");
                                }
                                sbrCenter.Append("</select>");
                                uxDrpCenterWrap.InnerHtml = sbrCenter.ToString();

                            } //center
                        } //village

                    }


                    #endregion

                    DataSet dsGroup = objOff.GetAllGroupsForUserID(UserID);

                    if (!DataUtils.IsDataSetNull(dsGroup, 0))
                    {
                        #region group list
                        StringBuilder sbrGrp = new StringBuilder();

                        foreach (DataRow drGrp in dsGroup.Tables[0].Rows)
                        {
                            string GroupID = drGrp["GroupID"].ToString();
                            string ClassCenter = "cmCenter" + drGrp["CenterID"].ToString();
                            string ClassVillage = "cmVillage" + drGrp["VillageID"].ToString();
                            string ClassBranch = "cmBranch" + drGrp["BranchID"].ToString();

                            string CenterID = drGrp["CenterID"].ToString();
                            string CenterURL = Page.GetRouteUrl(AppRoutes.Center.Name, new { OfficeID = CenterID });

                            string Village = drGrp["VillageName"].ToString();
                            string VillageURL = Page.GetRouteUrl(AppRoutes.Village.Name, new { OfficeID = drGrp["VillageID"].ToString() });

                            string Branch = drGrp["BranchName"].ToString();
                            string BranchURL = Page.GetRouteUrl(AppRoutes.BranchOffice.Name, new { OfficeID = drGrp["BranchID"].ToString() });


                            string GroupLeader = drGrp["GroupLeader"].ToString();
                            string GroupLeaderID = drGrp["GroupLeaderID"].ToString();

                            string GrpFeName = drGrp["StaffName"].ToString();
                            string GrpFeID = drGrp["StaffID"].ToString();
                            string GroupStatus = drGrp["GroupStatus"].ToString();
                            string GroupStatusClass = drGrp["CssClass"].ToString();
                            string GroupTitleDef = drGrp["TitleString"].ToString();
                            string GrpCreatedOnLocalDate = string.Empty;

                            DateTime GrpCreatedOnUTC = DateTime.SpecifyKind(DateTime.Parse(drGrp["CreatedDateTime"].ToString()), DateTimeKind.Utc);
                            GrpCreatedOnLocalDate = TimeZoneInfo.ConvertTimeFromUtc(GrpCreatedOnUTC, AppSettings.TimeZoneIst).ToDateShortMonth(false, '-');

                            sbrGrp.Append("<tr id='trGrp_" + GroupID + "' class='" + ClassBranch + " " + ClassVillage + " " + ClassCenter + "'>");

                            if (int.Parse(GroupID) < 10)
                            {
                                sbrGrp.Append("<td><a href='javascript:void(0)' id='ancGrp_" + GroupID + "'>Group 0" + GroupID + "</a></td>");
                            }
                            else
                            {
                                sbrGrp.Append("<td><a href='javascript:void(0)' id='ancGrp_" + GroupID + "'>Group " + GroupID + "</a></td>");
                            }

                            sbrGrp.Append("<td><span class='cmGrpStat " + GroupStatusClass + "' title='" + GroupTitleDef + "' >" + GroupStatus + "</span></td>");
                            sbrGrp.Append("<td><a href='#'>" + GroupLeader + "</a></td>");
                            sbrGrp.Append("<td><a href='#'>" + GrpFeName + "</a></td>");
                            sbrGrp.Append("<td>" + GrpCreatedOnLocalDate + "</td>");

                            if (int.Parse(CenterID) < 10)
                            {
                                sbrGrp.Append("<td><a href='" + CenterURL + "'>Center 0" + CenterID + "</a></td>");
                            }
                            else
                            {
                                sbrGrp.Append("<td><a href='" + CenterURL + "'>Center " + CenterID + "</a></td>");
                            }

                            sbrGrp.Append("<td><a href='" + VillageURL + "'>" + Village + "</a></td>");
                            sbrGrp.Append("<td><a href='" + BranchURL + "'>" + Branch + "</a></td>");


                            if (DeleteGroup)
                            {
                                sbrGrp.Append("<td><buutton class='btn btn-danger btn-mini' id='btnDel_" + GroupID + "'><i class='icon-white icon-trash'></i>&nbsp;Delete Group</buutton></td>");
                            }
                            else
                            {
                                sbrGrp.Append("<td> - </td>");
                            }

                            sbrGrp.Append("</tr>");
                        }

                        uxGroupBody.InnerHtml = sbrGrp.ToString();

                        #endregion group list

                        #region group details

                        DataSet dsGrpCli = objUsr.GetAllGroupDetailsForUserID(UserID);

                        if (!DataUtils.IsDataSetNull(dsGrpCli, 0))
                        {
                            
                            StringBuilder sbrGrpCli = new StringBuilder();

                            foreach (DataRow drGrpCli in dsGrpCli.Tables[0].Rows)
                            {
                                string GcGrpID = drGrpCli["GroupID"].ToString();
                                string GcCliID = drGrpCli["ClientID"].ToString();
                                string GcCliName = drGrpCli["Name"].ToString();
                                string GcLoanID = drGrpCli["LoanID"].ToString();
                                string GcLoanStatus = drGrpCli["LoanStatus"].ToString();
                                //string GcLPID = drGrpCli["LoanProductID"].ToString();
                                //string GcLP = drGrpCli["ProductName"].ToString();


                                sbrGrpCli.Append("<tr class='trGC_" + GcGrpID + "'>");
                                sbrGrpCli.Append("<td><a href='#'>" + GcCliName + "</a></td>");
                                sbrGrpCli.Append("<td><a href='#'>Loan " + GcLoanID + "</a></td>");
                                sbrGrpCli.Append("<td>" + GcLoanStatus + "</td>");
                                sbrGrpCli.Append("</tr>");
                            }

                            uxGCBody.InnerHtml = sbrGrpCli.ToString();
                        }
 

                        #endregion group details
                    }
                    else
                    {
                        uxGroups.InnerHtml = UiMsg.Center.NoGroups.ErrorWrap();
                    }
                }
                else
                {
                    uxCentContent.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                }
            }
        }
    }
}