using irception.Domain;
using irception.Domain.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

                    // If authorizing via PM auth command, store the UserID for this auth token
                    if (!string.IsNullOrEmpty(requestParams.SUID))
                    {
                        repo.MatchUserToAuthToken(user, requestParams.SUID);
                        repo.SaveChanges();
                    }

                    List<FirstChannelVisit> channelsVisited = repo.GetFirstChannelVisits(user.UserID);

                    // TODO: Merge this up
                    foreach (var chan in channelsVisited)
                        chan.DateVisitDisplay = chan.DateVisit.ToString();
                                        
                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        User = PlainUser.FromModel(user),
                        Token = PlainToken.FromModel(token),
                        ChannelsVisited = channelsVisited
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