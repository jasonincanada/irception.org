using System;
using System.Collections.Generic;
using System.Linq;

namespace ircbot.Domain
{
    public partial class Repository
    {
        public List<User> GetUsers()
        {
            return _context
                .Users
                .ToList();
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("username");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("password");

            string lower = username.ToLower();
            string password256 = Utils.GetSHA256(password);

            // TODO: add record of login attempt

            return _context
                .Users
                .Where(u => u.Username.ToLower() == lower
                            && u.PasswordSHA256 == password256)
                .FirstOrDefault();
        }

        /// <summary>
        /// Return any active token for this UserID.  If none, create a new one.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Token GetOrCreateToken(User user)
        {
            var token = _context
                .Tokens
                .Where(t => t.FKUserID == user.UserID
                            && t.Ended == null)
                .OrderBy(t => t.TokenID)
                .FirstOrDefault();

            if (token != null)
                return token;

            // Create a new one and get the ID
            long tokenID = Repository.CreateToken(user, new ircbotEntities());

            return _context
                .Tokens
                .Where(t => t.TokenID == tokenID)
                .FirstOrDefault();
        }

        /// <summary>
        /// Create and save a new Token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        private static long CreateToken(User user, ircbotEntities entities)
        {
            var token = new Token
            {
                FKUserID = user.UserID,
                IP = "ip",
                Started = DateTime.UtcNow,
                Token1 = Utils.Get32ByteUID()
            };

            entities
                .Tokens
                .Add(token);

            entities
                .SaveChanges();

            return token.TokenID;
        }

        /// <summary>
        /// Get existing Session object by its SUID
        /// </summary>
        /// <param name="suid"></param>
        /// <returns></returns>
        public Session GetSession(string suid)
        {
            return _context
                .Sessions
                .Include("Channel")
                .Include("Channel.Network")
                .Where(s => s.SUID == suid)
                .FirstOrDefault();
        }

        public void AddSession(Session session)
        {
            _context
                .Sessions
                .Add(session);
        }
    }
}
