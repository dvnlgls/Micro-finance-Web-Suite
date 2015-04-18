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
    public partial class center : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string CenterID = null;

            try
            {
                CenterID = (string)Page.RouteData.Values[AppRoutes.Center.ValOfficeID];
            }
            catch (Exception ex)
            {
            }

            if (CenterID != null)
            {
                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;
                string UserRoleID = objUsrDet.RoleID;

                string UserOfficeID = objUsrDet.OfficeID;
                string UserOfficeTypeID = objUsrDet.OfficeTypeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    //see if user has access to this page and if the page is valid

                    CustEnum.PageAccess Access = objUsr.CheckPageAccess(UserRoleID, DbSettings.Menus.Center);

                    if (Access == CustEnum.PageAccess.Yes)
                    {
                        //check for view and edit access

                        DataSet ds = objUsr.GetMenusActionsForRoleID(UserRoleID);
                        List<string> MenuActions = ds.Tables[0].AsEnumerable().Select(r => r[0].ToString()).ToList();

                        bool AccessView = MenuActions.Contains(DbSettings.AccessCenters.ViewCenterDetails);
                        bool AccessEdit = MenuActions.Contains(DbSettings.AccessCenters.EditCenter);

                        if (AccessView)
                        {
                            Office objOff = new Office();

                            hdnOid.Value = CenterID;

                            DataSet dsCen = objOff.GetCenterDetails(CenterID);

                            if (!DataUtils.IsDataSetNull(dsCen, 0))
                            {
                                DataRow drCEn = dsCen.Tables[0].Rows[0];

                                string CenterName = "Center " + drCEn["CenterID"].ToString();
                                string MeetingLoc = drCEn["MeetingLocation"].ToString();
                                string MeetingTime = drCEn["MeetingTime"].ToString();
                                string FeID = drCEn["StaffID"].ToString();
                                string CreatedOnLocalDate = string.Empty;

                                DateTime CreatedOnUTC = new DateTime();
                                CreatedOnUTC = DateTime.SpecifyKind(DateTime.Parse(drCEn["CreatedDateTime"].ToString()), DateTimeKind.Utc);
                                CreatedOnLocalDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedOnUTC, AppSettings.TimeZoneIst).ToDateShortMonth(true, '-');


                                uxCenterName.InnerHtml = CenterName;
                                uxCreatedOn.InnerHtml = CreatedOnLocalDate;
                                uxMeetingLoc.Value = MeetingLoc;
                                uxMeetingTime.Value = MeetingTime;

                                DataSet dsFE = objOff.GetFeByCenterID(CenterID);
                                if (!DataUtils.IsDataSetNull(dsFE, 0))
                                {
                                    StringBuilder sbrFE = new StringBuilder();
                                    sbrFE.Append("<select id='drpFE' class='cmEditable' disabled='disabled'>");

                                    foreach (DataRow drFE in dsFE.Tables[0].Rows)
                                    {
                                        if (drFE["UserID"].ToString() == FeID)
                                        {
                                            sbrFE.Append("<option selected='selected' value='" + drFE["UserID"].ToString() + "'>" + drFE["Name"].ToString() + "</option>");
                                        }
                                        else
                                        {
                                            sbrFE.Append("<option value='" + drFE["UserID"].ToString() + "'>" + drFE["Name"].ToString() + "</option>");
                                        }
                                    }
                                    sbrFE.Append("</select>");

                                    uxFE.InnerHtml = sbrFE.ToString();
                                }

                                //get groups

                                DataSet dsGroup = objOff.GetGroupsForCenterID(CenterID);

                                if (!DataUtils.IsDataSetNull(dsGroup, 0))
                                {
                                    StringBuilder sbrGrp = new StringBuilder();

                                    foreach (DataRow drGrp in dsGroup.Tables[0].Rows)
                                    {
                                        string GroupID = drGrp["GroupID"].ToString();
                                        string GrpFeName = drGrp["StaffName"].ToString();
                                        string GrpFeID = drGrp["StaffID"].ToString();
                                        string GroupStatus = drGrp["GroupStatus"].ToString();
                                        string GroupStatusClass = drGrp["CssClass"].ToString();
                                        string GroupTitleDef = drGrp["TitleString"].ToString();
                                        string GrpCreatedOnLocalDate = string.Empty;

                                        DateTime GrpCreatedOnUTC = DateTime.SpecifyKind(DateTime.Parse(drGrp["CreatedDateTime"].ToString()), DateTimeKind.Utc);
                                        GrpCreatedOnLocalDate = TimeZoneInfo.ConvertTimeFromUtc(GrpCreatedOnUTC, AppSettings.TimeZoneIst).ToDateShortMonth(false, '-');

                                        string GroupURL = Page.GetRouteUrl(AppRoutes.Groups.Name, new { }) + "?" + AppSettings.QueryStr.Group.Name + "=" + GroupID;

                                        sbrGrp.Append("<tr>");
                                        sbrGrp.Append("<td><a href='" + GroupURL + "'>Group " + GroupID + "</a></td>");
                                        sbrGrp.Append("<td><span class='cmGrpStat " + GroupStatusClass + "' title='"+ GroupTitleDef + "' >" + GroupStatus + "</span></td>");
                                        sbrGrp.Append("<td><a href='#'>" + GrpFeName + "</a></td>");                                        
                                        sbrGrp.Append("<td>" + GrpCreatedOnLocalDate + "</td>");
                                        sbrGrp.Append("</tr>");
                                    }

                                    uxGroupBody.InnerHtml = sbrGrp.ToString();
                                }
                                else
                                {
                                    uxGroups.InnerHtml = UiMsg.Center.NoGroups.ErrorWrap();
                                }

                                //allow edit only when view is enabled
                                if (!AccessEdit)
                                {
                                    uxEditCenter.InnerHtml = "";
                                }
                            }
                            else
                            {
                                uxCenterWrap.InnerHtml = UiMsg.Center.NoCenterData.ErrorWrap();
                            }
                        }
                        else
                        {
                            uxCenterWrap.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                        }
                    }
                    else
                    {
                        uxCenterWrap.InnerHtml = UiMsg.Global.InvalidPage.ErrorWrap();
                    }

                }
                else
                {
                    uxCenterWrap.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                }
            }

        }
    }
}