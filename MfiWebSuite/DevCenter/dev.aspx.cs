using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MfiWebSuite.BL.Utilities;
using MfiWebSuite.BL.CommonClass;

namespace MfiWebSuite.Dev
{
    public partial class dev : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAccCreate_Click(object sender, EventArgs e)
        {
            string Pwd = txtPwd.Value;

            int MinRandSize = 123456789;
            int MaxRandSize = 987654321;

            string SaltInput1 = string.Empty;
            string SaltInput2 = string.Empty;

            string SaltHash;
            string PwdHash;
            Random random = new Random();

            for (int ctr = 0; ctr < 100; ctr++)
            {
                SaltInput1 += random.Next(MinRandSize, MaxRandSize).ToString();
            }

            for (int ctr = 0; ctr < 100; ctr++)
            {
                SaltInput2 += random.Next(MinRandSize, MaxRandSize).ToString();
            }

            CryptEngine obj = new CryptEngine();

            SaltHash = obj.ComputeHash(SaltInput1, SaltInput2);

            PwdHash = obj.ComputeHash(Pwd, SaltHash);

            spnSalt.InnerText = SaltHash;
            spnPwd.InnerText = PwdHash;

        }
    }
}