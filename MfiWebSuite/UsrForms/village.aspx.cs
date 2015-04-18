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
    public partial class village : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string OfficeID = null;

            try
            {
                OfficeID = (string)Page.RouteData.Values[AppRoutes.Village.ValOfficeID];
            }
            catch (Exception ex)
            {
            }

            if (OfficeID != null)
            {
                //CHECK FOR USER ACCESS

                string OfficeTypeID = DbSettings.OfficeType.Village;

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;
                string UserRoleID = objUsrDet.RoleID;

                string UserOfficeID = objUsrDet.OfficeID;
                string UserOfficeTypeID = objUsrDet.OfficeTypeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    //check if user can access this office type

                    DataSet ds = objUsr.GetMenusActionsForRoleID(UserRoleID);
                    List<string> MenuActions = ds.Tables[0].AsEnumerable().Select(r => r[0].ToString()).ToList();
                    bool AccessView = MenuActions.Contains(DbSettings.AccessOffice.ViewVilTab);

                    if (AccessView)
                    {
                        //check if user can access this office of OfficeID

                        Office objOff = new Office();

                        CustEnum.PageAccess CanAccessOffice = CustEnum.PageAccess.Error;

                        if ((UserOfficeTypeID == OfficeTypeID) && (UserOfficeID == OfficeID))
                        {
                            CanAccessOffice = CustEnum.PageAccess.Yes;
                        }
                        else if (int.Parse(UserOfficeTypeID) < int.Parse(OfficeTypeID))
                        {
                            if (objOff.CheckIfSubOfficeExistUnderOfficeID(UserOfficeID, OfficeID) == "1")
                            {
                                CanAccessOffice = CustEnum.PageAccess.Yes;
                            }
                            else if (objOff.CheckIfSubOfficeExistUnderOfficeID(UserOfficeID, OfficeID) == "0")
                            {
                                CanAccessOffice = CustEnum.PageAccess.No;
                            }
                        }


                        if (CanAccessOffice == CustEnum.PageAccess.Yes)
                        {
                            //user can access this page

                            //Check for MenuActions privileges

                            hdnOid.Value = OfficeID;

                            bool AccessEditVil = MenuActions.Contains(DbSettings.AccessOffice.EditVil);
                            bool AccessViewCen = MenuActions.Contains(DbSettings.AccessCenters.ViewCenterList);
                            bool AccessAddCen = MenuActions.Contains(DbSettings.AccessCenters.AddCenter);
                            bool AccessDelCen = MenuActions.Contains(DbSettings.AccessCenters.DelCenter);
                            
                            #region build page data

                            StringBuilder sbrSubOff = new StringBuilder();

                            if (AccessView)
                            {
                                DataSet dsOff = objOff.GetOfficeDetails(OfficeID);
                                if (!DataUtils.IsDataSetNull(dsOff, 0))
                                {
                                    DataRow drOff = dsOff.Tables[0].Rows[0];

                                    string OffName = drOff["OfficeName"].ToString();                                    

                                    spnName.InnerHtml = OffName;
                                    
                                    //edit options. must follow only after view is completed
                                    if (!AccessEditVil)
                                    {
                                        editOff.InnerHtml = "";
                                    }
                                }
                                else
                                {
                                    viewRO.InnerHtml = UiMsg.PageRO.ViewVilFetchErr.ErrorWrap();
                                }

                            }
                            else
                            {
                                viewRO.InnerHtml = "";
                            }

                            StringBuilder sbrAddCen = new StringBuilder();

                            if (AccessAddCen)
                            {
                                sbrAddCen.Append("<div>");
                                sbrAddCen.Append("<a class='btn btn-primary' id='btnAddCenter'><i class='icon-white icon-plus'></i>&nbsp;Create New Center</a>");
                                sbrAddCen.Append("</div><br/>");
                                DataSet dsFE = objOff.GetFeByVillageID(OfficeID);
                                if (!DataUtils.IsDataSetNull(dsFE, 0))
                                {
                                    DataTable dtFe = dsFE.Tables[0];

                                    StringBuilder sbrFE = new StringBuilder();
                                    sbrFE.Append("<select id='drpFE'>");
                                    sbrFE.Append("<option value='0'>Select FE</option>");

                                    foreach (DataRow drFE in dtFe.Rows)
                                    {
                                        sbrFE.Append("<option value='" + drFE["UserID"].ToString() + "'>" + drFE["Name"].ToString() + "</option>");
                                    }

                                    sbrFE.Append("</select>");

                                    FE.InnerHtml = sbrFE.ToString();
                                }

                            }

                            if (AccessViewCen)
                            {
                                DataSet dsSubOff = objOff.GetCentersForVillageID(OfficeID);
                                sbrSubOff.Append("<div id='tblCenWrap' class='vil-cent-tbl-wrap'>");

                                if (!DataUtils.IsDataSetNull(dsSubOff, 0))
                                {
                                    sbrSubOff.Append("<table class='table table-bordered vil-cen-tbl display' id='tblCenters'>");
                                    sbrSubOff.Append("<thead>");
                                    sbrSubOff.Append("<tr>");
                                    sbrSubOff.Append("<th class='off'>Center</th>");
                                    sbrSubOff.Append("<th class='off'>Field Executive</th>");
                                    sbrSubOff.Append("<th class='off'>Meeting Location</th>");
                                    sbrSubOff.Append("<th class='off'>Meeting Time</th>");
                                    sbrSubOff.Append("<th class='off'>Created On</th>");
                                    if (AccessDelCen)
                                    {
                                        sbrSubOff.Append("<th><i class='icon-cog'></i>&nbsp;Options</th>");
                                        hdnAcD.Value = "1";
                                    }
                                    sbrSubOff.Append("</tr>");
                                    sbrSubOff.Append("</thead>");
                                    sbrSubOff.Append("<tbody>");

                                    foreach (DataRow dr in dsSubOff.Tables[0].Rows)
                                    {
                                        string CenterID = dr["CenterID"].ToString();
                                        string CenterName = "Center " + CenterID;
                                        string MeetingLocation = dr["MeetingLocation"].ToString();
                                        string MeetingTime = dr["MeetingTime"].ToString();
                                        string FeName = dr["StaffName"].ToString();
                                        string FeID = dr["StaffID"].ToString();

                                        DateTime CreatedUTC = new DateTime();
                                        string CreatedLocalDate = string.Empty;

                                        CreatedUTC = DateTime.SpecifyKind(DateTime.Parse(dr["CreatedDateTime"].ToString()), DateTimeKind.Utc);
                                        CreatedLocalDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedUTC, AppSettings.TimeZoneIst).ToDateShortMonth(false, '-');

                                        string SubOffURL = Page.GetRouteUrl(AppRoutes.Center.Name, new { OfficeID = dr["CenterID"].ToString() });

                                        sbrSubOff.Append("<tr id='trCen_" + CenterID + "'>");
                                        sbrSubOff.Append("<td>");

                                        if (int.Parse(CenterID) < 10)
                                        {
                                            sbrSubOff.Append("<a href='" + SubOffURL + "'>Center 0" + CenterID + "</a>");
                                        }
                                        else
                                        {
                                            sbrSubOff.Append("<a href='" + SubOffURL + "'>Center " + CenterID + "</a>");
                                        }

                                        
                                        sbrSubOff.Append("</td>");
                                        sbrSubOff.Append("<td><a href='#'>" + FeName + "</a></td>");
                                        sbrSubOff.Append("<td>" + MeetingLocation + "</td>");
                                        sbrSubOff.Append("<td>" + MeetingTime + "</td>");
                                        sbrSubOff.Append("<td>" + CreatedLocalDate + "</td>");

                                        if (AccessDelCen)
                                        {
                                            sbrSubOff.Append("<td><a class='btn btn-danger btn-mini' id='btnDel_" + CenterID + "'><i class='icon-white icon-trash'></i>&nbsp;Remove Center</a></td>");
                                        }
                                        sbrSubOff.Append("</tr>");

                                    }
                                    sbrSubOff.Append("</tbody>");
                                    sbrSubOff.Append("</table>");
                                    
                                    //uxCenterList.InnerHtml = sbrSubOff.ToString();
                                }
                                else
                                {
                                    //uxCenterList.InnerHtml = UiMsg.PageVillage.NoCenters.ErrorWrap();
                                    sbrSubOff.Append(UiMsg.PageVillage.NoCenters.ErrorWrap());
                                }
                                sbrSubOff.Append("</div>");
                                uxCenterList.InnerHtml = sbrAddCen.ToString() + sbrSubOff.ToString();
                            }
                            else
                            {
                                viewSubOff.InnerHtml = "";
                            }

                           

                            #endregion build page data

                        }
                        else if (CanAccessOffice == CustEnum.PageAccess.No)
                        {
                            uxOffcontent.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                        }

                    } //view access
                    else
                    {
                        uxOffcontent.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                    }

                }
                else
                {
                    Sessions objSession = new Sessions();
                    objSession.EndSession("&" + AppSettings.QueryStr.SessionExpired.Name + "=" + AppSettings.QueryStr.SessionExpired.Value);
                }

            }
            else
            {
                //error
                uxOffcontent.InnerHtml = UiMsg.Global.NoOfficeExists.ErrorWrap();
            }
        }
    }
}