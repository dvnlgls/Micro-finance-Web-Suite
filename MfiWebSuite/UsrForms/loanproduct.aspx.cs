using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.UserClass;
using System.Data;
using MfiWebSuite.BL.MfiClass;
using MfiWebSuite.BL.Utilities;
using System.Text;
using MfiWebSuite.BL.LoanClasses;

namespace MfiWebSuite.UsrForms
{
    public partial class loanproduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string PageID = DbSettings.Menus.LoanProduct;

            Users objUsr = new Users();
            UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

            string UserID = objUsrDet.UserID;
            string UserStatusID = objUsrDet.StatusID;
            string UserRoleID = objUsrDet.RoleID;

            string UserOfficeID = objUsrDet.OfficeID;
            string UserOfficeTypeID = objUsrDet.OfficeTypeID;

            if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
            {
                CustEnum.PageAccess Access = objUsr.CheckPageAccess(UserRoleID, PageID);

                if (Access == CustEnum.PageAccess.Yes)
                {
                    //check for privileges

                    DataSet dsAction = objUsr.GetMenusActionsForRoleID(UserRoleID);
                    List<string> MenuActions = dsAction.Tables[0].AsEnumerable().Select(r => r[0].ToString()).ToList();

                    bool AddLP = MenuActions.Contains(DbSettings.AccessLoanProduct.Add);
                    //bool EditLP = MenuActions.Contains(DbSettings.AccessLoanProduct.Edit);
                    bool DeleteLP = MenuActions.Contains(DbSettings.AccessLoanProduct.Delete);

                    if (!AddLP)
                    {
                        uxAddProduct.InnerHtml = "";
                    }

                    Loans objLoan = new Loans();

                    DataSet dsLP = objLoan.GetLoanProducts(null);

                    if (!DataUtils.IsDataSetNull(dsLP, 0))
                    {
                        StringBuilder sbrLP = new StringBuilder();

                        foreach (DataRow drGrp in dsLP.Tables[0].Rows)
                        {
                            string ProductID = drGrp["LoanProductID"].ToString();
                            string ProductName = drGrp["ProductName"].ToString();
                            string MaxDisbAmnt = drGrp["DefualtAmount"].ToString();
                            string Interest = drGrp["Interest"].ToString();
                            string Tenure = drGrp["Tenure"].ToString();
                            string FundSourceName = drGrp["FundSourceName"].ToString();
                            string CollectionTypeID = drGrp["CollectionTypeID"].ToString();
                            string CollectionFrequency = drGrp["CollectionFrequency"].ToString();
                            string CreatedOnLocalDate = string.Empty;

                            DateTime CreatedOnUTC = DateTime.SpecifyKind(DateTime.Parse(drGrp["CreatedDateTime"].ToString()), DateTimeKind.Utc);
                            CreatedOnLocalDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedOnUTC, AppSettings.TimeZoneIst).ToDateShortMonth(false, '-');

                            sbrLP.Append("<tr id='LP_" + ProductID + "'>");
                            sbrLP.Append("<td>" + ProductName + "</td>");
                            sbrLP.Append("<td>" + MaxDisbAmnt + "</td>");
                            sbrLP.Append("<td>" + Interest + "</td>");
                            sbrLP.Append("<td>" + Tenure + "</td>");
                            sbrLP.Append("<td>" + FundSourceName + "</td>");

                            if (CollectionTypeID == DbSettings.CollectionType.Monthly)
                            {
                                sbrLP.Append("<td>Monthly</td>");
                            }
                            else if (CollectionTypeID == DbSettings.CollectionType.Days)
                            {
                                sbrLP.Append("<td>" + CollectionFrequency + " Days </td>");
                            }

                            sbrLP.Append("<td>" + CreatedOnLocalDate + "</td>");

                            if (DeleteLP)
                            {
                                sbrLP.Append("<td><a class='btn btn-danger btn-mini' id='btnDel_" + ProductID + "'><i class='icon-white icon-trash'></i>&nbsp;Delete Product</a></td>");
                            }
                            else
                            {
                                sbrLP.Append("<td>-</td>");
                            }
                        }

                        uxLpBody.InnerHtml = sbrLP.ToString();

                        //generate fund source

                        Common objFS = new Common();

                        DataSet dsFS = objFS.GetFundSource(null);

                        if (!DataUtils.IsDataSetNull(dsFS, 0))
                        {
                            StringBuilder sbrFS = new StringBuilder();

                            sbrFS.Append("<select id='drpFS' class='span3'>");
                            sbrFS.Append("<option value='0'>Select Fund Source</option>");

                            foreach (DataRow drFS in dsFS.Tables[0].Rows)
                            {
                                string FsID = drFS["FundSourceID"].ToString();
                                string FundSourceName = drFS["FundSourceName"].ToString();

                                sbrFS.Append("<option value='" + FsID + "'>" + FundSourceName + "</option>");                              
                            }

                            sbrFS.Append("</select>");
                            uxFundSource.InnerHtml = sbrFS.ToString();
                        }

                    }
                    else
                    {
                        uxLpContent.InnerHtml = UiMsg.LoanProduct.NoLpData.ErrorWrap();
                    }
                }
                else
                {
                    uxLpContent.InnerHtml = UiMsg.Global.NoPageAccess.ErrorWrap();
                }
            }

        }
    }
}