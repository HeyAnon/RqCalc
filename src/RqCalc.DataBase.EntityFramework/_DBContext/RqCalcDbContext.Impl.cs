using Framework.Core;
using Framework.DataBase;

using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework._DBContext;

public partial class RqCalcDbContext : IDataSource<IPersistentDomainObjectBase>
{
    IReadOnlyCollection<TProjection> IDataSource<IPersistentDomainObjectBase>.GetFullList<TProjection>()
    {
        var implType = implementTypeResolver.Resolve(typeof(TProjection));
            
        return new Func<IReadOnlyCollection<object>>(this.GetFullListInternal<object>)
            .CreateGenericMethod(implType)
            .Invoke<IReadOnlyCollection<TProjection>>(this);
    }

    IQueryable<TProjection> IDataSource<IPersistentDomainObjectBase>.GetQueryable<TProjection>()
    {
        var implType = implementTypeResolver.Resolve(typeof(TProjection));

        return new Func<IQueryable<object>>(this.GetQueryableInternal<object>)
            .CreateGenericMethod(implType)
            .Invoke<IQueryable<TProjection>>(this);
    }

    private IReadOnlyCollection<TImplType> GetFullListInternal<TImplType>()
        where TImplType : class =>
        this.Set<TImplType>().ToReadOnlyCollection();

    private IQueryable<TImplType> GetQueryableInternal<TImplType>()
        where TImplType : class =>
        this.Set<TImplType>();
}