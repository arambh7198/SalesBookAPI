using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesBookAPI.Custom;
using SalesBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace SalesBookAPI.Controllers
{
    public class SalesController : ApiController
    {
        #region "Sales Post"

        [ActionName("getSalesForEdit")]
        [HttpPost]

        public JObject getSalesForEdit([FromBody]JObject data)
        {
            try
            {
                JObject RtnObject = new JObject();
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;

                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("UserCode", t.UserCode);
                IncludeParam.Add("SessionID", t.SessionID);
                IncludeParam.Add("SalesCode", data["Code"].ToString());

                DataSet ds = StaticGeneral.GetDataSet("PGetSalesForEdit", null, IncludeParam);
                if (ds.Tables.Count > 1)
                {
                    SetTableNames(ds);
                    List<string> Tables = new List<string>();
                    Tables.Add("Sales");
                    return ds.toJObjectWithRelations(Tables);
                }
                else {
                    RtnObject["Data"] = ds.Tables[0].ToJArray();
                    return RtnObject;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        [ActionName("saveSales")]
        [HttpPost]

        public JObject saveSales([FromBody]JObject data)
        {
            try
            {
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;
                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                // IncludeParam.Add("UserCode", t.UserCode.ToString());
                IncludeParam.Add("SessionID", t.SessionID.ToString());

                JObject rootObj = new JObject();
                rootObj["root"] = data;

                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(rootObj.ToString());

                IncludeParam.Add("Sales", doc.InnerXml.ToString());
                DataSet ds = StaticGeneral.GetDataSet("pSaveSalesInfo", null, IncludeParam);
                SetTableNames(ds);
                List<string> Tables = new List<string>();
                Tables.Add("Sales");
                return ds.toJObjectWithRelations(Tables);
            }
            catch (Exception)
            {
                throw;
            }
        }


        void SetTableNames(DataSet ds)
        {

            ds.Tables[0].TableName = "Sales";
            ds.Tables[1].TableName = "SalesDetails";


            ds.Relations.Add(new DataRelation("SalesDetails", new DataColumn[] { ds.Tables[0].Columns["Code"] }, new DataColumn[] { ds.Tables[1].Columns["SalesCode"] }));

            ds.AcceptChanges();
        }
        #endregion
    }
}