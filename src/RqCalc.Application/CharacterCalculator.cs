using Framework.DataBase;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Model;
using RqCalc.Model.Impl;

namespace RqCalc.Application;

public class CharacterCalculator : ICharacterCalculator
{
    private readonly ApplicationSettings settings;
    
    private readonly IDataSource<IPersistentDomainObjectBase> dataSource;

    private readonly IStatSource statSource;

    private readonly ICharacterSource defaultCharacter;

    public CharacterCalculator(
        ApplicationSettings settings,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IStatSource statSource)
    {
        this.settings = settings;
        this.dataSource = dataSource;
        this.statSource = statSource;

        this.defaultCharacter = this.CreateDefaultCharacter();
    }

    public int GetFreeStats(ICharacterSource characterInput)
    {
        if (characterInput == null) throw new ArgumentNullException(nameof(characterInput));

        var allAvailableStats = (characterInput.Level - 1) * this.settings.StatsPerLevel;

        var usedStats = characterInput.EditStats.Values.Sum(v => v - 1);

        return allAvailableStats - usedStats;
    }


    public ICharacterSource GetDefaultCharacter()
    {
        return this.defaultCharacter;
    }

    private ICharacterSource CreateDefaultCharacter()
    {
        var gender = dataSource.GetFullList<IGender>().OrderById().First();
        var @class = dataSource.GetFullList<IClass>().OrderById().First();
        var state = dataSource.GetFullList<IState>().OrderById().First();

        return new CharacterSource
        {
            Level = 1,
            Gender = gender,
            Class = @class,
            State = state,
            EditStats = this.GetEditStats(@class).ToDictionary(stat => stat, _ => 1)
        };
    }

    public IEnumerable<IStat> GetEditStats(IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return new[] { @class.PrimaryStat }.Concat(this.statSource.NotPrimaryEditStats);
    }
}