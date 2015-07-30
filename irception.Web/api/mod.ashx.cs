using irception.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Return channel info pertaining to moderator activities
    /// </summary>
    public class mod : APIBase, IHttpHandler
    {
        private HttpContext _context;

        private int ChannelID { get { return Convert.ToInt32(_context.Request["id"]); } }

        public void ProcessRequest(HttpContext context)
        {
            _context = context;

            string json = "{}";

            try
            {
                var repo = new Repository();

                List<AutoNSFW> autoNSFW = repo.GetChannelAutoNSFWList(ChannelID);
                List<Ignore> ignores = repo.GetIgnores(ChannelID);
                
                json = JsonConvert.SerializeObject(new
                {
                    success = true,

                    autoNSFW = autoNSFW
                        .Select(PlainAutoNSFW.FromModel)
                        .ToList(),

                    ignores = ignores
                        .Select(PlainIgnore.FromModel)
                        .ToList()
                });
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