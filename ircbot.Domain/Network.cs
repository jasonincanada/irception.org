//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ircbot.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Network
    {
        public Network()
        {
            this.Servers = new HashSet<Server>();
            this.Channels = new HashSet<Channel>();
        }
    
        public int NetworkID { get; set; }
        public string Name { get; set; }
        public string Nick { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Slug { get; set; }
    
        public virtual ICollection<Server> Servers { get; set; }
        public virtual ICollection<Channel> Channels { get; set; }
    }
}
