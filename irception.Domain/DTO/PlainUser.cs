﻿using System.Collections.Generic;
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
        public string DateRegisteredDisplay { get; set; }
        public List<PlainPermission> Permissions { get; set; }

        public static PlainUser FromModel(User user)
        {
            return new PlainUser
            {
                UserID = user.UserID,
                Username = user.Username,
                InviteLevel = user.InviteLevel,
                DateRegisteredDisplay = string.Format("{0:MMM d, yyyy}", user.DateAdded),
                Permissions = user
                    .Permissions
                    .Select(p => new PlainPermission
                    {
                        ChannelID = p.FKChannelID,
                        Permission = p.Permission1
                    })
                    .ToList()
            };
        }
    }
}
