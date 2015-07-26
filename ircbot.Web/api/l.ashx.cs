using ircbot.Domain;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ircbot.Web.api
{
    public class l : APIBase, IHttpHandler
    {        
        public void ProcessRequest(HttpContext context)
        {
            string json;
            
            try
            {
                var repo = new Repository();

                int fkChannelID = Convert.ToInt32(context.Request.Form["ChannelID"]);
                int messageLength = Convert.ToInt32(context.Request.Form["MessageLength"]);
                string nick = context.Request.Form["Nick"];

                Line line = new Line
                {
                    At = DateTime.UtcNow,
                    Nick = nick,
                    Length = messageLength,
                    FKChannelID = fkChannelID
                };

                if (!repo.OnIgnore(nick, fkChannelID))
                {
                    var lineID = repo.Line(line);

                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        ID = lineID
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