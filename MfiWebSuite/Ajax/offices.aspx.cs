using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.CommonClass;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.MfiClass;
using MfiWebSuite.BL.UserClass;
using System.Data;
using System.Text;
using System.Web.Services;


namespace MfiWebSuite.Ajax
{
    public partial class offices : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["AjaxMethod"] == "SaveOffDetails")
            {
                Sessions objSess = new Sessions();

                string UserID = objSess.GetUserID();

                string OpResult = CustEnum.Generic.Error_SessionExpired.ToString();

                if (UserID != null)
                {
                    string OfficeID = Request.Form["OID"].Trim().SafeSqlLiteral(1);
                    string Name = Request.Form["Name"].Trim().SafeSqlLiteral(1);
                    string Addr = Request.Form["Addr"].Trim().SafeSqlLiteral(1);
                    string Ph1 = Request.Form["Ph1"].Trim().SafeSqlLiteral(1);
                    string Ph2 = Request.Form["Ph2"].Trim().SafeSqlLiteral(1);
                    string Fax = Request.Form["Fax"].Trim().SafeSqlLiteral(1);
                    string Web = Request.Form["Web"].Trim().SafeSqlLiteral(1);
                    string Email = Request.Form["Email"].Trim().SafeSqlLiteral(1);

                    OpResult = CustEnum.Generic.Error_Default.ToString();

                    Office obj = new Office();

                    if (obj.UpdateOfficeDetails(OfficeID, UserID, DbSettings.Generic.Active, Name, Addr, Ph1, Ph2, Fax, Web, Email))
                    {
                        OpResult = CustEnum.Generic.Success_.ToString();
                    }
                }

                Response.Write(OpResult);
                Response.End();
            } //save ho            



            if (Request.Form["AjaxMethod"] == "FetchParent")
            {

                string OfficeTypeID = Request.Form["OTID"];

                string OpResult = CustEnum.Generic.Error_Default.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                string UserRoleID = objUsrDet.RoleID;
                string UserOfficeID = objUsrDet.OfficeID;
                string UserOfficeTypeID = objUsrDet.OfficeTypeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    if (int.Parse(UserOfficeTypeID) < int.Parse(OfficeTypeID))
                    {
                        Office objOff = new Office();
                        DataSet ds = objOff.GetImmediateParentOfficesForOfficeType(UserOfficeID, OfficeTypeID);

                        if (!DataUtils.IsDataSetNull(ds, 0))
                        {
                            StringBuilder sbrMenu = new StringBuilder();
                            sbrMenu.Append("<select id='drpParent'>");
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string ParentOfficeID = dr["ParentOfficeID"].ToString();
                                string ParentOfficeName = dr["OfficeName"].ToString();

                                sbrMenu.Append("<option id=drpOptParent_" + ParentOfficeID + ">" + ParentOfficeName + "</option>");
                            }
                            sbrMenu.Append("</select>");
                            OpResult = sbrMenu.ToString();
                        }
                    }
                }
                else
                {
                    OpResult = CustEnum.Generic.Error_SessionExpired.ToString();
                }

                Response.Write(OpResult);
                Response.End();
            } //fetch parent



            if (Request.Form["AjaxMethod"] == "AddOffice")
            {
                string ParentOfficeID = Request.Form["PID"].Trim().SafeSqlLiteral(1);
                string OfficeTypeID = Request.Form["OTID"].Trim().SafeSqlLiteral(1);
                string Name = Request.Form["Name"].Trim().SafeSqlLiteral(1);
                string Addr = Request.Form["Addr"].Trim().SafeSqlLiteral(1);
                string Ph1 = Request.Form["Ph1"].Trim().SafeSqlLiteral(1);
                string Ph2 = Request.Form["Ph2"].Trim().SafeSqlLiteral(1);
                string Fax = Request.Form["Fax"].Trim().SafeSqlLiteral(1);
                string Email = Request.Form["Email"].Trim().SafeSqlLiteral(1);

                string OpResult = CustEnum.Generic.Error_Default.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                //string UserRoleID = objUsrDet.RoleID;
                string UserOfficeID = objUsrDet.OfficeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Office objOff = new Office();

                    bool OfficeCreated = objOff.AddNewOffice(UserID, ParentOfficeID, OfficeTypeID, Name, Addr, Ph1, Ph2, Fax, Email);

                    if (OfficeCreated)
                        OpResult = CustEnum.Generic.Success_.ToString();

                }
                else
                {
                    OpResult = CustEnum.Generic.Error_SessionExpired.ToString();
                }

                Response.Write(OpResult);
                Response.End();
            } // add



            if (Request.Form["AjaxMethod"] == "DeleteOffice")
            {
                string OfficeID = Request.Form["OID"];

                string OpResult = CustEnum.DeleteOffice.Error.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                //string UserRoleID = objUsrDet.RoleID;
                string UserOfficeID = objUsrDet.OfficeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Office objOff = new Office();

                    string SubOffCount = objOff.GetSubOfficeCountForOffice(OfficeID);

                    if (SubOffCount != CustEnum.Generic.Error_Default.ToString())
                    {
                        if (int.Parse(SubOffCount) == 0)
                        {
                            //delete office
                            if (objOff.DeleteOffice(OfficeID, UserID) == CustEnum.Generic.Success_.ToString())
                            {
                                OpResult = CustEnum.DeleteOffice.Success.ToString();
                            }
                        }
                        else if (int.Parse(SubOffCount) > 0)
                        {
                            OpResult = CustEnum.DeleteOffice.HasSubOffice.ToString();
                        }
                    }

                }
                else
                {
                    OpResult = CustEnum.Generic.Error_SessionExpired.ToString();
                }

                Response.Write(OpResult);
                Response.End();
            }



            if (Request.Form["AjaxMethod"] == "AddCenter")
            {
                string VillageID = Request.Form["VillageID"];
                string MeetingLocation = Request.Form["Location"];
                string MeetingTime = Request.Form["Time"];
                string FEID = Request.Form["FE"];

                string OpResult = CustEnum.Generic.Error_Default.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Office objOff = new Office();
                    int CenterID = 0;

                    int.TryParse(objOff.AddNewCenter(VillageID, MeetingLocation, FEID, UserID, MeetingTime), out CenterID);

                    if (CenterID > 0)
                    {
                        OpResult = CustEnum.Generic.Success_.ToString() + CenterID.ToString();
                    }

                }

                Response.Write(OpResult);
                Response.End();
            }

            if (Request.Form["AjaxMethod"] == "EditCenter")
            {
                string CenterID = Request.Form["CenterID"];
                string MeetingLocation = Request.Form["Location"];
                string MeetingTime = Request.Form["Time"];
                string FEID = Request.Form["FE"];

                string OpResult = CustEnum.Generic.Error_Default.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Office objOff = new Office();

                    if (objOff.UpdateCenter(CenterID, MeetingLocation, MeetingTime, FEID, UserID))
                    {
                        OpResult = CustEnum.Generic.Success_.ToString();
                    }

                }

                Response.Write(OpResult);
                Response.End();
            } //edit center

            if (Request.Form["AjaxMethod"] == "DeleteCenter")
            {
                string CenterID = Request.Form["CID"];

                string OpResult = CustEnum.DeleteOffice.Error.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                //string UserRoleID = objUsrDet.RoleID;
                string UserOfficeID = objUsrDet.OfficeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Office objOff = new Office();

                    string SubOffCount = objOff.GetGroupCountForCenterID(CenterID);

                    if (SubOffCount != CustEnum.Generic.Error_Default.ToString())
                    {
                        if (SubOffCount == "0") // string comp is guaranteed to be safe check. else have to handle null/err cases
                        {
                            //delete office
                            if (objOff.DeleteCenter(CenterID, UserID) == CustEnum.Generic.Success_.ToString())
                            {
                                OpResult = CustEnum.DeleteOffice.Success.ToString();
                            }
                        }
                        else if (int.Parse(SubOffCount) > 0)
                        {
                            OpResult = CustEnum.DeleteOffice.HasSubOffice.ToString();
                        }
                    }

                }
                else
                {
                    OpResult = CustEnum.Generic.Error_SessionExpired.ToString();
                }

                Response.Write(OpResult);
                Response.End();
            } //DEL CENT

            if (Request.Form["AjaxMethod"] == "DeleteGroup")
            {
                string GroupID = Request.Form["GID"];

                string OpResult = CustEnum.DeleteOffice.Error.ToString();

                Users objUsr = new Users();
                UserKeyDetails objUsrDet = objUsr.GetUserKeyDetails();

                string UserID = objUsrDet.UserID;
                string UserStatusID = objUsrDet.StatusID;

                //string UserRoleID = objUsrDet.RoleID;
                string UserOfficeID = objUsrDet.OfficeID;

                if (UserID != null && UserStatusID == DbSettings.UserStatus.Active)
                {
                    Office objOff = new Office();

                    string SubOffCount = objOff.DeleteGroup(GroupID, UserID, DbSettings.LoanStatus.GroupDeleted);

                    if (SubOffCount != CustEnum.Generic.Error_Default.ToString())
                    {
                        if (SubOffCount == "1") // string comp is guaranteed to be safe check. else have to handle null/err cases
                        {
                            OpResult = CustEnum.DeleteOffice.Success.ToString();
                        }
                        else if (SubOffCount == "-1")
                        {
                            OpResult = CustEnum.DeleteOffice.HasSubOffice.ToString();
                        }
                    }

                }
                else
                {
                    OpResult = CustEnum.Generic.Error_SessionExpired.ToString();
                }

                Response.Write(OpResult);
                Response.End();
            }




        }
    }
}