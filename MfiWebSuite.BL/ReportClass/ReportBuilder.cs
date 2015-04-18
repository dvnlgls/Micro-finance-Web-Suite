using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.MfiClass;

namespace MfiWebSuite.BL.ReportClass
{
    public class ReportBuilder
    {
        public string LoanDisbursementReport(string BranchID, string FromDate, string ToDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.LoanDisb(BranchID, FromDate, ToDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Loan ID</th>");
                sbrHTML.Append("<th>Client Name</th>");
                sbrHTML.Append("<th>Age</th>");
                sbrHTML.Append("<th>Village</th>");
                sbrHTML.Append("<th>FE Name</th>");
                sbrHTML.Append("<th>Amnt. Applied</th>");
                sbrHTML.Append("<th>Amnt. Disbursed</th>");
                sbrHTML.Append("<th>Loan Cycle</th>");
                sbrHTML.Append("<th>Purpose</th>");
                sbrHTML.Append("<th>Sanction Date</th>");
                sbrHTML.Append("<th>Disbursement Date</th>");
                sbrHTML.Append("<th>Tenure</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Fund Source</th>");
                sbrHTML.Append("</tr>");
                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string LoanID = dr["LoanID"].ToString();
                    string ClientName = dr["ClientName"].ToString();
                    string Age = dr["Age"].ToString();
                    string VillageName = dr["VillageName"].ToString();

                    string FeName = dr["FeName"].ToString();
                    string AmountApplied = dr["AmountApplied"].ToString();
                    string AmountDisbursed = dr["AmountDisbursed"].ToString();

                    string LoanCycle = dr["LoanCycle"].ToString();
                    string PurposeName = dr["PurposeName"].ToString();
                    string DateOfSanction = dr["DateOfSanction"].ToString();
                    string DateOfDisbursement = dr["DateOfDisbursement"].ToString();

                    string Tenure = dr["Tenure"].ToString();
                    string Interest = dr["Interest"].ToString();
                    string FundSourceName = dr["FundSourceName"].ToString();


                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + LoanID + "</td>");
                    sbrHTML.Append("<td>" + ClientName + "</td>");
                    sbrHTML.Append("<td>" + Age + "</td>");
                    sbrHTML.Append("<td>" + VillageName + "</td>");
                    sbrHTML.Append("<td>" + FeName + "</td>");
                    sbrHTML.Append("<td>" + AmountApplied + "</td>");
                    sbrHTML.Append("<td>" + AmountDisbursed + "</td>");
                    sbrHTML.Append("<td>" + LoanCycle + "</td>");
                    sbrHTML.Append("<td>" + PurposeName + "</td>");
                    sbrHTML.Append("<td>" + DateTime.Parse(DateOfSanction).ToDateShortMonth(false, '-') + "</td>");
                    sbrHTML.Append("<td>" + DateTime.Parse(DateOfDisbursement).ToDateShortMonth(false, '-') + "</td>");
                    sbrHTML.Append("<td>" + Tenure + "</td>");
                    sbrHTML.Append("<td>" + Interest + "</td>");
                    sbrHTML.Append("<td>" + FundSourceName + "</td>");
                    sbrHTML.Append("</tr>");
                }

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        //outstanding reports
        public string LoanOutstandingBranchReport(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='2'>Loan ID</th>");
                sbrHTML.Append("<th rowspan='2'>Client Name</th>");
                sbrHTML.Append("<th rowspan='2'>Village</th>");
                sbrHTML.Append("<th rowspan='2'>Disbursement Date</th>");
                sbrHTML.Append("<th rowspan='2'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th rowspan='2'>Balance Principal</th>");
                sbrHTML.Append("<th rowspan='2'>Interest Accrued</th>");
                sbrHTML.Append("<th rowspan='2'>Total</th>");
                sbrHTML.Append("<th rowspan='2'>IDPD</th>");
                sbrHTML.Append("<th colspan='3'>Overdue Amnt.</th>");
                sbrHTML.Append("<th rowspan='2'>O.D. Days</th>");
                //sbrHTML.Append("<th rowspan='2'>No Repayment since 'N' days</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];

                //filter unique loans
                var distinctLoans = (from row in dt.AsEnumerable()
                                     select row.Field<int>("LoanID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var UniqueLoanID in distinctLoans)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                           select myRow).First();

                    string LoanID = ReferenceRecord["LoanID"].ToString();
                    string ClientName = ReferenceRecord["ClientName"].ToString();
                    string VillageName = ReferenceRecord["VillageName"].ToString();
                    string DateOfDisbursement = ReferenceRecord["DateOfDisbursement"].ToString();

                    double AmountDisbursed;
                    double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);


                    double CollectedOverduePrincipal;
                    double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);

                    double CollectedPrincipal;
                    double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);

                    double CollectedOverdueInterest;
                    double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);

                    double CollectedInterest;
                    double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);

                    double RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                    double RepaidInterest = CollectedOverdueInterest + CollectedInterest;

                    double RepaidTotal = RepaidPrincipal + RepaidInterest;

                    double BalancePrincipal = AmountDisbursed - RepaidPrincipal;

                    double Interest;
                    double.TryParse(ReferenceRecord["Interest"].ToString(), out Interest);

                    DateTime LastRepDate;
                    var objLastRepDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'");

                    try
                    {
                        LastRepDate = DateTime.Parse(objLastRepDate.ToString());
                    }
                    catch (Exception ex)
                    {
                        LastRepDate = DateTime.Parse(DateOfDisbursement);
                    }

                    int DayDiff = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastRepDate.ToString())).Days;

                    double IDPD = Math.Round(((Interest / 36500) * BalancePrincipal), 2, MidpointRounding.AwayFromZero);

                    double IntAccrued = Math.Round(IDPD * DayDiff, 2, MidpointRounding.AwayFromZero);

                    double TotalAsOn = BalancePrincipal + IntAccrued;

                    double ExpectedInterest;
                    double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);

                    double ExpectedPrincipal;
                    double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                    double OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                    double OverdueInterest = ExpectedInterest - RepaidInterest;
                    int ODDays = 0;

                    if ((OverduePrincipal + OverdueInterest) > 0)
                    {
                        var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + AsOnDate + "'");

                        try
                        {
                            DateTime.Parse(LastFullPaidDate.ToString());
                        }
                        catch (Exception ex)
                        {
                            //if no full payment till now, get the first planned rep date.
                            LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + LoanID + "'");
                        }

                        ODDays = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                    }

                    #region no-rep
                    //var NoRepSinceNDays = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND TotalCollected = '0'  AND PlannedDate <= '" + AsOnDate + "'");
                    //string strNoRepSinceNDays = "NA";

                    //try
                    //{
                    //    DateTime.Parse(NoRepSinceNDays.ToString());
                    //    strNoRepSinceNDays = (DateTime.Parse(AsOnDate) - DateTime.Parse(NoRepSinceNDays.ToString())).Days.ToString();
                    //}
                    //catch (Exception ex)
                    //{

                    //}

                    ////if (NoRepSinceNDays != null)
                    ////{

                    ////}

                    //var ZeroAmntPaidCount = dt.Compute("Count(LoanID)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND TotalCollected = '0'  AND PlannedDate <= '" + AsOnDate + "'");

                    //int NoRepCtr = 0;

                    //try
                    //{
                    //    int.TryParse(ZeroAmntPaidCount.ToString(), out NoRepCtr);

                    //}
                    //catch (Exception ex)
                    //{

                    //}

                    //if (NoRepCtr > 0)
                    //{
                    //    var LoanRecords = from myRow in dt.AsEnumerable()
                    //                      where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") <= DateTime.Parse(AsOnDate)
                    //                      select myRow;

                    //    if (LoanRecords.Count() > 1)
                    //    {
                    //        var LastReferenceDt = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'");
                    //        var ImmediatePredecessorDt = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + LastReferenceDt + "'");

                    //        var PredecessorTotalAmnt = (from myRow in dt.AsEnumerable()
                    //                          where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") == DateTime.Parse(ImmediatePredecessorDt.ToString())
                    //                          select myRow).First();

                    //        if (double.Parse(PredecessorTotalAmnt["TotalCollected"].ToString()) == 0)
                    //        {

                    //        }

                    //    }
                    //    else
                    //    {
                    //        //find difference
                    //    }

                    //    foreach (DataRow dr in LoanRecords)
                    //    {
                    //    }
                    //}

                    #endregion

                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + LoanID + "</td>");
                    sbrHTML.Append("<td>" + ClientName + "</td>");
                    sbrHTML.Append("<td>" + VillageName + "</td>");
                    sbrHTML.Append("<td>" + DateTime.Parse(DateOfDisbursement).ToDateShortMonth(false, '-') + "</td>");
                    sbrHTML.Append("<td>" + AmountDisbursed + "</td>");
                    sbrHTML.Append("<td>" + RepaidPrincipal + "</td>");
                    sbrHTML.Append("<td>" + RepaidInterest + "</td>");
                    sbrHTML.Append("<td>" + RepaidTotal + "</td>");
                    sbrHTML.Append("<td>" + BalancePrincipal + "</td>");
                    sbrHTML.Append("<td>" + IntAccrued + "</td>");
                    sbrHTML.Append("<td>" + TotalAsOn + "</td>");
                    sbrHTML.Append("<td>" + IDPD + "</td>");
                    sbrHTML.Append("<td>" + OverduePrincipal + "</td>");
                    sbrHTML.Append("<td>" + OverdueInterest + "</td>");
                    sbrHTML.Append("<td>" + (OverduePrincipal + OverdueInterest).ToString() + "</td>");
                    sbrHTML.Append("<td>" + ODDays + "</td>");
                    //sbrHTML.Append("<td>" + strNoRepSinceNDays + "</td>");
                    sbrHTML.Append("</tr>");

                }

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string LoanOutstandingProductReport(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                //sbrHTML.Append("<th rowspan='2'>Product Code</th>");
                sbrHTML.Append("<th rowspan='2'>Loan Product Name</th>");
                sbrHTML.Append("<th rowspan='2'>No. of Clients</th>");
                sbrHTML.Append("<th rowspan='2'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th rowspan='2'>Balance Principal</th>");
                sbrHTML.Append("<th rowspan='2'>Interest Accrued</th>");
                sbrHTML.Append("<th rowspan='2'>Total</th>");
                sbrHTML.Append("<th rowspan='2'>IDPD</th>");
                sbrHTML.Append("<th colspan='3'>Overdue Amnt.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];
                double GrossRepaidP;
                double GrossRepaidI;
                double GrossOverdueP;
                double GrossOverdueI;

                //filter unique products
                var distinctProducts = (from row in dt.AsEnumerable()
                                        select row.Field<int>("LoanProductID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var LoanProductID in distinctProducts)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanProductID") == int.Parse(LoanProductID.ToString())
                                           select myRow).First();

                    string ProductName = ReferenceRecord["ProductName"].ToString();

                    int ClientCount = 0;

                    var NoOfClients = (from row in dt.AsEnumerable()
                                       where row.Field<int>("LoanProductID") == int.Parse(LoanProductID.ToString())
                                       select row.Field<int>("ClientID")).Distinct();

                    if (NoOfClients.Count() > 0)
                        ClientCount = NoOfClients.Count();

                    var Loans = (from row in dt.AsEnumerable()
                                 where row.Field<int>("LoanProductID") == int.Parse(LoanProductID.ToString())
                                 select row.Field<int>("LoanID")).Distinct();

                    double ProductAmountDisbursed = 0;
                    double AmountDisbursed;

                    double ProductCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double ProductCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double ProductCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double ProductCollectedInterest = 0;
                    double CollectedInterest;

                    double ProductRepaidPrincipal = 0;
                    double RepaidPrincipal;

                    double ProductRepaidInterest = 0;
                    double RepaidInterest;

                    double ProductBalancePrincipal = 0;
                    double BalancePrincipal;

                    double ProductIntAccrued = 0;
                    double IntAccrued;

                    double ProductIDPD = 0;
                    double IDPD;

                    double ProductOverduePrincipal = 0;
                    double OverduePrincipal;

                    double ProductOverdueInterest = 0;
                    double OverdueInterest;


                    foreach (var UniqueLoanID in Loans)
                    {
                        var LoanRecord = (from myRow in dt.AsEnumerable()
                                          where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                          select myRow).First();

                        AmountDisbursed = double.Parse(LoanRecord["AmountDisbursed"].ToString());
                        ProductAmountDisbursed += AmountDisbursed;

                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);
                        ProductCollectedOverduePrincipal += CollectedOverduePrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);
                        ProductCollectedPrincipal += CollectedPrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);
                        ProductCollectedOverdueInterest += CollectedOverdueInterest;

                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);
                        ProductCollectedInterest += CollectedInterest;

                        RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                        ProductRepaidPrincipal += RepaidPrincipal;

                        RepaidInterest = CollectedOverdueInterest + CollectedInterest;
                        ProductRepaidInterest += RepaidInterest;

                        BalancePrincipal = AmountDisbursed - RepaidPrincipal;
                        ProductBalancePrincipal += BalancePrincipal;

                        double Interest;
                        double.TryParse(LoanRecord["Interest"].ToString(), out Interest);

                        var LastRepDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'");

                        int DayDiff = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastRepDate.ToString())).Days;

                        IDPD = Math.Round(((Interest / 36500) * BalancePrincipal), 2, MidpointRounding.AwayFromZero);
                        ProductIDPD += IDPD;

                        IntAccrued = Math.Round(IDPD * DayDiff, 2, MidpointRounding.AwayFromZero);
                        ProductIntAccrued += IntAccrued;


                        //overdue calculations
                        double ExpectedInterest;
                        double ExpectedPrincipal;

                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);
                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                        OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                        ProductOverduePrincipal += OverduePrincipal;

                        OverdueInterest = ExpectedInterest - RepaidInterest;
                        ProductOverdueInterest += OverdueInterest;

                    }


                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + ProductName + "</td>");
                    sbrHTML.Append("<td>" + ClientCount + "</td>");
                    sbrHTML.Append("<td>" + ProductAmountDisbursed + "</td>");
                    sbrHTML.Append("<td>" + ProductRepaidPrincipal + "</td>");
                    sbrHTML.Append("<td>" + ProductRepaidInterest + "</td>");
                    sbrHTML.Append("<td>" + (ProductRepaidPrincipal + ProductRepaidInterest).ToString() + "</td>");
                    sbrHTML.Append("<td>" + ProductBalancePrincipal + "</td>");
                    sbrHTML.Append("<td>" + ProductIntAccrued + "</td>");
                    sbrHTML.Append("<td>" + (ProductBalancePrincipal + ProductIntAccrued).ToString() + "</td>");
                    sbrHTML.Append("<td>" + ProductIDPD + "</td>");
                    sbrHTML.Append("<td>" + ProductOverduePrincipal + "</td>");
                    sbrHTML.Append("<td>" + ProductOverdueInterest + "</td>");
                    sbrHTML.Append("<td>" + (ProductOverduePrincipal + ProductOverdueInterest).ToString() + "</td>");

                    sbrHTML.Append("</tr>");

                }

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string LoanOutstandingVillageReport(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();
            Office objOff = new Office();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);
            DataSet dsVillageStat = objOff.GetVillageStatistics(BranchID);

            if (!DataUtils.IsDataSetNull(ds, 0) && !DataUtils.IsDataSetNull(dsVillageStat, 0))
            {
                DataTable dt = ds.Tables[0];
                DataTable dtVil = dsVillageStat.Tables[0];

                double GrossRepaidP;
                double GrossRepaidI;
                double GrossOverdueP;
                double GrossOverdueI;

                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                //sbrHTML.Append("<th rowspan='2'>Product Code</th>");
                sbrHTML.Append("<th rowspan='2'>Village Name</th>");
                sbrHTML.Append("<th rowspan='2'>Total Centers</th>");
                sbrHTML.Append("<th rowspan='2'>Total Groups</th>");
                sbrHTML.Append("<th rowspan='2'>Total Members</th>");
                sbrHTML.Append("<th rowspan='2'>Total Active Clients</th>");
                sbrHTML.Append("<th rowspan='2'>No of Loans</th>");
                sbrHTML.Append("<th rowspan='2'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th rowspan='2'>Balance Principal</th>");
                sbrHTML.Append("<th rowspan='2'>Interest Accrued</th>");
                sbrHTML.Append("<th rowspan='2'>Total</th>");
                sbrHTML.Append("<th rowspan='2'>IDPD</th>");
                sbrHTML.Append("<th colspan='3'>Overdue Amnt.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");



                //filter unique products
                var distinctVillages = (from row in dt.AsEnumerable()
                                        select row.Field<int>("VillageID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var VillageID in distinctVillages)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                           select myRow).First();

                    string VillageName = ReferenceRecord["VillageName"].ToString();

                    var TotalCenters = (from row in dtVil.AsEnumerable()
                                        where row.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                        select row.Field<int>("TotalCenters")).SingleOrDefault();

                    var TotalGroups = (from row in dtVil.AsEnumerable()
                                       where row.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                       select row.Field<int>("TotalGroups")).SingleOrDefault();

                    var TotalMembers = (from row in dtVil.AsEnumerable()
                                        where row.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                        select row.Field<int>("TotalClients")).SingleOrDefault();


                    int ActiveClientCount = (from row in dt.AsEnumerable()
                                             where row.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                             select row.Field<int>("ClientID")).Distinct().Count();

                    int ActiveLoansCount = 0;

                    var Loans = (from row in dt.AsEnumerable()
                                 where row.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                 select row.Field<int>("LoanID")).Distinct();

                    ActiveLoansCount = Loans.Count();

                    double EntityAmountDisbursed = 0;
                    double AmountDisbursed;

                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double EntityCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double EntityCollectedInterest = 0;
                    double CollectedInterest;

                    double EntityRepaidPrincipal = 0;
                    double RepaidPrincipal;

                    double EntityRepaidInterest = 0;
                    double RepaidInterest;

                    double EntityBalancePrincipal = 0;
                    double BalancePrincipal;

                    double EntityIntAccrued = 0;
                    double IntAccrued;

                    double EntityIDPD = 0;
                    double IDPD;

                    double EntityOverduePrincipal = 0;
                    double OverduePrincipal;

                    double EntityOverdueInterest = 0;
                    double OverdueInterest;


                    foreach (var UniqueLoanID in Loans)
                    {
                        var LoanRecord = (from myRow in dt.AsEnumerable()
                                          where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                          select myRow).First();

                        AmountDisbursed = double.Parse(LoanRecord["AmountDisbursed"].ToString());
                        EntityAmountDisbursed += AmountDisbursed;

                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);
                        EntityCollectedOverduePrincipal += CollectedOverduePrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);
                        EntityCollectedPrincipal += CollectedPrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);
                        EntityCollectedOverdueInterest += CollectedOverdueInterest;

                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);
                        EntityCollectedInterest += CollectedInterest;

                        RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                        EntityRepaidPrincipal += RepaidPrincipal;

                        RepaidInterest = CollectedOverdueInterest + CollectedInterest;
                        EntityRepaidInterest += RepaidInterest;

                        BalancePrincipal = AmountDisbursed - RepaidPrincipal;
                        EntityBalancePrincipal += BalancePrincipal;

                        double Interest;
                        double.TryParse(LoanRecord["Interest"].ToString(), out Interest);

                        var LastRepDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'");

                        int DayDiff = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastRepDate.ToString())).Days;

                        IDPD = Math.Round(((Interest / 36500) * BalancePrincipal), 2, MidpointRounding.AwayFromZero);
                        EntityIDPD += IDPD;

                        IntAccrued = Math.Round(IDPD * DayDiff, 2, MidpointRounding.AwayFromZero);
                        EntityIntAccrued += IntAccrued;


                        //overdue calculations
                        double ExpectedInterest;
                        double ExpectedPrincipal;

                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);
                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                        OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                        EntityOverduePrincipal += OverduePrincipal;

                        OverdueInterest = ExpectedInterest - RepaidInterest;
                        EntityOverdueInterest += OverdueInterest;

                        //html
                        sbrHTML.Append("<tr>");
                        sbrHTML.Append("<td>" + VillageName + "</td>");
                        sbrHTML.Append("<td>" + TotalCenters + "</td>");
                        sbrHTML.Append("<td>" + TotalGroups + "</td>");
                        sbrHTML.Append("<td>" + TotalMembers + "</td>");
                        sbrHTML.Append("<td>" + ActiveClientCount + "</td>");
                        sbrHTML.Append("<td>" + ActiveLoansCount + "</td>");
                        sbrHTML.Append("<td>" + EntityAmountDisbursed + "</td>");
                        sbrHTML.Append("<td>" + EntityRepaidPrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityRepaidInterest + "</td>");
                        sbrHTML.Append("<td>" + (EntityRepaidPrincipal + EntityRepaidInterest).ToString() + "</td>");
                        sbrHTML.Append("<td>" + EntityBalancePrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityIntAccrued + "</td>");
                        sbrHTML.Append("<td>" + (EntityBalancePrincipal + EntityIntAccrued).ToString() + "</td>");
                        sbrHTML.Append("<td>" + EntityIDPD + "</td>");
                        sbrHTML.Append("<td>" + EntityOverduePrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityOverdueInterest + "</td>");
                        sbrHTML.Append("<td>" + (EntityOverduePrincipal + EntityOverdueInterest).ToString() + "</td>");

                        sbrHTML.Append("</tr>");

                    }




                }

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }


        //overdue reports
        public string LoanOverdueBranch(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='3'>Loan ID</th>");
                sbrHTML.Append("<th rowspan='3'>Client Name</th>");
                sbrHTML.Append("<th rowspan='3'>FE Name</th>");
                sbrHTML.Append("<th rowspan='3'>Village</th>");
                sbrHTML.Append("<th rowspan='3'>Loan Product</th>");
                sbrHTML.Append("<th rowspan='3'>Disbursement Date</th>");
                sbrHTML.Append("<th rowspan='3'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th rowspan='3'>Last Repayment Date</th>");
                sbrHTML.Append("<th rowspan='2' colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th colspan='12'>Age Wise Analysis</th>");
                sbrHTML.Append("<th rowspan='3'>Due Date</th>");
                sbrHTML.Append("<th rowspan='3'>No of days O.D.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th colspan='3'>1-30 Days</th>");
                sbrHTML.Append("<th colspan='3'>31-60 Days</th>");
                sbrHTML.Append("<th colspan='3'>61-90 Days</th>");
                sbrHTML.Append("<th colspan='3'>Above 90 Days</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];

                //filter unique loans
                var distinctLoans = (from row in dt.AsEnumerable()
                                     select row.Field<int>("LoanID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var UniqueLoanID in distinctLoans)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                           select myRow).First();

                    string LoanID = ReferenceRecord["LoanID"].ToString();
                    string ClientName = ReferenceRecord["ClientName"].ToString();
                    string FeName = ReferenceRecord["FeName"].ToString();
                    string VillageName = ReferenceRecord["VillageName"].ToString();
                    string ProductName = ReferenceRecord["ProductName"].ToString();
                    string DateOfDisbursement = ReferenceRecord["DateOfDisbursement"].ToString();

                    double AmountDisbursed;
                    double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);

                    string LastRepDate;
                    var objLastRepDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'");

                    try
                    {
                        LastRepDate = DateTime.Parse(objLastRepDate.ToString()).ToString(); //two conversions necessary to check parsing.
                    }
                    catch (Exception ex)
                    {
                        LastRepDate = "NA";
                    }

                    string NxtRepDate;
                    var objNxtRepDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + LoanID + "' AND PlannedDate > '" + AsOnDate + "'");

                    try
                    {
                        NxtRepDate = DateTime.Parse(objLastRepDate.ToString()).ToString(); //two conversions necessary to check parsing.
                    }
                    catch (Exception ex)
                    {
                        NxtRepDate = "NA";
                    }

                    double CollectedOverduePrincipal;
                    double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);

                    double CollectedPrincipal;
                    double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);

                    double CollectedOverdueInterest;
                    double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);

                    double CollectedInterest;
                    double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);

                    double RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                    double RepaidInterest = CollectedOverdueInterest + CollectedInterest;
                    double RepaidTotal = RepaidPrincipal + RepaidInterest;

                    double Interest;
                    double.TryParse(ReferenceRecord["Interest"].ToString(), out Interest);

                    double ExpectedInterest;
                    double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);

                    double ExpectedPrincipal;
                    double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                    double OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                    double OverdueInterest = ExpectedInterest - RepaidInterest;
                    int ODDays = 0;

                    double ODP130 = 0;
                    double ODI130 = 0;
                    double ODP3160 = 0;
                    double ODI3160 = 0;
                    double ODP6190 = 0;
                    double ODI6190 = 0;
                    double ODP90Plus = 0;
                    double ODI90Plus = 0;

                    if ((OverduePrincipal + OverdueInterest) > 0)
                    {
                        var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + AsOnDate + "'");

                        try
                        {
                            DateTime.Parse(LastFullPaidDate.ToString());
                        }
                        catch (Exception ex)
                        {
                            //if no full payment till now, get the first planned rep date.
                            LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + LoanID + "'");
                        }

                        ODDays = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                        if (ODDays <= 30)
                        {
                            ODP130 = OverduePrincipal;
                            ODI130 = OverdueInterest;
                        }
                        else if (ODDays > 30 && ODDays <= 60)
                        {
                            ODP3160 = OverduePrincipal;
                            ODI3160 = OverdueInterest;
                        }
                        else if (ODDays > 60 && ODDays <= 90)
                        {
                            ODP6190 = OverduePrincipal;
                            ODI6190 = OverdueInterest;
                        }
                        else
                        {
                            ODP90Plus = OverduePrincipal;
                            ODI90Plus = OverdueInterest;
                        }
                    }


                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + LoanID + "</td>");
                    sbrHTML.Append("<td>" + ClientName + "</td>");
                    sbrHTML.Append("<td>" + FeName + "</td>");
                    sbrHTML.Append("<td>" + VillageName + "</td>");
                    sbrHTML.Append("<td>" + ProductName + "</td>");
                    sbrHTML.Append("<td>" + DateTime.Parse(DateOfDisbursement).ToDateShortMonth(false, '-') + "</td>");
                    sbrHTML.Append("<td>" + AmountDisbursed + "</td>");
                    sbrHTML.Append("<td>" + LastRepDate + "</td>");
                    sbrHTML.Append("<td>" + RepaidPrincipal + "</td>");
                    sbrHTML.Append("<td>" + RepaidInterest + "</td>");
                    sbrHTML.Append("<td>" + RepaidTotal + "</td>");

                    sbrHTML.Append("<td>" + ODP130 + "</td>");
                    sbrHTML.Append("<td>" + ODI130 + "</td>");
                    sbrHTML.Append("<td>" + (ODP130 + ODI130).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0

                    sbrHTML.Append("<td>" + ODP3160 + "</td>");
                    sbrHTML.Append("<td>" + ODI3160 + "</td>");
                    sbrHTML.Append("<td>" + (ODP3160 + ODI3160).ToString() + "</td>");

                    sbrHTML.Append("<td>" + ODP6190 + "</td>");
                    sbrHTML.Append("<td>" + ODI6190 + "</td>");
                    sbrHTML.Append("<td>" + (ODP6190 + ODI6190).ToString() + "</td>");

                    sbrHTML.Append("<td>" + ODP90Plus + "</td>");
                    sbrHTML.Append("<td>" + ODI90Plus + "</td>");
                    sbrHTML.Append("<td>" + (ODP90Plus + ODI90Plus).ToString() + "</td>");

                    sbrHTML.Append("<td>" + NxtRepDate + "</td>");
                    sbrHTML.Append("<td>" + ODDays + "</td>");

                    sbrHTML.Append("</tr>");

                }

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string LoanOverdueFE(string BranchID, string AsOnDate, string FeID)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='3'>Loan ID</th>");
                sbrHTML.Append("<th rowspan='3'>Client Name</th>");
                sbrHTML.Append("<th rowspan='3'>Village</th>");
                sbrHTML.Append("<th rowspan='3'>Loan Product</th>");
                sbrHTML.Append("<th rowspan='3'>Disbursement Date</th>");
                sbrHTML.Append("<th rowspan='3'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th rowspan='3'>Last Repayment Date</th>");
                sbrHTML.Append("<th rowspan='2' colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th colspan='12'>Age Wise Analysis</th>");
                sbrHTML.Append("<th rowspan='3'>Due Date</th>");
                sbrHTML.Append("<th rowspan='3'>No of days O.D.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th colspan='3'>1-30 Days</th>");
                sbrHTML.Append("<th colspan='3'>31-60 Days</th>");
                sbrHTML.Append("<th colspan='3'>61-90 Days</th>");
                sbrHTML.Append("<th colspan='3'>Above 90 Days</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];

                //filter unique loans
                var distinctLoans = (from row in dt.AsEnumerable()
                                     select row.Field<int>("LoanID")).Distinct();

                if (FeID != "0" && FeID != null)
                {
                    distinctLoans = null;
                    distinctLoans = (from row in dt.AsEnumerable()
                                     where row.Field<int>("FeID") == int.Parse(FeID)
                                     select row.Field<int>("LoanID")).Distinct();
                }


                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var UniqueLoanID in distinctLoans)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                           select myRow).First();

                    string LoanID = ReferenceRecord["LoanID"].ToString();
                    string ClientName = ReferenceRecord["ClientName"].ToString();
                    string FeName = ReferenceRecord["FeName"].ToString();
                    string VillageName = ReferenceRecord["VillageName"].ToString();
                    string ProductName = ReferenceRecord["ProductName"].ToString();
                    string DateOfDisbursement = ReferenceRecord["DateOfDisbursement"].ToString();

                    double AmountDisbursed;
                    double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);

                    string LastRepDate;
                    var objLastRepDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'");

                    try
                    {
                        LastRepDate = DateTime.Parse(objLastRepDate.ToString()).ToString(); //two conversions necessary to check parsing.
                    }
                    catch (Exception ex)
                    {
                        LastRepDate = "NA";
                    }

                    string NxtRepDate;
                    var objNxtRepDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + LoanID + "' AND PlannedDate > '" + AsOnDate + "'");

                    try
                    {
                        NxtRepDate = DateTime.Parse(objLastRepDate.ToString()).ToString(); //two conversions necessary to check parsing.
                    }
                    catch (Exception ex)
                    {
                        NxtRepDate = "NA";
                    }

                    double CollectedOverduePrincipal;
                    double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);

                    double CollectedPrincipal;
                    double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);

                    double CollectedOverdueInterest;
                    double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);

                    double CollectedInterest;
                    double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + LoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);

                    double RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                    double RepaidInterest = CollectedOverdueInterest + CollectedInterest;
                    double RepaidTotal = RepaidPrincipal + RepaidInterest;

                    double Interest;
                    double.TryParse(ReferenceRecord["Interest"].ToString(), out Interest);

                    double ExpectedInterest;
                    double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);

                    double ExpectedPrincipal;
                    double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                    double OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                    double OverdueInterest = ExpectedInterest - RepaidInterest;
                    int ODDays = 0;

                    double ODP130 = 0;
                    double ODI130 = 0;
                    double ODP3160 = 0;
                    double ODI3160 = 0;
                    double ODP6190 = 0;
                    double ODI6190 = 0;
                    double ODP90Plus = 0;
                    double ODI90Plus = 0;

                    if ((OverduePrincipal + OverdueInterest) > 0)
                    {
                        var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + LoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + AsOnDate + "'");

                        try
                        {
                            DateTime.Parse(LastFullPaidDate.ToString());
                        }
                        catch (Exception ex)
                        {
                            //if no full payment till now, get the first planned rep date.
                            LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + LoanID + "'");
                        }

                        ODDays = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                        if (ODDays <= 30)
                        {
                            ODP130 = OverduePrincipal;
                            ODI130 = OverdueInterest;
                        }
                        else if (ODDays > 30 && ODDays <= 60)
                        {
                            ODP3160 = OverduePrincipal;
                            ODI3160 = OverdueInterest;
                        }
                        else if (ODDays > 60 && ODDays <= 90)
                        {
                            ODP6190 = OverduePrincipal;
                            ODI6190 = OverdueInterest;
                        }
                        else
                        {
                            ODP90Plus = OverduePrincipal;
                            ODI90Plus = OverdueInterest;
                        }
                    }


                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + LoanID + "</td>");
                    sbrHTML.Append("<td>" + ClientName + "</td>");
                    sbrHTML.Append("<td>" + VillageName + "</td>");
                    sbrHTML.Append("<td>" + ProductName + "</td>");
                    sbrHTML.Append("<td>" + DateTime.Parse(DateOfDisbursement).ToDateShortMonth(false, '-') + "</td>");
                    sbrHTML.Append("<td>" + AmountDisbursed + "</td>");
                    sbrHTML.Append("<td>" + LastRepDate + "</td>");
                    sbrHTML.Append("<td>" + RepaidPrincipal + "</td>");
                    sbrHTML.Append("<td>" + RepaidInterest + "</td>");
                    sbrHTML.Append("<td>" + RepaidTotal + "</td>");

                    sbrHTML.Append("<td>" + ODP130 + "</td>");
                    sbrHTML.Append("<td>" + ODI130 + "</td>");
                    sbrHTML.Append("<td>" + (ODP130 + ODI130).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0

                    sbrHTML.Append("<td>" + ODP3160 + "</td>");
                    sbrHTML.Append("<td>" + ODI3160 + "</td>");
                    sbrHTML.Append("<td>" + (ODP3160 + ODI3160).ToString() + "</td>");

                    sbrHTML.Append("<td>" + ODP6190 + "</td>");
                    sbrHTML.Append("<td>" + ODI6190 + "</td>");
                    sbrHTML.Append("<td>" + (ODP6190 + ODI6190).ToString() + "</td>");

                    sbrHTML.Append("<td>" + ODP90Plus + "</td>");
                    sbrHTML.Append("<td>" + ODI90Plus + "</td>");
                    sbrHTML.Append("<td>" + (ODP90Plus + ODI90Plus).ToString() + "</td>");

                    sbrHTML.Append("<td>" + NxtRepDate + "</td>");
                    sbrHTML.Append("<td>" + ODDays + "</td>");

                    sbrHTML.Append("</tr>");

                }

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string LoanOverdueCenter(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='3'>Center</th>");
                sbrHTML.Append("<th rowspan='3'>FE Name</th>");
                sbrHTML.Append("<th rowspan='3'>Village</th>");
                sbrHTML.Append("<th rowspan='3'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th rowspan='2' colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th colspan='16'>Age Wise Analysis</th>");
                sbrHTML.Append("<th rowspan='3'>Total Overdue</th>");
                sbrHTML.Append("<th rowspan='3'>No of A/c.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th colspan='4'>1-30 Days</th>");
                sbrHTML.Append("<th colspan='4'>31-60 Days</th>");
                sbrHTML.Append("<th colspan='4'>61-90 Days</th>");
                sbrHTML.Append("<th colspan='4'>Above 90 Days</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];


                var distinctCenters = (from row in dt.AsEnumerable()
                                       select row.Field<int>("CenterID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var CenterID in distinctCenters)
                {
                    var CenterRecord = (from myRow in dt.AsEnumerable()
                                        where myRow.Field<int>("CenterID") == int.Parse(CenterID.ToString())
                                        select myRow).First();

                    string FeName = CenterRecord["FeName"].ToString();
                    string VillageName = CenterRecord["VillageName"].ToString();

                    //filter unique loans
                    var distinctLoans = (from row in dt.AsEnumerable()
                                         where row.Field<int>("CenterID") == int.Parse(CenterID.ToString())
                                         select row.Field<int>("LoanID")).Distinct();

                    int TotalLoans = distinctLoans.Count();

                    double EntityAmountDisbursed = 0;
                    double AmountDisbursed;

                    double EntityRepaidPrincipal = 0;
                    double RepaidPrincipal;

                    double EntityRepaidInterest = 0;
                    double RepaidInterest;

                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double EntityCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double EntityCollectedInterest = 0;
                    double CollectedInterest;

                    //
                    double Entity_OD_P_1_30 = 0;
                    double OD_P_1_30 = 0;

                    double Entity_OD_I_1_30 = 0;
                    double OD_I_1_30 = 0;

                    double Entity_OD_Loans_1_30 = 0;

                    double Entity_OD_P_31_60 = 0;
                    double OD_P_31_60 = 0;

                    double Entity_OD_I_31_60 = 0;
                    double OD_I_31_60 = 0;

                    double Entity_OD_Loans_31_60 = 0;

                    double Entity_OD_P_61_90 = 0;
                    double OD_P_61_90 = 0;

                    double Entity_OD_I_61_90 = 0;
                    double OD_I_61_90 = 0;

                    double Entity_OD_Loans_61_90 = 0;

                    double Entity_OD_P_GT_90 = 0;
                    double OD_P_GT_90 = 0;

                    double Entity_OD_I_GT_90 = 0;
                    double OD_I_GT_90 = 0;

                    double Entity_OD_Loans_GT_90 = 0;


                    foreach (var UniqueLoanID in distinctLoans)
                    {
                        var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                               where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                               select myRow).First();


                        double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);
                        EntityAmountDisbursed += AmountDisbursed;

                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);
                        EntityCollectedOverduePrincipal += CollectedOverduePrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);
                        EntityCollectedPrincipal += CollectedPrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);
                        EntityCollectedOverdueInterest += CollectedOverdueInterest;

                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);
                        EntityCollectedInterest += CollectedInterest;

                        RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                        EntityRepaidPrincipal += RepaidPrincipal;

                        RepaidInterest = CollectedOverdueInterest + CollectedInterest;
                        EntityRepaidInterest += RepaidInterest;


                        double ExpectedInterest;
                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);

                        double ExpectedPrincipal;
                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                        double OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                        double OverdueInterest = ExpectedInterest - RepaidInterest;

                        int ODDays = 0;


                        if ((OverduePrincipal + OverdueInterest) > 0)
                        {
                            var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + AsOnDate + "'");

                            try
                            {
                                DateTime.Parse(LastFullPaidDate.ToString());
                            }
                            catch (Exception ex)
                            {
                                //if no full payment till now, get the first planned rep date.
                                LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                            }

                            ODDays = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                            if (ODDays <= 30)
                            {
                                OD_P_1_30 = OverduePrincipal;
                                Entity_OD_P_1_30 += OD_P_1_30;

                                OD_I_1_30 = OverdueInterest;
                                Entity_OD_I_1_30 += OD_I_1_30;

                                Entity_OD_Loans_1_30++;
                            }
                            else if (ODDays > 30 && ODDays <= 60)
                            {
                                OD_P_31_60 = OverduePrincipal;
                                Entity_OD_P_31_60 += OD_P_31_60;

                                OD_I_31_60 = OverdueInterest;
                                Entity_OD_I_31_60 += OD_I_31_60;

                                Entity_OD_Loans_31_60++;
                            }
                            else if (ODDays > 60 && ODDays <= 90)
                            {
                                OD_P_61_90 = OverduePrincipal;
                                Entity_OD_P_61_90 += OD_P_61_90;

                                OD_I_61_90 = OverdueInterest;
                                Entity_OD_I_61_90 += OD_I_61_90;

                                Entity_OD_Loans_61_90++;
                            }
                            else
                            {
                                OD_P_GT_90 = OverduePrincipal;
                                Entity_OD_P_GT_90 += OD_P_GT_90;

                                OD_I_GT_90 = OverdueInterest;
                                Entity_OD_I_GT_90 += OD_I_GT_90;

                                Entity_OD_Loans_GT_90++;
                            }
                        }

                    } //unique loans in this center


                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + CenterID + "</td>");
                    sbrHTML.Append("<td>" + FeName + "</td>");
                    sbrHTML.Append("<td>" + VillageName + "</td>");

                    sbrHTML.Append("<td>" + EntityAmountDisbursed + "</td>");

                    sbrHTML.Append("<td>" + EntityRepaidPrincipal + "</td>");
                    sbrHTML.Append("<td>" + EntityRepaidInterest + "</td>");
                    sbrHTML.Append("<td>" + (EntityRepaidPrincipal + EntityRepaidInterest).ToString() + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_1_30 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_1_30 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_1_30 + Entity_OD_I_1_30).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_1_30 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_31_60 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_31_60 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_31_60 + Entity_OD_I_31_60).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_31_60 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_61_90 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_61_90 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_61_90 + Entity_OD_I_61_90).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_61_90 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_GT_90 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_GT_90 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_GT_90 + Entity_OD_I_GT_90).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_GT_90 + "</td>");

                    sbrHTML.Append("<td>" + (Entity_OD_P_1_30 + Entity_OD_I_1_30 + Entity_OD_P_31_60 + Entity_OD_I_31_60 + Entity_OD_P_61_90 + Entity_OD_I_61_90 + Entity_OD_P_GT_90 + Entity_OD_I_GT_90).ToString() + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_Loans_1_30 + Entity_OD_Loans_31_60 + Entity_OD_Loans_61_90 + Entity_OD_Loans_GT_90).ToString() + "</td>");

                    sbrHTML.Append("</tr>");

                }//center loop



                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string LoanOverdueProduct(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='3'>Product</th>");
                sbrHTML.Append("<th rowspan='3'>No of Loans Disbursed</th>");
                sbrHTML.Append("<th rowspan='3'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th rowspan='3'>Demand Principal</th>");
                sbrHTML.Append("<th rowspan='2' colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th colspan='16'>Age Wise Analysis</th>");
                sbrHTML.Append("<th rowspan='3'>Total Overdue</th>");
                sbrHTML.Append("<th rowspan='3'>No of A/c.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th colspan='4'>1-30 Days</th>");
                sbrHTML.Append("<th colspan='4'>31-60 Days</th>");
                sbrHTML.Append("<th colspan='4'>61-90 Days</th>");
                sbrHTML.Append("<th colspan='4'>Above 90 Days</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];


                var distinctProducts = (from row in dt.AsEnumerable()
                                        select row.Field<int>("LoanProductID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var LoanProductID in distinctProducts)
                {
                    var ProductRecord = (from myRow in dt.AsEnumerable()
                                         where myRow.Field<int>("LoanProductID") == int.Parse(LoanProductID.ToString())
                                         select myRow).First();

                    string ProductName = ProductRecord["ProductName"].ToString();

                    //filter unique loans
                    var distinctLoans = (from row in dt.AsEnumerable()
                                         where row.Field<int>("LoanProductID") == int.Parse(LoanProductID.ToString())
                                         select row.Field<int>("LoanID")).Distinct();

                    int TotalLoans = distinctLoans.Count();

                    double EntityAmountDisbursed = 0;
                    double AmountDisbursed;

                    double EntityRepaidPrincipal = 0;
                    double RepaidPrincipal;

                    double EntityRepaidInterest = 0;
                    double RepaidInterest;

                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double EntityCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double EntityCollectedInterest = 0;
                    double CollectedInterest;

                    //
                    double Entity_OD_P_1_30 = 0;
                    double OD_P_1_30 = 0;

                    double Entity_OD_I_1_30 = 0;
                    double OD_I_1_30 = 0;

                    double Entity_OD_Loans_1_30 = 0;

                    double Entity_OD_P_31_60 = 0;
                    double OD_P_31_60 = 0;

                    double Entity_OD_I_31_60 = 0;
                    double OD_I_31_60 = 0;

                    double Entity_OD_Loans_31_60 = 0;

                    double Entity_OD_P_61_90 = 0;
                    double OD_P_61_90 = 0;

                    double Entity_OD_I_61_90 = 0;
                    double OD_I_61_90 = 0;

                    double Entity_OD_Loans_61_90 = 0;

                    double Entity_OD_P_GT_90 = 0;
                    double OD_P_GT_90 = 0;

                    double Entity_OD_I_GT_90 = 0;
                    double OD_I_GT_90 = 0;

                    double Entity_OD_Loans_GT_90 = 0;


                    foreach (var UniqueLoanID in distinctLoans)
                    {
                        var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                               where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                               select myRow).First();


                        double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);
                        EntityAmountDisbursed += AmountDisbursed;

                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);
                        EntityCollectedOverduePrincipal += CollectedOverduePrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);
                        EntityCollectedPrincipal += CollectedPrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);
                        EntityCollectedOverdueInterest += CollectedOverdueInterest;

                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);
                        EntityCollectedInterest += CollectedInterest;

                        RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                        EntityRepaidPrincipal += RepaidPrincipal;

                        RepaidInterest = CollectedOverdueInterest + CollectedInterest;
                        EntityRepaidInterest += RepaidInterest;


                        double ExpectedInterest;
                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);

                        double ExpectedPrincipal;
                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                        double OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                        double OverdueInterest = ExpectedInterest - RepaidInterest;

                        int ODDays = 0;


                        if ((OverduePrincipal + OverdueInterest) > 0)
                        {
                            var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + AsOnDate + "'");

                            try
                            {
                                DateTime.Parse(LastFullPaidDate.ToString());
                            }
                            catch (Exception ex)
                            {
                                //if no full payment till now, get the first planned rep date.
                                LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                            }

                            ODDays = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                            if (ODDays <= 30)
                            {
                                OD_P_1_30 = OverduePrincipal;
                                Entity_OD_P_1_30 += OD_P_1_30;

                                OD_I_1_30 = OverdueInterest;
                                Entity_OD_I_1_30 += OD_I_1_30;

                                Entity_OD_Loans_1_30++;
                            }
                            else if (ODDays > 30 && ODDays <= 60)
                            {
                                OD_P_31_60 = OverduePrincipal;
                                Entity_OD_P_31_60 += OD_P_31_60;

                                OD_I_31_60 = OverdueInterest;
                                Entity_OD_I_31_60 += OD_I_31_60;

                                Entity_OD_Loans_31_60++;
                            }
                            else if (ODDays > 60 && ODDays <= 90)
                            {
                                OD_P_61_90 = OverduePrincipal;
                                Entity_OD_P_61_90 += OD_P_61_90;

                                OD_I_61_90 = OverdueInterest;
                                Entity_OD_I_61_90 += OD_I_61_90;

                                Entity_OD_Loans_61_90++;
                            }
                            else
                            {
                                OD_P_GT_90 = OverduePrincipal;
                                Entity_OD_P_GT_90 += OD_P_GT_90;

                                OD_I_GT_90 = OverdueInterest;
                                Entity_OD_I_GT_90 += OD_I_GT_90;

                                Entity_OD_Loans_GT_90++;
                            }
                        }

                    } //unique loans in this center


                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + ProductName + "</td>");
                    sbrHTML.Append("<td>" + TotalLoans + "</td>");
                    sbrHTML.Append("<td>" + EntityAmountDisbursed + "</td>");
                    sbrHTML.Append("<td>" + (EntityAmountDisbursed - EntityRepaidPrincipal).ToString() + "</td>");

                    sbrHTML.Append("<td>" + EntityRepaidPrincipal + "</td>");
                    sbrHTML.Append("<td>" + EntityRepaidInterest + "</td>");
                    sbrHTML.Append("<td>" + (EntityRepaidPrincipal + EntityRepaidInterest).ToString() + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_1_30 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_1_30 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_1_30 + Entity_OD_I_1_30).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_1_30 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_31_60 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_31_60 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_31_60 + Entity_OD_I_31_60).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_31_60 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_61_90 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_61_90 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_61_90 + Entity_OD_I_61_90).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_61_90 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_GT_90 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_GT_90 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_GT_90 + Entity_OD_I_GT_90).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_GT_90 + "</td>");

                    sbrHTML.Append("<td>" + (Entity_OD_P_1_30 + Entity_OD_I_1_30 + Entity_OD_P_31_60 + Entity_OD_I_31_60 + Entity_OD_P_61_90 + Entity_OD_I_61_90 + Entity_OD_P_GT_90 + Entity_OD_I_GT_90).ToString() + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_Loans_1_30 + Entity_OD_Loans_31_60 + Entity_OD_Loans_61_90 + Entity_OD_Loans_GT_90).ToString() + "</td>");

                    sbrHTML.Append("</tr>");

                }//center loop



                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string LoanOverdueVillage(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.OutstandingGeneric(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='3'>Village</th>");
                sbrHTML.Append("<th rowspan='3'>FE Name</th>");
                sbrHTML.Append("<th rowspan='3'>Disbursed Amnt.</th>");
                sbrHTML.Append("<th rowspan='2' colspan='3'>Repaid till date</th>");
                sbrHTML.Append("<th colspan='16'>Age Wise Analysis</th>");
                sbrHTML.Append("<th rowspan='3'>Total Overdue</th>");
                sbrHTML.Append("<th rowspan='3'>No of A/c.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th colspan='4'>1-30 Days</th>");
                sbrHTML.Append("<th colspan='4'>31-60 Days</th>");
                sbrHTML.Append("<th colspan='4'>61-90 Days</th>");
                sbrHTML.Append("<th colspan='4'>Above 90 Days</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>O.D. Prn</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Total</th>");
                sbrHTML.Append("<th>No of A/c.</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];


                var distinctVillage = (from row in dt.AsEnumerable()
                                       select row.Field<int>("VillageID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();

                foreach (var VillageID in distinctVillage)
                {
                    var VillageRecord = (from myRow in dt.AsEnumerable()
                                         where myRow.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                         select myRow).First();

                    string VillageName = VillageRecord["VillageName"].ToString();
                    string FeName = VillageRecord["FeName"].ToString();

                    //filter unique loans
                    var distinctLoans = (from row in dt.AsEnumerable()
                                         where row.Field<int>("VillageID") == int.Parse(VillageID.ToString())
                                         select row.Field<int>("LoanID")).Distinct();

                    //int TotalLoans = distinctLoans.Count();

                    double EntityAmountDisbursed = 0;
                    double AmountDisbursed;

                    double EntityRepaidPrincipal = 0;
                    double RepaidPrincipal;

                    double EntityRepaidInterest = 0;
                    double RepaidInterest;

                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double EntityCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double EntityCollectedInterest = 0;
                    double CollectedInterest;

                    //
                    double Entity_OD_P_1_30 = 0;
                    double OD_P_1_30 = 0;

                    double Entity_OD_I_1_30 = 0;
                    double OD_I_1_30 = 0;

                    double Entity_OD_Loans_1_30 = 0;

                    double Entity_OD_P_31_60 = 0;
                    double OD_P_31_60 = 0;

                    double Entity_OD_I_31_60 = 0;
                    double OD_I_31_60 = 0;

                    double Entity_OD_Loans_31_60 = 0;

                    double Entity_OD_P_61_90 = 0;
                    double OD_P_61_90 = 0;

                    double Entity_OD_I_61_90 = 0;
                    double OD_I_61_90 = 0;

                    double Entity_OD_Loans_61_90 = 0;

                    double Entity_OD_P_GT_90 = 0;
                    double OD_P_GT_90 = 0;

                    double Entity_OD_I_GT_90 = 0;
                    double OD_I_GT_90 = 0;

                    double Entity_OD_Loans_GT_90 = 0;


                    foreach (var UniqueLoanID in distinctLoans)
                    {
                        var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                               where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                               select myRow).First();


                        double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);
                        EntityAmountDisbursed += AmountDisbursed;

                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverduePrincipal);
                        EntityCollectedOverduePrincipal += CollectedOverduePrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedPrincipal);
                        EntityCollectedPrincipal += CollectedPrincipal;

                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedOverdueInterest);
                        EntityCollectedOverdueInterest += CollectedOverdueInterest;

                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out CollectedInterest);
                        EntityCollectedInterest += CollectedInterest;

                        RepaidPrincipal = CollectedOverduePrincipal + CollectedPrincipal;
                        EntityRepaidPrincipal += RepaidPrincipal;

                        RepaidInterest = CollectedOverdueInterest + CollectedInterest;
                        EntityRepaidInterest += RepaidInterest;


                        double ExpectedInterest;
                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedInterest);

                        double ExpectedPrincipal;
                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + AsOnDate + "'").ToString(), out ExpectedPrincipal);

                        double OverduePrincipal = ExpectedPrincipal - RepaidPrincipal;
                        double OverdueInterest = ExpectedInterest - RepaidInterest;

                        int ODDays = 0;


                        if ((OverduePrincipal + OverdueInterest) > 0)
                        {
                            var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + AsOnDate + "'");

                            try
                            {
                                DateTime.Parse(LastFullPaidDate.ToString());
                            }
                            catch (Exception ex)
                            {
                                //if no full payment till now, get the first planned rep date.
                                LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                            }

                            ODDays = (DateTime.Parse(AsOnDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                            if (ODDays <= 30)
                            {
                                OD_P_1_30 = OverduePrincipal;
                                Entity_OD_P_1_30 += OD_P_1_30;

                                OD_I_1_30 = OverdueInterest;
                                Entity_OD_I_1_30 += OD_I_1_30;

                                Entity_OD_Loans_1_30++;
                            }
                            else if (ODDays > 30 && ODDays <= 60)
                            {
                                OD_P_31_60 = OverduePrincipal;
                                Entity_OD_P_31_60 += OD_P_31_60;

                                OD_I_31_60 = OverdueInterest;
                                Entity_OD_I_31_60 += OD_I_31_60;

                                Entity_OD_Loans_31_60++;
                            }
                            else if (ODDays > 60 && ODDays <= 90)
                            {
                                OD_P_61_90 = OverduePrincipal;
                                Entity_OD_P_61_90 += OD_P_61_90;

                                OD_I_61_90 = OverdueInterest;
                                Entity_OD_I_61_90 += OD_I_61_90;

                                Entity_OD_Loans_61_90++;
                            }
                            else
                            {
                                OD_P_GT_90 = OverduePrincipal;
                                Entity_OD_P_GT_90 += OD_P_GT_90;

                                OD_I_GT_90 = OverdueInterest;
                                Entity_OD_I_GT_90 += OD_I_GT_90;

                                Entity_OD_Loans_GT_90++;
                            }
                        }

                    } //unique loans in this center


                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + VillageName + "</td>");
                    sbrHTML.Append("<td>" + FeName + "</td>");
                    sbrHTML.Append("<td>" + EntityAmountDisbursed + "</td>");

                    sbrHTML.Append("<td>" + EntityRepaidPrincipal + "</td>");
                    sbrHTML.Append("<td>" + EntityRepaidInterest + "</td>");
                    sbrHTML.Append("<td>" + (EntityRepaidPrincipal + EntityRepaidInterest).ToString() + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_1_30 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_1_30 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_1_30 + Entity_OD_I_1_30).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_1_30 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_31_60 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_31_60 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_31_60 + Entity_OD_I_31_60).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_31_60 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_61_90 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_61_90 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_61_90 + Entity_OD_I_61_90).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_61_90 + "</td>");

                    sbrHTML.Append("<td>" + Entity_OD_P_GT_90 + "</td>");
                    sbrHTML.Append("<td>" + Entity_OD_I_GT_90 + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_P_GT_90 + Entity_OD_I_GT_90).ToString() + "</td>"); // w/o tostring conv, you might get 00 if both are 0
                    sbrHTML.Append("<td>" + Entity_OD_Loans_GT_90 + "</td>");

                    sbrHTML.Append("<td>" + (Entity_OD_P_1_30 + Entity_OD_I_1_30 + Entity_OD_P_31_60 + Entity_OD_I_31_60 + Entity_OD_P_61_90 + Entity_OD_I_61_90 + Entity_OD_P_GT_90 + Entity_OD_I_GT_90).ToString() + "</td>");
                    sbrHTML.Append("<td>" + (Entity_OD_Loans_1_30 + Entity_OD_Loans_31_60 + Entity_OD_Loans_61_90 + Entity_OD_Loans_GT_90).ToString() + "</td>");

                    sbrHTML.Append("</tr>");

                }//center loop



                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        //
        public string PurposeWiseLoanPortfolio(string BranchID, string AsOnDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.PurposeLoanPortfolio(BranchID, AsOnDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Purpose</th>");
                sbrHTML.Append("<th>Amount Disbursed</th>");
                sbrHTML.Append("<th>No of Loans Disbursed</th>");
                sbrHTML.Append("<th>No of Active Borrowers </th>");
                sbrHTML.Append("<th>Loan Oustanding</th>");
                sbrHTML.Append("</tr>");
                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                DataTable dt = ds.Tables[0];

                //filter unique products
                var distinctPurpose = (from row in dt.AsEnumerable()
                                       select row.Field<int>("LoanPurposeID")).Distinct();

                AsOnDate = DateTime.Parse(AsOnDate).ToString();


                foreach (var LoanPurposeID in distinctPurpose)
                {
                    double EntityAmountDisbursed = 0;
                    double AmountDisbursed;

                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    int EntityLoans = 0;
                    int EntityClients = 0;

                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanPurposeID") == int.Parse(LoanPurposeID.ToString())
                                           select myRow).First();

                    string PurposeName = ReferenceRecord["PurposeName"].ToString();
                    try
                    {

                        var Loans = (from row in dt.AsEnumerable()
                                     where row.Field<int>("LoanPurposeID") == int.Parse(LoanPurposeID.ToString())
                                     select row.Field<int>("LoanID")).Distinct();

                        EntityLoans += Loans.Count();

                        var Clients = (from row in dt.AsEnumerable()
                                       where row.Field<int>("LoanPurposeID") == int.Parse(LoanPurposeID.ToString())
                                       select row.Field<int>("ClientID")).Distinct();

                        EntityClients += Clients.Count();



                        foreach (var UniqueLoanID in Loans)
                        {
                            var LoanRecord = (from myRow in dt.AsEnumerable()
                                              where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                              select myRow).First();

                            AmountDisbursed = double.Parse(LoanRecord["AmountDisbursed"].ToString());
                            EntityAmountDisbursed += AmountDisbursed;


                            double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "'").ToString(), out CollectedOverduePrincipal);
                            EntityCollectedOverduePrincipal += CollectedOverduePrincipal;

                            double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "'").ToString(), out CollectedPrincipal);
                            EntityCollectedPrincipal += CollectedPrincipal;

                        }
                    }
                    catch (Exception ex) { }

                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + PurposeName + "</td>");
                    sbrHTML.Append("<td>" + EntityAmountDisbursed + "</td>");
                    sbrHTML.Append("<td>" + EntityLoans + "</td>");
                    sbrHTML.Append("<td>" + EntityClients + "</td>");
                    sbrHTML.Append("<td>" + (EntityAmountDisbursed - (EntityCollectedOverduePrincipal + EntityCollectedPrincipal)).ToString() + "</td>");

                    sbrHTML.Append("</tr>");


                }//unique purpose


                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        //due vs collection
        public string DueVsCollecBranch(string BranchID, string FromDate, string ToDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();
            Office objOff = new Office();

            if (BranchID == "0")
                BranchID = null;

            DataSet ds = objRprt.DueVsCollectionGeneric(BranchID, FromDate, ToDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                DataTable dt = ds.Tables[0];

                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='2'>Repayment Date</th>");
                sbrHTML.Append("<th colspan='5'>Due</th>");
                sbrHTML.Append("<th colspan='6'>Collections</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Arrears Principal</th>");
                sbrHTML.Append("<th>Arrears Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Arrears Principal</th>");
                sbrHTML.Append("<th>Arrears Interest</th>");
                sbrHTML.Append("<th>Prepayment</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");



                //filter unique products
                var distinctDates = (from row in dt.AsEnumerable()
                                     select row.Field<DateTime>("PlannedDate")).Distinct();

                foreach (var PlannedDate in distinctDates)
                {

                    var Loans = (from row in dt.AsEnumerable()
                                 where row.Field<DateTime>("PlannedDate") == DateTime.Parse(PlannedDate.ToString())
                                 select row.Field<int>("LoanID")).Distinct();


                    //due
                    double EntityDuePrincipal = 0;
                    double EntityDueInterest = 0;

                    double EntityDueArrearPrincipal = 0;
                    double EntityDueArrearInterest = 0;

                    //collec
                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double EntityCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double EntityCollectedInterest = 0;
                    double CollectedInterest;

                    double EntityTotalCollected = 0;
                    double TotalCollected;

                    foreach (var UniqueLoanID in Loans)
                    {
                        //current due:
                        double DueP;
                        double DueI;

                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out DueP);
                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "'AND PlannedDate = '" + PlannedDate + "'").ToString(), out DueI);

                        EntityDuePrincipal += DueP;
                        EntityDueInterest += DueI;

                        //overdue for this installment:                        
                        double ExpectedPrincipal;
                        double ExpectedInterest;

                        //get how much should have been paid so far (p & I)
                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + PlannedDate + "'").ToString(), out ExpectedInterest);
                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + PlannedDate + "'").ToString(), out ExpectedPrincipal);

                        //get how much has been paid until before current date (p & I)
                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1'  AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedOverduePrincipal);
                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedPrincipal);
                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedOverdueInterest);
                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedInterest);

                        EntityDueArrearPrincipal += ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                        EntityDueArrearInterest += ExpectedInterest = (CollectedOverdueInterest + CollectedInterest);

                        CollectedOverduePrincipal = 0;
                        CollectedPrincipal = 0;
                        CollectedOverdueInterest = 0;
                        CollectedInterest = 0;
                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedOverduePrincipal);
                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedPrincipal);
                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedOverdueInterest);
                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedInterest);
                        double.TryParse(dt.Compute("Sum(TotalCollected)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out TotalCollected);

                        EntityCollectedPrincipal += CollectedPrincipal;
                        EntityCollectedInterest += CollectedInterest;
                        EntityCollectedOverdueInterest += CollectedOverdueInterest;
                        EntityCollectedOverduePrincipal += CollectedOverduePrincipal;
                        EntityTotalCollected += TotalCollected;

                    }

                    //html
                    sbrHTML.Append("<tr>");
                    sbrHTML.Append("<td>" + PlannedDate.ToDateShortMonth(false, '-') + "</td>");
                    sbrHTML.Append("<td>" + EntityDuePrincipal + "</td>");
                    sbrHTML.Append("<td>" + EntityDueInterest + "</td>");
                    sbrHTML.Append("<td>" + EntityDueArrearPrincipal + "</td>");
                    sbrHTML.Append("<td>" + EntityDueArrearInterest + "</td>");
                    sbrHTML.Append("<td>" + (EntityDuePrincipal + EntityDueInterest + EntityDueArrearPrincipal + EntityDueArrearInterest).ToString() + "</td>");
                    sbrHTML.Append("<td>" + EntityCollectedPrincipal + "</td>");
                    sbrHTML.Append("<td>" + EntityCollectedInterest + "</td>");
                    sbrHTML.Append("<td>" + EntityCollectedOverduePrincipal + "</td>");
                    sbrHTML.Append("<td>" + EntityCollectedOverdueInterest + "</td>");
                    sbrHTML.Append("<td>" + (EntityTotalCollected - (EntityCollectedPrincipal + EntityCollectedInterest + EntityCollectedOverduePrincipal + EntityCollectedOverdueInterest)).ToString() + "</td>");
                    sbrHTML.Append("<td>" + EntityTotalCollected + "</td>");
                    sbrHTML.Append("</tr>");

                }

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string DueVsCollecCenter(string BranchID, string CenterID, string FromDate, string ToDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();
            Office objOff = new Office();

            if (BranchID == "0")
                BranchID = null;

            if (CenterID == "0")
                CenterID = null;

            DataSet ds = objRprt.DueVsCollectionGeneric(BranchID, FromDate, ToDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                DataTable dt = ds.Tables[0];

                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='2'>Center ID</th>");
                sbrHTML.Append("<th rowspan='2'>Repayment Date</th>");
                sbrHTML.Append("<th colspan='5'>Due</th>");
                sbrHTML.Append("<th colspan='6'>Collections</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Arrears Principal</th>");
                sbrHTML.Append("<th>Arrears Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Arrears Principal</th>");
                sbrHTML.Append("<th>Arrears Interest</th>");
                sbrHTML.Append("<th>Prepayment</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                var distinctCenters = (from row in dt.AsEnumerable()
                                       select row.Field<int>("CenterID")).Distinct();

                if (CenterID != null)
                {
                    distinctCenters = (from row in dt.AsEnumerable()
                                       where row.Field<int>("CenterID") == int.Parse(CenterID)
                                       select row.Field<int>("CenterID")).Distinct();
                }

                foreach (var UniqueCenterID in distinctCenters)
                {

                    //filter unique dates
                    var distinctDates = (from row in dt.AsEnumerable()
                                         where row.Field<int>("CenterID") == int.Parse(UniqueCenterID.ToString())
                                         select row.Field<DateTime>("PlannedDate")).Distinct();

                    //due
                    double EntityDuePrincipal = 0;
                    double EntityDueInterest = 0;

                    double EntityDueArrearPrincipal = 0;
                    double EntityDueArrearInterest = 0;

                    //collec
                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double EntityCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double EntityCollectedInterest = 0;
                    double CollectedInterest;

                    double EntityTotalCollected = 0;
                    double TotalCollected;

                    string EntityPlannedDate = null;

                    foreach (var PlannedDate in distinctDates)
                    {
                        EntityPlannedDate = PlannedDate.ToDateShortMonth(false, '-');

                        var Loans = (from row in dt.AsEnumerable()
                                     where row.Field<DateTime>("PlannedDate") == DateTime.Parse(PlannedDate.ToString())
                                     select row.Field<int>("LoanID")).Distinct();

                        foreach (var UniqueLoanID in Loans)
                        {
                            //current due:
                            double DueP;
                            double DueI;

                            double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out DueP);
                            double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "'AND PlannedDate = '" + PlannedDate + "'").ToString(), out DueI);

                            EntityDuePrincipal += DueP;
                            EntityDueInterest += DueI;

                            //overdue for this installment:                        
                            double ExpectedPrincipal;
                            double ExpectedInterest;

                            //get how much should have been paid so far (p & I)
                            double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + PlannedDate + "'").ToString(), out ExpectedInterest);
                            double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + PlannedDate + "'").ToString(), out ExpectedPrincipal);

                            //get how much has been paid until before current date (p & I)
                            double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1'  AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedOverduePrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedPrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedOverdueInterest);
                            double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedInterest);

                            EntityDueArrearPrincipal += ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                            EntityDueArrearInterest += ExpectedInterest = (CollectedOverdueInterest + CollectedInterest);

                            CollectedOverduePrincipal = 0;
                            CollectedPrincipal = 0;
                            CollectedOverdueInterest = 0;
                            CollectedInterest = 0;
                            double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedOverduePrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedPrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedOverdueInterest);
                            double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedInterest);
                            double.TryParse(dt.Compute("Sum(TotalCollected)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out TotalCollected);

                            EntityCollectedPrincipal += CollectedPrincipal;
                            EntityCollectedInterest += CollectedInterest;
                            EntityCollectedOverdueInterest += CollectedOverdueInterest;
                            EntityCollectedOverduePrincipal += CollectedOverduePrincipal;
                            EntityTotalCollected += TotalCollected;

                        }

                        sbrHTML.Append("<tr>");
                        sbrHTML.Append("<td>" + UniqueCenterID + "</td>");
                        sbrHTML.Append("<td>" + EntityPlannedDate + "</td>");
                        sbrHTML.Append("<td>" + EntityDuePrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityDueInterest + "</td>");
                        sbrHTML.Append("<td>" + EntityDueArrearPrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityDueArrearInterest + "</td>");
                        sbrHTML.Append("<td>" + (EntityDuePrincipal + EntityDueInterest + EntityDueArrearPrincipal + EntityDueArrearInterest).ToString() + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedPrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedInterest + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedOverduePrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedOverdueInterest + "</td>");
                        sbrHTML.Append("<td>" + (EntityTotalCollected - (EntityCollectedPrincipal + EntityCollectedInterest + EntityCollectedOverduePrincipal + EntityCollectedOverdueInterest)).ToString() + "</td>");
                        sbrHTML.Append("<td>" + EntityTotalCollected + "</td>");
                        sbrHTML.Append("</tr>");

                    }//unique dates

                    //html


                }//unique center

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string DueVsCollecFE(string BranchID, string FeID, string FromDate, string ToDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();
            Office objOff = new Office();

            if (BranchID == "0")
                BranchID = null;

            if (FeID == "0")
                FeID = null;

            DataSet ds = objRprt.DueVsCollectionGeneric(BranchID, FromDate, ToDate);

            if (!DataUtils.IsDataSetNull(ds, 0))
            {
                DataTable dt = ds.Tables[0];

                sbrHTML.Append("<table class='table table-bordered'>");

                sbrHTML.Append("<thead>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th rowspan='2'>Repayment Date</th>");
                sbrHTML.Append("<th colspan='5'>Due</th>");
                sbrHTML.Append("<th colspan='6'>Collections</th>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Arrears Principal</th>");
                sbrHTML.Append("<th>Arrears Interest</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("<th>Principal</th>");
                sbrHTML.Append("<th>Interest</th>");
                sbrHTML.Append("<th>Arrears Principal</th>");
                sbrHTML.Append("<th>Arrears Interest</th>");
                sbrHTML.Append("<th>Prepayment</th>");
                sbrHTML.Append("<th>Total</th>");

                sbrHTML.Append("</tr>");

                sbrHTML.Append("</thead>");

                sbrHTML.Append("<tbody>");

                var distinctFE = (from row in dt.AsEnumerable()
                                  select row.Field<int>("FeID")).Distinct();

                if (FeID != null)
                {
                    distinctFE = (from row in dt.AsEnumerable()
                                  where row.Field<int>("FeID") == int.Parse(FeID)
                                  select row.Field<int>("FeID")).Distinct();
                }

                foreach (var UniqueFeID in distinctFE)
                {

                    //filter unique dates
                    var distinctDates = (from row in dt.AsEnumerable()
                                         where row.Field<int>("FeID") == int.Parse(UniqueFeID.ToString())
                                         select row.Field<DateTime>("PlannedDate")).Distinct();

                    //due
                    double EntityDuePrincipal = 0;
                    double EntityDueInterest = 0;

                    double EntityDueArrearPrincipal = 0;
                    double EntityDueArrearInterest = 0;

                    //collec
                    double EntityCollectedOverduePrincipal = 0;
                    double CollectedOverduePrincipal;

                    double EntityCollectedPrincipal = 0;
                    double CollectedPrincipal;

                    double EntityCollectedOverdueInterest = 0;
                    double CollectedOverdueInterest;

                    double EntityCollectedInterest = 0;
                    double CollectedInterest;

                    double EntityTotalCollected = 0;
                    double TotalCollected;

                    string EntityPlannedDate = null;

                    foreach (var PlannedDate in distinctDates)
                    {
                        EntityPlannedDate = PlannedDate.ToDateShortMonth(false, '-');

                        var Loans = (from row in dt.AsEnumerable()
                                     where row.Field<DateTime>("PlannedDate") == DateTime.Parse(PlannedDate.ToString())
                                     select row.Field<int>("LoanID")).Distinct();

                        foreach (var UniqueLoanID in Loans)
                        {
                            //current due:
                            double DueP;
                            double DueI;

                            double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out DueP);
                            double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "'AND PlannedDate = '" + PlannedDate + "'").ToString(), out DueI);

                            EntityDuePrincipal += DueP;
                            EntityDueInterest += DueI;

                            //overdue for this installment:                        
                            double ExpectedPrincipal;
                            double ExpectedInterest;

                            //get how much should have been paid so far (p & I)
                            double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + PlannedDate + "'").ToString(), out ExpectedInterest);
                            double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + PlannedDate + "'").ToString(), out ExpectedPrincipal);

                            //get how much has been paid until before current date (p & I)
                            double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1'  AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedOverduePrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedPrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedOverdueInterest);
                            double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate < '" + PlannedDate + "'").ToString(), out CollectedInterest);

                            EntityDueArrearPrincipal += ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                            EntityDueArrearInterest += ExpectedInterest = (CollectedOverdueInterest + CollectedInterest);

                            CollectedOverduePrincipal = 0;
                            CollectedPrincipal = 0;
                            CollectedOverdueInterest = 0;
                            CollectedInterest = 0;
                            double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedOverduePrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedPrincipal);
                            double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedOverdueInterest);
                            double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out CollectedInterest);
                            double.TryParse(dt.Compute("Sum(TotalCollected)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate = '" + PlannedDate + "'").ToString(), out TotalCollected);

                            EntityCollectedPrincipal += CollectedPrincipal;
                            EntityCollectedInterest += CollectedInterest;
                            EntityCollectedOverdueInterest += CollectedOverdueInterest;
                            EntityCollectedOverduePrincipal += CollectedOverduePrincipal;
                            EntityTotalCollected += TotalCollected;

                        }

                        sbrHTML.Append("<tr>");
                        sbrHTML.Append("<td>" + EntityPlannedDate + "</td>");
                        sbrHTML.Append("<td>" + EntityDuePrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityDueInterest + "</td>");
                        sbrHTML.Append("<td>" + EntityDueArrearPrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityDueArrearInterest + "</td>");
                        sbrHTML.Append("<td>" + (EntityDuePrincipal + EntityDueInterest + EntityDueArrearPrincipal + EntityDueArrearInterest).ToString() + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedPrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedInterest + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedOverduePrincipal + "</td>");
                        sbrHTML.Append("<td>" + EntityCollectedOverdueInterest + "</td>");
                        sbrHTML.Append("<td>" + (EntityTotalCollected - (EntityCollectedPrincipal + EntityCollectedInterest + EntityCollectedOverduePrincipal + EntityCollectedOverdueInterest)).ToString() + "</td>");
                        sbrHTML.Append("<td>" + EntityTotalCollected + "</td>");
                        sbrHTML.Append("</tr>");

                    }//unique dates

                    //html


                }//unique center

                sbrHTML.Append("</tbody>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        //
        public string FeDetailed(string FeID, string FromDate, string ToDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            DataSet ds = objRprt.FeStatistics(FeID, FromDate, ToDate);
            DataSet dsGeneric = objRprt.OutstandingGeneric(null, ToDate);

            if (!DataUtils.IsDataSetNull(ds, 0) && !DataUtils.IsDataSetNull(dsGeneric, 0))
            {
                DataTable dt = ds.Tables[0];

                string FeName = dt.Select("Param = 'FeName'").Single()[1].ToString();
                string Gender = dt.Select("Param = 'Gender'").Single()[1].ToString();
                string OfficeName = dt.Select("Param = 'OfficeName'").Single()[1].ToString();
                string ClientsTillDate = dt.Select("Param = 'ClientsTillDate'").Single()[1].ToString();
                string ClientsPeriod = dt.Select("Param = 'ClientsPeriod'").Single()[1].ToString();
                string GroupsTillDate = dt.Select("Param = 'GroupsTillDate'").Single()[1].ToString();
                string GroupsPeriod = dt.Select("Param = 'GroupsPeriod'").Single()[1].ToString();
                string JoinedMFI = dt.Select("Param = 'JoinedMFI'").Single()[1].ToString();
                string LeftMFI = dt.Select("Param = 'LeftMFI'").Single()[1].ToString();
                string Centers = dt.Select("Param = 'Centers'").Single()[1].ToString();
                string ActiveLoans = dt.Select("Param = 'ActiveLoans'").Single()[1].ToString();

                try
                {
                    JoinedMFI = DateTime.Parse(JoinedMFI).ToDateShortMonth(false, '-');
                }
                catch (Exception ex)
                {
                }

                try
                {
                    LeftMFI = DateTime.Parse(LeftMFI).ToDateShortMonth(false, '-');
                }
                catch (Exception ex)
                {
                }

                //html - Group formation
                StringBuilder sbrGroup = new StringBuilder();

                sbrGroup.Append("<table class='table table-bordered'>");
                sbrGroup.Append("<tr>");
                sbrGroup.Append("<td colspan='3' class='rpt-hdr'>Group Formation</td>");
                sbrGroup.Append("</tr>");

                sbrGroup.Append("<tr>");
                sbrGroup.Append("<td></td>");
                sbrGroup.Append("<td>To Date</td>");
                sbrGroup.Append("<td>This Period</td>");
                sbrGroup.Append("</tr>");

                sbrGroup.Append("<tr>");
                sbrGroup.Append("<td>Clients Added</td>");
                sbrGroup.Append("<td>" + ClientsTillDate + "</td>");
                sbrGroup.Append("<td>" + ClientsPeriod + "</td>");
                sbrGroup.Append("</tr>");

                sbrGroup.Append("<tr>");
                sbrGroup.Append("<td>Groups Added</td>");
                sbrGroup.Append("<td>" + GroupsTillDate + "</td>");
                sbrGroup.Append("<td>" + GroupsPeriod + "</td>");
                sbrGroup.Append("</tr>");

                sbrGroup.Append("</table>");

                dt.Clear();
                dt = dsGeneric.Tables[0];

                var distinctLoans = (from row in dt.AsEnumerable()
                                     where row.Field<int>("FeID") == int.Parse(FeID) && row.Field<DateTime>("DateOfDisbursement") >= DateTime.Parse(FromDate) && row.Field<DateTime>("DateOfDisbursement") <= DateTime.Parse(ToDate)
                                     select row.Field<int>("LoanID")).Distinct();


                double AmountDisbursed;

                double EntityOutstandingPrincipal = 0;
                double EntityOutstandingInterest = 0;

                double ExpectedPrincipal = 0;
                double ExpectedInterest = 0;

                double CollectedOverduePrincipal;
                double CollectedPrincipal;
                double CollectedOverdueInterest;
                double CollectedInterest;

                double EntityOverdueOutstandingP = 0;

                //
                double Entity_OD_Total_1_30 = 0;
                double Entity_OD_POut_1_30 = 0;
                int Entity_OD_Loans_1_30 = 0;

                double Entity_OD_Total_31_60 = 0;
                double Entity_OD_POut_31_60 = 0;
                int Entity_OD_Loans_31_60 = 0;

                double Entity_OD_Total_61_90 = 0;
                double Entity_OD_POut_61_90 = 0;
                int Entity_OD_Loans_61_90 = 0;

                double Entity_OD_Total_GT_90 = 0;
                double Entity_OD_POut_GT_90 = 0;
                int Entity_OD_Loans_GT_90 = 0;
                //

                foreach (var UniqueLoanID in distinctLoans)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                           select myRow).First();

                    double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);

                    double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverduePrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedPrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverdueInterest);
                    double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedInterest);

                    double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedPrincipal);
                    double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND  IsCompleted = '1' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedInterest);

                    EntityOutstandingPrincipal += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                    EntityOutstandingInterest += ExpectedInterest - (CollectedOverdueInterest + CollectedInterest);

                    double OverduePrincipal = ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                    double OverdueInterest = ExpectedInterest - (CollectedOverdueInterest + CollectedInterest);

                    int ODDays = 0;


                    if ((OverduePrincipal + OverdueInterest) > 0)
                    {
                        var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + ToDate + "'");

                        try
                        {
                            DateTime.Parse(LastFullPaidDate.ToString());
                        }
                        catch (Exception ex)
                        {
                            //if no full payment till now, get the first planned rep date.
                            LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                        }

                        ODDays = (DateTime.Parse(ToDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                        if (ODDays > 0)
                        {
                            EntityOverdueOutstandingP += OverduePrincipal;// AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);

                            if (ODDays <= 30)
                            {
                                Entity_OD_Total_1_30 += OverduePrincipal + OverdueInterest;
                                Entity_OD_POut_1_30 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_1_30++;
                            }
                            else if (ODDays > 30 && ODDays <= 60)
                            {
                                Entity_OD_Total_31_60 += OverduePrincipal + OverdueInterest;
                                Entity_OD_POut_31_60 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_31_60++;
                            }
                            else if (ODDays > 60 && ODDays <= 90)
                            {
                                Entity_OD_Total_61_90 += OverduePrincipal + OverdueInterest;
                                Entity_OD_POut_61_90 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_61_90++;
                            }
                            else
                            {
                                Entity_OD_Total_GT_90 += OverduePrincipal + OverdueInterest;
                                Entity_OD_POut_GT_90 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_GT_90++;
                            }

                        }

                    }
                } //loans for fe

                double PAR = 0;

                if (EntityOverdueOutstandingP > 0 && EntityOutstandingPrincipal > 0)
                {
                    try
                    {
                        PAR = Math.Round(EntityOverdueOutstandingP / EntityOutstandingPrincipal, 2, MidpointRounding.AwayFromZero);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //html - account summary
                StringBuilder sbrAccountSummary = new StringBuilder();

                sbrAccountSummary.Append("<table class='table table-bordered'>");
                sbrAccountSummary.Append("<tr>");
                sbrAccountSummary.Append("<td colspan='2' class='rpt-hdr'>Account Summary</td>");
                sbrAccountSummary.Append("</tr>");

                sbrAccountSummary.Append("<tr>");
                sbrAccountSummary.Append("<td>Active Loans</td>");
                sbrAccountSummary.Append("<td>" + distinctLoans.Count().ToString() + "</td>");
                sbrAccountSummary.Append("</tr>");
                sbrAccountSummary.Append("<tr>");
                sbrAccountSummary.Append("<td>Principal Amount Outstanding</td>");
                sbrAccountSummary.Append("<td>" + EntityOutstandingPrincipal + "</td>");
                sbrAccountSummary.Append("</tr>");
                sbrAccountSummary.Append("<tr>");
                sbrAccountSummary.Append("<td>Interest Amount Outstanding</td>");
                sbrAccountSummary.Append("<td>" + EntityOutstandingInterest + "</td>");
                sbrAccountSummary.Append("</tr>");
                sbrAccountSummary.Append("<tr>");
                sbrAccountSummary.Append("<td>Portfolio at Risk %</td>");
                sbrAccountSummary.Append("<td>" + PAR.ToString() + "</td>");
                sbrAccountSummary.Append("</tr>");
                sbrAccountSummary.Append("</table>");

                StringBuilder sbrClientSummary = new StringBuilder();

                sbrClientSummary.Append("<table class='table table-bordered'>");
                sbrClientSummary.Append("<tr>");
                sbrClientSummary.Append("<td colspan='2' class='rpt-hdr'>Client Summary</td>");
                sbrClientSummary.Append("</tr>");

                sbrClientSummary.Append("<tr>");
                sbrClientSummary.Append("<td>Groups</td>");
                sbrClientSummary.Append("<td>" + GroupsPeriod + "</td>");
                sbrClientSummary.Append("</tr>");

                sbrClientSummary.Append("<tr>");
                sbrClientSummary.Append("<td>No of Clients (This Period)</td>");
                sbrClientSummary.Append("<td>" + ClientsPeriod + "</td>");
                sbrClientSummary.Append("</tr>");

                sbrClientSummary.Append("<tr>");
                sbrClientSummary.Append("<td>Clients with loans</td>");
                sbrClientSummary.Append("<td>" + ActiveLoans + "</td>");
                sbrClientSummary.Append("</tr>");

                int DormantClients = 0;
                try
                {
                    DormantClients = int.Parse(ClientsTillDate) - int.Parse(ActiveLoans);
                }
                catch (Exception ex)
                {
                }

                sbrClientSummary.Append("<tr>");
                sbrClientSummary.Append("<td>Dormant Clients</td>");
                sbrClientSummary.Append("<td>" + DormantClients + "</td>");
                sbrClientSummary.Append("</tr>");

                sbrClientSummary.Append("</table>");

                //html - day wise analysis
                StringBuilder sbrDayWise = new StringBuilder();

                sbrDayWise.Append("<table class='table table-bordered'>");
                sbrDayWise.Append("<tr>");
                sbrDayWise.Append("<td colspan='5' class='rpt-hdr'>Ageing in Arrears by Days</td>");
                sbrDayWise.Append("</tr>");

                sbrDayWise.Append("<tr>");
                sbrDayWise.Append("<td></td>");
                sbrDayWise.Append("<td>No of Clients</td>");
                sbrDayWise.Append("<td>No of Loans</td>");
                sbrDayWise.Append("<td>Arrears Amnt.</td>");
                sbrDayWise.Append("<td>Principal Outstanding</td>");
                sbrDayWise.Append("</tr>");

                sbrDayWise.Append("<tr>");
                sbrDayWise.Append("<td>0 to 30 Days in Arrears</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_1_30 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_1_30 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Total_1_30 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_POut_1_30 + "</td>");
                sbrDayWise.Append("</tr>");

                sbrDayWise.Append("<tr>");
                sbrDayWise.Append("<td>31 to 60 Days in Arrears</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_31_60 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_31_60 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Total_31_60 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_POut_31_60 + "</td>");
                sbrDayWise.Append("</tr>");

                sbrDayWise.Append("<tr>");
                sbrDayWise.Append("<td>61 to 90 Days in Arrears</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_61_90 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_61_90 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Total_61_90 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_POut_61_90 + "</td>");
                sbrDayWise.Append("</tr>");

                sbrDayWise.Append("<tr>");
                sbrDayWise.Append("<td>Above 90 Days in Arrears</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_GT_90 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Loans_GT_90 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_Total_GT_90 + "</td>");
                sbrDayWise.Append("<td>" + Entity_OD_POut_GT_90 + "</td>");
                sbrDayWise.Append("</tr>");

                sbrDayWise.Append("<tr style='font-weight:bold'>");
                sbrDayWise.Append("<td>Total</td>");
                sbrDayWise.Append("<td>" + (Entity_OD_Loans_1_30 + Entity_OD_Loans_31_60 + Entity_OD_Loans_61_90 + Entity_OD_Loans_GT_90).ToString() + "</td>");
                sbrDayWise.Append("<td>" + (Entity_OD_Loans_1_30 + Entity_OD_Loans_31_60 + Entity_OD_Loans_61_90 + Entity_OD_Loans_GT_90).ToString() + "</td>");
                sbrDayWise.Append("<td>" + (Entity_OD_Total_1_30 + Entity_OD_Total_31_60 + Entity_OD_Total_61_90 + Entity_OD_Total_GT_90).ToString() + "</td>");
                sbrDayWise.Append("<td>" + (Entity_OD_POut_1_30 + Entity_OD_POut_31_60 + Entity_OD_POut_61_90 + Entity_OD_POut_GT_90).ToString() + "</td>");
                sbrDayWise.Append("</tr>");

                sbrDayWise.Append("</table>");



                var distinctCenters = (from row in dt.AsEnumerable()
                                       where row.Field<int>("FeID") == int.Parse(FeID) && row.Field<DateTime>("DateOfDisbursement") >= DateTime.Parse(FromDate) && row.Field<DateTime>("DateOfDisbursement") <= DateTime.Parse(ToDate)
                                       select row.Field<int>("CenterID")).Distinct();

                StringBuilder sbrCenter = new StringBuilder();
                sbrCenter.Append("<table class='table table-bordered'>");
                sbrCenter.Append("<tr>");
                sbrCenter.Append("<td colspan='5' class='rpt-hdr'>Summary of Centers Managed</td>");
                sbrCenter.Append("</tr>");

                sbrCenter.Append("<tr>");
                sbrCenter.Append("<td>Center</td>");
                sbrCenter.Append("<td>Clients</td>");
                sbrCenter.Append("<td>Principal Outstanding</td>");
                sbrCenter.Append("<td>Arrears Amount</td>");
                sbrCenter.Append("<td>PAR</td>");
                sbrCenter.Append("</tr>");

                int CentTotalClients = 0;
                double CentTotalPOut = 0;
                double CentTotalOut = 0;

                foreach (var UniqueCenterID in distinctCenters)
                {
                    double CenterOutP = 0;
                    double CenterOverdueP = 0;
                    double CenterOverdueI = 0;
                    double CenterPAR = 0;
                    double CenterOverdueOutstandingP = 0;

                    var CenterLoans = (from row in dt.AsEnumerable()
                                       where row.Field<int>("FeID") == int.Parse(FeID) && row.Field<int>("CenterID") == int.Parse(UniqueCenterID.ToString()) && row.Field<DateTime>("DateOfDisbursement") >= DateTime.Parse(FromDate) && row.Field<DateTime>("DateOfDisbursement") <= DateTime.Parse(ToDate)
                                       select row.Field<int>("LoanID")).Distinct();

                    var Clients = (from row in dt.AsEnumerable()
                                   where row.Field<int>("FeID") == int.Parse(FeID) && row.Field<int>("CenterID") == int.Parse(UniqueCenterID.ToString())
                                   select row.Field<int>("ClientID")).Distinct();

                    foreach (var UniqueLoanID in CenterLoans)
                    {
                        var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                               where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                               select myRow).First();

                        double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);

                        double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverduePrincipal);
                        double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedPrincipal);
                        double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverdueInterest);
                        double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedInterest);

                        double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedPrincipal);
                        double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedInterest);

                        CenterOutP += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);

                        double OverduePrincipal = ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                        double OverdueInterest = ExpectedInterest - (CollectedOverdueInterest + CollectedInterest);

                        CenterOverdueP += OverduePrincipal;
                        CenterOverdueI += OverdueInterest;

                        int ODDays = 0;

                        if ((OverduePrincipal + OverdueInterest) > 0)
                        {
                            var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate <= '" + ToDate + "'");

                            try
                            {
                                DateTime.Parse(LastFullPaidDate.ToString());
                            }
                            catch (Exception ex)
                            {
                                //if no full payment till now, get the first planned rep date.
                                LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                            }

                            ODDays = (DateTime.Parse(ToDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                            if (ODDays > 0)
                            {
                                CenterOverdueOutstandingP += CenterOverdueP;// AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                            }

                        }
                    } //unique loans for center

                    if (CenterOverdueOutstandingP > 0 && CenterOutP > 0)
                    {
                        try
                        {
                            CenterPAR = Math.Round(CenterOverdueOutstandingP / CenterOutP, 2, MidpointRounding.AwayFromZero);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    sbrCenter.Append("<tr>");
                    sbrCenter.Append("<td>" + UniqueCenterID + "</td>");
                    sbrCenter.Append("<td>" + Clients.Count().ToString() + "</td>");
                    sbrCenter.Append("<td>" + CenterOutP + "</td>");
                    sbrCenter.Append("<td>" + (CenterOverdueP + CenterOverdueI).ToString() + "</td>");
                    sbrCenter.Append("<td>" + CenterPAR + "</td>");
                    sbrCenter.Append("</tr>");

                    CentTotalClients += Clients.Count();
                    CentTotalPOut += CenterOutP;
                    CentTotalOut += CenterOverdueP + CenterOverdueI;
                }

                sbrCenter.Append("<tr style='font-weight:bold'>");
                sbrCenter.Append("<td>Total</td>");
                sbrCenter.Append("<td>" + CentTotalClients + "</td>");
                sbrCenter.Append("<td>" + CentTotalPOut + "</td>");
                sbrCenter.Append("<td>" + CentTotalOut + "</td>");
                sbrCenter.Append("<td>-</td>");
                sbrCenter.Append("</tr>");

                sbrCenter.Append("</table>");


                //final html

                sbrHTML.Append("<div style='float: left; width: 860px;'>");

                sbrHTML.Append("<ul class='unstyled' style='float:left; margin-right:100px;' >");
                sbrHTML.Append("<li>");
                sbrHTML.Append("<table class='table table-bordered'>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>Name of FE</td>");
                sbrHTML.Append("<td>" + FeName + "</td>");
                sbrHTML.Append("</tr>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>Office</td>");
                sbrHTML.Append("<td>" + OfficeName + "</td>");
                sbrHTML.Append("</tr>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>Gender</td>");
                sbrHTML.Append("<td>" + Gender + "</td>");
                sbrHTML.Append("</tr>");
                sbrHTML.Append("</table>");
                sbrHTML.Append("</li>");
                sbrHTML.Append("</ul>");

                sbrHTML.Append("<ul class='unstyled' style='float:left'>");
                sbrHTML.Append("<li>");
                sbrHTML.Append("<table class='table table-bordered'>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td colspan='2' class='rpt-hdr'>Key Dates</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>Date of Joining</td>");
                sbrHTML.Append("<td>" + JoinedMFI + "</td>");
                sbrHTML.Append("</tr>");
                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>Date of Leaving</td>");
                sbrHTML.Append("<td>" + LeftMFI + "</td>");
                sbrHTML.Append("</tr>");
                sbrHTML.Append("</table>");

                sbrHTML.Append("</li>");
                sbrHTML.Append("</ul>");

                sbrHTML.Append("</div>");

                //
                sbrHTML.Append("<div style='float: left; width: 860px;'>");
                sbrHTML.Append("<ul class='unstyled' style='float:left; margin-right:100px;' >");
                sbrHTML.Append("<li>");
                sbrHTML.Append(sbrGroup.ToString());
                sbrHTML.Append("</li>");
                sbrHTML.Append("</ul>");

                sbrHTML.Append("<ul class='unstyled' style='float:left; margin-right:100px;' >");
                sbrHTML.Append("<li>");
                sbrHTML.Append(sbrClientSummary.ToString());
                sbrHTML.Append("</li>");
                sbrHTML.Append("</ul>");

                sbrHTML.Append("<ul class='unstyled' style='float:left' >");
                sbrHTML.Append("<li>");
                sbrHTML.Append(sbrAccountSummary.ToString());
                sbrHTML.Append("</li>");
                sbrHTML.Append("</ul>");

                sbrHTML.Append("</div>");

                //

                sbrHTML.Append("<div style='float:left'>");
                sbrHTML.Append(sbrDayWise.ToString());
                sbrHTML.Append("</div>");

                sbrHTML.Append("<div style='float:left'>");
                sbrHTML.Append(sbrCenter.ToString());
                sbrHTML.Append("</div>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }

        public string MasterBranchReport(string BranchID, string FromDate, string ToDate)
        {
            StringBuilder sbrHTML = new StringBuilder();

            Reports objRprt = new Reports();

            DataSet ds;
            if(BranchID == "0")
                ds = objRprt.MasterBranchReport(null, FromDate, ToDate);
            else
                ds = objRprt.MasterBranchReport(BranchID, FromDate, ToDate);
            DataSet dsGeneric = objRprt.OutstandingGeneric(null, ToDate);

            if (!DataUtils.IsDataSetNull(ds, 0) && !DataUtils.IsDataSetNull(dsGeneric, 0))
            {
                DataTable dt = ds.Tables[0];

                //VALES for start of the month
                string TotalVillages1 = dt.Select("Param = 'TotalVillages'").Single()[1].ToString();
                string TotalCenters1 = dt.Select("Param = 'TotalCenters'").Single()[1].ToString();
                string TotalGroups1 = dt.Select("Param = 'TotalGroups'").Single()[1].ToString();
                string TotalClients1 = dt.Select("Param = 'TotalClients'").Single()[1].ToString();
                string TotalActiveClients1 = dt.Select("Param = 'TotalActiveClients'").Single()[1].ToString();
                string ClientsCycle11 = dt.Select("Param = 'ClientsCycle1'").Single()[1].ToString();
                string ClientsCycle21 = dt.Select("Param = 'ClientsCycle2'").Single()[1].ToString();
                string ClientsCycle31 = dt.Select("Param = 'ClientsCycle3'").Single()[1].ToString();
                string Loans1 = dt.Select("Param = 'Loans'").Single()[1].ToString();
                string AmountDisbursed1 = dt.Select("Param = 'AmountDisbursed'").Single()[1].ToString();
                string DuePrincipal1 = dt.Select("Param = 'DuePrincipal'").Single()[1].ToString();
                string Repaid1 = dt.Select("Param = 'Repaid'").Single()[1].ToString();
                string Prepayment1 = dt.Select("Param = 'Prepayment'").Single()[1].ToString();
                string OutstandingP1 = dt.Select("Param = 'OutstandingP'").Single()[1].ToString();

                //VALES for end of the month
                string TotalVillages2 = dt.Select("Param = 'TotalVillages'").Single()[2].ToString();
                string TotalCenters2 = dt.Select("Param = 'TotalCenters'").Single()[2].ToString();
                string TotalGroups2 = dt.Select("Param = 'TotalGroups'").Single()[2].ToString();
                string TotalClients2 = dt.Select("Param = 'TotalClients'").Single()[2].ToString();
                string TotalActiveClients2 = dt.Select("Param = 'TotalActiveClients'").Single()[2].ToString();
                string ClientsCycle12 = dt.Select("Param = 'ClientsCycle1'").Single()[2].ToString();
                string ClientsCycle22 = dt.Select("Param = 'ClientsCycle2'").Single()[2].ToString();
                string ClientsCycle32 = dt.Select("Param = 'ClientsCycle3'").Single()[2].ToString();
                string Loans2 = dt.Select("Param = 'Loans'").Single()[2].ToString();
                string AmountDisbursed2 = dt.Select("Param = 'AmountDisbursed'").Single()[2].ToString();
                string DuePrincipal2 = dt.Select("Param = 'DuePrincipal'").Single()[2].ToString();
                string Repaid2 = dt.Select("Param = 'Repaid'").Single()[2].ToString();
                string Prepayment2 = dt.Select("Param = 'Prepayment'").Single()[2].ToString();
                string OutstandingP2 = dt.Select("Param = 'OutstandingP'").Single()[2].ToString();

                #region out reach and loan 

                sbrHTML.Append("<table class='table table-bordered'>");
                sbrHTML.Append("<tr class='rpt-hdr'>");
                sbrHTML.Append("<td>S.No.</td>");
                sbrHTML.Append("<td>Particulars</td>");
                sbrHTML.Append("<td>At the Begining of Month</td>");
                sbrHTML.Append("<td> +/-During of Month</td>");
                sbrHTML.Append("<td>At the End of Month</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr class='rpt-hdr'>");
                sbrHTML.Append("<td colspan='5' style='text-align:center'>Out Reach</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>1</td>");
                sbrHTML.Append("<td>No of Villages Covered</td>");
                sbrHTML.Append("<td>" + TotalVillages1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(TotalVillages2) - int.Parse(TotalVillages1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + TotalVillages2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>2</td>");
                sbrHTML.Append("<td>No. of Centers</td>");
                sbrHTML.Append("<td>" + TotalCenters1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(TotalCenters2) - int.Parse(TotalCenters1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + TotalCenters2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>3</td>");
                sbrHTML.Append("<td>No Groups </td>");
                sbrHTML.Append("<td>" + TotalGroups1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(TotalGroups2) - int.Parse(TotalGroups1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + TotalGroups2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>4</td>");
                sbrHTML.Append("<td>Total Number of Members </td>");
                sbrHTML.Append("<td>" + TotalClients1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(TotalClients2) - int.Parse(TotalClients1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + TotalClients2 + "</td>");
                sbrHTML.Append("</tr>");


                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>5</td>");
                sbrHTML.Append("<td>Total Number of Active Borrowers </td>");
                sbrHTML.Append("<td>" + TotalActiveClients1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(TotalActiveClients2) - int.Parse(TotalActiveClients1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + TotalActiveClients2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>6</td>");
                sbrHTML.Append("<td>Borrowers in First Loan Cycle </td>");
                sbrHTML.Append("<td>" + ClientsCycle11 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(ClientsCycle12) - int.Parse(ClientsCycle11)).ToString() + "</td>");
                sbrHTML.Append("<td>" + ClientsCycle12 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>7</td>");
                sbrHTML.Append("<td>Borrowers in Second Loan Cycle</td>");
                sbrHTML.Append("<td>" + ClientsCycle21 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(ClientsCycle22) - int.Parse(ClientsCycle21)).ToString() + "</td>");
                sbrHTML.Append("<td>" + ClientsCycle22 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>8</td>");
                sbrHTML.Append("<td>Borrowers in Third Loan Cycle</td>");
                sbrHTML.Append("<td>" + ClientsCycle31 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(ClientsCycle32) - int.Parse(ClientsCycle31)).ToString() + "</td>");
                sbrHTML.Append("<td>" + ClientsCycle32 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr class='rpt-hdr'>");
                sbrHTML.Append("<td colspan='5' style='text-align:center'>Loan Portfolio </td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>9</td>");
                sbrHTML.Append("<td>No of Loan Disbursed</td>");
                sbrHTML.Append("<td>" + Loans1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(Loans2) - int.Parse(Loans1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + Loans2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>10</td>");
                sbrHTML.Append("<td>Amount of Loan Disbursed</td>");
                sbrHTML.Append("<td>" + AmountDisbursed1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(AmountDisbursed2) - int.Parse(AmountDisbursed1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + AmountDisbursed2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>11</td>");
                sbrHTML.Append("<td>Due Amount  ( Pricipal )</td>");
                sbrHTML.Append("<td>" + DuePrincipal1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(DuePrincipal2) - int.Parse(DuePrincipal1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + DuePrincipal2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>12</td>");
                sbrHTML.Append("<td>Repayment Collection</td>");
                sbrHTML.Append("<td>" + Repaid1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(Repaid2) - int.Parse(Repaid1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + Repaid2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>13</td>");
                sbrHTML.Append("<td>Prepayment</td>");
                sbrHTML.Append("<td>" + Prepayment1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(Prepayment2) - int.Parse(Prepayment1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + Prepayment2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>14</td>");
                sbrHTML.Append("<td>Loan Oustanding</td>");
                sbrHTML.Append("<td>" + OutstandingP1 + "</td>");
                sbrHTML.Append("<td>" + (int.Parse(OutstandingP2) - int.Parse(OutstandingP1)).ToString() + "</td>");
                sbrHTML.Append("<td>" + OutstandingP2 + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr class='rpt-hdr'>");
                sbrHTML.Append("<td colspan='5' style='text-align:center'>Portfolio Quality</td>");
                sbrHTML.Append("</tr>");

                double RepaymentRate1 = Math.Round((double.Parse(Repaid1) - double.Parse(Prepayment1)) / double.Parse(DuePrincipal1), 2, MidpointRounding.AwayFromZero);
                double RepaymentRate2 = Math.Round((double.Parse(Repaid2) - double.Parse(Prepayment2)) / double.Parse(DuePrincipal2), 2, MidpointRounding.AwayFromZero);

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>15</td>");
                sbrHTML.Append("<td>Repayment Rate [( 12-13) / 11]</td>");
                sbrHTML.Append("<td>" + RepaymentRate1 + "</td>");
                sbrHTML.Append("<td>" + (RepaymentRate2 - RepaymentRate1).ToString() + "</td>");
                sbrHTML.Append("<td>" + RepaymentRate2 + "</td>");
                sbrHTML.Append("</tr>");

                #endregion

                #region ageing calculation

                dt.Clear();
                dt = dsGeneric.Tables[0];

                var distinctLoans = (from row in dt.AsEnumerable()
                                     where row.Field<int>("BranchID") == int.Parse(BranchID) && row.Field<DateTime>("PlannedDate") >= DateTime.Parse(FromDate) && row.Field<DateTime>("PlannedDate") <= DateTime.Parse(ToDate)
                                     select row.Field<int>("LoanID")).Distinct();

                if (BranchID == "0")
                {
                    distinctLoans = (from row in dt.AsEnumerable()
                                     where row.Field<DateTime>("PlannedDate") >= DateTime.Parse(FromDate) && row.Field<DateTime>("PlannedDate") <= DateTime.Parse(ToDate)
                                     select row.Field<int>("LoanID")).Distinct();
                }


                double AmountDisbursed;

                double EntityOutstandingPrincipal = 0;
                double EntityOutstandingInterest = 0;

                double ExpectedPrincipal = 0;
                double ExpectedInterest = 0;

                double CollectedOverduePrincipal;
                double CollectedPrincipal;
                double CollectedOverdueInterest;
                double CollectedInterest;

                double EntityOverdueOutstandingP = 0;

                //
                int Entity_OnTime_Loans = 0;
                double Entity_OnTime_OutP = 0;

                double Entity_OD_Total_1_30 = 0;
                double Entity_OD_POut_1_30 = 0;
                int Entity_OD_Loans_1_30 = 0;

                double Entity_OD_Total_31_60 = 0;
                double Entity_OD_POut_31_60 = 0;
                int Entity_OD_Loans_31_60 = 0;

                double Entity_OD_Total_61_90 = 0;
                double Entity_OD_POut_61_90 = 0;
                int Entity_OD_Loans_61_90 = 0;

                double Entity_OD_Total_GT_90 = 0;
                double Entity_OD_POut_GT_90 = 0;
                int Entity_OD_Loans_GT_90 = 0;
                //



                foreach (var UniqueLoanID in distinctLoans)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                           select myRow).First();

                    double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);

                    double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate >= '" + FromDate + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverduePrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate >= '" + FromDate + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedPrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate >= '" + FromDate + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverdueInterest);
                    double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate >= '" + FromDate + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedInterest);

                    double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate >= '" + FromDate + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedPrincipal);
                    double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate >= '" + FromDate + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedInterest);

                    EntityOutstandingPrincipal += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                    EntityOutstandingInterest += ExpectedInterest - (CollectedOverdueInterest + CollectedInterest);

                    double OverduePrincipal = ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                    double OverdueInterest = ExpectedInterest - (CollectedOverdueInterest + CollectedInterest);

                    int ODDays = 0;


                    if ((OverduePrincipal + OverdueInterest) > 0)
                    {
                        var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2'  AND PlannedDate >= '" + FromDate + "' AND PlannedDate <= '" + ToDate + "'");

                        try
                        {
                            DateTime.Parse(LastFullPaidDate.ToString());
                        }
                        catch (Exception ex)
                        {
                            //if no full payment till now, get the first planned rep date.
                            LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                        }

                        ODDays = (DateTime.Parse(ToDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                        if (ODDays > 0)
                        {
                            EntityOverdueOutstandingP += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);

                            if (ODDays <= 30)
                            {
                                Entity_OD_Total_1_30 += OverduePrincipal;
                                Entity_OD_POut_1_30 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_1_30++;
                            }
                            else if (ODDays > 30 && ODDays <= 60)
                            {
                                Entity_OD_Total_31_60 += OverduePrincipal;
                                Entity_OD_POut_31_60 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_31_60++;
                            }
                            else if (ODDays > 60 && ODDays <= 90)
                            {
                                Entity_OD_Total_61_90 += OverduePrincipal;
                                Entity_OD_POut_61_90 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_61_90++;
                            }
                            else
                            {
                                Entity_OD_Total_GT_90 += OverduePrincipal;
                                Entity_OD_POut_GT_90 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                                Entity_OD_Loans_GT_90++;
                            }

                        }

                    }
                    else
                    {
                        //no overdue.
                        Entity_OnTime_Loans++;
                        Entity_OnTime_OutP += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                    }
                } //unique loans


                StringBuilder sbrAgeing = new StringBuilder();
                sbrAgeing.Append("<tr class='rpt-hdr'>");
                sbrAgeing.Append("<td>18</td>");
                sbrAgeing.Append("<td>Portfolio Ageing</td>");
                sbrAgeing.Append("<td>No of Loans</td>");
                sbrAgeing.Append("<td>Overdue Principal</td>");
                sbrAgeing.Append("<td>Outstanding Principal</td>");
                sbrAgeing.Append("</tr>");

                sbrAgeing.Append("<tr>");
                sbrAgeing.Append("<td></td>");
                sbrAgeing.Append("<td>On time repayment</td>");
                sbrAgeing.Append("<td>" + Entity_OnTime_Loans + "</td>");
                sbrAgeing.Append("<td>0</td>");
                sbrAgeing.Append("<td>" + Entity_OnTime_OutP + "</td>");
                sbrAgeing.Append("</tr>");

                sbrAgeing.Append("<tr>");
                sbrAgeing.Append("<td></td>");
                sbrAgeing.Append("<td>1-30 days past dues</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Loans_1_30 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Total_1_30 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_POut_1_30 + "</td>");
                sbrAgeing.Append("</tr>");

                sbrAgeing.Append("<tr>");
                sbrAgeing.Append("<td></td>");
                sbrAgeing.Append("<td>31-60 days past due</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Loans_31_60 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Total_31_60 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_POut_31_60 + "</td>");
                sbrAgeing.Append("</tr>");

                sbrAgeing.Append("<tr>");
                sbrAgeing.Append("<td></td>");
                sbrAgeing.Append("<td>61-90 days past due</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Loans_61_90 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Total_61_90 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_POut_61_90 + "</td>");
                sbrAgeing.Append("</tr>");

                sbrAgeing.Append("<tr>");
                sbrAgeing.Append("<td></td>");
                sbrAgeing.Append("<td>Above 90 days</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Loans_GT_90 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_Total_GT_90 + "</td>");
                sbrAgeing.Append("<td>" + Entity_OD_POut_GT_90 + "</td>");
                sbrAgeing.Append("</tr>");

                #endregion ageing

                #region PAR calculation

                //FROM COLUMNS:
                double PAR_LT_30_From = 0;
                double PAR_GT_30_From = 0;
                double PAR_LT_30_To = 0;
                double PAR_GT_30_To = 0;

                double EntityOverdueOutstandingP_LT_30 = 0;
                double EntityOutstandingPrincipal_LT_30 = 0;
                double EntityOverdueOutstandingP_GT_30 = 0;
                double EntityOutstandingPrincipal_GT_30 = 0;

                distinctLoans = (from row in dt.AsEnumerable()
                                 where row.Field<int>("BranchID") == int.Parse(BranchID) && row.Field<DateTime>("PlannedDate") <= DateTime.Parse(FromDate)
                                 select row.Field<int>("LoanID")).Distinct();

                if (BranchID == "0")
                {
                    distinctLoans = (from row in dt.AsEnumerable()
                                     where row.Field<DateTime>("PlannedDate") <= DateTime.Parse(FromDate)
                                     select row.Field<int>("LoanID")).Distinct();
                }

                AmountDisbursed = 0;

                ExpectedPrincipal = 0;
                ExpectedInterest = 0;

                CollectedOverduePrincipal = 0;
                CollectedPrincipal = 0;
                CollectedOverdueInterest = 0;
                CollectedInterest = 0;
                //                               

                foreach (var UniqueLoanID in distinctLoans)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                           select myRow).First();

                    double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);

                    double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + FromDate + "'").ToString(), out CollectedOverduePrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + FromDate + "'").ToString(), out CollectedPrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + FromDate + "'").ToString(), out CollectedOverdueInterest);
                    double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + FromDate + "'").ToString(), out CollectedInterest);

                    double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + FromDate + "'").ToString(), out ExpectedPrincipal);
                    double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + FromDate + "'").ToString(), out ExpectedInterest);
                    
                    double OverduePrincipal = ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                    double OverdueInterest = ExpectedInterest - (CollectedOverdueInterest + CollectedInterest);

                    int ODDays = 0;


                    if ((OverduePrincipal + OverdueInterest) > 0)
                    {
                        var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2' AND PlannedDate <= '" + FromDate + "'");

                        try
                        {
                            DateTime.Parse(LastFullPaidDate.ToString());
                        }
                        catch (Exception ex)
                        {
                            //if no full payment till now, get the first planned rep date.
                            LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                        }

                        ODDays = (DateTime.Parse(ToDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                        if (ODDays > 0 && ODDays <= 30)
                        {
                            EntityOverdueOutstandingP_LT_30 += OverduePrincipal;
                            EntityOutstandingPrincipal_LT_30 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                        }
                        else if (ODDays > 30)
                        {
                            EntityOverdueOutstandingP_GT_30 += OverduePrincipal;
                            EntityOutstandingPrincipal_GT_30 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                        }

                    }
                } //unique loans

                if (EntityOverdueOutstandingP_LT_30 > 0 && EntityOutstandingPrincipal_LT_30 > 0)
                {
                    try
                    {
                        PAR_LT_30_From = Math.Round(EntityOverdueOutstandingP_LT_30 / EntityOutstandingPrincipal_LT_30, 2, MidpointRounding.AwayFromZero);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (EntityOverdueOutstandingP_GT_30 > 0 && EntityOutstandingPrincipal_GT_30 > 0)
                {
                    try
                    {
                        PAR_GT_30_From = Math.Round(EntityOverdueOutstandingP_GT_30 / EntityOutstandingPrincipal_GT_30, 2, MidpointRounding.AwayFromZero);
                    }
                    catch (Exception ex)
                    {
                    }
                }


                //TO COLUMNS
                distinctLoans = (from row in dt.AsEnumerable()
                                 where row.Field<int>("BranchID") == int.Parse(BranchID) && row.Field<DateTime>("PlannedDate") <= DateTime.Parse(ToDate)
                                 select row.Field<int>("LoanID")).Distinct();

                if (BranchID == "0")
                {
                    distinctLoans = (from row in dt.AsEnumerable()
                                     where row.Field<DateTime>("PlannedDate") <= DateTime.Parse(ToDate)
                                     select row.Field<int>("LoanID")).Distinct();
                }

                AmountDisbursed = 0;

                EntityOverdueOutstandingP_LT_30 = 0;
                EntityOutstandingPrincipal_LT_30 = 0;
                EntityOverdueOutstandingP_GT_30 = 0;
                EntityOutstandingPrincipal_GT_30 = 0;

                ExpectedPrincipal = 0;
                ExpectedInterest = 0;

                CollectedOverduePrincipal = 0;
                CollectedPrincipal = 0;
                CollectedOverdueInterest = 0;
                CollectedInterest = 0;
                //                               

                foreach (var UniqueLoanID in distinctLoans)
                {
                    var ReferenceRecord = (from myRow in dt.AsEnumerable()
                                           where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString())
                                           select myRow).First();

                    double.TryParse(ReferenceRecord["AmountDisbursed"].ToString(), out AmountDisbursed);

                    double.TryParse(dt.Compute("Sum(CollectedOverduePrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverduePrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedPrincipal);
                    double.TryParse(dt.Compute("Sum(CollectedOverdueInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedOverdueInterest);
                    double.TryParse(dt.Compute("Sum(CollectedInterest)", "LoanID = '" + UniqueLoanID + "' AND PlannedDate <= '" + ToDate + "'").ToString(), out CollectedInterest);

                    double.TryParse(dt.Compute("Sum(ExpectedPrincipal)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedPrincipal);
                    double.TryParse(dt.Compute("Sum(ExpectedInterest)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND PlannedDate <= '" + ToDate + "'").ToString(), out ExpectedInterest);

                    double OverduePrincipal = ExpectedPrincipal - (CollectedOverduePrincipal + CollectedPrincipal);
                    double OverdueInterest = ExpectedInterest - (CollectedOverdueInterest + CollectedInterest);

                    int ODDays = 0;

                    if ((OverduePrincipal + OverdueInterest) > 0)
                    {
                        var LastFullPaidDate = dt.Compute("Max(PlannedDate)", "LoanID = '" + UniqueLoanID + "' AND IsCompleted = '1' AND CollectionStatus = '2' AND PlannedDate <= '" + ToDate + "'");

                        try
                        {
                            DateTime.Parse(LastFullPaidDate.ToString());
                        }
                        catch (Exception ex)
                        {
                            //if no full payment till now, get the first planned rep date.
                            LastFullPaidDate = dt.Compute("Min(PlannedDate)", "LoanID = '" + UniqueLoanID + "'");
                        }

                        ODDays = (DateTime.Parse(ToDate) - DateTime.Parse(LastFullPaidDate.ToString())).Days;

                        if (ODDays > 0 && ODDays <= 30)
                        {
                            EntityOverdueOutstandingP_LT_30 += OverduePrincipal;
                            EntityOutstandingPrincipal_LT_30 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                        }
                        else if (ODDays > 30)
                        {
                            EntityOverdueOutstandingP_GT_30 += OverduePrincipal;
                            EntityOutstandingPrincipal_GT_30 += AmountDisbursed - (CollectedOverduePrincipal + CollectedPrincipal);
                        }

                    }
                } //unique loans

                if (EntityOverdueOutstandingP_LT_30 > 0 && EntityOutstandingPrincipal_LT_30 > 0)
                {
                    try
                    {
                        PAR_LT_30_To = Math.Round(EntityOverdueOutstandingP_LT_30 / EntityOutstandingPrincipal_LT_30, 2, MidpointRounding.AwayFromZero);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (EntityOverdueOutstandingP_GT_30 > 0 && EntityOutstandingPrincipal_GT_30 > 0)
                {
                    try
                    {
                        PAR_GT_30_To = Math.Round(EntityOverdueOutstandingP_GT_30 / EntityOutstandingPrincipal_GT_30, 2, MidpointRounding.AwayFromZero);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                #endregion

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>16</td>");
                sbrHTML.Append("<td>Portfolio at Risk above 1 Day</td>");
                sbrHTML.Append("<td>" + PAR_LT_30_From + "</td>");
                sbrHTML.Append("<td>" + (PAR_LT_30_To - PAR_LT_30_From).ToString() + "</td>");
                sbrHTML.Append("<td>" + PAR_LT_30_To + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append("<tr>");
                sbrHTML.Append("<td>16</td>");
                sbrHTML.Append("<td>Portfolio at Risk above 30 Days</td>");
                sbrHTML.Append("<td>" + PAR_GT_30_From + "</td>");
                sbrHTML.Append("<td>" + (PAR_GT_30_To - PAR_GT_30_From).ToString() + "</td>");
                sbrHTML.Append("<td>" + PAR_GT_30_To + "</td>");
                sbrHTML.Append("</tr>");

                sbrHTML.Append(sbrAgeing.ToString());

                sbrHTML.Append("</table>");

            }
            else
            {
                sbrHTML.Append(UiMsg.Report.NoData.InfoWrap());
            }

            return sbrHTML.ToString();
        }
    }
}