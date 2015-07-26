using System;
using System.Collections.Generic;
using System.Linq;

namespace irception.Domain
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
        /// Add an Auth object to the database.  Does not call SaveChanges() on the context.
        /// </summary>
        /// <param name="auth"></param>
        public void AddAuth(Auth auth)
        {
            _context
                .Auths
                .Add(auth);
        }

        /// <summary>
        /// Update the Auth table with the UserID that authenticated the token. Does not call SaveChanges() on the context.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="suid"></param>
        public void MatchUserToAuthToken(User user, string suid)
        {
            var auth = _context
                .Auths
                .Where(a => a.SUID == suid)
                .FirstOrDefault();

            if (auth == null)
                return;

            if (auth.FKUserID != null)
                return;

            auth.FKUserID = user.UserID;
            auth.DateAuthenticated = DateTime.UtcNow;
        }

        /// <summary>
        /// Check whether this nick!user@host has already authenticated to this network
        /// </summary>
        /// <param name="networkID"></param>
        /// <param name="nick"></param>
        /// <param name="username"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public bool HaveAuth(int networkID, string nick, string username, string host)
        {
            return _context
                .Auths
                .Any(a => a.FKNetworkID == networkID
                            && a.Nick == nick
                            && a.Username == username
                            && a.Host == host
                            && a.DateAuthenticated != null);
        }
    }
}
