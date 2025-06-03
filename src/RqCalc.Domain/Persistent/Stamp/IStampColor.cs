using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Stamp;

public interface IStampColor : IImageDirectoryBase
{
    string Argb { get; }

    IImage BigImage { get; }
}