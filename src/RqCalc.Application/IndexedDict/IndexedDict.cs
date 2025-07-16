using System.Collections;

using RqCalc.Application._Extensions;

namespace RqCalc.Application.IndexedDict;

public class IndexedDict<T> : IIndexedDict<T>
    where T : class
{
    private readonly T[] source;

    private readonly IReadOnlyDictionary<T, int> reverseDict;


    public IndexedDict(IEnumerable<T> orderedSource)
    {
        this.source = orderedSource.ToArray();

        this.BitSize = (this.Count - 1).GetBitSize();

        this.reverseDict = this.source.Select((v, index) => (v, index)).ToDictionary();
    }


    public int BitSize { get; }

    public int Count => this.source.Length;

    public T this[int index] => this.source[index];

    public int this[T value] => this.reverseDict[value];

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this.source).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}