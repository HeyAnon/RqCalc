using Framework.Persistent;
using RqCalc.Domain._Base;

namespace RqCalc.Domain;

public interface IAuraBonus : IPersistentDomainObjectBase, IBonus, ISharedContainer, IValueObject<decimal>, IVersionObject;