using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.UserClass;
using System.Data;
using MfiWebSuite.BL.Utilities;
using System.Text;

namespace MfiWebSuite.MasterPages
{
    public partial class _default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Sessions obj = new Sessions();
            string UserID = obj.GetUserID();

            if (UserID != null)
            {
                Users objUsr = new Users();
                DataSet ds = objUsr.GetUsrKeyDetails(UserID);

                if (!DataUtils.IsDataSetNull(ds, 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    string UserRoleID = dr["UserRoleID"].ToString();
                    string Name = dr["Name"].ToString().ToTitleCase();
                    uxUserName.InnerText = UiMsg.MasterDefault.UserSalutation + Name;

                    DataSet dsMenu = objUsr.GetMenusForRoleID(UserRoleID);
                    if (!DataUtils.IsDataSetNull(dsMenu, 0))
                    {
                        List<DataRow> lstMenu = dsMenu.Tables[0].AsEnumerable().ToList();

                        List<DataRow> drParentMenu = (from r in lstMenu
                                                      where r.Field<int>("ParentMenuID") == 0 && r.Field<int>("IsDisplay") == 1 //second filter(isdisplay) added on 3 jul 12
                                                      orderby r.Field<int>("OrderID")
                                                      select r).ToList<DataRow>();

                        StringBuilder sbrMainMenu = new StringBuilder();

                        for (int ctr = 0; ctr < drParentMenu.Count; ctr++)
                        {
                            //string IsDisplay = drParentMenu[ctr]["IsDisplay"].ToString(); 

                            string MenuID = drParentMenu[ctr]["MenuID"].ToString();
                            string MainMenuName = null;

                            List<DataRow> drSubMenu = (from r in lstMenu
                                                       where r.Field<int>("ParentMenuID") == int.Parse(MenuID) && r.Field<int>("IsDisplay") == 1 //second filter(isdisplay) added on 3 jul 12
                                                       orderby r.Field<int>("OrderID")
                                                       select r).ToList<DataRow>();


                            MainMenuName = drParentMenu[ctr]["DisplayName"].ToString();

                            if (drSubMenu.Count == 0)
                            {
                                string MainFilePath = null;                                

                                sbrMainMenu.Append("<li>");
                                MainFilePath = drParentMenu[ctr]["FilePath"].ToString();

                                if (string.IsNullOrEmpty(MainFilePath))
                                    MainFilePath = "#";
                                else
                                    MainFilePath = "/" + MainFilePath;

                                sbrMainMenu.Append("<a href='" + MainFilePath + "'>" + MainMenuName + "</a>");
                                sbrMainMenu.Append("</li>");
                            }
                            else
                            {
                                sbrMainMenu.Append("<li class='dropdown'>");
                                sbrMainMenu.Append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" + MainMenuName + "<b class='caret'></b></a>");
                                sbrMainMenu.Append("<ul class='dropdown-menu'>");

                                for (int ctr2 = 0; ctr2 < drSubMenu.Count; ctr2++)
                                {
                                    string SubFilePath = null;
                                    string SubMenuName = null;

                                    SubMenuName = drSubMenu[ctr2]["DisplayName"].ToString();
                                    SubFilePath = drSubMenu[ctr2]["FilePath"].ToString();

                                    if (string.IsNullOrEmpty(SubFilePath))
                                        SubFilePath = "#";
                                    else
                                        SubFilePath = "/" + SubFilePath;

                                    sbrMainMenu.Append("<li><a href='" + SubFilePath + "'>" + SubMenuName + "</a></li>");
                                }

                                sbrMainMenu.Append("</ul>");
                                sbrMainMenu.Append("</li>");
                            }

                        }

                        menu.InnerHtml = sbrMainMenu.ToString();

                    }
                }
            }
            else
            {
                obj.EndSession("&" + AppSettings.QueryStr.SessionExpired.Name + "=" + AppSettings.QueryStr.SessionExpired.Value);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Sessions obj = new Sessions();
            obj.EndSession();
        }

      
    }
}