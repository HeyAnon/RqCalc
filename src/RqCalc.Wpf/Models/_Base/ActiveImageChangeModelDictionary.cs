using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models._Base
{
    public class ActiveImageChangeModelDictionary<T, TKey, TValue> : ActiveImageChangeModelItem<T, IReadOnlyDictionary<TKey, TValue>>
        where T : class, IImageObject
    {
        public ActiveImageChangeModelDictionary(IServiceProvider context, IImage? defaultImage = null) 
            : base(context, defaultImage)
        {
            this.Value = new Dictionary<TKey, TValue>();
        }
    }
}