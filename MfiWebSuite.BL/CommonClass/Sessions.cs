using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MfiWebSuite.BL.CommonClass;
using System.Web.Security;

namespace MfiWebSuite.BL.CommonClass
{
    public class Sessions
    {
        public bool SetSession(string UserID)//, string UserRoleID, string Name)
        {
            bool ReturnResponse = false;

            try
            {
                HttpContext.Current.Session[AppSettings.Session.UserID] = UserID;                

                ReturnResponse = true;
            }
            catch (Exception ex)
            {

            }

            return ReturnResponse;
        }

        public string GetUserID()
        {
            string UserID = null;
            try
            {
                //UserID = HttpContext.Current.Session[AppSettings.Session.UserID].ToString();
                UserID = HttpContext.Current.User.Identity.Name;
            }
            catch { }

            return UserID;
        }
               

        public void EndSession(string QueryStr)
        {
            try
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.User = null;
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage(QueryStr);                
            }
            catch { }
        }

        public void EndSession()
        {
            try
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.User = null;
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
            catch { }
        }
    }
}