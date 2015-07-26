using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ircbot.Domain
{
    public class API
    {
        public class YouTubeThumbnail
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class YouTubeSnippet
        {
            public string title { get; set; }
            public string description { get; set; }

            public Dictionary<string, YouTubeThumbnail> thumbnails { get; set; }
        }
        
        public class YouTubeItem
        {
            public string kind { get; set; }
            public string id { get; set; }
            public YouTubeSnippet snippet { get; set; }
        }

        public class YouTube
        {
            public string kind { get; set; }
            public List<YouTubeItem> items { get; set; }
        }

        public class URLRequestParams
        {
            public string Nick { get; set; }
            public string URL { get; set; }
            public int ChannelID { get; set; }
            public YouTube YouTube { get; set; }
        }

        public static void DecorateURLForYouTube(URL url, YouTube youTube)
        {
            url.Type = "youtube";
            url.Thumbnail = youTube.items[0].snippet.thumbnails["default"].url;
            url.Title = youTube.items[0].snippet.title;
            url.Description = youTube.items[0].snippet.description;
        }

        public static URLRequestParams GetURLRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<URLRequestParams>(requestBody);
        }

        public class SessionRequestParams
        {
            public string Nick { get; set; }
            public string NickUserHost { get; set; }
            public int ChannelID { get; set; }
        }

        public static SessionRequestParams GetSessionRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<SessionRequestParams>(requestBody);
        }

        public class LoginRequestParams
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public static LoginRequestParams GetLoginRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<LoginRequestParams>(requestBody);
        }

        public class AttrRequestParams
        {
            public int URLID { get; set; }
            public string SetAttr { get; set; }
        }

        public static AttrRequestParams GetAttrRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<AttrRequestParams>(requestBody);
        }
    }
}
