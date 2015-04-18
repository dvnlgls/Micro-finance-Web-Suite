using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MfiWebSuite.BL.CommonClass
{
    public class DbSettings
    {
        public class Generic
        {
            public static string Active = "1";
            public static string InActive = "0";
        }

        public class Fixed
        {
            public static string HOID = "1";
        }

        public class UserStatus
        {
            public static string Active = "1";
        }

        public class UserRoles
        {
            public static string Dev = "1";
            public static string CEO = "2";
            public static string BranchManager = "3";
            public static string FieldExecutive = "4";
            public static string Accountant = "5";
        }

        public class ClientStatus
        {
            public static string Idle = "1";
            public static string PendingLoan = "2";
            public static string ActiveLoan = "3";

        }


        public class OfficeType
        {
            public static string HeadOffice = "1";
            public static string RegionalOffice = "2";
            public static string AreaOffice = "3";
            public static string BranchOffice = "4";
            public static string Village = "5";
        }

        public class Menus
        {
            public static string Office = "29";
            public static string HeadOffice = "36";
            public static string RegionalOffice = "37";
            public static string CIF = "6";
            public static string LAF = "8";
            public static string LoansPending = "9";
            public static string Center = "30";
            public static string LoanProduct = "31";
            public static string Groups = "7";
            public static string LoanRepayment = "10";
        }

        public class MenusAction
        {
            public static string DeleteGroup = "30";
        }

        public class AccessHO
        {
            public static string ViewHO = "1";
            public static string EditHO = "2";
            public static string ViewRO = "3";
        }

        public class AccessRO
        {
            public static string ViewRO = "4";
            public static string EditRO = "5";
            public static string ViewAO = "6";
        }

        public class AccessOffice
        {
            public static string ViewHoTab = "7";
            public static string ViewRoTab = "8";
            public static string AddRo = "9";
            public static string DeleteRo = "10";
            public static string ViewAoTab = "11";
            public static string AddAo = "12";
            public static string DeleteAo = "13";
            public static string ViewBoTab = "14";
            public static string AddBo = "15";
            public static string DeleteBo = "16";
            public static string ViewVilTab = "17";
            public static string AddVil = "18";
            public static string DeleteVil = "19";

            //corres to office details page
            public static string EditHo = "20";
            public static string EditRo = "21";
            public static string EditAo = "22";
            public static string EditBo = "23";
            public static string EditVil = "24";

        }

        public class AccessCenters
        {
            public static string ViewCenterList = "25";
            public static string ViewCenterDetails = "26";
            public static string AddCenter = "27";
            public static string DelCenter = "28";
            public static string EditCenter = "29";
        }

        public class AccessLoanProduct
        {
            public static string Add = "31";
            public static string Edit = "32";
            public static string Delete = "33";
        }

        public class LoanStatus
        {
            public static string Applied = "1";
            public static string ActiveLoan = "2";
            public static string GroupDeleted = "3";
            public static string Rejected = "4";
        }


        public class GroupStatus
        {
            public static string New = "1";
            public static string Active = "3";
        }

        public class ErrorType
        {
            public static string LafProcess = "1";
            public static string RPSError = "2";
        }

        public class CollectionType
        {
            public static string Monthly = "1";
            public static string Days = "2";

        }

        public class ReportType
        {
            public const string LoanDisbursementReport = "12";
            public const string CenterCollectionSheet = "13";
            public const string OutstandingReportBranch = "14";
            public const string OutstandingReportProduct = "15";
            public const string OutstandingReportVillage = "42";
            public const string SubLedgerBalances = "16";
            public const string OverdueReportBranch = "17";
            public const string OverdueReportFE = "18";
            public const string OverdueReportCenter = "19";
            public const string OverdueReportProduct = "20";
            public const string OverdueReportVillage = "21";
            public const string PurposeWiseLoanPortfolio = "22";
            public const string DuevsCollectionBranch = "23";
            public const string DuevsCollectionCenter = "24";
            public const string DuevsCollectionFE = "25";
            public const string FEDetailed = "26";
            public const string MonthlyBranchStatus = "27";
            public const string HOReport = "28";
        }

    }
}