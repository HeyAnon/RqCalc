using System.Collections.ObjectModel;

using Framework.Core;
using Framework.HierarchicalExpand;
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
using RqCalc.Logic.Serializer._Internal;
using RqCalc.Model;
using RqCalc.Model._Extensions;
using RqCalc.Model.Impl;

namespace RqCalc.Logic.Serializer.Character;

internal class CharacterVersionSerializer
{
    private readonly ApplicationContext _context;
        
    private readonly IIndexedDict<IClass> _classes;
    private readonly IIndexedDict<IGender> _genders;
    private readonly IIndexedDict<IState> _states;
    private readonly IIndexedDict<IEvent> _events;
    private readonly IIndexedDict<IElixir> _elixirs;
    private readonly IIndexedDict<ILegacyGuildBonus> _guildBonuses;
    private readonly IIndexedDict<IConsumable> _consumables;
    private readonly IIndexedDict<IStampColor> _stampColors;

    private readonly IReadOnlyDictionary<IClass, IReadOnlyCollection<IStat>> _classStats;
    private readonly IReadOnlyDictionary<IClass, IIndexedDict<IAura>> _classAuras;
    private readonly IReadOnlyDictionary<IClass, IIndexedDict<IBuff>> _classBuffs;
    private readonly IReadOnlyDictionary<Tuple<IStamp, IStampColor>, IStampVariant> _stampVariants;
    private readonly IReadOnlyDictionary<Tuple<IClass, IEquipmentSlot>, IIndexedDict<IEquipment>> _equipments;
    private readonly IReadOnlyCollection<Tuple<IEquipmentSlot, int>> _slots;
    private readonly IReadOnlyDictionary<IEquipmentType, IIndexedDict<IStamp>> _stamps;
    private readonly IReadOnlyDictionary<IEquipmentSlot, IIndexedDict<ICard>> _cardsBySlot;
    private readonly IReadOnlyDictionary<IEquipmentType, IIndexedDict<ICard>> _cardsBySlotType;
    private readonly IReadOnlyDictionary<IClass, ReadOnlyCollection<Tuple<ITalentBranch, IReadOnlyList<IReadOnlyList<ITalent>>>>> _talents;
    private readonly IReadOnlyDictionary<IEquipmentSlot, IIndexedDict<IEquipmentElixir>> _equipmentElixirs;

    public readonly IVersion Version;


    internal readonly IReadOnlyDictionary<IAura, Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>> AuraSharedBonuses;

    private readonly IIndexedDict<IAura> _sharedAuras;

    private readonly IIndexedDict<IBuff> _sharedBuffs;

    private readonly IIndexedDict<IBuff> _cardBuffs;

    private readonly IIndexedDict<IBuff> _stampBuffs;

    private readonly IReadOnlyDictionary<IGender, IReadOnlyCollection<Tuple<ICollectedGroup, IIndexedDict<ICollectedItem>>>> _collectedGroups;


    private readonly IReadOnlyCollection<IGuildTalentBranch> _guildBranches;

    private readonly int _guildBranchCount;

    private readonly int _guildBranchSize;


    public CharacterVersionSerializer(ApplicationContext context, IVersion version)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        this.Version = version ?? throw new ArgumentNullException(nameof(version));

        var classes = this._context.DataSource.GetFullList<IClass>();

        this._classes = IndexedDict.Create(classes, false);
        this._genders = IndexedDict.Create(this._context.DataSource.GetFullList<IGender>(), false);
        this._states = IndexedDict.Create(this._context.DataSource.GetFullList<IState>(), false);
        this._events = IndexedDict.Create(this._context.DataSource.GetFullList<IEvent>().WhereVersion(this.Version), true);
        this._elixirs = IndexedDict.Create(this._context.DataSource.GetFullList<IElixir>(), true);
        this._stampColors = IndexedDict.Create(this._context.DataSource.GetFullList<IStampColor>(), false);

        this._classStats = classes.Select(c => c.GetRoot()).Distinct().ToDictionary(c => c, c => this._context.GetEditStats(c).OrderById().ToReadOnlyCollectionI());

        this._stampVariants = this._context.DataSource.GetFullList<IStampVariant>().ToDictionary(var => Tuple.Create(var.Stamp, var.Color));

        this._consumables = IndexedDict.Create(this._context.DataSource.GetFullList<IConsumable>(), false);

        {
            var classAurasRequest = from aura in this._context.DataSource.GetFullList<IAura>()

                where aura.Contains(version)

                from @class in aura.Class.GetAllChildren()

                group aura by @class;

            this._classAuras = classAurasRequest.ToDictionary(g => g.Key, g => IndexedDict.Create(g, true));
        }

        {
            var classBuffsRequest = from buff in this._context.DataSource.GetFullList<IBuff>()

                where buff.Class != null && buff.Contains(version)

                from @class in buff.Class.GetAllChildren()

                group buff by @class;

            this._classBuffs = classBuffsRequest.ToDictionary(g => g.Key, g => IndexedDict.Create(g, false));
        }



        if (this.Version.GuildTalents)
        {
            this._guildBranches = this._context.DataSource.GetFullList<IGuildTalentBranch>();

            this._guildBranchCount = this._guildBranches.Count;
            this._guildBranchSize = this._guildBranches.Select(b => b.Talents.Count()).Distinct().Single();
        }
        else
        {
            this._guildBonuses = IndexedDict.Create(this._context.DataSource.GetFullList<ILegacyGuildBonus>(), false);
        }
            

        {
            var equipmentsRequest = from equipment in context.DataSource.GetFullList<IEquipment>()

                where equipment.Contains(version)

                from @class in classes

                where equipment.IsAllowed(@class)

                let currentSlot = equipment.Type.Slot

                from slot in @class.AllowExtraWeapon && currentSlot.IsPrimarySlot() && equipment.Type.WeaponInfo.IsSingleHand ? new[] { currentSlot, currentSlot.ExtraSlot } : new[] { currentSlot }

                group equipment by Tuple.Create(@class, slot);


            this._equipments = equipmentsRequest.ToDictionary(g => g.Key, g => IndexedDict.Create(g, false));
        }

        this._slots = this._context.DataSource.GetFullList<IEquipmentSlot>().OrderById().SelectMany(slot => Enumerable.Range(0, slot.Count).Select(index => Tuple.Create(slot, index))).ToReadOnlyCollection();

            
        {
            var stampEquipmentTypes = this._context.DataSource.GetFullList<IEquipmentType>().Where(t => t.Slot.IsWeapon != null).ToList();

            var stampsRequest = from stamp in this._context.DataSource.GetFullList<IStamp>()

                from type in stamp.Equipments.Any() ? stamp.Equipments.WhereVersion(this.Version).Select(e => e.Type) : stampEquipmentTypes

                group stamp by type;


            this._stamps = stampsRequest.ToReadOnlyDictionary(g => g.Key, g => IndexedDict.Create(g, false));
        }

        if (this.Version.SerializeCardBySlotType)
        {
            var cardsRequest = from card in this._context.DataSource.GetFullList<ICard>()

                where card.Contains(version)

                from type in card.GetAllTypes(this.Version)

                group card by type;

            this._cardsBySlotType = cardsRequest.ToReadOnlyDictionary(g => g.Key, g => IndexedDict.Create(g, true));
        }
        else
        {
            var cardsRequest = from card in this._context.DataSource.GetFullList<ICard>()

                where card.Contains(version)

                from slot in card.GetSlots(this.Version)

                group card by slot;

            this._cardsBySlot = cardsRequest.ToReadOnlyDictionary(g => g.Key, g => IndexedDict.Create(g, true));
        }

        this._talents = classes.OrderById().ToReadOnlyDictionary(@class => @class, @class =>

            @class.GetAllTalentBranches().OrderById().ToReadOnlyCollection(branch =>

                Tuple.Create(branch, branch.Talents.GroupBy(t => t.HIndex).OrderBy(v => v.Key).ToReadOnlyListI(g =>

                    g.OrderBy(v => v.VIndex).ToReadOnlyListI()))));


        if (this.Version.SharedAuras)
        {
            var aurasRequest = from aura in this._context.DataSource.GetFullList<IAura>()

                where aura.Contains(this.Version)

                let bc1 = new VirtualBonusBaseContainer { Bonuses = aura.GetBonuses(this.Version, true, false).ToList() }

                let bc2 = new VirtualBonusBaseContainer { Bonuses = aura.GetBonuses(this.Version, true, true).ToList() }

                where bc1.Bonuses.Any() || bc2.Bonuses.Any()

                select new
                {
                    Key = aura,

                    Value = new Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>(bc1, bc2)
                };

            this.AuraSharedBonuses = aurasRequest.ToDictionary(pair => pair.Key, pair => pair.Value);

            this._sharedAuras = IndexedDict.Create(this.AuraSharedBonuses.Keys, false);
        }

        if (this.Version.SharedBuffs)
        {
            var buffsRequest = from buff in this._context.DataSource.GetFullList<IBuff>()

                where buff.IsShared() && buff.Contains(this.Version)

                select buff;

            this._sharedBuffs = IndexedDict.Create(buffsRequest, false);
        }

        if (this.Version.CardBuffs)
        {
            var buffsRequest = from buff in this._context.DataSource.GetFullList<IBuff>()

                where buff.Card != null && buff.Contains(this.Version)

                select buff;

            this._cardBuffs = IndexedDict.Create(buffsRequest, false);
        } 
            
        if (this.Version.StampBuffs)
        {
            var buffsRequest = from buff in this._context.DataSource.GetFullList<IBuff>()

                where buff.Stamp != null && buff.Contains(this.Version)

                select buff;

            this._stampBuffs = IndexedDict.Create(buffsRequest, false);
        }

        {
            var request = from equipmentElixir in this._context.DataSource.GetFullList<IEquipmentElixir>()

                where equipmentElixir.Contains(this.Version)

                from slot in equipmentElixir.Slots

                group equipmentElixir by slot into g

                select new
                {
                    Key = g.Key,

                    Value = IndexedDict.Create(g, true)
                };

            this._equipmentElixirs = request.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        if (this.Version.Collections)
        {
            var request = from gender in this._context.DataSource.GetFullList<IGender>().OrderById()

                let groupRequest = from collectionGroup in this._context.DataSource.GetFullList<ICollectedGroup>().OrderById()
                              
                    let items = collectionGroup.Items.Where(item => item.IsAllowed(gender, this.Version))

                    select Tuple.Create(collectionGroup, IndexedDict.Create(items, false))
                                                 
                select gender.ToKeyValuePair(groupRequest.ToReadOnlyCollectionI());

                
            this._collectedGroups = request.ToDictionary();
        }
    }

    public CharacterSource Parse(BitReader reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));


        var @class = reader.Read(this._classes);
            
        var gender = reader.Read(this._genders);

        var state = reader.Read(this._states);

        var @event = reader.Read(this._events);

        var elixir = reader.Read(this._elixirs);

        var level = reader.ReadByMax(@class.Specialization.MaxLevel ?? this.Version.MaxLevel);

        var aura = this._classAuras.GetMaybeValue(@class).Select(reader.Read).GetValueOrDefault();

        var classBuffs = this._classBuffs.GetMaybeValue(@class).Select(reader.ReadDict).GetValueOrDefault(() => new Dictionary<IBuff, int>());

            
        var guildTalents = this.Version.GuildTalents ? this.ReadGuildTalents(reader).ToDictionary() : new Dictionary<IGuildTalent, int>();

        if (!this.Version.GuildTalents)
        {
            var guildBonuses = reader.ReadDict(this._guildBonuses); // ignored
        }

        var consumables = reader.ReadList(this._consumables);

        var enableAura = reader.Read();

        var enableBuffs = reader.Read();

        var enableConsumables = reader.Read();

        var enableElixir = reader.Read();

        var enableGuildBonuses = reader.Read();

        var enableTalents = reader.Read();

        var editStats = this._classStats[@class.GetRoot()].ToDictionary(s => s, s => reader.ReadByMax(this._context.Settings.MaxStatCount));

        var equipments = this.ReadEquipments(reader, @class);

        var talents = this.ReadTalents(reader, @class).ToList();

        var sharedAuras = this.Version.SharedAuras ? Enumerable.Range(0, reader.ReadByMax(this._context.Settings.MaxPartySize - 1)).ToDictionary(_ => reader.Read(this._sharedAuras), _ => reader.Read()) : new Dictionary<IAura, bool>();

        var sharedBuffs = this.Version.SharedBuffs ? reader.ReadDict(this._sharedBuffs) : new Dictionary<IBuff, int>();

        var cardBuffs = this.Version.CardBuffs ? reader.ReadDict(this._cardBuffs) : new Dictionary<IBuff, int>();

        var stampBuffs = this.Version.StampBuffs ? reader.ReadDict(this._stampBuffs) : new Dictionary<IBuff, int>();

        var lostControl = this.Version.LostControl && reader.Read();

        var enableCollections = this.Version.Collections && reader.Read();
            
        var collectedItems = this.Version.Collections ? this._collectedGroups[gender].SelectMany(pair => reader.ReadListOptimize(pair.Item2)).ToList() : new List<ICollectedItem>();
            
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
        if (reader == null) throw new ArgumentNullException(nameof(reader));
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        var equipmentsRequest = from slotPair in this._slots

            let currentSlot = slotPair.Item1

            where reader.Read()

            let equipment = reader.Read(this._equipments[Tuple.Create(@class, currentSlot)])

            let baseSlot = equipment.Type.Slot

            let maxCardCount = baseSlot.IsWeapon == null ? 0 : baseSlot.IsWeapon.Value ? this._context.Settings.WeaponCardCount : this._context.Settings.EquipmentCardCount

            let active = !equipment.IsActivate() || reader.Read()

            let upgrade = baseSlot.IsWeapon == null ? 0 : reader.ReadByMax(this._context.Settings.MaxUpgradeLevel)

            let stampVariant = baseSlot.IsWeapon == null || !reader.Read() ? null : this._stampVariants[Tuple.Create(reader.Read(this._stamps[equipment.Type]), reader.Read(this._stampColors))]

            let cards = baseSlot.IsWeapon == null ? new List<ICard>() : Enumerable.Range(0, maxCardCount).ToList(_ => this.Version.SerializeCardBySlotType ? reader.Read(this._cardsBySlotType[equipment.Type]) : reader.Read(this._cardsBySlot[baseSlot]))

            let elixir = this._equipmentElixirs.GetValueOrDefault(baseSlot).Maybe(reader.Read)

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

    private IEnumerable<ITalent> ReadTalents(BitReader reader, IClass @class)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return from branchPair in this._talents[@class]

            let talentCount = reader.ReadByMax(branchPair.Item2.Count)
                   
            from talentVertList in branchPair.Item2.Take(talentCount)
                   
            let talentIndex = reader.ReadByMax(talentVertList.Count - 1)
                   
            let talent = talentVertList[talentIndex]
                   
            select talent;
    }

    private IEnumerable<KeyValuePair<IGuildTalent, int>> ReadGuildTalents(BitReader reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));

        var branchCount = reader.ReadByMax(this._guildBranchCount);

        foreach (var pair in this._guildBranches.Select((branch, index) => new { Branch = branch, Index = index }).Take(branchCount))
        {
            var talentOrderIndex = reader.ReadByMax(this._guildBranchSize);

            var talent = pair.Branch.Talents.Single(tal => tal.OrderIndex == talentOrderIndex);

            var isLast = pair.Index == branchCount - 1;

            var points = isLast ? reader.ReadByMax(pair.Branch.MaxPoints - 1) + 1 : pair.Branch.MaxPoints;

            yield return new KeyValuePair<IGuildTalent, int>(talent, points);
        }
    }

    public void Format(BitWriter writer, ICharacterSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));
            
        writer.Write(character.Class, this._classes);
        writer.Write(character.Gender, this._genders);
        writer.Write(character.State, this._states);
        writer.Write(character.Event, this._events);
        writer.Write(character.Elixir, this._elixirs);

        writer.WriteByMax(character.Level, character.Class.Specialization.MaxLevel ?? this.Version.MaxLevel);

        this._classAuras.GetMaybeValue(character.Class).Match(auraDict => writer.Write(character.Aura, auraDict));

        {
            var buffs = character.Buffs.Where(b => b.Key.Class != null).ToDictionary();

            this._classBuffs.GetMaybeValue(character.Class).Match(buffDict => writer.Write(buffs, buffDict));    
        }

        if (this.Version.GuildTalents)
        {
            var datas = character.GuildTalents.Where(tal => tal.Value != 0)
                .OrderBy(tal => tal.Key.Branch.Id)
                .Reverse()
                .Select((pair, index) => new { Talent = pair.Key, Points = pair.Value, IsLast = index == 0 })
                .Reverse()
                .ToArray();

            writer.WriteByMax(datas.Length, this._guildBranchCount);

            foreach (var data in datas)
            {
                writer.WriteByMax(data.Talent.OrderIndex, this._guildBranchSize);

                if (data.IsLast)
                {
                    writer.WriteByMax(data.Points - 1, data.Talent.Branch.MaxPoints - 1);
                }
            }
        }
        else
        {
            //throw new Exception("serialization not supported");
            writer.WriteByMax(0, this._guildBonuses.Count);
        }

            
        writer.Write(character.Consumables, this._consumables);

        writer.Write(character.EnableAura, character.EnableBuffs, character.EnableConsumables, character.EnableElixir, character.EnableGuildTalents, character.EnableTalents);

        this._classStats[character.Class.GetRoot()].Foreach(stat => writer.WriteByMax(character.EditStats[stat], this._context.Settings.MaxStatCount));

        foreach (var pair in this._slots)
        {
            var charEquip = character.Equipments.GetValueOrDefault(pair.Item1, pair.Item2);

            writer.Write(charEquip != null);

            if (charEquip != null)
            {
                writer.Write(charEquip.Equipment, this._equipments[Tuple.Create(character.Class, pair.Item1)]);

                if (charEquip.Equipment.IsActivate())
                {
                    writer.Write(charEquip.Active);
                }

                var baseSlot = charEquip.Equipment.Type.Slot;

                if (baseSlot.IsWeapon != null)
                {
                    writer.WriteByMax(charEquip.Upgrade, this._context.Settings.MaxUpgradeLevel);

                    writer.Write(charEquip.StampVariant != null);

                    if (charEquip.StampVariant != null)
                    {
                        writer.Write(charEquip.StampVariant.Stamp, this._stamps[charEquip.Equipment.Type]);
                        writer.Write(charEquip.StampVariant.Color, this._stampColors);
                    }

                    var maxCardCount = baseSlot.IsWeapon.Value ? this._context.Settings.WeaponCardCount : this._context.Settings.EquipmentCardCount;

                    for (int i = 0; i < maxCardCount; i++)
                    {
                        var card = charEquip.Cards.Count > i ? charEquip.Cards[i] : null;

                        if (this.Version.SerializeCardBySlotType)
                        {
                            writer.Write(card, this._cardsBySlotType[charEquip.Equipment.Type]);
                        }
                        else
                        {
                            writer.Write(card, this._cardsBySlot[baseSlot]);
                        }
                    }
                }

                this._equipmentElixirs.GetValueOrDefault(baseSlot).Maybe(dict => writer.Write(charEquip.Elixir, dict));
            }
        }

        {

            var charBranches = character.Talents.GroupBy(tal => tal.Branch).ToDictionary(g => g.Key, g => g.OrderBy(v => v.HIndex).ToReadOnlyCollection());

            foreach (var branchPair in this._talents[character.Class])
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
            writer.WriteByMax(character.SharedAuras.Count, this._context.Settings.MaxPartySize - 1);

            foreach (var pair in character.SharedAuras.OrderById())
            {
                writer.Write(pair.Key, this._sharedAuras);
                writer.Write(pair.Value);
            }
        }

        if (this.Version.SharedBuffs)
        {
            var buffs = character.Buffs.Where(b => b.Key.IsShared()).ToDictionary();

            writer.Write(buffs, this._sharedBuffs);
        }

        if (this.Version.CardBuffs)
        {
            var buffs = character.Buffs.Where(b => b.Key.Card != null).ToDictionary();

            writer.Write(buffs, this._cardBuffs);
        }

        if (this.Version.StampBuffs)
        {
            var buffs = character.Buffs.Where(b => b.Key.Stamp != null).ToDictionary();

            writer.Write(buffs, this._stampBuffs);
        }

        if (this.Version.LostControl)
        {
            writer.Write(character.LostControl);
        }

        if (this.Version.Collections)
        {
            writer.Write(character.EnableCollecting);

            var grouped = character.CollectedItems.GroupBy(item => item.Group).ToDictionary(pair => pair.Key, pair => pair.ToList());

            foreach (var pair in this._collectedGroups[character.Gender])
            {
                var items = grouped.GetValueOrDefault(pair.Item1) ?? new List<ICollectedItem>();

                writer.WriteOptimize(items, pair.Item2);
            }
        }
    }

    public void FullFormat(BitWriter writer, ICharacterSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));

        writer.WriteByMax(this.Version.Id, byte.MaxValue);

        this.Format(writer, character);
    }
}