using Framework.DataBase;
using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Application._Extensions;

public static class DataSourceExtensions
{
    public static T? TryGetById<T>(this IDataSource<IPersistentDomainObjectBase> dataSource, int id)
        where T : class, IPersistentDomainObjectBase, IIdentityObject<int> =>
        dataSource.GetQueryable<T>().SingleOrDefault(obj => obj.Id == id);
}