using RqCalc.Domain.BonusType;

namespace RqCalc.Application;

public interface IBonusTypeService
{
    IReadOnlyList<IBonusType> AttackBonusTypes { get; }

    IReadOnlyDictionary<IBonusType, int> BonusTypePriority { get; }
}