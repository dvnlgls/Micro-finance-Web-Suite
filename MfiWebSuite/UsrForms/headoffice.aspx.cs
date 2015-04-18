using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.UserClass;
using MfiWebSuite.BL.Utilities;
using System.Data;
using MfiWebSuite.BL.MfiClass;
using System.Text;
using System.Web.Routing;

namespace MfiWebSuite.UsrForms
{
    public partial class headoffice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Users objUsr = new Users();
            UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

            string UserID = objUsrDet.UserID;
            string UserStatusID = objUsrDet.StatusID;
            string UserRoleID = objUsrDet.RoleID;
            
            string UserOfficeTypeID = objUsrDet.OfficeTypeID;

            if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
            {
                if (UserOfficeTypeID == DbSettings.OfficeType.HeadOffice)
                {
                    //user can access this page

                    DataSet ds = objUsr.GetMenusActionsForRoleID(UserRoleID);

                    List<string> MenuActions = ds.Tables[0].AsEnumerable().Select(r => r[0].ToString()).ToList();

                    //Check for MenuActions privileges
                    bool AccessViewHo = MenuActions.Contains(DbSettings.AccessOffice.ViewHoTab);
                    bool AccessEditHO = MenuActions.Contains(DbSettings.AccessOffice.EditHo);
                    bool AccessViewRO = MenuActions.Contains(DbSettings.AccessOffice.ViewRoTab);

                    Office objOff = new Office();

                    if (AccessViewHo)
                    {
                        DataSet dsOff = objOff.GetAllImmediateSubOffices("0", DbSettings.OfficeType.HeadOffice);
                        if (!DataUtils.IsDataSetNull(dsOff, 0))
                        {
                            DataRow drOff = dsOff.Tables[0].Rows[0];

                            string OffName = drOff["OfficeName"].ToString();
                            string OffAddr = drOff["Address"].ToString();
                            string OffPhone1 = drOff["Phone1"].ToString();
                            string OffPhone2 = drOff["Phone2"].ToString();
                            string OffFax = drOff["Fax"].ToString();
                            string OffEmail = drOff["EmailID"].ToString();
                            string OffWebsite = drOff["Website"].ToString();

                            spnName.InnerHtml = OffName;
                            spnAddr.InnerHtml = OffAddr;
                            spnPh1.InnerHtml = OffPhone1;
                            spnPh2.InnerHtml = OffPhone2;
                            spnFax.InnerHtml = OffFax;
                            spnMail.InnerHtml = OffEmail;
                            spnWeb.InnerHtml = "<a href='http://" + OffWebsite + "' target='_blank'>" + OffWebsite + "</a>";

                            //edit options. must follow only after view is completed
                            if (!AccessEditHO)
                            {
                                editHO.InnerHtml = "";
                            }
                        }
                        else
                        {
                            viewHO.InnerHtml = UiMsg.PageHO.ViewHoFetchErr.ErrorWrap();
                        }
                        
                    }
                    else
                    {
                        viewHO.InnerHtml = "";
                    }
                   

                    if (AccessViewRO)
                    {
                        DataSet dsSubOff = objOff.GetAllImmediateSubOffices(DbSettings.OfficeType.HeadOffice, DbSettings.OfficeType.RegionalOffice);
                        
                        if (!DataUtils.IsDataSetNull(dsSubOff, 0))
                        {
                            StringBuilder sbrSubOff = new StringBuilder();

                            foreach (DataRow dr in dsSubOff.Tables[0].Rows)
                            {
                                string SubOffName = dr["OfficeName"].ToString();
                                string SubOffAddr = dr["Address"].ToString();
                                string SubOffPh = dr["Phone1"].ToString();
                                string SubOffURL = Page.GetRouteUrl(AppRoutes.RegionalOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                sbrSubOff.Append("<tr>");
                                sbrSubOff.Append("<td>");
                                sbrSubOff.Append("<a href='" + SubOffURL +"'>" + SubOffName + "</a>");
                                sbrSubOff.Append("</td>");
                                sbrSubOff.Append("<td>" + SubOffAddr + "</td>");
                                sbrSubOff.Append("<td>" + SubOffPh + "</td>");
                                sbrSubOff.Append("</tr>");

                            }
                            uxSubOfficeTblBody.InnerHtml = sbrSubOff.ToString();
                        }
                        else
                        {
                            viewRO.InnerHtml = UiMsg.PageHO.ViewRoFetchErr.ErrorWrap();
                        }
                    }
                    else
                    {
                        viewRO.InnerHtml = "";
                    }

                }
                else
                {
                    gloErrHO.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                }

            }
            else
            {
                Sessions objSession = new Sessions();
                objSession.EndSession("&" + AppSettings.QueryStr.SessionExpired.Name + "=" + AppSettings.QueryStr.SessionExpired.Value);
            }
        }
    }
}