using Newtonsoft.Json.Linq;
using SalesBookAPI.BL;
using SalesBookAPI.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesBookAPI.Controllers
{
    public class loginController : ApiController
    {
        Login_BL bl = new Login_BL();
        // GET api/<controller>
        [AllowAnonymous]
        [ActionName("AuthenticateUser")]
        [HttpPost]
        public JObject AuthenticateUser([FromBody]JObject data)
        {
            try
            {
                JObject RtnObject = new JObject();
                DataTable dtRtnData = bl.ValidateUser(data["UserName"].ToString(), data["Password"].ToString());
                if (dtRtnData != null && dtRtnData.Rows.Count > 0)
                {
                    RtnObject["Data"] = dtRtnData.ToJArray();
                }
                else
                {
                    RtnObject["Data"] = "Invalid UserName and Password";
                }
                return RtnObject;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}