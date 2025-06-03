using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
[KnownType(typeof(EquipmentUpgradeInfo))]
[KnownType(typeof(WeaponUpgradeInfo))]
[KnownType(typeof(DefenseWeaponUpgradeInfo))]
public abstract class EquipmentUpgradeBaseInfo : IEquatable<EquipmentUpgradeBaseInfo>
{
    protected EquipmentUpgradeBaseInfo(int allStatBonus)
    {
        this.AllStatBonus = allStatBonus;
    }


    public abstract bool IsEmpty { get; }

    public int AllStatBonus { get; private set; }


    public void Match(Action<EquipmentUpgradeInfo> a1, Action<WeaponUpgradeInfo> a2, Action<DefenseWeaponUpgradeInfo> a3)
    {
        if (a1 == null) throw new ArgumentNullException(nameof(a1));
        if (a2 == null) throw new ArgumentNullException(nameof(a2));

        this.Match(e => { a1(e); return default(object); }, 
            e => { a2(e); return default(object); },
            e => { a3(e); return default(object); });
    }

    public abstract T Match<T>(Func<EquipmentUpgradeInfo, T> f1, Func<WeaponUpgradeInfo, T> f2, Func<DefenseWeaponUpgradeInfo, T> f3);


    public abstract bool Equals(EquipmentUpgradeBaseInfo other);


    public static bool operator ==(EquipmentUpgradeBaseInfo info1, EquipmentUpgradeBaseInfo info2)
    {
        return object.ReferenceEquals(info1, info2) || (!object.ReferenceEquals(info1, null) && info1.Equals(info2));
    }

    public static bool operator !=(EquipmentUpgradeBaseInfo info1, EquipmentUpgradeBaseInfo info2)
    {
        return !(info1 == info2);
    }
}

[DataContract]
public class EquipmentUpgradeInfo : EquipmentUpgradeBaseInfo, IEquatable<EquipmentUpgradeInfo>
{
    public EquipmentUpgradeInfo(int allStatBonus, int defense, int hp)
        : base(allStatBonus)
    {
        this.Defense = defense;
        this.Hp = hp;
    }


    [DataMember]
    public int Defense { get; private set; }

    [DataMember]
    public int Hp { get; private set; }


    public override bool IsEmpty => this.Defense == 0 && this.Hp == 0;

    public override T Match<T>(Func<EquipmentUpgradeInfo, T> f1, Func<WeaponUpgradeInfo, T> f2, Func<DefenseWeaponUpgradeInfo, T> f3)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2));
        if (f3 == null) throw new ArgumentNullException(nameof(f3));

        return f1(this);
    }

    public override bool Equals(EquipmentUpgradeBaseInfo other)
    {
        return this.Equals(other as EquipmentUpgradeInfo);
    }

    public bool Equals(EquipmentUpgradeInfo other)
    {
        return !object.ReferenceEquals(other, null) && this.Defense == other.Defense && this.Hp == other.Hp;
    }

    public override int GetHashCode()
    {
        return this.Hp ^ this.Defense;
    }
}

[DataContract]
public class WeaponUpgradeInfo : EquipmentUpgradeBaseInfo, IEquatable<WeaponUpgradeInfo>
{
    public WeaponUpgradeInfo(int allStatBonus, int attack)
        : base(allStatBonus)
    {
        this.Attack = attack;
    }


    [DataMember]
    public int Attack { get; private set; }


    public override bool IsEmpty => this.Attack == 0;

    public override T Match<T>(Func<EquipmentUpgradeInfo, T> f1, Func<WeaponUpgradeInfo, T> f2, Func<DefenseWeaponUpgradeInfo, T> f3)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2));
        if (f3 == null) throw new ArgumentNullException(nameof(f3));

        return f2 (this);
    }

    public override bool Equals(EquipmentUpgradeBaseInfo other)
    {
        return this.Equals(other as WeaponUpgradeInfo);
    }

    public bool Equals(WeaponUpgradeInfo other)
    {
        return !object.ReferenceEquals(other, null) && this.Attack == other.Attack;
    }

    public override int GetHashCode()
    {
        return this.Attack;
    }
}

[DataContract]
public class DefenseWeaponUpgradeInfo : WeaponUpgradeInfo, IEquatable<DefenseWeaponUpgradeInfo>
{
    public DefenseWeaponUpgradeInfo(int allStatBonus, int attack, int defense)
        : base(allStatBonus, attack)
    {
        this.Defense = defense;
    }


    [DataMember]
    public int Defense { get; private set; }


    public override bool IsEmpty => base.IsEmpty && this.Defense == 0;

    public override T Match<T>(Func<EquipmentUpgradeInfo, T> f1, Func<WeaponUpgradeInfo, T> f2, Func<DefenseWeaponUpgradeInfo, T> f3)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2));
        if (f3 == null) throw new ArgumentNullException(nameof(f3));

        return f3(this);
    }

    public override bool Equals(EquipmentUpgradeBaseInfo other)
    {
        return this.Equals(other as WeaponUpgradeInfo);
    }

    public bool Equals(DefenseWeaponUpgradeInfo other)
    {
        return base.Equals(other) && this.Defense == other.Defense;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Defense;
    }
}