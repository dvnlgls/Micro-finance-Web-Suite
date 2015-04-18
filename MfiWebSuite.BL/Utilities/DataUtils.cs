using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MfiWebSuite.BL.Utilities
{
    public class DataUtils
    {
        public static bool IsDataSetNull(DataSet ds, int TableIndex)
        {

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[TableIndex].Rows.Count > 0)
            {
                return false;
            }
            else
                return true;

        }

        public static string ComputeHash(string InputTxt, string SaltInput)
        {
            #region generating random salt
            // Define min and max salt sizes.
            //int minSaltSize = 4;
            //int maxSaltSize = 8;
            //byte[] saltBytes;

            //// Generate a random number for the size of the salt.
            //Random random = new Random();
            //int saltSize = random.Next(minSaltSize, maxSaltSize);

            //// Allocate a byte array, which will hold the salt.
            //saltBytes = new byte[saltSize];

            //// Initialize a random number generator.
            //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            //// Fill the salt with cryptographically strong byte values.
            //rng.GetNonZeroBytes(saltBytes); 
            #endregion

            //REMOVE THE FOLLOWING WHEN GOING LIVE. THIS IS FOR TEST ONLY!!!!
            //SaltInput = "test";

            byte[] saltBytes = Encoding.UTF8.GetBytes(SaltInput);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(InputTxt);

            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA512Managed();
            
            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;

        }

        public static bool VerifyHash(string plainText, string SaltInput, string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits = 512, hashSizeInBytes;                     

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, SaltInput);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns true if string is not alphanumeric
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsAlphaNum(this string str)
        {
            //if (string.IsNullOrEmpty(str))
            //    return false;

            return (str.ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c)));
        }

        public static string ToRupees(this string Amount)
        {
            float Value;
            float.TryParse(Amount, out Value);

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencyGroupSeparator = ",";
            int[] RupeeSeparator = { 3, 2 };
            nfi.CurrencyGroupSizes = RupeeSeparator;
            nfi.CurrencySymbol = "Rs. ";
            nfi.CurrencyDecimalDigits = 2;

            return Value.ToString("C", nfi);

        }

        public static string ToTitleCase(this string Str)
        {
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            return ti.ToTitleCase(Str);

        }
              
                
        /// <summary>
        /// removes sql injection capable elements. Level 1 - basic; 2 - advanced
        /// </summary>
        /// <param name="theValue"></param>
        /// <param name="theLevel"></param>
        /// <returns></returns>
        public static string SafeSqlLiteral(this System.Object theValue, System.Object theLevel)
        {

            // Written by user CWA, CoolWebAwards.com Forums. 2 February 2010
            // http://forum.coolwebawards.com/threads/12-Preventing-SQL-injection-attacks-using-C-NET

            // intLevel represent how thorough the value will be checked for dangerous code
            // intLevel (1) - Do just the basic. This level will already counter most of the SQL injection attacks
            // intLevel (2) -   (non breaking space) will be added to most words used in SQL queries to prevent unauthorized access to the database. Safe to be printed back into HTML code. Don't use for usernames or passwords

            string strValue = (string)theValue;
            int intLevel = (int)theLevel;

            if (strValue != null)
            {
                if (intLevel > 0)
                {
                    strValue = strValue.Replace("'", "''"); // Most important one! This line alone can prevent most injection attacks
                    strValue = strValue.Replace("--", "");
                    strValue = strValue.Replace("[", "[[]");
                    strValue = strValue.Replace("%", "[%]");
                }
                if (intLevel > 1)
                {
                    string[] myArray = new string[] { "xp_ ", "update ", "insert ", "select ", "drop ", "alter ", "create ", "rename ", "delete ", "replace " };
                    int i = 0;
                    int i2 = 0;
                    int intLenghtLeft = 0;
                    for (i = 0; i < myArray.Length; i++)
                    {
                        string strWord = myArray[i];
                        Regex rx = new Regex(strWord, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        MatchCollection matches = rx.Matches(strValue);
                        i2 = 0;
                        foreach (Match match in matches)
                        {
                            GroupCollection groups = match.Groups;
                            intLenghtLeft = groups[0].Index + myArray[i].Length + i2;
                            strValue = strValue.Substring(0, intLenghtLeft - 1) + "&nbsp;" + strValue.Substring(strValue.Length - (strValue.Length - intLenghtLeft), strValue.Length - intLenghtLeft);
                            i2 += 5;
                        }
                    }
                }

            }

            return strValue;

        }

        public static String GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// returns date in this format: 12-Jun-87. Date separator can be specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="IncludeTime"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static string ToDateShortMonth(this DateTime date, bool IncludeTime, char Separator)
        {
            string Day = date.ToString("dd");
            string Month = date.ToString("MMM");
            string Year = date.ToString("yy");
            string Hrs = date.ToString("hh");
            string Min = date.ToString("mm");
            string Meridian = date.ToString("tt");

            string ReturnDate = "";

            if (IncludeTime)
            {
                ReturnDate = Day + Separator + Month + Separator + Year + " " + Hrs + ":" + Min + " " + Meridian;
            }
            else
            {
                ReturnDate = Day + Separator + Month + Separator + Year;
            }

            return ReturnDate;
        }

        public static string ToYearMonth(this DateTime date, char Separator)
        {
            string Month = date.ToString("MMM");
            string Year = date.ToString("yy");

            string ReturnDate = "";

            ReturnDate = Year + Separator + Month;

            return ReturnDate;
        }


    }
}