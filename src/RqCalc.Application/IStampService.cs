using RqCalc.Domain;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;

namespace RqCalc.Application;

public interface IStampService
{
    bool? IsAllowedStamp(IStamp stamp, IEquipment equipment, IClass currentClass);
}