namespace irception.Domain
{
    public class PlainIgnore
    {
        public string Nick { get; set; }
        public string AddedBy { get; set; }

        public static PlainIgnore FromModel(Ignore i)
        {
            return new PlainIgnore
            {
                Nick = i.Nick,
                AddedBy = i.User.Username
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
