using Newtonsoft.Json.Linq;
using SalesBookAPI.Custom;
using SalesBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SalesBookAPI.BL
{
    public class CustVend_BL
    {
        #region "CRUD Operation"
        public JObject GetData(JObject data, Token t)
        {
            JObject RtnObject = new JObject();

            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();

            List<string> ExcludeParam = new List<string>();
            ExcludeParam.Add("TotalCount");

            IncludeParam.Add("ResultType", "1");
            IncludeParam.Add("SessionID", t.SessionID);
            IncludeParam.Add("LoginCode", t.UserCode);
            DataTable dtDataCount = StaticGeneral.GetDataTable("CustVend_Select", data, ExcludeParam, IncludeParam);

            IncludeParam["ResultType"] = "2";
            DataTable dtData = StaticGeneral.GetDataTable("CustVend_Select", data, ExcludeParam, IncludeParam);

            RtnObject["Data"] = dtData.ToJArray();
            RtnObject["DataCount"] = dtDataCount.Rows[0]["totalRowsCount"].ToString();
            return RtnObject;
        }

        public JObject SaveData(JObject data, Token t)
        {
            JObject RtnObject = new JObject();

            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("UserCode", t.UserCode);
            IncludeParam.Add("SessionID", t.SessionID);
            //IncludeParam.Add("UserCode", t.LoginCode); 
            //  data["UserCode"] = t.LoginCode;
            List<string> ExcludeParam = new List<string>();
            ExcludeParam.Add("Locked");

            DataTable dtData = StaticGeneral.GetDataTable("CustVend_Insert", data, ExcludeParam, IncludeParam);

            RtnObject["Data"] = dtData.ToJArray();

            return RtnObject;

        }

        public JObject UpdateData(JObject data, Token t)
        {
            JObject RtnObject = new JObject();

            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("UserCode", t.UserCode);
            IncludeParam.Add("SessionID", t.SessionID);
            List<string> ExcludeParam = new List<string>();

            DataTable dtData = StaticGeneral.GetDataTable("CustVend_Update", data, ExcludeParam, IncludeParam);

            RtnObject["Data"] = dtData.ToJArray();

            return RtnObject;

        }


        public JObject DeleteData(int Code, Token t)
        {
            JObject RtnObject = new JObject();

            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("Code", Code);
            IncludeParam.Add("SessionID", t.SessionID);
            DataTable dtData = StaticGeneral.GetDataTable("CustVend_Delete", IncludeParam);

            RtnObject["Data"] = dtData.ToJArray();

            return RtnObject;

        }
        #endregion
    }
}