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

        public string UserPassword
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


        public Token()
        {

        }

        public Token(string SerializeString)
        {
            SerializeString = StaticGeneral.DecryptedText(SerializeString);
            string[] str = SerializeString.Split(new string[] { Separator }, StringSplitOptions.None);

            UserCode = str[0];
            UserName = str[1];
            UserPassword = str[2];
            ExpiryDate = str[3];
            SessionID = str[4];

        }
        #endregion

        #region "Helpers"
        public string GetTokenSerialized()
        {
            return
                StaticGeneral.EncryptedText(UserCode + Separator + UserName + Separator + UserPassword + Separator + ExpiryDate + Separator + SessionID);
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
                Para.Add("UserCode", UserCode);
                Para.Add("UserPassword", StaticGeneral.EncryptedText(UserPassword));
                Para.Add("SessionID", SessionID);
                DataTable dt = StaticGeneral.GetDataTable("wsp_AuthenticateUserToken", Para);
                if (dt.Rows.Count == 1 && Convert.ToBoolean(dt.Rows[0]["IsValid"]))
                {
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
                strErr = "Invalid Credentials";
                return false;
            }
        }
    }
}