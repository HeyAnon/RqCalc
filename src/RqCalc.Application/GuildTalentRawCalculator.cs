using Framework.DataBase;

using RqCalc.Domain._Base;
using RqCalc.Domain.GuildTalent;
using RqCalc.Model;

namespace RqCalc.Application;

public class GuildTalentRawCalculator(IDataSource<IPersistentDomainObjectBase> dataSource) : IGuildTalentCalculator
{
    public const string Key = "Raw";

    public int GetFreeGuildTalents(IGuildTalentBuildSource source)
    {
        var pointLimit = dataSource.GetFullList<IGuildTalentBranch>().Sum(b => b.MaxPoints);

        var usedPoints = source.GuildTalents.Sum(tal => tal.Value);

        return pointLimit - usedPoints;
    }
}