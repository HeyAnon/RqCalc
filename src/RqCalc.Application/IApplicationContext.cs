using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;

using RqCalc.Application.ImageSource;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;
using RqCalc.Model;

namespace RqCalc.Application;

public interface IGuildTalentCalculator
{
    int GetFreeGuildTalents(IGuildTalentBuildSource characterInput);
}



public interface IApplicationContext : ITypeResolverContainer<string>
{
    IDataSource<IPersistentDomainObjectBase> DataSource { get; }

    IReadOnlyList<IStat> NotPrimaryEditStats { get; }

    ISerializer<byte[], ICharacterSource> CharacterBinarySerializer { get; }

    ISerializer<string, ICharacterSource> CharacterSerializer { get; }

    ISerializer<string, ITalentBuildSource> TalentSerializer { get; }

    ISerializer<string, IGuildTalentBuildSource> GuildTalentSerializer { get; }

    IImageSourceService ImageSourceService { get; }

    IReadOnlyDictionary<IAura, Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>> AurasSharedBonuses
    {
        get;
    }


    ICharacterResult Calc(ICharacterSource character);




    IEquipmentClass GetEquipmentClass(IEquipment equipment);


    ICharacterSource GetDefaultCharacter();

    void Validate(ICharacterSource character);

    void Validate(ITalentBuildSource character);

    void Validate(IGuildTalentBuildSource character);
}