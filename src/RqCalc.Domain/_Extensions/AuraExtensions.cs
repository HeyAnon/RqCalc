using RqCalc.Domain.Persistent;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Talent;

namespace RqCalc.Domain._Extensions;

public static class AuraExtensions
{
    public static IEnumerable<IBonusBase> GetBonuses(this IAura aura, IVersion version, bool shared, bool withTalents)
    {
        return aura.GetBonuses(version, shared, withTalents ? aura.DependencyTalentBonuses : new List<ITalentBonus>());
    }

    public static IEnumerable<IBonusBase> GetBonuses(this IAura aura, IVersion version, bool shared, IEnumerable<ITalentBonus> talentBonuses)
    {
        if (aura == null) throw new ArgumentNullException(nameof(aura));
        if (talentBonuses == null) throw new ArgumentNullException(nameof(talentBonuses));

        var auraBonusesRequest = from bonus in aura.GetOrderedBonuses()

            where bonus.Contains(version)

            let expandedBonus = shared ? bonus.ExpandShared() : bonus

            where expandedBonus.Type.Stats.Any(s => expandedBonus.Variables[s.VarIndex] != 0)

            select new { Bonus = expandedBonus, Priority = 0 };


        var talentAuraBonusesRequest = from bonus in talentBonuses

            where bonus.AuraCondition == aura
                                           
            let expandedBonus = shared ? bonus.ExpandShared() : bonus

            where expandedBonus.Type.Stats.Any(s => expandedBonus.Variables[s.VarIndex] != 0)

            select new { Bonus = expandedBonus, Priority = 1 };


        return from pair in auraBonusesRequest.Concat(talentAuraBonusesRequest)

            group pair by pair.Bonus.Type into bonusTypeGroup

            let activeBonusPair = bonusTypeGroup.OrderByDescending(pair => pair.Priority).First()

            select activeBonusPair.Bonus;
    }
}