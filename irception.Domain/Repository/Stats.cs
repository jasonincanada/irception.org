using System;
using System.Collections.Generic;
using System.Linq;

namespace irception.Domain
{
    public class LabelValuePair
    {
        public string Label { get; set; }
        public long Value { get; set; }
    }

    public class LabelValuesPair
    {
        public string Label { get; set; }
        public List<long> Values { get; set; }
    }

    public partial class Repository
    {
        /// <summary>
        /// Return most active chatters by number of lines
        /// </summary>
        /// <param name="channelSlug"></param>
        /// <param name="networkSlug"></param>
        /// <param name="daysAgo"></param>
        /// <returns></returns>
        public List<LabelValuePair> StatsLineCounts(string channelSlug, string networkSlug, int daysAgo, int nickCount)
        {
            DateTime minDate = DateTime.UtcNow.AddDays(-daysAgo);

            return _context
                .Lines
                .Where(l => l.Channel.Slug == channelSlug
                            && l.Channel.Network.Slug == networkSlug
                            && l.At >= minDate)
                .GroupBy(g => new { g.Nick })
                .Select(l => new LabelValuePair
                {
                    Label = l.Key.Nick,
                    Value = l.Count()
                })
                .OrderByDescending(o => o.Value)
                .Take(nickCount)
                .ToList();
        }

        // Daily intervals for now
        public List<LabelValuesPair> statsRace(string channelSlug, string networkSlug, int intervalCount, int nickCount, List<string> intervalLabels)
        {
            DateTime minDate = DateTime.UtcNow.AddDays(-intervalCount);
                        
            var deets = _context
                .Lines
                .Where(l => l.Channel.Slug == channelSlug
                            && l.Channel.Network.Slug == networkSlug
                            && l.At >= minDate)
                .GroupBy(g => new
                {
                    Nick = g.Nick,
                    Year = g.At.Year,
                    Month = g.At.Month,
                    Day = g.At.Day
                })
                .ToList()
                .Select(l => new
                {
                    l.Key.Nick,
                    Date = new DateTime(l.Key.Year, l.Key.Month, l.Key.Day),
                    LineCount = l.Count()
                })
                .ToList();
                        
            var nicks = deets
                .Select(d => d.Nick)
                .Distinct()
                .ToList();

            // TODO: There's got to be an easier way to do this
            Dictionary<string, List<long>> dict = new Dictionary<string, List<long>>();

            foreach (var n in nicks)
            {
                List<long> onetoten = new List<long>();

                for (int i = 0; i < intervalCount + 1; i++)
                    onetoten.Add(0);
                
                dict.Add(n, onetoten);
            };

            DateTime day = DateTime.UtcNow.Date.AddDays(-intervalCount);
            
            for (int interval = 0, j = intervalCount; interval < intervalCount + 1; interval++, j--)
            {
                // Build the x axis labels                
                intervalLabels.Add(DateTime.UtcNow.Date.AddDays(-interval).ToShortDateString());
                //intervalLabels.Add(day.ToShortDateString());

                // Build the y axis values
                DateTime next = day.AddDays(1);

                foreach (string nick in nicks)
                {
                    long lines = deets
                        .Where(d => d.Nick == nick
                                    && d.Date == day)
                        .Select(d => d.LineCount)
                        .Sum();

                    dict[nick][j] = lines;
                    
                    // This keeps a running total
                    if (interval >= 1)
                        dict[nick][j] += dict[nick][j + 1];
                }

                day = day.AddDays(1);
            }
            
            List<LabelValuesPair> rows = new List<LabelValuesPair>();                        
            
            foreach (var nick in nicks)
            {    
                rows.Add(new LabelValuesPair
                {
                    Label = nick,
                    Values = dict[nick]
                });
            }

            return rows
                .OrderByDescending(o => o.Values[0])
                .Take(nickCount)
                .ToList();
        }
    }
}
