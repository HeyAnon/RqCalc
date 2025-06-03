using System;

using Framework.Core;

using Anon.RQ_Calc.DataBase;
using Anon.RQ_Calc.DataBase.EntityFramework;
using Anon.RQ_Calc.Domain;
using Framework.DataBase;

namespace Rq_Calc.ServiceFacade
{
    public class DataSourceFactory : FuncFactory<IDataSource<IPersistentDomainObjectBase>>
    {
        public DataSourceFactory(Func<IDataSource<IPersistentDomainObjectBase>> createFunc)
            : base(createFunc)
        {
        }


        public static readonly DataSourceFactory Settings =

            Properties.Settings.Default.ConnectionString.Pipe(connectionString =>
            {
                AppDomain.CurrentDomain.RelativeSearchPath.Maybe(path => Environment.CurrentDirectory = path);

                var implementTypeResolver = ImplementTypeResolver.Default.WithCache().WithLock();

                return new DataSourceFactory(() =>
                {
                    var connection = new System.Data.SQLite.SQLiteConnection(connectionString);
                    
                    return new RQDBContext(connection, true, implementTypeResolver);
                });
            });
    }
}