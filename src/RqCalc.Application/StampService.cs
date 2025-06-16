using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;

namespace RqCalc.Application;

public class StampService(IEquipmentService equipmentService, IVersion lastVersion) : IStampService
{
    public bool? IsAllowedStamp(IStamp stamp, IEquipment equipment, IClass currentClass)
    {
        var equipmentClass = equipmentService.GetEquipmentClass(equipment);

        if (stamp.Equipments.Any() && !stamp.Equipments.WhereVersion(lastVersion).Select(e => e.Type).Contains(equipment.Type))
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