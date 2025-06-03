using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;

using Framework.Persistent;
using Framework.DataBase;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase
{
    public static class DataSourceExtensions
    {
        public static IStaticImage GetStaticImage(this IDataSource<IPersistentDomainObjectBase> dataSource, StaticImageType type)
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

            return dataSource.GetFullList<IStaticImage>().GetByName(type.ToString());
        }

        public static T GetObject<T>(this IDataSource<IPersistentDomainObjectBase> dataSource, Expression<Func<T, bool>> filter)
             where T : class, IPersistentDomainObjectBase
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return dataSource.GetQueryable<T>().Where(filter).FirstOrDefault().FromMaybe($"Object with filter \"{filter.ExpandConst()}\" not found");
        }

        public static T GetById<T>(this IDataSource<IPersistentDomainObjectBase> dataSource, int id)
             where T : class, IPersistentIdentityDomainObjectBase
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

            if (id == default(int))
            {
                return null;
            }

            return dataSource.GetObject<T>(obj => obj.Id == id);
        }
        
        public static List<T> GetObjects<T>(this IDataSource<IPersistentDomainObjectBase> dataSource, Func<T, bool> filter)
            where T : class, IPersistentDomainObjectBase
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return dataSource.GetFullList<T>().Where(filter).ToList();
        }
    }
}