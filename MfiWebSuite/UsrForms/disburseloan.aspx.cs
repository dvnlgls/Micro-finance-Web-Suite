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
    public partial class disburseloan : System.Web.UI.Page
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
                    //first, check if the group id is valid. 
                    string GroupID = null;

                    try
                    {
                        GroupID = (string)Page.RouteData.Values[AppRoutes.DisburseLoans.ValOfficeID];
                    }
                    catch (Exception ex){}

                    if (GroupID != null)
                    {
                        //check the group status to prevent erroneous operations

                        Loans objLoan = new Loans();

                        DataSet dsLoan = objLoan.GetLafDetails(GroupID);
                        
                        if (!DataUtils.IsDataSetNull(dsLoan, 0))
                        {
                            DataRow drBasicDetails =dsLoan.Tables[0].Rows[0];
                            string GroupStatusID = drBasicDetails["GroupStatusID"].ToString();

                            if (GroupStatusID == DbSettings.GroupStatus.New)
                            {
                                //can proceed       

                                hdnGID.Value = GroupID;

                                DateTime AppliedOnUTC = new DateTime();
                                string AppliedOnLocalDate = string.Empty;

                                AppliedOnUTC = DateTime.SpecifyKind(DateTime.Parse(drBasicDetails["CreatedDateTime"].ToString()), DateTimeKind.Utc);
                                AppliedOnLocalDate = TimeZoneInfo.ConvertTimeFromUtc(AppliedOnUTC, AppSettings.TimeZoneIst).ToDateShortMonth(true, '-');

                                int DaysAgo;
                                int.TryParse(drBasicDetails["DaysAgo"].ToString(), out DaysAgo);
                                
                                string DaysPlurality = "Days";
                                if (DaysAgo <= 1)
                                    DaysPlurality = "Day";

                                string FE = drBasicDetails["FE"].ToString();

                                string CenterID = drBasicDetails["CenterID"].ToString();
                                string CenterURL = Page.GetRouteUrl(AppRoutes.Center.Name, new { OfficeID = CenterID });

                                string Village = drBasicDetails["VillageName"].ToString();
                                string VillageURL = Page.GetRouteUrl(AppRoutes.Village.Name, new { OfficeID = drBasicDetails["VillageID"].ToString() });

                                string Branch = drBasicDetails["BranchName"].ToString();
                                string BranchURL = Page.GetRouteUrl(AppRoutes.BranchOffice.Name, new { OfficeID = drBasicDetails["BranchID"].ToString() });

                                string LoanProdID = drBasicDetails["LoanProductID"].ToString();
                                string LoanProdName = drBasicDetails["LoanProduct"].ToString();
                                string LoanProdInt = drBasicDetails["Interest"].ToString();
                                string LoanProdAmnt = drBasicDetails["DefualtAmount"].ToString();
                                string LoanProdTenure = drBasicDetails["Tenure"].ToString();
                                string CollectionTypeID = drBasicDetails["CollectionTypeID"].ToString();
                                string CollectionFrequency = drBasicDetails["CollectionFrequency"].ToString();

                                //pass vales to ui
                                string GroupURL = Page.GetRouteUrl(AppRoutes.Groups.Name, new { }) + "?" + AppSettings.QueryStr.Group.Name +"=" + GroupID;

                                uxGroupID.InnerHtml = "<a href='" + GroupURL + "'>Group " + GroupID + "</a>";
                                uxAppliedOn.InnerHtml = AppliedOnLocalDate + " &nbsp;&nbsp;(" + DaysAgo.ToString() + " " + DaysPlurality + " ago)";
                                uxFE.InnerHtml = FE;
                                uxHierarchy.InnerHtml = "<a href='" + CenterURL + "'>Center " + CenterID + "</a>&nbsp;&nbsp;<i class='icon icon-arrow-right gen-nav-op-1'></i>&nbsp;&nbsp;" + "<a href='" + VillageURL + "'>" + Village + "</a>&nbsp;&nbsp;<i class='icon icon-arrow-right gen-nav-op-1'></i>&nbsp;&nbsp;" + "<a href='" + BranchURL + "'>" + Branch + "</a>";

                                uxLP.InnerHtml = "<a href='#'>" + LoanProdName + "</a>";
                                uxLpamnt.InnerHtml = LoanProdAmnt;
                                uxLpInt.InnerHtml = LoanProdInt;
                                uxLpTenure.InnerHtml = LoanProdTenure;
                                hdnCFTID.Value = CollectionTypeID;
                                hdnCFV.Value = CollectionFrequency;


                                //generate client list
                                StringBuilder sbrClients = new StringBuilder();

                                sbrClients.Append("<tr class='template'>");

                                sbrClients.Append("<td colspan='3' class='template-info'>Values entered in this row will be copied to all clients&nbsp; <i class='icon-hand-right'></i></td>");
                                sbrClients.Append("<td><input type='text' class='ldisb-amnt gloAmnt' /></td>");
                                sbrClients.Append("<td><input readonly='readonly' type='text' class='ldisb-date gloDate cmDisbDate' /></td>");
                                sbrClients.Append("<td><input readonly='readonly' type='text' class='lfr-date gloFrDate cmFRDate' /></td>");
                                sbrClients.Append("<td><input type='text' class='span1 gloEMI' /></td>");
                                sbrClients.Append("<td></td>");

                                sbrClients.Append("</tr>");

                                foreach (DataRow dr in dsLoan.Tables[0].Rows)
                                {
                                    string ClientName = dr["Name"].ToString();
                                    string ClientID = dr["ClientID"].ToString();
                                    string GroupLeader = dr["IsGroupLeader"].ToString();

                                    string LoanID = dr["LoanID"].ToString();
                                    string PurposeName = dr["PurposeName"].ToString();
                                    string AmntApplied = dr["AmountApplied"].ToString();

                                    sbrClients.Append("<tr class='cmClientRow' id='LN_" + LoanID +"'>");

                                    if (GroupLeader == "1")
                                    {
                                        sbrClients.Append("<td><a href='#' title='Group Leader' class='label label-info GroupLeader'>" + ClientName + "</a></td>");
                                    }
                                    else
                                    {
                                        sbrClients.Append("<td><a href='#'>" + ClientName + "</a></td>");
                                    }


                                    sbrClients.Append("<td>" + PurposeName + "</td>");
                                    sbrClients.Append("<td>" + AmntApplied + "</td>");                                    
                                    sbrClients.Append("<td><input type='text' class='ldisb-amnt cmDisbAmnt' value='" + AmntApplied + "' /></td>");
                                    sbrClients.Append("<td><input readonly='readonly' type='text' class='ldisb-date cmDisbDate' /></td>");
                                    sbrClients.Append("<td><input readonly='readonly' type='text' class='lfr-date cmFRDate' /></td>");
                                    sbrClients.Append("<td><input type='text' class='span1 cmEMI' /></td>");
                                    sbrClients.Append("<td><a id='RPS_" + LoanID + "' class='ldisb-rps-a'>View Schedule</a></td>");

                                    sbrClients.Append("</tr>");                                
   
                                }
                                uxClientList.InnerHtml = sbrClients.ToString();

                            }
                            else
                            {
                                DisburseLoan.InnerHtml =  UiMsg.LoansDisbursement.InvalidStatus.ErrorWrap();
                            }
                            
                        }
                        else
                        {
                            DisburseLoan.InnerHtml = UiMsg.LoansDisbursement.NoData.ErrorWrap();
                        }
                            
                    }
                    else
                    {
                        DisburseLoan.InnerHtml = UiMsg.Global.InvalidPage.ErrorWrap();
                    }
                }
                else
                {
                    DisburseLoan.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                }
            }
            else
            {
                DisburseLoan.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
            }
        }
    }
}