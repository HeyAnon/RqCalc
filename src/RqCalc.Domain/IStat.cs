using Framework.Persistent;

using RqCalc.Domain._Base;
using RqCalc.Domain.Formula;

namespace RqCalc.Domain;

public interface IStat : IImageDirectoryBase, IBonusContainer<IStatBonus>, IParentSource<IStat>, IOrderObject<int>
{
    IRace? Race { get; }

    IElement? Element { get; }


    IReadOnlyDictionary<RestoreStatType, IStat> RestoreStats { get; }

    IReadOnlyCollection<IFormula> Sources { get; }
        
    IFormula? DescriptionFormula { get; }


    StatType Type { get; }


    int? RoundDigits { get; }

    bool IsEditable { get; }

    decimal DefaultValue { get; }

    bool IsPercent { get; }

    bool? IsMelee { get; }

    string? ProgressName { get; }


    string? DescriptionTemplate { get; }
}