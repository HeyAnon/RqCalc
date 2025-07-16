using System.Collections;
using System.ComponentModel;

using Framework.Core;

using RqCalc.Application._Extensions;
using RqCalc.Application.IndexedDict;
using RqCalc.Domain._Base;

namespace RqCalc.Application.Serializer._Internal;

internal class BitWriter
{
    private readonly List<bool> bits = [];


    public BitWriter()
    {
    }


    public void Write(bool value) => this.bits.Add(value);

    public void Write(params bool[] values)
    {
        if (values == null) throw new ArgumentNullException(nameof(values));

        this.bits.AddRange(values);
    }

    public void Write(int value, int bitSize)
    {
        if (bitSize < 0) throw new ArgumentOutOfRangeException(nameof(bitSize));
        if (value < 0 || value >= (1 << bitSize)) throw new ArgumentOutOfRangeException(nameof(value));

        for (var i = 0; i < bitSize; i++)
        {
            this.Write(value % 2 == 1);
            value >>= 1;
        }
    }

    public void WriteByMax(int value, int maxValue)
    {
        if (value < 0 || value > maxValue) throw new ArgumentOutOfRangeException(nameof(value));

        if (maxValue == 0)
        {
            if (value == 0)
            {
                return;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        this.Write(value, maxValue.GetBitSize());
    }

    public void Write<T>(T value, IIndexedDict<T> dict) => this.Write(dict[value], dict.BitSize);

    public void Write<T>(KeyValuePair<T, int> pair, IIndexedDict<T> dict)
        where T : IStackObject
    {
        this.Write(pair.Key, dict);

        this.WriteByMax(pair.Value, pair.Key.MaxStackCount);
    }

    public void Write<T>(IReadOnlyDictionary<T, int> source, IIndexedDict<T> dict)
        where T : IStackObject
    {
        this.WriteByMax(source.Count, dict.Count);

        source.Foreach(pair => this.Write(pair, dict));
    }

    public void Write<T>(IReadOnlyCollection<T> source, IIndexedDict<T> dict, int? maxCount = null)
    {
        this.WriteByMax(source.Count, maxCount ?? dict.Count);

        source.Foreach(element => this.Write(element, dict));
    }

    public void WriteOptimize<T>(IReadOnlyCollection<T> source, IIndexedDict<T> dict)
    {
        var mode = this.GetCollectionMode(source, dict);

        this.WriteMode(mode);

        switch (mode)
        {
            case IndexDictCollectionMode.Empty:
            case IndexDictCollectionMode.Full:
                return;

            case IndexDictCollectionMode.HalfOrLess:
            {
                this.Write(source, dict, dict.Count / 2);

                break;
            }

            case IndexDictCollectionMode.MoreHalf:
            {
                var missedItems = dict.Except(source).ToList();

                this.Write(missedItems, dict, (dict.Count + 1) / 2);

                break;
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }

    private void WriteMode(IndexDictCollectionMode mode)
    {
        if (!Enum.IsDefined(typeof(IndexDictCollectionMode), mode)) throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(IndexDictCollectionMode));

        this.WriteByMax((int)mode, 3);
    }

    private IndexDictCollectionMode GetCollectionMode<T>(IReadOnlyCollection<T> source, IIndexedDict<T> dict)
    {
        if (source.Count == 0)
        {
            return IndexDictCollectionMode.Empty;
        }
        else if (source.Count == dict.Count)
        {
            return IndexDictCollectionMode.Full;
        }
        else if (source.Count <= dict.Count / 2)
        {
            return IndexDictCollectionMode.HalfOrLess;
        }
        else
        {
            return IndexDictCollectionMode.MoreHalf;
        }
    }

    public bool[] GetBits() => this.bits.ToArray();

    public byte[] GetBytes()
    {
        var bitArray = new BitArray(this.GetBits());

        var bytes = new byte[(int)Math.Ceiling((decimal)bitArray.Length / 8)];

        bitArray.CopyTo(bytes, 0);

        return bytes;
    }
}