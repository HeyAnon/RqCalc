using System.Collections;
using Framework.Persistent;

namespace RqCalc.Logic.Serializer._Internal;

internal interface IIndexedDict<T> : IEnumerable<T>
{
    int BitSize { get; }

    int Count { get; }

    T this[int index] { get; }

    int this[T value] { get; }
}

internal static class IndexedDict
{
    public static IIndexedDict<T> Create<T>(IEnumerable<T> source, bool allowNull)
        where T : class, IIdentityObject<int>
    {
        return Create(source, allowNull, v => v.Id);
    }

    public static IIndexedDict<T> Create<T, TKey>(IEnumerable<T> source, bool allowNull, Func<T, TKey> orderSelector)
        where T : class
    {
        return new Impl<T>(source.OrderBy(orderSelector), allowNull);
    }


    private class Impl<T> : IIndexedDict<T>
        where T : class
    {
        private readonly bool _allowNull;

        private readonly T[] _source;

        private readonly IReadOnlyDictionary<T, int> _reverseDict;


        internal Impl(IEnumerable<T> orderedSource, bool allowNull)
        {
            if (orderedSource == null) throw new ArgumentNullException(nameof(orderedSource));
                
            this._source = orderedSource.ToArray();
            this._allowNull = allowNull;
                
            this.BitSize = (this.Count - 1).GetBitSize();

            this._reverseDict = this._source.Select((v, index) => (v, index)).ToDictionary();
        }


        public int BitSize { get; }

        public int Count => this._source.Length + this.NullOffset;

        public T this[int index]
        {
            get
            {
                if (index == 0 && this._allowNull)
                {
                    return null;
                }
                else
                {
                    return this._source[index - this.NullOffset];
                }
            }
        }

        public int this[T value]
        {
            get
            {
                if (value == null && this._allowNull)
                {
                    return 0;
                }
                else
                {
                    return this._reverseDict[value] + this.NullOffset;
                }
            }
        }

        private int NullOffset => this._allowNull ? 1 : 0;

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this._source).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}