using RqCalc.Domain._Base;

namespace RqCalc.Domain.Stamp;

public interface IStampColor : IImageDirectoryBase
{
    string Argb { get; }

    IImage BigImage { get; }
}