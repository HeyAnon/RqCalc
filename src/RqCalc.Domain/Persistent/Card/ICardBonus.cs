using Framework.Persistent;
using RqCalc.Core;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent.BonusType;

namespace RqCalc.Domain.Persistent.Card;

public interface ICardBonus : ITypeObject<IBonusType>, IOrderObject<int>, IPersistentDomainObjectBase
{
    IEnumerable<ICardBonusVariable> Variables { get; }

    ICard Card { get; }

    ICard? RequiredCard { get; }

    ICard? NegateCard { get; }

    ICard? MultiplyEffectCard { get; }

    ICardSet? RequiredSet { get; }

    int RequiredSetSize { get; }


    CardUpgradeEquipmentInfo? UpgradeEquipmentInfo { get; }
}