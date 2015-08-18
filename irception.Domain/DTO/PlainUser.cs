using System.Collections.Generic;
using System.Linq;

namespace irception.Domain.DTO
{
    public class PlainPermission
    {
        public int ChannelID { get; set; }
        public string Permission { get; set; }
    }

    public class PlainUser
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int InviteLevel { get; set; }
        public string Signature { get; set; }
        public string DateRegisteredDisplay { get; set; }
        public List<PlainPermission> Permissions { get; set; }
        public List<PlainUser> Invitees { get; set; }
        public PlainUser InvitedBy { get; set; }

        public static PlainUser FromModel(User user, bool withInvitees = false)
        {
            var u = new PlainUser
            {
                UserID = user.UserID,
                Username = user.Username,
                InviteLevel = user.InviteLevel,
                DateRegisteredDisplay = string.Format("{0:MMM d, yyyy}", user.DateAdded),
                Signature = user.Signature,
                Permissions = user
                    .Permissions
                    .Select(p => new PlainPermission
                    {
                        ChannelID = p.FKChannelID,
                        Permission = p.Permission1
                    })
                    .ToList(),
                Invitees = new List<PlainUser>()
            };

            if (withInvitees)
            {
                u.Invitees = user
                    .Invitees
                    .Where(i => i.DateAccepted != null && i.UserAcceptedBy.DateDeleted == null)
                    .Select(i => FromModel(i.UserAcceptedBy))
                    .ToList();
            }

            return u;
        }
    }
}
