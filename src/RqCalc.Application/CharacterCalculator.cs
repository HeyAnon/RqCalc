using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;

using RqCalc.Domain._Base;
using RqCalc.Model;
using RqCalc.Application._Extensions;
using RqCalc.Application.Calculation;
using RqCalc.Application.Settings;

namespace RqCalc.Application;

public class CharacterCalculator(
    ApplicationSettings settings,
    IDataSource<IPersistentDomainObjectBase> dataSource,
    IStatService statService,
    ICharacterValidator characterValidator,
    ISerializer<string, ICharacterSource> characterSerializer)
    : ICharacterCalculator
{
    public int GetFreeStats(ICharacterSource characterInput)
    {
        var allAvailableStats = (characterInput.Level - 1) * settings.StatsPerLevel;

        var usedStats = characterInput.EditStats.Values.Sum(v => v - 1);
        
        return allAvailableStats - usedStats;
    }


    public CharacterCalculationResult Calculate(ICharacterSource character)
    {
        characterValidator.Validate(character);

        var calc = new CharacterCalculationStartupState(this, character);

        var stats = calc.GetStats().ChangeValue(d => d.Normalize());

        return new CharacterCalculationResult(
            Code: characterSerializer.Serialize(character),
            Stats:stats,
            Equipments:calc.GetEquipmentResults(),
            StatDescriptions: calc.GetDescriptionValues(stats));
    }
}