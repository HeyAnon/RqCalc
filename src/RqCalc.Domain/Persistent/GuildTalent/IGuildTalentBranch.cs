using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent.GuildTalent;

public interface IGuildTalentBranch : IDirectoryBase
{
    IEnumerable<IGuildTalent> Talents { get; }

    int MaxPoints { get; }
}