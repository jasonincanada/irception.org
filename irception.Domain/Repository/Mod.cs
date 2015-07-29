using System.Collections.Generic;
using System.Linq;

namespace irception.Domain
{
    public partial class Repository
    {
        public List<AutoNSFW> GetChannelAutoNSFWList(int channelID)
        {
            return _context
                .AutoNSFWs
                .Where(a => a.DateDeleted == null && a.FKChannelID == channelID)
                .ToList();                
        }
    }
}
