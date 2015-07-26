using irception.Domain;
using irception.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Summary description for auth
    /// </summary>
    public class auth : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var requestBody = context.Request.Form["RequestBody"];
                var requestParams = API.GetAuthRequestParams(requestBody);
                var repo = new Repository();

                // Create a new auth record for the user, give him the UID for the web
                Auth auth = new Auth
                {
                    FKNetworkID = requestParams.NetworkID,
                    Nick = requestParams.Nick,
                    Username = requestParams.Username,
                    Host = requestParams.Host,
                    SUID = Utils.Get32ByteUID(),
                    DateIssued = DateTime.UtcNow
                };

                repo.AddAuth(auth);
                repo.SaveChanges();

                json = JsonConvert.SerializeObject(new
                {
                    success = true,
                    ID = auth.AuthID,
                    SUID = auth.SUID
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