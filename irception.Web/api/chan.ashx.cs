using irception.Domain;
using irception.Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace irception.Web.api
{
    public class chan : APIBase, IHttpHandler
    {
        private HttpContext _context;

        private string RequestChannel { get { return _context.Request["channel"]; } }
        private string RequestNetwork { get { return _context.Request["network"]; } }
        private long URLUpdateHistoryID
        {
            get
            {
                long luuhid;

                if (long.TryParse(_context.Request["luuhid"], out luuhid))
                    return luuhid;

                return 0;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            _context = context;

            List<URL> urls = new List<URL>();
            string json = "{}";

            try
            {
                var repo = new Repository();
                Channel channel = repo.GetChannelBySlug(context.Request["network"], RequestChannel);

                if (channel == null)
                    throw new Exception("Channel doesn't exist.");

                var lastUrlID = URLUpdateHistoryID;                

                if (lastUrlID == 0)
                    urls = repo.GetURLs(channel.ChannelID);
                else
                {
                    var ids = repo.GetUpdatedURLs(channel.ChannelID, lastUrlID);

                    urls = repo.GetURLs(ids);
                }

                long lastURLUpdateHistoryID = repo.GetLastURLUpdateHistoryID();

                List<PlainURL> plains = urls
                    .Select(PlainURL.FromModel)
                    .ToList();

                if (lastUrlID == 0)
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        URLs = plains,
                        luuhid = lastURLUpdateHistoryID,
                        Channel = PlainChannel.FromModel(channel),
                        NetworkSlug = channel.Network.Slug,
                        NetworkName = channel.Network.Name,
                        ChannelSlug = channel.Slug
                    });
                }
                else
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        URLs = plains,
                        luuhid = lastURLUpdateHistoryID
                    });
                }
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new
                {
                    success = false,
                    error = "There was an error: " + ex.Message
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