using Framework.Core;
using Framework.Core.Serialization;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Model;
using RqCalc.Domain.Persistent;
using RqCalc.Logic._Extensions;
using RqCalc.Logic.Serializer._Internal;

namespace RqCalc.Logic.Serializer.Character;

internal class CharacterSerializer : ISerializer<byte[], ICharacterSource>
{
    private readonly ApplicationContext _context;

    private readonly IReadOnlyDictionary<int, Lazy<CharacterVersionSerializer>> _serializers;

    internal readonly CharacterVersionSerializer LastVersionSerializer;


    public CharacterSerializer(ApplicationContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));

        var serializersRequest = from version in context.DataSource.GetFullList<IVersion>()

            where version.Id <= context.LastVersion.Id

            select version.Id.ToKeyValuePair(LazyHelper.Create(() => new CharacterVersionSerializer(this._context, version)));

        this._serializers = serializersRequest.ToDictionary();

        this.LastVersionSerializer = this._serializers[context.LastVersion.Id].Value;
    }


    public ICharacterSource Parse(byte[] input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        var reader = new BitReader(input);

        var preVersion = reader.ReadByMax(byte.MaxValue);

        var version = preVersion == 0 ? 1 : preVersion;

        var serializer = this._serializers.GetValue(version, () => new Exception($"Invalid Version: {version}"));

        var character = serializer.Value.Parse(reader);

        //--------------------------

        character.Buffs.RemoveBy(buff => !buff.Key.Contains(this.LastVersionSerializer.Version));

        if (character.Aura.Maybe(aura => !aura.Contains(this.LastVersionSerializer.Version)))
        {
            character.Aura = null;
        }

        foreach (var equipmentPair in character.Equipments.ToArray())
        {
            var equipmentData = equipmentPair.Value;

            var equipment = equipmentData.Equipment;

            if (equipment.Contains(this.LastVersionSerializer.Version))
            {
                var maxUpgradeLevel = this._context.GetMaxUpgradeLevel(equipment);

                equipmentData.Upgrade = Math.Min(equipmentData.Upgrade, maxUpgradeLevel);

                equipmentData.Cards.ToList().Foreach((card, i) =>
                {
                    if (card != null && !(card.Contains(this.LastVersionSerializer.Version) && card.IsAllowed(equipment.Type, this.LastVersionSerializer.Version, this._context.GetEquipmentClass(equipment))))
                    {
                        equipmentData.Cards[i] = null;
                    }
                });

                if (equipmentData.StampVariant != null)
                {
                    if (this._context.IsAllowedStamp(equipmentData.StampVariant.Stamp, equipment, character.Class) == false)
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

        //-------------------------------

        return character;
    }

    public byte[] Format(ICharacterSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        var writer = new BitWriter();
            
        this.LastVersionSerializer.FullFormat(writer, character);
            
        return writer.GetBytes();
    }
}