using irception.Domain;
using irception.Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Log an account in via username/password
    /// </summary>
    public class login : APIBase, IHttpHandler
    {       

        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {                
                var repo = new Repository();
                API.LoginRequestParams requestParams = API.GetLoginRequestParams(GetRequestBody(context));

                User user = repo.Login(requestParams.Username, requestParams.Password);
                
                if (user != null)
                {
                    Token token = repo.GetOrCreateToken(user);
                    
                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        User = PlainUser.FromModel(user),
                        Token = PlainToken.FromModel(token)
                    });
                }
                else
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = false,
                        UserMessage = "Invalid login."
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