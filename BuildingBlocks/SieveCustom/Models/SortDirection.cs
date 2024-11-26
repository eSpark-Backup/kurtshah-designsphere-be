using System.Runtime.Serialization;

namespace SieveCustom.Models;

public enum SortDirection
{
    [EnumMember]
    Undefined,

    [EnumMember]
    Ascending,

    [EnumMember]
    Descending
}