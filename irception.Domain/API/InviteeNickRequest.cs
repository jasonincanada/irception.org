using Newtonsoft.Json;

namespace irception.Domain
{
    public partial class API
    {
        public class InviteeNickRequestParams
        {
            public string SUID { get; set; }
        }

        public static InviteeNickRequestParams GetInviteeNickRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<InviteeNickRequestParams>(requestBody);
        }
    }
}
