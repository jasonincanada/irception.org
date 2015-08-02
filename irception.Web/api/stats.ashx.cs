using irception.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace irception.Web.api
{
    /// <summary>
    /// Statistics queries
    /// </summary>
    public class stats : APIBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string json;

            try
            {
                var channelSlug = context.Request["channel"];
                var networkSlug = context.Request["network"];
                var dataset = context.Request["dataset"];
                var repo = new Repository();

                // Top 25 chatters by line count over last 10 days
                if (dataset == "lines")
                {
                    var data = repo.StatsLineCounts(channelSlug, networkSlug, 14, 20);

                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        labels = data.Select(d => d.Label).ToList(),
                        data = data.Select(d => d.Value).ToList()
                    });
                }

                // Line graph of top 10 chatters by cummulative interval
                else
                {
                    var labels = new List<string>();
                    var data = repo.StatsRace(channelSlug, networkSlug, 14, 10, labels);

                    json = JsonConvert.SerializeObject(new
                    {
                        success = true,
                        labels = labels,
                        series = data
                            .Select(d => d.Label)
                            .ToList(),
                        data = data
                            .Select(d => d.Values)
                            .ToList()
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