using System.Web;
using Newtonsoft.Json;

namespace irception.Web.api
{
    /// <summary>
    /// Summary description for test
    /// </summary>
    public class test : APIBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string message = "Hello, world!";

            string json = JsonConvert.SerializeObject(new { message });

            context.Response.ContentType = "text/json";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}