using irception.Domain;
using Newtonsoft.Json;
using System;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Handle invites from the bot on IRC
    /// </summary>
    public class invite : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var requestBody = context.Request.Form["RequestBody"];
                var requestParams = API.GetInviteRequestParams(requestBody);
                var repo = new Repository();

                var inviter = repo.GetAuthenticatedUser(requestParams.InviterNick, requestParams.InviterUser, requestParams.InviterHost);
                Invite invite = repo.GetOrCreateInvite(inviter, requestParams.InviteeNick, requestParams.InviteeUser, requestParams.InviteeHost, requestParams.ChannelID);
                                               
                json = JsonConvert.SerializeObject(new
                {
                    success = true,                    
                    SUID = invite.SUID
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