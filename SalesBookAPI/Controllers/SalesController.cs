using Newtonsoft.Json.Linq;
using SalesBookAPI.Custom;
using SalesBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesBookAPI.Controllers
{
    public class SalesController : ApiController
    {
        #region "Sales Post"
        [ActionName("saveSales")]
        [HttpPost]

        public JArray saveSales([FromBody]JObject data)
        {
            try
            {
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;
                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("UserCode", t.UserCode.ToString());
                IncludeParam.Add("SessionID", t.SessionID.ToString());
                return StaticGeneral.GetDataTable("Sales_InsertUpdate", data, null, IncludeParam).ToJArray();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}