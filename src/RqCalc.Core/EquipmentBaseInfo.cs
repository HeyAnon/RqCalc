using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
[KnownType(typeof(EquipmentInfo))]
[KnownType(typeof(WeaponInfo))]
[KnownType(typeof(DefenseWeaponInfo))]
public abstract class EquipmentBaseInfo : IEquatable<EquipmentBaseInfo>
{
    protected EquipmentBaseInfo(int internalLevel)
    {
        this.InternalLevel = internalLevel;
    }


    public int InternalLevel { get; }


    public void Match(Action<EquipmentInfo> a1, Action<WeaponInfo> a2, Action<DefenseWeaponInfo> a3)
    {
        if (a1 == null) throw new ArgumentNullException(nameof(a1));
        if (a2 == null) throw new ArgumentNullException(nameof(a2));
        if (a3 == null) throw new ArgumentNullException(nameof(a3));

        this.Match(e => { a1(e); return default(object); }, 
            e => { a2(e); return default(object); },
            e => { a3(e); return default(object); });
    }

    public abstract T Match<T>(Func<EquipmentInfo, T> f1, Func<WeaponInfo, T> f2, Func<DefenseWeaponInfo, T> f3);

    public abstract bool Equals(EquipmentBaseInfo other);

    public static bool operator ==(EquipmentBaseInfo info1, EquipmentBaseInfo info2)
    {
        return object.ReferenceEquals(info1, info2) || (!object.ReferenceEquals(info1, null) && info1.Equals(info2));
    }

    public static bool operator !=(EquipmentBaseInfo info1, EquipmentBaseInfo info2)
    {
        return !(info1 == info2);
    }
}

[DataContract]
public class EquipmentInfo : EquipmentBaseInfo, IEquatable<EquipmentInfo>
{
    public EquipmentInfo(int internalLevel, int defense)
        : base(internalLevel)
    {
        this.Defense = defense;
    }


    [DataMember]
    public int Defense { get; private set; }


    public override T Match<T>(Func<EquipmentInfo, T> f1, Func<WeaponInfo, T> f2, Func<DefenseWeaponInfo, T> f3)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2));
        if (f3 == null) throw new ArgumentNullException(nameof(f3));

        return f1(this);
    }

    public override bool Equals(EquipmentBaseInfo other)
    {
        return this.Equals(other as EquipmentInfo);
    }

    public bool Equals(EquipmentInfo other)
    {
        return !object.ReferenceEquals(other, null) && this.InternalLevel == other.InternalLevel && this.Defense == other.Defense;
    }

    public override int GetHashCode()
    {
        return this.InternalLevel ^ this.Defense;
    }
}

[DataContract]
public class WeaponInfo : EquipmentBaseInfo, IEquatable<WeaponInfo>
{
    public WeaponInfo(int internalLevel, int attack, decimal attackSpeed)
        : base(internalLevel)
    {
        this.Attack = attack;
        this.AttackSpeed = attackSpeed;
    }


    [DataMember]
    public int Attack { get; private set; }

    [DataMember]
    public decimal AttackSpeed { get; private set; }


    public override T Match<T>(Func<EquipmentInfo, T> f1, Func<WeaponInfo, T> f2, Func<DefenseWeaponInfo, T> f3)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2));
        if (f3 == null) throw new ArgumentNullException(nameof(f3));

        return f2 (this);
    }


    public override bool Equals(EquipmentBaseInfo other)
    {
        return this.Equals(other as WeaponInfo);
    }

    public bool Equals(WeaponInfo other)
    {
        return !object.ReferenceEquals(other, null) && this.InternalLevel == other.InternalLevel && this.Attack == other.Attack && this.AttackSpeed == other.AttackSpeed;
    }

    public override int GetHashCode()
    {
        return this.InternalLevel ^ this.Attack ^ this.AttackSpeed.GetHashCode();
    }
}

[DataContract]
public class DefenseWeaponInfo : WeaponInfo, IEquatable<DefenseWeaponInfo>
{
    public DefenseWeaponInfo(int internalLevel, int attack, decimal attackSpeed, int defense)
        : base(internalLevel, attack, attackSpeed)
    {
        this.Defense = defense;
    }


    [DataMember]
    public int Defense { get; private set; }


    public override T Match<T>(Func<EquipmentInfo, T> f1, Func<WeaponInfo, T> f2, Func<DefenseWeaponInfo, T> f3)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2)); 
        if (f3 == null) throw new ArgumentNullException(nameof(f3));

        return f3(this);
    }


    public override bool Equals(EquipmentBaseInfo other)
    {
        return this.Equals(other as DefenseWeaponInfo);
    }

    public bool Equals(DefenseWeaponInfo other)
    {
        return base.Equals(other) && this.Defense == other.Defense;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Defense;
    }
}