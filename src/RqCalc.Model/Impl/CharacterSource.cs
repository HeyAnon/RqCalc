using System.ComponentModel.DataAnnotations;

using Framework.Core;

using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.CollectedStatistic;
using RqCalc.Domain.GuildTalent;

namespace RqCalc.Model.Impl;

public class CharacterSource : TalentBuildSource, ICharacterSource, IEquatable<CharacterSource>
{
    [Required]
    public IGender Gender { get; set; } = null!;

    [Required]
    public IState State { get; set; } = null!;

    public IEvent? Event { get; set; }





    public Dictionary<IStat, int> EditStats { get; set; } = new();

    public Dictionary<IAura, bool> SharedAuras { get; set; } = new();


    public Dictionary<CharacterEquipmentIdentity, CharacterEquipmentData> Equipments { get; set; } = new();


    public IElixir? Elixir { get; set; }

    public List<IConsumable> Consumables { get; set; } = [];

    public Dictionary<IGuildTalent, int> GuildTalents { get; set; } = new();


    public IAura? Aura { get; set; }


    public Dictionary<IBuff, int> Buffs { get; set; } = new();

    public List<ICollectedItem> CollectedItems { get; set; } = [];


    public bool EnableElixir { get; set; }
        
    public bool EnableConsumables { get; set; }
        
    public bool EnableGuildTalents { get; set; }

    public bool EnableAura { get; set; }

    public bool EnableBuffs { get; set; }

    public bool EnableTalents { get; set; }

    public bool LostControl { get; set; }

    public bool EnableCollecting { get; set; }


    IReadOnlyDictionary<IStat, int> ICharacterSource.EditStats => this.EditStats;

    IReadOnlyDictionary<IGuildTalent, int> IGuildTalentBuildSource.GuildTalents => this.GuildTalents;

    IReadOnlyDictionary<IBuff, int> ICharacterSource.Buffs => this.Buffs;

    IReadOnlyDictionary<IAura, bool> ICharacterSource.SharedAuras => this.SharedAuras;

    IReadOnlyList<IConsumable> ICharacterSource.Consumables => this.Consumables;

    IReadOnlyList<ICollectedItem> ICharacterSource.CollectedItems => this.CollectedItems;


    IReadOnlyDictionary<CharacterEquipmentIdentity, ICharacterEquipmentData> ICharacterSource.Equipments => this.Equipments.ChangeValue(v => (ICharacterEquipmentData)v);

    public bool Equals(CharacterSource other)
    {
        if (other == null || !base.Equals(other))
        {
            return false;
        }

        var equipments = this.Equipments.OrderBy(pair => pair.Key.Slot.Id).ThenBy(pair => pair.Key.Index).ToList();

        var otherEquipments = other.Equipments.OrderBy(pair => pair.Key.Slot.Id).ThenBy(pair => pair.Key.Index).ToList();


        return this.Gender == other.Gender
               && this.Event == other.Event
               && this.State == other.State
               && this.Elixir == other.Elixir
               && this.Aura == other.Aura

               && this.EditStats.OrderById().SequenceEqual(other.EditStats.OrderById())
               && this.GuildTalents.OrderById().SequenceEqual(other.GuildTalents.OrderById())
               && this.Buffs.OrderById().SequenceEqual(other.Buffs.OrderById())

               && this.SharedAuras.OrderById().SequenceEqual(other.SharedAuras.OrderById())

               && equipments.SequenceEqual(otherEquipments)
                
               && this.EnableConsumables == other.EnableConsumables
               && this.EnableAura == other.EnableAura
               && this.EnableBuffs == other.EnableBuffs
               && this.EnableElixir == other.EnableElixir
               && this.EnableGuildTalents == other.EnableGuildTalents
               && this.EnableTalents == other.EnableTalents;

    }

    public override bool Equals(object obj) => this.Equals(obj as CharacterSource);

    public override int GetHashCode() => this.Equipments.Count;
}