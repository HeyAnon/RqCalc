using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models._Base;

public class ActiveImageChangeModelCollection<T, TItem> : ActiveImageChangeModelItem<T, IReadOnlyList<TItem>>
    where T : class, IImageObject
{
    public ActiveImageChangeModelCollection(IImage defaultImage = null)
        : base(context, defaultImage)
    {
        this.Value = new TItem[0];
    }
}