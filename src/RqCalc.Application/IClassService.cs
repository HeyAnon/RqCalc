using RqCalc.Domain;

namespace RqCalc.Application;

public interface IClassService
{
    int GetHpBonuses(IClass @class, int level);
}