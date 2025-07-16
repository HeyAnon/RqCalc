using Framework.HierarchicalExpand;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;

namespace RqCalc.Domain._Extensions;

public static class StampExtensions
{
    public static IStampVariant GetByColor(this IStamp stamp, IStampColor color)
    {
        if (stamp == null) throw new ArgumentNullException(nameof(stamp));
        if (color == null) throw new ArgumentNullException(nameof(color));

        return stamp.Variants.Single(sv => sv.Color == color);
    }

    public static IEnumerable<IClass> GetStampClasses(this IEquipment equipment, IClass @class, bool sharedStamp)
    {
        if (sharedStamp)
        {
            return [];
        }
        else if (equipment.IsPersonal)
        {
            return @class.GetAllParents();
        }
        else
        {
            return equipment.GetClassConditions();
        }
    }

    public static bool? IsAllowedWithSharing(this IStamp stamp, IEquipment equipment, IEquipmentClass equipmentClass, IClass currentClass, IVersion version)
    {
        if (stamp == null) throw new ArgumentNullException(nameof(stamp));
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));
        if (equipmentClass == null) throw new ArgumentNullException(nameof(equipmentClass));
        if (currentClass == null) throw new ArgumentNullException(nameof(currentClass));
        if (version == null) throw new ArgumentNullException(nameof(version));

        if (stamp.Equipments.Any() && !stamp.Equipments.WhereVersion(version).Select(e => e.Type).Contains(equipment.Type))
        {
            return false;
        }

        var filterStats = stamp.GetFilterStats().ToList();

        if (filterStats.Any())
        {
            var currentClassAllowed = filterStats.All(currentClass.IsAllowed);

            if (!currentClassAllowed)
            {
                var stampClasses = equipment.GetStampClasses(currentClass, equipment.Type.Slot.SharedStamp && equipmentClass.SharedStamp).ToArray();

                if (stampClasses.Any(c => filterStats.All(c.IsAllowed)))
                {
                    return null;
                }
                else
                {
                    return false;
                }
            }
        }
            
        return true;
    }

    public static bool IsAllowed(this IClass @class, IStat stat)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));
        if (stat == null) throw new ArgumentNullException(nameof(stat));

        if (stat.IsMelee != null)
        {
            return @class.IsMelee == stat.IsMelee.Value;
        }

        if (stat.Type == StatType.Energy)
        {
            return @class.EnergyStat == stat;
        }

        if (stat.Type == StatType.Primary)
        {
            return @class.PrimaryStat == stat;
        }

        return true;
    }

    public static IEnumerable<IStat> GetFilterStats(this IStamp stamp)
    {
        if (stamp == null) throw new ArgumentNullException(nameof(stamp));

        var request = from variant in stamp.Variants

            from bonus in variant.Bonuses
                          
            from stat in bonus.Type.GetFilterStats()
                          
            select stat;

        return request.Distinct();
    }
}