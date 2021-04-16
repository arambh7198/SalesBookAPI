using SalesBookAPI.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SalesBookAPI.Models
{
    public class Token
    {
        #region "Variable,Properties,Constructor"
        public string DeviceCode = "";
        string Separator = "$#%";
        string strErr = "";

        public string UserCode
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string ExpiryDate
        {
            get;
            set;
        }
       
        public string SessionID
        {
            get;
            set;
        }

        public string LoginCode
        {
            get;
            set;
        }

        public string LocationCode
        {
            get;
            set;
        }

        public Token()
        {

        }
        
        public Token(string SerializeString)
        {
            string[] str = SerializeString.Split(new string[] { Separator }, StringSplitOptions.None);

            UserCode = str[0];
            UserName = str[1];
            Password = str[2];
            ExpiryDate = str[3];
            SessionID = str[4];
            DeviceCode = str[5];
            LoginCode = str[6];
            LocationCode = str[7];
        }
        #endregion

        #region "Helpers"
        public string GetTokenSerialized()
        {
            return UserCode + Separator +  UserName + Separator + Password + Separator + ExpiryDate + Separator + SessionID + Separator + DeviceCode + Separator + LoginCode + Separator + LocationCode;
        }

        public string GetError()
        {
            return strErr;
        }
        #endregion

        public bool IsValid()
        {
            try
            {
                Dictionary<string, object> Para = new Dictionary<string, object>();
                Para.Add("@UserCode", UserCode);
                Para.Add("@DeviceCode", DeviceCode);
                Para.Add("@LocationCode", LocationCode);
                Para.Add("@SessionID", SessionID);
                
                DataTable dt = StaticGeneral.GetDataTable("pAuthenticateTokenForAasanAPI", Para);
                if (dt.Rows.Count == 1)
                {
                    DeviceCode = Convert.ToString(dt.Rows[0]["Code"]);
                    return true;
                }
                else
                {
                    strErr = "Invalid Credentials";
                    return false;
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return false;
            }
        }
    }
}