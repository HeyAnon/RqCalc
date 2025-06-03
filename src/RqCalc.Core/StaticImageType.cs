using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
public enum StaticImageType
{
    [EnumMember]
    EmptyCard = 1,

    [EnumMember]
    EquipmentToolTipCardEmpty = 2,

    [EnumMember]
    EquipmentToolTipCard = 3,

    [EnumMember]
    Stamp = 4,

    [EnumMember]
    Aura = 5,

    [EnumMember]
    Talent = 6,

    [EnumMember]
    Feed = 7,

    [EnumMember]
    Elixir = 8,

    [EnumMember]
    Guild = 9,

    [EnumMember]
    EmptyStamp = 10,

    [EnumMember]
    Consumable = 11,

    [EnumMember]
    Buff = 12,

    [EnumMember]
    EquipmentElixirClock = 13,

    [EnumMember]
    Collections = 14
}