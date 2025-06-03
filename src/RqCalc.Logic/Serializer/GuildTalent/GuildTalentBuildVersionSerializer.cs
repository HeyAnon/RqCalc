using RqCalc.Domain.Model;
using RqCalc.Domain.Persistent;
using RqCalc.Domain.Persistent.GuildTalent;
using RqCalc.Logic.Serializer._Internal;

namespace RqCalc.Logic.Serializer.GuildTalent;

internal class GuildTalentBuildVersionSerializer
{
    private readonly ApplicationContext _context;

    private readonly IVersion _version;

    private readonly IReadOnlyCollection<IGuildTalentBranch> _guildBranches;

    private readonly int _guildbranchCount;

    private readonly int _guildBranchSize;

    public GuildTalentBuildVersionSerializer(ApplicationContext context, IVersion version)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        this._version = version ?? throw new ArgumentNullException(nameof(version));

        this._guildBranches = this._context.DataSource.GetFullList<IGuildTalentBranch>();

        this._guildbranchCount = this._guildBranches.Count;
        this._guildBranchSize = this._guildBranches.Select(b => b.Talents.Count()).Distinct().Single();
    }


    public void FullFormat(BitWriter writer, IGuildTalentBuildSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));

        writer.WriteByMax(this._version.Id, byte.MaxValue);

        this.Format(writer, character);
    }

    public void Format(BitWriter writer, IGuildTalentBuildSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));

        var datas = character.GuildTalents.Where(tal => tal.Value != 0)
            .OrderBy(tal => tal.Key.Branch.Id)
            .Reverse()
            .Select((pair, index) => new { Talent = pair.Key, Points = pair.Value, IsLast = index == 0 })
            .Reverse()
            .ToArray();

        writer.WriteByMax(datas.Length, this._guildbranchCount);

        foreach (var data in datas)
        {
            writer.WriteByMax(data.Talent.OrderIndex, this._guildBranchSize - 1);

            if (data.IsLast)
            {
                writer.WriteByMax(data.Points - 1, data.Talent.Branch.MaxPoints - 1);
            }
        }
    }

    public IGuildTalentBuildSource Parse(BitReader reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));
            
        var talents = this.ReadGuildTalents(reader).ToList();

        return new GuildTalentBuildSource
        {
            GuildTalents = talents.ToDictionary()
        };
    }


    private IEnumerable<KeyValuePair<IGuildTalent, int>> ReadGuildTalents(BitReader reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));

        var branchCount = reader.ReadByMax(this._guildbranchCount);

        foreach (var pair in this._guildBranches.Select((branch, index) => new { Branch = branch, Index = index }).Take(branchCount))
        {
            var talentOrderIndex = reader.ReadByMax(this._guildBranchSize - 1);

            var talent = pair.Branch.Talents.Single(tal => tal.OrderIndex == talentOrderIndex);

            var isLast = pair.Index == branchCount - 1;

            var points = isLast ? reader.ReadByMax(pair.Branch.MaxPoints - 1) + 1 : pair.Branch.MaxPoints;

            yield return new KeyValuePair<IGuildTalent, int>(talent, points);
        }
    }
}