using System;

namespace ircbot.Domain.DTO
{
    public class PlainToken
    {        
        public string Token { get; set; }

        public int UserID { get; set; }

        public DateTime Started { get; set; }                
        public string IP { get; set; }        

        public static PlainToken FromModel(Token token)
        {
            return new PlainToken
            {
                IP = token.IP,
                Started = token.Started,
                Token = token.Token1,
                UserID = token.FKUserID
            };
        }
    }
}
