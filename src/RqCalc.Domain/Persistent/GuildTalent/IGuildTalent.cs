using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.GuildTalent;

public interface IGuildTalent : IImageDirectoryBase, IDescriptionObject, IActiveObject, IBonusContainer<IGuildTalentBonus>, IOrderObject<int>
{
    IEnumerable<IGuildTalentVariable> Variables { get; }

    IGuildTalentBranch Branch { get; }

    IImage GrayImage { get; }
}