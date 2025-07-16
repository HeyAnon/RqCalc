using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain.GuildTalent;

public interface IGuildTalent : IImageDirectoryBase, IDescriptionObject, IActiveObject, IBonusContainer<IGuildTalentBonus>, IOrderObject<int>
{
    IReadOnlyCollection<IGuildTalentVariable> Variables { get; }

    IGuildTalentBranch Branch { get; }

    IImage GrayImage { get; }
}