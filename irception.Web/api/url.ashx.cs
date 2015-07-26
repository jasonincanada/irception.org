using irception.Domain;
using Newtonsoft.Json;
using System;
using System.Web;

namespace irception.Web.api
{
    public class url : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var requestBody = context.Request.Form["RequestBody"];
                var requestParams = API.GetURLRequestParams(requestBody);
                var repo = new Repository();

                if (!repo.OnIgnore(requestParams.Nick, requestParams.ChannelID))
                {

                    URL url = new URL
                    {
                        At = DateTime.UtcNow,
                        Nick = requestParams.Nick,
                        URL1 = requestParams.URL,
                        FKChannelID = requestParams.ChannelID
                    };

                    if (requestParams.YouTube != null)
                    {
                        try
                        {
                            API.DecorateURLForYouTube(url, requestParams.YouTube);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    int urlID = repo.URL(url);

                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        ID = urlID
                    });
                }
                else
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = false,
                        error = "Ignored nick"
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