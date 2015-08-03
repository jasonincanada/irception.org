using System;
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

        public User VerifyLoginToken(string loginToken)
        {
            return _context
                .Tokens
                .Include("User")
                .Where(t => t.Token1 == loginToken
                            && t.Ended == null)
                .Select(u => u.User)
                .FirstOrDefault();
        }
    }
}
