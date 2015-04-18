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
using System.Text;
using MfiWebSuite.BL.Utilities;

namespace MfiWebSuite.UsrForms
{
    public partial class office : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string PageID = DbSettings.Menus.Office;

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
                    DataSet ds = objUsr.GetMenusActionsForRoleID(UserRoleID);

                    List<string> MenuActions = ds.Tables[0].AsEnumerable().Select(r => r[0].ToString()).ToList();

                    bool AccessViewHoTab = MenuActions.Contains(DbSettings.AccessOffice.ViewHoTab);

                    bool AccessViewRoTab = MenuActions.Contains(DbSettings.AccessOffice.ViewRoTab);
                    bool AccessAddRo = MenuActions.Contains(DbSettings.AccessOffice.AddRo);
                    bool AccessDeleteRo = MenuActions.Contains(DbSettings.AccessOffice.DeleteRo);

                    bool AccessViewAoTab = MenuActions.Contains(DbSettings.AccessOffice.ViewAoTab);
                    bool AccessAddAo = MenuActions.Contains(DbSettings.AccessOffice.AddAo);
                    bool AccessDeleteAo = MenuActions.Contains(DbSettings.AccessOffice.DeleteAo);

                    bool AccessViewBoTab = MenuActions.Contains(DbSettings.AccessOffice.ViewBoTab);
                    bool AccessAddBo = MenuActions.Contains(DbSettings.AccessOffice.AddBo);
                    bool AccessDeleteBo = MenuActions.Contains(DbSettings.AccessOffice.DeleteBo);

                    bool AccessViewVilTab = MenuActions.Contains(DbSettings.AccessOffice.ViewVilTab);
                    bool AccessAddVil = MenuActions.Contains(DbSettings.AccessOffice.AddVil);
                    bool AccessDeleteVil = MenuActions.Contains(DbSettings.AccessOffice.DeleteVil);

                    Office objOff = new Office();

                    StringBuilder sbrTab = new StringBuilder();
                    StringBuilder sbrTabContent = new StringBuilder();

                    //tab typeID
                    string OfficeTypeIdHO = DbSettings.OfficeType.HeadOffice;
                    string OfficeTypeIdRO = DbSettings.OfficeType.RegionalOffice;
                    string OfficeTypeIdAO = DbSettings.OfficeType.AreaOffice;
                    string OfficeTypeIdBO = DbSettings.OfficeType.BranchOffice;
                    string OfficeTypeIdVil = DbSettings.OfficeType.Village;

                    #region HO TAB
                    if (AccessViewHoTab)
                    {
                        if (OfficeTypeIdHO == UserOfficeTypeID)
                        {
                            //user belongs to this office level

                            sbrTab.Append("<li class='active'><a href='#HO' data-toggle='tab'>Head Office</a></li>");

                            DataSet dsOff = objOff.GetOfficeDetails(DbSettings.Fixed.HOID);
                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                DataRow drOff = dsOff.Tables[0].Rows[0];

                                string OffURL = Page.GetRouteUrl(AppRoutes.HeadOffice.Name, new { });

                                string OffName = drOff["OfficeName"].ToString();
                                string OffAddr = drOff["Address"].ToString();
                                string OffPhone1 = drOff["Phone1"].ToString();
                                string OffPhone2 = drOff["Phone2"].ToString();
                                string OffFax = drOff["Fax"].ToString();
                                string OffEmail = drOff["EmailID"].ToString();
                                string OffWebsite = drOff["Website"].ToString();

                                sbrTabContent.Append("<div class='tab-pane active' id='HO'>");

                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered'>");

                                sbrTabContent.Append("<thead>");

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Head Office</th>");
                                sbrTabContent.Append("<th>Address</th>");
                                sbrTabContent.Append("<th>Phone 1</th>");
                                sbrTabContent.Append("<th>Phone 2</th>");
                                sbrTabContent.Append("<th>Fax</th>");
                                sbrTabContent.Append("<th>Email</th>");
                                sbrTabContent.Append("<th>Website</th>");
                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                sbrTabContent.Append("<td>" + OffFax + "</td>");
                                sbrTabContent.Append("<td>" + OffEmail + "</td>");
                                sbrTabContent.Append("<td><a href='http://" + OffWebsite + "' target='_blank'>" + OffWebsite + "</a></td>");
                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</tbody>");

                                sbrTabContent.Append("</table>");
                                sbrTabContent.Append("</div>");

                                sbrTabContent.Append("</div>");
                            }
                        }

                    } //ho tab
                    #endregion HO TAB

                    #region RO TAB
                    if (AccessViewRoTab)
                    {                        
                        
                        if (UserOfficeTypeID == OfficeTypeIdRO)
                        {
                            //user belongs to this office level

                            //build tab
                            sbrTab.Append("<li class='active'><a href='#RO' data-toggle='tab'>Regional Office</a></li>");

                            //build tab data
                            DataSet dsOff = objOff.GetOfficeDetails(UserOfficeID);

                            sbrTabContent.Append("<div class='tab-pane active' id='RO'>");

                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered'>");

                                sbrTabContent.Append("<thead>");
                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Regional Office</th>");
                                sbrTabContent.Append("<th>Address</th>");
                                sbrTabContent.Append("<th>Phone 1</th>");
                                sbrTabContent.Append("<th>Phone 2</th>");
                                sbrTabContent.Append("<th>Fax</th>");
                                sbrTabContent.Append("<th>Email</th>");
                                sbrTabContent.Append("</tr>");
                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                DataRow dr = dsOff.Tables[0].Rows[0];
                                string OffURL = Page.GetRouteUrl(AppRoutes.RegionalOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                string OffID = dr["OfficeID"].ToString();
                                string OffName = dr["OfficeName"].ToString();
                                string OffAddr = dr["Address"].ToString();
                                string OffPhone1 = dr["Phone1"].ToString();
                                string OffPhone2 = dr["Phone2"].ToString();
                                string OffFax = dr["Fax"].ToString();
                                string OffEmail = dr["EmailID"].ToString();

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                sbrTabContent.Append("<td>" + OffFax + "</td>");
                                sbrTabContent.Append("<td>" + OffEmail + "</td>");
                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</tbody>");
                                sbrTabContent.Append("</table>");

                                sbrTabContent.Append("</div>"); //div for tbl wrapper                                                                                            
                            }
                            else
                            {
                                sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                            }

                            sbrTabContent.Append("</div>"); //div for tab pane

                        }
                        else if (int.Parse(UserOfficeTypeID) < int.Parse(OfficeTypeIdRO))
                        {
                            //this type of office coes under the user's office. 

                            //tab
                            sbrTab.Append("<li><a href='#RO' data-toggle='tab'>Regional Office</a></li>");
                            
                            //tab data                           
                            sbrTabContent.Append("<div class='tab-pane' id='RO'>");

                            //options
                            if (AccessAddRo)
                            {
                                sbrTabContent.Append("<div class='of-optn-wrap'>");
                                sbrTabContent.Append("<button class='btn btn-primary' id='btnAdd_" + OfficeTypeIdRO + "'><i class='icon-white icon-plus'></i>&nbsp;Create New Regional Office</button>");
                                sbrTabContent.Append("</div>");
                            }

                            //reg offices
                            DataSet dsOff = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.RegionalOffice);
                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered'>");

                                sbrTabContent.Append("<thead>");

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Regional Office</th>");
                                sbrTabContent.Append("<th>Address</th>");
                                sbrTabContent.Append("<th>Phone 1</th>");
                                sbrTabContent.Append("<th>Phone 2</th>");
                                sbrTabContent.Append("<th>Fax</th>");
                                sbrTabContent.Append("<th>Email</th>");

                                if (AccessDeleteRo)
                                {
                                    sbrTabContent.Append("<th><i class='icon-cog'></i>&nbsp;Options</th>");
                                }

                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                foreach (DataRow dr in dsOff.Tables[0].Rows)
                                {

                                    string OffURL = Page.GetRouteUrl(AppRoutes.RegionalOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                    string OffID = dr["OfficeID"].ToString();
                                    string OffName = dr["OfficeName"].ToString();
                                    string OffAddr = dr["Address"].ToString();
                                    string OffPhone1 = dr["Phone1"].ToString();
                                    string OffPhone2 = dr["Phone2"].ToString();
                                    string OffFax = dr["Fax"].ToString();
                                    string OffEmail = dr["EmailID"].ToString();


                                    sbrTabContent.Append("<tr>");
                                    sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                    sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                    sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                    sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                    sbrTabContent.Append("<td>" + OffFax + "</td>");
                                    sbrTabContent.Append("<td>" + OffEmail + "</td>");

                                    if (AccessDeleteRo)
                                    {
                                        sbrTabContent.Append("<td>");
                                            sbrTabContent.Append("<button class='btn btn-danger btn-mini' id='btnDel_" + OffID + "'><i class='icon-white icon-trash'></i>&nbsp;Remove Office</button>");
                                        sbrTabContent.Append("</td>");
                                    }

                                    sbrTabContent.Append("</tr>");
                                }

                                sbrTabContent.Append("</tbody>");

                                sbrTabContent.Append("</table>");
                                sbrTabContent.Append("</div>"); //div for tbl wrapper                                

                            } //ds not null
                            else
                            {
                                //sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                            }
                            sbrTabContent.Append("</div>"); //div for tab pane

                        } //otid <                                              

                    } //ro tab

                    #endregion RO TAB

                    #region AO TAB
                    if (AccessViewAoTab)
                    {

                        if (UserOfficeTypeID == OfficeTypeIdAO)
                        {
                            //user belongs to this office level

                            //build tab
                            sbrTab.Append("<li class='active'><a href='#AO' data-toggle='tab'>Area Office</a></li>");

                            //build tab data
                            DataSet dsOff = objOff.GetOfficeDetails(UserOfficeID);
                            
                            sbrTabContent.Append("<div class='tab-pane active' id='AO'>");

                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered'>");

                                sbrTabContent.Append("<thead>");
                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Area Office</th>");
                                sbrTabContent.Append("<th>Address</th>");
                                sbrTabContent.Append("<th>Phone 1</th>");
                                sbrTabContent.Append("<th>Phone 2</th>");
                                sbrTabContent.Append("<th>Fax</th>");
                                sbrTabContent.Append("<th>Email</th>");
                                sbrTabContent.Append("</tr>");
                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                DataRow dr = dsOff.Tables[0].Rows[0];
                                string OffURL = Page.GetRouteUrl(AppRoutes.AreaOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                string OffID = dr["OfficeID"].ToString();
                                string OffName = dr["OfficeName"].ToString();
                                string OffAddr = dr["Address"].ToString();
                                string OffPhone1 = dr["Phone1"].ToString();
                                string OffPhone2 = dr["Phone2"].ToString();
                                string OffFax = dr["Fax"].ToString();
                                string OffEmail = dr["EmailID"].ToString();

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                sbrTabContent.Append("<td>" + OffFax + "</td>");
                                sbrTabContent.Append("<td>" + OffEmail + "</td>");
                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</tbody>");
                                sbrTabContent.Append("</table>");

                                sbrTabContent.Append("</div>"); //div for tbl wrapper                                                            
                                
                            }
                            else
                            {
                                sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                            }

                            sbrTabContent.Append("</div>"); //div for tab pane

                        }
                        else if (int.Parse(UserOfficeTypeID) < int.Parse(OfficeTypeIdAO))
                        {
                            //this type of office coes under the user's office. 

                            //tab
                            sbrTab.Append("<li><a href='#AO' data-toggle='tab'>Area Office</a></li>");

                            //tab data                           
                            sbrTabContent.Append("<div class='tab-pane' id='AO'>");

                            //options
                            if (AccessAddAo)
                            {
                                sbrTabContent.Append("<div class='of-optn-wrap'>");
                                sbrTabContent.Append("<button class='btn btn-primary' id='btnAdd_" + OfficeTypeIdAO + "'><i class='icon-white icon-plus'></i>&nbsp;Create New Area Office</button>");
                                sbrTabContent.Append("</div>");
                            }

                            //reg offices
                            DataSet dsOff = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.AreaOffice);
                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered'>");

                                sbrTabContent.Append("<thead>");

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Area Office</th>");
                                sbrTabContent.Append("<th>Address</th>");
                                sbrTabContent.Append("<th>Phone 1</th>");
                                sbrTabContent.Append("<th>Phone 2</th>");
                                sbrTabContent.Append("<th>Fax</th>");
                                sbrTabContent.Append("<th>Email</th>");

                                if (AccessDeleteAo)
                                {
                                    sbrTabContent.Append("<th><i class='icon-cog'></i>&nbsp;Options</th>");
                                }

                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                foreach (DataRow dr in dsOff.Tables[0].Rows)
                                {

                                    string OffURL = Page.GetRouteUrl(AppRoutes.AreaOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                    string OffID = dr["OfficeID"].ToString();
                                    string OffName = dr["OfficeName"].ToString();
                                    string OffAddr = dr["Address"].ToString();
                                    string OffPhone1 = dr["Phone1"].ToString();
                                    string OffPhone2 = dr["Phone2"].ToString();
                                    string OffFax = dr["Fax"].ToString();
                                    string OffEmail = dr["EmailID"].ToString();


                                    sbrTabContent.Append("<tr>");
                                    sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                    sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                    sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                    sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                    sbrTabContent.Append("<td>" + OffFax + "</td>");
                                    sbrTabContent.Append("<td>" + OffEmail + "</td>");

                                    if (AccessDeleteAo)
                                    {
                                        sbrTabContent.Append("<td>");
                                        sbrTabContent.Append("<button class='btn btn-danger btn-mini' id='btnDel_" + OffID + "'><i class='icon-white icon-trash'></i>&nbsp;Remove Office</button>");
                                        sbrTabContent.Append("</td>");
                                    }

                                    sbrTabContent.Append("</tr>");
                                }

                                sbrTabContent.Append("</tbody>");

                                sbrTabContent.Append("</table>");
                                sbrTabContent.Append("</div>"); //div for tbl wrapper                                

                            } //ds not null
                            else
                            {
                                //sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                            }
                            sbrTabContent.Append("</div>"); //div for tab pane

                        } //otid <                                              

                    } //ro tab

                    #endregion AO TAB

                    #region BO TAB
                    if (AccessViewBoTab)
                    {

                        if (UserOfficeTypeID == OfficeTypeIdBO)
                        {
                            //user belongs to this office level

                            //build tab
                            sbrTab.Append("<li class='active'><a href='#BO' data-toggle='tab'>Branch Office</a></li>");

                            //build tab data
                            DataSet dsOff = objOff.GetOfficeDetails(UserOfficeID);
                            
                            sbrTabContent.Append("<div class='tab-pane active' id='BO'>");

                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered'>");

                                sbrTabContent.Append("<thead>");
                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Branch Office</th>");
                                sbrTabContent.Append("<th>Address</th>");
                                sbrTabContent.Append("<th>Phone 1</th>");
                                sbrTabContent.Append("<th>Phone 2</th>");
                                sbrTabContent.Append("<th>Fax</th>");
                                sbrTabContent.Append("<th>Email</th>");
                                sbrTabContent.Append("</tr>");
                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                DataRow dr = dsOff.Tables[0].Rows[0];
                                string OffURL = Page.GetRouteUrl(AppRoutes.BranchOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                string OffID = dr["OfficeID"].ToString();
                                string OffName = dr["OfficeName"].ToString();
                                string OffAddr = dr["Address"].ToString();
                                string OffPhone1 = dr["Phone1"].ToString();
                                string OffPhone2 = dr["Phone2"].ToString();
                                string OffFax = dr["Fax"].ToString();
                                string OffEmail = dr["EmailID"].ToString();

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                sbrTabContent.Append("<td>" + OffFax + "</td>");
                                sbrTabContent.Append("<td>" + OffEmail + "</td>");
                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</tbody>");
                                sbrTabContent.Append("</table>");

                                sbrTabContent.Append("</div>"); //div for tbl wrapper                                                                                            
                            }
                            else
                            {
                                //sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                            }

                            sbrTabContent.Append("</div>"); //div for tab pane

                        }
                        else if (int.Parse(UserOfficeTypeID) < int.Parse(OfficeTypeIdBO))
                        {
                            //this type of office coes under the user's office. 

                            //tab
                            sbrTab.Append("<li><a href='#BO' data-toggle='tab'>Branch Office</a></li>");

                            //tab data                           
                            sbrTabContent.Append("<div class='tab-pane' id='BO'>");

                            //options
                            if (AccessAddBo)
                            {
                                sbrTabContent.Append("<div class='of-optn-wrap'>");
                                sbrTabContent.Append("<button class='btn btn-primary' id='btnAdd_" + OfficeTypeIdBO + "'><i class='icon-white icon-plus'></i>&nbsp;Create New Branch Office</button>");
                                sbrTabContent.Append("</div>");
                            }

                            //reg offices
                            DataSet dsOff = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.BranchOffice);
                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered'>");

                                sbrTabContent.Append("<thead>");

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Branch Office</th>");
                                sbrTabContent.Append("<th>Address</th>");
                                sbrTabContent.Append("<th>Phone 1</th>");
                                sbrTabContent.Append("<th>Phone 2</th>");
                                sbrTabContent.Append("<th>Fax</th>");
                                sbrTabContent.Append("<th>Email</th>");

                                if (AccessDeleteBo)
                                {
                                    sbrTabContent.Append("<th><i class='icon-cog'></i>&nbsp;Options</th>");
                                }

                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                foreach (DataRow dr in dsOff.Tables[0].Rows)
                                {

                                    string OffURL = Page.GetRouteUrl(AppRoutes.BranchOffice.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                    string OffID = dr["OfficeID"].ToString();
                                    string OffName = dr["OfficeName"].ToString();
                                    string OffAddr = dr["Address"].ToString();
                                    string OffPhone1 = dr["Phone1"].ToString();
                                    string OffPhone2 = dr["Phone2"].ToString();
                                    string OffFax = dr["Fax"].ToString();
                                    string OffEmail = dr["EmailID"].ToString();


                                    sbrTabContent.Append("<tr>");
                                    sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                    sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                    sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                    sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                    sbrTabContent.Append("<td>" + OffFax + "</td>");
                                    sbrTabContent.Append("<td>" + OffEmail + "</td>");

                                    if (AccessDeleteBo)
                                    {
                                        sbrTabContent.Append("<td>");
                                        sbrTabContent.Append("<button class='btn btn-danger btn-mini' id='btnDel_" + OffID + "'><i class='icon-white icon-trash'></i>&nbsp;Remove Office</button>");
                                        sbrTabContent.Append("</td>");
                                    }

                                    sbrTabContent.Append("</tr>");
                                }

                                sbrTabContent.Append("</tbody>");

                                sbrTabContent.Append("</table>");
                                sbrTabContent.Append("</div>"); //div for tbl wrapper                                

                            } //ds not null
                            else
                            {
                                //sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                            }
                            sbrTabContent.Append("</div>"); //div for tab pane

                        } //otid <                                              

                    } //ro tab

                    #endregion BO TAB

                    #region VIL TAB
                    if (AccessViewVilTab)
                    {                        
                        #region office = vil

                        //the following scenario won't occur as per business rules

                        //if (UserOfficeTypeID == OfficeTypeIdVil)
                        //{
                        //    //user belongs to this office level

                        //    //build tab
                        //    sbrTab.Append("<li class='active'><a href='#VIL' data-toggle='tab'>Village</a></li>");

                        //    //build tab data
                        //    DataSet dsOff = objOff.GetOfficeDetails(UserOfficeID);
                            
                        //    sbrTabContent.Append("<div class='tab-pane active' id='VIL'>");

                        //    if (!DataUtils.IsDataSetNull(dsOff, 0))
                        //    {
                        //        sbrTabContent.Append("<div class='of-tbl-wrap'>");
                        //        sbrTabContent.Append("<table class='table table-bordered'>");

                        //        sbrTabContent.Append("<thead>");
                        //        sbrTabContent.Append("<tr>");
                        //        sbrTabContent.Append("<th>Village</th>");
                        //        sbrTabContent.Append("<th>Parent Branch</th>");
                        //        //sbrTabContent.Append("<th>Address</th>");
                        //        //sbrTabContent.Append("<th>Phone 1</th>");
                        //        //sbrTabContent.Append("<th>Phone 2</th>");
                        //        //sbrTabContent.Append("<th>Fax</th>");
                        //        //sbrTabContent.Append("<th>Email</th>");
                        //        sbrTabContent.Append("</tr>");
                        //        sbrTabContent.Append("</thead>");

                        //        sbrTabContent.Append("<tbody>");

                        //        DataRow dr = dsOff.Tables[0].Rows[0];
                        //        string OffURL = Page.GetRouteUrl(AppRoutes.Village.Name, new { OfficeID = dr["OfficeID"].ToString() });

                        //        string OffID = dr["OfficeID"].ToString();
                        //        string OffName = dr["OfficeName"].ToString();
                        //        string ParentOffice = dr["ParentOfficeName"].ToString();
                        //        string OffAddr = dr["Address"].ToString();
                        //        string OffPhone1 = dr["Phone1"].ToString();
                        //        string OffPhone2 = dr["Phone2"].ToString();
                        //        string OffFax = dr["Fax"].ToString();
                        //        string OffEmail = dr["EmailID"].ToString();

                        //        sbrTabContent.Append("<tr>");
                        //        sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                        //        sbrTabContent.Append("<td>" + ParentOffice + "</td>");
                        //        //sbrTabContent.Append("<td>" + OffAddr + "</td>");
                        //        //sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                        //        //sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                        //        //sbrTabContent.Append("<td>" + OffFax + "</td>");
                        //        //sbrTabContent.Append("<td>" + OffEmail + "</td>");
                        //        sbrTabContent.Append("</tr>");

                        //        sbrTabContent.Append("</tbody>");
                        //        sbrTabContent.Append("</table>");

                        //        sbrTabContent.Append("</div>"); //div for tbl wrapper                                                                                            
                        //    }
                        //    else
                        //    {
                        //        sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                        //    }

                        //    sbrTabContent.Append("</div>"); //div for tab pane

                        //}

                        #endregion office = vil

                        if (int.Parse(UserOfficeTypeID) < int.Parse(OfficeTypeIdVil))
                        {
                            //this type of office coes under the user's office. 

                            //tab
                            sbrTab.Append("<li><a href='#VIL' data-toggle='tab'>Village</a></li>");

                            //tab data                           
                            sbrTabContent.Append("<div class='tab-pane' id='VIL'>");

                            //options
                            if (AccessAddVil)
                            {
                                sbrTabContent.Append("<div class='of-optn-wrap'>");
                                sbrTabContent.Append("<button class='btn btn-primary' id='btnAdd_" + OfficeTypeIdVil + "'><i class='icon-white icon-plus'></i>&nbsp;Create New Village</button>");
                                sbrTabContent.Append("</div>");
                            }

                            //reg offices
                            DataSet dsOff = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.Village);
                            if (!DataUtils.IsDataSetNull(dsOff, 0))
                            {
                                sbrTabContent.Append("<div class='of-tbl-wrap'>");
                                sbrTabContent.Append("<table class='table table-bordered' id='tblVillage'>");

                                sbrTabContent.Append("<thead>");

                                sbrTabContent.Append("<tr>");
                                sbrTabContent.Append("<th>Village</th>");
                                sbrTabContent.Append("<th>Parent Branch</th>");
                                //sbrTabContent.Append("<th>Address</th>");
                                //sbrTabContent.Append("<th>Phone 1</th>");
                                //sbrTabContent.Append("<th>Phone 2</th>");
                                //sbrTabContent.Append("<th>Fax</th>");
                                //sbrTabContent.Append("<th>Email</th>");

                                if (AccessDeleteVil)
                                {
                                    sbrTabContent.Append("<th><i class='icon-cog'></i>&nbsp;Options</th>");
                                }

                                sbrTabContent.Append("</tr>");

                                sbrTabContent.Append("</thead>");

                                sbrTabContent.Append("<tbody>");

                                foreach (DataRow dr in dsOff.Tables[0].Rows)
                                {

                                    string OffURL = Page.GetRouteUrl(AppRoutes.Village.Name, new { OfficeID = dr["OfficeID"].ToString() });

                                    string OffID = dr["OfficeID"].ToString();
                                    string OffName = dr["OfficeName"].ToString();
                                    string ParentOffice = dr["ParentOfficeName"].ToString();
                                    string OffAddr = dr["Address"].ToString();
                                    string OffPhone1 = dr["Phone1"].ToString();
                                    string OffPhone2 = dr["Phone2"].ToString();
                                    string OffFax = dr["Fax"].ToString();
                                    string OffEmail = dr["EmailID"].ToString();


                                    sbrTabContent.Append("<tr>");
                                    sbrTabContent.Append("<td><a href='" + OffURL + "'>" + OffName + "</a></td>");
                                    sbrTabContent.Append("<td>" + ParentOffice + "</td>");
                                    //sbrTabContent.Append("<td>" + OffAddr + "</td>");
                                    //sbrTabContent.Append("<td>" + OffPhone1 + "</td>");
                                    //sbrTabContent.Append("<td>" + OffPhone2 + "</td>");
                                    //sbrTabContent.Append("<td>" + OffFax + "</td>");
                                    //sbrTabContent.Append("<td>" + OffEmail + "</td>");

                                    if (AccessDeleteVil)
                                    {
                                        sbrTabContent.Append("<td>");
                                        sbrTabContent.Append("<button class='btn btn-danger btn-mini' id='btnDel_" + OffID + "'><i class='icon-white icon-trash'></i>&nbsp;Remove Village</button>");
                                        sbrTabContent.Append("</td>");
                                    }

                                    sbrTabContent.Append("</tr>");
                                }

                                sbrTabContent.Append("</tbody>");

                                sbrTabContent.Append("</table>");
                                sbrTabContent.Append("</div>"); //div for tbl wrapper                                

                            } //ds not null
                            else
                            {                                
                                //sbrTabContent.Append(UiMsg.Global.NoData.ErrorWrap());
                            }
                            sbrTabContent.Append("</div>"); //div for tab pane

                        } //otid <                                              

                    } //ro tab

                    #endregion VIL TAB

                    tab.InnerHtml = sbrTab.ToString();
                    tabContent.InnerHtml = "<div id='locNotHO' class='gen-not-1'></div>" + sbrTabContent.ToString();
                }
            }
        }
    }
}