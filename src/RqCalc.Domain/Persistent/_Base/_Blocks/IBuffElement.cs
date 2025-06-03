using Framework.Persistent;

namespace RqCalc.Domain.Persistent._Base._Blocks;

public interface IBuffDescriptionElement : IValueObject<decimal?>, IOrderObject<int>
{
    IBuffDescription Description { get; }
}