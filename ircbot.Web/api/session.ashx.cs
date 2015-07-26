using ircbot.Domain;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ircbot.Web.api
{
    public class session : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var requestBody = context.Request.Form["RequestBody"];
                var requestParams = API.GetSessionRequestParams(requestBody);
                var repo = new Repository();

                // Create a new session for the user, give him the UID for the web
                Session session = new Session
                {
                    FKChannelID = requestParams.ChannelID,
                    Nick = requestParams.Nick,
                    NickUserHost = requestParams.NickUserHost,
                    SUID = Utils.Get32ByteUID(),
                    Started = DateTime.UtcNow
                };

                repo.AddSession(session);
                repo.SaveChanges();                

                json = JsonConvert.SerializeObject(new
                {
                    success = true,
                    ID = session.SessionID,
                    SUID = session.SUID
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