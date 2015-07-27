using Newtonsoft.Json;

namespace irception.Domain
{
    public partial class API
    {
        public class RegisterRequestParams
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string SUID { get; set; }
        }

        public static RegisterRequestParams GetRegisterRequestParams(string requestBody)
        {
            return JsonConvert.DeserializeObject<RegisterRequestParams>(requestBody);
        }
    }
}
