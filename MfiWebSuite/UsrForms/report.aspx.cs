using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.MfiClass;
using System.Text;
using MfiWebSuite.BL.UserClass;
using System.Data;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.LoanClasses;

namespace MfiWebSuite.UsrForms
{
    public partial class report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ReportID = null;

            try
            {
                ReportID = (string)Page.RouteData.Values["ReportID"];
            }
            catch (Exception ex)
            {
            }

            if (ReportID != null)
            {
                hdnRID.Value = ReportID;

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;
                string UserRoleID = objUsrDet.RoleID;

                string UserOfficeID = objUsrDet.OfficeID;
                string UserOfficeTypeID = objUsrDet.OfficeTypeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    CustEnum.PageAccess Access = objUsr.CheckPageAccess(UserRoleID, ReportID);

                    if (Access == CustEnum.PageAccess.Yes)
                    {
                        string ReportTitle = "Report"; //default name

                        #region set page title
                        switch (ReportID)
                        {
                            case DbSettings.ReportType.CenterCollectionSheet:
                                ReportTitle = "Center Collection Sheet";
                                break;

                            case DbSettings.ReportType.DuevsCollectionBranch:
                                ReportTitle = "Due vs Collection - Branch";
                                break;

                            case DbSettings.ReportType.DuevsCollectionCenter:
                                ReportTitle = "Due vs Collection - Center";
                                break;
                            case DbSettings.ReportType.DuevsCollectionFE:
                                ReportTitle = "Due vs Collection - FE";
                                break;

                            case DbSettings.ReportType.FEDetailed:
                                ReportTitle = "FE Detailed";
                                break;

                            case DbSettings.ReportType.HOReport:
                                ReportTitle = "HO Report";
                                break;

                            case DbSettings.ReportType.LoanDisbursementReport:
                                ReportTitle = "Loan Disbursement Report";
                                break;

                            case DbSettings.ReportType.MonthlyBranchStatus:
                                ReportTitle = "Monthly Branch Status";
                                break;

                            case DbSettings.ReportType.OutstandingReportBranch:
                                ReportTitle = "Outstanding Report - Branch";
                                break;

                            case DbSettings.ReportType.OutstandingReportProduct:
                                ReportTitle = "Outstanding Report - Product";
                                break;

                            case DbSettings.ReportType.OutstandingReportVillage:
                                ReportTitle = "Outstanding Report - Village";
                                break;

                            case DbSettings.ReportType.OverdueReportBranch:
                                ReportTitle = "Overdue Report - Branch";
                                break;

                            case DbSettings.ReportType.OverdueReportCenter:
                                ReportTitle = "Overdue Report - Center";
                                break;

                            case DbSettings.ReportType.OverdueReportFE:
                                ReportTitle = "Overdue Report - FE";
                                break;

                            case DbSettings.ReportType.OverdueReportProduct:
                                ReportTitle = "Overdue Report - Product";
                                break;

                            case DbSettings.ReportType.OverdueReportVillage:
                                ReportTitle = "Overdue Report - Village";
                                break;

                            case DbSettings.ReportType.PurposeWiseLoanPortfolio:
                                ReportTitle = "Purpose Wise Loan Portfolio";
                                break;

                            case DbSettings.ReportType.SubLedgerBalances:
                                ReportTitle = "Sub-Ledger Balances";
                                break;

                            default:
                                break;
                        }

                        var page = (Page)HttpContext.Current.Handler;
                        page.Title = ReportTitle;

                        #endregion

                        uxReportTitle.InnerHtml = ReportTitle;

                        #region generate filters

                        bool ShowBranch = true; //set to true by default
                        bool ShowVillage = false;
                        bool ShowCenter = false;
                        bool ShowAsOn = false;
                        bool ShowFrom = false;
                        bool ShowTo = false;
                        bool ShowMonth = false;
                        bool ShowFE = false;
                        bool ShowLP = false;

                        //bool BranchGenerated = false;

                        //decide the filters that has to be shown
                        if (ReportID == DbSettings.ReportType.OverdueReportBranch)
                        {
                            //ShowLP = true;
                        }

                        if (ReportID == DbSettings.ReportType.OverdueReportFE ||
                            ReportID == DbSettings.ReportType.DuevsCollectionFE ||
                            ReportID == DbSettings.ReportType.FEDetailed)
                        {
                            ShowFE = true;
                        }

                        if (ReportID == DbSettings.ReportType.OverdueReportBranch)
                        {
                            //ShowVillage = true;
                        }

                        if (ReportID == DbSettings.ReportType.DuevsCollectionCenter)
                        {
                            ShowCenter = true;
                        }

                        if (ReportID == DbSettings.ReportType.OutstandingReportBranch ||
                            ReportID == DbSettings.ReportType.OutstandingReportProduct ||
                            ReportID == DbSettings.ReportType.OutstandingReportVillage ||
                            ReportID == DbSettings.ReportType.SubLedgerBalances ||
                            ReportID == DbSettings.ReportType.OverdueReportBranch ||
                            ReportID == DbSettings.ReportType.OverdueReportFE ||
                            ReportID == DbSettings.ReportType.OverdueReportCenter ||
                            ReportID == DbSettings.ReportType.OverdueReportProduct ||
                            ReportID == DbSettings.ReportType.OverdueReportVillage ||
                            ReportID == DbSettings.ReportType.PurposeWiseLoanPortfolio
                            )
                        {
                            ShowAsOn = true;
                        }

                        if (ReportID == DbSettings.ReportType.LoanDisbursementReport ||
                            ReportID == DbSettings.ReportType.DuevsCollectionBranch ||
                            ReportID == DbSettings.ReportType.DuevsCollectionCenter ||
                            ReportID == DbSettings.ReportType.DuevsCollectionFE ||
                            ReportID == DbSettings.ReportType.FEDetailed

                            )
                        {
                            ShowFrom = true;
                        }

                        if (ReportID == DbSettings.ReportType.LoanDisbursementReport ||
                            ReportID == DbSettings.ReportType.DuevsCollectionBranch ||
                            ReportID == DbSettings.ReportType.DuevsCollectionCenter ||
                            ReportID == DbSettings.ReportType.DuevsCollectionFE ||
                            ReportID == DbSettings.ReportType.FEDetailed)
                        {
                            ShowTo = true;
                        }

                        if (ReportID == DbSettings.ReportType.MonthlyBranchStatus)
                        {
                            ShowMonth = true;
                        }

                        //show required filters
                        if (ShowBranch)
                        {
                            uxLblBranch.Style.Add("display", "block");
                            uxBranchWrap.Style.Add("display", "block");
                        }

                        if (ShowFE)
                        {
                            uxLblFe.Style.Add("display", "block");
                            uxFEWrap.Style.Add("display", "block");
                        }

                        if (ShowFrom)
                        {
                            uxLblFrom.Style.Add("display", "block");
                            uxFromWrap.Style.Add("display", "block");
                        }


                        if (ShowLP)
                        {
                            uxLblLP.Style.Add("display", "block");
                            uxLPWrap.Style.Add("display", "block");
                        }

                        if (ShowMonth)
                        {
                            uxLblMonth.Style.Add("display", "block");
                            uxMonthPickerWrap.Style.Add("display", "block");
                        }

                        if (ShowTo)
                        {
                            uxLblTo.Style.Add("display", "block");
                            uxToWrap.Style.Add("display", "block");
                        }

                        if (ShowVillage)
                        {
                            uxLblVillage.Style.Add("display", "block");
                            uxVillageWrap.Style.Add("display", "block");
                        }


                        if (ShowCenter)
                        {
                            uxLblCenter.Style.Add("display", "block");
                            uxCenterWrap.Style.Add("display", "block");
                        }


                        if (ShowAsOn)
                        {
                            uxLblAsOn.Style.Add("display", "block");
                            uxAsOnWrap.Style.Add("display", "block");
                        }

                        Office objOff = new Office();

                        if (ShowBranch)
                        {
                            //branch
                            StringBuilder sbrBranch = new StringBuilder();

                            if (int.Parse(UserOfficeTypeID) < int.Parse(DbSettings.OfficeType.BranchOffice))
                            {
                                //user belongs to a higher office type, so the sp will fetch branches
                                DataSet dsBranch = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.BranchOffice);

                                sbrBranch.Append("<select class='span2' id='uxBranch'>");
                                sbrBranch.Append("<option value='0'>All Branches</option>");

                                foreach (DataRow drBranch in dsBranch.Tables[0].Rows)
                                {
                                    string BranchID = drBranch["OfficeID"].ToString();
                                    string BranchName = drBranch["OfficeName"].ToString();

                                    sbrBranch.Append("<option value='" + BranchID + "'>" + BranchName + "</option>");
                                }
                                sbrBranch.Append("</select>");
                                uxBranchWrap.InnerHtml = sbrBranch.ToString();
                                //BranchGenerated = true;

                            }

                            else if (UserOfficeTypeID == DbSettings.OfficeType.BranchOffice)
                            {
                                //user belongs to a branch
                                DataSet dsBranchDetails = objOff.GetOfficeDetails(UserOfficeID);

                                if (!DataUtils.IsDataSetNull(dsBranchDetails, 0))
                                {
                                    sbrBranch.Append("<select disabled='disabled' class='span2' id='uxBranch'>");

                                    string BranchID = dsBranchDetails.Tables[0].Rows[0]["OfficeID"].ToString();
                                    string BranchName = dsBranchDetails.Tables[0].Rows[0]["OfficeName"].ToString();

                                    sbrBranch.Append("<option value='" + BranchID + "'>" + BranchName + "</option>");

                                    sbrBranch.Append("</select>");
                                    uxBranchWrap.InnerHtml = sbrBranch.ToString();
                                    //BranchGenerated = true;
                                }
                            }
                        } //end branch

                        if (ShowVillage)
                        {
                            //villages
                            DataSet dsVillage = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.Village);
                            StringBuilder sbrVillage = new StringBuilder();

                            if (!DataUtils.IsDataSetNull(dsVillage, 0))
                            {
                                sbrVillage.Append("<select class='span2' id='uxVillage'>");
                                sbrVillage.Append("<option class='0' value='0'>All Villages</option>");

                                foreach (DataRow drVill in dsVillage.Tables[0].Rows)
                                {
                                    string VillaID = drVill["OfficeID"].ToString();
                                    string VillName = drVill["OfficeName"].ToString();
                                    string BranchID = drVill["ParentOfficeID"].ToString();

                                    sbrVillage.Append("<option class='" + BranchID + "' value='" + VillaID + "'>" + VillName + "</option>");
                                }
                                sbrVillage.Append("</select>");
                                uxVillageWrap.InnerHtml = sbrVillage.ToString();
                            }
                        } //show village

                        if (ShowCenter)
                        {
                            //centers

                            DataSet dsCen = objOff.GetAllCentersForUserID(UserID);
                            StringBuilder sbrCenter = new StringBuilder();

                            if (!DataUtils.IsDataSetNull(dsCen, 0))
                            {
                                sbrCenter.Append("<select class='span2' id='uxCenter'>");
                                sbrCenter.Append("<option class='0' value='0'>All Centers</option>");

                                foreach (DataRow dr in dsCen.Tables[0].Rows)
                                {
                                    string CenterID = dr["CenterID"].ToString();
                                    string VillageID = dr["ParentOfficeID"].ToString();

                                    sbrCenter.Append("<option class='" + VillageID + "' value='" + CenterID + "'>Center " + CenterID + "</option>");
                                }
                                sbrCenter.Append("</select>");
                                uxCenterWrap.InnerHtml = sbrCenter.ToString();

                            } 
                        } //center

                        if (ShowFE)
                        {
                            DataSet dsFE = objOff.GetAllFE();
                            StringBuilder sbrFE = new StringBuilder();

                            if (!DataUtils.IsDataSetNull(dsFE, 0))
                            {
                                sbrFE.Append("<select class='span2' id='uxFE'>");
                                sbrFE.Append("<option class='0' value='0'>All FE</option>");

                                foreach (DataRow drFE in dsFE.Tables[0].Rows)
                                {
                                    string FeUserID = drFE["UserID"].ToString();
                                    string FeName = drFE["Name"].ToString();
                                    string BranchID = drFE["OfficeID"].ToString();

                                    sbrFE.Append("<option class='" + BranchID + "' value='" + FeUserID + "'>" + FeName + "</option>");
                                }
                                sbrFE.Append("</select>");
                                uxFEWrap.InnerHtml = sbrFE.ToString();
                            }
                        }

                        if (ShowLP)
                        {
                            Loans onjLoan = new Loans();
                            DataSet dsLP = onjLoan.GetLoanProducts(null);
                            StringBuilder sbrLP = new StringBuilder();

                            if (!DataUtils.IsDataSetNull(dsLP, 0))
                            {
                                sbrLP.Append("<select class='span2' id='uxLP'>");
                                sbrLP.Append("<option class='0' value='0'>All Products</option>");

                                foreach (DataRow drFE in dsLP.Tables[0].Rows)
                                {
                                    string LPID = drFE["LoanProductID"].ToString();
                                    string LPName = drFE["ProductName"].ToString();                                    

                                    sbrLP.Append("<option value='" + LPID + "'>" + LPName + "</option>");
                                }
                                sbrLP.Append("</select>");
                                uxLPWrap.InnerHtml = sbrLP.ToString();
                            }
                        }

                    }


                        #endregion
                }
            }
        }
    }
}
