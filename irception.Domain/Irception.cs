using System.Text.RegularExpressions;

namespace irception.Domain
{
    public class Irception
    {
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            var regex = new Regex("^[a-zA-Z0-9_-]{1,50}$");

            return regex.IsMatch(username);
        }
    }
}
