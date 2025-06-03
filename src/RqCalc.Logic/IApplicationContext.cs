using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;
using RqCalc.Logic.ImageSource;
using RqCalc.Model;

namespace RqCalc.Logic;

public interface IApplicationContext : ITypeResolverContainer<string>
{
    IVersion LastVersion { get; }

    IApplicationSettings Settings { get; }

    IDataSource<IPersistentDomainObjectBase> DataSource { get; }

    IReadOnlyList<IStat> NotPrimaryEditStats { get; }

    ISerializer<byte[], ICharacterSource> BinarySerializer { get; }

    ISerializer<string, ICharacterSource> Serializer { get; }

    ISerializer<string, ITalentBuildSource> TalentSerializer { get; }

    ISerializer<string, IGuildTalentBuildSource> GuildTalentSerializer { get; }

    IImageSourceService ImageSourceService { get; }

    IReadOnlyDictionary<IAura, Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>> AurasSharedBonuses
    {
        get;
    }


    ICharacterResult Calc(ICharacterSource character);


    int GetFreeStats(ICharacterSource characterInput);

    int GetFreeTalents(ITalentBuildSource characterInput);

    int GetFreeGuildTalents(IGuildTalentBuildSource characterInput);



    IEquipmentClass GetEquipmentClass(IEquipment equipment);


    ICharacterSource GetDefaultCharacter();

    void Validate(ICharacterSource character);

    void Validate(ITalentBuildSource character);

    void Validate(IGuildTalentBuildSource character);
}