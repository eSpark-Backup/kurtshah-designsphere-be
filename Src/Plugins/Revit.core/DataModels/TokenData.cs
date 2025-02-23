using System;
using System.Runtime.Serialization;

namespace Revit.core.DataModels
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