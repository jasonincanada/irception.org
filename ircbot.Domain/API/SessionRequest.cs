using Newtonsoft.Json;

namespace ircbot.Domain
{
    public partial class API
    {
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
    }
}
