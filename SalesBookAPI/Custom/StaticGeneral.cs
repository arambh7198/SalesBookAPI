
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace SalesBookAPI.Custom
{
    public class StaticGeneral
    {
        public static string GetDBConnectionString()
        {

            return GetDBConnectionString("Connection"); 
        }
      

        public static DataSet GetDataSet(SqlCommand scmd)
        {
            if (scmd.Connection == null)
            {
                scmd.Connection = new SqlConnection(SiteConfig.ConnectionString);
            }
            SqlDataAdapter sda = new SqlDataAdapter(scmd);
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;
        }

        public static DataTable GetDataTable(SqlCommand scmd, int TimeOut = -1)
        {
            if (scmd.Connection == null)
            {
                scmd.Connection = new SqlConnection(SiteConfig.ConnectionString);
            }
            if (TimeOut != -1)
            {
                scmd.CommandTimeout = TimeOut;
            }
            SqlDataAdapter sda = new SqlDataAdapter(scmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

      

        public static DataTable GetDataTable(string SPorQuery, Dictionary<string, object> Parameters, bool isSP = true, int TimeOut = -1)
        {
            SqlCommand sCmd = new SqlCommand(SPorQuery);
            if (isSP)
            {
                sCmd.CommandType = CommandType.StoredProcedure;
            }

            if (TimeOut != -1)
            {
                sCmd.CommandTimeout = TimeOut;
            }
            if (Parameters != null)
            {
                foreach (KeyValuePair<string, object> Para in Parameters)
                {
                    sCmd.Parameters.AddWithValue(Para.Key, (Para.Value == null ? DBNull.Value : Para.Value));
                }
            }
            return GetDataTable(sCmd);
        }


        

        public static DataSet GetDataSet(string SPorQuery, Dictionary<string, object> Parameters, bool isSP = true, int TimeOut = -1)
        {
            SqlCommand sCmd = new SqlCommand(SPorQuery);
            if (isSP)
            {
                sCmd.CommandType = CommandType.StoredProcedure;
            }

            if (TimeOut != -1)
            {
                sCmd.CommandTimeout = TimeOut;
            }
            foreach (KeyValuePair<string, object> Para in Parameters)
            {
                sCmd.Parameters.AddWithValue(Para.Key, (Para.Value == null ? DBNull.Value : Para.Value));
            }
            return GetDataSet(sCmd);
        }

        public static DataTable GetDataTable(string strSQL)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, sCon);
            DataTable dtRtn = new DataTable();
            sda.Fill(dtRtn);
            return dtRtn;
        }

        public static DataTable GetDataTable(string strSQL, JObject data, List<string> ExcludeParams = null, Dictionary<string, object> IncludeParams = null)
        {
            List<SqlParameter> sp = GetParameterList(data, ExcludeParams,IncludeParams);

            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlCommand cmd = new SqlCommand(strSQL, sCon);

            //foreach(SqlParameter sq in sp)
            //{
            //    cmd.Parameters.Add(sq);
            //}
            cmd.Parameters.AddRange(sp.ToArray());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dtRtn = new DataTable();
            sda.Fill(dtRtn);
            return dtRtn;
        }

        public static DataTable GetAGOfficeDataTable(string strSQL, JObject data, List<string> ExcludeParams = null, Dictionary<string, object> IncludeParams = null)
        {
            List<SqlParameter> sp = GetParameterList(data, ExcludeParams, IncludeParams);

            SqlConnection sCon = new SqlConnection(SiteConfig.AkashgangaOfficeDBConnectionString);
            SqlCommand cmd = new SqlCommand(strSQL, sCon);

            //foreach(SqlParameter sq in sp)
            //{
            //    cmd.Parameters.Add(sq);
            //}
            cmd.Parameters.AddRange(sp.ToArray());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dtRtn = new DataTable();
            sda.Fill(dtRtn);
            return dtRtn;
        }


        public static DataSet GetDataSet(string strSQL)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, sCon);
            DataSet dsRtn = new DataSet();
            sda.Fill(dsRtn);
            return dsRtn;
        }

        public static int ExecuteNonQuery(string strSQL)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            sCon.Open();
            SqlCommand sCmd = new SqlCommand(strSQL, sCon);

            int iRtn = sCmd.ExecuteNonQuery();
            sCon.Close();
            return iRtn;
        }
        public static int ExecuteSQLCommand(SqlCommand sqlCmd)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            sCon.Open();
            sqlCmd.Connection = sCon;
            int iRtn = sqlCmd.ExecuteNonQuery();
            sCon.Close();
            return iRtn;
        }

        public static List<SqlParameter> GetParameterList(JObject data, List<string> ExcludeParams = null,Dictionary<string,object> IncludeParams = null)
        {
            List<SqlParameter> Rtn = new List<SqlParameter>();
            if (ExcludeParams == null)
            {
                ExcludeParams = new List<string>();
            }

            if (IncludeParams != null)
            {
                foreach (var obj in IncludeParams)
                {
                    if (obj.Value.ToString() == "null" || obj.Value.ToString() == "undefined" || obj.Value.ToString() == "''" || obj.Value.ToString() == "" || obj.Value.ToString() == "{}")
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), DBNull.Value);
                        Rtn.Add(dbPara1);
                    }
                    else
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), obj.Value.ToString());
                        Rtn.Add(dbPara1);
                    }
                }
            }

            if (data != null)
            {
                foreach (var obj in data)
                {
                    if (ExcludeParams.Where(x => x.ToLower() == obj.Key.ToLower()).Count() > 0)
                    {
                        continue;
                    }

                    if (obj.Value.ToString() == "null" || obj.Value.ToString() == "undefined" || obj.Value.ToString() == "''" || obj.Value.ToString() == "" || obj.Value.ToString() == "{}")
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), DBNull.Value);
                        Rtn.Add(dbPara1);
                    }
                    else
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), obj.Value.ToString());
                        Rtn.Add(dbPara1);
                    }
                }
            }
            return Rtn;
        }



        static string GetDBConnectionString(string strWhichConnection)
        {
            try
            {
                return WebConfigurationManager.ConnectionStrings[strWhichConnection].ConnectionString.Trim();
            }
            catch
            {
                throw new Exception("Could not find [" + strWhichConnection + "] in <connectionStrings> section of your web.config file." + Environment.NewLine + "Please check your web.config file.");
            }

        }
        static string GetAppSettingValue(string strKey)
        {

            try
            {
                return WebConfigurationManager.AppSettings[strKey].Trim();
            }
            catch
            {
                throw new Exception("[" + strKey + "] not defined in AppSettings of web.config file." + Environment.NewLine + "Please check web.config.");
            }
            throw new Exception("No App.Settings defined in web.config." + Environment.NewLine + "Please check web.config.");
        }
        public static bool SQLCompatibleText(string strText)
        {
            bool InvalidText = false;
            InvalidText = !(strText.IndexOf("'") >= 0);
            if (InvalidText) InvalidText = !(strText.IndexOf("--") >= 0);
            if (InvalidText) InvalidText = !(strText.IndexOf("/*") >= 0);
            return InvalidText;
        }
        public static bool SQLCompatibleText(string strText, bool DisAllowSpace)
        {
            if (DisAllowSpace)
            {
                //return !(strText.IndexOf(" ") >= 0);
                bool rtn = !(strText.IndexOf(" ") >= 0);
                if (!rtn) return rtn;
            }
            return SQLCompatibleText(strText);
        }
        public static bool IsSQLInjectionSafe(string strInString)
        {
            if (strInString.Trim().Length == 0 ||
               strInString.Trim().ToLower().IndexOf(" ") >= 0 ||
               strInString.Trim().ToLower().IndexOf("'") >= 0 ||
               strInString.Trim().ToLower().IndexOf("script") >= 0 ||
               strInString.Trim().ToLower().IndexOf("<") > 0)
            {
                return false;
            }
            return true;
        }

        public static bool DisAllowKeys(string strCheckIn, string strDisallow)
        {
            char[] chrSplit = strDisallow.ToCharArray();
            foreach (char item in chrSplit)
            {
                if (strCheckIn.ToLower().IndexOf(item.ToString().ToLower()) >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNumeric(string strData)
        {
            decimal dcl = 0;
            try
            {
                dcl = Convert.ToDecimal(strData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static decimal ConvertToNumeric(string strData)
        {
            decimal dcl = 0;
            try
            {
                dcl = Convert.ToDecimal(strData);
                return dcl;
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ConvertToNumeric(string strData, string strSpecificStringToRemove)
        {
            decimal dcl = 0;
            try
            {
                dcl = Convert.ToDecimal(strData.Replace(strSpecificStringToRemove, ""));
                return dcl;
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ConvertToNumeric(string strData, bool RemoveAllStrings)
        {
            decimal dcl = 0;
            string strNumericValue = "";
            try
            {
                for (int i = 0; i <= strData.Length - 1; i++)
                {
                    if (IsNumeric(strData.Substring(i, 1)))
                    {
                        strNumericValue += strData.Substring(i, 1);
                    }
                }
                dcl = ConvertToNumeric(strNumericValue);
                return dcl;
            }
            catch
            {
                return 0;
            }
        }
        public static int GetCharCountInString(string strText, string strCharToCount)
        {
            int CharCount = 0;
            for (int i = 0; i <= strText.Length - 1; i++)
            {
                if (strText.Substring(i, 1).ToLower().Trim().Equals(strCharToCount.ToLower().Trim()))
                {
                    CharCount = CharCount + 1;
                }
            }
            return CharCount;
        }
        public static string GetWebSiteName()
        {
            return ConfigurationManager.AppSettings["SiteName"].Trim();
        }

        public static string EncryptedText(string strToEncrypt)
        {
            Encrypto enc = new Encrypto();
            return enc.Encrypt(strToEncrypt, true);
        }

        public static string DecryptedText(string strToDecrypt)
        {
            Encrypto enc = new Encrypto();
            return enc.Decrypt(strToDecrypt,true);
        }

        public static object GetParameterValue(JObject objToFind, string keyName)
        {
            
            if(objToFind[keyName] == null )
            {
                return DBNull.Value;
            }

            JToken obj = objToFind[keyName] as JToken;
            
                if (obj.ToString() == "" || obj.ToString() == "{}")
                {
                 return DBNull.Value;
                }
                else
                {
                    
                    return obj.ToObject<object>();                    
                }
        }

        public static string CheckNull(object obj, bool EncloseQuotes, bool ReturnNull)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(Convert.ToString(obj)))
            {
                return ReturnNull ? "null" : (EncloseQuotes ? "''" : "");
            }

            if (EncloseQuotes)
            {
                return Convert.ToString("'" + obj.ToString() + "'");
            }
            return obj.ToString();
        }

        public static void LogRequests(string DeviceCode, string RequestBody, string RequestHeaders,string Method,string RequestURI)
        {
            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("@DeviceCode", DeviceCode.ToString());
            //IncludeParam.Add("@SectionName", SectionName.ToString());
            IncludeParam.Add("@RequestBody", RequestBody.ToString());
            IncludeParam.Add("@RequestHeaders", RequestHeaders.ToString());
            IncludeParam.Add("@Method", Method.ToString());
            IncludeParam.Add("@RequestURI", RequestURI.ToString());
            DataTable dt = StaticGeneral.GetDataTable("pRequestLog_Aasan", IncludeParam);
        }

        public static void LogException(string DeviceCode,string exMessage,string SectionName,string RequestBody, string RequestHeaders)
        {
            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("@DeviceCode", DeviceCode.ToString());
            IncludeParam.Add("@SectionName", SectionName.ToString());
            IncludeParam.Add("@ErrorMsg", exMessage.ToString());
            IncludeParam.Add("@RequestBody", RequestBody.ToString());
            IncludeParam.Add("@RequestHeaders", RequestHeaders.ToString());
            DataTable dt = StaticGeneral.GetDataTable("pErrorLog_Aasan_InsertForAasan", IncludeParam);
        }

        public static string PGetMilkCollectionSMSMssage(string strCattle,string strLiters,string strFat,string strSNF,
            string strAmount,string strCollectionDate,string strMemCode,string strSocietyName,string strCollectionTime,string strSession)
        {
            StringBuilder sbRtn = new StringBuilder();
            sbRtn.AppendLine("    " + strSocietyName + "    ");
            sbRtn.AppendLine("Mem:" + strMemCode);
            sbRtn.AppendLine(Convert.ToDateTime(strCollectionDate).ToString("dd/MM/yyyy"));
            sbRtn.AppendLine(strCollectionTime + " / " + (strSession == "M" ? "Mor" : "Eve" ));
            sbRtn.AppendLine(strCattle == "B" ? "Buff" : "Cow" ) ;
            sbRtn.AppendLine("L = " + strLiters);
            sbRtn.AppendLine("F = " + strFat + (strSNF != ""  ? " S = " + strSNF : ""));
            sbRtn.AppendLine("Amt :" + strAmount);
            sbRtn.AppendLine("AG - AASAN");
            return sbRtn.ToString();
        }


        public static string PGetSessionReportSMSMssage(string strSocietyName,
            string strBuffMembers,string strBuffLiters, string strBuffAvgFat,string strBuffAvgSNF,string strBuffTotalAmount,
            string strCowMembers, string strCowLiters, string strCowAvgFat, string strCowAvgSNF, string strCowTotalAmount)
        {
            StringBuilder sbRtn = new StringBuilder();
            sbRtn.AppendLine("    " + strSocietyName + "    ");
            sbRtn.AppendLine("Buff Milk");
            sbRtn.AppendLine("Members : " + strBuffMembers);
            sbRtn.AppendLine("Av Fat = " + strBuffAvgFat + " Av SNF = " + strBuffAvgSNF);
            sbRtn.AppendLine("Tot Amt = " + strBuffTotalAmount);
            sbRtn.AppendLine("Cow Milk");
            sbRtn.AppendLine("Members : " + strCowMembers);
            sbRtn.AppendLine("Av Fat = " + strCowAvgFat + " Av SNF = " + strCowAvgSNF);
            sbRtn.AppendLine("Tot Amt = " + strCowTotalAmount);
            sbRtn.AppendLine("AG - AASAN");
            return sbRtn.ToString();
        }

        public static void SendMessage(string strDeviceCode,string strMemberCode,string strSendTo,string strMessage) {

            using (var wb = new WebClient())
            {
                //var data = new NameValueCollection();
                //data["feedid"] = SiteConfig.SMSFeedId;
                //data["senderid"] = SiteConfig.SMSSenderID;
                //data["username"] = SiteConfig.SMSUserName;
                //data["password"] = SiteConfig.SMSPassword;

                //data["To"] = Convert.ToString("8652331355");
                //data["Text"] = Convert.ToString("Test message");
                ////var response = wb.UploadValues(SiteConfig.SMSURL, "POST", data);
                //  var json = wb.DownloadString(SiteConfig.SMSURL + "&to=" + strSendTo + "&sender=SKEPLI&message= " + strMessage);
                string TempstrSendTo = strSendTo.Length == 10 ? "91" + strSendTo : strSendTo;
                //string strRequest = SiteConfig.SMSURL + "&contacts=" + TempstrSendTo + "&senderid=BV-AGASAN&msg=" + HttpUtility.UrlEncode(strMessage);
                string strRequest = SiteConfig.SMSURL + "&contacts=" + TempstrSendTo + "&senderid=AGASAN&msg=" + HttpUtility.UrlEncode(strMessage);
                var json = wb.DownloadString(strRequest);
                string[] ErrorCodesPrefix = { "1001", "1002", "1003", "1004", "1005", "1006", "1007", "1008", "1009", "1010" };


                //JObject JNew = JObject.Parse(json);
                //int Failed = 1;
                //try
                //{
                //    Failed = JNew["status"].ToString().ToLower() == "ok" ? 0 : 1;
                //}
                //catch (Exception)
                //{

                //}
                int Failed = 1;
                try
                {
                  
                    if (ErrorCodesPrefix.Any(x => json.StartsWith(x)))
                    {
                        Failed = 1;
                    }
                    else
                    {
                        Failed = 0;
                    }
                }
                catch (Exception ex)
                {
                    StaticGeneral.LogException("-1", ex.Message, "SendMessage", strRequest, json != null ? json.ToString() : "");
                }
               

                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("@DeviceCode", strDeviceCode);
                IncludeParam.Add("@MemberCode", strMemberCode);
                IncludeParam.Add("@MobileNumber", strSendTo);
                IncludeParam.Add("@Message", strMessage);
                IncludeParam.Add("@Failed", Failed);
                IncludeParam.Add("@RtnJSON", json != null ?  json.ToString() : "");

                DataTable dt = StaticGeneral.GetDataTable("PSmsLog_Insert_Aasan", IncludeParam);

                

            }
        }
         
        #region "Excel Mechanism"
        public static string getExcelConnectionString(string strFilePath, string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension == ".txt" || strExtension == ".csv")
            {
                if (Environment.Is64BitOperatingSystem) //32bt mein yeh nahi hai
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath.Trim() + ";Extended Properties=\"text;HDR=No;FMT=Delimited\"";
                }
                return "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" + strFilePath.Trim() + ";Extended Properties=\"text;HDR=No;FMT=Delimited\"";
            }
            else if (strExtension == ".xlsx")
            {
                return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath.Trim() + ";Extended Properties=\"Excel 12.0 Xml;HDR=No\"";
            }
            else
            {
                if (Environment.Is64BitOperatingSystem) //32bt mein yeh nahi hai
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;" +
                        "Data Source=" + strFilePath + ";" +
                        "Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\"";
                }
                return "Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Data Source=" + strFilePath + ";" +
                        "Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\"";
            }
        }
        public static OleDbConnection getExcelConnection(string strExcelConnectionString)
        {
            return new OleDbConnection(strExcelConnectionString);
        }
        public static DataTable getExcelDataTable(string strQuery, string strExcelConnectionString)
        {
            return getExcelDataTable(strQuery, getExcelConnection(strExcelConnectionString));
        }
        public static DataTable getExcelDataTable(string strQuery, OleDbConnection oleConn)
        {
            DataTable dt = new DataTable();
            OleDbCommand Ocmd = new OleDbCommand(strQuery, oleConn);
            OleDbDataAdapter Oda = new OleDbDataAdapter(Ocmd);
            Oda.Fill(dt);
            return dt;
        }
        #endregion

        public static string CheckNull(JToken obj, string ValueIfNull = "NULL")
        {
            object o = obj.ToObject<object>();

            if (obj == null || obj.ToString() == "" || obj.ToString() == "{}")
            {
                return ValueIfNull;
            }
            
            if (o is DateTime)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(o.ToString(), out dt))
                {
                    return Convert.ToDateTime(o.ToString()).ToString("MM/dd/yyyy hh:mm:ss.fff") ;
                }
            }
            if (o is bool)
            {
                return (bool)o ? "1" : "0";
            }
            return  Convert.ToString(o) ;
        }


        public static string StringCheckNull(JToken obj, string ValueIfNull = "NULL")
        {
            object o = obj.ToObject<object>();

            if (obj == null || obj.ToString() == "" || obj.ToString() == "{}")
            {
                return ValueIfNull;
            }

            if (o is DateTime)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(o.ToString(), out dt))
                {
                    return "'" + Convert.ToDateTime(o.ToString()).ToString("MM/dd/yyyy hh:mm:ss.fff") + "'" ;
                }
            }
            if (o is bool)
            {
                return (bool)o ? "1" : "0";
            }
            return "'" + Convert.ToString(o) + "'";
        }

    }
}