using RqCalc.Domain;

namespace RqCalc.Application.Calc;

public record ChangedCharacterCalculationState : ICharacterCalculationState
{
    public ChangedCharacterCalculationState(ICharacterCalculationStateBase characterCalculationStateBase)
    {
        this.Class = characterCalculationStateBase.Class;
        this.Level = characterCalculationStateBase.Level;
        this.Gender = characterCalculationStateBase.Gender;
        this.CurrentWeaponInfo = characterCalculationStateBase.CurrentWeaponInfo;
    }


    public IClass Class { get; }

    public int Level { get; }

    public IGender Gender { get; }

    public WeaponInfo? CurrentWeaponInfo { get; }

    public IReadOnlyDictionary<IStat, decimal> Stats { get; set; }

    public Dictionary<int, decimal> CustomVariables { get; set; }

    public ICharacterCalculationState ChangeVariable(decimal variable)
    {
        return new CalcState(this)
        {
            Stats = this.Stats,

            CustomVariables = new Dictionary<int, decimal> {{0, variable} }
        };
    }


    IReadOnlyDictionary<IStat, decimal> ICharacterCalculationState.Stats
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

    IReadOnlyDictionary<int, decimal> ICharacterCalculationState.CustomVariables
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