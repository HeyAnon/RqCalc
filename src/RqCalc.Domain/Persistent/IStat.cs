using Framework.Persistent;
using RqCalc.Core;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Formula;

namespace RqCalc.Domain.Persistent;

public interface IStat : IImageDirectoryBase, IBonusContainer<IStatBonus>, IParentSource<IStat>, IOrderObject<int>
{
    IRace Race { get; }

    IElement Element { get; }


    IReadOnlyDictionary<RestoreStatType, IStat> RestoreStats { get; }

    IEnumerable<IFormula> Sources { get; }
        
    IFormula DescriptionFormula { get; }


    StatType Type { get; }


    int? RoundDigits { get; }

    bool IsEditable { get; }

    decimal DefaultValue { get; }

    bool IsPercent { get; }

    bool? IsMelee { get; }

    string ProgressName { get; }


    string DescriptionTemplate { get; }
}