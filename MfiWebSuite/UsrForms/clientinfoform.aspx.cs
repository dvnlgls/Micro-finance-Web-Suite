using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.MfiClass;
using System.Data;
using MfiWebSuite.BL.UserClass;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.Utilities;
using System.Text;

namespace MfiWebSuite.UsrForms
{
    public partial class clientinfoform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Users objUsr = new Users();
            UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

            string UserID = objUsrDet.UserID;
            string UserStatusID = objUsrDet.StatusID;

            string UserRoleID = objUsrDet.RoleID;
            string UserOfficeID = objUsrDet.OfficeID;
            if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
            {
                CustEnum.PageAccess Access = objUsr.CheckPageAccess(UserRoleID, DbSettings.Menus.CIF);
                if (Access == CustEnum.PageAccess.Yes)
                {
                    string rr = null;
                    string did = null;
                    string dcn = null;
                    try
                    {
                        rr = Request.QueryString["rr"]; //redirect reason
                        did = Request.QueryString["did"]; //client id
                        dcn = Request.QueryString["dcn"]; //client name

                        if (rr == "s")
                        {
                            //gloErrHO.InnerHtml = String.Format(UiMsg.CIF.ClientAdded, "<a href='" + did + "'>" + dcn + "</a>").SuccessWrap();
                            gloErrHO.InnerHtml = String.Format(UiMsg.CIF.ClientAdded, "<a href='#'>" + dcn + "</a>").SuccessWrap();
                        }                        
                    }
                    catch { }

                    if (!Page.IsPostBack)
                    {
                        {
                            Office objOff = new Office();
                            DataSet dsOff = objOff.GetAllSubOfficeByTypeID(UserOfficeID, DbSettings.OfficeType.Village);

                            StringBuilder sbrVillage = new StringBuilder();
                            string OffID;
                            string OffName;

                            sbrVillage.Append("<select id='drpVil' class='span2'>");
                            //sbrVillage.Append("<option id='0'>Select</option>");
                            foreach (DataRow dr in dsOff.Tables[0].Rows)
                            {
                                OffID = dr["OfficeID"].ToString();
                                OffName = dr["OfficeName"].ToString();

                                sbrVillage.Append("<option id='" + OffID + "'>" + OffName + "</option>");

                            }
                            sbrVillage.Append("</select>");
                            uxClientVill.InnerHtml = sbrVillage.ToString();
                        }
                    }
                }
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Users objUsr = new Users();
            UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

            string UserID = objUsrDet.UserID;
            string UserStatusID = objUsrDet.StatusID;
                        
            if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
            {
                #region var
                
                string ClientName = uxClientName.Value.Trim().SafeSqlLiteral(1);
                string ClientVill = hdnOID.Value.Trim().SafeSqlLiteral(1);
                string CreatedByID = UserID;

                string Gender = uxClientGender.Value;
                string Xof = uxClientXof.Value.Trim().SafeSqlLiteral(1);
                string Age = uxClientAge.Value.Trim().SafeSqlLiteral(1);

                //family det
                string FamilyName = uxFamilyName1.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyName2.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyName3.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyName4.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyName5.Value.Trim().SafeSqlLiteral(1);

                string FamilyRel = uxFamilyRel1.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyRel2.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyRel3.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyRel4.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyRel5.Value.Trim().SafeSqlLiteral(1);

                string FamilySex = uxFamilyGender1.Value.Trim().SafeSqlLiteral(1) +
                                      AppSettings.Delimitter +
                                      uxFamilyGender2.Value.Trim().SafeSqlLiteral(1) +
                                      AppSettings.Delimitter +
                                      uxFamilyGender3.Value.Trim().SafeSqlLiteral(1) +
                                      AppSettings.Delimitter +
                                      uxFamilyGender4.Value.Trim().SafeSqlLiteral(1) +
                                      AppSettings.Delimitter +
                                      uxFamilyGender5.Value.Trim().SafeSqlLiteral(1);

                string FamilyAge = uxFamilyAge1.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyAge2.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyAge3.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyAge4.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyAge5.Value.Trim().SafeSqlLiteral(1);

                string FamilyOccupation = uxFamilyOccu1.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyOccu2.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyOccu3.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyOccu4.Value.Trim().SafeSqlLiteral(1) +
                                    AppSettings.Delimitter +
                                    uxFamilyOccu5.Value.Trim().SafeSqlLiteral(1);

                string FamilyEducation = uxFamilyEdu1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxFamilyEdu2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxFamilyEdu3.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxFamilyEdu4.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxFamilyEdu5.Value.Trim().SafeSqlLiteral(1);

                //earnings

                string EarningNature = uxEarningNature1.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningNature2.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningNature3.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningNature4.Value.Trim().SafeSqlLiteral(1);

                string EarningIncome = uxEarningREvenue1.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningREvenue2.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningREvenue3.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningREvenue4.Value.Trim().SafeSqlLiteral(1);

                string EarningExpen = uxEarningExpen1.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningExpen2.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningExpen3.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningExpen4.Value.Trim().SafeSqlLiteral(1);

                string EarningSurplus = uxEarningSurplus1.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningSurplus2.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningSurplus3.Value.Trim().SafeSqlLiteral(1) +
                                  AppSettings.Delimitter +
                                  uxEarningSurplus4.Value.Trim().SafeSqlLiteral(1);

                string Caste = uxCaste.Value;
                string Address = uxClientAddress.Value.Trim().SafeSqlLiteral(1);
                string PIN = uxClientPIN.Value.Trim().SafeSqlLiteral(1);
                string Phone = uxClientPhone.Value.Trim().SafeSqlLiteral(1);
                string HasSavingsAccount = uxClientBankAcc.Value;

                string ConsumptionExpen = uxconsExpen.Value.Trim().SafeSqlLiteral(1);
                string ConsumptionFestiveExpen = uxConsFestive.Value.Trim().SafeSqlLiteral(1);

                string AssetVillage = uxAssetVillage.Value.Trim().SafeSqlLiteral(1);
                string AssetPlotNo = uxAssetPlotNo.Value.Trim().SafeSqlLiteral(1);
                string AssetExtent = uxAssetExtent.Value.Trim().SafeSqlLiteral(1);
                string AssetRooms = uxAssetRooms.Value.Trim().SafeSqlLiteral(1);
                string AssetRoof = uxAssetRoof.Value.Trim().SafeSqlLiteral(1);
                string AssetValue = uxAssetValue.Value.Trim().SafeSqlLiteral(1);


                string AssetDesc = uxAssetDesc1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetDesc2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetDesc3.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetDesc4.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetDesc5.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetDesc6.Value.Trim().SafeSqlLiteral(1);


                string AssetValue2 = uxAssetMkVal1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetMkVal2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetMkVal3.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetMkVal4.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetMkVal5.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxAssetMkVal6.Value.Trim().SafeSqlLiteral(1);

                string Creditsource = uxCreditsource1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditsource2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditsource3.Value.Trim().SafeSqlLiteral(1);


                string CreditROI = uxCreditROI1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditROI2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditROI3.Value.Trim().SafeSqlLiteral(1);

                string CreditPeriod = uxCreditPeriod1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditPeriod2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditPeriod3.Value.Trim().SafeSqlLiteral(1);


                string CreditPurpose = uxCreditPurpose1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditPurpose2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditPurpose3.Value.Trim().SafeSqlLiteral(1);


                string CreditBorrowed = uxCreditBorrowed1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditBorrowed2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditBorrowed3.Value.Trim().SafeSqlLiteral(1);


                string CreditDue = uxCreditDue1.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditDue2.Value.Trim().SafeSqlLiteral(1) +
                                   AppSettings.Delimitter +
                                   uxCreditDue3.Value.Trim().SafeSqlLiteral(1);


                string NomineeName = uxNomineeName.Value.Trim().SafeSqlLiteral(1);
                string NomineeRel = uxNomineeRel.Value.Trim().SafeSqlLiteral(1);
                string NomineeAge = uxNomineeAge.Value.Trim().SafeSqlLiteral(1);
                string NomineeVillage = uxNomineePlace.Value.Trim().SafeSqlLiteral(1);

                #endregion var

                #region fnCall
                string Result = objUsr.AddNewClient(
             ClientName
             , DbSettings.ClientStatus.Idle
            , ClientVill
            , CreatedByID
            , Gender
            , Xof
            , Age
            , FamilyName
            , FamilyRel
            , FamilySex
            , FamilyAge
            , FamilyOccupation
            , FamilyEducation
            , EarningNature
            , EarningIncome
            , EarningExpen
            , EarningSurplus
            , ConsumptionExpen
            , ConsumptionFestiveExpen
            , Caste
            , Address
            , PIN
            , Phone
            , HasSavingsAccount
            , AssetVillage
            , AssetPlotNo
            , AssetExtent
            , AssetRooms
            , AssetRoof
            , AssetValue
            , AssetDesc
            , AssetValue2
            , Creditsource
            , CreditROI
            , CreditPeriod
            , CreditPurpose
            , CreditBorrowed
            , CreditDue
            , NomineeName
            , NomineeRel
            , NomineeAge
            , NomineeVillage
                        );

                #endregion fnCall

                int InsertedID;
                int.TryParse(Result, out InsertedID);

                if (InsertedID > 0)
                {
                    Response.Redirect(AppRoutes.ClientInfoForm.URL + "?rr=s&did=" + InsertedID + "&dcn=" + uxClientName.Value);
                }
                else
                {
                    gloErrHO.InnerHtml = UiMsg.CIF.AddErr.ErrorWrap();
                }
            }

        }
    }
}