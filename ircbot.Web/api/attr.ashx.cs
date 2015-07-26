using ircbot.Domain;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ircbot.Web.api
{
    /// <summary>
    /// Set an attribute on an URL (NSFW, etc)
    /// </summary>
    public class attr : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var repo = new Repository();

                API.AttrRequestParams requestParams = API.GetAttrRequestParams(GetRequestBody(context));
                
                if (requestParams.SetAttr == "nsfw")
                    repo.SetURLNSFW(requestParams.URLID);

                json = JsonConvert.SerializeObject(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new
                {
                    success = false,
                    error = "There was an exception: " + ex.Message
                });
            }

            SetNoCaching(context);
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