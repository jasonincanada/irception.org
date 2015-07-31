using System;

namespace irception.Domain.DTO
{
    public class PlainURL
    {
        public int URLID { get; set; }
        public string URL { get; set; }
        public string Nick { get; set; }
        public DateTime At { get; set; }
        public string AtString { get; set; }
        public long AtTicks { get; set; }
        public int SecondsElapsed { get; set; }
        public bool NSFW { get; set; }
        public int VoteCount { get; set; }

        public string Type { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static PlainURL FromModel(URL url)
        {
            return new PlainURL
            {
                URLID = url.URLID,
                URL = url.URL1,
                At = url.At,
                SecondsElapsed = Convert.ToInt32((DateTime.UtcNow - url.At).TotalSeconds),
                AtTicks = url.At.Ticks,
                AtString = url.At.ToShortDateString() + " " + url.At.ToShortTimeString(),
                Nick = url.Nick,
                Type = url.Type,
                Thumbnail = url.Thumbnail,
                Title = url.Title,
                Description = url.Description,
                NSFW = url.NSFW ?? false,
                VoteCount = url.VoteCount ?? 0
            };
        }
    }
}
