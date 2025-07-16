using Framework.DataBase;

using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Application;

public class LastVersionService(IDataSource<IPersistentDomainObjectBase> dataSource) : ILastVersionService
{

    public IVersion LastVersion { get; } = dataSource.GetFullList<IVersion>().OrderBy(ver => ver.Id).Last();
}