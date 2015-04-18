using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MfiWebSuite.BL.CommonClass
{
    public class AppSettings
    {
        public class Session
        {
            public static string UserID = "UserID";
            public static string RoleID = "RoleID";
            public static string Name = "Name";
        }

        public class QueryStr
        {
            public class SessionExpired
            {
                public static string Name = "rr";
                public static string Value = "se";
            }

            public class Group
            {
                public static string Name = "gid";
            }
        }

        public static TimeZoneInfo TimeZoneIst = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public static string Delimitter = "###~###";
    }
}