using Framework.DataBase;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Model;
using RqCalc.Model.Impl;

namespace RqCalc.Application;

public class DefaultCharacterSource(IDataSource<IPersistentDomainObjectBase> dataSource, IStatService statService) : IDefaultCharacterSource
{
    private ICharacterSource? defaultCharacter;

    public ICharacterSource GetDefaultCharacter() => this.defaultCharacter ??= this.CreateDefaultCharacter();

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
            EditStats = statService.GetEditStats(@class).ToDictionary(stat => stat, _ => 1)
        };
    }
}