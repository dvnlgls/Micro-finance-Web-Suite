using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using MfiWebSuite.BL.CommonClass;

namespace MfiWebSuite
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteTable.Routes.MapPageRoute(AppRoutes.Login.Name, AppRoutes.Login.URL, AppRoutes.Login.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.Home.Name, AppRoutes.Home.URL, AppRoutes.Home.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.Dev.Name, AppRoutes.Dev.URL, AppRoutes.Dev.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.Office.Name, AppRoutes.Office.URL, AppRoutes.Office.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.HeadOffice.Name, AppRoutes.HeadOffice.URL, AppRoutes.HeadOffice.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.RegionalOffice.Name, AppRoutes.RegionalOffice.URL, AppRoutes.RegionalOffice.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.AreaOffice.Name, AppRoutes.AreaOffice.URL, AppRoutes.AreaOffice.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.BranchOffice.Name, AppRoutes.BranchOffice.URL, AppRoutes.BranchOffice.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.Village.Name, AppRoutes.Village.URL, AppRoutes.Village.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.Center.Name, AppRoutes.Center.URL, AppRoutes.Center.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.Groups.Name, AppRoutes.Groups.URL, AppRoutes.Groups.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.ClientInfoForm.Name, AppRoutes.ClientInfoForm.URL, AppRoutes.ClientInfoForm.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.LoanAppForm.Name, AppRoutes.LoanAppForm.URL, AppRoutes.LoanAppForm.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.PendingLoans.Name, AppRoutes.PendingLoans.URL, AppRoutes.PendingLoans.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.DisburseLoans.Name, AppRoutes.DisburseLoans.URL, AppRoutes.DisburseLoans.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.LoanProduct.Name, AppRoutes.LoanProduct.URL, AppRoutes.LoanProduct.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.LoanProdDetail.Name, AppRoutes.LoanProdDetail.URL, AppRoutes.LoanProdDetail.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.LoanRepayment.Name, AppRoutes.LoanRepayment.URL, AppRoutes.LoanRepayment.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.Report.Name, AppRoutes.Report.URL, AppRoutes.Report.PhysicalFile);
            RouteTable.Routes.MapPageRoute(AppRoutes.CCSReport.Name, AppRoutes.CCSReport.URL, AppRoutes.CCSReport.PhysicalFile);

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
