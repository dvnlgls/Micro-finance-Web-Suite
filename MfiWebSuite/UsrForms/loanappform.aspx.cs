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
    public partial class loanappform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string PageID = DbSettings.Menus.LAF;

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
                    Office objOff = new Office();
                    Loans objLoan = new Loans();

                    DataSet dsOff = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.Village);
                    StringBuilder sbrVillage = new StringBuilder();

                    if (!DataUtils.IsDataSetNull(dsOff, 0))
                    {
                        sbrVillage.Append("<select class='span2' id='drpVillage'>");
                        sbrVillage.Append("<option value='0'>Select Village</option>");

                        foreach (DataRow dr in dsOff.Tables[0].Rows)
                        {
                            string OffID = dr["OfficeID"].ToString();
                            string OffName = dr["OfficeName"].ToString();
                            string ParentID = dr["ParentOfficeID"].ToString();

                            sbrVillage.Append("<option class='" + ParentID + "' value='" + OffID + "'>" + OffName + "</option>");
                        }
                        sbrVillage.Append("</select>");
                        drpVillageWrap.InnerHtml = sbrVillage.ToString();

                        DataSet dsCen = objOff.GetAllCentersForUserID(UserID);
                        StringBuilder sbrCenter = new StringBuilder();

                        if (!DataUtils.IsDataSetNull(dsCen, 0))
                        {
                            sbrCenter.Append("<select class='span2' id='drpCenter'>");
                            sbrCenter.Append("<option value='0'>Select Center</option>");

                            foreach (DataRow dr in dsCen.Tables[0].Rows)
                            {
                                string CenterID = dr["CenterID"].ToString();
                                string VillageID = dr["ParentOfficeID"].ToString();

                                sbrCenter.Append("<option class='" + VillageID + "' value='" + CenterID + "'>Center " + CenterID + "</option>");
                            }
                            sbrCenter.Append("</select>");
                            drpCenterWrap.InnerHtml = sbrCenter.ToString();

                        } //center
                    } //village

                    DataSet dsLP = objLoan.GetLoanProducts(null);
                    StringBuilder sbrLP = new StringBuilder();

                    StringBuilder sbrLpDet = new StringBuilder();

                    if (!DataUtils.IsDataSetNull(dsLP, 0))
                    {
                        sbrLP.Append("<select class='span2' id='drpLP'>");
                        sbrLP.Append("<option value='0'>Select</option>");

                        foreach (DataRow dr in dsLP.Tables[0].Rows)
                        {
                            string LPID = dr["LoanProductID"].ToString();
                            string ProductName = dr["ProductName"].ToString();

                            string DefualtAmount = dr["DefualtAmount"].ToString();
                            string Interest = dr["Interest"].ToString();
                            string Tenure = dr["Tenure"].ToString();

                            sbrLP.Append("<option value='" + LPID + "'>" + ProductName + "</option>");

                            sbrLpDet.Append("<tr id='" + LPID + "' style='display:none'>");
                            sbrLpDet.Append("<td>" + ProductName + "</td>");
                            sbrLpDet.Append("<td>" + DefualtAmount + "</td>");
                            sbrLpDet.Append("<td>" + Interest + "</td>");
                            sbrLpDet.Append("<td>" + Tenure + "</td>");
                            sbrLpDet.Append("</tr>");
                        }
                        sbrLP.Append("</select>");
                        drpLpWrap.InnerHtml = sbrLP.ToString();
                        lpDetailsBody.InnerHtml = sbrLpDet.ToString();

                        DataSet dsLoanCycle = objLoan.GetLoanCycles(null);
                        StringBuilder sbrCycle = new StringBuilder();

                        if (!DataUtils.IsDataSetNull(dsLoanCycle, 0))
                        {
                           
                            foreach (DataRow dr in dsLoanCycle.Tables[0].Rows)
                            {
                                string LoanProductID = dr["LoanProductID"].ToString();
                                string Cycle = dr["Cycle"].ToString();
                                string MinValue = dr["MinValue"].ToString();
                                string MaxValue = dr["MaxValue"].ToString();
                                
                                sbrCycle.Append("<tr class='LC_" + LoanProductID + " hide'>");
                                sbrCycle.Append("<td>" + Cycle + "</td>");
                                sbrCycle.Append("<td>" + MinValue + "</td>");
                                sbrCycle.Append("<td>" + MaxValue + "</td>");                                
                                sbrCycle.Append("</tr>");

                            }

                            lcBody.InnerHtml = sbrCycle.ToString();

                        }

                    } //lp


                    DataSet dsFE = objUsr.GetAllFeForUserID(UserID);
                    StringBuilder sbrFE = new StringBuilder();

                    if (!DataUtils.IsDataSetNull(dsFE, 0))
                    {
                        sbrFE.Append("<select class='span2' id='drpFE'>");
                        sbrFE.Append("<option value='0'>Select</option>");

                        foreach (DataRow dr in dsFE.Tables[0].Rows)
                        {
                            string FEID = dr["UserID"].ToString();
                            string FeName = dr["Name"].ToString();
                            string OfficeID = dr["OfficeID"].ToString();

                            sbrFE.Append("<option class='" + OfficeID + "' value='" + FEID + "'>" + FeName + "</option>");
                        }
                        sbrFE.Append("</select>");
                        drpFeWrap.InnerHtml = sbrFE.ToString();

                    } //fe


                    DataSet dsPurp = objLoan.GetLoanPurpose();
                    StringBuilder sbrPurp = new StringBuilder();

                    if (!DataUtils.IsDataSetNull(dsPurp, 0))
                    {
                        sbrPurp.Append("<select class='lp' id='drpLoanPurpose'>");
                        //sbrPurp.Append("<option value='0'>Select</option>");

                        foreach (DataRow dr in dsPurp.Tables[0].Rows)
                        {
                            string PurpID = dr["LoanPurposeID"].ToString();
                            string PurpName = dr["PurposeName"].ToString();


                            sbrPurp.Append("<option value='" + PurpID + "'>" + PurpName + "</option>");
                        }
                        sbrPurp.Append("</select>");
                        drpLoanPurposeWrap.InnerHtml = sbrPurp.ToString();

                    } //purpose

                    DataSet dsCli = objUsr.GetLoanEligClientsForUserID(UserID);
                    StringBuilder sbrCli = new StringBuilder();

                    if (!DataUtils.IsDataSetNull(dsCli, 0))
                    {
                        sbrCli.Append("<select class='name' id='drpClients'>");
                        sbrCli.Append("<option value='0'>Select</option>");

                        foreach (DataRow dr in dsCli.Tables[0].Rows)
                        {
                            string ClientID = dr["ClientID"].ToString();
                            string Name = dr["Name"].ToString();
                            string VillageID = dr["VillageID"].ToString();


                            sbrCli.Append("<option class='" + VillageID + "' value='" + ClientID + "'>" + Name + "</option>");
                        }
                        sbrCli.Append("</select>");
                        drpCliWrap.InnerHtml = sbrCli.ToString();

                    } //purpose
                }
            }

        }
    }
}