using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Anon.RQ_Calc.Domain;

using Framework.Core;
using Framework.DataBase;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public partial class RQDBContext : IDataSource<IPersistentDomainObjectBase>
    {
        IReadOnlyCollection<TProjection> IDataSource<IPersistentDomainObjectBase>.GetFullList<TProjection>()
        {
            var implType = this._implementTypeResolver.Resolve(typeof(TProjection));
            
            return new Func<IReadOnlyCollection<object>>(this.GetFullListInternal<object>)
                  .CreateGenericMethod(implType)
                  .Invoke<IReadOnlyList<TProjection>>(this);
        }

        IQueryable<TProjection> IDataSource<IPersistentDomainObjectBase>.GetQueryable<TProjection>()
        {
            var implType = this._implementTypeResolver.Resolve(typeof(TProjection));

            return new Func<IQueryable<object>>(this.GetQueryableInternal<object>)
                  .CreateGenericMethod(implType)
                  .Invoke<IQueryable<TProjection>>(this);
        }

        private IReadOnlyCollection<TImplType> GetFullListInternal<TImplType>()
            where TImplType : class
        {
            return this.Set<TImplType>().ToReadOnlyCollection();
        }

        private IQueryable<TImplType> GetQueryableInternal<TImplType>()
            where TImplType : class
        {
            return this.Set<TImplType>();
        }
    }
}