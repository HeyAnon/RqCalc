using Framework.Persistent;

using RqCalc.Domain._Base;
using RqCalc.Domain.Card;

namespace RqCalc.Domain.Equipment;

public interface IEquipment : IImageDirectoryBase, ILevelObject, ITypeObject<IEquipmentType>, IBonusContainer<IEquipmentBonus>, IVersionObject
{
    IReadOnlyCollection<IEquipmentCondition> Conditions { get; }
        
    IGender? Gender { get; }

    EquipmentBaseInfo? Info { get; }

    IImage? CostumeImage { get; }

    ICard? PrimaryCard { get; }

    bool IsCostume { get; }

    bool IsPersonal { get; }
}