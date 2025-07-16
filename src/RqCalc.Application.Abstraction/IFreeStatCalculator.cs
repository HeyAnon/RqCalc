using RqCalc.Model;

namespace RqCalc.Application;

public interface IFreeStatCalculator
{
    int GetFreeStats(ICharacterSource characterInput);
}