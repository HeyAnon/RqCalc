namespace RqCalc.Domain._Base;

public interface IVersionObject
{
    IVersion? StartVersion { get; }

    IVersion? EndVersion { get; }
}