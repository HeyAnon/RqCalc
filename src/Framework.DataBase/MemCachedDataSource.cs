using Framework.Core;

namespace Framework.DataBase;

public class MemCachedDataSource<TPersistentDomainObjectBase> : IDataSource<TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class
{
    private readonly ITypeResolver<Type> _implementTypeResolver;

    private readonly Dictionary<Type, Array> _dict;


    public MemCachedDataSource(ITypeResolver<Type> implementTypeResolver, IEnumerable<TPersistentDomainObjectBase> source)
    {
        this._implementTypeResolver = implementTypeResolver;

        this._dict = source.GroupBy(obj => obj.GetType()).ToDictionary(pair => pair.Key, pair => pair.ToArray(pair.Key));
    }


    public IReadOnlyCollection<TDomainObject> GetFullList<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var implType = this._implementTypeResolver.Resolve(typeof(TDomainObject), true);

        return (IReadOnlyCollection<TDomainObject>)this._dict[implType];
    }

    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.GetFullList<TDomainObject>().AsQueryable();
    }


    void IDisposable.Dispose()
    {
    }
}