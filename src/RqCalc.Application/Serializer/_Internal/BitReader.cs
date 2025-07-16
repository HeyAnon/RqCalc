using System.Collections;
using Framework.Core;
using RqCalc.Application._Extensions;
using RqCalc.Application.IndexedDict;
using RqCalc.Domain._Base;

namespace RqCalc.Application.Serializer._Internal;

internal class BitReader
{
    private readonly Queue<bool> bits;


    public BitReader(bool[] source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        this.bits = new Queue<bool>(source);
    }

    public BitReader(byte[] source)
    {
        var bitArray = new BitArray(source);

        var bits = new bool[source.Length * 8];

        bitArray.CopyTo(bits, 0);

        this.bits = new Queue<bool>(bits);
    }


    public bool Read() => this.bits.Dequeue();

    public int Read(int bitSize)
    {
        if (bitSize < 0) throw new ArgumentOutOfRangeException(nameof(bitSize));

        var value = 0;

        var @base = 1;

        for (var i = 0; i < bitSize; i++)
        {
            value += @base * (this.Read() ? 1 : 0);

            @base <<= 1;
        }

        return value;
    }

    public int ReadByMax(int maxValue)
    {
        if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue));

        if (maxValue == 0)
        {
            return 0;
        }

        return this.Read(maxValue.GetBitSize());
    }

    public T Read<T>(IIndexedDict<T> dict) => dict[this.Read(dict.BitSize)];

    public KeyValuePair<T, int> ReadPair<T>(IIndexedDict<T> dict)
        where T : IStackObject
    {
        var key = this.Read(dict);

        return new KeyValuePair<T, int>(key, this.ReadByMax(key.MaxStackCount));
    }

    public Dictionary<T, int> ReadDict<T>(IIndexedDict<T> dict)
        where T : IStackObject
    {
        var count = this.ReadByMax(dict.Count);

        return Enumerable.Range(0, count).Select(_ => this.ReadPair(dict)).ToDictionary();
    }

    public List<T> ReadList<T>(IIndexedDict<T> dict, int? maxCount = null)
    {
        var count = this.ReadByMax(maxCount ?? dict.Count);

        return Enumerable.Range(0, count).ToList(_ => this.Read(dict));
    }

    public List<T> ReadListOptimize<T>(IIndexedDict<T> dict)
    {
        var mode = this.ReadDictMode();

        switch (mode)
        {
            case IndexDictCollectionMode.Empty:
                return [];

            case IndexDictCollectionMode.Full:
                return dict.ToList();

            case IndexDictCollectionMode.HalfOrLess:
                return this.ReadList(dict, dict.Count / 2);

            case IndexDictCollectionMode.MoreHalf:
            {
                var missedItems = this.ReadList(dict, (dict.Count + 1) / 2);

                return dict.Except(missedItems).ToList();
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }

    private IndexDictCollectionMode ReadDictMode() => (IndexDictCollectionMode)this.ReadByMax(3);
}