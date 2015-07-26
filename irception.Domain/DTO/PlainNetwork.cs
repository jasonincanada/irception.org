using System;
using System.Collections.Generic;
using System.Linq;

namespace irception.Domain.DTO
{
    public class PlainServer
    {
        public int ServerID { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public static PlainServer FromModel(Server server)
        {
            return new PlainServer
            {
                ServerID = server.ServerID,
                Host = server.Host,
                Port = server.Port ?? 6667
            };
        }
    }

    public class PlainChannel
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public bool InChannel { get; set; }
        public string Slug { get; set; }

        public static PlainChannel FromModel(Channel channel)
        {
            return new PlainChannel
            {
                ChannelID = channel.ChannelID,
                ChannelName = channel.ChannelName,
                Slug = channel.Slug,
                InChannel = channel.InChannel
            };
        }
    }

    public class PlainNetwork
    {
        public List<PlainServer> Servers { get; set; }
        public List<PlainChannel> Channels { get; set; }

        public int NetworkID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Nick { get; set; }
        public bool IsActive { get; set; }

        public static PlainNetwork FromModelNetwork(Network network)
        {
            return new PlainNetwork
            {
                NetworkID = network.NetworkID,
                Name = network.Name,
                Slug = network.Slug,
                Nick = network.Nick,
                IsActive = network.IsActive ?? false,

                Channels = network
                    .Channels
                    .Select(PlainChannel.FromModel)                    
                    .ToList(),

                Servers = network
                    .Servers
                    .Select(PlainServer.FromModel)
                    .ToList()                
            };
        }
    }
       
}
