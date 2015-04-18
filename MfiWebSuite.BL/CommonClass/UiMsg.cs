using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MfiWebSuite.BL.CommonClass
{
    public class UiMsg
    {
        public class Global
        {
            public static string Error = "Sorry, encountered an unexpected error. Please try again.";
            public static string SessionExpired = "Your session has expired. Please login to continue.";
            public static string NoPageAccess = "You are not authorized to view this page.";
            public static string NoOfficeExists = "This office doesn't seem to exist. Please check your URL.";
            public static string InvalidPage = "This page seems to be invalid. Please check your URL.";
            public static string NoData = "Currently there is no data to fetch.";
        }             

        public class Login
        {
            public static string InvalidCredentials = "Invalid Email Id or Password. Please enter valid credentials";
            public static string MultipleEmailIds = "There is a problem with your account. Please contact system administrator";
            public static string NotAuthorized = "You are not authorized to login.";            
        }

        public class MasterDefault
        {
            public static string UserSalutation = "Hi ";
        }

        public class PageHO
        {
            public static string ViewHoFetchErr = "Unable to fetch HO details. Please refresh your page to try again.";
            public static string ViewRoFetchErr = "Unable to load RO details. Check if there are Regional Offices under this HO.";
        }

        public class PageRO
        {
            public static string ViewRoFetchErr = "Unable to fetch Regional Office. Check if there are Regional Offices under this office.";
            public static string ViewAoFetchErr = "Unable to fetch Area Office. Check if there are Area Offices under this office.";
            public static string ViewBoFetchErr = "Unable to fetch Branch Office. Check if there are Branch Offices under this office.";
            public static string ViewVilFetchErr = "Unable to fetch Villages. Check if there are Villages under this office.";
            public static string ViewCenterFetchErr = "Unable to fetch Centers. Check if there are Centers under this village.";
        }

        public class PageVillage
        {
            public static string NoCenters = "There are no centers in this Village.";
        }

        public class CIF
        {
            public static string ClientAdded = "<button class='close' data-dismiss='alert'>×</button><strong>Success!</strong> Client information form of <strong>{0}</strong> has been saved.";
            public static string AddErr = "<button class='close' data-dismiss='alert'>×</button><strong>Error.</strong> We encountered an error while saving the form. Please try again.";
        }

        public class LoansPending
        {            
            public static string NoNewLoans = "There are no new loans to show at the moment.";
        }

        public class LoansDisbursement
        {
            public static string InvalidStatus = "Please check the group status. You can disburse loans only to new or sanctioned groups.";
            public static string NoData = "Please check the group. Either it does not exist or it doesn't have any clients in it.";
        }

        public class Center
        {
            public static string NoCenterData = "Unable to fetch center details. Please try again or contact system administrator.";
            public static string NoGroups = "There are no groups for this center yet. Groups are created automatically while applying for new loan.";
        }

        public class LoanProduct
        {
            public static string NoLpData = "Unable to fetch Loan Product details. Please try again or contact system administrator.";
            public static string NoLP = "There are no Loan Products to show at the moment.";
        }
        public class Report
        {
            public static string NoData = "Either there is no data to fetch or encountered an error.";
        }
    }

    public static class Wrap
    {
        public static string ErrorWrap(this string Msg)
        {
            return "<div class='alert alert-error'>" + Msg + "</div>";
        }

        public static string SuccessWrap(this string Msg)
        {
            return "<div class='alert alert-success'>" + Msg + "</div>";
        }

        public static string InfoWrap(this string Msg)
        {
            return "<div class='alert alert-info'>" + Msg + "</div>";
        }
    }
}