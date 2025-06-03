namespace Framework.DataBase;

public interface IDataSource<in TPersistentDomainObjectBase> : IDisposable
    where TPersistentDomainObjectBase : class
{
    IReadOnlyCollection<TDomainObject> GetFullList<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase;

    IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase;
}