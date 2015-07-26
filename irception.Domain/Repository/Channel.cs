using System.Linq;

namespace irception.Domain
{
    public partial class Repository
    {
        /// <summary>
        /// Get Channel object by its slug
        /// </summary>
        /// <param name="network"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public Channel GetChannelBySlug(string network, string channel)
        {
            return _context
                .Channels
                .Include("Network")
                .Where(c => c.Slug == channel && c.Network.Slug == network)
                .FirstOrDefault();
        }

        /// <summary>
        /// Check whether this nick is being ignored on this channel
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="channelID"></param>
        /// <returns>True if the nick is being ignored</returns>
        public bool OnIgnore(string nick, int channelID)
        {
            return _context
                .Ignores
                .Any(i => i.Nick == nick
                        && i.FKChannelID == channelID
                        && i.DateRemoved == null);
        }
        
        /// <summary>
        /// Add a Line to the table. Calls SaveChanges() and returns the new LineID
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="nick"></param>
        /// <param name="messageLength"></param>
        public long Line(Line line)
        {
            _context
                .Lines
                .Add(line);

            SaveChanges();

            return line.LineID;
        }
    }
}
