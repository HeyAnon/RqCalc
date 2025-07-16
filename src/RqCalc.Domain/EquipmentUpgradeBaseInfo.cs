namespace RqCalc.Domain;

public abstract record EquipmentUpgradeBaseInfo(int AllStatBonus)
{
    public abstract bool IsEmpty { get; }
}

public record EquipmentUpgradeInfo(int AllStatBonus, int Defense, int Hp) : EquipmentUpgradeBaseInfo(AllStatBonus)
{
    public override bool IsEmpty => this.Defense == 0 && this.Hp == 0;
}

public record WeaponUpgradeInfo(int AllStatBonus, int Attack) : EquipmentUpgradeBaseInfo(AllStatBonus)
{
    public override bool IsEmpty => this.Attack == 0;
}

public record DefenseWeaponUpgradeInfo(int AllStatBonus, int Attack, int Defense) : WeaponUpgradeInfo(AllStatBonus, Attack)
{
    public override bool IsEmpty => base.IsEmpty && this.Defense == 0;
}