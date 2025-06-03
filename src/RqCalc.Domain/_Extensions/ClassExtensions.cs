using Framework.Core;
using Framework.HierarchicalExpand;
using RqCalc.Domain.Persistent;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Equipment;
using RqCalc.Domain.Persistent.Talent;

namespace RqCalc.Domain._Extensions;

public static class ClassExtensions
{
    public static bool IsSubsetOf(this IClass @class, IClass set)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));
        if (set == null) throw new ArgumentNullException(nameof(set));

        return set.GetAllChildren().Contains(@class);
    }

    public static bool IsSubsetOf(this IClass @class, IClassObject set)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));
        if (set == null) throw new ArgumentNullException(nameof(set));

        return @class.IsSubsetOf(set.Class);
    }

    public static bool IsSubsetOf(this IClass @class, IEnumerable<IClass> sets)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));
            
        return sets.Any(@class.IsSubsetOf);
    }

    public static bool IsSubsetOf(this IClass @class, IEnumerable<IClassObject> sets)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return @class.IsSubsetOf(sets.Select(obj => obj.Class));
    }

    public static int GetLevelOffset(this IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return @class.GetAllParents(true).Count();
    }

    public static int GetMinLevel(this IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return @class.Parent.Maybe(p => p.Specialization.MaxLevel.Value, 1);
    }

    public static int GetSumBonusTalentCount(this IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return @class.GetAllParents().Sum(c => c.Specialization.BonusTalentCount);
    }


    public static IEnumerable<IStat> GetStats(this IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        yield return @class.PrimaryStat;
        yield return @class.EnergyStat;

        foreach (var restoreStat in @class.EnergyStat.RestoreStats.Values)
        {
            yield return restoreStat;
        }
    }

    public static IEnumerable<IAura> GetAuras(this IClass @class, int level, IVersion version)
    {
        return @class.GetAllParents().SelectMany(c => c.Auras)
            .Reverse()
            .Where(aura => aura.Level <= level)
            .WhereVersion(version);
    }

    public static bool IsAllowed(this IClass @class, IEquipmentSlot slot)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));
        if (slot == null) throw new ArgumentNullException(nameof(slot));

        return slot.Types.Any(type => !type.Conditions.Any() || @class.IsSubsetOf(type.Conditions));
    }

    public static IEnumerable<ITalentBranch> GetAllTalentBranches(this IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return @class.GetAllParents().Reverse().SelectMany(c => c.TalentBranches);
    }
}