using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MfiWebSuite.BL.Utilities;
using System.Data.SqlClient;
using System.Text;
using MfiWebSuite.BL.CommonClass;

namespace MfiWebSuite.BL.LoanClasses
{
    public class Loans
    {
        /// <summary>
        /// pass null to fetch all products.
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public DataSet GetLoanProducts(string ProductID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetLoanProducts", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (ProductID != null)
                {
                    cmd.Parameters.AddWithValue("@ParentID", int.Parse(ProductID));
                }

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return ds;

        }

        public DataSet GetLoanCycles(string LoanProductID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetLoanCycles", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (LoanProductID != null)
                {
                    cmd.Parameters.AddWithValue("@LoanProductID", LoanProductID);
                }

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return ds;

        }

        public DataSet GetLoanPurpose()
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetLoanPurpose", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return ds;

        }

        /// <summary>
        /// returns loan id if successful. else 0 will be returned if failed at db level.
        /// by default generic error will be returned. so, use try parse to determine if > 0
        /// </summary>
        /// <param name="GroupClientID"></param>
        /// <param name="LoanStatusID"></param>
        /// <param name="LoanProductID"></param>
        /// <param name="StaffAppliedID"></param>
        /// <param name="AmountApplied"></param>
        /// <param name="PurposeID"></param>
        /// <param name="CreatedByID"></param>
        /// <returns></returns>
        public string ApplyLoanForClient(string GroupClientID, string LoanStatusID, string LoanProductID, string StaffAppliedID, string AmountApplied, string PurposeID, string CreatedByID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_ApplyLoanForClient", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupClientID", int.Parse(GroupClientID));
                cmd.Parameters.AddWithValue("@LoanStatusID", int.Parse(LoanStatusID));
                cmd.Parameters.AddWithValue("@LoanProductID", int.Parse(LoanProductID));
                cmd.Parameters.AddWithValue("@StaffAppliedID", int.Parse(StaffAppliedID));
                cmd.Parameters.AddWithValue("@AmountApplied", double.Parse(AmountApplied));
                cmd.Parameters.AddWithValue("@PurposeID", int.Parse(PurposeID));
                cmd.Parameters.AddWithValue("@CreatedByID", int.Parse(CreatedByID));

                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;
        }

        public DataSet GetLafDetails(string GroupID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetLafDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", int.Parse(GroupID));

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return ds;

        }

        public string DisburseLoan(string LoanID, string UserID, string GroupID, string GroupStatusID, string PrincOut, string IntRate, string TotalInst, string StartDate, string FrDate, string Emi)
        {
            string OpResult = CustEnum.Disbursement.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_DisburseLoan", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LoanID", int.Parse(LoanID));
                cmd.Parameters.AddWithValue("@GroupID", int.Parse(GroupID));
                cmd.Parameters.AddWithValue("@GroupStatusID", int.Parse(GroupStatusID));
                cmd.Parameters.AddWithValue("@UserID", int.Parse(UserID));
                cmd.Parameters.AddWithValue("@PrincOut", float.Parse(PrincOut));
                cmd.Parameters.AddWithValue("@IntRate", float.Parse(IntRate));
                cmd.Parameters.AddWithValue("@TotalInst", int.Parse(TotalInst));
                cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(StartDate));

                if (FrDate != null)
                {
                    cmd.Parameters.AddWithValue("@FrDate", DateTime.Parse(FrDate));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FrDate", DBNull.Value);
                }

                if (Emi != null)
                {
                    cmd.Parameters.AddWithValue("@Emi", float.Parse(Emi));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Emi", DBNull.Value);
                }

                con.Open();
                if (cmd.ExecuteScalar().ToString() == "1")
                {
                    OpResult = CustEnum.Disbursement.Success_.ToString();
                }
                else
                {
                    OpResult = CustEnum.Disbursement.Error_Process_.ToString();
                }
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;
        }


        public string AddLoanProduct(string LpName, string MaxAmount, string Interest, string Tenure, string FSID, string RepTypeID, string RepDays, string CreatedByID, string LcMin1, string LcMin2, string LcMin3, string LcMin4, string LcMin5, string LcMax1, string LcMax2, string LcMax3, string LcMax4, string LcMax5)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_AddLoanProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LpName", LpName);
                cmd.Parameters.AddWithValue("@UserID", int.Parse(CreatedByID));
                cmd.Parameters.AddWithValue("@MaxAmount", float.Parse(MaxAmount));
                cmd.Parameters.AddWithValue("@Interest", float.Parse(Interest));
                cmd.Parameters.AddWithValue("@Tenure", int.Parse(Tenure));
                cmd.Parameters.AddWithValue("@FSID", int.Parse(FSID));
                cmd.Parameters.AddWithValue("@RepTypeID", int.Parse(RepTypeID));
                
                cmd.Parameters.AddWithValue("@RepDays", int.Parse(RepDays));

                if (LcMin1 != null)
                    cmd.Parameters.AddWithValue("@LcMin1", int.Parse(LcMin1));

                if (LcMin2 != null)
                    cmd.Parameters.AddWithValue("@LcMin2", float.Parse(LcMin2));

                if (LcMin3 != null)
                    cmd.Parameters.AddWithValue("@LcMin3", float.Parse(LcMin3));

                if (LcMin4 != null)
                    cmd.Parameters.AddWithValue("@LcMin4", float.Parse(LcMin4));

                if (LcMin5 != null)
                    cmd.Parameters.AddWithValue("@LcMin5", float.Parse(LcMin5));

                if (LcMax1 != null)
                    cmd.Parameters.AddWithValue("@LcMax1", float.Parse(LcMax1));

                if (LcMax2 != null)
                    cmd.Parameters.AddWithValue("@LcMax2", float.Parse(LcMax2));

                if (LcMax3 != null)
                    cmd.Parameters.AddWithValue("@LcMax3", float.Parse(LcMax3));

                if (LcMax4 != null)
                    cmd.Parameters.AddWithValue("@LcMax4", float.Parse(LcMax4));

                if (LcMax5 != null)
                    cmd.Parameters.AddWithValue("@LcMax5", float.Parse(LcMax5));


                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();

            }

            return OpResult;
        }

        public string DeleteLP(string LPID, string UserID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteLP", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LPID", int.Parse(LPID));
                cmd.Parameters.AddWithValue("@UserID", int.Parse(UserID));

                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;

        }

        public DataSet GetPlannedMeetingDateForCenter(string CenterID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetPlannedMeetingDateForCenter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CenterId", int.Parse(CenterID));
                
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return ds;

        }

        public DataSet GetRepaymentDataForCenterID(string CenterID, DateTime PlannedDate)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetRepaymentDataForCenterID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CenterId", int.Parse(CenterID));
                cmd.Parameters.AddWithValue("@PlannedDate", PlannedDate);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return ds;

        }

        public bool SaveRepaymentData(string InstallmentID, string CollectedAmount, string CollectedDate, string ReceiptNo, string UserID)
        {
            bool Result = false;

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_SaveRepaymentData", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InstallmentID", int.Parse(InstallmentID   ));
                cmd.Parameters.AddWithValue("@CollectedAmount", float.Parse(CollectedAmount));
                cmd.Parameters.AddWithValue("@CollectedDate", DateTime.Parse(CollectedDate));
                cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
                cmd.Parameters.AddWithValue("@UserID", int.Parse(UserID));
                
                con.Open();
                cmd.ExecuteNonQuery();
                Result = true;

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return Result;

        }

    }

    public class LAF
    {

        public List<LafData> clients { get; set; }
    }

    public class LafData
    {
        public string CliID { get; set; }
        public string Amnt { get; set; }
        public string PurpID { get; set; }
        public string Cycle { get; set; }
    }


    public class Disbursement
    {

        public List<LoanData> loan { get; set; }
    }

    public class LoanData
    {

        public string LoanID { get; set; }
        public string Amnt { get; set; }
        public string DisbDate { get; set; }
        public string FrDate { get; set; }
        public string Emi { get; set; }
    }

    public class Repayment
    {

        public List<RepData> repayment { get; set; }
    }

    public class RepData
    {

        public string LoanInstID { get; set; }
        public string CollectedAmnt { get; set; }
        public string ReceiptNo { get; set; }        
        
    }

}