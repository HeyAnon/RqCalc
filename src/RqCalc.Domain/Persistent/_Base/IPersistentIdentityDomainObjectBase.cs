using Framework.Persistent;

namespace RqCalc.Domain.Persistent._Base;

public interface IPersistentIdentityDomainObjectBase : IPersistentDomainObjectBase, IIdentityObject<int>
{
        
}