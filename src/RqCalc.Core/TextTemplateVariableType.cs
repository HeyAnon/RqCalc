using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
public enum TextTemplateVariableType
{
    [EnumMember]
    Const = 0,

    [EnumMember]
    Attack = 1,

    [EnumMember]
    Defense = 2,
}