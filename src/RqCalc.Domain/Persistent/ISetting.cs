using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent;

public interface ISetting : IPersistentDomainObjectBase, IValueObject<string>
{
    string Key { get; }
}