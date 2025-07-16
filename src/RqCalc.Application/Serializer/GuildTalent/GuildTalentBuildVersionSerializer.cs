using Framework.DataBase;

using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.GuildTalent;
using RqCalc.Model;

namespace RqCalc.Application.Serializer.GuildTalent;

internal class GuildTalentBuildVersionSerializer
{
    private readonly IVersion version;

    private readonly IReadOnlyCollection<IGuildTalentBranch> guildBranches;

    private readonly int guildBranchCount;

    private readonly int guildBranchSize;

    public GuildTalentBuildVersionSerializer(IDataSource<IPersistentDomainObjectBase> dataSource, IVersion version)
    {
        this.version = version;

        this.guildBranches = dataSource.GetFullList<IGuildTalentBranch>();

        this.guildBranchCount = this.guildBranches.Count;
        this.guildBranchSize = this.guildBranches.Select(b => b.Talents.Count()).Distinct().Single();
    }


    public void FullFormat(BitWriter writer, IGuildTalentBuildSource character)
    {
        writer.WriteByMax(this.version.Id, byte.MaxValue);

        this.Format(writer, character);
    }

    public void Format(BitWriter writer, IGuildTalentBuildSource character)
    {
        var dataList = character.GuildTalents.Where(tal => tal.Value != 0)
            .OrderBy(tal => tal.Key.Branch.Id)
            .Reverse()
            .Select((pair, index) => new { Talent = pair.Key, Points = pair.Value, IsLast = index == 0 })
            .Reverse()
            .ToArray();

        writer.WriteByMax(dataList.Length, this.guildBranchCount);

        foreach (var data in dataList)
        {
            writer.WriteByMax(data.Talent.OrderIndex, this.guildBranchSize - 1);

            if (data.IsLast)
            {
                writer.WriteByMax(data.Points - 1, data.Talent.Branch.MaxPoints - 1);
            }
        }
    }

    public IGuildTalentBuildSource Parse(BitReader reader)
    {
        var talents = this.ReadGuildTalents(reader).ToList();

        return new GuildTalentBuildSource
        {
            GuildTalents = talents.ToDictionary()
        };
    }


    private IEnumerable<KeyValuePair<IGuildTalent, int>> ReadGuildTalents(BitReader reader)
    {
        var branchCount = reader.ReadByMax(this.guildBranchCount);

        foreach (var pair in this.guildBranches.Select((branch, index) => new { Branch = branch, Index = index }).Take(branchCount))
        {
            var talentOrderIndex = reader.ReadByMax(this.guildBranchSize - 1);

            var talent = pair.Branch.Talents.Single(tal => tal.OrderIndex == talentOrderIndex);

            var isLast = pair.Index == branchCount - 1;

            var points = isLast ? reader.ReadByMax(pair.Branch.MaxPoints - 1) + 1 : pair.Branch.MaxPoints;

            yield return new KeyValuePair<IGuildTalent, int>(talent, points);
        }
    }
}