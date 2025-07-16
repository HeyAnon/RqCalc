using Framework.DataBase;

using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Application;

public class AuraServiceFactory(IDataSource<IPersistentDomainObjectBase> dataSource) : IAuraServiceFactory
{
    public IAuraService Create(IVersion version) => new AuraService(dataSource, version);
}
