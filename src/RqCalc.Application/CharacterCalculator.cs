using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;

using RqCalc.Application.Calc;
using RqCalc.Domain._Base;
using RqCalc.Model;
using RqCalc.Model.Impl;
using System.ComponentModel.DataAnnotations;
using RqCalc.Model._Extensions;
using RqCalc.Application.Settings;

namespace RqCalc.Application;

public class CharacterCalculator(
    ApplicationSettings settings,
    IDataSource<IPersistentDomainObjectBase> dataSource,
    IStatSource statSource,
    ICharacterValidator characterValidator)
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

        var calc = new CharacterCalc(this, character);

        var stats = calc.GetStats().ChangeValue(d => d.Normalize());

        return new CharacterCalculateResult
        {
            Stats = stats,

            Code = this.CharacterSerializer.Serialize(character),

            //TalentCode = this.TalentSerializer.Serialize(character),

            //GuildTalentCode = this.GuildTalentSerializer.Serialize(character),

            Equipments = calc.GetEquipmentResults(),

            StatDescriptions = calc.GetDescriptionValues(stats)
        };
    }
}