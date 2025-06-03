using RqCalc.Domain._Extensions;
using RqCalc.Domain.Persistent.GuildTalent;

namespace RqCalc.Domain.Model;

public class GuildTalentBuildSource : IGuildTalentBuildSource, IEquatable<GuildTalentBuildSource>
{
    public Dictionary<IGuildTalent, int> GuildTalents { get; set; } = new Dictionary<IGuildTalent, int>();


    IReadOnlyDictionary<IGuildTalent, int> IGuildTalentBuildSource.GuildTalents => this.GuildTalents;



    public bool Equals(GuildTalentBuildSource other)
    {
        if (other == null)
        {
            return false;
        }

        return this.GuildTalents.OrderById().SequenceEqual(other.GuildTalents.OrderById());
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as GuildTalentBuildSource);
    }

    public override int GetHashCode()
    {
        return this.GuildTalents.Count;
    }
}