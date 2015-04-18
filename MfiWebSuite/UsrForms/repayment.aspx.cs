using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.UserClass;
using System.Text;
using System.Data;
using MfiWebSuite.BL.MfiClass;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.LoanClasses;

namespace MfiWebSuite.UsrForms
{
    public partial class repayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string PageID = DbSettings.Menus.LoanRepayment;

            Users objUsr = new Users();
            UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

            string UserID = objUsrDet.UserID;
            string UserStatusID = objUsrDet.StatusID;

            string UserRoleID = objUsrDet.RoleID;
            string UserOfficeID = objUsrDet.OfficeID;

            if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
            {
                CustEnum.PageAccess Access = objUsr.CheckPageAccess(UserRoleID, PageID);

                if (Access == CustEnum.PageAccess.Yes)
                {
                    Office objOff = new Office();
                    Loans objLoan = new Loans();

                    DataSet dsCen = objOff.GetAllCentersForUserID(UserID);
                    StringBuilder sbrCenter = new StringBuilder();

                    if (!DataUtils.IsDataSetNull(dsCen, 0))
                    {
                        sbrCenter.Append("<select class='span2' id='drpCenter'>");
                        sbrCenter.Append("<option value='0'>Select Center</option>");

                        foreach (DataRow dr in dsCen.Tables[0].Rows)
                        {
                            string CenterID = dr["CenterID"].ToString();
                            string VillageID = dr["ParentOfficeID"].ToString();

                            sbrCenter.Append("<option class='" + VillageID + "' value='" + CenterID + "'>Center " + CenterID + "</option>");
                        }
                        sbrCenter.Append("</select>");
                        uxDrpCenterWrap.InnerHtml = sbrCenter.ToString();

                    } //center
                    

                }
                else
                {
                    uxRepayWrap.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                }
            }
            else
            {
                uxRepayWrap.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
            }
        }
    }
}