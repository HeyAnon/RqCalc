using Framework.Core;
using Framework.Core.Serialization;

using RqCalc.Application.Calculation;
using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Application;

public class CharacterRawCalculator(
    ApplicationSettings settings,
    IVersion lastVersion,
    IStatService statService,
    IClassService classService,
    IFormulaService formulaService,
    IBonusTypeService bonusTypeService,
    IEquipmentForgeService equipmentForgeService,
    ISerializer<string, ICharacterSource> characterSerializer)
    : ICharacterCalculator
{
    public const string Key = "Raw";

    public CharacterCalculationResult Calculate(ICharacterSource character)
    {
        var calculationState =
            new CharacterCalculationStartupState(settings, lastVersion, statService, classService, formulaService, bonusTypeService, equipmentForgeService, character);

        var stats = calculationState.GetStats().ChangeValue(d => d.Normalize());

        return new CharacterCalculationResult(
            Code: characterSerializer.Serialize(character),
            Stats: stats,
            Equipments: calculationState.GetEquipmentResults(),
            StatDescriptions: calculationState.GetDescriptionValues(stats));
    }
}