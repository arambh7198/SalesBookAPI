﻿using Newtonsoft.Json.Linq;
using SalesBookAPI.Custom;
using SalesBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesBookAPI.Controllers
{
    public class CommonController : ApiController
    {
        #region "Feedback Post"
        [ActionName("getPartyInfo")]
        [HttpPost]

        public JArray getPartyInfo([FromBody]JObject data)
        {
            try
            {
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;
                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("UserCode", t.UserCode.ToString());
                IncludeParam.Add("SessionID", t.SessionID.ToString());
                return StaticGeneral.GetDataTable("pGetCustVendForAutoComplete", data, null, IncludeParam).ToJArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [ActionName("getCityInfo")]
        [HttpPost]

        public JArray getCityInfo([FromBody]JObject data)
        {
            try
            {
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;
                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("UserCode", t.UserCode.ToString());
                IncludeParam.Add("SessionID", t.SessionID.ToString());
                return StaticGeneral.GetDataTable("pGetCityForAutoComplete", data, null, IncludeParam).ToJArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [ActionName("getItemInfo")]
        [HttpPost]

        public JArray getItemInfo([FromBody]JObject data)
        {
            try
            {
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;
                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("UserCode", t.UserCode.ToString());
                IncludeParam.Add("SessionID", t.SessionID.ToString());
                return StaticGeneral.GetDataTable("pGetItemForAutoComplete", data, null, IncludeParam).ToJArray();
            }
            catch (Exception)
            {
                throw;
            }
        }


        [ActionName("getBankInfo")]
        [HttpPost]

        public JArray getBankInfo([FromBody]JObject data)
        {
            try
            {
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;
                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("UserCode", t.UserCode.ToString());
                IncludeParam.Add("SessionID", t.SessionID.ToString());
                return StaticGeneral.GetDataTable("pGetBankForAutoComplete", data, null, IncludeParam).ToJArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}