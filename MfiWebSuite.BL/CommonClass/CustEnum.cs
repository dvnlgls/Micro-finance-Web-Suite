using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MfiWebSuite.BL.CommonClass
{
    public class CustEnum
    {
        public enum Generic
        {
            Success_,
            Error_Default,
            Error_SessionExpired
        }

        public enum CreateAccount
        {
            Success_,
            Error_IncorrectParams
        }

        public enum UserAuth
        {
            Success_,
            Error_
        }

        public enum PageAccess
        {
            Error,
            Yes,
            No
        }

        public enum DeleteOffice
        {
            Error,
            Success,
            HasSubOffice
        }

        public enum LAF
        {
            Error_Default,
            Error_NoClients,
            Error_Process_,
            Success_
        }

        public enum Disbursement
        {
            Error_Default,            
            Error_Process_,
            Success_
        }

        public enum DeleteLP
        {
            Error,
            Success,
            LinkedToLoan
        }

    }
}