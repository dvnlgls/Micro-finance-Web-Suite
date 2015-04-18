using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MfiWebSuite.BL.Utilities;
using System.Data.SqlClient;
using System.Text;
using MfiWebSuite.BL.CommonClass;

namespace MfiWebSuite.BL.MfiClass
{
    public class Office
    {
        //Utils

        //public string BuildDeleteBtn(string EntityTypeID, string EntityID, string EntityName)
        //{
        //    StringBuilder sbrDelBtn = new StringBuilder();
        //    sbrDelBtn.Append("<button class='btn btn-danger' id='btnDel_" + EntityTypeID +"_" + EntityID + "'><i class='icon-white icon-trash'></i>&nbsp;Remove " + EntityName +"</button>");

        //    return sbrDelBtn.ToString();
        //}               

        //Utils

        public DataSet GetOfficeDetails(string OfficeID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetOfficeDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOfficeID = new SqlParameter("@OfficeID", SqlDbType.Int);
                pOfficeID.Value = int.Parse(OfficeID);
                cmd.Parameters.Add(pOfficeID);

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

        public DataSet GetAllSubOfficeByTypeID(string UserOfficeID, string SubOfficeTypeID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetSubOfficeOfTypeID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOfficeID = new SqlParameter("@OfficeID", SqlDbType.Int);
                pOfficeID.Value = int.Parse(UserOfficeID);
                cmd.Parameters.Add(pOfficeID);

                SqlParameter pSubOfficeTypeID = new SqlParameter("@OfficeTypeID", SqlDbType.Int);
                pSubOfficeTypeID.Value = int.Parse(SubOfficeTypeID);
                cmd.Parameters.Add(pSubOfficeTypeID);

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

        public DataSet GetImmediateParentOfficesForOfficeType(string OfficeID, string SubOfficeTypeID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetParentOfficesForOfficeType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOfficeID = new SqlParameter("@OfficeID", SqlDbType.Int);
                pOfficeID.Value = int.Parse(OfficeID);
                cmd.Parameters.Add(pOfficeID);

                SqlParameter pSubOfficeTypeID = new SqlParameter("@OfficeTypeID", SqlDbType.Int);
                pSubOfficeTypeID.Value = int.Parse(SubOfficeTypeID);
                cmd.Parameters.Add(pSubOfficeTypeID);

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

        public string CheckIfSubOfficeExistUnderOfficeID(string UserOfficeID, string SubOfficeID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_CheckIfSubOfficeExistUnderOfficeID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOfficeID = new SqlParameter("@OfficeID", SqlDbType.Int);
                pOfficeID.Value = int.Parse(UserOfficeID);
                cmd.Parameters.Add(pOfficeID);

                SqlParameter pSubOfficeID = new SqlParameter("@SubOfficeID", SqlDbType.Int);
                pSubOfficeID.Value = int.Parse(SubOfficeID);
                cmd.Parameters.Add(pSubOfficeID);

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

        public string GetSubOfficeCountForOffice(string OfficeID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetSubOfficeCountForOffice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOfficeID = new SqlParameter("@OfficeID", SqlDbType.Int);
                pOfficeID.Value = int.Parse(OfficeID);
                cmd.Parameters.Add(pOfficeID);


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

        public string GetGroupCountForCenterID(string CenterID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetGroupCountForCenterID", con);
                cmd.CommandType = CommandType.StoredProcedure;                              
                
                cmd.Parameters.AddWithValue("@CenterID", int.Parse(CenterID));

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

        public string DeleteOffice(string OfficeID, string UserID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteOffice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOfficeID = new SqlParameter("@OfficeID", SqlDbType.Int);
                pOfficeID.Value = int.Parse(OfficeID);
                cmd.Parameters.Add(pOfficeID);

                SqlParameter pUserID = new SqlParameter("@UserID", SqlDbType.Int);
                pUserID.Value = int.Parse(UserID);
                cmd.Parameters.Add(pUserID);

                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();

                if (OpResult == "1")
                {
                    OpResult = CustEnum.Generic.Success_.ToString();
                }

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;

        }

        public string DeleteCenter(string CenterID, string UserID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteCenter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                
                cmd.Parameters.AddWithValue("@CenterID", int.Parse(CenterID));
                cmd.Parameters.AddWithValue("@UserID", int.Parse(UserID));

                con.Open();
                OpResult = cmd.ExecuteScalar().ToString();

                if (OpResult == "1")
                {
                    OpResult = CustEnum.Generic.Success_.ToString();
                }

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }

            return OpResult;

        }

        public string DeleteGroup(string GroupID, string UserID, string LoanStatusID)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", int.Parse(GroupID));
                cmd.Parameters.AddWithValue("@UserID", int.Parse(UserID));
                cmd.Parameters.AddWithValue("@LoanStatusID", int.Parse(LoanStatusID));

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

        public DataSet GetFeByVillageID(string VillageID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetFeByVillageID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pVillageID = new SqlParameter("@VillageID", SqlDbType.Int);
                pVillageID.Value = int.Parse(VillageID);
                cmd.Parameters.Add(pVillageID);

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

        public DataSet GetFeByCenterID(string CenterID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllFesForCenterID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CenterID", int.Parse(CenterID));

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

        public DataSet GetCenterDetails(string CenterID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetCenterDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CenterID", int.Parse(CenterID));

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

        public DataSet GetGroupsForCenterID(string CenterID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetGroupsForCenterID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CenterID", int.Parse(CenterID));

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

        /// <summary>
        /// Fetches all the active suboffice for a given office id. 
        /// If SubOfficeTypeID is null, all the sub-offices are fetched
        /// </summary>
        /// <param name="ParentOfficeID"></param>
        /// <param name="SubOfficeTypeID"></param>
        /// <returns></returns>
        public DataSet GetAllImmediateSubOffices(string ParentOfficeID, string SubOfficeTypeID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllImmediateSubOffices", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pParentOfficeID = new SqlParameter("@ParentOfficeID", SqlDbType.Int);
                pParentOfficeID.Value = int.Parse(ParentOfficeID);
                cmd.Parameters.Add(pParentOfficeID);

                if (SubOfficeTypeID != null)
                {
                    SqlParameter pSubOfficeTypeID = new SqlParameter("@SubOfficeTypeID", SqlDbType.Int);
                    pSubOfficeTypeID.Value = int.Parse(SubOfficeTypeID);
                    cmd.Parameters.Add(pSubOfficeTypeID);
                }
                else
                {
                    SqlParameter pSubOfficeTypeID = new SqlParameter("@SubOfficeTypeID", SqlDbType.Int);
                    pSubOfficeTypeID.Value = DBNull.Value;
                    cmd.Parameters.Add(pSubOfficeTypeID);
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

        public bool UpdateOfficeDetails(string OfficeID, string UpdatedByID, string IsActive = "1", string OfficeName = null, string Address = null, string Phone1 = null, string Phone2 = null, string Fax = null, string Website = null, string EmailID = null)
        {
            bool OpResult = false;

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateOfficeDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOfficeID = new SqlParameter("@OfficeID", SqlDbType.Int);
                pOfficeID.Value = int.Parse(OfficeID);
                cmd.Parameters.Add(pOfficeID);

                SqlParameter pUpdatedByID = new SqlParameter("@UpdatedByID", SqlDbType.Int);
                pUpdatedByID.Value = int.Parse(UpdatedByID);
                cmd.Parameters.Add(pUpdatedByID);

                SqlParameter pIsActive = new SqlParameter("@IsActive", SqlDbType.Int);
                pIsActive.Value = int.Parse(IsActive);
                cmd.Parameters.Add(pIsActive);

                SqlParameter pOfficeName = new SqlParameter("@OfficeName", SqlDbType.VarChar);
                pOfficeName.Value = OfficeName;
                cmd.Parameters.Add(pOfficeName);

                SqlParameter pAddress = new SqlParameter("@Address", SqlDbType.VarChar);
                pAddress.Value = Address;
                cmd.Parameters.Add(pAddress);

                SqlParameter pPhone1 = new SqlParameter("@Phone1", SqlDbType.VarChar);
                pPhone1.Value = Phone1;
                cmd.Parameters.Add(pPhone1);

                SqlParameter pPhone2 = new SqlParameter("@Phone2", SqlDbType.VarChar);
                pPhone2.Value = Phone2;
                cmd.Parameters.Add(pPhone2);

                SqlParameter pFax = new SqlParameter("@Fax", SqlDbType.VarChar);
                pFax.Value = Fax;
                cmd.Parameters.Add(pFax);

                SqlParameter pWebsite = new SqlParameter("@Website", SqlDbType.VarChar);
                pWebsite.Value = Website;
                cmd.Parameters.Add(pWebsite);

                SqlParameter pEmailID = new SqlParameter("@EmailID", SqlDbType.VarChar);
                pEmailID.Value = EmailID;
                cmd.Parameters.Add(pEmailID);

                con.Open();
                if (int.Parse(cmd.ExecuteScalar().ToString()) == 1)
                {
                    OpResult = true;
                }

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();

            }

            return OpResult;
        }

        public bool UpdateCenter(string CenterID, string MeetingLocation, string MeetingTime, string FEID, string UpdatedByID)
        {
            bool OpResult = false;

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateCenter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CenterID", int.Parse(CenterID));
                cmd.Parameters.AddWithValue("@MeetingLocation", MeetingLocation);
                cmd.Parameters.AddWithValue("@MeetingTime", MeetingTime);
                cmd.Parameters.AddWithValue("@FEID", int.Parse(FEID));
                cmd.Parameters.AddWithValue("@UpdatedByID", int.Parse(UpdatedByID));

                con.Open();

                if (int.Parse(cmd.ExecuteScalar().ToString()) == 1)
                {
                    OpResult = true;
                }

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();

            }

            return OpResult;
        }

        public DataSet GetCentersForVillageID(string VillageID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetCentersForVillageID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pVillageID = new SqlParameter("@VillageID", SqlDbType.Int);
                pVillageID.Value = int.Parse(VillageID);
                cmd.Parameters.Add(pVillageID);

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

        public DataSet GetAllCentersForUserID(string UserID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllCentersForUserID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserID = new SqlParameter("@UserID", SqlDbType.Int);
                pUserID.Value = int.Parse(UserID);
                cmd.Parameters.Add(pUserID);

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

        public DataSet GetAllGroupsForUserID(string UserID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllGroupsForUserID", con);
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

        public bool AddNewOffice(string CreatedByID, string ParentOfficeID, string OfficeTypeID, string OfficeName, string Address, string Phone1, string Phone2, string Fax, string EmailID)
        {
            bool OpResult = false;

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_AddNewOffice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pCreatedByID = new SqlParameter("@CreatedByID", SqlDbType.Int);
                pCreatedByID.Value = int.Parse(CreatedByID);
                cmd.Parameters.Add(pCreatedByID);

                SqlParameter pParentOfficeID = new SqlParameter("@ParentOfficeID", SqlDbType.Int);
                pParentOfficeID.Value = int.Parse(ParentOfficeID);
                cmd.Parameters.Add(pParentOfficeID);

                SqlParameter pOfficeTypeID = new SqlParameter("@OfficeTypeID", SqlDbType.Int);
                pOfficeTypeID.Value = int.Parse(OfficeTypeID);
                cmd.Parameters.Add(pOfficeTypeID);

                SqlParameter pOfficeName = new SqlParameter("@OfficeName", SqlDbType.VarChar);
                pOfficeName.Value = OfficeName;
                cmd.Parameters.Add(pOfficeName);

                SqlParameter pAddress = new SqlParameter("@Address", SqlDbType.VarChar);
                pAddress.Value = Address;
                cmd.Parameters.Add(pAddress);

                SqlParameter pPhone1 = new SqlParameter("@Phone1", SqlDbType.VarChar);
                pPhone1.Value = Phone1;
                cmd.Parameters.Add(pPhone1);

                SqlParameter pPhone2 = new SqlParameter("@Phone2", SqlDbType.VarChar);
                pPhone2.Value = Phone2;
                cmd.Parameters.Add(pPhone2);

                SqlParameter pFax = new SqlParameter("@Fax", SqlDbType.VarChar);
                pFax.Value = Fax;
                cmd.Parameters.Add(pFax);

                SqlParameter pEmailID = new SqlParameter("@EmailID", SqlDbType.VarChar);
                pEmailID.Value = EmailID;
                cmd.Parameters.Add(pEmailID);

                con.Open();
                if (int.Parse(cmd.ExecuteScalar().ToString()) == 1)
                {
                    OpResult = true;
                }

            }
            catch (Exception ex) { }
            finally
            {
                con.Close();

            }

            return OpResult;
        }

        public string AddNewCenter(string VillageID, string MeetingLocation, string StaffID, string CreatedByID, string MeetingTime)
        {
            string OpResult = CustEnum.Generic.Error_Default.ToString();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_AddNewCenter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@VillageID", int.Parse(VillageID));
                cmd.Parameters.AddWithValue("@MeetingLocation", MeetingLocation);
                cmd.Parameters.AddWithValue("@MeetingTime", MeetingTime);
                cmd.Parameters.AddWithValue("@StaffID", int.Parse(StaffID));
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

        public DataSet GetAllFE()
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllFE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                                
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

        public DataSet GetVillageStatistics(string BranchID)
        {
            DataSet ds = new DataSet();

            Connection obj = new Connection();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetVillageStatistics", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (BranchID != null)
                {
                    cmd.Parameters.AddWithValue("@BranchID", int.Parse(BranchID));
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