using irception.Domain.DTO;

namespace irception.Domain
{
    public class PlainIgnore
    {
        public string Nick { get; set; }
        public PlainUser UserAddedBy { get; set; }

        public static PlainIgnore FromModel(Ignore i)
        {
            return new PlainIgnore
            {
                Nick = i.Nick,
                UserAddedBy = PlainUser.FromModel(i.User)
            };
        }
    }
    
    public class PlainAutoNSFW
    {
        public int AutoNSFWID { get; private set; }
        public string Fragment { get; private set; }

        public static PlainAutoNSFW FromModel(AutoNSFW a)
        {
            return new PlainAutoNSFW
            {
                AutoNSFWID = a.AutoNSFWID,
                Fragment = a.Fragment
            };
        }
    }
}
