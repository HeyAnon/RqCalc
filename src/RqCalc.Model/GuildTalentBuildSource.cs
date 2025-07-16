using RqCalc.Domain._Extensions;
using RqCalc.Domain.GuildTalent;

namespace RqCalc.Model;

public class GuildTalentBuildSource : IGuildTalentBuildSource, IEquatable<GuildTalentBuildSource>
{
    public Dictionary<IGuildTalent, int> GuildTalents { get; set; } = new();


    IReadOnlyDictionary<IGuildTalent, int> IGuildTalentBuildSource.GuildTalents => this.GuildTalents;



    public bool Equals(GuildTalentBuildSource other)
    {
        if (other == null)
        {
            return false;
        }

        return this.GuildTalents.OrderById().SequenceEqual(other.GuildTalents.OrderById());
    }

    public override bool Equals(object obj) => this.Equals(obj as GuildTalentBuildSource);

    public override int GetHashCode() => this.GuildTalents.Count;
}