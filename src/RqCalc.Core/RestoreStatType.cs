using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
public enum RestoreStatType
{
    [EnumMember]
    PerKill = -3,

    [EnumMember]
    PerHit = -2,

    [EnumMember]
    Passive = -1
}