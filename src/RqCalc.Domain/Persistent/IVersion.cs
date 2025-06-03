using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent;

public interface IVersion : IPersistentIdentityDomainObjectBase
{
    int MaxLevel { get; }

    bool SharedAuras { get; }

    bool SharedBuffs { get; }

    bool CardBuffs { get; }

    bool StampBuffs { get; }

    bool LostControl { get; }

    bool Collections { get; }

    int MaxTalentLevel { get; }

    bool SerializeCardBySlotType { get; }

    bool GuildTalents { get; }
}