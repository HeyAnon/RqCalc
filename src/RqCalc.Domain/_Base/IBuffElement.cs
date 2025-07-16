using Framework.Persistent;

namespace RqCalc.Domain._Base;

public interface IBuffDescriptionElement : IValueObject<decimal?>, IOrderObject<int>
{
    IBuffDescription Description { get; }
}