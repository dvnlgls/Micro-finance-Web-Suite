using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MfiWebSuite.BL.Utilities;
using System.Data.SqlClient;
using System.Text;
using MfiWebSuite.BL.CommonClass;

namespace MfiWebSuite.BL.CommonClass
{
    public class Common
    {
        public string LogError(string ErrorType, string Message, string LoggedBy)
        {
            string ErrorID = "0";

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("log_Error", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ErrorType", int.Parse(ErrorType));
                cmd.Parameters.AddWithValue("@Message", Message);
                cmd.Parameters.AddWithValue("@LoggedByID", int.Parse(LoggedBy));

                con.Open();
                ErrorID = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return ErrorID;

        }

        /// <summary>
        /// input is optional. if "null" is passed, all entries will be returned
        /// </summary>
        /// <param name="FundSourceID"></param>
        /// <returns></returns>

        public DataSet GetFundSource(string FundSourceID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetFundSource", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (FundSourceID != null)
                {
                    cmd.Parameters.AddWithValue("@FSID", int.Parse(FundSourceID));
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
    }
}