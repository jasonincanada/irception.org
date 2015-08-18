using irception.Domain;
using irception.Domain.DTO;
using Newtonsoft.Json;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Get public information for a user
    /// </summary>
    public class user : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json = "{}";

            var repo = new Repository();
            var model = repo.GetUser(context.Request["username"]);
            var user = PlainUser.FromModel(model, true);

            if (user.InviteLevel > 0)
            {
                var invitedby = PlainUser.FromModel(repo.GetUser(model.FKUserIDInvitedBy ?? 0), false);

                json = JsonConvert.SerializeObject(new
                {
                    User = user,
                    InvitedBy = invitedby
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    User = user
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