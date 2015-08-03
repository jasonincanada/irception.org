using Newtonsoft.Json;

namespace irception.Domain
{
    public partial class API
    {
        public class MeRequestParams
        {
            public string Token { get; set; }
            public string Signature { get; set; }
        }

        public static MeRequestParams GetMeRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<MeRequestParams>(requestBody);
        }
    }
}
