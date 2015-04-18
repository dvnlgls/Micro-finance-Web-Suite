using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.ReportClass;

namespace MfiWebSuite.Ajax
{
    public partial class reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["AjaxMethod"] == "GetReport")
            {
                string ReportID = Request.Form["RID"];
                string BranchID = Request.Form["BranchID"];
                string VillageID = Request.Form["VillageID"];
                string CenterID = Request.Form["CenterID"];
                string FeID = Request.Form["FeID"];
                string LpID = Request.Form["LpID"];
                string AsOnDate = Request.Form["AsOnDt"];
                string FromDate = Request.Form["FromDt"];
                string ToDate = Request.Form["ToDt"];
                string Month = Request.Form["Month"];
                string Year = Request.Form["Year"];


                string OpResult = CustEnum.Generic.Error_Default.ToString();
                ReportBuilder objRprt = new ReportBuilder();

                if (ReportID == DbSettings.ReportType.LoanDisbursementReport)
                {

                    OpResult = objRprt.LoanDisbursementReport(BranchID, FromDate, ToDate);
                }

                if (ReportID == DbSettings.ReportType.OutstandingReportBranch)
                {
                    OpResult = objRprt.LoanOutstandingBranchReport(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.OutstandingReportProduct)
                {
                    OpResult = objRprt.LoanOutstandingProductReport(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.OutstandingReportVillage)
                {
                    OpResult = objRprt.LoanOutstandingVillageReport(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.OverdueReportBranch)
                {
                    OpResult = objRprt.LoanOverdueBranch(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.OverdueReportFE)
                {
                    OpResult = objRprt.LoanOverdueFE(BranchID, AsOnDate, FeID);
                }

                if (ReportID == DbSettings.ReportType.OverdueReportCenter)
                {
                    OpResult = objRprt.LoanOverdueCenter(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.OverdueReportProduct)
                {
                    OpResult = objRprt.LoanOverdueProduct(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.OverdueReportVillage)
                {
                    OpResult = objRprt.LoanOverdueVillage(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.PurposeWiseLoanPortfolio)
                {
                    OpResult = objRprt.PurposeWiseLoanPortfolio(BranchID, AsOnDate);
                }

                if (ReportID == DbSettings.ReportType.DuevsCollectionBranch)
                {
                    OpResult = objRprt.DueVsCollecBranch(BranchID, FromDate, ToDate);
                }

                if (ReportID == DbSettings.ReportType.DuevsCollectionCenter)
                {
                    OpResult = objRprt.DueVsCollecCenter(BranchID, CenterID, FromDate, ToDate);
                }

                if (ReportID == DbSettings.ReportType.DuevsCollectionFE)
                {
                    OpResult = objRprt.DueVsCollecFE(BranchID, FeID, FromDate, ToDate);
                }

                if (ReportID == DbSettings.ReportType.FEDetailed)
                {
                    if (FeID != "0")
                        OpResult = objRprt.FeDetailed(FeID, FromDate, ToDate);
                }

                if (ReportID == DbSettings.ReportType.MonthlyBranchStatus)
                {
                    FromDate = Year + "-" + Month + "-1";
                    ToDate = Year + "-" + Month + "-" + DateTime.DaysInMonth(int.Parse(Year), int.Parse(Month)).ToString();
                    OpResult = objRprt.MasterBranchReport(BranchID, FromDate, ToDate);
                }

                Response.Write(OpResult);
                Response.End();
            }

        }
    }
}