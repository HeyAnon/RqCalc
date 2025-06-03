using RqCalc.Domain._Base;

namespace RqCalc.Domain.GuildTalent;

public interface IGuildTalentBranch : IDirectoryBase
{
    IEnumerable<IGuildTalent> Talents { get; }

    int MaxPoints { get; }
}