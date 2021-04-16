using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SalesBookAPI.Providers
{
    public class CORSHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var task = base.SendAsync(request, cancellationToken);

                task.Wait();

                HttpResponseMessage response = task.Result;
                if (response == null) return null;
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                //return response;
                return Task.Factory.StartNew<HttpResponseMessage>(() => { return response; });
                /*
                * 
                return base.SendAsync(request, cancellationToken).ContinueWith(
               (task) =>
               {
                   HttpResponseMessage response = task.Result;
                   if (response == null) return null;
                   response.Headers.Add("Access-Control-Allow-Origin", "*");
                   return response;
               }
                );*/
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}