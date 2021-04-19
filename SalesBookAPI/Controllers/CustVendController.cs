using Newtonsoft.Json.Linq;
using SalesBookAPI.BL;
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
    public class CustVendController : ApiController
    {
        CustVend_BL bl = new CustVend_BL();

        #region "CRUD"
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
                    RtnObject = bl.GetData(data, t);
                }

                return RtnObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [ActionName("saveData")]
        [HttpPost]
        public JObject SaveData([FromBody]JObject data)
        {
            try
            {
                JObject RtnObject = new JObject();
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;

                if (data != null)
                {
                    RtnObject = bl.SaveData(data, t);
                }

                return RtnObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [ActionName("updateData")]
        [HttpPost]
        public JObject UpdateData([FromBody]JObject data)
        {
            try
            {
                JObject RtnObject = new JObject();
                Token t = Request.Properties[SiteConfig.LoginKeyName] as Token;

                if (data != null)
                {
                    RtnObject = bl.UpdateData(data, t);
                }

                return RtnObject;
            }
            catch (Exception)
            {
                throw;
            }
        }


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
                    RtnObject = bl.DeleteData(Code, t);
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
