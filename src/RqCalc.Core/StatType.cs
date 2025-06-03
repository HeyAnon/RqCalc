using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
public enum StatType
{
    [EnumMember]
    Primary = -5,

    [EnumMember]
    Energy = -4,

    [EnumMember]
    RestorePerKill = -3,

    [EnumMember]
    RestorePerHit = -2,

    [EnumMember]
    PassiveRestore = -1,

    [EnumMember]
    Other = 0,

    [EnumMember]
    DamageBy = 1,

    [EnumMember]
    Resist = 2,

    [EnumMember]
    DamageTo = 3
}