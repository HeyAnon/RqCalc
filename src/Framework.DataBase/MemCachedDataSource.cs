using Framework.Core;

namespace Framework.DataBase;

public class MemCachedDataSource<TPersistentDomainObjectBase> : IDataSource<TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class
{
    private readonly ITypeResolver<Type> implementTypeResolver;

    private readonly Dictionary<Type, Array> dict;


    public MemCachedDataSource(ITypeResolver<Type> implementTypeResolver, IEnumerable<TPersistentDomainObjectBase> source)
    {
        this.implementTypeResolver = implementTypeResolver;

        this.dict = source.GroupBy(obj => obj.GetType()).ToDictionary(pair => pair.Key, pair => pair.ToArray(pair.Key));
    }


    public IReadOnlyCollection<TDomainObject> GetFullList<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var implType = this.implementTypeResolver.Resolve(typeof(TDomainObject), true);

        return (IReadOnlyCollection<TDomainObject>)this.dict[implType];
    }

    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase =>
        this.GetFullList<TDomainObject>().AsQueryable();

    void IDisposable.Dispose()
    {
    }
}