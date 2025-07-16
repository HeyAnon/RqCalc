using System;

using Framework.Core;

using Framework.Core.Serialization;
using Framework.ExpressionParsers;

using Anon.RQ_Calc.DataBase;
using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;
using Framework.DataBase;


namespace Rq_Calc.ServiceFacade
{
    public class ServiceFacadeEnvironment : IServiceEnvironment<IDataSource<IPersistentDomainObjectBase>>
    {
        private readonly IFactory<IDataSource<IPersistentDomainObjectBase>> _dataSourceFactory;

        private readonly ITypeResolver<string> _typeResolver;


        public readonly IImageSourceService ImageSourceService;

        public readonly IApplicationContext DefaultApplicationContext;

        
        public readonly INativeExpressionParser ExpressionParser = CSharpNativeExpressionParser.Compile.WithCache();
        

        public ServiceFacadeEnvironment(IFactory<IDataSource<IPersistentDomainObjectBase>> dataSourceFactory, ITypeResolver<string> typeResolver)
        {
            if (dataSourceFactory == null) throw new ArgumentNullException(nameof(dataSourceFactory));
            if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));

            this._dataSourceFactory = dataSourceFactory;
            
            this._typeResolver = typeResolver;
            
            {
                using (var dataSource = this._dataSourceFactory.Create())
                {
                    var memDataSource = dataSource.LoadToMemory(MemoryTypeCache.Default);

                    this.ImageSourceService = new PreLoadedImageSourceService(memDataSource);

                    this.DefaultApplicationContext = this.GetApplicationContext(memDataSource);
                }
            }
        }



        public IApplicationContext GetApplicationContext(IDataSource<IPersistentDomainObjectBase> dataSource)
        {
            return new ApplicationContext(dataSource, this.ExpressionParser, Serializer.Base64, this._typeResolver, this.ImageSourceService);
        }

        //public TResult EvaluateDB<TResult>(Func<IDataSource<IPersistentDomainObjectBase>, TResult> func)
        //{
        //    using (var dataSource = this._dataSourceFactory.Create(false))
        //    {
        //        return func(dataSource);
        //    }
        //}
        

        private static readonly object ConfigurationInstanceLocker = new object();

        private static ServiceFacadeEnvironment _configuration;


        public static ServiceFacadeEnvironment Configuration
        {
            get
            {
                lock (ConfigurationInstanceLocker)
                {
                    if (_configuration == null)
                    {
                        _configuration = new ServiceFacadeEnvironment(DataSourceFactory.Settings, DomainTypeResolver.Default);
                    }

                    return _configuration;
                }
            }
        }
    }

    public interface IServiceEnvironment<out TDataSource>
    {
        //TResult EvaluateDB<TResult>(Func<TDataSource, TResult> func);
    }
}