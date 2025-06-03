using RqCalc.Core;
using RqCalc.Domain;

namespace RqCalc.Logic.Calc;

internal class CalcState : ICalcState
{
    public CalcState(ICalcState state)
        : this((ICharacterCalc)state)
    {

    }

    public CalcState(ICharacterCalc characterCalc)
        : this()
    {
        if (characterCalc == null) throw new ArgumentNullException(nameof(characterCalc));

        this.Class = characterCalc.Class;
        this.Level = characterCalc.Level;
        this.Gender = characterCalc.Gender;
        this.CurrentWeaponInfo = characterCalc.CurrentWeaponInfo;
    }

    private CalcState()
    {
    }


    public IClass Class { get; set; }

    public int Level { get; set; }

    public IGender Gender { get; set; }

    public WeaponInfo CurrentWeaponInfo { get; set; }


    public IReadOnlyDictionary<IStat, decimal> Stats { get; set; }

    public Dictionary<int, decimal> CustomVariables { get; set; }

    public ICalcState ChangeVariable(decimal variable)
    {
        return new CalcState(this)
        {
            Stats = this.Stats,

            CustomVariables = new Dictionary<int, decimal> {{0, variable} }
        };
    }


    IReadOnlyDictionary<IStat, decimal> ICalcState.Stats
    {
        get
        {
            if (this.Stats == null)
            {
                throw new Exception("Stats not initialized");
            }

            return this.Stats;
        }
    }

    IReadOnlyDictionary<int, decimal> ICalcState.CustomVariables
    {
        get
        {
            if (this.CustomVariables == null)
            {
                throw new Exception("CustomVariables not initialized");
            }
                
            return this.CustomVariables;
        }
    }
}