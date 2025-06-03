namespace RqCalc.Domain._Base;

public interface IImage : IPersistentIdentityDomainObjectBase
{
    //System.Data.Linq.Binary
    byte[] Data { get; }
}