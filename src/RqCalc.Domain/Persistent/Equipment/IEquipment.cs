using Framework.Persistent;

using RqCalc.Core;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Card;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipment : IImageDirectoryBase, ILevelObject, ITypeObject<IEquipmentType>, IBonusContainer<IEquipmentBonus>, IVersionObject
{
    IEnumerable<IEquipmentCondition> Conditions { get; }
        
    IGender? Gender { get; }

    EquipmentBaseInfo Info { get; }

    IImage CostumeImage { get; }

    ICard PrimaryCard { get; }

    bool IsCostume { get; }

    bool IsPersonal { get; }
}