namespace RqCalc.Domain.Persistent._Base._Blocks;

public interface IVersionObject
{
    IVersion StartVersion { get; }

    IVersion EndVersion { get; }
}