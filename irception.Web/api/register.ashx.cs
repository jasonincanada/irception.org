using irception.Domain;
using irception.Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Register a new user account via the SUID from the invite
    /// </summary>
    public class register : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var repo = new Repository();
                API.RegisterRequestParams requestParams = API.GetRegisterRequestParams(GetRequestBody(context));

                requestParams.Username = requestParams.Username.Trim();

                // TODO: username validation

                if (repo.InviteAccepted(requestParams.SUID))
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = false,
                        UserMessage = "This invite has already been accepted."
                    });
                }
                else
                { 
                    if (!repo.UsernameAvailable(requestParams.Username))
                    {
                        json = JsonConvert.SerializeObject(new
                        {
                            success = false,
                            UserMessage = "This username has already been used."
                        });
                    }
                    else
                    {                                     
                        User user = repo.Register(requestParams.Username, requestParams.Password, requestParams.SUID);

                        if (user == null)
                            throw new Exception("Error during registration.");

                        Token token = repo.GetOrCreateToken(user);

                        json = JsonConvert.SerializeObject(new
                        {
                            success = true,
                            User = PlainUser.FromModel(user),
                            Token = PlainToken.FromModel(token)
                        });
                    }
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