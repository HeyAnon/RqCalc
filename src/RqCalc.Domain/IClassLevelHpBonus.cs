using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain;

public interface IClassLevelHpBonus : IPersistentDomainObjectBase, IValueObject<int>, ILevelObject
{
    IClass Class { get; }
}