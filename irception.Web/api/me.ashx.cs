using irception.Domain;
using Newtonsoft.Json;
using System;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Updates to the user details on the /me page
    /// </summary>
    public class me : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {                
                var requestParams = API.GetMeRequestParams(GetRequestBody(context));
                var repo = new Repository();

                // Verify user token
                var user = repo.VerifyLoginToken(requestParams.Token);

                if (user != null)
                {
                    repo.UpdateSignature(user, requestParams.Signature);
                    repo.SaveChanges();

                    json = JsonConvert.SerializeObject(new
                    {
                        success = true
                    });
                }
                else
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = false
                    });
                }
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