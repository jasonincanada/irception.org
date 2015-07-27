using Newtonsoft.Json;

namespace irception.Domain
{
    public partial class API
    {
        public class InviteRequestParams
        {
            public string InviterNick { get; set; }
            public string InviterUser { get; set; }
            public string InviterHost { get; set; }
            public string InviteeNick { get; set; }
            public string InviteeUser { get; set; }
            public string InviteeHost { get; set; }
            public int ChannelID { get; set; }
        }

        public static InviteRequestParams GetInviteRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<InviteRequestParams>(requestBody);
        }
    }

}
