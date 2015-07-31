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

        /// <summary>
        /// Return a User object that previously authenticated under this nick/user/host
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="username"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public User GetAuthenticatedUser(string nick, string username, string host)
        {
            var auth = _context
                .Auths
                .Where(a => a.DateAuthenticated != null
                            && a.Nick == nick
                            && a.Username == username
                            && a.Host == host)
                .OrderByDescending(a => a.AuthID)
                .FirstOrDefault();

            if (auth == null)
                return null;

            return auth.User;
        }

        /// <summary>
        /// Create a new Invite object, save it to the table and return it.  If this nick!username@host already has an
        /// invite, return the existing object.
        /// </summary>
        /// <param name="inviter"></param>
        /// <param name="nick"></param>
        /// <param name="username"></param>
        /// <param name="host"></param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public Invite GetOrCreateInvite(User inviter, string nick, string username, string host, int channelID)
        {
            var existing = _context
                .Invites
                .Where(i => i.Nick == nick && i.User == username && i.Host == host)
                .FirstOrDefault();

            if (existing != null)
                return existing;

            var invite = new Invite
            {
                FKChannelID = channelID,
                DateInvited = DateTime.UtcNow,
                FKUserIDInvitedBy = inviter.UserID,
                Nick = nick,
                User = username,
                Host = host,
                SUID = Utils.Get32ByteUID()
            };

            _context
                .Invites
                .Add(invite);

            _context
                .SaveChanges();

            return invite;
        }

        /// <summary>
        /// Get the nick associated with the passed invite SUID
        /// </summary>
        /// <param name="suid"></param>
        /// <returns></returns>
        public string GetInviteeNick(string suid)
        {
            var invite = _context
                .Invites
                .Where(i => i.SUID == suid)
                .FirstOrDefault();

            if (invite == null)
                return "unknown";

            return invite.Nick;
        }

        /// <summary>
        /// Check if the passed username is available for a new account
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True if the username is available, ie, has not been used yet.</returns>
        public bool UsernameAvailable(string username)
        {
            return false == _context
                .Users
                .Any(u => u.Username.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Accept an invitation and set up a new user account
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="suid">Invitation SUID</param>
        /// <returns></returns>
        public User Register(string username, string password, string suid)
        {
            Invite invite = GetInvite(suid);
            User invitedby = invite.UserInvitedBy;
            DateTime now = DateTime.UtcNow;

            User user = new User
            {
                DateAdded = now,
                FKUserIDInvitedBy = invite.FKUserIDInvitedBy,
                InviteLevel = invitedby.InviteLevel + 1,
                PasswordSHA256 = Utils.GetSHA256(password),
                Username = username
            };

            _context
                .Users
                .Add(user);

            invite.UserAcceptedBy = user;
            invite.DateAccepted = now;

            SaveChanges();

            return user;
        }

        /// <summary>
        /// Get an invite by its SUID
        /// </summary>
        /// <param name="suid"></param>
        /// <returns></returns>
        public Invite GetInvite(string suid)
        {
            return _context
                .Invites
                .Include("UserInvitedBy")
                .Where(i => i.SUID == suid)
                .FirstOrDefault();
        }

        /// <summary>
        /// Return true if this invite has already been processed
        /// </summary>
        /// <param name="suid"></param>
        /// <returns></returns>
        public bool InviteAccepted(string suid)
        {
            return _context
                .Invites
                .Any(i => i.SUID == suid
                            && i.DateAccepted != null);
        }

        /// <summary>
        /// Get a list of channels this user has visited
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<FirstChannelVisit> GetFirstChannelVisits(int userID)
        {
            return _context
                .ChannelVisits
                .Where(cv => cv.FKUserID == userID)
                .GroupBy(g => new
                {
                    ChannelSlug = g.Channel.Slug,
                    NetworkSlug = g.Channel.Network.Slug
                })
                .Select(cv => new FirstChannelVisit
                {
                    ChannelSlug = cv.Key.ChannelSlug,
                    NetworkSlug = cv.Key.NetworkSlug,
                    DateVisit = cv.Min(c => c.DateVisit)
                })
                .ToList();
        }

        /// <summary>
        /// Return publically-viewable information for a user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            return _context
                .Users
                .Where(u => u.Username == username)
                .FirstOrDefault();
        }
    }    
}
