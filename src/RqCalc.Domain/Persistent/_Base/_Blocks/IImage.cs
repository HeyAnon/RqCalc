namespace RqCalc.Domain.Persistent._Base._Blocks;

public interface IImage : IPersistentIdentityDomainObjectBase
{
    //System.Data.Linq.Binary
    byte[] Data { get; }
}