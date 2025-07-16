using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain;

public interface ISetting : IPersistentDomainObjectBase, IValueObject<string>
{
    string Key { get; }
}