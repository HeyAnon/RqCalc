using System.Collections;

using RqCalc.Application._Extensions;

namespace RqCalc.Application.IndexedDict;

public class NullableIndexedDict<T>(IIndexedDict<T> baseDict) : IIndexedDict<T?>
    where T : class
{
    private const int NullOffset = 1;

    public int BitSize { get; } = (baseDict.Count + NullOffset - 1).GetBitSize();

    public int Count => baseDict.Count + NullOffset;

    public T? this[int index]
    {
        get
        {
            if (index == 0)
            {
                return null;
            }
            else
            {
                return baseDict[index - NullOffset];
            }
        }
    }

    public int this[T? value]
    {
        get
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return baseDict[value] + NullOffset;
            }
        }
    }

    public IEnumerator<T> GetEnumerator() => baseDict.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}