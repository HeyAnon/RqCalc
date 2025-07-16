using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;

using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Model;
using RqCalc.Model.Impl;

namespace RqCalc.Application.Serializer.Character;

public class CharacterSerializer : ISerializer<byte[], ICharacterSource>
{
    private readonly IVersion lastVersion;

    private readonly IEquipmentService equipmentService;

    private readonly IStampService stampService;

    private readonly IReadOnlyDictionary<int, Lazy<CharacterVersionSerializer>> serializers;
    
    private readonly Lazy<CharacterVersionSerializer> lazyLastSerializer;

    public CharacterSerializer(
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IVersion lastVersion,
        ApplicationSettings settings,
        IStatService statService,
        IEquipmentService equipmentService,
        IStampService stampService,
        IAuraServiceFactory auraServiceFactory)
    {
        this.lastVersion = lastVersion;
        this.equipmentService = equipmentService;
        this.stampService = stampService;

        var serializersRequest =

            from version in dataSource.GetFullList<IVersion>()

            where version.Id <= lastVersion.Id

            select (version.Id, LazyHelper.Create(() => new CharacterVersionSerializer(settings, statService, dataSource, auraServiceFactory, version)));

        this.serializers = serializersRequest.ToDictionary();

        this.lazyLastSerializer = LazyHelper.Create(() => this.serializers[lastVersion.Id].Value);
    }

    public ICharacterSource Parse(byte[] input)
    {
        var reader = new BitReader(input);

        var preVersion = reader.ReadByMax(byte.MaxValue);

        var version = preVersion == 0 ? 1 : preVersion;

        var serializer = this.serializers.GetValue(version, () => new Exception($"Invalid Version: {version}"));

        var character = serializer.Value.Parse(reader);

        //--------------------------

        this.Normalize(character);

        //-------------------------------

        return character;
    }

    public byte[] Format(ICharacterSource character)
    {
        var writer = new BitWriter();

        this.lazyLastSerializer.Value.FullFormat(writer, character);

        return writer.GetBytes();
    }

    private void Normalize(CharacterSource character)
    {
        character.Buffs.RemoveBy(buff => !buff.Key.Contains(this.lastVersion));

        if (character.Aura.Maybe(aura => !aura.Contains(this.lastVersion)))
        {
            character.Aura = null;
        }

        foreach (var equipmentPair in character.Equipments.ToArray())
        {
            var equipmentData = equipmentPair.Value;

            var equipment = equipmentData.Equipment;

            if (equipment.Contains(this.lastVersion))
            {
                var maxUpgradeLevel = this.equipmentService.GetMaxUpgradeLevel(equipment);

                equipmentData.Upgrade = Math.Min(equipmentData.Upgrade, maxUpgradeLevel);

                equipmentData.Cards.ToList().Foreach((card, i) =>
                {
                    if (card != null && !(card.Contains(this.lastVersion) &&
                                          card.IsAllowed(equipment.Type, this.lastVersion, this.equipmentService.GetEquipmentClass(equipment))))
                    {
                        equipmentData.Cards[i] = null;
                    }
                });

                if (equipmentData.StampVariant != null)
                {
                    if (this.stampService.IsAllowedStamp(equipmentData.StampVariant.Stamp, equipment, character.Class) == false)
                    {
                        equipmentData.StampVariant = null;
                    }
                }

            }
            else
            {
                character.Equipments.Remove(equipmentPair.Key);
            }
        }
    }
}
