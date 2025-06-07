using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application._Extensions;

public static class ApplicationContextExtensions
{
    public static int GetMaxUpgradeLevel(this IApplicationContext context, IEquipment equipment)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));

        var @class = context.GetEquipmentClass(equipment);

        return @class.MaxUpgradeLevel ?? context.Settings.MaxUpgradeLevel;
    }

    public static IEnumerable<ITalent> GetLimitedTalents(this IApplicationContext context, ITalentBuildSource source)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var freeTalents = context.GetFreeTalents(source);

        if (freeTalents < 0)
        {
            var removingTalentRequest = from talent in source.Talents

                orderby talent.Branch.Id, talent.HIndex

                select talent;

            var removingTalents = removingTalentRequest.Reverse().Take(-freeTalents).ToList();

            return source.Talents.Except(removingTalents).ToList();
        }
        else
        {
            return source.Talents;
        }
    }

    public static bool? IsAllowedStamp(this IApplicationContext context, IStamp stamp, IEquipment equipment, IClass currentClass)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (stamp == null) throw new ArgumentNullException(nameof(stamp));
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));
        if (currentClass == null) throw new ArgumentNullException(nameof(currentClass));
            
        var equipmentClass = context.GetEquipmentClass(equipment);

        if (stamp.Equipments.Any() && !stamp.Equipments.WhereVersion(context.LastVersion).Select(e => e.Type).Contains(equipment.Type))
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

                if (!stampClasses.Any() || stampClasses.Any(c => filterStats.All(c.IsAllowed)))
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
}