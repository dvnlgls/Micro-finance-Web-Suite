using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace MfiWebSuite.BL.Utilities
{
    public class Connection
    {
        SqlConnection _con;

        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MwsCon"].ConnectionString;
        }

        public bool OpenConnection()
        {
            string ConnString;

            try
            {
                ConnString = this.GetConnectionString();
                _con = new SqlConnection(ConnString);
                _con.Open();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void CloseConnection()
        {
            try
            {
                _con.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}