using SalesBookAPI.Custom;
using SalesBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SalesBookAPI.BL
{
    public class Login_BL
    {

        public DataTable ValidateUser(string UserName, string Password)
        {
            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password))
            {
                throw new Exception();
            }
            Dictionary<string, object> IncludeSelectParam = new Dictionary<string, object>();
            IncludeSelectParam.Add("UserName", UserName);
            IncludeSelectParam.Add("Password", StaticGeneral.EncryptedText(Password));
            IncludeSelectParam.Add("PCIP", GetIp());
            IncludeSelectParam.Add("BrowserInfo", GetHttpHeaders());
            DataTable dtData = StaticGeneral.GetDataTable("AuthenticateUser", null, null, IncludeSelectParam);
            if (dtData != null && dtData.Rows.Count > 0)
            {
                Token t = new Token();
                DataRow dr = dtData.Rows[0];
                t.UserCode = dr["UserCode"] != null ? dr["UserCode"].ToString() : "";
                t.UserName = dr["UserName"] != null ? dr["UserName"].ToString() : "";
                t.UserPassword = Password;
                t.SessionID = dr["SessionCode"] != null ? dr["SessionCode"].ToString() : "";
                string strEncToken = t.GetTokenSerialized();
                dr["Token"] = strEncToken;
            }
            else
            {
                //throw new Exception();
            }
            return dtData;
        }

        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public string GetHttpHeaders()
        {
            string ip = "";
            ip = System.Web.HttpContext.Current.Request.ServerVariables["ALL_HTTP"];
            return ip;
        }
    }
}