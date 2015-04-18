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
    public partial class areaoffice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string OfficeID = null;

            try
            {
                OfficeID = (string)Page.RouteData.Values[AppRoutes.AreaOffice.ValOfficeID];
            }
            catch (Exception ex)
            {
            }

            if (OfficeID != null)
            {
                //CHECK FOR USER ACCESS

                string OfficeTypeID = DbSettings.OfficeType.AreaOffice;

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
                    bool AccessView = MenuActions.Contains(DbSettings.AccessOffice.ViewAoTab);

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

                            bool AccessEditAO = MenuActions.Contains(DbSettings.AccessOffice.EditAo);
                            bool AccessViewBO = MenuActions.Contains(DbSettings.AccessOffice.ViewBoTab);

                            #region build page data

                            if (AccessView)
                            {
                                DataSet dsOff = objOff.GetOfficeDetails(OfficeID);
                                if (!DataUtils.IsDataSetNull(dsOff, 0))
                                {
                                    DataRow drOff = dsOff.Tables[0].Rows[0];

                                    string OffName = drOff["OfficeName"].ToString();
                                    string OffAddr = drOff["Address"].ToString();
                                    string OffPhone1 = drOff["Phone1"].ToString();
                                    string OffPhone2 = drOff["Phone2"].ToString();
                                    string OffFax = drOff["Fax"].ToString();
                                    string OffEmail = drOff["EmailID"].ToString();

                                    spnName.InnerHtml = OffName;
                                    spnAddr.InnerHtml = OffAddr;
                                    spnPh1.InnerHtml = OffPhone1;
                                    spnPh2.InnerHtml = OffPhone2;
                                    spnFax.InnerHtml = OffFax;
                                    spnMail.InnerHtml = OffEmail;

                                    //edit options. must follow only after view is completed
                                    if (!AccessEditAO)
                                    {
                                        editOff.InnerHtml = "";
                                    }
                                }
                                else
                                {
                                    viewRO.InnerHtml = UiMsg.PageRO.ViewAoFetchErr.ErrorWrap();
                                }

                            }
                            else
                            {
                                viewRO.InnerHtml = "";
                            }


                            if (AccessViewBO)
                            {
                                DataSet dsSubOff = objOff.GetAllImmediateSubOffices(OfficeID, DbSettings.OfficeType.BranchOffice);

                                if (!DataUtils.IsDataSetNull(dsSubOff, 0))
                                {
                                    StringBuilder sbrSubOff = new StringBuilder();

                                    foreach (DataRow dr in dsSubOff.Tables[0].Rows)
                                    {
                                        string SubOffName = dr["OfficeName"].ToString();
                                        string SubOffAddr = dr["Address"].ToString();
                                        string SubOffPh = dr["Phone1"].ToString();
                                        string SubOffURL = Page.GetRouteUrl(AppRoutes.BranchOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                        sbrSubOff.Append("<tr>");
                                        sbrSubOff.Append("<td>");
                                        sbrSubOff.Append("<a href='" + SubOffURL + "'>" + SubOffName + "</a>");
                                        sbrSubOff.Append("</td>");
                                        sbrSubOff.Append("<td>" + SubOffAddr + "</td>");
                                        sbrSubOff.Append("<td>" + SubOffPh + "</td>");
                                        sbrSubOff.Append("</tr>");

                                    }
                                    uxSubOfficeTblBody.InnerHtml = sbrSubOff.ToString();
                                }
                                else
                                {
                                    viewSubOff.InnerHtml = UiMsg.PageRO.ViewBoFetchErr.ErrorWrap();
                                }
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