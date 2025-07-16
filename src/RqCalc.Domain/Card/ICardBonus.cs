using Framework.Persistent;

using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;

namespace RqCalc.Domain.Card;

public interface ICardBonus : ITypeObject<IBonusType>, IOrderObject<int>, IPersistentDomainObjectBase
{
    IReadOnlyCollection<ICardBonusVariable> Variables { get; }

    ICard Card { get; }

    ICard? RequiredCard { get; }

    ICard? NegateCard { get; }

    ICard? MultiplyEffectCard { get; }

    ICardSet? RequiredSet { get; }

    int RequiredSetSize { get; }


    CardUpgradeEquipmentInfo? UpgradeEquipmentInfo { get; }
}