using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.LoanClasses;
using MfiWebSuite.BL.UserClass;
using System.Text;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.MfiClass;
using System.Data;

namespace MfiWebSuite.Ajax
{
    public partial class loan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.Form["AjaxMethod"] == "laf")
            {
                Sessions objSess = new Sessions();

                string UserID = objSess.GetUserID();

                string OpResult = CustEnum.Generic.Error_SessionExpired.ToString();

                if (UserID != null)
                {
                    //string VillageID = Request.Form["Vil"]; //if time permits, try to check if the clients belong to this village!
                    string CenterID = Request.Form["Cen"];
                    string FeID = Request.Form["FE"];
                    string LpID = Request.Form["LP"];
                    string ClientObj = Request.Form["Client"];

                    int ClientCount = 0;
                    int ClientGroupCount = 0;
                    int LoanCount = 0;

                    OpResult = CustEnum.LAF.Error_Default.ToString();

                    StringBuilder sbrErrorMsgs = new StringBuilder();

                    LAF objCLi = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<LAF>(ClientObj);

                    ClientCount = objCLi.clients.Count;

                    if (ClientCount > 0)
                    {
                        //populate error trace
                        sbrErrorMsgs.AppendLine("Center: " + CenterID);
                        sbrErrorMsgs.AppendLine("FeID: " + FeID);
                        sbrErrorMsgs.AppendLine("Lpid: " + LpID);
                        sbrErrorMsgs.AppendLine("Formatted Client object str: " + ClientObj.SafeSqlLiteral(1));

                        //create group.
                        int GroupID;

                        Users objUser = new Users();

                        int.TryParse(objUser.CreateGroup(CenterID, FeID, UserID), out GroupID);

                        if (GroupID > 0)
                        {
                            //foreach (var item in objCLi.clients)
                            for (int ctr = 0; ctr < objCLi.clients.Count; ctr++)
                            {
                                string ClientID = objCLi.clients[ctr].CliID;
                                string LoanAmount = objCLi.clients[ctr].Amnt;
                                string PurposeID = objCLi.clients[ctr].PurpID;
                                string LoanCycle = objCLi.clients[ctr].Cycle;

                                int IsLeader = 0;
                                if (ctr == 0)
                                {
                                    IsLeader = 1; //only the first client will be group leader
                                }

                                //add client to group
                                int GroupClientID;
                                int.TryParse(objUser.AddGroupClient(GroupID.ToString(), ClientID, IsLeader.ToString(), UserID), out GroupClientID);

                                if (GroupClientID > 0)
                                {
                                    ClientGroupCount++;

                                    //apply loan for the client.
                                    Loans objLoan = new Loans();

                                    int LoanID;
                                    int.TryParse(objLoan.ApplyLoanForClient(GroupClientID.ToString(), DbSettings.LoanStatus.Applied, LpID, FeID, LoanAmount, PurposeID, UserID), out LoanID);

                                    if (LoanID > 0)
                                    {
                                        LoanCount++;
                                    }
                                    else
                                    {
                                        //error. loan not applied.
                                        sbrErrorMsgs.AppendLine("Loan not applied for Client: " + ClientID + "");
                                    }
                                }
                                else
                                {
                                    //error: client not added to group client table
                                    sbrErrorMsgs.AppendLine("Client: " + ClientID + " not added to Group.");
                                }

                            }
                        }
                        else
                        {
                            //error. group creation failed.
                            sbrErrorMsgs.AppendLine("Group creation failed");
                        }

                        if ((ClientCount == ClientGroupCount) && (ClientCount == LoanCount))
                        {
                            OpResult = CustEnum.LAF.Success_.ToString() + GroupID.ToString();
                        }
                        else
                        {
                            Common objErr = new Common();

                            string ErrorID = objErr.LogError(DbSettings.ErrorType.LafProcess, sbrErrorMsgs.ToString(), UserID);

                            OpResult = CustEnum.LAF.Error_Process_.ToString() + ErrorID;
                        }

                    }
                    else
                    {
                        //error. no clients
                        // OpResult = CustEnum.LAF.Error_NoClients.ToString();
                    }


                }

                Response.Write(OpResult);
                Response.End();

            } //LAF



            if (Request.Form["AjaxMethod"] == "disburse")
            {
                Sessions objSess = new Sessions();

                string UserID = objSess.GetUserID();

                string OpResult = CustEnum.Disbursement.Error_Default.ToString();

                if (UserID != null)
                {
                    string GroupID = Request.Form["GID"];
                    string Interest = Request.Form["Int"];
                    string Installments = Request.Form["Inst"];
                    string LoanData = Request.Form["LoanData"];

                    int SuccessCtr = 0; //to count no of successfull trans

                    Disbursement objDisb = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Disbursement>(LoanData);

                    if (objDisb.loan.Count > 0)
                    {
                        Loans objLoan = new Loans();

                        for (int ctr = 0; ctr < objDisb.loan.Count; ctr++)
                        {
                            string LoanID = objDisb.loan[ctr].LoanID;
                            string DisbAmount = objDisb.loan[ctr].Amnt;
                            string DisbDate = objDisb.loan[ctr].DisbDate;
                            string FirstRepDate = objDisb.loan[ctr].FrDate;
                            string Emi = objDisb.loan[ctr].Emi;

                            if (FirstRepDate == "")
                                FirstRepDate = null;

                            if (Emi == "")
                                Emi = null;

                            if (objLoan.DisburseLoan(LoanID, UserID, GroupID, DbSettings.GroupStatus.Active, DisbAmount, Interest, Installments, DisbDate, FirstRepDate, Emi) == CustEnum.Disbursement.Success_.ToString())
                                SuccessCtr++;

                        }

                        if (SuccessCtr == objDisb.loan.Count)
                            OpResult = CustEnum.Disbursement.Success_.ToString();
                        else
                        {
                            Common objErr = new Common();

                            string ErrorID = objErr.LogError(DbSettings.ErrorType.RPSError, GroupID, UserID);

                            OpResult = CustEnum.Disbursement.Error_Process_.ToString() + ErrorID;
                        }
                    }
                }

                Response.Write(OpResult);
                Response.End();
            }


            if (Request.Form["AjaxMethod"] == "RejectLoan")
            {
                string GroupID = Request.Form["GID"];

                string OpResult = CustEnum.DeleteOffice.Error.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                //string UserRoleID = objUsrDet.RoleID;
                string UserOfficeID = objUsrDet.OfficeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Office objOff = new Office();

                    string Result = objOff.DeleteGroup(GroupID, UserID, DbSettings.LoanStatus.Rejected);

                    if (Result != CustEnum.Generic.Error_Default.ToString())
                    {
                        if (Result == "1") // string comp is guaranteed to be safe check. else have to handle null/err cases
                        {
                            OpResult = CustEnum.DeleteOffice.Success.ToString();
                        }
                        //else if (Result == "-1")
                        //{
                        //    OpResult = CustEnum.DeleteOffice.HasSubOffice.ToString();
                        //}
                    }

                }
                else
                {
                    OpResult = CustEnum.Generic.Error_SessionExpired.ToString();
                }

                Response.Write(OpResult);
                Response.End();
            }


            if (Request.Form["AjaxMethod"] == "AddLP")
            {
                string LpName = Request.Form["Name"].SafeSqlLiteral(1);
                string MaxAmount = Request.Form["Amount"];
                string Interest = Request.Form["Interest"];
                string Tenure = Request.Form["Tenure"];
                string FSID = Request.Form["FS"];
                string RepTypeID = Request.Form["RepType"];
                string RepDays = Request.Form["RepDays"];
                string CycleRange = Request.Form["CycleRange"];

                string LcMin1 = null;
                string LcMin2 = null;
                string LcMin3 = null;
                string LcMin4 = null;
                string LcMin5 = null;

                string LcMax1 = null;
                string LcMax2 = null;
                string LcMax3 = null;
                string LcMax4 = null;
                string LcMax5 = null;

                if (!string.IsNullOrEmpty(CycleRange))
                {
                    int Length = CycleRange.Split('~').Length;

                    for (int ctr = 0; ctr < Length; ctr++)
                    {
                        string CycleID = (CycleRange.Split('~')[ctr]).Split('_')[0];

                        if (CycleID == "1")
                        {
                            LcMin1 = (CycleRange.Split('~')[ctr]).Split('_')[1];
                            LcMax1 = (CycleRange.Split('~')[ctr]).Split('_')[2];
                        }

                        if (CycleID == "2")
                        {
                            LcMin2 = (CycleRange.Split('~')[ctr]).Split('_')[1];
                            LcMax2 = (CycleRange.Split('~')[ctr]).Split('_')[2];
                        }

                        if (CycleID == "3")
                        {
                            LcMin3 = (CycleRange.Split('~')[ctr]).Split('_')[1];
                            LcMax3 = (CycleRange.Split('~')[ctr]).Split('_')[2];
                        }

                        if (CycleID == "4")
                        {
                            LcMin4 = (CycleRange.Split('~')[ctr]).Split('_')[1];
                            LcMax4 = (CycleRange.Split('~')[ctr]).Split('_')[2];
                        }

                        if (CycleID == "5")
                        {
                            LcMin5 = (CycleRange.Split('~')[ctr]).Split('_')[1];
                            LcMax5 = (CycleRange.Split('~')[ctr]).Split('_')[2];
                        }
                    }

                }


                string OpResult = CustEnum.Generic.Error_Default.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Loans objLoan = new Loans();
                    int LPID = 0;
                    int RepValue;

                    int.TryParse(RepDays, out RepValue);

                    if (RepValue < 1)
                        RepDays = "0";

                    int.TryParse(objLoan.AddLoanProduct(LpName, MaxAmount, Interest, Tenure, FSID, RepTypeID, RepDays, UserID, LcMin1, LcMin2, LcMin3, LcMin4, LcMin5, LcMax1, LcMax2, LcMax3, LcMax4, LcMax5), out LPID);

                    if (LPID > 0)
                    {
                        OpResult = CustEnum.Generic.Success_.ToString() + LPID.ToString();
                    }

                }


                Response.Write(OpResult);
                Response.End();
            }

            if (Request.Form["AjaxMethod"] == "DeleteLP")
            {
                string LPID = Request.Form["LPID"];

                string OpResult = CustEnum.DeleteLP.Error.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                //string UserRoleID = objUsrDet.RoleID;
                string UserOfficeID = objUsrDet.OfficeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Loans objLoan = new Loans();

                    int Result;

                    int.TryParse(objLoan.DeleteLP(LPID, UserID), out Result);

                    if (Result == 1)
                    {
                        OpResult = CustEnum.DeleteLP.Success.ToString();
                    }

                    else if (Result == -1)
                    {
                        OpResult = CustEnum.DeleteLP.LinkedToLoan.ToString();
                    }

                }

                Response.Write(OpResult);
                Response.End();
            }



            if (Request.Form["AjaxMethod"] == "GetMeetingDatesForCenter")
            {
                string CenterID = Request.Form["CenterID"];

                string OpResult = CustEnum.Generic.Error_Default.ToString();
                bool IsFirstRecord = true; //to skip disabled attribute if more than one rep dates are present for a center


                Loans objLoan = new Loans();

                DataSet dsDate = objLoan.GetPlannedMeetingDateForCenter(CenterID);

                StringBuilder sbrDate = new StringBuilder();

                if (!DataUtils.IsDataSetNull(dsDate, 0))
                {
                    //sbrDate.Append("<select class='span2' id='drpPlannedDate'>");
                    //sbrDate.Append("<option value='0'>Select Center</option>");

                    foreach (DataRow dr in dsDate.Tables[0].Rows)
                    {
                        string Date = dr["PlannedDate"].ToString();

                        if (IsFirstRecord)
                        {
                            sbrDate.Append("<option value='" + Date + "'>" + DateTime.Parse(Date).ToDateShortMonth(false, '-') + "</option>");
                            IsFirstRecord = false;
                        }
                        else
                            sbrDate.Append("<option value='" + Date + "' disabled='disabled' style='color:red'>" + DateTime.Parse(Date).ToDateShortMonth(false, '-') + "</option>");

                        
                    }
                    //sbrDate.Append("</select>");

                    OpResult = sbrDate.ToString();
                }


                Response.Write(OpResult);
                Response.End();
            }


            if (Request.Form["AjaxMethod"] == "GetRepaymentData")
            {
                string CenterID = Request.Form["CenterID"];
                string PlannedDate = Request.Form["PlannedDate"];
                bool IsReport = false;
                int LoanRecordCtr = 0; //to check if there is any loan record pushed to ui. If 0, no need to generate the "rep. process button";

                try
                {
                    if (Request.Form["IsReport"] == "1")
                        IsReport = true;
                }
                catch (Exception ex)
                {   
                }

                string OpResult = CustEnum.Generic.Error_Default.ToString();

                Loans objLoan = new Loans();

                DataSet dsRep = objLoan.GetRepaymentDataForCenterID(CenterID, DateTime.Parse(PlannedDate));

                StringBuilder sbrRepData = new StringBuilder();

                if (!DataUtils.IsDataSetNull(dsRep, 0))
                {
                    DataTable dt = dsRep.Tables[0];

                    //filter unique loans
                    var distinctGroups = (from row in dt.AsEnumerable()
                                          where row.Field<DateTime>("PlannedDate") == DateTime.Parse(PlannedDate)
                                          select row.Field<int>("GroupID")).Distinct();

                    foreach (var UniqueGroupID in distinctGroups)
                    {
                        //filter unique loans
                        var distinctLoans = (from row in dt.AsEnumerable()
                                             where row.Field<int>("GroupID") == int.Parse(UniqueGroupID.ToString())
                                             select row.Field<int>("LoanID")).Distinct();

                        double GroupTotalDue = 0;

                        foreach (var UniqueLoanID in distinctLoans)
                        {
                            //get the record for the current meeting date
                            var currentLoan = from myRow in dt.AsEnumerable()
                                              where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") == DateTime.Parse(PlannedDate)
                                              select myRow;

                            foreach (DataRow dr in currentLoan)
                            {
                                LoanRecordCtr++;

                                string LoanID = dr["LoanID"].ToString();
                                string LoanInstID = dr["LoanInstID"].ToString();
                                string ClientID = dr["ClientID"].ToString();
                                string ClientName = dr["Name"].ToString();
                                string ProductName = dr["ProductName"].ToString();
                                int InstallmentNo = (from myRow in dt.AsEnumerable()
                                                     where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") <= DateTime.Parse(PlannedDate)
                                                     select myRow).Count();

                                //double TotalExpectedPrincipal = (from myRow in dt.AsEnumerable()
                                //                                 where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") < DateTime.Parse(PlannedDate)
                                //                                 select myRow).Sum(row => row.Field<double>("ExpectedPrincipal"));
                                
                                //double TotalExpectedInterest = (from myRow in dt.AsEnumerable()
                                //                               where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") < DateTime.Parse(PlannedDate)
                                //                                select myRow).Sum(row => row.Field<double>("ExpectedInterest"));


                                //double TotalCollectedInterest = (from myRow in dt.AsEnumerable()
                                //                                 where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") < DateTime.Parse(PlannedDate)
                                //                                 select myRow).Sum(row => row.Field<double>("CollectedInterest"));
                                
                                //double TotalCollectedPrincipal = (from myRow in dt.AsEnumerable()
                                //                                  where myRow.Field<int>("LoanID") == int.Parse(UniqueLoanID.ToString()) && myRow.Field<DateTime>("PlannedDate") < DateTime.Parse(PlannedDate)
                                //                                 select myRow).Sum(row => row.Field<double>("CollectedPrincipal"));

                                double OverdueInterest = double.Parse(dr["OverdueInterest"].ToString());
                                double OverduePrincipal = double.Parse(dr["OverduePrincipal"].ToString());

                                //if( (TotalExpectedInterest - TotalCollectedInterest) > 0)
                                //    OverdueInterest =  TotalExpectedInterest - TotalCollectedInterest;

                                //if ((TotalExpectedPrincipal - TotalCollectedPrincipal) > 0)
                                //    OverduePrincipal = TotalExpectedPrincipal - TotalCollectedPrincipal;

                                double CurrentPrincipal = double.Parse(dr["ExpectedPrincipal"].ToString());
                                double CurrentInterest = double.Parse(dr["ExpectedInterest"].ToString());

                                double CurrentDue = CurrentPrincipal + OverduePrincipal + CurrentInterest + OverdueInterest;

                                GroupTotalDue += CurrentDue;

                                //html
                                sbrRepData.Append("<tr id='TR_" + LoanInstID + "'>");
                                sbrRepData.Append("<td>" + LoanID + "</td>");
                                sbrRepData.Append("<td>" + ClientID + "</td>");
                                sbrRepData.Append("<td>" + ClientName + "</td>");
                                sbrRepData.Append("<td>" + ProductName + "</td>");
                                sbrRepData.Append("<td>" + InstallmentNo.ToString() + "</td>");
                                sbrRepData.Append("<td>" + OverdueInterest.ToString() + "</td>");
                                sbrRepData.Append("<td>" + OverduePrincipal.ToString() + "</td>");
                                sbrRepData.Append("<td>" + CurrentInterest.ToString() + "</td>");
                                sbrRepData.Append("<td>" + CurrentPrincipal.ToString() + "</td>");
                                sbrRepData.Append("<td>" + CurrentDue.ToString() + "</td>");

                                if (IsReport)
                                {
                                    sbrRepData.Append("<td></td><td></td><td></td><td></td><td></td><td></td>");
                                }
                                else
                                {
                                    sbrRepData.Append("<td><input type='text' class='input-mini' value='" + OverdueInterest.ToString() + "' id='ODI_" + LoanInstID + "'/></td>");
                                    sbrRepData.Append("<td><input type='text' class='input-mini' value='" + OverduePrincipal.ToString() + "' id='ODP_" + LoanInstID + "' /></td>");
                                    sbrRepData.Append("<td><input type='text' class='input-mini' value='" + CurrentInterest.ToString() + "' id='CI_" + LoanInstID + "'/></td>");
                                    sbrRepData.Append("<td><input type='text' class='input-mini' value='" + CurrentPrincipal.ToString() + "' id='CP_" + LoanInstID + "'/></td>");
                                    sbrRepData.Append("<td><input type='text' class='input-mini' value='" + CurrentDue.ToString() + "' id='TD_" + LoanInstID + "'/></td>");
                                    sbrRepData.Append("<td><input type='text' class='input-mini' id='RNO_" + LoanInstID + "'/></td>");
                                }
                                sbrRepData.Append("</tr>");
                                break;
                            }

                        } //unique loan

                        sbrRepData.Append("<tr>");
                        sbrRepData.Append("<td colspan='9'><strong>Group Total</strong></td>");
                        sbrRepData.Append("<td><strong>" + GroupTotalDue + "</strong></td>");
                        sbrRepData.Append("<td colspan='6'></td>");
                        sbrRepData.Append("</tr>");

                    }//unique group

                    if (LoanRecordCtr > 0 && !IsReport)
                    {
                        sbrRepData.Append("<tr>");
                        sbrRepData.Append("<td colspan='16'>");
                        sbrRepData.Append("<a id='btnSaveRep' class='btn btn-large btn-success pull-right'><i class='icon-white icon-check'></i> Save</a>");
                        sbrRepData.Append("</td>");
                        sbrRepData.Append("</tr>");
                    }

                    OpResult = sbrRepData.ToString();
                }


                Response.Write(OpResult);
                Response.End();
            }



            if (Request.Form["AjaxMethod"] == "repayment")
            {
                string CollectionDate = Request.Form["CollecDate"];
                string RepaymentData = Request.Form["RepData"];

                string OpResult = CustEnum.Generic.Error_Default.ToString();
                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();
                string UserID = objUsrDet.UserID;
                
                Repayment objRep = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Repayment>(RepaymentData);

                int SuccessfulUpdateCtr = 0;

                if (objRep.repayment.Count > 0)
                {
                    Loans objLoan = new Loans();

                    for (int ctr = 0; ctr < objRep.repayment.Count; ctr++)
                    {
                        string LoanInstID = objRep.repayment[ctr].LoanInstID;
                        string CollectedAmnt = objRep.repayment[ctr].CollectedAmnt;
                        string ReceiptNo = objRep.repayment[ctr].ReceiptNo;

                        if (objLoan.SaveRepaymentData(LoanInstID, CollectedAmnt, CollectionDate, ReceiptNo, UserID))
                            SuccessfulUpdateCtr++;
                    }
                }

                if (objRep.repayment.Count == SuccessfulUpdateCtr)
                    OpResult = "success";

                Response.Write(OpResult);
                Response.End();
            }
      
          
      



        }
    }
}

