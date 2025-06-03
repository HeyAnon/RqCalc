using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent;

public interface IClassLevelHpBonus : IPersistentDomainObjectBase, IClassObject, IValueObject<int>, ILevelObject
{

}