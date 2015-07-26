using System.IO;
using System.Web;
using ircbot.Domain;

namespace ircbot.Web
{
    public class APIBase
    {
        protected ircCache Cache { get; set; }

        public APIBase()
        {
            Cache = Global.Cache;
        }

        protected void SetNoCaching(HttpContext context)
        {
            context.Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            context.Response.AppendHeader("Pragma", "no-cache");
            context.Response.AppendHeader("Expires", "0");
        }

        protected static string GetRequestBody(HttpContext context)
        {
            var ms = new MemoryStream();

            context.Request.InputStream.CopyTo(ms);
            ms.Position = 0;

            using (var sr = new StreamReader(ms))
            {
                return sr.ReadToEnd();
            }
        }
    }
}