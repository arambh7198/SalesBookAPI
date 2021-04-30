using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesBookAPI.BL;
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
        Sales_BL bl = new Sales_BL();
        [ActionName("getData")]
        [HttpPost]
        public JObject GetData([FromBody]JObject data)
        {
            try
            {
                JObject RtnObject = new JObject();
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;

                if (data != null)
                {
                    Dictionary<string, object> IncludeParam = new Dictionary<string, object>();

                    List<string> ExcludeParam = new List<string>();
                    ExcludeParam.Add("TotalCount");

                    IncludeParam.Add("ResultType", "1");
                    IncludeParam.Add("SessionID", t.SessionID);
                    IncludeParam.Add("LoginCode", t.UserCode);
                    DataTable dtDataCount = StaticGeneral.GetDataTable("Sales_Select", data, ExcludeParam, IncludeParam);

                    IncludeParam["ResultType"] = "2";
                    DataTable dtData = StaticGeneral.GetDataTable("Sales_Select", data, ExcludeParam, IncludeParam);

                    RtnObject["Data"] = dtData.ToJArray();
                    RtnObject["DataCount"] = dtDataCount.Rows[0]["totalRowsCount"].ToString();
                    return RtnObject;
                }

                return RtnObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                else
                {
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


        [ActionName("deleteData")]
        [HttpDelete]
        public JObject DeleteData(int Code = -1)
        {
            try
            {
                JObject RtnObject = new JObject();
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;

                if (Code != -1)
                {
                    Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                    IncludeParam.Add("Code", Code);
                    IncludeParam.Add("SessionID", t.SessionID);
                    DataTable dtData = StaticGeneral.GetDataTable("Sales_Delete", IncludeParam);

                    RtnObject["Data"] = dtData.ToJArray();

                    return RtnObject;
                }
                return RtnObject;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region "Sales Invoice Report"
        [ActionName("GetInvoiceReport")]
        [HttpPost]
        public JObject GetInvoiceReport([FromBody]JObject data)
        {
            try
            {
                JObject RtnObject = new JObject();
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;
                string FilePath = "";
                if (data != null)
                {
                    if (data["Format"].ToString() == "PDF")
                    {
                        FilePath = bl.getUserReportPDF(data, t);
                    }
                    else
                    {
                        FilePath = bl.getUserReportExcel(data, t);
                    }
                }

                if (FilePath != null && FilePath.ToString() != "")
                {
                    RtnObject["FilePath"] = FilePath;
                }
                return RtnObject;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}