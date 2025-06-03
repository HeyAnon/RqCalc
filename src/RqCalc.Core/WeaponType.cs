using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
public class WeaponTypeInfo
{
    public WeaponTypeInfo(bool isMelee, bool isSingleHand)
    {
        this.IsMelee = isMelee;
        this.IsSingleHand = isSingleHand;
    }


    [DataMember]
    public bool IsMelee { get; private set; }

    [DataMember]
    public bool IsSingleHand { get; private set; }
}