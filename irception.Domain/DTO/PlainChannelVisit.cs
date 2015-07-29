using System;

namespace irception.Domain
{
    public abstract class PlainChannelVisit
    {
        public string ChannelSlug { get; set; }
        public string NetworkSlug { get; set; }
    }

    public class FirstChannelVisit : PlainChannelVisit
    {
        public DateTime DateVisit { get; set; }
        public string DateVisitDisplay { get; set; }
    }
}
