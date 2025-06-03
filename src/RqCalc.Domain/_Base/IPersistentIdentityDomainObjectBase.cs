using Framework.Persistent;

namespace RqCalc.Domain._Base;

public interface IPersistentIdentityDomainObjectBase : IPersistentDomainObjectBase, IIdentityObject<int>;