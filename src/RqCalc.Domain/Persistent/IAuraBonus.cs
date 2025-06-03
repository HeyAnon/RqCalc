using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent;

public interface IAuraBonus : IPersistentDomainObjectBase, IBonus, ISharedContainer, IValueObject<decimal>, IVersionObject
{
        
}