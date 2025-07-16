using RqCalc.Domain;

namespace RqCalc.Application.Calculation;

public record CharacterCalculationChangedState(IClass Class, int Level, IGender Gender, WeaponInfo? CurrentWeaponInfo) : ICharacterCalculationChangedState
{
    public CharacterCalculationChangedState(ICharacterCalculationState sourceState)
        : this(sourceState.Class, sourceState.Level, sourceState.Gender, sourceState.CurrentWeaponInfo)
    {
    }

    public IReadOnlyDictionary<IStat, decimal>? Stats { get; init; }

    public decimal? CustomVariable { get; init; }

    ICharacterCalculationChangedState ICharacterCalculationChangedState.ChangeVariable(decimal variable) => this with { CustomVariable = variable };

    IReadOnlyDictionary<IStat, decimal> ICharacterCalculationChangedState.Stats
    {
        get
        {
            if (this.Stats == null)
            {
                throw new Exception("Stats not initialized");
            }
            else
            {
                return this.Stats;
            }
        }
    }

    IReadOnlyDictionary<int, decimal> ICharacterCalculationChangedState.CustomVariables
    {
        get
        {
            if (this.CustomVariable == null)
            {
                throw new Exception("CustomVariables not initialized");
            }

            return new Dictionary<int, decimal> { { 0, this.CustomVariable.Value } };
        }
    }
}