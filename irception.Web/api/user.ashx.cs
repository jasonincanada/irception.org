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
            var repo = new Repository();
            var user = repo.GetUser(context.Request["username"]);

            var json = JsonConvert.SerializeObject(new
            {
                User = PlainUser.FromModel(user)
            });

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