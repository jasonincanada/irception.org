using System;
using System.Collections.Generic;
using System.Linq;

namespace irception.Domain
{
    public partial class Repository
    {
        /// <summary>
        /// Set the NSFW flag for the passed URL ID
        /// </summary>
        /// <param name="urlID"></param>
        public void SetURLNSFW(int urlID)
        {
            var url = _context
                .URLs
                .Where(u => u.URLID == urlID)
                .FirstOrDefault();

            if (url == null)
                return;

            url.NSFW = true;

            AddURLUpdateHistory(urlID);
            SaveChanges();
        }

        /// <summary>
        /// Unset the NSFW flag for the passed URL ID
        /// </summary>
        /// <param name="urlID"></param>
        public void UnsetURLNSFW(int urlID)
        {
            var url = _context
                .URLs
                .Where(u => u.URLID == urlID)
                .FirstOrDefault();

            if (url == null)
                return;

            url.NSFW = false;

            AddURLUpdateHistory(urlID);
            SaveChanges();
        }

        public List<Ignore> GetIgnores(int channelID)
        {
            return _context
                .Ignores
                .Where(i => i.FKChannelID == channelID && i.DateRemoved == null)
                .ToList();
        }

        /// <summary>
        /// Get the latest URLs for the given channenl
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public List<URL> GetURLs(int channelID)
        {
            return _context
                .URLs
                .Where(u => u.FKChannelID == channelID)
                .OrderByDescending(u => u.URLID)
                .Take(100) // TODO: extract
                .ToList();
        }

        /// <summary>
        /// Get the URLs with the passed IDs
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<URL> GetURLs(List<int> ids)
        {
            return _context
                .URLs
                .Where(u => ids.Contains(u.URLID))
                .OrderByDescending(u => u.URLID)
                .Take(100) // TODO: extract
                .ToList();
        }
        
        /// <summary>
        /// Get the latest/highest URLUpdateHistoryID to speed subsequent ajax requests for new URLs
        /// </summary>
        /// <returns></returns>
        public long GetLastURLUpdateHistoryID()
        {
            return _context
                .URLUpdateHistories
                .OrderByDescending(u => u.URLUpdateHistoryID)
                .Take(1)
                .Select(u => u.URLUpdateHistoryID)
                .FirstOrDefault();
        }

        /// <summary>
        /// Return a List of distinct URLIDs updated since the last known poll
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="lastUrlID"></param>
        /// <returns></returns>
        public List<int> GetUpdatedURLs(int channelID, long lastUrlID)
        {
            return _context
                .URLUpdateHistories
                .Where(u => u.URLUpdateHistoryID > lastUrlID
                                && u.URL.FKChannelID == channelID)
                .Select(u => u.FKURLID)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Add a URL to the table. Calls SaveChanges() and returns the new URLID
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public int URL(URL url)
        {
            _context
                .URLs
                .Add(url);

            SaveChanges();
            AddURLUpdateHistory(url.URLID);
            SaveChanges();

            return url.URLID;
        }

        /// <summary>
        /// Add a record to URLUpdateHistory. Does not call SaveChanges() on the context.
        /// </summary>
        /// <param name="url"></param>
        private void AddURLUpdateHistory(int id)
        {
            _context
                .URLUpdateHistories
                .Add(new URLUpdateHistory
                {
                    At = DateTime.UtcNow,
                    FKURLID = id
                });
        }
    }
}
