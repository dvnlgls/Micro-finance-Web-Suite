using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.CommonClass;

namespace MfiWebSuite.Ajax
{
    public partial class account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["AjaxMethod"] == "CreateAccount")
            {
                string EmailID = Request.Form["EmailID"].SafeSqlLiteral(1);
                string Pwd = Request.Form["Pwd"].SafeSqlLiteral(1);

                string OpResult = CustEnum.Generic.Error_Default.ToString();

                if (string.IsNullOrEmpty(EmailID) || string.IsNullOrEmpty(Pwd))
                {
                    OpResult = CustEnum.CreateAccount.Error_IncorrectParams.ToString();
                }
                else
                {
                }

                Response.Write(OpResult);
                Response.End();
            }


        }
    }
}