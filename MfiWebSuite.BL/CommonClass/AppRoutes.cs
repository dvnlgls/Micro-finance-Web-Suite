using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MfiWebSuite.BL.CommonClass
{
    public class AppRoutes
    {
        //if you use parameters, use Page.GetRouteUrl to fetch the url

        public class Login
        {
            public static string Name = "Login";
            public static string URL = "login";
            public static string PhysicalFile = "~/login.aspx";
        }

        public class Home
        {
            public static string Name = "Home";
            public static string URL = "home";
            public static string PhysicalFile = "~/default.aspx";
        }

        public class Dev
        {
            public static string Name = "Dev";
            public static string URL = "dev";
            public static string PhysicalFile = "~/DevCenter/dev.aspx";
        }

        public class Office
        {
            public static string Name = "Office";
            public static string URL = "office";
            public static string PhysicalFile = "~/UsrForms/office.aspx";
            
        }

        public class HeadOffice
        {
            public static string Name = "HeadOffice";
            public static string URL = "head-office";
            public static string PhysicalFile = "~/UsrForms/headoffice.aspx";
        }

        public class RegionalOffice
        {
            public static string Name = "RegionalOffice";
            public static string ValOfficeID = "OfficeID";

            public static string URL = "{" + ValOfficeID + "}/regional-office";
            public static string PhysicalFile = "~/UsrForms/regionaloffice.aspx";
            
        }

        public class AreaOffice
        {
            public static string Name = "AreaOffice";
            public static string ValOfficeID = "OfficeID";

            public static string URL = "{" + ValOfficeID + "}/area-office";
            public static string PhysicalFile = "~/UsrForms/areaoffice.aspx";

        }

        public class BranchOffice
        {
            public static string Name = "BranchOffice";
            public static string ValOfficeID = "OfficeID";

            public static string URL = "{" + ValOfficeID + "}/branch-office";
            public static string PhysicalFile = "~/UsrForms/branchoffice.aspx";

        }

        public class Village
        {
            public static string Name = "Village";
            public static string ValOfficeID = "OfficeID";

            public static string URL = "{" + ValOfficeID + "}/village";
            public static string PhysicalFile = "~/UsrForms/village.aspx";

        }

        public class Center
        {
            public static string Name = "Center";
            public static string ValOfficeID = "OfficeID";

            public static string URL = "{" + ValOfficeID + "}/center";
            public static string PhysicalFile = "~/UsrForms/center.aspx";

        }

        public class Groups
        {
            public static string Name = "Groups";
            public static string URL = "groups";
            public static string PhysicalFile = "~/UsrForms/groups.aspx";
        }

        
        public class ClientInfoForm
        {
            public static string Name = "ClientInfoForm";
            public static string URL = "client-info-form";
            public static string PhysicalFile = "~/UsrForms/clientinfoform.aspx";
        }

        public class LoanAppForm
        {
            public static string Name = "LoanAppForm";
            public static string URL = "loan-application-form";
            public static string PhysicalFile = "~/UsrForms/loanappform.aspx";
        }

        public class PendingLoans
        {
            public static string Name = "PendingLoans";
            public static string URL = "pending-loans";
            public static string PhysicalFile = "~/UsrForms/pendingloans.aspx";
        }

        public class DisburseLoans
        {   
            public static string Name = "DisburseLoans";
            public static string ValOfficeID = "OfficeID";
            public static string URL = "{" + ValOfficeID + "}/disburse-loans";            
            public static string PhysicalFile = "~/UsrForms/disburseloan.aspx";
        }

        public class LoanProduct
        {
            public static string Name = "LoanProduct";
            public static string URL = "loan-products";
            public static string PhysicalFile = "~/UsrForms/loanproduct.aspx";
        }

        public class LoanProdDetail
        {
            public static string Name = "LoanProdDetail";
            public static string ValOfficeID = "OfficeID";

            public static string URL = "{" + ValOfficeID + "}/loan-product-detail";
            public static string PhysicalFile = "~/UsrForms/loanproductdetail.aspx";

        }

        public class LoanRepayment
        {
            public static string Name = "LoanRepayment";
            public static string URL = "loan-repayment";
            public static string PhysicalFile = "~/UsrForms/repayment.aspx";
        }

        public class Report
        {
            public static string Name = "Report";
            public static string URL = "{ReportID}/report";
            public static string PhysicalFile = "~/UsrForms/report.aspx";
        }

        public class CCSReport
        {
            public static string Name = "CCS";
            public static string URL = "center-collection-sheet";
            public static string PhysicalFile = "~/UsrForms/centercollecreport.aspx";
        }

    }
}