using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.UserClass;
using System.Data;

namespace MfiWebSuite
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string QryStrSE = null;

            try
            {
                QryStrSE = Request.QueryString[AppSettings.QueryStr.SessionExpired.Name];
            }
            catch { }

            if (QryStrSE == AppSettings.QueryStr.SessionExpired.Value)
            {
                divLoginErr.InnerHtml = UiMsg.Global.SessionExpired.ErrorWrap();
            }

        }


        protected void btnMwsLogin_Click(object sender, EventArgs e)
        {           

            string EmailID = txtEmailID.Value.Trim().SafeSqlLiteral(1);
            string Pwd = txtPassword.Value.Trim().SafeSqlLiteral(1);

            if (string.IsNullOrEmpty(EmailID) || string.IsNullOrEmpty(Pwd))
            {
                //error
                divLoginErr.InnerHtml = UiMsg.Login.InvalidCredentials.ErrorWrap();
            }
            else
            {
                Users obj = new Users();
                DataSet ds = obj.GetUsrAuthDetails(EmailID);

                if (!DataUtils.IsDataSetNull(ds, 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (dr["OpResult"].ToString() == "0")
                    {
                        // count is more than one
                        divLoginErr.InnerHtml = UiMsg.Login.InvalidCredentials.ErrorWrap();
                    }
                    else
                    {
                        string UserStatusID = dr["UserStatusID"].ToString();

                        if (UserStatusID == DbSettings.UserStatus.Active)
                        {
                            string Salt = dr["Salt"].ToString();
                            string SavedPwd = dr["Password"].ToString();
                            string UserID = dr["UserID"].ToString();

                            CryptEngine objCrypt = new CryptEngine();

                            if (objCrypt.VerifyHash(Pwd, Salt, SavedPwd))
                            {
                                //auth success                                                                                  
                                FormsAuthentication.RedirectFromLoginPage(UserID, false);
                                //string[] url = Request.UrlReferrer.Query.Split(new string[] { "ReturnUrl=%2f" }, StringSplitOptions.None);
                                //Response.Redirect(url[1]);
                                
                            }
                            else
                            {
                                divLoginErr.InnerHtml = UiMsg.Login.InvalidCredentials.ErrorWrap();
                            }
                        }
                        else
                        {
                            //user is not allowed to login
                            divLoginErr.InnerHtml = UiMsg.Login.NotAuthorized.ErrorWrap();
                        }
                    }
                } //dataset not null
                else
                {
                    divLoginErr.InnerHtml = UiMsg.Global.Error.ErrorWrap();
                }

            } //valid mail & pwd

        }//btn login

    }
}