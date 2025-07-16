using System.Collections.ObjectModel;

using Framework.Core;
using Framework.DataBase;
using Framework.HierarchicalExpand;

using RqCalc.Application.IndexedDict;
using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain._Legacy;
using RqCalc.Domain.Card;
using RqCalc.Domain.CollectedStatistic;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.GuildTalent;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;
using RqCalc.Model;
using RqCalc.Model._Extensions;
using RqCalc.Model.Impl;

namespace RqCalc.Application.Serializer.Character;

internal class CharacterVersionSerializer
{
    private readonly IIndexedDict<IClass> classDict;
    private readonly IIndexedDict<IGender> genderDict;
    private readonly IIndexedDict<IState> stateDict;
    private readonly IIndexedDict<IEvent?> eventDict;
    private readonly IIndexedDict<IElixir?> elixirDict;
    private readonly IIndexedDict<ILegacyGuildBonus> guildLegacyBonusDict;
    private readonly IIndexedDict<IConsumable> consumableDict;
    private readonly IIndexedDict<IStampColor> stampColorDict;

    private readonly IReadOnlyDictionary<IClass, IReadOnlyCollection<IStat>> classStatDict;
    private readonly IReadOnlyDictionary<IClass, IIndexedDict<IAura?>> classAuraDict;
    private readonly IReadOnlyDictionary<IClass, IIndexedDict<IBuff>> classBuffDict;
    private readonly IReadOnlyDictionary<Tuple<IStamp, IStampColor>, IStampVariant> stampVariantDict;
    private readonly IReadOnlyDictionary<Tuple<IClass, IEquipmentSlot>, IIndexedDict<IEquipment>> equipmentDict;
    private readonly IReadOnlyCollection<Tuple<IEquipmentSlot, int>> slotDict;
    private readonly IReadOnlyDictionary<IEquipmentType, IIndexedDict<IStamp>> stampDict;
    private readonly IReadOnlyDictionary<IEquipmentSlot, IIndexedDict<ICard?>> cardsBySlotDict;
    private readonly IReadOnlyDictionary<IEquipmentType, IIndexedDict<ICard?>> cardsBySlotTypeDict;
    private readonly IReadOnlyDictionary<IClass, ReadOnlyCollection<Tuple<ITalentBranch, IReadOnlyList<IReadOnlyList<ITalent>>>>> talentDict;
    private readonly IReadOnlyDictionary<IEquipmentSlot, IIndexedDict<IEquipmentElixir?>> equipmentElixirDict;

    private readonly ApplicationSettings settings;
    
    private readonly IIndexedDict<IAura> sharedAuraDict;

    private readonly IIndexedDict<IBuff> sharedBuffDict;

    private readonly IIndexedDict<IBuff> cardBuffDict;

    private readonly IIndexedDict<IBuff> stampBuffDict;

    private readonly IReadOnlyDictionary<IGender, IReadOnlyCollection<Tuple<ICollectedGroup, IIndexedDict<ICollectedItem>>>> collectedGroupDict;


    private readonly IReadOnlyCollection<IGuildTalentBranch>? guildBranchDict;

    private readonly int guildBranchCount;

    private readonly int guildBranchSize;


    public CharacterVersionSerializer(
        ApplicationSettings settings,
        IStatService statService,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IAuraServiceFactory auraServiceFactory,
        IVersion version)
    {
        this.settings = settings;
        this.Version = version;

        this.classDict = dataSource.GetFullList<IClass>().ToIndexedDict();
        this.genderDict = dataSource.GetFullList<IGender>().ToIndexedDict();
        this.stateDict = dataSource.GetFullList<IState>().ToIndexedDict();
        this.eventDict = dataSource.GetFullList<IEvent>().WhereVersion(this.Version).ToIndexedDict().ToNullable();
        this.elixirDict = dataSource.GetFullList<IElixir>().ToIndexedDict().ToNullable();
        this.stampColorDict = dataSource.GetFullList<IStampColor>().ToIndexedDict();

        this.classStatDict = this.classDict.Select(c => c.GetRoot()).Distinct().ToDictionary(c => c, c => statService.GetEditStats(c).OrderById().ToReadOnlyCollectionI());

        this.stampVariantDict = dataSource.GetFullList<IStampVariant>().ToDictionary(var => Tuple.Create(var.Stamp, var.Color));

        this.consumableDict = dataSource.GetFullList<IConsumable>().ToIndexedDict();

        {
            var classAurasRequest =

                from aura in dataSource.GetFullList<IAura>()

                where aura.Contains(version)

                from @class in aura.Class.GetAllChildren()

                group aura by @class;

            this.classAuraDict = classAurasRequest.ToDictionary(g => g.Key, g => g.ToIndexedDict().ToNullable());
        }

        {
            var classBuffsRequest =

                from buff in dataSource.GetFullList<IBuff>()

                where buff.Class != null && buff.Contains(version)

                from @class in buff.Class.GetAllChildren()

                group buff by @class;

            this.classBuffDict = classBuffsRequest.ToDictionary(g => g.Key, g => g.ToIndexedDict());
        }

        if (this.Version.GuildTalents)
        {
            this.guildBranchDict = dataSource.GetFullList<IGuildTalentBranch>();

            this.guildBranchCount = this.guildBranchDict.Count;
            this.guildBranchSize = this.guildBranchDict.Select(b => b.Talents.Count()).Distinct().Single();
        }
        else
        {
            this.guildLegacyBonusDict = dataSource.GetFullList<ILegacyGuildBonus>().ToIndexedDict();
        }

        {
            var equipmentsRequest =

                from equipment in dataSource.GetFullList<IEquipment>()

                where equipment.Contains(version)

                from @class in this.classDict

                where equipment.IsAllowed(@class)

                let currentSlot = equipment.Type.Slot

                from slot in @class.AllowExtraWeapon && currentSlot.IsPrimarySlot() && equipment.Type.WeaponInfo.IsSingleHand
                    ? new[] { currentSlot, currentSlot.ExtraSlot }
                    : new[] { currentSlot }

                group equipment by Tuple.Create(@class, slot);

            this.equipmentDict = equipmentsRequest.ToDictionary(g => g.Key, g => g.ToIndexedDict());
        }

        this.slotDict = dataSource.GetFullList<IEquipmentSlot>().OrderById().SelectMany(slot => Enumerable.Range(0, slot.Count).Select(index => Tuple.Create(slot, index)))
                                  .ToReadOnlyCollection();

        {
            var stampEquipmentTypes = dataSource.GetFullList<IEquipmentType>().Where(t => t.Slot.IsWeapon != null).ToList();

            var stampsRequest =

                from stamp in dataSource.GetFullList<IStamp>()

                from type in stamp.Equipments.Any() ? stamp.Equipments.WhereVersion(this.Version).Select(e => e.Type) : stampEquipmentTypes

                group stamp by type;

            this.stampDict = stampsRequest.ToReadOnlyDictionary(g => g.Key, g => g.ToIndexedDict());
        }

        if (this.Version.SerializeCardBySlotType)
        {
            var cardsRequest =

                from card in dataSource.GetFullList<ICard>()

                where card.Contains(version)

                from type in card.GetAllTypes(this.Version)

                group card by type;

            this.cardsBySlotTypeDict = cardsRequest.ToReadOnlyDictionary(g => g.Key, g => g.ToIndexedDict().ToNullable());
        }
        else
        {
            var cardsRequest =

                from card in dataSource.GetFullList<ICard>()

                where card.Contains(version)

                from slot in card.GetSlots(this.Version)

                group card by slot;

            this.cardsBySlotDict = cardsRequest.ToReadOnlyDictionary(g => g.Key, g => g.ToIndexedDict().ToNullable());
        }

        this.talentDict = this.classDict.OrderById().ToReadOnlyDictionary(@class => @class, @class =>

                                                                                                @class.GetAllTalentBranches().OrderById().ToReadOnlyCollection(branch =>

                                                                                                    Tuple.Create(branch, branch.Talents.GroupBy(t => t.HIndex).OrderBy(v => v.Key).ToReadOnlyListI(g =>

                                                                                                        g.OrderBy(v => v.VIndex).ToReadOnlyListI()))));


        if (this.Version.SharedAuras)
        {
            this.sharedAuraDict = auraServiceFactory.Create(this.Version).AurasSharedBonuses.Keys.ToIndexedDict();
        }

        if (this.Version.SharedBuffs)
        {
            var buffsRequest =

                from buff in dataSource.GetFullList<IBuff>()

                where buff.IsShared() && buff.Contains(this.Version)

                select buff;

            this.sharedBuffDict = buffsRequest.ToIndexedDict();
        }

        if (this.Version.CardBuffs)
        {
            var buffsRequest =

                from buff in dataSource.GetFullList<IBuff>()

                where buff.Card != null && buff.Contains(this.Version)

                select buff;

            this.cardBuffDict = buffsRequest.ToIndexedDict();
        }

        if (this.Version.StampBuffs)
        {
            var buffsRequest =

                from buff in dataSource.GetFullList<IBuff>()

                where buff.Stamp != null && buff.Contains(this.Version)

                select buff;

            this.stampBuffDict = buffsRequest.ToIndexedDict();
        }

        {
            var request =

                from equipmentElixir in dataSource.GetFullList<IEquipmentElixir>()

                where equipmentElixir.Contains(this.Version)

                from slot in equipmentElixir.Slots

                group equipmentElixir by slot;

            this.equipmentElixirDict = request.ToDictionary(g => g.Key, g => g.ToIndexedDict().ToNullable());
        }

        if (this.Version.Collections)
        {
            var request =

                from gender in dataSource.GetFullList<IGender>().OrderById()

                let groupRequest =

                    from collectionGroup in dataSource.GetFullList<ICollectedGroup>().OrderById()

                    let items = collectionGroup.Items.Where(item => item.IsAllowed(gender, this.Version))

                    select Tuple.Create(collectionGroup, items.ToIndexedDict())

                select (gender, groupRequest.ToReadOnlyCollectionI());

            this.collectedGroupDict = request.ToDictionary();
        }
    }

    public IVersion Version { get; }

    public CharacterSource Parse(BitReader reader)
    {
        var @class = reader.Read(this.classDict);

        var gender = reader.Read(this.genderDict);

        var state = reader.Read(this.stateDict);

        var @event = reader.Read(this.eventDict);

        var elixir = reader.Read(this.elixirDict);

        var level = reader.ReadByMax(@class.Specialization.MaxLevel ?? this.Version.MaxLevel);

        var aura = this.classAuraDict.GetMaybeValue(@class).Select(reader.Read).GetValueOrDefault();

        var classBuffs = this.classBuffDict.GetMaybeValue(@class).Select(reader.ReadDict).GetValueOrDefault(() => new Dictionary<IBuff, int>());


        var guildTalents = this.Version.GuildTalents ? this.ReadGuildTalents(reader).ToDictionary() : new Dictionary<IGuildTalent, int>();

        if (!this.Version.GuildTalents)
        {
            _ = reader.ReadDict(this.guildLegacyBonusDict); // ignored
        }

        var consumables = reader.ReadList(this.consumableDict);

        var enableAura = reader.Read();

        var enableBuffs = reader.Read();

        var enableConsumables = reader.Read();

        var enableElixir = reader.Read();

        var enableGuildBonuses = reader.Read();

        var enableTalents = reader.Read();

        var editStats = this.classStatDict[@class.GetRoot()].ToDictionary(s => s, _ => reader.ReadByMax(this.settings.MaxStatCount));

        var equipments = this.ReadEquipments(reader, @class);

        var talents = this.ReadTalents(reader, @class).ToList();

        var sharedAuras = this.Version.SharedAuras
            ? Enumerable.Range(0, reader.ReadByMax(this.settings.MaxPartySize - 1)).ToDictionary(_ => reader.Read(this.sharedAuraDict), _ => reader.Read())
            : new Dictionary<IAura, bool>();

        var sharedBuffs = this.Version.SharedBuffs ? reader.ReadDict(this.sharedBuffDict) : new Dictionary<IBuff, int>();

        var cardBuffs = this.Version.CardBuffs ? reader.ReadDict(this.cardBuffDict) : new Dictionary<IBuff, int>();

        var stampBuffs = this.Version.StampBuffs ? reader.ReadDict(this.stampBuffDict) : new Dictionary<IBuff, int>();

        var lostControl = this.Version.LostControl && reader.Read();

        var enableCollections = this.Version.Collections && reader.Read();

        var collectedItems = this.Version.Collections ? this.collectedGroupDict[gender].SelectMany(pair => reader.ReadListOptimize(pair.Item2)).ToList() : [];

        return new CharacterSource
        {
            Class = @class,
            Gender = gender,
            State = state,
            Event = @event,
            Elixir = elixir,
            Level = level,
            Aura = aura,
            Buffs = classBuffs.Concat(sharedBuffs).Concat(cardBuffs).Concat(stampBuffs),
            GuildTalents = guildTalents,
            Consumables = consumables,
            CollectedItems = collectedItems,
            EnableAura = enableAura,
            EnableBuffs = enableBuffs,
            EnableConsumables = enableConsumables,
            EnableElixir = enableElixir,
            EnableGuildTalents = enableGuildBonuses,
            EnableTalents = enableTalents,
            EditStats = editStats,
            Equipments = equipments,
            Talents = talents,
            SharedAuras = sharedAuras,
            LostControl = lostControl,
            EnableCollecting = enableCollections
        };
    }

    private Dictionary<CharacterEquipmentIdentity, CharacterEquipmentData> ReadEquipments(BitReader reader, IClass @class)
    {
        var equipmentsRequest =

            from slotPair in this.slotDict

            let currentSlot = slotPair.Item1

            where reader.Read()

            let equipment = reader.Read(this.equipmentDict[Tuple.Create(@class, currentSlot)])

            let baseSlot = equipment.Type.Slot

            let maxCardCount = baseSlot.IsWeapon == null ? 0 : baseSlot.IsWeapon.Value ? this.settings.WeaponCardCount : this.settings.EquipmentCardCount

            let active = !equipment.IsActivate() || reader.Read()

            let upgrade = baseSlot.IsWeapon == null ? 0 : reader.ReadByMax(this.settings.MaxUpgradeLevel)

            let stampVariant = baseSlot.IsWeapon == null || !reader.Read()
                ? null
                : this.stampVariantDict[Tuple.Create(reader.Read(this.stampDict[equipment.Type]), reader.Read(this.stampColorDict))]

            let cards = baseSlot.IsWeapon == null
                ? []
                : Enumerable.Range(0, maxCardCount).ToList(_ => this.Version.SerializeCardBySlotType ? reader.Read(this.cardsBySlotTypeDict[equipment.Type]) : reader.Read(this.cardsBySlotDict[baseSlot]))

            let elixir = this.equipmentElixirDict.GetValueOrDefault(baseSlot).Maybe(reader.Read)

            select new
            {
                Key = new CharacterEquipmentIdentity(currentSlot, slotPair.Item2),

                Value = new CharacterEquipmentData
                {
                    Equipment = equipment,

                    Active = active,

                    Upgrade = upgrade,

                    StampVariant = stampVariant,

                    Cards = cards,

                    Elixir = elixir
                }
            };

        return equipmentsRequest.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private IEnumerable<ITalent> ReadTalents(BitReader reader, IClass @class) =>
        from branchPair in this.talentDict[@class]

        let talentCount = reader.ReadByMax(branchPair.Item2.Count)

        from talentVertList in branchPair.Item2.Take(talentCount)

        let talentIndex = reader.ReadByMax(talentVertList.Count - 1)

        let talent = talentVertList[talentIndex]

        select talent;

    private IEnumerable<KeyValuePair<IGuildTalent, int>> ReadGuildTalents(BitReader reader)
    {
        var branchCount = reader.ReadByMax(this.guildBranchCount);

        foreach (var pair in this.guildBranchDict!.Select((branch, index) => new { Branch = branch, Index = index }).Take(branchCount))
        {
            var talentOrderIndex = reader.ReadByMax(this.guildBranchSize);

            var talent = pair.Branch.Talents.Single(tal => tal.OrderIndex == talentOrderIndex);

            var isLast = pair.Index == branchCount - 1;

            var points = isLast ? reader.ReadByMax(pair.Branch.MaxPoints - 1) + 1 : pair.Branch.MaxPoints;

            yield return new KeyValuePair<IGuildTalent, int>(talent, points);
        }
    }

    public void Format(BitWriter writer, ICharacterSource character)
    {
        writer.Write(character.Class, this.classDict);
        writer.Write(character.Gender, this.genderDict);
        writer.Write(character.State, this.stateDict);
        writer.Write(character.Event, this.eventDict);
        writer.Write(character.Elixir, this.elixirDict);

        writer.WriteByMax(character.Level, character.Class.Specialization.MaxLevel ?? this.Version.MaxLevel);

        this.classAuraDict.GetMaybeValue(character.Class).Match(auraDict => writer.Write(character.Aura, auraDict));

        {
            var buffs = character.Buffs.Where(b => b.Key.Class != null).ToDictionary();

            this.classBuffDict.GetMaybeValue(character.Class).Match(buffDict => writer.Write(buffs, buffDict));
        }

        if (this.Version.GuildTalents)
        {
            var dataList = character.GuildTalents.Where(tal => tal.Value != 0)
                .OrderBy(tal => tal.Key.Branch.Id)
                .Reverse()
                .Select((pair, index) => new { Talent = pair.Key, Points = pair.Value, IsLast = index == 0 })
                .Reverse()
                .ToList();

            writer.WriteByMax(dataList.Count, this.guildBranchCount);

            foreach (var data in dataList)
            {
                writer.WriteByMax(data.Talent.OrderIndex, this.guildBranchSize);

                if (data.IsLast)
                {
                    writer.WriteByMax(data.Points - 1, data.Talent.Branch.MaxPoints - 1);
                }
            }
        }
        else
        {
            //throw new Exception("serialization not supported");
            writer.WriteByMax(0, this.guildLegacyBonusDict.Count);
        }


        writer.Write(character.Consumables, this.consumableDict);

        writer.Write(character.EnableAura, character.EnableBuffs, character.EnableConsumables, character.EnableElixir, character.EnableGuildTalents, character.EnableTalents);

        this.classStatDict[character.Class.GetRoot()].Foreach(stat => writer.WriteByMax(character.EditStats[stat], this.settings.MaxStatCount));

        foreach (var pair in this.slotDict)
        {
            var charEquip = character.Equipments.GetValueOrDefault(pair.Item1, pair.Item2);

            writer.Write(charEquip != null);

            if (charEquip != null)
            {
                writer.Write(charEquip.Equipment, this.equipmentDict[Tuple.Create(character.Class, pair.Item1)]);

                if (charEquip.Equipment.IsActivate())
                {
                    writer.Write(charEquip.Active);
                }

                var baseSlot = charEquip.Equipment.Type.Slot;

                if (baseSlot.IsWeapon != null)
                {
                    writer.WriteByMax(charEquip.Upgrade, this.settings.MaxUpgradeLevel);

                    writer.Write(charEquip.StampVariant != null);

                    if (charEquip.StampVariant != null)
                    {
                        writer.Write(charEquip.StampVariant.Stamp, this.stampDict[charEquip.Equipment.Type]);
                        writer.Write(charEquip.StampVariant.Color, this.stampColorDict);
                    }

                    var maxCardCount = baseSlot.IsWeapon.Value ? this.settings.WeaponCardCount : this.settings.EquipmentCardCount;

                    for (var i = 0; i < maxCardCount; i++)
                    {
                        var card = charEquip.Cards.Count > i ? charEquip.Cards[i] : null;

                        if (this.Version.SerializeCardBySlotType)
                        {
                            writer.Write(card, this.cardsBySlotTypeDict[charEquip.Equipment.Type]);
                        }
                        else
                        {
                            writer.Write(card, this.cardsBySlotDict[baseSlot]);
                        }
                    }
                }

                this.equipmentElixirDict.GetValueOrDefault(baseSlot).Maybe(dict => writer.Write(charEquip.Elixir, dict));
            }
        }

        {

            var charBranches = character.Talents.GroupBy(tal => tal.Branch).ToDictionary(g => g.Key, g => g.OrderBy(v => v.HIndex).ToReadOnlyCollection());

            foreach (var branchPair in this.talentDict[character.Class])
            {
                var charTalents = charBranches.GetValueOrDefault(branchPair.Item1);

                writer.WriteByMax(charTalents.Maybe(v => v.Count, 0), branchPair.Item2.Count);

                if (charTalents != null)
                {
                    foreach (var talentPair in branchPair.Item2.Zip(charTalents, (definition, talent) => new { Definition = definition, Talent = talent }))
                    {
                        var index = talentPair.Definition.IndexOf(talentPair.Talent);

                        writer.WriteByMax(index, talentPair.Definition.Count - 1);
                    }
                }
            }
        }

        if (this.Version.SharedAuras)
        {
            writer.WriteByMax(character.SharedAuras.Count, this.settings.MaxPartySize - 1);

            foreach (var pair in character.SharedAuras.OrderById())
            {
                writer.Write(pair.Key, this.sharedAuraDict);
                writer.Write(pair.Value);
            }
        }

        if (this.Version.SharedBuffs)
        {
            var buffs = character.Buffs.Where(b => b.Key.IsShared()).ToDictionary();

            writer.Write(buffs, this.sharedBuffDict);
        }

        if (this.Version.CardBuffs)
        {
            var buffs = character.Buffs.Where(b => b.Key.Card != null).ToDictionary();

            writer.Write(buffs, this.cardBuffDict);
        }

        if (this.Version.StampBuffs)
        {
            var buffs = character.Buffs.Where(b => b.Key.Stamp != null).ToDictionary();

            writer.Write(buffs, this.stampBuffDict);
        }

        if (this.Version.LostControl)
        {
            writer.Write(character.LostControl);
        }

        if (this.Version.Collections)
        {
            writer.Write(character.EnableCollecting);

            var grouped = character.CollectedItems.GroupBy(item => item.Group).ToDictionary(pair => pair.Key, pair => pair.ToList());

            foreach (var pair in this.collectedGroupDict[character.Gender])
            {
                var items = grouped.GetValueOrDefault(pair.Item1) ?? [];

                writer.WriteOptimize(items, pair.Item2);
            }
        }
    }

    public void FullFormat(BitWriter writer, ICharacterSource character)
    {
        writer.WriteByMax(this.Version.Id, byte.MaxValue);

        this.Format(writer, character);
    }
}
