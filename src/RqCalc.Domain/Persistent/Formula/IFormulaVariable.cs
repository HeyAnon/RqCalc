using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Formula;

public interface IFormulaVariable : IPersistentDomainObjectBase, ITypeObject<FormulaVariableType>, IIndexObject
{
    IFormula Formula { get; }

    IStat TypeStat { get; }
}


public enum FormulaVariableType
{
    Decimal = 0,

    Int32 = 1,
        
    Level = 2, // Int32

    MaxLevel = 3, // Int32

    Stat = 4, // Decimal

    CurrentWeaponInfo = 5,
        
    LevelMultiplicity = 7, // Int32

    HpPerVitality = 8, // Decimal

    LevelDef = 9,

    LevelAttack = 10,

    StatDescription = 11 // Decimal
}