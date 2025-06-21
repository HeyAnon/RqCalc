using Framework.Core;

namespace Framework.Reactive.ObservableRecurse
{
    public class RefWrapper<T> : IEquatable<RefWrapper<T>>
        where T : class 
    {
        public readonly T Item;


        public RefWrapper (T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException ("item");
            }


            this.Item = item;
        }


        public bool Equals (RefWrapper<T> other)
        {
            return other.Maybe (v => object.ReferenceEquals (this.Item, v.Item));
        }

        public override int GetHashCode ()
        {
            return 0;
        }
    }
}