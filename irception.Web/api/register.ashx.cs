using irception.Domain;
using irception.Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

                string username = requestParams.Username.Trim();

                if (!Irception.IsValidUsername(username))
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = false,
                        UserMessage = "Usernames consist of a-z 0-9 _ -"
                    });
                }                
                else if (!repo.UsernameAvailable(username))
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = false,
                        UserMessage = "Username taken."
                    });
                }
                else if (repo.InviteAccepted(requestParams.SUID))
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = false,
                        UserMessage = "Invite already accepted."
                    });
                }
                else
                {
                    User user = repo.Register(username, requestParams.Password, requestParams.SUID);

                    if (user == null)
                        throw new Exception("Error during registration.");

                    Token token = repo.GetOrCreateToken(user);

                    List<FirstChannelVisit> channelsVisited = repo.GetFirstChannelVisits(user.UserID);

                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        User = PlainUser.FromModel(user),
                        Token = PlainToken.FromModel(token),
                        ChannelsVisited = channelsVisited
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