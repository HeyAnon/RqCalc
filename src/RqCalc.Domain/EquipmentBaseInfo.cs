namespace RqCalc.Domain;

public abstract record EquipmentBaseInfo(int InternalLevel);

public record EquipmentInfo(int InternalLevel, int Defense) : EquipmentBaseInfo(InternalLevel);

public record WeaponInfo(int InternalLevel, int Attack, decimal AttackSpeed) : EquipmentBaseInfo(InternalLevel);

public record DefenseWeaponInfo(int InternalLevel, int Attack, decimal AttackSpeed, int Defense) : EquipmentBaseInfo(InternalLevel);