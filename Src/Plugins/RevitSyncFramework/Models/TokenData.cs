using System;
using System.Runtime.Serialization;

namespace RevitSyncFramework.Models
{
    [DataContract]
    public class TokenData
    {
        [DataMember]
        public string AccessToken { get; set; }

        [DataMember]
        public DateTime Expiration { get; set; }
    }
}