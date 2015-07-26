using irception.Domain;
using irception.Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Web;

namespace irception.Web.api
{
    public class connect : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var SUID = context.Request["SUID"];                
                var repo = new Repository();

                // Look for this session
                var session = repo.GetSession(SUID);

                json = JsonConvert.SerializeObject(new
                {
                    success = true,
                    SUID = session.SUID,
                    Nick = session.Nick,
                    NickUserHost = session.NickUserHost,
                    Channel = PlainChannel.FromModel(session.Channel),
                    ChannelSlug = session.Channel.Slug,
                    NetworkSlug = session.Channel.Network.Slug
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