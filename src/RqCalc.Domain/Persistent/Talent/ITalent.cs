using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Talent;

public interface ITalent : IImageDirectoryBase, IDescriptionObject, IActiveObject, IBonusContainer<ITalentBonus>
{
    IEnumerable<ITalentVariable> Variables { get; }

    IEnumerable<ITalentBuffDescription> BuffDescriptions { get; }


    ITalentBranch Branch { get; }

    IImage GrayImage { get; }

    int VIndex { get; }

    int HIndex { get; }

    string PassiveDescription { get; }

    string EquipmentCondition { get; }
}