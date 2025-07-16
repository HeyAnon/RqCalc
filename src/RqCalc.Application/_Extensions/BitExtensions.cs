using System.Numerics;

namespace RqCalc.Application._Extensions;

internal static class BitExtensions
{
    public static int GetBitSize(this int value) =>
        value switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(value)),
            0 => 1,
            _ => BitOperations.Log2((uint)value) + 1
        };
}