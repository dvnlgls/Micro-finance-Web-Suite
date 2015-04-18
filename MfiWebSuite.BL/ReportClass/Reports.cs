using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using MfiWebSuite.BL.Utilities;
using System.Data.SqlClient;

namespace MfiWebSuite.BL.ReportClass
{
    public class Reports
    {
        public DataSet LoanDisb(string BranchID, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_Report_LoanDisb", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (BranchID != null)
                {
                    cmd.Parameters.AddWithValue("@BranchID", int.Parse(BranchID));
                }

                cmd.Parameters.AddWithValue("@FromDate", DateTime.Parse(FromDate));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.Parse(ToDate));

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

        public DataSet OutstandingGeneric(string BranchID, string AsOnDate)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_Report_Outstanding_Generic", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (BranchID != null)
                {
                    cmd.Parameters.AddWithValue("@BranchID", int.Parse(BranchID));
                }

                cmd.Parameters.AddWithValue("@AsOnDate", DateTime.Parse(AsOnDate));
                
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

        public DataSet PurposeLoanPortfolio(string BranchID, string AsOnDate)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_Report_PurposeLoanPortfolio", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (BranchID != null)
                {
                    cmd.Parameters.AddWithValue("@BranchID", int.Parse(BranchID));
                }

                cmd.Parameters.AddWithValue("@AsOnDate", DateTime.Parse(AsOnDate));

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

        public DataSet DueVsCollectionGeneric(string BranchID, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_Report_DueCollecGeneric", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (BranchID != null)
                {
                    cmd.Parameters.AddWithValue("@BranchID", int.Parse(BranchID));
                }

                cmd.Parameters.AddWithValue("@FromDate", DateTime.Parse(FromDate));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.Parse(ToDate));

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

        public DataSet FeStatistics(string FeID, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_Report_FE_Stats", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FEID", int.Parse(FeID));
                cmd.Parameters.AddWithValue("@FromDate", DateTime.Parse(FromDate));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.Parse(ToDate));

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

        public DataSet MasterBranchReport(string BranchID, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_Report_Branch_Stats", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (BranchID != null)
                {
                    cmd.Parameters.AddWithValue("@BranchID", int.Parse(BranchID));
                }

                cmd.Parameters.AddWithValue("@FromDate", DateTime.Parse(FromDate));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.Parse(ToDate));

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

    }
}