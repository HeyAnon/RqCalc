using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models._Base;

public class ActiveImageChangeModelDictionary<T, TKey, TValue> : ActiveImageChangeModelItem<T, IReadOnlyDictionary<TKey, TValue>>
    where T : class, IImageObject
    where TKey : notnull
{
    public ActiveImageChangeModelDictionary(IImage? defaultImage = null)
        : base(defaultImage) =>
        this.Value = new Dictionary<TKey, TValue>();
}