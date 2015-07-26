using Newtonsoft.Json;

namespace irception.Domain
{
    public partial class API
    {
        public class AuthRequestParams
        {
            public string Nick { get; set; }
            public string Username { get; set; }
            public string Host { get; set; }
            public int NetworkID { get; set; }
        }

        public static AuthRequestParams GetAuthRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<AuthRequestParams>(requestBody);
        }
    }
}
