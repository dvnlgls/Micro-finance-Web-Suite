using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MfiWebSuite.BL.Utilities;
using System.Data.SqlClient;
using MfiWebSuite.BL.CommonClass;
using System.Data;

namespace MfiWebSuite.BL.UserClass
{
    public class Users
    {
        public DataSet GetUsrAuthDetails(string EmailID)
        {
            //if (EmailID == AppVariables.WarningFbNoEmailProvided)
            //    return false;

            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetUserAuthenticationData", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pEmailId = new SqlParameter("@EmailId", SqlDbType.VarChar);
                pEmailId.Value = EmailID;
                cmd.Parameters.Add(pEmailId);

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

        //try to deprecate this method. 
        public DataSet GetUsrKeyDetails(string UserID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetUserSysKeyDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserId = new SqlParameter("@UserID", SqlDbType.Int);
                pUserId.Value = int.Parse(UserID);
                cmd.Parameters.Add(pUserId);

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

        public DataSet GetAllFeForUserID(string UserID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllFeForUserID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserId = new SqlParameter("@UserID", SqlDbType.Int);
                pUserId.Value = int.Parse(UserID);
                cmd.Parameters.Add(pUserId);

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

        public DataSet GetLoanEligClientsForUserID(string UserID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetLoanEligClientsForUserID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserId = new SqlParameter("@UserID", SqlDbType.Int);
                pUserId.Value = int.Parse(UserID);
                cmd.Parameters.Add(pUserId);

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

        public DataSet GetMenusForRoleID(string RoleID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetMenusForRole", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pRoleId = new SqlParameter("@RoleID", SqlDbType.Int);
                pRoleId.Value = int.Parse(RoleID);
                cmd.Parameters.Add(pRoleId);

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

        public DataSet GetMenusActionsForRoleID(string RoleID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetMenuActionsForRoleID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pRoleId = new SqlParameter("@RoleID", SqlDbType.Int);
                pRoleId.Value = int.Parse(RoleID);
                cmd.Parameters.Add(pRoleId);

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

        public UserKeyDetails GetUserKeyDetails()
        {
            UserKeyDetails obj = new UserKeyDetails();

            //get user id first
            Sessions objSession = new Sessions();
            string UserID = objSession.GetUserID();

            if (UserID != null)
            {
                Users objUsr = new Users();
                DataSet ds = objUsr.GetUsrKeyDetails(UserID);

                if (!DataUtils.IsDataSetNull(ds, 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    obj.UserID = UserID;
                    obj.StatusID = dr["UserStatusID"].ToString();
                    obj.RoleID = dr["UserRoleID"].ToString();
                    obj.OfficeID = dr["OfficeID"].ToString();
                    obj.Name = dr["Name"].ToString().ToTitleCase();
                    obj.OfficeTypeID = dr["OfficeTypeID"].ToString();
                }
            }

            return obj;
        }

        public CustEnum.PageAccess CheckPageAccess(string RoleID, string PageID)
        {
            CustEnum.PageAccess OpResult = CustEnum.PageAccess.Error;

            try
            {
                Users objUsr = new Users();

                DataSet dsMenu = objUsr.GetMenusForRoleID(RoleID);
                if (!DataUtils.IsDataSetNull(dsMenu, 0))
                {
                    List<DataRow> lst = dsMenu.Tables[0].AsEnumerable().ToList();


                    int count = (from r in lst
                                 where r.Field<int>("MenuID") == int.Parse(PageID)
                                 select r.Field<int>("MenuID")).SingleOrDefault<int>();

                    if (count == 0)
                    {
                        OpResult = CustEnum.PageAccess.No;

                    }
                    else
                    {
                        OpResult = CustEnum.PageAccess.Yes;
                    }
                }
            }
            catch { }

            return OpResult;
        }

        public string AddNewClient(
            string ClientName
            , string ClientStatusID
            , string ClientVill
            , string CreatedByID
            , string Gender
            , string Xof
            , string Age
            , string FamilyName
            , string FamilyRel
            , string FamilySex
            , string FamilyAge
            , string FamilyOccupation
            , string FamilyEducation
            , string EarningNature
            , string EarningIncome
            , string EarningExpen
            , string EarningSurplus
            , string ConsumptionExpen
            , string ConsumptionFestiveExpen
            , string Caste
            , string Address
            , string PIN
            , string Phone
            , string HasSavingsAccount
            , string AssetVillage
            , string AssetPlotNo
            , string AssetExtent
            , string AssetRooms
            , string AssetRoof
            , string AssetValue
            , string AssetDesc
            , string AssetValue2
            , string Creditsource
            , string CreditROI
            , string CreditPeriod
            , string CreditPurpose
            , string CreditBorrowed
            , string CreditDue
            , string NomineeName
            , string NomineeRel
            , string NomineeAge
            , string NomineeVillage
                        )
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_SaveClientDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClientName", ClientName);
                cmd.Parameters.AddWithValue("@ClientStatusID", int.Parse(ClientStatusID));
                cmd.Parameters.AddWithValue("@VillageID", int.Parse(ClientVill));
                cmd.Parameters.AddWithValue("@CreatedByID", int.Parse(CreatedByID));
                cmd.Parameters.AddWithValue("@Gender", int.Parse(Gender));
                cmd.Parameters.AddWithValue("@Xof", Xof);
                cmd.Parameters.AddWithValue("@Age", Age);
                cmd.Parameters.AddWithValue("@FamilyName", FamilyName);
                cmd.Parameters.AddWithValue("@FamilyRel", FamilyRel);
                cmd.Parameters.AddWithValue("@FamilySex", FamilySex);
                cmd.Parameters.AddWithValue("@FamilyAge", FamilyAge);
                cmd.Parameters.AddWithValue("@FamilyOccupation", FamilyOccupation);
                cmd.Parameters.AddWithValue("@FamilyEducation", FamilyEducation);
                cmd.Parameters.AddWithValue("@EarningNature", EarningNature);
                cmd.Parameters.AddWithValue("@EarningIncome", EarningIncome);
                cmd.Parameters.AddWithValue("@EarningExpen", EarningExpen);
                cmd.Parameters.AddWithValue("@EarningSurplus", EarningSurplus);
                cmd.Parameters.AddWithValue("@ConsumptionExpen", ConsumptionExpen);
                cmd.Parameters.AddWithValue("@ConsumptionFestiveExpen", ConsumptionFestiveExpen);
                cmd.Parameters.AddWithValue("@Caste", Caste);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@PIN", PIN);
                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@HasSavingsAccount", int.Parse(HasSavingsAccount));
                cmd.Parameters.AddWithValue("@AssetVillage", AssetVillage);
                cmd.Parameters.AddWithValue("@AssetPlotNo", AssetPlotNo);
                cmd.Parameters.AddWithValue("@AssetExtent", AssetExtent);
                cmd.Parameters.AddWithValue("@AssetRooms", AssetRooms);
                cmd.Parameters.AddWithValue("@AssetRoof", AssetRoof);
                cmd.Parameters.AddWithValue("@AssetValue", AssetValue);
                cmd.Parameters.AddWithValue("@AssetDesc", AssetDesc);
                cmd.Parameters.AddWithValue("@AssetValue2", AssetValue2);
                cmd.Parameters.AddWithValue("@Creditsource", Creditsource);
                cmd.Parameters.AddWithValue("@CreditROI", CreditROI);
                cmd.Parameters.AddWithValue("@CreditPeriod", CreditPeriod);
                cmd.Parameters.AddWithValue("@CreditPurpose", CreditPurpose);
                cmd.Parameters.AddWithValue("@CreditBorrowed", CreditBorrowed);
                cmd.Parameters.AddWithValue("@CreditDue", CreditDue);
                cmd.Parameters.AddWithValue("@NomineeName", NomineeName);
                cmd.Parameters.AddWithValue("@NomineeRel", NomineeRel);
                cmd.Parameters.AddWithValue("@NomineeAge", NomineeAge);
                cmd.Parameters.AddWithValue("@NomineeVillage", NomineeVillage);


                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();
                
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;
        }
        /// <summary>
        /// returns group id if successful. use try parse to check.
        /// </summary>
        /// <param name="CenterID"></param>
        /// <param name="FEID"></param>
        /// <param name="CreatedByID"></param>
        /// <returns></returns>
        public string CreateGroup(string CenterID, string FEID, string CreatedByID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_CreateNewGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pCenterID = new SqlParameter("@CenterID", SqlDbType.Int);
                pCenterID.Value = int.Parse(CenterID);
                cmd.Parameters.Add(pCenterID);

                SqlParameter pFE = new SqlParameter("@FEID", SqlDbType.Int);
                pFE.Value = int.Parse(FEID);
                cmd.Parameters.Add(pFE);

                SqlParameter pCreatedBy = new SqlParameter("@CreatedByID", SqlDbType.Int);
                pCreatedBy.Value = int.Parse(CreatedByID);
                cmd.Parameters.Add(pCreatedBy);

                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;
        }

        /// <summary>
        /// returns group client id is successful. else 0 will be returned if failed at db level.
        /// by default generic error will be returned. so, use try parse to determine if > 0
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="ClientID"></param>
        /// <param name="IsLeader"></param>
        /// <param name="CreatedByID"></param>
        /// <returns></returns>
        public string AddGroupClient(string GroupID, string ClientID, string IsLeader, string CreatedByID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_AddGroupClient", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@GroupID", int.Parse(GroupID));
                cmd.Parameters.AddWithValue("@ClientID", int.Parse(ClientID));
                cmd.Parameters.AddWithValue("@IsLeader", int.Parse(IsLeader));
                cmd.Parameters.AddWithValue("@CreatedByID", int.Parse(CreatedByID));

                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;
        }

        public DataSet GetNewGroupsForUserID(string UserID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetNewGroupsForUserID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", int.Parse(UserID));

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

        public DataSet GetAllGroupDetailsForUserID(string UserID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllGroupDetailsForUserID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", int.Parse(UserID));

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

    public class UserKeyDetails
    {
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public string StatusID { get; set; }
        public string OfficeID { get; set; }
        public string Name { get; set; }
        public string OfficeTypeID { get; set; }

    }

}