using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;
using RqCalc.Application._Extensions;
using RqCalc.Application.Calculation;
using RqCalc.Application.Settings;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Model;

namespace RqCalc.Application;

public class CharacterCalculator(
    ApplicationSettings settings,
    IVersion lastVersion,
    IStatService statService,
    IClassService classService,
    IFormulaService formulaService,
    IBonusTypeService bonusTypeService,
    IEquipmentForgeService equipmentForgeService,
    ICharacterValidator characterValidator,
    ISerializer<string, ICharacterSource> characterSerializer)
    : ICharacterCalculator
{
    public CharacterCalculationResult Calculate(ICharacterSource character)
    {
        characterValidator.Validate(character);

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