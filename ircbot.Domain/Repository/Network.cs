using System;
using System.Collections.Generic;
using System.Linq;

namespace ircbot.Domain
{
    public partial class Repository
    {
        /// <summary>
        /// Get all Networks, including their Servers and Channels
        /// </summary>
        /// <returns></returns>
        public List<Network> GetNetworks()
        {
            return _context
                .Networks
                .Include("Servers")
                .Include("Channels")
                .ToList();
        }        
    }
}
