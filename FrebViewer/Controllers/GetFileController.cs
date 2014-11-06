using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace FrebViewer.Controllers
{
    public class GetFileController : ApiController
    {
        public HttpResponseMessage Get()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string filename = queryString["filename"];

            var file = new Files();
            var content = file.GetContent(filename);
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(content, Encoding.UTF8, "text/xml");
            resp.Headers.CacheControl = new CacheControlHeaderValue();
            resp.Headers.CacheControl.MaxAge = new TimeSpan(0, 5, 0);
            resp.Headers.CacheControl.Public = true;
            
            return resp;
        }
    }
}
