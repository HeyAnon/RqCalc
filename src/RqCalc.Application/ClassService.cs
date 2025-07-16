using Framework.Core;
using Framework.DataBase;

using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Application;

public class ClassService : IClassService
{
    private readonly Lazy<Dictionary<IClass, int[]>> lazyLevelHpBonusCache;

    public ClassService(IDataSource<IPersistentDomainObjectBase> dataSource) =>
        this.lazyLevelHpBonusCache = LazyHelper.Create(() =>
        
                                                           dataSource.GetFullList<IClass>().Where(@class => @class.Parent == null).ToDictionary(

                                                               @class => @class,

                                                               @class => new Dictionary<int, int> { { 0, 0 } }.Concat(@class.LevelHpBonuses.ToDictionary(bonus => bonus.Level, bonus => bonus.Value)).ToArrayI()));

    public int GetHpBonuses(IClass @class, int level) => this.lazyLevelHpBonusCache.Value[@class][level];
}