namespace RqCalc.Logic.Serializer._Internal;

internal static class SerializeExtensions
{
    public static int GetBitSize(this int value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));

        return (int)Math.Ceiling(Math.Log(value + 1, 2));
    }
}