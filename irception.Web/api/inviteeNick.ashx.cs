using irception.Domain;
using Newtonsoft.Json;
using System;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Get the nick name associated with the passed SUID to use for the default username value when registering
    /// </summary>
    public class inviteeNick : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var repo = new Repository();

                API.InviteeNickRequestParams requestParams = API.GetInviteeNickRequestParams(GetRequestBody(context));

                string nick = repo.GetInviteeNick(requestParams.SUID);

                json = JsonConvert.SerializeObject(new
                {
                    success = true,
                    nick = nick
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