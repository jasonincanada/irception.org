using Newtonsoft.Json;

namespace irception.Domain
{
    public partial class API
    {
        public class LoginRequestParams
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public static LoginRequestParams GetLoginRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<LoginRequestParams>(requestBody);
        }
    }
}
