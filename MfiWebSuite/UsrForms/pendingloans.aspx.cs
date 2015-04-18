using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.UserClass;
using MfiWebSuite.BL.MfiClass;
using System.Data;
using MfiWebSuite.BL.Utilities;
using System.Text;
using MfiWebSuite.BL.LoanClasses;

namespace MfiWebSuite.UsrForms
{
    public partial class pendingloans : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string PageID = DbSettings.Menus.LoansPending;

            Users objUsr = new Users();
            UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

            string UserID = objUsrDet.UserID;
            string UserStatusID = objUsrDet.StatusID;

            string UserRoleID = objUsrDet.RoleID;
            string UserOfficeID = objUsrDet.OfficeID;


            if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
            {
                CustEnum.PageAccess Access = objUsr.CheckPageAccess(UserRoleID, PageID);

                if (Access == CustEnum.PageAccess.Yes)
                {
                    DataSet dsLoans = objUsr.GetNewGroupsForUserID(UserID);

                    if (!DataUtils.IsDataSetNull(dsLoans, 0))
                    {
                        //display the loan count
                        LoanCount.InnerHtml = dsLoans.Tables[0].Rows.Count.ToString();

                        StringBuilder sbrLoans = new StringBuilder();

                        sbrLoans.Append("<table class='table table-bordered display' id='tblLoans'>");
                        sbrLoans.Append("<thead>");
                        sbrLoans.Append("<tr>");
                        sbrLoans.Append("<th>Loan</th>");
                        sbrLoans.Append("<th>Group</th>");
                        sbrLoans.Append("<th>Applied On</th>");
                        sbrLoans.Append("<th>Applied By</th>");
                        sbrLoans.Append("<th>Center</th>");
                        sbrLoans.Append("<th>Village</th>");
                        sbrLoans.Append("<th>Branch</th>");                        
                        sbrLoans.Append("</tr>");
                        sbrLoans.Append("</thead>");

                        sbrLoans.Append("<tbody>");

                        foreach (DataRow dr in dsLoans.Tables[0].Rows)
                        {
                            string GroupID = dr["GroupID"].ToString();
                            
                            int DaysAgo;
                            int.TryParse(dr["DaysAgo"].ToString(), out DaysAgo);
                            string DaysPlurality = "Days";

                            string AppliedBy = dr["GroupStaff"].ToString();

                            string LoanURL = Page.GetRouteUrl(AppRoutes.DisburseLoans.Name, new { OfficeID = dr["GroupID"].ToString() });

                            string CenterID = dr["CenterID"].ToString();
                            string CenterURL = Page.GetRouteUrl(AppRoutes.Center.Name, new { OfficeID = CenterID });

                            string Village = dr["Village"].ToString();
                            string VillageURL = Page.GetRouteUrl(AppRoutes.Village.Name, new { OfficeID = dr["VillageID"].ToString() });
                            
                            string Branch = dr["Branch"].ToString();
                            string BranchURL = Page.GetRouteUrl(AppRoutes.BranchOffice.Name, new { OfficeID = dr["BranchID"].ToString() });

                            DateTime AppliedOnUTC = new DateTime();
                            string AppliedOnLocalDate = string.Empty;

                            AppliedOnUTC = DateTime.SpecifyKind(DateTime.Parse(dr["CreatedDateTime"].ToString()), DateTimeKind.Utc);
                            AppliedOnLocalDate = TimeZoneInfo.ConvertTimeFromUtc(AppliedOnUTC, AppSettings.TimeZoneIst).ToDateShortMonth(false, '-');

                            sbrLoans.Append("<tr>");
                            sbrLoans.Append("<td><a class='btn btn-success' href='" + LoanURL + "' >View Loan Form</a></td>");

                            //the following is for the sake of table sorting where Center 8 > center 10!
                            string GroupURL = Page.GetRouteUrl(AppRoutes.Groups.Name, new { }) + "?" + AppSettings.QueryStr.Group.Name + "=" + GroupID;

                            if (int.Parse(GroupID) < 10)
                            {
                                sbrLoans.Append("<td><a href='" + GroupURL + "'>Group 0" + GroupID + "</a></td>");
                            }
                            else
                            {
                                sbrLoans.Append("<td><a href='" + GroupURL + "'>Group " + GroupID + "</a></td>");
                            }
                            

                            if (DaysAgo <= 1)
                                DaysPlurality = "Day";

                            sbrLoans.Append("<td>" + AppliedOnLocalDate + " &nbsp;&nbsp;(" + DaysAgo.ToString() + " " + DaysPlurality + " ago)</td>");

                            sbrLoans.Append("<td>" + AppliedBy + "</td>");

                            //the following is for the sake of table sorting where Center 8 > center 10!
                            if (int.Parse(CenterID) < 10)
                            {
                                sbrLoans.Append("<td><a href='" + CenterURL + "'>Center 0" + CenterID + "</a></td>");
                            }
                            else
                            {
                                sbrLoans.Append("<td><a href='" + CenterURL + "'>Center " + CenterID + "</a></td>");
                            }

                            sbrLoans.Append("<td><a href='" + VillageURL +"'>" + Village + "</a></td>");
                            sbrLoans.Append("<td><a href='"+ BranchURL + "'>" + Branch + "</a></td>");
                            
                            sbrLoans.Append("</tr>");

                        }
                        sbrLoans.Append("</tbody>");
                        sbrLoans.Append("</table>");

                        PendingLoans.InnerHtml = sbrLoans.ToString();
                    }
                    else
                    {
                        PendingLoans.InnerHtml = UiMsg.LoansPending.NoNewLoans.InfoWrap();
                    }

                }
                else
                {
                    PendingLoans.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                }
            }
            else
            {
                PendingLoans.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
            }

        }
    }
}