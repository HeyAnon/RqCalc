using RqCalc.Domain;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.Model;

public interface ICharacterSource : ICharacterSourceBase, ITalentBuildSource, IGuildTalentBuildSource
{
    IState State { get; }

    IEvent? Event { get; }


    IReadOnlyDictionary<IStat, int> EditStats { get; }


    IReadOnlyDictionary<CharacterEquipmentIdentity, ICharacterEquipmentData> Equipments { get; }


    IElixir? Elixir { get; }


    IReadOnlyList<IConsumable> Consumables { get; }


    IAura? Aura { get; }

    IReadOnlyDictionary<IAura, bool> SharedAuras { get; }

    IReadOnlyDictionary<IBuff, int> Buffs { get; }


    IReadOnlyList<ICollectedItem> CollectedItems { get; }


    bool EnableElixir { get; }

    bool EnableConsumables { get; }

    bool EnableGuildTalents { get; }

    bool EnableAura { get; }

    bool EnableBuffs { get; }

    bool EnableTalents { get; }


    bool LostControl { get; }


    bool EnableCollecting { get; }
}